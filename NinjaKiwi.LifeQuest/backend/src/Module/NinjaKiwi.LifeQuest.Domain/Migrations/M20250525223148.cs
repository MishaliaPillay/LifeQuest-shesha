using FluentMigrator;
using Shesha.FluentMigrator;

namespace NinjaKiwi.LifeQuest.Domain.Migrations
{
    [Migration(20250525223148)]
    public class M20250525223148 : Migration
    {
        public override void Up()
        {
            // Meal Plans table
            Create.Table("LifQu_MealPlans")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Status").AsInt32().NotNullable() // Enum stored as int

                .WithColumn("CompletedAt").AsDateTime().Nullable()
                .WithAuditColumns();

            // MealPlanMeals: many-to-many between MealPlan and Meal
            Create.Table("LifQu_MealPlanMeals")
                .WithColumn("MealPlanId").AsGuid().NotNullable()
                    .ForeignKey("FK_LifQu_MealPlanMeals_MealPlans", "LifQu_MealPlans", "Id")
                .WithColumn("MealId").AsGuid().NotNullable()
                    .ForeignKey("FK_LifQu_MealPlanMeals_Meals", "LifQu_Meals", "Id");

            // Optional: add composite PK for many-to-many
            Create.PrimaryKey("PK_LifQu_MealPlanMeals")
                .OnTable("LifQu_MealPlanMeals")
                .Columns("MealPlanId", "MealId");

        }

        public override void Down()
        {
            Delete.Table("LifQu_MealPlanMeals");
            Delete.Table("LifQu_MealPlans");

            // You might not need this if it was already created before
            // Delete.Column("MealPlanDayId").FromTable("LifQu_MealPlanDayMeals");
            // Delete.Column("MealId").FromTable("LifQu_MealPlanDayMeals");
        }
    }
}
