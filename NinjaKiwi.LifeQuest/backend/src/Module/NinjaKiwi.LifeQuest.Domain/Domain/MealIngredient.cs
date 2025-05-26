using System;
using Abp.Domain.Entities;
using Shesha.Domain.Attributes;

namespace NinjaKiwi.LifeQuest.Domain.Domain
{
    [Entity(TypeShortAlias = "LifQu.MealIngredient")]
    public class MealIngredient
    {



        /// <summary>
        /// 
        /// </summary>

        public virtual Meals Meal { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public virtual Ingredient Ingredient { get; set; }




    }



}
