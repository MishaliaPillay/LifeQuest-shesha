using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.Extensions.Logging;
using NHibernate.Linq;
using NinjaKiwi.LifeQuest.Common.Services.Dtos;
using NinjaKiwi.LifeQuest.Domain.Domain;
using NinjaKiwi.LifeQuest.Domain;

namespace NinjaKiwi.LifeQuest.Common.Services
{
    public class MealPlanAppService : ApplicationService
    {
        private readonly IRepository<MealPlan, Guid> _planRepo;
        private readonly IRepository<Meal, Guid> _mealRepo;
        private readonly IRepository<MealPlanDay, Guid> _mealPlanDayRepo;
        private readonly IRepository<MealPlanDayMeal, Guid> _mealPlanDayMealRepo;
        private readonly IRepository<MealPlanMeal, Guid> _mealPlanMealRepo;
        private readonly IRepository<MealIngredient, Guid> _mealIngredientRepo;
        private readonly IRepository<Ingredient, Guid> _ingredientRepo;
        private readonly IRepository<HealthPath, Guid> _healthPathRepo;
        private readonly ILogger<MealPlanAppService> _logger;

        public MealPlanAppService(
            IRepository<MealPlan, Guid> planRepo,
            IRepository<Meal, Guid> mealRepo,
            IRepository<MealPlanDay, Guid> mealPlanDayRepo,
            IRepository<MealPlanDayMeal, Guid> mealPlanDayMealRepo,
            IRepository<MealPlanMeal, Guid> mealPlanMealRepo,
            IRepository<MealIngredient, Guid> mealIngredientRepo,
            IRepository<Ingredient, Guid> ingredientRepo,
            IRepository<HealthPath, Guid> healthPathRepo,
            ILogger<MealPlanAppService> logger)
        {
            _planRepo = planRepo;
            _mealRepo = mealRepo;
            _mealPlanDayRepo = mealPlanDayRepo;
            _mealPlanDayMealRepo = mealPlanDayMealRepo;
            _mealPlanMealRepo = mealPlanMealRepo;
            _mealIngredientRepo = mealIngredientRepo;
            _ingredientRepo = ingredientRepo;
            _healthPathRepo = healthPathRepo;
            _logger = logger;
        }

        public async Task<MealPlanDto> CreateAsync(CreateMealPlanDto input)
        {
            var allMealIds = input.MealPlanDays?
                .SelectMany(d => d.Meals)
                .Distinct()
                .ToList() ?? new List<Guid>();

            var meals = await _mealRepo.GetAll()
                .Where(m => allMealIds.Contains(m.Id))
                .ToListAsync();

            if (meals.Count != allMealIds.Count)
                throw new UserFriendlyException("Some meals in meal plan days were not found.");

            var plan = new MealPlan
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                Status = MealPlanStatus.Active,
                MealPlanDays = new List<MealPlanDay>()
            };

            if (input.HealthPathId.HasValue)
            {
                var path = await _healthPathRepo.FirstOrDefaultAsync(input.HealthPathId.Value);
                if (path == null)
                    throw new UserFriendlyException("HealthPath not found.");

                plan.HealthPathId = input.HealthPathId.Value; // Set the FK directly
                plan.HealthPath = path; // Optional: set navigation property
            }

            await _planRepo.InsertAsync(plan);
            await CurrentUnitOfWork.SaveChangesAsync();

            if (input.MealPlanDays != null)
            {
                var mealDict = meals.ToDictionary(m => m.Id);

                foreach (var day in input.MealPlanDays.OrderBy(d => d.Order))
                {
                    var mealPlanDay = new MealPlanDay
                    {
                        Id = Guid.NewGuid(),
                        Order = day.Order,
                        Description = day.Description,
                        MealPlanId = plan.Id,
                        MealPlanDayMeals = new List<MealPlanDayMeal>()
                    };

                    await _mealPlanDayRepo.InsertAsync(mealPlanDay);

                    foreach (var mealId in day.Meals)
                    {
                        if (!mealDict.TryGetValue(mealId, out var meal))
                            throw new UserFriendlyException($"Meal with ID {mealId} not found.");

                        var mealPlanDayMeal = new MealPlanDayMeal
                        {
                            Id = Guid.NewGuid(),
                            MealPlanDayId = mealPlanDay.Id,
                            MealId = meal.Id
                        };

                        await _mealPlanDayMealRepo.InsertAsync(mealPlanDayMeal);
                        mealPlanDay.MealPlanDayMeals.Add(mealPlanDayMeal);
                    }

                    plan.MealPlanDays.Add(mealPlanDay);
                }

                await CurrentUnitOfWork.SaveChangesAsync();
            }

            // Reload the saved plan with all navigation properties to return DTO
            var savedPlan = await _planRepo.FirstOrDefaultAsync(p => p.Id == plan.Id);
            if (savedPlan == null)
                throw new UserFriendlyException("Failed to retrieve the created meal plan.");

