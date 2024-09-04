using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamestore.DAL.Migrations;

/// <inheritdoc />
public partial class UpdateGenreMakeFieldsNullable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<byte[]>(
            name: "Picture",
            table: "Genres",
            type: "varbinary(max)",
            nullable: true,
            oldClrType: typeof(byte[]),
            oldType: "varbinary(max)");

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "Genres",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AddColumn<int>(
            name: "OriginalId",
            table: "Games",
            type: "int",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "OriginalId",
            table: "Games");

        migrationBuilder.AlterColumn<byte[]>(
            name: "Picture",
            table: "Genres",
            type: "varbinary(max)",
            nullable: false,
            defaultValue: Array.Empty<byte>(),
            oldClrType: typeof(byte[]),
            oldType: "varbinary(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Description",
            table: "Genres",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);
    }
}
