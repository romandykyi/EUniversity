using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EUniversity.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddAssignedGrades : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AssignedGrades",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                GradeId = table.Column<int>(type: "int", nullable: false),
                GroupId = table.Column<int>(type: "int", nullable: false),
                AssignerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                ReassignerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                CreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                UpdateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ActivityTypeId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AssignedGrades", x => x.Id);
                table.ForeignKey(
                    name: "FK_AssignedGrades_ActivityTypes_ActivityTypeId",
                    column: x => x.ActivityTypeId,
                    principalTable: "ActivityTypes",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_AssignedGrades_AspNetUsers_AssignerId",
                    column: x => x.AssignerId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_AssignedGrades_AspNetUsers_ReassignerId",
                    column: x => x.ReassignerId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_AssignedGrades_AspNetUsers_StudentId",
                    column: x => x.StudentId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_AssignedGrades_Grades_GradeId",
                    column: x => x.GradeId,
                    principalTable: "Grades",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AssignedGrades_Groups_GroupId",
                    column: x => x.GroupId,
                    principalTable: "Groups",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AssignedGrades_ActivityTypeId",
            table: "AssignedGrades",
            column: "ActivityTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_AssignedGrades_AssignerId",
            table: "AssignedGrades",
            column: "AssignerId");

        migrationBuilder.CreateIndex(
            name: "IX_AssignedGrades_GradeId",
            table: "AssignedGrades",
            column: "GradeId");

        migrationBuilder.CreateIndex(
            name: "IX_AssignedGrades_GroupId",
            table: "AssignedGrades",
            column: "GroupId");

        migrationBuilder.CreateIndex(
            name: "IX_AssignedGrades_ReassignerId",
            table: "AssignedGrades",
            column: "ReassignerId");

        migrationBuilder.CreateIndex(
            name: "IX_AssignedGrades_StudentId",
            table: "AssignedGrades",
            column: "StudentId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AssignedGrades");
    }
}
