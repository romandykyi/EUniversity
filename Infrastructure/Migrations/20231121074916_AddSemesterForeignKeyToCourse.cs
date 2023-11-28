using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EUniversity.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddSemesterForeignKeyToCourse : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "SemesterId",
            table: "Courses",
            type: "int",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Courses_SemesterId",
            table: "Courses",
            column: "SemesterId");

        migrationBuilder.AddForeignKey(
            name: "FK_Courses_Semesters_SemesterId",
            table: "Courses",
            column: "SemesterId",
            principalTable: "Semesters",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Courses_Semesters_SemesterId",
            table: "Courses");

        migrationBuilder.DropIndex(
            name: "IX_Courses_SemesterId",
            table: "Courses");

        migrationBuilder.DropColumn(
            name: "SemesterId",
            table: "Courses");
    }
}
