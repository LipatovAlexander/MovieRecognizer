using FluentMigrator;

namespace DatabaseMigrator.Migrations;

[TimestampedMigration(2024, 02, 18, 15, 51)]
public class Job : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Job")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("ExternalId").AsString().NotNullable().Indexed()
            .WithColumn("Type").AsString().NotNullable()
            .WithColumn("MovieRecognitionId").AsGuid().NotNullable().ForeignKey("MovieRecognition", "Id").Indexed()
            .WithColumn("ParentJobId").AsGuid().Nullable().ForeignKey("Job", "Id").Indexed();
    }
}