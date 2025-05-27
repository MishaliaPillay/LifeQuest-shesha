using FluentMigrator;
using Shesha.FluentMigrator;

namespace NinjaKiwi.LifeQuest.Domain.Migrations
{
    [Migration(20250527200856)] // Use a unique and current timestamp
    public class M20250527200856 : Migration
    {
        public override void Up()
        {
            // Step 1: Add HealthPathId as nullable
            Alter.Table("LifQu_MealPlans")
                .AddColumn("HealthPathId").AsGuid().Nullable();


            // Step 3: Make HealthPathId not nullable and add foreign key
            Alter.Column("HealthPathId").OnTable("LifQu_MealPlans").AsGuid().NotNullable();

            Create.ForeignKey("FK_LifQu_MealPlans_HealthPaths")
                .FromTable("LifQu_MealPlans").ForeignColumn("HealthPathId")
                .ToTable("LifQu_Paths").PrimaryColumn("Id");


        }

        public override void Down()
        {
            Delete.ForeignKey("FK_LifQu_MealPlans_HealthPaths").OnTable("LifQu_MealPlans");
            Delete.Column("HealthPathId").FromTable("LifQu_MealPlans");
        }
    }
}
