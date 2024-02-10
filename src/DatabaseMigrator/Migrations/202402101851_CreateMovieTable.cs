using FluentMigrator;

namespace DatabaseMigrator.Migrations;

[TimestampedMigration(2024, 02, 10, 18, 51)]
public class CreateMovieTable : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table("Movie")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("ImdbId").AsString().NotNullable().Indexed()
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