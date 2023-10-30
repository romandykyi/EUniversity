using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EUniversity.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddStudentGroup : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "CourseId",
            table: "Groups",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateTable(
            name: "StudentGroups",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                GroupId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_StudentGroups", x => x.Id);
                table.ForeignKey(
                    name: "FK_StudentGroups_AspNetUsers_StudentId",
                    column: x => x.StudentId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_StudentGroups_Groups_GroupId",
                    column: x => x.GroupId,
                    principalTable: "Groups",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Groups_CourseId",
            table: "Groups",
            column: "CourseId");

        migrationBuilder.CreateIndex(
            name: "IX_StudentGroups_GroupId",
            table: "StudentGroups",
            column: "GroupId");

        migrationBuilder.CreateIndex(
            name: "IX_StudentGroups_StudentId",
            table: "StudentGroups",
            column: "StudentId");

        migrationBuilder.AddForeignKey(
            name: "FK_Groups_Courses_CourseId",
            table: "Groups",
            column: "CourseId",
            principalTable: "Courses",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Groups_Courses_CourseId",
            table: "Groups");

        migrationBuilder.DropTable(
            name: "StudentGroups");

        migrationBuilder.DropIndex(
            name: "IX_Groups_CourseId",
            table: "Groups");

        migrationBuilder.DropColumn(
            name: "CourseId",
            table: "Groups");
    }
}
