using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Shesha.Domain.Attributes;

namespace NinjaKiwi.LifeQuest.Domain.Domain
{
    [Table("LifQu_MealPlanMeals")]
    [Entity(TypeShortAlias = "LifQu.MealPlanMeal")]
    public class MealPlanMeal : Entity<Guid>
    {
        public Guid MealPlanId { get; set; }
        public virtual MealPlan MealPlan { get; set; }

        public Guid MealId { get; set; }
        public virtual Meal Meal { get; set; }
    }

}
