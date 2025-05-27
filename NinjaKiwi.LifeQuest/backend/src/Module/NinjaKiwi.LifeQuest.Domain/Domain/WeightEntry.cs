using System;
using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace NinjaKiwi.LifeQuest.Domain.Domain
{
    /// <summary>
    /// Represents a weight tracking entry for a Player
    /// </summary>
    [Table("WeightEntries")]
    public class WeightEntry : Entity<Guid>
    {
        [ForeignKey(nameof(Player))]
        public Guid PlayerId { get; set; }

        public float Weight { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }

        public virtual Player Player { get; set; }
    }
}
