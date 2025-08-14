using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kp.Ms.Oxi.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckOutStatusEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("0d837a0c-1194-496e-8441-e0e358195aea"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("27f5dd1c-e184-4e7d-85aa-8729b11ca15b"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("4b511975-cef2-4e39-a3ba-95469a450591"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("61b40a77-a376-4532-89ea-b3d5fb8d4179"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("a07c1481-7241-4463-97e8-ba883b808b1b"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("a856c274-dafa-40e1-aecd-954498e546b9"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("ae353229-2164-4f92-a84b-8d7cde48bbd7"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("c2dcc2a2-5687-41ac-a575-d8ef011f25d5"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("c6e75554-3536-41ad-b18d-b78ec2d9dcc7"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("c7bb375d-7b1a-4400-9bf2-1587154240d1"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("e156c5b5-73c9-4514-aefd-aea12befea47"));

            migrationBuilder.AddColumn<bool>(
                name: "processed",
                table: "check_out",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "check_out_status",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    booking_id = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_check_out_status", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "hotel",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { new Guid("073f7916-66ed-406e-82e6-48a09db25190"), "AERCS", "Кортъярд Марриотт Сочи Красная Поляна" },
                    { new Guid("23e771c2-5515-41cc-87fc-962fa0496b65"), "AERMC", "Сочи Марриотт Красная Поляна" },
                    { new Guid("4de0f494-9c82-4702-8f1c-dbfa6543bb47"), "SCODL", "Долина 960" },
                    { new Guid("526fdd1e-595d-4379-b04d-797c92e0b2a0"), "BOGATYR", "Отель-замок Богатырь" },
                    { new Guid("7cdfb7d5-9bd0-4e74-a2a4-f43844f53179"), "APART", "Апартаменты Курорта Красная Поляна" },
                    { new Guid("8693c019-76e4-4380-9b24-ee2c71e97290"), "RIXOS", "Риксос Красная Поляна Сочи" },
                    { new Guid("9bc381f2-999d-4812-bbe5-753e1494dd6e"), "HB755", "Мовенпик Красная Поляна" },
                    { new Guid("b11a13f3-78d9-47a6-9174-090735f87dad"), "HB530", "Новотель Резорт и спа Красная Поляна" },
                    { new Guid("b89aef32-3697-472d-b668-72275605ef24"), "HB769", "Новотель Фит Красная Поляна" },
                    { new Guid("b9de80f5-2710-4e4e-8c9b-c7ecd8279dcf"), "HB933", "Панорама by Mercure Красная Поляна" },
                    { new Guid("f43fbefb-2176-41c1-90f3-8971adf746ee"), "HB754", "Ибис Стайлс Красная Поляна" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_check_out_status_booking_id",
                table: "check_out_status",
                column: "booking_id");

            migrationBuilder.CreateIndex(
                name: "ix_check_out_status_booking_id_status",
                table: "check_out_status",
                columns: new[] { "booking_id", "status" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "check_out_status");

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("073f7916-66ed-406e-82e6-48a09db25190"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("23e771c2-5515-41cc-87fc-962fa0496b65"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("4de0f494-9c82-4702-8f1c-dbfa6543bb47"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("526fdd1e-595d-4379-b04d-797c92e0b2a0"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("7cdfb7d5-9bd0-4e74-a2a4-f43844f53179"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("8693c019-76e4-4380-9b24-ee2c71e97290"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("9bc381f2-999d-4812-bbe5-753e1494dd6e"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("b11a13f3-78d9-47a6-9174-090735f87dad"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("b89aef32-3697-472d-b668-72275605ef24"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("b9de80f5-2710-4e4e-8c9b-c7ecd8279dcf"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("f43fbefb-2176-41c1-90f3-8971adf746ee"));

            migrationBuilder.DropColumn(
                name: "processed",
                table: "check_out");

            migrationBuilder.InsertData(
                table: "hotel",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { new Guid("0d837a0c-1194-496e-8441-e0e358195aea"), "AERMC", "Сочи Марриотт Красная Поляна" },
                    { new Guid("27f5dd1c-e184-4e7d-85aa-8729b11ca15b"), "HB755", "Мовенпик Красная Поляна" },
                    { new Guid("4b511975-cef2-4e39-a3ba-95469a450591"), "BOGATYR", "Отель-замок Богатырь" },
                    { new Guid("61b40a77-a376-4532-89ea-b3d5fb8d4179"), "SCODL", "Долина 960" },
                    { new Guid("a07c1481-7241-4463-97e8-ba883b808b1b"), "HB933", "Панорама by Mercure Красная Поляна" },
                    { new Guid("a856c274-dafa-40e1-aecd-954498e546b9"), "AERCS", "Кортъярд Марриотт Сочи Красная Поляна" },
                    { new Guid("ae353229-2164-4f92-a84b-8d7cde48bbd7"), "HB754", "Ибис Стайлс Красная Поляна" },
                    { new Guid("c2dcc2a2-5687-41ac-a575-d8ef011f25d5"), "HB769", "Новотель Фит Красная Поляна" },
                    { new Guid("c6e75554-3536-41ad-b18d-b78ec2d9dcc7"), "RIXOS", "Риксос Красная Поляна Сочи" },
                    { new Guid("c7bb375d-7b1a-4400-9bf2-1587154240d1"), "HB530", "Новотель Резорт и спа Красная Поляна" },
                    { new Guid("e156c5b5-73c9-4514-aefd-aea12befea47"), "APART", "Апартаменты Курорта Красная Поляна" }
                });
        }
    }
}
