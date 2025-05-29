
using FluentMigrator;
using Shesha.FluentMigrator;

namespace NinjaKiwi.LifeQuest.Domain.Migrations
{
    [Migration(20250529103844)]
    public class M20250529103844 : Migration
    {
        public override void Up()
        {
            Alter.Table("LifQu_Meals")
                .AddColumn("MealPlanDayId").AsGuid().Nullable();

            Create.ForeignKey("FK_LifQu_Meals_MealPlanDayId_RefListMealPlanDays")
                .FromTable("LifQu_Meals").ForeignColumn("MealPlanDayId")
                .ToTable("Frwk_ReferenceListItems").PrimaryColumn("Id");
        }


        public override void Down()
        {
            // Drop foreign key first (if created)
            Delete.ForeignKey("FK_LifQu_Meals_MealPlanDayId_RefListMealPlanDays").OnTable("LifQu_Meals");

            // Then drop column
            Delete.Column("MealPlanDayId").FromTable("LifQu_Meals");
        }
    }
}
