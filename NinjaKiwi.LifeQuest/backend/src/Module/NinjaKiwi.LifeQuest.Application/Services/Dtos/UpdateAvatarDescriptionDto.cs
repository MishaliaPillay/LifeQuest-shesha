using System;

namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    public class UpdateAvatarDescriptionDto
    {
        public Guid PlayerId { get; set; }
        public string Description { get; set; }
    }

}
