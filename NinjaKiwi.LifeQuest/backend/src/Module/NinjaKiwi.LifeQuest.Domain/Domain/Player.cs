using Shesha.Domain.Attributes;
using Shesha.Domain;
using System;

namespace NinjaKiwi.LifeQuest.Domain.Domain
{
    /// <summary>
    /// A person within the application that is a Player
    /// </summary>
    [Entity(TypeShortAlias = "LifQu.Player")]
    public class Player : Person
    {




        /// <summary>
        /// The player's experience points
        /// </summary>
        public virtual double? Xp { get; set; }

        /// <summary>
        /// The player's level
        /// </summary>
        public virtual int? Level { get; set; }

        /// <summary>
        /// The player's avatar image URL
        /// </summary>
        public virtual string Avatar { get; set; }

        /// <summary>
        /// The path selected by the player
        /// </summary>

        /// <summary>
        /// Navigation property to the Path entity
        /// </summary>

        /// <summary>
        /// The player's avatar description
        /// </summary>
        public virtual string AvatarDescription { get; set; }



    }
}
