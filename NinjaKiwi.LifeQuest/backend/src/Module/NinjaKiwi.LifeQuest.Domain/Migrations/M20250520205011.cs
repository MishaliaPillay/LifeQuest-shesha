
using FluentMigrator;
using Shesha.FluentMigrator;
namespace NinjaKiwi.LifeQuest.Domain.Migrations
{
    [Migration(20250520090100)]
    public class M20250520090100_CreateActivityType : Migration
    {
        public override void Up()
        {
            Create.Table("LifQu_ActivityTypes")
                .WithIdAsGuid()
                .WithFullAuditColumns()
                .WithColumn("Category").AsString(255).Nullable()
                .WithColumn("Calories").AsInt32().Nullable()
                .WithColumn("Description").AsString(1000).Nullable()
                .WithColumn("Duration").AsString(100).Nullable();
        }

        public override void Down()
        {
            Delete.Table("LifeQu_ActivityTypes");
        }
    }
}
