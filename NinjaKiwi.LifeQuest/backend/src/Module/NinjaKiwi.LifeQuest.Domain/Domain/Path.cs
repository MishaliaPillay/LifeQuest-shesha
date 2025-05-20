using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Shesha.Domain.Attributes;

namespace NinjaKiwi.LifeQuest.Domain.Domain
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
