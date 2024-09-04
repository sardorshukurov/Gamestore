using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamestore.DAL.Migrations;

/// <inheritdoc />
public partial class AddExplicitRelationUserRoles : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_UserRoles_Users_UserId",
            table: "UserRoles");

        migrationBuilder.DropIndex(
            name: "IX_UserRoles_UserId",
            table: "UserRoles");

        migrationBuilder.DropColumn(
            name: "UserId",
            table: "UserRoles");

        migrationBuilder.AlterColumn<string>(
            name: "LastName",
            table: "Users",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.CreateTable(
            name: "UserUserRole",
            columns: table => new
            {
                RolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserUserRole", x => new { x.RolesId, x.UserId });
                table.ForeignKey(
                    name: "FK_UserUserRole_UserRoles_RolesId",
                    column: x => x.RolesId,
                    principalTable: "UserRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserUserRole_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_UserUserRole_UserId",
            table: "UserUserRole",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UserUserRole");

        migrationBuilder.AlterColumn<string>(
            name: "LastName",
            table: "Users",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AddColumn<Guid>(
            name: "UserId",
            table: "UserRoles",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_UserRoles_UserId",
            table: "UserRoles",
            column: "UserId");

        migrationBuilder.AddForeignKey(
            name: "FK_UserRoles_Users_UserId",
            table: "UserRoles",
            column: "UserId",
            principalTable: "Users",
            principalColumn: "Id");
    }
}
