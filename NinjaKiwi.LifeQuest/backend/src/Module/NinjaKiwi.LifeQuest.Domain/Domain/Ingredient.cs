using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Shesha.Domain.Attributes;

namespace NinjaKiwi.LifeQuest.Domain.Domain
{
    [Table("LifQu_Ingredients")]
    [Entity(TypeShortAlias = "LifQu.Ingredient")]
    public class Ingredient : Entity<Guid>
    {  /// <summary>
       /// The name of the ingredient
       /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// The calories of the ingredient
        /// </summary>
        public virtual int Calories { get; set; }
        /// <summary>
        /// The carbohydrates of ingredient
        /// </summary>
        public virtual int Carbohydrates { get; set; }
        /// <summary>
        /// The ServingSize of ingredient
        /// </summary>
        public virtual int ServingSize { get; set; }
        /// <summary>
        /// The protein of ingredient
        /// </summary>
        public virtual int Protein { get; set; }
        /// <summary>
        /// The fat of the ingredient
        /// </summary>
        public virtual int Fat { get; set; }


    }



}
