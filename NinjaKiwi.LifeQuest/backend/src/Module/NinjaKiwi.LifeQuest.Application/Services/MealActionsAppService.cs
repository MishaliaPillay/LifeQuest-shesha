using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Linq;
using NinjaKiwi.LifeQuest.Common.Services.Dtos;
using NinjaKiwi.LifeQuest.Domain.Domain;
using Shesha;
using Abp.Domain.Uow;


namespace NinjaKiwi.LifeQuest.Services.Meals
{
    public class MealActionsAppService : SheshaAppServiceBase
    {

        private readonly IRepository<Meal, Guid> _mealRepository;
        private readonly IRepository<MealIngredient, Guid> _mealIngredientRepository;
        private readonly IRepository<MealPlanMeal, Guid> _mealPlanMealRepository;
        private readonly IRepository<Ingredient, Guid> _ingredientRepository;

        public MealActionsAppService(
            IRepository<Meal, Guid> mealRepository,
            IRepository<MealIngredient, Guid> mealIngredientRepository,
            IRepository<Ingredient, Guid> ingredientRepository
        )
        {
            _mealRepository = mealRepository;
            _mealIngredientRepository = mealIngredientRepository;
            _ingredientRepository = ingredientRepository;
        }



        [HttpPost, Route("api/app/meal-actions/create")]
        public async Task<MealDto> CreateMealAsync([FromBody] CreateMealDto input)
        {
            try
            {
                // Create Meal without ingredients
                var meal = new Meal
                {
                    Name = input.Name,
                    Description = input.Description,
                    Calories = input.Calories,
                    Score = input.Score,
                    IsComplete = input.IsComplete,
                    MealIngredients = new List<MealIngredient>()  // Initialize empty
                };

                // Insert Meal first and save changes to generate Meal.Id
                await _mealRepository.InsertAsync(meal);
                await CurrentUnitOfWork.SaveChangesAsync();

                // If there are ingredients, insert MealIngredients linking to meal.Id
                if (input.IngredientIds != null && input.IngredientIds.Any())
                {
                    var mealIngredients = input.IngredientIds.Select(ingredientId => new MealIngredient
                    {
                        MealId = meal.Id,
                        IngredientId = ingredientId
                    }).ToList();

                    foreach (var mealIngredient in mealIngredients)
                    {
                        await _mealIngredientRepository.InsertAsync(mealIngredient);
                    }

                    await CurrentUnitOfWork.SaveChangesAsync();

                    // Refresh MealIngredients collection from DB or attach manually
                    meal.MealIngredients = mealIngredients;
                }

                // Return the DTO with ingredient IDs
                return new MealDto
                {
                    Id = meal.Id,
                    Name = meal.Name,
                    Description = meal.Description,
                    Calories = meal.Calories,
                    Score = meal.Score,
                    IsComplete = meal.IsComplete,
                    IngredientIds = meal.MealIngredients.Select(x => x.IngredientId).ToList()
                };
            }
            catch (Exception ex)
            {
                Logger.Error("Meal creation failed", ex); // Log error properly
                throw;
            }
        }



        private string ExtractJsonFromMarkdown(string markdown)
        {
            if (string.IsNullOrWhiteSpace(markdown)) return "{}";

            // Check for code block syntax
            var start = markdown.IndexOf("```");
            if (start >= 0)
            {
                var end = markdown.LastIndexOf("```");
                if (end > start)
                {
                    // Remove the language specifier if present
                    var content = markdown.Substring(start + 3, end - start - 3).Trim();
                    var firstLineEnd = content.IndexOf('\n');
                    if (firstLineEnd > 0 && firstLineEnd < 20) // Short first line could be a language specifier
                    {
                        var firstLine = content.Substring(0, firstLineEnd).Trim();
                        if (firstLine == "json")
                        {
                            content = content.Substring(firstLineEnd).Trim();
                        }
                    }
                    return content;
                }
            }

            return markdown.Trim(); // fallback
        }

