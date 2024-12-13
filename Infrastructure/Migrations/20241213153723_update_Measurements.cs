using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_Measurements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                oldDefaultValue: new DateTime(2024, 12, 13, 22, 7, 32, 945, DateTimeKind.Local).AddTicks(6737));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Roles",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 12, 13, 22, 7, 32, 945, DateTimeKind.Local).AddTicks(6737),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 12, 13, 22, 37, 22, 667, DateTimeKind.Local).AddTicks(4690));
        }
    }
}
