using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kp.Ms.Oxi.Migrations
{
    /// <inheritdoc />
    public partial class AddBogatyrHotel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("3cc43174-9a92-4765-84cc-62a760eaa91e"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("4af397f8-ade2-472c-bade-1e08dc05e6df"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("60355e46-5a2d-4772-ac1b-fa014403893e"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("8304dc32-6580-44e2-80eb-8b4cb168051f"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("94d2d5e0-c6ac-4756-9b47-e335173422a5"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("a8645454-a521-46e8-a740-0c0d3f3da685"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("aea5d99c-8ff1-448a-baf6-0a2efe36f127"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("d10f490d-a62b-4396-8d10-b41addbe035b"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("d4fde7b7-cb39-4ab3-b207-dcfb028b4919"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("f8722c54-e89e-47e6-be47-a2767a953f46"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "hotel",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { new Guid("3cc43174-9a92-4765-84cc-62a760eaa91e"), "SCODL", "Долина 960" },
                    { new Guid("4af397f8-ade2-472c-bade-1e08dc05e6df"), "HB769", "Новотель Фит Красная Поляна" },
                    { new Guid("60355e46-5a2d-4772-ac1b-fa014403893e"), "HB755", "Мовенпик Красная Поляна" },
                    { new Guid("8304dc32-6580-44e2-80eb-8b4cb168051f"), "HB933", "Панорама by Mercure Красная Поляна" },
                    { new Guid("94d2d5e0-c6ac-4756-9b47-e335173422a5"), "HB754", "Ибис Стайлс Красная Поляна" },
                    { new Guid("a8645454-a521-46e8-a740-0c0d3f3da685"), "AERCS", "Кортъярд Марриотт Сочи Красная Поляна" },
                    { new Guid("aea5d99c-8ff1-448a-baf6-0a2efe36f127"), "RIXOS", "Риксос Красная Поляна Сочи" },
                    { new Guid("d10f490d-a62b-4396-8d10-b41addbe035b"), "AERMC", "Сочи Марриотт Красная Поляна" },
                    { new Guid("d4fde7b7-cb39-4ab3-b207-dcfb028b4919"), "HB530", "Новотель Резорт и спа Красная Поляна" },
                    { new Guid("f8722c54-e89e-47e6-be47-a2767a953f46"), "APART", "Апартаменты Курорта Красная Поляна" }
                });
        }
    }
}
