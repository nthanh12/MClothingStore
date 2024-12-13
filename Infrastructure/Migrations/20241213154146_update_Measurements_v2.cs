using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_Measurements_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Measurements",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Roles",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 12, 13, 22, 41, 45, 412, DateTimeKind.Local).AddTicks(3256),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 12, 13, 22, 37, 22, 667, DateTimeKind.Local).AddTicks(4690));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Roles",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 12, 13, 22, 37, 22, 667, DateTimeKind.Local).AddTicks(4690),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 12, 13, 22, 41, 45, 412, DateTimeKind.Local).AddTicks(3256));

            migrationBuilder.AddColumn<string>(
                name: "Measurements",
                table: "OrderDetails",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
