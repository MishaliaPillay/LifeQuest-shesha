using System;
using System.Collections.Generic;

namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    public class CreateMealPlanDto
    {

        public string Name { get; set; }
        public List<MealPlanDayDto> MealPlanDays { get; set; }
    }



}
