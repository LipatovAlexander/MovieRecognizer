using FluentMigrator;

namespace DatabaseMigrator.Migrations;

[TimestampedMigration(2024, 02, 10, 19, 50)]
public class CreateParentJobIdColumn : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Column("ParentJobId").OnTable("Job")
            .AsGuid().Nullable().ForeignKey("Job", "Id").Indexed();
    }
}