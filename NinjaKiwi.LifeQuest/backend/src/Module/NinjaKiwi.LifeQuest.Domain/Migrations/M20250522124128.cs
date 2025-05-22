using FluentMigrator;
using Shesha.FluentMigrator;

namespace NinjaKiwi.LifeQuest.Domain.Migrations
{
    [Migration(20250522124128)]
    public class M20250522124128_CreateActivities : Migration
    {
        public override void Up()
        {
            Create.Table("LifQu_Activities")
                .WithIdAsGuid()
                .WithFullAuditColumns()
                .WithColumn("Category").AsString(255).Nullable()
                .WithColumn("Calories").AsInt32().NotNullable()
                .WithColumn("Description").AsString(1000).Nullable()
                .WithColumn("Duration").AsInt32().NotNullable()
                .WithColumn("Xp").AsInt32().NotNullable()
                .WithColumn("Level").AsInt32().NotNullable()
                .WithColumn("IsComplete").AsBoolean().NotNullable()
                .WithColumn("Order").AsInt32().NotNullable();

            // If you want to add ExercisePlanId back later:
            //.WithColumn("ExercisePlanId").AsGuid().Nullable().ForeignKey("LifQu_ExercisePlans", "Id");
        }

        public override void Down()
        {
            Delete.Table("LifQu_Activities");
        }
    }
}
