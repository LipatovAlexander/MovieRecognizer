using FluentMigrator;

namespace DatabaseMigrator.Migrations;

[TimestampedMigration(2024, 02, 18, 15, 49)]
public class VideoFrame : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("VideoFrame")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("VideoId").AsGuid().NotNullable().ForeignKey("Video", "Id").Indexed()
            .WithColumn("Timestamp").AsCustom("interval").NotNullable()
            .WithColumn("ExternalId").AsString().NotNullable().Indexed();
    }
}