using System;

namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    public class PlayerDto
    {
        public Guid Id { get; set; }
        public long? UserId { get; set; }
        public string? Avatar { get; set; }
        public string? AvatarDescription { get; set; }
        public double? Xp { get; set; }
        public int? Level { get; set; }
        public string? Name { get; set; }
    }



}
