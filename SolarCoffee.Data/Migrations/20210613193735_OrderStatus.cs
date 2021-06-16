using Microsoft.EntityFrameworkCore.Migrations;

namespace SolarCoffee.Data.Migrations
{
    public partial class OrderStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "SalesOrders");

            migrationBuilder.AddColumn<int>(
                name: "OrderStatus",
                table: "SalesOrders",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "SalesOrders");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "SalesOrders",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
