using System;
using System.Collections.Generic;

namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    public class ActivityDto
    {
        public Guid Id { get; set; }
        public string Category { get; set; }
        public int Calories { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int Xp { get; set; }
        public int Level { get; set; }
        public bool IsComplete { get; set; }
        public int Order { get; set; }

        public List<Guid> ActivityTypeIds { get; set; } = new();
    }

}