            await _planRepo.EnsureCollectionLoadedAsync(savedPlan, p => p.MealPlanDays);
            foreach (var day in savedPlan.MealPlanDays ?? new List<MealPlanDay>())
            {
                await _mealPlanDayRepo.EnsureCollectionLoadedAsync(day, d => d.MealPlanDayMeals);
                foreach (var dayMeal in day.MealPlanDayMeals ?? new List<MealPlanDayMeal>())
                {
                    await _mealPlanDayMealRepo.EnsurePropertyLoadedAsync(dayMeal, dm => dm.Meal);
                    if (dayMeal.Meal != null)
                    {
                        await _mealRepo.EnsureCollectionLoadedAsync(dayMeal.Meal, m => m.MealIngredients);
                        foreach (var mi in dayMeal.Meal.MealIngredients ?? new List<MealIngredient>())
                        {
                            await _mealIngredientRepo.EnsurePropertyLoadedAsync(mi, i => i.Ingredient);
                        }
                    }
                }
            }

            return ObjectMapper.Map<MealPlanDto>(savedPlan);
        }

        public async Task<MealPlanDto> GetAsync(Guid id)
        {
            var plan = await _planRepo.FirstOrDefaultAsync(p => p.Id == id);
            if (plan == null)
                throw new UserFriendlyException("MealPlan not found.");

            await _planRepo.EnsureCollectionLoadedAsync(plan, p => p.MealPlanDays);

            foreach (var day in plan.MealPlanDays)
            {
                await _mealPlanDayRepo.EnsureCollectionLoadedAsync(day, d => d.MealPlanDayMeals);

                foreach (var dayMeal in day.MealPlanDayMeals)
                {
                    await _mealPlanDayMealRepo.EnsurePropertyLoadedAsync(dayMeal, dm => dm.Meal);
                    await _mealRepo.EnsureCollectionLoadedAsync(dayMeal.Meal, m => m.MealIngredients);

                    foreach (var mi in dayMeal.Meal.MealIngredients)
                    {
                        await _mealIngredientRepo.EnsurePropertyLoadedAsync(mi, i => i.Ingredient);
                    }
                }
            }

            return ObjectMapper.Map<MealPlanDto>(plan);
        }

        public async Task<List<MealPlanDayWithMealsDto>> GetMealPlanDaysWithMealsByPlanIdAsync(Guid mealPlanId)
        {
            var plan = await _planRepo.FirstOrDefaultAsync(p => p.Id == mealPlanId);
            if (plan == null)
                throw new UserFriendlyException("MealPlan not found.");

            await _planRepo.EnsureCollectionLoadedAsync(plan, p => p.MealPlanDays);

            foreach (var day in plan.MealPlanDays)
            {
                await _mealPlanDayRepo.EnsureCollectionLoadedAsync(day, d => d.MealPlanDayMeals);
                foreach (var dm in day.MealPlanDayMeals)
                {
                    await _mealPlanDayMealRepo.EnsurePropertyLoadedAsync(dm, d => d.Meal);
                    await _mealRepo.EnsureCollectionLoadedAsync(dm.Meal, m => m.MealIngredients);
                    foreach (var mi in dm.Meal.MealIngredients)
                    {
                        await _mealIngredientRepo.EnsurePropertyLoadedAsync(mi, i => i.Ingredient);
                    }
                }
            }

            var result = plan.MealPlanDays
                .OrderBy(d => d.Order)
                .Select(day => new MealPlanDayWithMealsDto
                {
                    Order = day.Order,
                    Description = day.Description,
                    Score = day.MealPlanDayMeals.Sum(dm => dm.Meal?.Score ?? 0),
                    Meals = day.MealPlanDayMeals.Select(dm => new MealDto
                    {
                        Id = dm.Meal.Id,
                        Name = dm.Meal.Name,
                        Description = dm.Meal.Description,
                        Calories = dm.Meal.Calories,
                        IsComplete = dm.Meal.IsComplete,
                        Score = dm.Meal.Score,
                        IngredientIds = dm.Meal.MealIngredients.Select(mi => mi.Ingredient.Id).ToList(),
                        Ingredients = dm.Meal.MealIngredients.Select(mi => new IngredientDto
                        {
                            Id = mi.Ingredient.Id,
                            Name = mi.Ingredient.Name,
                            ServingSize = mi.Ingredient.ServingSize,
                            Calories = mi.Ingredient.Calories,
                            Protein = mi.Ingredient.Protein,
                            Carbohydrates = mi.Ingredient.Carbohydrates,
                            Fats = mi.Ingredient.Fat
                        }).ToList()
                    }).ToList()
                }).ToList();

            return result;
        }

        public async Task CompletePlanAsync(Guid planId)
        {
            var plan = await _planRepo.FirstOrDefaultAsync(p => p.Id == planId);
            if (plan == null)
                throw new UserFriendlyException("MealPlan not found.");

            if (plan.Status != MealPlanStatus.Active)
                throw new UserFriendlyException("Only active plans can be completed.");

            plan.Status = MealPlanStatus.Completed;
            await _planRepo.UpdateAsync(plan);
        }
    }
}