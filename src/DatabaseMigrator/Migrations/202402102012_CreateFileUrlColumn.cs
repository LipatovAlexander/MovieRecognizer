using FluentMigrator;

namespace DatabaseMigrator.Migrations;

[TimestampedMigration(2024, 02, 10, 20, 12)]
public class CreateFileUrlColumn : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Column("FileUrl").OnTable("Video")
            .AsString().NotNullable();
    }
}