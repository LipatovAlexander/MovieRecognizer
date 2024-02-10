using FluentMigrator;

namespace DatabaseMigrator.Migrations;

[TimestampedMigration(2024, 02, 10, 18, 48)]
public class CreateVideoFrameTable : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("VideoFrame")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("VideoId").AsGuid().NotNullable().ForeignKey("Video", "Id").Indexed()
            .WithColumn("StorageUrl").AsString().NotNullable()
            .WithColumn("Timestamp").AsCustom("interval").NotNullable();
    }
}