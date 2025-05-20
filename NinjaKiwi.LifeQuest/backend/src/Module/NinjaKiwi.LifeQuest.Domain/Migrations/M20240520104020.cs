using FluentMigrator;
using Shesha.FluentMigrator;

namespace Shesha.Paths.Domain.Migrations
{
    [Migration(20240520104020)]
    public class M20240520104020 : Migration
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
