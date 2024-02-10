using FluentMigrator;

namespace DatabaseMigrator.Migrations;

[TimestampedMigration(2024, 02, 10, 18, 56)]
public class CreateJobTable : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Job")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("ExternalId").AsString().NotNullable().Indexed()
            .WithColumn("Type").AsString().NotNullable()
            .WithColumn("MovieRecognitionId").AsGuid().NotNullable().ForeignKey("MovieRecognition", "Id").Indexed();
    }
}