namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    public class CreateIngredientDto
    {
        public string Name { get; set; }
        public int ServingSize { get; set; }
        public int Calories { get; set; }
        public int Protein { get; set; }
        public int Carbohydrates { get; set; }
        public int Fats { get; set; }
    }
}
