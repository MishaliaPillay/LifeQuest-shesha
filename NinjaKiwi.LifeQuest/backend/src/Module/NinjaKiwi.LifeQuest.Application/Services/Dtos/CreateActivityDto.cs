using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    public class CreateActivityDto
    {
        public string Category { get; set; }
        public int Calories { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int Xp { get; set; }
        public int Level { get; set; }
        public bool IsComplete { get; set; }
        public int Order { get; set; }

        // This replaces the nested list of ActivityActivityTypes
        public List<Guid> ActivityTypeIds { get; set; } = new();
    }

}
