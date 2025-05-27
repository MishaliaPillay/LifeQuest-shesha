using FluentMigrator;
using Shesha.FluentMigrator;

namespace NinjaKiwi.LifeQuest.Domain.Migrations
{
    [Migration(20250525084727)]
    public class M20250525084727_Add_Meals_And_MealIngredients : Migration
    {
        public override void Up()
        {
            Create.Table("LifQu_Meals")
    .WithIdAsGuid()
    .WithCreationAuditColumns()
    .WithModificationAuditColumns()
    .WithDeletionAuditColumns()
    .WithColumn("Name").AsString(250).NotNullable()
    .WithColumn("Description").AsString(int.MaxValue).Nullable()
    .WithColumn("Calories").AsInt32().NotNullable()
    .WithColumn("Score").AsInt32().NotNullable()
    .WithColumn("IsComplete").AsBoolean().NotNullable();



            // MealIngredient table
            Create.Table("LifQu_MealIngredients")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("MealId").AsGuid().NotNullable()
                .WithColumn("IngredientId").AsGuid().NotNullable();

            // Foreign Keys
            Create.ForeignKey("FK_LifQu_MealIngredients_MealId_LifQu_Meals_Id")
                .FromTable("LifQu_MealIngredients").ForeignColumn("MealId")
                .ToTable("LifQu_Meals").PrimaryColumn("Id");

            Create.ForeignKey("FK_LifQu_MealIngredients_IngredientId_LifQu_Ingredients_Id")
                .FromTable("LifQu_MealIngredients").ForeignColumn("IngredientId")
                .ToTable("LifQu_Ingredients").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("LifQu_MealIngredients");
            Delete.Table("LifQu_Meals");
        }
    }
}
