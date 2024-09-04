using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamestore.DAL.Migrations;

/// <inheritdoc />
public partial class AddNorthwindAttributes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Address",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "City",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "ContactName",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "ContactTitle",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "Country",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "Fax",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "Phone",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "PostalCode",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "Region",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AlterColumn<float>(
            name: "Discount",
            table: "OrdersGames",
            type: "real",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AddColumn<int>(
            name: "EmployeeId",
            table: "Order",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<double>(
            name: "Freight",
            table: "Order",
            type: "float",
            nullable: false,
            defaultValue: 0.0);

        migrationBuilder.AddColumn<DateTime>(
            name: "RequiredDate",
            table: "Order",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<string>(
            name: "ShipAddress",
            table: "Order",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "ShipCity",
            table: "Order",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "ShipCountry",
            table: "Order",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "ShipName",
            table: "Order",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<int>(
            name: "ShipPostalCode",
            table: "Order",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<string>(
            name: "ShipRegion",
            table: "Order",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<int>(
            name: "ShipVia",
            table: "Order",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<DateTime>(
            name: "ShippedVia",
            table: "Order",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<string>(
            name: "Description",
            table: "Genres",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<byte[]>(
            name: "Picture",
            table: "Genres",
            type: "varbinary(max)",
            nullable: false,
            defaultValue: Array.Empty<byte>());

        migrationBuilder.AlterColumn<float>(
            name: "Discount",
            table: "Games",
            type: "real",
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AddColumn<string>(
            name: "QuantityPerUnit",
            table: "Games",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<int>(
            name: "ReorderLevel",
            table: "Games",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "UnitOnOrder",
            table: "Games",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateTable(
            name: "PaymentMethods",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PaymentMethods", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PaymentMethods");

        migrationBuilder.DropColumn(
            name: "Address",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "City",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "ContactName",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "ContactTitle",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "Country",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "Fax",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "Phone",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "PostalCode",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "Region",
            table: "Publishers");

        migrationBuilder.DropColumn(
            name: "EmployeeId",
            table: "Order");

        migrationBuilder.DropColumn(
            name: "Freight",
            table: "Order");

        migrationBuilder.DropColumn(
            name: "RequiredDate",
            table: "Order");

        migrationBuilder.DropColumn(
            name: "ShipAddress",
            table: "Order");

        migrationBuilder.DropColumn(
            name: "ShipCity",
            table: "Order");

        migrationBuilder.DropColumn(
            name: "ShipCountry",
            table: "Order");

        migrationBuilder.DropColumn(
            name: "ShipName",
            table: "Order");

        migrationBuilder.DropColumn(
            name: "ShipPostalCode",
            table: "Order");

        migrationBuilder.DropColumn(
            name: "ShipRegion",
            table: "Order");

        migrationBuilder.DropColumn(
            name: "ShipVia",
            table: "Order");

        migrationBuilder.DropColumn(
            name: "ShippedVia",
            table: "Order");

        migrationBuilder.DropColumn(
            name: "Description",
            table: "Genres");

        migrationBuilder.DropColumn(
            name: "Picture",
            table: "Genres");

        migrationBuilder.DropColumn(
            name: "QuantityPerUnit",
            table: "Games");

        migrationBuilder.DropColumn(
            name: "ReorderLevel",
            table: "Games");

        migrationBuilder.DropColumn(
            name: "UnitOnOrder",
            table: "Games");

        migrationBuilder.AlterColumn<int>(
            name: "Discount",
            table: "OrdersGames",
            type: "int",
            nullable: false,
            oldClrType: typeof(float),
            oldType: "real");

        migrationBuilder.AlterColumn<int>(
            name: "Discount",
            table: "Games",
            type: "int",
            nullable: false,
            oldClrType: typeof(float),
            oldType: "real");
    }
}
