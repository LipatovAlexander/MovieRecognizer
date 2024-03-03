using FluentMigrator;

namespace DatabaseMigrator.Migrations;

[TimestampedMigration(2024, 02, 18, 15, 45)]
public class MovieRecognition : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("MovieRecognition")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("VideoUrl").AsString().NotNullable()
            .WithColumn("CreatedAt").AsDateTimeOffset().NotNullable()
            .WithColumn("Status").AsString().NotNullable();
    }
}