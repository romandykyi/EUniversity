using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EUniversity.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddClasses : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Classes",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ClassroomId = table.Column<int>(type: "int", nullable: false),
                GroupId = table.Column<int>(type: "int", nullable: false),
                SubstituteTeacherId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                DurationTicks = table.Column<long>(type: "bigint", nullable: false),
                Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                CreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                UpdateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Classes", x => x.Id);
                table.ForeignKey(
                    name: "FK_Classes_AspNetUsers_SubstituteTeacherId",
                    column: x => x.SubstituteTeacherId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Classes_Classrooms_ClassroomId",
                    column: x => x.ClassroomId,
                    principalTable: "Classrooms",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Classes_Groups_GroupId",
                    column: x => x.GroupId,
                    principalTable: "Groups",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Classes_ClassroomId",
            table: "Classes",
            column: "ClassroomId");

        migrationBuilder.CreateIndex(
            name: "IX_Classes_GroupId",
            table: "Classes",
            column: "GroupId");

        migrationBuilder.CreateIndex(
            name: "IX_Classes_SubstituteTeacherId",
            table: "Classes",
            column: "SubstituteTeacherId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Classes");
    }
}
