using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Change_Discount_To_DiscountRate_Promotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Promotions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 9, 15, 19, 46, 110, DateTimeKind.Local).AddTicks(2338),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 8, 12, 52, 2, 843, DateTimeKind.Local).AddTicks(5113));

            migrationBuilder.AddColumn<double>(
                name: "DiscountRate",
                table: "Promotions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "PaperWorks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 9, 15, 19, 46, 109, DateTimeKind.Local).AddTicks(3341),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 8, 12, 52, 2, 842, DateTimeKind.Local).AddTicks(3163));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Diamonds",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 9, 15, 19, 46, 108, DateTimeKind.Local).AddTicks(6235),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 8, 12, 52, 2, 841, DateTimeKind.Local).AddTicks(4115));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountRate",
                table: "Promotions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 8, 12, 52, 2, 843, DateTimeKind.Local).AddTicks(5113),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 9, 15, 19, 46, 110, DateTimeKind.Local).AddTicks(2338));

            migrationBuilder.AddColumn<string>(
                name: "Discount",
                table: "Promotions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "PaperWorks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 8, 12, 52, 2, 842, DateTimeKind.Local).AddTicks(3163),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 9, 15, 19, 46, 109, DateTimeKind.Local).AddTicks(3341));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Diamonds",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 8, 12, 52, 2, 841, DateTimeKind.Local).AddTicks(4115),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 9, 15, 19, 46, 108, DateTimeKind.Local).AddTicks(6235));
        }
    }
}
