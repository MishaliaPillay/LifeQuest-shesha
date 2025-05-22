using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Shesha.Domain.Attributes;

namespace NinjaKiwi.LifeQuest.Domain.Domain
{
    [Entity(TypeShortAlias = "LifQu.ActivityType")]
    public class ActivityType : FullAuditedEntity<Guid>
    {  /// <summary>
       /// The category of the exercise
       /// </summary>
        public virtual string Category { get; set; }
        /// <summary>
        /// The calories of the exercise
        /// </summary>
        public virtual int Calories { get; set; }
        /// <summary>
        /// The description of exercise
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// The duration of the exercise
        /// </summary>
        public virtual string Duration { get; set; }

        // Navigation property to related activities
        //  public virtual IList<ActivityActivityType> Activities { get; set; } = new List<ActivityActivityType>();
    }



}
