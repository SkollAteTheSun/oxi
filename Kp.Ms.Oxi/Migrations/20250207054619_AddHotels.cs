using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kp.Ms.Oxi.Migrations
{
    /// <inheritdoc />
    public partial class AddHotels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "hotel",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { new Guid("036e9caa-e468-4926-b36b-150bc7b145be"), "HB754", "Ибис Стайлс Красная Поляна" },
                    { new Guid("0fa69c57-d9cd-4632-aad0-76d902eebeda"), "HB769", "Новотель Фит Красная Поляна" },
                    { new Guid("122ca673-51d6-4286-ad96-fa61dac1e378"), "HB530", "Новотель Резорт и спа Красная Поляна" },
                    { new Guid("30c78768-fb1a-41b7-891b-912697bff9d2"), "AERCS", "Кортъярд Марриотт Сочи Красная Поляна" },
                    { new Guid("32ef195f-6fdf-4d2b-be3d-c05b74e64305"), "SCODL", "Долина 960" },
                    { new Guid("420314bf-ed9d-4dff-b911-a599cb2648e1"), "AERMC", "Сочи Марриотт Красная Поляна" },
                    { new Guid("742f36bd-1daa-4c1c-a504-cae36bcfda17"), "HB933", "Панорама by Mercure Красная Поляна" },
                    { new Guid("8b9124dd-de35-4aa3-81fd-0f44640152c7"), "RIXOS", "Риксос Красная Поляна Сочи" },
                    { new Guid("cf64c90a-18ab-4f4e-bd32-fd9c673f7990"), "APART", "Апартаменты Курорта Красная Поляна" },
                    { new Guid("f1382e97-b8a1-4f1c-8062-4669b6e77959"), "HB755", "Мовенпик Красная Поляна" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("036e9caa-e468-4926-b36b-150bc7b145be"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("0fa69c57-d9cd-4632-aad0-76d902eebeda"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("122ca673-51d6-4286-ad96-fa61dac1e378"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("30c78768-fb1a-41b7-891b-912697bff9d2"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("32ef195f-6fdf-4d2b-be3d-c05b74e64305"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("420314bf-ed9d-4dff-b911-a599cb2648e1"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("742f36bd-1daa-4c1c-a504-cae36bcfda17"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("8b9124dd-de35-4aa3-81fd-0f44640152c7"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("cf64c90a-18ab-4f4e-bd32-fd9c673f7990"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("f1382e97-b8a1-4f1c-8062-4669b6e77959"));
        }
    }
}
