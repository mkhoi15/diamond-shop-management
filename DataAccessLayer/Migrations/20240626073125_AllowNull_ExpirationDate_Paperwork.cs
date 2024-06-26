using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.migrations
{
    /// <inheritdoc />
    public partial class AllowNull_ExpirationDate_Paperwork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 26, 14, 31, 24, 457, DateTimeKind.Local).AddTicks(2573),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 26, 12, 48, 27, 779, DateTimeKind.Local).AddTicks(2672));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "PaperWorks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "PaperWorks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 26, 14, 31, 24, 456, DateTimeKind.Local).AddTicks(1364),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 26, 12, 48, 27, 778, DateTimeKind.Local).AddTicks(6002));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Diamonds",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 26, 14, 31, 24, 455, DateTimeKind.Local).AddTicks(2817),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 26, 12, 48, 27, 778, DateTimeKind.Local).AddTicks(2031));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 26, 12, 48, 27, 779, DateTimeKind.Local).AddTicks(2672),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 26, 14, 31, 24, 457, DateTimeKind.Local).AddTicks(2573));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "PaperWorks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "PaperWorks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 26, 12, 48, 27, 778, DateTimeKind.Local).AddTicks(6002),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 26, 14, 31, 24, 456, DateTimeKind.Local).AddTicks(1364));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Diamonds",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 26, 12, 48, 27, 778, DateTimeKind.Local).AddTicks(2031),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 26, 14, 31, 24, 455, DateTimeKind.Local).AddTicks(2817));
        }
    }
}
