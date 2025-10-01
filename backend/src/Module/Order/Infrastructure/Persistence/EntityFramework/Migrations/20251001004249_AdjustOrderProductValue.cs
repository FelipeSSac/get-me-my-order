using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.Infrastructure.Persistence.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AdjustOrderProductValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "unit_price",
                table: "order_products",
                newName: "unit_price_amount");

            migrationBuilder.AddColumn<string>(
                name: "unit_price_currency",
                table: "order_products",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unit_price_currency",
                table: "order_products");

            migrationBuilder.RenameColumn(
                name: "unit_price_amount",
                table: "order_products",
                newName: "unit_price");
        }
    }
}
