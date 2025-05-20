using FluentMigrator;
using Shesha.FluentMigrator;
using System;

namespace NinjaKiwi.LifeQuest.Domain.Migrations
{
    [Migration(20250520160000)]
    public class M20250520160000 : Migration
    {
        public override void Up()
        {


            // Then add columns with the correct prefix matching your entity TypeShortAlias
            Alter.Table("Core_Persons")
                .AddColumn("LifQu_Xp").AsDouble().Nullable()
                .AddColumn("LifQu_Level").AsInt32().Nullable()
                .AddColumn("LifQu_Avatar").AsString().Nullable()
                .AddColumn("LifQu_AvatarDescription").AsString().Nullable()
                .AddColumn("LifQu_SelectedPathId").AsGuid().Nullable()
                .ForeignKey("FK_Core_Persons_SelectedPath", "LifQu_Paths", "Id");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}