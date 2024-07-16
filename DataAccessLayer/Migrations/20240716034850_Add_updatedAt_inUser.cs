using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.migrations
{
    /// <inheritdoc />
    public partial class Add_updatedAt_inUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 10, 48, 50, 159, DateTimeKind.Local).AddTicks(2067),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 9, 15, 43, 13, 437, DateTimeKind.Local).AddTicks(3416));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "PaperWorks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 16, 10, 48, 50, 158, DateTimeKind.Local).AddTicks(3180),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 9, 15, 43, 13, 436, DateTimeKind.Local).AddTicks(1139));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Diamonds",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 16, 10, 48, 50, 157, DateTimeKind.Local).AddTicks(7758),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 9, 15, 43, 13, 435, DateTimeKind.Local).AddTicks(2996));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 9, 15, 43, 13, 437, DateTimeKind.Local).AddTicks(3416),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 10, 48, 50, 159, DateTimeKind.Local).AddTicks(2067));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Promotions",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "PaperWorks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 9, 15, 43, 13, 436, DateTimeKind.Local).AddTicks(1139),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 16, 10, 48, 50, 158, DateTimeKind.Local).AddTicks(3180));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Diamonds",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 9, 15, 43, 13, 435, DateTimeKind.Local).AddTicks(2996),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 16, 10, 48, 50, 157, DateTimeKind.Local).AddTicks(7758));
        }
    }
}
