using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Shesha.Domain.Attributes;

namespace NinjaKiwi.LifeQuest.Domain.Domain
{
    [Table("LifQu_MealIngredients")]
    [Entity(TypeShortAlias = "LifQu.MealIngredient")]
    public class MealIngredient : Entity<Guid>
    {
        public virtual Guid MealId { get; set; }
        public virtual Guid IngredientId { get; set; }

        public virtual Meal Meal { get; set; }
        public virtual Ingredient Ingredient { get; set; }
    }
}
