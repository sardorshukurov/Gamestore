using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamestore.DAL.Migrations;

/// <inheritdoc />
public partial class MakeFiledsNullable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Region",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "PostalCode",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "Phone",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "Fax",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "Country",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "ContactTitle",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "ContactName",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "City",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "Address",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "ShipRegion",
            table: "Order",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "ShipName",
            table: "Order",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "ShipCountry",
            table: "Order",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "ShipCity",
            table: "Order",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.AlterColumn<string>(
            name: "ShipAddress",
            table: "Order",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Region",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "PostalCode",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Phone",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Fax",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Country",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "ContactTitle",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "ContactName",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "City",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Address",
            table: "Publishers",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "ShipRegion",
            table: "Order",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "ShipName",
            table: "Order",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "ShipCountry",
            table: "Order",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "ShipCity",
            table: "Order",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "ShipAddress",
            table: "Order",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: string.Empty,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);
    }
}
