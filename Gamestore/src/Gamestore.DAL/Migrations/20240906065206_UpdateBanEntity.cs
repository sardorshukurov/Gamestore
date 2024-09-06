using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamestore.DAL.Migrations;

/// <inheritdoc />
public partial class UpdateBanEntity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "UserName",
            table: "Bans");

        migrationBuilder.AddColumn<Guid>(
            name: "UserId",
            table: "Bans",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "UserId",
            table: "Bans");

        migrationBuilder.AddColumn<string>(
            name: "UserName",
            table: "Bans",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);
    }
}
