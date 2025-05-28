using System.Collections.Generic;

namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    public class CreateMealWithIngredientsDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }
        public List<CreateIngredientDto> Ingredients { get; set; }
    }
}
