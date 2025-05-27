using System;
using System.Collections.Generic;

namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    public class CreateMealDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }
        public int Score { get; set; }
        public bool IsComplete { get; set; }
        public List<Guid> IngredientIds { get; set; } // optional
    }
}
