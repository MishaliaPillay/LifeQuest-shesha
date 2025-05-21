using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using NinjaKiwi.LifeQuest.Domain.Domain;
using Shesha.DynamicEntities.Dtos;
using Shesha;
using NinjaKiwi.LifeQuest.Common.Services.Dtos;
using NinjaKiwi.LifeQuest.Application.Services.Dtos;

namespace NinjaKiwi.LifeQuest.Common.Services
{
    public class ActivityTypeAppService : SheshaAppServiceBase
    {
        private readonly IRepository<ActivityType, Guid> _activityTypeRepo;

        public ActivityTypeAppService(IRepository<ActivityType, Guid> activityTypeRepo)
        {
            _activityTypeRepo = activityTypeRepo;
        }

        /// <summary>
        /// Generates a personalized ActivityType using AI and stores it.
        /// </summary>
        [HttpPost]
        public async Task<DynamicDto<ActivityType, Guid>> GenerateActivityTypeAsync([FromBody] ExerciseGenerationRequestDto input)
        {
            try
            {
                var prompt = BuildPrompt(input);
                var apiKey = "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";
                //Environment.GetEnvironmentVariable("OPENROUTER_API_KEY");

                if (string.IsNullOrWhiteSpace(apiKey))
                    throw new UserFriendlyException("Missing OpenRouter API key");

                var client = new HttpClient();
                var requestBody = new
                {
                    model = "deepseek/deepseek-prover-v2:free",
                    messages = new[] { new { role = "user", content = prompt } }
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "https://openrouter.ai/api/v1/chat/completions")
                {
                    Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
                };
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                // Add logging to see what's being sent
                Logger.Debug($"Sending request to OpenRouter: {JsonSerializer.Serialize(requestBody)}");

                var response = await client.SendAsync(request);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                Logger.Debug($"Raw OpenRouter response: {jsonResponse}");

                if (!response.IsSuccessStatusCode)
                {
                    Logger.Error($"OpenRouter API failed with status {response.StatusCode}: {jsonResponse}");
                    throw new UserFriendlyException($"AI service returned error: {response.StatusCode}");
                }

                var activityDto = ParseActivityTypeFromResponse(jsonResponse);

                if (activityDto == null)
                {
                    Logger.Error("Failed to parse activity data from response");
                    throw new UserFriendlyException("Could not parse AI response");
                }

                // Validate the extracted data
                if (string.IsNullOrWhiteSpace(activityDto.Category) ||
                    string.IsNullOrWhiteSpace(activityDto.Description) ||
                    string.IsNullOrWhiteSpace(activityDto.Duration) ||
                    activityDto.Calories <= 0)
                {
                    Logger.Warn($"Incomplete AI response parsed: {JsonSerializer.Serialize(activityDto)}");
                    throw new UserFriendlyException("Incomplete or invalid AI response");
                }

                var entity = new ActivityType
                {
                    Id = Guid.NewGuid(),
                    Category = activityDto.Category,
                    Description = activityDto.Description,
                    Duration = activityDto.Duration,
                    Calories = activityDto.Calories
                };

                Logger.Debug($"Inserting ActivityType: {JsonSerializer.Serialize(entity)}");
                await _activityTypeRepo.InsertAsync(entity);

                return await MapToDynamicDtoAsync<ActivityType, Guid>(entity);
            }
            catch (Exception ex)
            {
                Logger.Error("ActivityType generation failed", ex);
                throw new UserFriendlyException("Could not generate activity type.");
            }
        }

        private string BuildPrompt(ExerciseGenerationRequestDto input)
        {
            var sb = new StringBuilder("Generate one personalized exercise using the following profile:\n");

            if (input.Age > 0) sb.AppendLine($"- Age: {input.Age}");
            if (!string.IsNullOrWhiteSpace(input.BodyType)) sb.AppendLine($"- Body Type: {input.BodyType}");
            if (!string.IsNullOrWhiteSpace(input.Gender)) sb.AppendLine($"- Gender: {input.Gender}");
            if (!string.IsNullOrWhiteSpace(input.FitnessLevel)) sb.AppendLine($"- Fitness Level: {input.FitnessLevel}");
            if (input.CurrentWeight > 0) sb.AppendLine($"- Weight: {input.CurrentWeight}kg");
            if (!string.IsNullOrWhiteSpace(input.Limitations)) sb.AppendLine($"- Limitations: {input.Limitations}");
            if (!string.IsNullOrWhiteSpace(input.PreferredExerciseTypes)) sb.AppendLine($"- Preferred Exercises: {input.PreferredExerciseTypes}");
            if (input.AvailableEquipment != null && input.AvailableEquipment.Length > 0)
                sb.AppendLine($"- Equipment: {string.Join(", ", input.AvailableEquipment)}");

            sb.AppendLine("\nReturn only raw JSON in the following format without any additional text or explanations:\n");
            sb.AppendLine("{");
            sb.AppendLine("  \"category\": \"string\",");
            sb.AppendLine("  \"description\": \"string\",");
            sb.AppendLine("  \"duration\": \"string\",");
            sb.AppendLine("  \"calories\": number");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private CreateActivityTypeDto ParseActivityTypeFromResponse(string jsonResponse)
        {
            try
            {
                // First try to parse the full response to extract the content
                using var responseDoc = JsonDocument.Parse(jsonResponse);

                // Extract the message content from OpenRouter API response
                if (responseDoc.RootElement.TryGetProperty("choices", out var choices) &&
                    choices.GetArrayLength() > 0)
                {
                    var firstChoice = choices[0];
                    if (firstChoice.TryGetProperty("message", out var message) &&
                        message.TryGetProperty("content", out var content))
                    {
                        var contentStr = content.GetString();
                        Logger.Debug($"Extracted message content: {contentStr}");

                        // Extract JSON from the content
                        var jsonContent = ExtractJsonBlock(contentStr);
                        if (!string.IsNullOrEmpty(jsonContent))
                        {
                            // Parse the extracted JSON
                            return JsonSerializer.Deserialize<CreateActivityTypeDto>(jsonContent, new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });
                        }
                    }
                }

                Logger.Error($"Failed to extract JSON from response structure: {jsonResponse}");
                return null;
            }
            catch (JsonException jex)
            {
                Logger.Error($"JSON parsing failed: {jex.Message}", jex);
                return null;
            }
        }

        private string ExtractJsonBlock(string content)
        {
            try
            {
                // Try to find JSON content between curly braces
                var start = content.IndexOf('{');
                var end = content.LastIndexOf('}');

                if (start >= 0 && end > start)
                {
                    var jsonBlock = content.Substring(start, end - start + 1);
                    Logger.Debug($"Extracted JSON block: {jsonBlock}");

                    // Validate that this is actually valid JSON
                    using var _ = JsonDocument.Parse(jsonBlock);
                    return jsonBlock;
                }

                Logger.Error($"Could not find valid JSON block in content: {content}");
                return null;
            }
            catch (JsonException jex)
            {
                Logger.Error($"Failed to extract JSON block: {jex.Message}", jex);
                return null;
            }
        }
    }
}