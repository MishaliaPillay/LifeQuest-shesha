using FluentMigrator;
using Shesha.FluentMigrator;
using System;


namespace NinjaKiwi.LifeQuest.Domain.Migrations
{
    [Migration(20250520150500)]
    public class M20250520150500 : Migration
    {
        public override void Up()
        {
            Alter.Table("Core_Persons")
                .AddColumn("LifeQuest_Xp").AsDouble().Nullable()
                .AddColumn("LifeQuest_Level").AsInt32().Nullable()
                .AddColumn("LifeQuest_Avatar").AsString().Nullable()
                .AddColumn("LifeQuest_AvatarDescription").AsString().Nullable()
                ;
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
