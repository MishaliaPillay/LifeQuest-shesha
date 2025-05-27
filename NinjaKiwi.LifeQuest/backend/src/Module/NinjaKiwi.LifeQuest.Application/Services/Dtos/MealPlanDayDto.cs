using System;
using System.Collections.Generic;

namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    public class MealPlanDayDto
    {
        public int Order { get; set; }  // The order of the day in the meal plan
        public string Description { get; set; }  // Description of the meal plan day
        public List<Guid> Meals { get; set; }  // List of meal IDs (no meal details)
        public int Score { get; set; }  // Score associated with the meal plan day
    }

}
