using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EUniversity.Infrastructure.Migrations;

/// <inheritdoc />
public partial class RemoveClassDurationTicks : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "DurationTicks",
            table: "Classes");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<long>(
            name: "DurationTicks",
            table: "Classes",
            type: "bigint",
            nullable: false,
            defaultValue: 0L);
    }
}
