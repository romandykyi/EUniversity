using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EUniversity.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddCoursesGroupsAndGrades : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Courses",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Courses", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Grades",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Score = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Grades", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Groups",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Groups", x => x.Id);
                table.ForeignKey(
                    name: "FK_Groups_AspNetUsers_TeacherId",
                    column: x => x.TeacherId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "CourseGrades",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CourseId = table.Column<int>(type: "int", nullable: false),
                GradeId = table.Column<int>(type: "int", nullable: false),
                TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CourseGrades", x => x.Id);
                table.ForeignKey(
                    name: "FK_CourseGrades_AspNetUsers_StudentId",
                    column: x => x.StudentId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_CourseGrades_AspNetUsers_TeacherId",
                    column: x => x.TeacherId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_CourseGrades_Courses_CourseId",
                    column: x => x.CourseId,
                    principalTable: "Courses",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_CourseGrades_Grades_GradeId",
                    column: x => x.GradeId,
                    principalTable: "Grades",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_CourseGrades_CourseId",
            table: "CourseGrades",
            column: "CourseId");

        migrationBuilder.CreateIndex(
            name: "IX_CourseGrades_GradeId",
            table: "CourseGrades",
            column: "GradeId");

        migrationBuilder.CreateIndex(
            name: "IX_CourseGrades_StudentId",
            table: "CourseGrades",
            column: "StudentId");

        migrationBuilder.CreateIndex(
            name: "IX_CourseGrades_TeacherId",
            table: "CourseGrades",
            column: "TeacherId");

        migrationBuilder.CreateIndex(
            name: "IX_Groups_TeacherId",
            table: "Groups",
            column: "TeacherId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CourseGrades");

        migrationBuilder.DropTable(
            name: "Groups");

        migrationBuilder.DropTable(
            name: "Courses");

        migrationBuilder.DropTable(
            name: "Grades");
    }
}
