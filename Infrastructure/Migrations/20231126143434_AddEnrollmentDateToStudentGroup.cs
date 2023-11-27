using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EUniversity.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddEnrollmentDateToStudentGroup : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "EnrollmentDate",
            table: "StudentGroups",
            type: "datetimeoffset",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "EnrollmentDate",
            table: "StudentGroups");
    }
}
