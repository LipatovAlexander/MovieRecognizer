using FluentMigrator;

namespace DatabaseMigrator.Migrations;

[TimestampedMigration(2024, 02, 05, 18, 21)]
public class RenameRecognitionRequest : ForwardOnlyMigration
{
    public override void Up()
    {
        Rename.Table("RecognitionRequests").To("MovieRecognitions");
    }
}