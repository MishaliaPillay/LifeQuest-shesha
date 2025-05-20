using Shesha.Domain.Attributes;
using Shesha.Domain;
using System;
using Castle.Components.DictionaryAdapter;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column("LifQu_Xp")] 
        public virtual double? Xp { get; set; }

        /// <summary>
        /// The player's level
        /// </summary>
        [Column("LifQu_Level")] 
        public virtual int? Level { get; set; }

        /// <summary>
        /// The player's avatar image URL
        /// </summary>
        [Column("LifQu_Avatar")]
        public virtual string Avatar { get; set; }

        /// <summary>
        /// The player's avatar description
        /// </summary>
        [Column("LifQu_AvatarDescription")] 
        public virtual string AvatarDescription { get; set; }

        /// <summary>
        /// Navigation property to the selected Path
        /// </summary>
        [Reference]
        [Column("LifQu_SelectedPathId")] 
        public virtual Path SelectedPath { get; set; }
    }
}