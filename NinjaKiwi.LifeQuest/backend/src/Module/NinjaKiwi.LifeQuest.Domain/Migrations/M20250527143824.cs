using System;

using FluentMigrator;

namespace NinjaKiwi.LifeQuest.Domain.Migrations
{
    [Migration(20250527143824)]
    public class M20250527143824 : Migration
    {
        public override void Up()
        {
            Alter.Table("LifQu_Meals")
                .AddColumn("MealPlanId").AsGuid().Nullable();

            Create.ForeignKey("FK_LifQu_Meals_MealPlanId_LifQu_MealPlans_Id")
                .FromTable("LifQu_Meals").ForeignColumn("MealPlanId")
                .ToTable("LifQu_MealPlans").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_LifQu_Meals_MealPlanId_LifQu_MealPlans_Id").OnTable("LifQu_Meals");
            Delete.Column("MealPlanId").FromTable("LifQu_Meals");
        }
    }
}
