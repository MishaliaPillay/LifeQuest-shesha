using FluentMigrator;
using Shesha.FluentMigrator;
using System;

namespace NinjaKiwi.LifeQuest.Domain.Migrations
{

    // <summary>
    /// Adding the Members table
    /// </summary>

    [Migration(20231124085300)]
    public class M20231124085300 : Migration
    {
        /// <summary>
        /// Code to execute when executing the migrations
        /// </summary>
        public override void Up()
        {
            Alter.Table("Core_Persons")
                .AddColumn("LifQu_MembershipNumber").AsString().Nullable()
                .AddForeignKeyColumn("LifQu_IdDocumentId", "Frwk_StoredFiles").Nullable()
                .AddColumn("LifQu_MembershipStartDate").AsDateTime().Nullable()
                .AddColumn("LifQu_MembershipEndDate").AsDateTime().Nullable()
                .AddColumn("LifQu_MembershipStatusLkp").AsInt64().Nullable();
        }
        /// <summary>
        /// Code to execute when rolling back the migration
        /// </summary>
        public override void Down()
        {
            throw new NotImplementedException();
        }
    }

}
