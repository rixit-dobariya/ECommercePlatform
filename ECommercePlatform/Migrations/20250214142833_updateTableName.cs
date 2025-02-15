using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommercePlatform.Migrations
{
    /// <inheritdoc />
    public partial class updateTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_orderHeaders_OrderHeaderId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_orderHeaders_Addresses_BillingAddressId",
                table: "orderHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_orderHeaders_Addresses_ShippingAddressId",
                table: "orderHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_orderHeaders_Users_UserId",
                table: "orderHeaders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_orderHeaders",
                table: "orderHeaders");

            migrationBuilder.RenameTable(
                name: "orderHeaders",
                newName: "OrderHeaders");

            migrationBuilder.RenameIndex(
                name: "IX_orderHeaders_UserId",
                table: "OrderHeaders",
                newName: "IX_OrderHeaders_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_orderHeaders_ShippingAddressId",
                table: "OrderHeaders",
                newName: "IX_OrderHeaders_ShippingAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_orderHeaders_BillingAddressId",
                table: "OrderHeaders",
                newName: "IX_OrderHeaders_BillingAddressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderHeaders",
                table: "OrderHeaders",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                table: "OrderDetails",
                column: "OrderHeaderId",
                principalTable: "OrderHeaders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_Addresses_BillingAddressId",
                table: "OrderHeaders",
                column: "BillingAddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_Addresses_ShippingAddressId",
                table: "OrderHeaders",
                column: "ShippingAddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_Users_UserId",
                table: "OrderHeaders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_Addresses_BillingAddressId",
                table: "OrderHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_Addresses_ShippingAddressId",
                table: "OrderHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_Users_UserId",
                table: "OrderHeaders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderHeaders",
                table: "OrderHeaders");

            migrationBuilder.RenameTable(
                name: "OrderHeaders",
                newName: "orderHeaders");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHeaders_UserId",
                table: "orderHeaders",
                newName: "IX_orderHeaders_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHeaders_ShippingAddressId",
                table: "orderHeaders",
                newName: "IX_orderHeaders_ShippingAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderHeaders_BillingAddressId",
                table: "orderHeaders",
                newName: "IX_orderHeaders_BillingAddressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_orderHeaders",
                table: "orderHeaders",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_orderHeaders_OrderHeaderId",
                table: "OrderDetails",
                column: "OrderHeaderId",
                principalTable: "orderHeaders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orderHeaders_Addresses_BillingAddressId",
                table: "orderHeaders",
                column: "BillingAddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_orderHeaders_Addresses_ShippingAddressId",
                table: "orderHeaders",
                column: "ShippingAddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_orderHeaders_Users_UserId",
                table: "orderHeaders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
