using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamestore.DAL.Migrations;

/// <inheritdoc />
public partial class AddRelationGamePublisher : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_Publisher",
            table: "Publisher");

        migrationBuilder.RenameTable(
            name: "Publisher",
            newName: "Publishers");

        migrationBuilder.RenameIndex(
            name: "IX_Publisher_CompanyName",
            table: "Publishers",
            newName: "IX_Publishers_CompanyName");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Publishers",
            table: "Publishers",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_Games_PublisherId",
            table: "Games",
            column: "PublisherId");

        migrationBuilder.AddForeignKey(
            name: "FK_Games_Publishers_PublisherId",
            table: "Games",
            column: "PublisherId",
            principalTable: "Publishers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Games_Publishers_PublisherId",
            table: "Games");

        migrationBuilder.DropIndex(
            name: "IX_Games_PublisherId",
            table: "Games");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Publishers",
            table: "Publishers");

        migrationBuilder.RenameTable(
            name: "Publishers",
            newName: "Publisher");

        migrationBuilder.RenameIndex(
            name: "IX_Publishers_CompanyName",
            table: "Publisher",
            newName: "IX_Publisher_CompanyName");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Publisher",
            table: "Publisher",
            column: "Id");
    }
}
