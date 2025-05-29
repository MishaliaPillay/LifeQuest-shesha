using System;

namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    public class PlayerHealthPathDto
    {
        public Guid PlayerId { get; set; }
        public string FullName { get; set; }
        public string HealthPathTitle { get; set; }
    }
}
