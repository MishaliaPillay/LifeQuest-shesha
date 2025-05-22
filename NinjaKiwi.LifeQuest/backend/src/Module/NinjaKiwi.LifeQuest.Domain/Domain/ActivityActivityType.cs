using System;
using Abp.Domain.Entities;
using Shesha.Domain.Attributes;

namespace NinjaKiwi.LifeQuest.Domain.Domain
{
    [Entity(TypeShortAlias = "LifQu.ActivityActivityType")]
    public class ActivityActivityType : Entity<Guid>
    {  /// <summary>
       /// 
       /// </summary>
        public virtual Guid ActivityId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual Activity Activity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid ActivityTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public virtual ActivityType ActivityType { get; set; }


    }



}
