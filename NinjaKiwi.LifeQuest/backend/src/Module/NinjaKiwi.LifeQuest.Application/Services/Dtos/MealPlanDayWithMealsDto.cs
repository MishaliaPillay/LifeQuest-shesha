using System.Collections.Generic;

namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    public class MealPlanDayWithMealsDto
    {
        public int Order { get; set; }
        public string Description { get; set; }
        public int Score { get; set; }
        public List<MealDto> Meals { get; set; } = new();
    }


}
