using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using NinjaKiwi.LifeQuest.Domain.Domain;
using Shesha.Domain.Attributes;

namespace NinjaKiwi.LifeQuest.Domain
{
    [Table("LifQu_MealPlanDays")]
    [Entity(TypeShortAlias = "LifQu.MealPlanDays")]
    public class MealPlanDay : Entity<Guid>  // Inherit from Entity<Guid>
    {
        /// <summary>
        /// The order of the meal plan day
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// The description of the meal plan day
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Foreign key to MealPlan
        /// </summary>
        public Guid MealPlanId { get; set; }

        /// <summary>
        /// Navigation property to MealPlan
        /// </summary>


        /// <summary>
        /// If the meal plan day is completed
        /// </summary>
        public virtual bool IsComplete { get; set; }

        /// <summary>
        /// Collection of meals for this day
        /// </summary>
        public virtual ICollection<MealPlanDayMeal> MealPlanDayMeals { get; set; } = new List<MealPlanDayMeal>();
    }
}