using Shesha.Domain.Attributes;
using Shesha.Domain;
using System;
using Abp.Domain.Entities.Auditing;

namespace Shesha.Paths.Domain
{
    /// <summary>
    /// Represents a Path in LifeQuest ( FitnessPath, HealthPath)
    /// </summary>
    [Entity(TypeShortAlias = "LifQu.Path")]
    public class Path : FullAuditedEntity<Guid> // Inherit from base entity
    {
        /// <summary>
        /// The title or name of the path
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// The type of path (e.g. "Fitness", "Health")
        /// </summary>
        public virtual string PathType { get; set; }
    }
}
