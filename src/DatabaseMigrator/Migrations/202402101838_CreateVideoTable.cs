using FluentMigrator;

namespace DatabaseMigrator.Migrations;

[TimestampedMigration(2024, 02, 10, 18, 38)]
public class CreateVideoTable : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Video")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("Author").AsString().NotNullable()
            .WithColumn("Duration").AsCustom("interval").NotNullable()
            .WithColumn("VideoUrl").AsString().NotNullable()
            .WithColumn("WebSiteUrl").AsString().NotNullable();

        Create.Column("VideoId").OnTable("MovieRecognition")
            .AsGuid().Nullable().ForeignKey("Video", "Id").Indexed();
    }
}