using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kp.Ms.Oxi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCheckOutIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_check_out_external_id",
                table: "check_out");

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

            migrationBuilder.InsertData(
                table: "hotel",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { new Guid("22dd8978-a194-4324-a8ca-b24309f62868"), "HB755", "Мовенпик Красная Поляна" },
                    { new Guid("24071a8f-0f2c-4ac1-83d9-d0ddd98faf1f"), "HB530", "Новотель Резорт и спа Красная Поляна" },
                    { new Guid("5800a575-4811-430b-a413-a1f8f35134d6"), "HB754", "Ибис Стайлс Красная Поляна" },
                    { new Guid("a70bb331-2265-4add-8c4c-c8243cabe74b"), "HB769", "Новотель Фит Красная Поляна" },
                    { new Guid("b7a41c45-72fd-412b-91f0-ff8df06b78b0"), "AERMC", "Сочи Марриотт Красная Поляна" },
                    { new Guid("cb76665a-4056-4450-b1ea-a8d94fbd46ad"), "HB933", "Панорама by Mercure Красная Поляна" },
                    { new Guid("d71b5f5f-4fad-4a40-ad77-3385befd914f"), "APART", "Апартаменты Курорта Красная Поляна" },
                    { new Guid("e1082b10-803f-4941-ba29-949d9d227b78"), "RIXOS", "Риксос Красная Поляна Сочи" },
                    { new Guid("e22b71c4-dc97-4cd0-92bd-18bbf13af536"), "SCODL", "Долина 960" },
                    { new Guid("e6bf720e-2178-48af-99a1-65eb16d76967"), "AERCS", "Кортъярд Марриотт Сочи Красная Поляна" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_check_out_hotel_code_external_id",
                table: "check_out",
                columns: new[] { "hotel_code", "external_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_check_out_hotel_code_external_id",
                table: "check_out");

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("22dd8978-a194-4324-a8ca-b24309f62868"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("24071a8f-0f2c-4ac1-83d9-d0ddd98faf1f"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("5800a575-4811-430b-a413-a1f8f35134d6"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("a70bb331-2265-4add-8c4c-c8243cabe74b"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("b7a41c45-72fd-412b-91f0-ff8df06b78b0"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("cb76665a-4056-4450-b1ea-a8d94fbd46ad"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("d71b5f5f-4fad-4a40-ad77-3385befd914f"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("e1082b10-803f-4941-ba29-949d9d227b78"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("e22b71c4-dc97-4cd0-92bd-18bbf13af536"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("e6bf720e-2178-48af-99a1-65eb16d76967"));

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

            migrationBuilder.CreateIndex(
                name: "ix_check_out_external_id",
                table: "check_out",
                column: "external_id",
                unique: true);
        }
    }
}
