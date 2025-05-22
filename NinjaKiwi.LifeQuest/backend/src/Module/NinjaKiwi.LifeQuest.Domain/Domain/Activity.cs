using System;
using System.Collections.Generic;
using Abp.Domain.Entities.Auditing;
using Shesha.Domain.Attributes;

namespace NinjaKiwi.LifeQuest.Domain.Domain
{
    [Entity(TypeShortAlias = "LifQu.Activity")]
    public class Activity: FullAuditedEntity<Guid>
    {
      
            /// <summary>
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
            public virtual int Duration { get; set; }

            /// <summary>
            /// The XP awarded for completing this activity
            /// </summary>
            public virtual int Xp { get; set; }

            /// <summary>
            /// The level required to unlock this activity
            /// </summary>
            public virtual int Level { get; set; }

            /// <summary>
            /// Navigation property to related activity types
            /// </summary>
            public virtual ICollection<ActivityActivityType> ActivityActivityTypes { get; set; } = new List<ActivityActivityType>();

            /// <summary>
            /// Whether the activity is marked as complete
            /// </summary>
            public virtual bool IsComplete { get; set; }

           

            /// <summary>
            /// The order of the activity in a sequence
            /// </summary>
            public virtual int Order { get; set; }

            /// <summary>
            /// Optional foreign key to an ExercisePlan
            /// </summary>
           // public virtual Guid? ExercisePlanId { get; set; }

            // public virtual ExercisePlan ExercisePlan { get; set; }
        
    }



}
