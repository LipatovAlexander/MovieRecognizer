using FluentMigrator;

namespace DatabaseMigrator.Migrations;

[TimestampedMigration(2024, 02, 04, 22, 36)]
public class CreateRecognitionRequest : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("RecognitionRequests")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("VideoUrl").AsString().NotNullable()
            .WithColumn("CreatedAt").AsDateTimeOffset().NotNullable()
            .WithColumn("Status").AsString().NotNullable();
    }
}