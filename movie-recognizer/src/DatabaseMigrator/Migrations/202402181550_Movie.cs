using FluentMigrator;

namespace DatabaseMigrator.Migrations;

[TimestampedMigration(2024, 02, 18, 15, 50)]
public class Movie : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Movie")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("ExternalId").AsString().NotNullable().Indexed()
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("Year").AsString().NotNullable()
            .WithColumn("Genres").AsString().NotNullable()
            .WithColumn("Actors").AsString().NotNullable()
            .WithColumn("Plot").AsString().NotNullable()
            .WithColumn("Country").AsString().NotNullable()
            .WithColumn("PosterUrl").AsString().NotNullable()
            .WithColumn("Type").AsString().NotNullable();

        Create.Column("MovieId").OnTable("MovieRecognition")
            .AsGuid().Nullable().ForeignKey("Movie", "Id").Indexed();
    }
}