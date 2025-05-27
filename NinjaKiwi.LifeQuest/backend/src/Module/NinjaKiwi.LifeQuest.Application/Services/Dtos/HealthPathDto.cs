using System;
using System.Collections.Generic;

namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    public class HealthPathDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<MealPlanDto> MealPlans { get; set; }

        public Guid PlayerId { get; set; }
    }
}
