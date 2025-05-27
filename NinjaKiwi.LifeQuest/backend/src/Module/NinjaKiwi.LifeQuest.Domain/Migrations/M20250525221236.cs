using FluentMigrator;
using Shesha.FluentMigrator;

namespace NinjaKiwi.LifeQuest.Domain.Migrations
{
    [Migration(20250525221236)]
    public class M20250525221236 : Migration
    {
        public override void Up()
        {
            Create.Table("LifQu_MealPlanDays")
                .WithIdAsGuid()
                .WithColumn("Order").AsInt32().NotNullable()
                .WithColumn("Description").AsString().Nullable()
                .WithColumn("IsComplete").AsBoolean().NotNullable();


            Create.Table("LifQu_MealPlanDayMeals")
                .WithIdAsGuid()
                .WithColumn("MealPlanDayId").AsGuid().NotNullable().ForeignKey("FK_LifQu_MealPlanDayMeals_MealPlanDays", "LifQu_MealPlanDays", "Id")
                .WithColumn("MealId").AsGuid().NotNullable().ForeignKey("FK_LifQu_MealPlanDayMeals_Meals", "LifQu_Meals", "Id");

        }

        public override void Down()
        {
            Delete.Table("LifQu_MealPlanDayMeals");
            Delete.Table("LifQu_MealPlanDays");
        }
    }
}
