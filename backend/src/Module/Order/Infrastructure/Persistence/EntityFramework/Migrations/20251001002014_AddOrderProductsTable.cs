using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.Infrastructure.Persistence.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_products_product_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_product_id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "product_id",
                table: "orders");

            migrationBuilder.RenameColumn(
                name: "value_currency",
                table: "orders",
                newName: "total_value_currency");

            migrationBuilder.RenameColumn(
                name: "value_amount",
                table: "orders",
                newName: "total_value_amount");

            migrationBuilder.CreateTable(
                name: "order_products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_products_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_products_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_order_products_order_id_product_id",
                table: "order_products",
                columns: new[] { "order_id", "product_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_order_products_product_id",
                table: "order_products",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_products");

            migrationBuilder.RenameColumn(
                name: "total_value_currency",
                table: "orders",
                newName: "value_currency");

            migrationBuilder.RenameColumn(
                name: "total_value_amount",
                table: "orders",
                newName: "value_amount");

            migrationBuilder.AddColumn<Guid>(
                name: "product_id",
                table: "orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_orders_product_id",
                table: "orders",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_products_product_id",
                table: "orders",
                column: "product_id",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
