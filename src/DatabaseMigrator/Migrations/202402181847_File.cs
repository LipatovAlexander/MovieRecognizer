using FluentMigrator;

namespace DatabaseMigrator.Migrations;

[TimestampedMigration(2024, 02, 18, 18, 47)]
public class File : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("File")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("ExternalId").AsString().NotNullable().Indexed();

        Create.Column("FileId").OnTable("VideoFrame")
            .AsGuid().NotNullable().ForeignKey("File", "Id").Indexed();
    }
}