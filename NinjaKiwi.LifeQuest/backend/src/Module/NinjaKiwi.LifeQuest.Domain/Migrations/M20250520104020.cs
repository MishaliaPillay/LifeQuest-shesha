using FluentMigrator;
using Shesha.FluentMigrator;

namespace NinjaKiwi.LifeQuest.Domain.Migrations
{
    [Migration(20250520104020)]
    public class M20250520104020 : Migration
    {
        public override void Up()
        {
            Create.Table("LifQu_Paths")
                .WithColumn("Id").AsGuid().PrimaryKey().NotNullable()
                .WithFullAuditColumns()
                .WithColumn("Title").AsString().Nullable()
                .WithColumn("PathType").AsString().Nullable();
        }

        public override void Down()
        {
            Delete.Table("LifQu_Paths");
        }
    }
}
