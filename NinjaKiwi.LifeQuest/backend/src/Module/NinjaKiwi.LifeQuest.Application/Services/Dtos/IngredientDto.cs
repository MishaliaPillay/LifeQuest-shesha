using System;

namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    public class IngredientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double ServingSize { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; }
        public double Fats { get; set; }
    }
}
