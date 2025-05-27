using System;
using Abp.Domain.Entities.Auditing;
using Shesha.Domain.Attributes;

namespace NinjaKiwi.LifeQuest.Domain.Domain
{
    /// <summary>
    /// Represents a Path in LifeQuest (FitnessPath, HealthPath)
    /// </summary>
    [Entity(TypeShortAlias = "LifQu.Path")]
    [Discriminator] // ✅ Add this line
    public class Path : FullAuditedEntity<Guid>
    {
        public virtual string Title { get; set; }

        public virtual string PathType { get; set; }
    }
}
