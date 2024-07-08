using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class diamond_accessory_isDelete_DefaultValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 6, 20, 55, 45, 49, DateTimeKind.Local).AddTicks(5960),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 6, 20, 6, 39, 340, DateTimeKind.Local).AddTicks(5981));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "PaperWorks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 6, 20, 55, 45, 48, DateTimeKind.Local).AddTicks(9328),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 6, 20, 6, 39, 339, DateTimeKind.Local).AddTicks(5510));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Diamonds",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 6, 20, 55, 45, 48, DateTimeKind.Local).AddTicks(5229),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 6, 20, 6, 39, 339, DateTimeKind.Local).AddTicks(189));

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DiamondAccessories",
                type: "bit",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 6, 20, 6, 39, 340, DateTimeKind.Local).AddTicks(5981),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 6, 20, 55, 45, 49, DateTimeKind.Local).AddTicks(5960));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "PaperWorks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 6, 20, 6, 39, 339, DateTimeKind.Local).AddTicks(5510),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 6, 20, 55, 45, 48, DateTimeKind.Local).AddTicks(9328));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Diamonds",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 6, 20, 6, 39, 339, DateTimeKind.Local).AddTicks(189),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 6, 20, 55, 45, 48, DateTimeKind.Local).AddTicks(5229));

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "DiamondAccessories",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);
        }
    }
}
