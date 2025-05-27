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
        private readonly IRepository<MealIngredient, Guid> _mealIngredientRepo;
        private readonly IRepository<Ingredient, Guid> _ingredientRepo;
        private readonly ILogger<MealPlanAppService> _logger;

        public MealPlanAppService(
            IRepository<MealPlan, Guid> planRepo,
            IRepository<Meal, Guid> mealRepo,
            IRepository<MealPlanDay, Guid> mealPlanDayRepo,
            IRepository<MealPlanDayMeal, Guid> mealPlanDayMealRepo,
            IRepository<MealIngredient, Guid> mealIngredientRepo,
            IRepository<Ingredient, Guid> ingredientRepo,
            ILogger<MealPlanAppService> logger)
        {
            _planRepo = planRepo;
            _mealRepo = mealRepo;
            _mealPlanDayRepo = mealPlanDayRepo;
            _mealPlanDayMealRepo = mealPlanDayMealRepo;
            _mealIngredientRepo = mealIngredientRepo;
            _ingredientRepo = ingredientRepo;
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

            // Step 1: Create and save the MealPlan first
            var plan = new MealPlan
            {
                Name = input.Name,
                Status = MealPlanStatus.Active
            };

            // Insert the plan first to get its database ID
            var insertedPlan = await _planRepo.InsertAsync(plan);
            await CurrentUnitOfWork.SaveChangesAsync(); // Ensure the plan gets its ID

            // Step 2: Create MealPlanDays with the correct MealPlanId
            if (input.MealPlanDays != null)
            {
                foreach (var dayInput in input.MealPlanDays.OrderBy(d => d.Order))
                {
                    var mealPlanDay = new MealPlanDay
                    {
                        Order = dayInput.Order,
                        Description = dayInput.Description,
                        IsComplete = false,
                        MealPlanId = insertedPlan.Id // Set the foreign key explicitly
                    };

                    // Insert the day to get its database ID
                    var insertedDay = await _mealPlanDayRepo.InsertAsync(mealPlanDay);
                    await CurrentUnitOfWork.SaveChangesAsync(); // Ensure the day gets its ID

                    // Step 3: Create MealPlanDayMeals with the correct MealPlanDayId
                    foreach (var mealId in dayInput.Meals)
                    {
                        var meal = meals.FirstOrDefault(m => m.Id == mealId);
                        if (meal == null)
                            throw new UserFriendlyException($"Meal with ID {mealId} not found.");

                        var mealPlanDayMeal = new MealPlanDayMeal
                        {
                            MealPlanDayId = insertedDay.Id, // Set the foreign key explicitly
                            MealId = meal.Id
                        };

                        await _mealPlanDayMealRepo.InsertAsync(mealPlanDayMeal);
                    }
                }
            }

            // Final save for any remaining changes
            await CurrentUnitOfWork.SaveChangesAsync();

            // Return the complete meal plan
            return await GetAsync(insertedPlan.Id);
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
