using FluentMigrator;
using Shesha.FluentMigrator;

namespace NinjaKiwi.LifeQuest.Domain.Migrations
{
    [Migration(20250522124250)]
    public class M20250522124250_CreateActivityActivityTypes : Migration
    {
        public override void Up()
        {
            Create.Table("LifQu_ActivityActivityTypes")
                .WithIdAsGuid()
                .WithColumn("ActivityId").AsGuid().NotNullable().ForeignKey("LifQu_Activities", "Id")
                .WithColumn("ActivityTypeId").AsGuid().NotNullable().ForeignKey("LifQu_ActivityTypes", "Id");
        }

        public override void Down()
        {
            Delete.Table("LifQu_ActivityActivityTypes");
        }
    }
}
