using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateusercustomer_addtransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserCustomers_UserID",
                table: "UserCustomers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Roles",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 12, 10, 9, 1, 56, 975, DateTimeKind.Local).AddTicks(3744),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 12, 3, 13, 41, 17, 788, DateTimeKind.Local).AddTicks(9495));

            migrationBuilder.CreateIndex(
                name: "IX_UserCustomers_UserID_CustomerID",
                table: "UserCustomers",
                columns: new[] { "UserID", "CustomerID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserCustomers_UserID_CustomerID",
                table: "UserCustomers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Roles",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 12, 3, 13, 41, 17, 788, DateTimeKind.Local).AddTicks(9495),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 12, 10, 9, 1, 56, 975, DateTimeKind.Local).AddTicks(3744));

            migrationBuilder.CreateIndex(
                name: "IX_UserCustomers_UserID",
                table: "UserCustomers",
                column: "UserID");
        }
    }
}
