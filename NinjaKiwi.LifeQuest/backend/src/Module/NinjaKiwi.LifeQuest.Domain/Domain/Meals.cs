using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Shesha.Domain.Attributes;

namespace NinjaKiwi.LifeQuest.Domain.Domain
{
    [Table("LifQu_Meals")]
    [Entity(TypeShortAlias = "LifQu.Meals")]
    public class Meal : FullAuditedEntity<Guid>
    {

        /// <summary>
        /// The name of the meal
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// The description of the meal
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// The calories of the meals
        /// </summary>
        public virtual int Calories { get; set; }
        /// <summary>
        /// The score of the exercise
        /// </summary>
        public virtual int Score { get; set; }
        /// <summary>
        /// if meal is completed
        /// </summary>
        public virtual bool IsComplete { get; set; }
        public virtual ICollection<MealIngredient> MealIngredients { get; set; }
        public virtual ICollection<MealPlanMeal> MealPlanMeals { get; set; } = new List<MealPlanMeal>();
        public virtual Guid? MealPlanId { get; set; }

        public virtual MealPlan MealPlan { get; set; }

        public Meal()
        {
            MealIngredients = new List<MealIngredient>();
        }
    }


}
