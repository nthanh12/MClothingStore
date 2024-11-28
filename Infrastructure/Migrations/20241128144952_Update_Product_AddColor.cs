using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Product_AddColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Roles",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 11, 28, 21, 49, 51, 789, DateTimeKind.Local).AddTicks(3141),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 11, 28, 20, 33, 37, 19, DateTimeKind.Local).AddTicks(2983));

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Products");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Roles",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 11, 28, 20, 33, 37, 19, DateTimeKind.Local).AddTicks(2983),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 11, 28, 21, 49, 51, 789, DateTimeKind.Local).AddTicks(3141));

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
