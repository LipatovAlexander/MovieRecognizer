using FluentMigrator;

namespace DatabaseMigrator.Migrations;

[TimestampedMigration(2024, 02, 10, 18, 28)]
public class RenameMovieRecognitionsTable : ForwardOnlyMigration
{
    public override void Up()
    {
        Rename.Table("MovieRecognitions").To("MovieRecognition");
    }
}