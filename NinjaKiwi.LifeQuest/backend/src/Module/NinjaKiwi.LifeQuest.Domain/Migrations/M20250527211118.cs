using FluentMigrator;

namespace NinjaKiwi.LifeQuest.Domain.Migrations
{
    [Migration(20250527121000)]
    public class M20250527121000_AddFrwkDiscriminatorToPaths : Migration
    {
        public override void Up()
        {
            // Drop default constraint - you may need raw SQL because FluentMigrator doesn't track default constraints
            Execute.Sql(@"
        DECLARE @constraintName nvarchar(200);
        SELECT @constraintName = df.name
        FROM sys.default_constraints df
        JOIN sys.columns c ON c.default_object_id = df.object_id
        WHERE c.object_id = OBJECT_ID('LifQu_Paths')
          AND c.name = 'Frwk_Discriminator';

        IF @constraintName IS NOT NULL
            EXEC('ALTER TABLE LifQu_Paths DROP CONSTRAINT ' + @constraintName);
    ");

            // Alter column to remove default (just ensure NOT NULL stays)
            Alter.Column("Frwk_Discriminator")
                .OnTable("LifQu_Paths")
                .AsString(200)
                .NotNullable();
        }

        public override void Down()
        {
            // Optionally add back default if needed
            Alter.Column("Frwk_Discriminator")
                .OnTable("LifQu_Paths")
                .AsString(200)
                .NotNullable()
                .WithDefaultValue("LifQu.Path");
        }
    }
}
