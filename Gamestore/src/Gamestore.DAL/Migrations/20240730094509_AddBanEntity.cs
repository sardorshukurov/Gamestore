using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamestore.DAL.Migrations;

/// <inheritdoc />
public partial class AddBanEntity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Bans",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Duration = table.Column<int>(type: "int", nullable: false),
                StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Bans", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Bans");
    }
}
