using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;  // Add this using statement
using Shesha.Domain.Attributes;

namespace NinjaKiwi.LifeQuest.Domain.Domain
{
    [Table("LifQu_MealPlanDayMeals")]
    [Entity(TypeShortAlias = "LifQu.MealPlanDayMeal")]
    public class MealPlanDayMeal : Entity<Guid>  // Add this inheritance
    {
        /// <summary>
        /// Foreign key to MealPlanDay
        /// </summary>
        public Guid MealPlanDayId { get; set; }

        /// <summary>
        /// Foreign key to Meal
        /// </summary>
        public Guid MealId { get; set; }

        /// <summary>
        /// Navigation property to MealPlanDay
        /// </summary>
        public virtual MealPlanDay MealPlanDay { get; set; }

        /// <summary>
        /// Navigation property to Meal
        /// </summary>
        public virtual Meal Meal { get; set; }

        // Remove the manual Id property since Entity<Guid> provides it
    }
}