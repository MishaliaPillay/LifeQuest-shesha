using System;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Shesha.Domain.Attributes;

namespace NinjaKiwi.LifeQuest.Domain.Domain
{
    /// <summary>
    /// Represents a step-tracking entry for a Player
    /// </summary>
    [Entity(TypeShortAlias = "LifQu.StepEntry")]
    [Table("LifQu_StepEntries")]
    public class StepEntry : Entity<Guid>
    {
        /// <summary>
        /// The Player that owns this StepEntry
        /// </summary>
        [Column("PlayerId")]
        public virtual Player Player { get; set; }

        /// <summary>
        /// Number of steps taken
        /// </summary>
        [Column("Steps")]
        public virtual int Steps { get; set; }

        /// <summary>
        /// The date of the entry
        /// </summary>
        [Column("Date")]
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Optional note
        /// </summary>
        [Column("Note")]
        public virtual string Note { get; set; }
        public Guid PlayerId { get; set; }
        /// <summary>
        /// Calories burned
        /// </summary>
        [Column("CaloriesBurned")]
        public virtual int CaloriesBurned { get; set; }
    }
}
