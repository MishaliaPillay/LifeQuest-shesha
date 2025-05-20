using Shesha.Domain.Attributes;
using Shesha.Domain;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Shesha.Paths.Domain;

namespace Shesha.Player.Domain.Domain
{
    /// <summary>
    /// A person within the application that is a Player
    /// </summary>
    [Entity(TypeShortAlias = "LifQu.Player")]
    public class Player : Person
    {
        /// <summary>
        /// The experience points of the player
        /// </summary>
        public virtual double Xp { get; set; }

        /// <summary>
        /// The level of the player
        /// </summary>
        public virtual int Level { get; set; }

        /// <summary>
        /// URL or identifier for the player's avatar image
        /// </summary>
        public virtual string Avatar { get; set; }

        /// <summary>
        /// The ID of the associated Path (e.g. FitnessPath, HealthPath)
        /// </summary>
        public virtual Guid? PathId { get; set; }
        public virtual Path SelectedPath { get; set; } // No alias needed anymore


        /// <summary>
        /// A description of the player's avatar
        /// </summary>
        public virtual string AvatarDescription { get; set; }
    }
}
