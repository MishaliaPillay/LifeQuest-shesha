using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                Logger.Debug($"AI Response: {jsonResponse}");

                var activityDto = ParseActivityDto(jsonResponse);
                if (activityDto == null)
                    throw new UserFriendlyException("Failed to parse AI response.");
                if (string.IsNullOrWhiteSpace(activityDto.Category) ||
    string.IsNullOrWhiteSpace(activityDto.Description) ||
    string.IsNullOrWhiteSpace(activityDto.Duration) ||
    activityDto.Calories <= 0)
                {
                    Logger.Warn("Incomplete AI response parsed");
                    throw new UserFriendlyException("Incomplete or invalid AI response.");
                }


                var entity = new ActivityType
                {
                    Id = Guid.NewGuid(),
                    Category = activityDto.Category,
                    Description = activityDto.Description,
                    Duration = activityDto.Duration,
                    Calories = activityDto.Calories
                };

                await _activityTypeRepo.InsertAsync(entity);
                Logger.Debug($"Inserting ActivityType: {JsonSerializer.Serialize(entity)}");

                return await MapToDynamicDtoAsync<ActivityType, Guid>(entity);
            }
            catch (JsonException jex)
            {
                Logger.Error("JSON parsing failed", jex);
                throw new UserFriendlyException("Invalid response format from AI.");
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


            sb.AppendLine("\nReturn only raw JSON in the following format:\n");
            sb.AppendLine("{");
            sb.AppendLine("  \"category\": \"string\",");
            sb.AppendLine("  \"description\": \"string\",");
            sb.AppendLine("  \"duration\": \"string\",");
            sb.AppendLine("  \"calories\": number");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private CreateActivityTypeDto ParseActivityDto(string aiResponse)
        {
            var json = ExtractJsonBlock(aiResponse);
            return JsonSerializer.Deserialize<CreateActivityTypeDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        private string ExtractJsonBlock(string content)
        {
            var start = content.IndexOf('{');
            var end = content.LastIndexOf('}');
            Logger.Debug($"Extracted JSON: {content}");

            return (start >= 0 && end > start) ? content.Substring(start, end - start + 1) : content;
        }
    }
}
