using System;

using FluentMigrator;
using Shesha.FluentMigrator;

namespace NinjaKiwi.LifeQuest.Domain.Migrations
{
    [Migration(20250527193157)]
    public class M20250527193157 : Migration
    {
        public override void Up()
        {
            Create.Table("LifQu_StepEntries")
                .WithIdAsGuid()
                .WithColumn("PlayerId").AsGuid().NotNullable()
                .WithColumn("Steps").AsInt32().NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("Note").AsString(1000).Nullable()
                .WithColumn("CaloriesBurned").AsInt32().NotNullable();

            Create.ForeignKey("FK_LifQu_StepEntries_Player")
                .FromTable("LifQu_StepEntries").ForeignColumn("PlayerId")
                .ToTable("Core_Persons").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_LifQu_StepEntries_Player").OnTable("LifQu_StepEntries");
            Delete.Table("LifQu_StepEntries");
        }
    }
}
