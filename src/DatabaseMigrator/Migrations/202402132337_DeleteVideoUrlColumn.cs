using FluentMigrator;

namespace DatabaseMigrator.Migrations;

[TimestampedMigration(2024, 02, 13, 23, 37)]
public class DeleteVideoUrlColumn : ForwardOnlyMigration
{
    public override void Up()
    {
        Delete.Column("VideoUrl").FromTable("Video");
    }
}