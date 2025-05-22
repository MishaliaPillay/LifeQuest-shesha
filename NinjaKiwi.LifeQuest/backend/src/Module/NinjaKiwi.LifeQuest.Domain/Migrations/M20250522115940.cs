using FluentMigrator;
using Shesha.FluentMigrator;

namespace NinjaKiwi.LifeQuest.Domain.Migrations
{
    [Migration(20250522115940)]
    public class M20250522115940_CreateIngredients : Migration
    {
        public override void Up()
        {
            Create.Table("LifQu_Ingredients")
                .WithIdAsGuid()
                .WithFullAuditColumns()
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Calories").AsInt32().NotNullable()
                .WithColumn("Carbohydrates").AsInt32().NotNullable()
                .WithColumn("ServingSize").AsInt32().NotNullable()
                .WithColumn("Protein").AsInt32().NotNullable()
                .WithColumn("Fat").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("LifQu_Ingredients");
        }
    }
}
