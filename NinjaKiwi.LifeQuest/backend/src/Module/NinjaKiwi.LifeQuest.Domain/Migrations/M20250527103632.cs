using FluentMigrator;
using Shesha.FluentMigrator;

namespace NinjaKiwi.LifeQuest.Domain.Migrations
{
    [Migration(20250527103632)]
    public class M20250527103632 : Migration
    {
        public override void Up()
        {
            Alter.Table("LifQu_MealPlanDays")
                .AddColumn("MealPlanId").AsGuid().NotNullable()
                .ForeignKey("FK_LifQu_MealPlanDays_MealPlans", "LifQu_MealPlans", "Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_LifQu_MealPlanDays_MealPlans").OnTable("LifQu_MealPlanDays");
            Delete.Column("MealPlanId").FromTable("LifQu_MealPlanDays");
        }
    }
}