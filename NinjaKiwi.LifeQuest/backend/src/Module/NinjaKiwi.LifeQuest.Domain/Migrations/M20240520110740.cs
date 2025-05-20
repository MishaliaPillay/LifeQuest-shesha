using FluentMigrator;
using Shesha.FluentMigrator;
using System;

namespace Shesha.Player.Domain.Migrations
{
    /// <summary>
    /// Adding Player-specific fields to Core_Persons
    /// </summary>
    [Migration(20240520110740)]
    public class M20240520110740 : Migration
    {
        public override void Up()
        {
            Alter.Table("Core_Persons")
                .AddColumn("LifQu_Xp").AsDouble().Nullable()
                .AddColumn("LifQu_Level").AsInt32().Nullable()
                .AddColumn("LifQu_Avatar").AsString().Nullable()
                .AddColumn("LifQu_AvatarDescription").AsString().Nullable()
                .AddForeignKeyColumn("LifQu_PathId", "LifQu_Paths").Nullable();

        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
