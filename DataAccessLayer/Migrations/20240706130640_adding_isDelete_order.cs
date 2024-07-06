using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.migrations
{
    /// <inheritdoc />
    public partial class adding_isDelete_order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 6, 20, 6, 39, 340, DateTimeKind.Local).AddTicks(5981),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 27, 20, 6, 14, 937, DateTimeKind.Local).AddTicks(3567));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "PaperWorks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 6, 20, 6, 39, 339, DateTimeKind.Local).AddTicks(5510),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 27, 20, 6, 14, 936, DateTimeKind.Local).AddTicks(6743));

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Orders",
                type: "bit",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "OrderDetails",
                type: "bit",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Diamonds",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2024, 7, 6, 20, 6, 39, 339, DateTimeKind.Local).AddTicks(189),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 27, 20, 6, 14, 936, DateTimeKind.Local).AddTicks(2807));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 27, 20, 6, 14, 937, DateTimeKind.Local).AddTicks(3567),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 6, 20, 6, 39, 340, DateTimeKind.Local).AddTicks(5981));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "PaperWorks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 27, 20, 6, 14, 936, DateTimeKind.Local).AddTicks(6743),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 6, 20, 6, 39, 339, DateTimeKind.Local).AddTicks(5510));

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Orders",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "OrderDetails",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Diamonds",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 27, 20, 6, 14, 936, DateTimeKind.Local).AddTicks(2807),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 7, 6, 20, 6, 39, 339, DateTimeKind.Local).AddTicks(189));
        }
    }
}
