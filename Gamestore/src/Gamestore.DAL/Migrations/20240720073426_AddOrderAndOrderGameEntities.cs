using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamestore.DAL.Migrations;

/// <inheritdoc />
public partial class AddOrderAndOrderGameEntities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Order",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Order", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "OrdersGames",
            columns: table => new
            {
                OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Price = table.Column<double>(type: "float", nullable: false),
                Quantity = table.Column<int>(type: "int", nullable: false),
                Discount = table.Column<int>(type: "int", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OrdersGames", x => new { x.OrderId, x.ProductId });
                table.ForeignKey(
                    name: "FK_OrdersGames_Games_ProductId",
                    column: x => x.ProductId,
                    principalTable: "Games",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_OrdersGames_Order_OrderId",
                    column: x => x.OrderId,
                    principalTable: "Order",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_OrdersGames_ProductId",
            table: "OrdersGames",
            column: "ProductId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "OrdersGames");

        migrationBuilder.DropTable(
            name: "Order");
    }
}
