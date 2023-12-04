using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EUniversity.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddClassTypes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "ClassTypeId",
            table: "Classes",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateTable(
            name: "ClassTypes",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                CreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                UpdateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClassTypes", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Classes_ClassTypeId",
            table: "Classes",
            column: "ClassTypeId");

        migrationBuilder.AddForeignKey(
            name: "FK_Classes_ClassTypes_ClassTypeId",
            table: "Classes",
            column: "ClassTypeId",
            principalTable: "ClassTypes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Classes_ClassTypes_ClassTypeId",
            table: "Classes");

        migrationBuilder.DropTable(
            name: "ClassTypes");

        migrationBuilder.DropIndex(
            name: "IX_Classes_ClassTypeId",
            table: "Classes");

        migrationBuilder.DropColumn(
            name: "ClassTypeId",
            table: "Classes");
    }
}
