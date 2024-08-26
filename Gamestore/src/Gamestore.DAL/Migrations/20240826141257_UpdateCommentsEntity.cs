using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamestore.DAL.Migrations;

/// <inheritdoc />
public partial class UpdateCommentsEntity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "Discount",
            table: "Games",
            type: "int",
            nullable: false,
            oldClrType: typeof(float),
            oldType: "real");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<float>(
            name: "Discount",
            table: "Games",
            type: "real",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");
    }
}
