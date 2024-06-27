using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.migrations
{
    /// <inheritdoc />
    public partial class Adding_IsDelete_and_CreateAt_in_Diamond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 26, 8, 18, 5, 900, DateTimeKind.Local).AddTicks(5765),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 25, 15, 22, 45, 229, DateTimeKind.Local).AddTicks(4332));

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Diamonds",
                type: "bit",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Diamonds",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 26, 8, 18, 5, 899, DateTimeKind.Local).AddTicks(4578));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Diamonds");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 25, 15, 22, 45, 229, DateTimeKind.Local).AddTicks(4332),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 26, 8, 18, 5, 900, DateTimeKind.Local).AddTicks(5765));

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Diamonds",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);
        }
    }
}