        [HttpGet, Route("api/app/meal-actions/get-by-id/{id}")]
        public async Task<MealDto> GetMealByIdAsync(Guid id)
        {
            var meal = await _mealRepository.GetAsync(id);

            return new MealDto
            {
                Id = meal.Id,
                Name = meal.Name,
                Description = meal.Description,
                Calories = meal.Calories,
                Score = meal.Score,
                IsComplete = meal.IsComplete,
                IngredientIds = meal.MealIngredients?.Select(x => x.IngredientId).ToList() ?? new List<Guid>()
            };
        }


        [HttpGet, Route("api/app/meal-actions/get-all")]
        public async Task<List<MealDto>> GetAllMealsAsync()
        {
            var meals = await _mealRepository.GetAllListAsync();

            return meals.Select(meal => new MealDto
            {
                Id = meal.Id,
                Name = meal.Name,
                Description = meal.Description,
                Calories = meal.Calories,
                Score = meal.Score,
                IsComplete = meal.IsComplete,
                IngredientIds = meal.MealIngredients?.Select(x => x.IngredientId).ToList() ?? new List<Guid>()
            }).ToList();
        }

        public async Task<MealDto> GenerateAIMealAsync(GenerateAIMealInputDto input)
        {
            try
            {
                var httpClient = new HttpClient();
                var apiKey = "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";

                if (string.IsNullOrWhiteSpace(apiKey))
                    throw new Exception("OpenRouter API key is not configured. Please set OPENROUTER_API_KEY.");

                var prompt = $@"
Generate a healthy {input.MealType} meal in JSON format. Include at least 2 ingredients, no more than 3.
{(input.MaxCalories > 0 ? $"Max {input.MaxCalories} calories." : "")}
{(!string.IsNullOrWhiteSpace(input.DietaryRequirement) ? $"Meet dietary requirement: {input.DietaryRequirement}." : "")}
{(!string.IsNullOrWhiteSpace(input.PreferredCuisine) ? $"Use {input.PreferredCuisine} style." : "")}
Respond only with JSON inside a code block.
Format:
{{
  ""name"": ""Grilled Chicken Salad"",
  ""description"": ""A healthy mix of grilled chicken and fresh vegetables."",
  ""calories"": 450,
  ""ingredients"": [
    {{
      ""name"": ""Grilled Chicken Breast"",
      ""servingSize"": 150,
      ""calories"": 250,
      ""protein"": 30,
      ""carbohydrates"": 0,
      ""fats"": 10
    }},
    {{
      ""name"": ""Lettuce"",
      ""servingSize"": 50,
      ""calories"": 15,
      ""protein"": 1,
      ""carbohydrates"": 2,
      ""fats"": 0
    }}
  ]
}}";

                var requestBody = new
                {
                    model = "deepseek/deepseek-prover-v2:free",
                    messages = new[] { new { role = "user", content = prompt } }
                };

                var request = new HttpRequestMessage(HttpMethod.Post, "https://openrouter.ai/api/v1/chat/completions")
                {
                    Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var messageContent = JsonDocument.Parse(jsonResponse)
                    .RootElement.GetProperty("choices")[0]
                    .GetProperty("message").GetProperty("content").GetString();

                var cleanedJson = ExtractJsonFromMarkdown(messageContent);

                Logger.Debug("Raw AI message content: " + messageContent);
                Logger.Debug("Extracted JSON: " + cleanedJson);

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                var mealWithIngredients = JsonSerializer.Deserialize<CreateMealWithIngredientsDto>(cleanedJson, jsonOptions)
                    ?? throw new Exception("Failed to deserialize meal data - result was null");

                if (mealWithIngredients.Ingredients == null || mealWithIngredients.Ingredients.Count == 0)
                    throw new Exception("No ingredients found in the AI response");

                // Manually create and insert ingredients
                var ingredientIds = new List<Guid>();
                foreach (var ingredientDto in mealWithIngredients.Ingredients)
                {
                    var ingredient = new Ingredient
                    {
                        Name = ingredientDto.Name,
                        ServingSize = ingredientDto.ServingSize,
                        Calories = ingredientDto.Calories,
                        Protein = ingredientDto.Protein,
                        Carbohydrates = ingredientDto.Carbohydrates,
                        Fat = ingredientDto.Fats
                    };

                    using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
                    {
                        await _ingredientRepository.InsertAsync(ingredient);
                        ingredientIds.Add(ingredient.Id);
                    }


                }

                // Insert meal
                var meal = new Meal
                {
                    Name = mealWithIngredients.Name,
                    Description = mealWithIngredients.Description,
                    Calories = mealWithIngredients.Calories,
                    Score = 0,
                    IsComplete = false,
                    MealIngredients = new List<MealIngredient>() // Empty init
                };

                await _mealRepository.InsertAsync(meal);
                await CurrentUnitOfWork.SaveChangesAsync(); // Needed to get meal.Id

                // Insert meal-ingredient links
                foreach (var ingredientId in ingredientIds)
                {
                    var mealIngredient = new MealIngredient
                    {
                        MealId = meal.Id,
                        IngredientId = ingredientId
                    };
                    await _mealIngredientRepository.InsertAsync(mealIngredient);
                }

                await CurrentUnitOfWork.SaveChangesAsync();

                return new MealDto
                {
                    Id = meal.Id,
                    Name = meal.Name,
                    Description = meal.Description,
                    Calories = meal.Calories,
                    Score = meal.Score,
                    IsComplete = meal.IsComplete,
                    IngredientIds = ingredientIds
                };
            }
            catch (JsonException jsonEx)
            {
                Logger.Error($"JSON parsing error: {jsonEx.Message}", jsonEx);
                throw new UserFriendlyException("Failed to parse AI response.");
            }
            catch (HttpRequestException httpEx)
            {
                Logger.Error($"HTTP error: {httpEx.Message}", httpEx);
                throw new UserFriendlyException("Could not connect to meal AI service.");
            }
            catch (Exception ex)
            {
                Logger.Error($"AI meal generation failed: {ex.Message}", ex);
                throw new UserFriendlyException("Could not generate AI meal: " + ex.Message);
            }
        }

        [HttpGet, Route("api/app/meal-actions/get-by-meal-plan-id/{mealPlanId}")]
        public async Task<List<MealDto>> GetByMealPlanIdAsync(Guid mealPlanId)
        {
            try
            {
                // Get all MealPlanMeal entries for the given meal plan using GetAllListAsync
                var mealPlanMeals = await _mealPlanMealRepository.GetAllListAsync();
                var filteredMealPlanMeals = mealPlanMeals.Where(mpm => mpm.MealPlanId == mealPlanId).ToList();

                if (!filteredMealPlanMeals.Any())
                {
                    return new List<MealDto>();
                }

                // Extract meal IDs
                var mealIds = filteredMealPlanMeals.Select(mpm => mpm.MealId).ToList();

                // Get all meals that belong to this meal plan using GetAllListAsync
                var allMeals = await _mealRepository.GetAllListAsync();
                var meals = allMeals.Where(m => mealIds.Contains(m.Id)).ToList();

                return meals.Select(meal => new MealDto
                {
                    Id = meal.Id,
                    Name = meal.Name,
                    Description = meal.Description,
                    Calories = meal.Calories,
                    Score = meal.Score,
                    IsComplete = meal.IsComplete,
                    IngredientIds = meal.MealIngredients?.Select(x => x.IngredientId).ToList() ?? new List<Guid>()
                }).ToList();
            }
            catch (Exception ex)
            {
                Logger.Error("GetByMealPlanIdAsync failed", ex);
                throw;
            }
        }
    }
}