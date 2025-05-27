using System;
using System.Collections.Generic;

namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    public class CreateMealPlanDayInput
    {
        public string Description { get; set; }
        public List<Guid> MealIds { get; set; }
    }

}
