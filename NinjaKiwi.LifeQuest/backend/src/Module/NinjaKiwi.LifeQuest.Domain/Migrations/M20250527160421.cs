using FluentMigrator;
using Shesha.FluentMigrator;

namespace NinjaKiwi.LifeQuest.Domain.Migrations
{
    [Migration(20250527160421)] // new timestamp to reflect a new migration
    public class M20250527160421 : Migration
    {
        public override void Up()
        {
            Create.Table("WeightEntries")
                .WithIdAsGuid()
                .WithColumn("PlayerId").AsGuid().NotNullable()
                .WithColumn("Weight").AsFloat().NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("Note").AsString(500).Nullable();

            Create.ForeignKey("FK_WeightEntries_Player")
                .FromTable("WeightEntries").ForeignColumn("PlayerId")
                .ToTable("Core_Persons").PrimaryColumn("Id"); // Players are stored in Persons
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_WeightEntries_Player").OnTable("WeightEntries");
            Delete.Table("WeightEntries");
        }
    }
}
