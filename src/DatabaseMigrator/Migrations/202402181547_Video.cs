using FluentMigrator;

namespace DatabaseMigrator.Migrations;

[TimestampedMigration(2024, 02, 18, 15, 47)]
public class Video : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Video")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("ExternalId").AsString().NotNullable().Indexed()
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("Author").AsString().NotNullable()
            .WithColumn("Duration").AsCustom("interval").Nullable();

        Create.Column("VideoId").OnTable("MovieRecognition")
            .AsGuid().Nullable().ForeignKey("Video", "Id").Indexed();
    }
}