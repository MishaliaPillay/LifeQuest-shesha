namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    public class GenerateAIMealInputDto
    {
        public string DietaryRequirement { get; set; } = "vegetarian";
        public string PreferredCuisine { get; set; } = string.Empty; // e.g. "Mexican", "Mediterranean"
        public string MealType { get; set; } = string.Empty;     // e.g. "breakfast", "dinner"    
        public int MaxCalories { get; set; } = 0;// e.g. 500
    }
}
