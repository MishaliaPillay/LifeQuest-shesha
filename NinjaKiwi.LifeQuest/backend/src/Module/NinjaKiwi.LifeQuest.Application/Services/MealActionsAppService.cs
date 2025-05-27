using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Linq;
using NinjaKiwi.LifeQuest.Common.Services.Dtos;
using NinjaKiwi.LifeQuest.Domain.Domain;
using Shesha;


namespace NinjaKiwi.LifeQuest.Services.Meals
{
    public class MealActionsAppService : SheshaAppServiceBase
    {

        private readonly IRepository<Meal, Guid> _mealRepository;
        private readonly IRepository<MealIngredient, Guid> _mealIngredientRepository;
        private readonly IRepository<MealPlanMeal, Guid> _mealPlanMealRepository;

        public MealActionsAppService(
            IRepository<Meal, Guid> mealRepository,
            IRepository<MealIngredient, Guid> mealIngredientRepository,
            IRepository<MealPlanMeal, Guid> mealPlanMealRepository)
        {
            _mealRepository = mealRepository;
            _mealIngredientRepository = mealIngredientRepository;
            _mealPlanMealRepository = mealPlanMealRepository;
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