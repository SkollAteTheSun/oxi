using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kp.Ms.Oxi.Migrations
{
    /// <inheritdoc />
    public partial class AddTransationDeliveredFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("16742ceb-8714-4c32-9c8b-7998b24d9aae"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("1a1635f2-f8fe-41d7-b138-d1f8ca75aa41"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("59e50cc0-9f76-4a5f-b6c7-79334f59a7d9"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("6910b197-7ed8-467c-ac02-17bdde61e193"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("ab68de50-f9cd-45f3-9306-9de69362e361"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("b4ad5ec4-c25a-40ea-88ce-c037ffec3e03"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("d320e8f2-cb3f-4bfb-a73f-0709b804d48d"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("d46b4d41-fb17-4331-ba5e-91c980b30721"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("e0cb3488-44b8-4d21-b137-f526ce287b10"));

            migrationBuilder.DeleteData(
                table: "hotel",
                keyColumn: "id",
                keyValue: new Guid("ff700dc8-0e0f-41e0-9a4d-746b25296d11"));

            migrationBuilder.AddColumn<bool>(
                name: "delivered",
                table: "transaction",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "delivered_at",
                table: "transaction",
                type: "timestamp with time zone",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "delivered",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "delivered_at",
                table: "transaction");

            migrationBuilder.InsertData(
                table: "hotel",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { new Guid("16742ceb-8714-4c32-9c8b-7998b24d9aae"), "HB530", "Новотель Резорт и спа Красная Поляна" },
                    { new Guid("1a1635f2-f8fe-41d7-b138-d1f8ca75aa41"), "APART", "Апартаменты Курорта Красная Поляна" },
                    { new Guid("59e50cc0-9f76-4a5f-b6c7-79334f59a7d9"), "RIXOS", "Риксос Красная Поляна Сочи" },
                    { new Guid("6910b197-7ed8-467c-ac02-17bdde61e193"), "HB754", "Ибис Стайлс Красная Поляна" },
                    { new Guid("ab68de50-f9cd-45f3-9306-9de69362e361"), "AERCS", "Кортъярд Марриотт Сочи Красная Поляна" },
                    { new Guid("b4ad5ec4-c25a-40ea-88ce-c037ffec3e03"), "AERMC", "Сочи Марриотт Красная Поляна" },
                    { new Guid("d320e8f2-cb3f-4bfb-a73f-0709b804d48d"), "HB933", "Панорама by Mercure Красная Поляна" },
                    { new Guid("d46b4d41-fb17-4331-ba5e-91c980b30721"), "SCODL", "Долина 960" },
                    { new Guid("e0cb3488-44b8-4d21-b137-f526ce287b10"), "HB755", "Мовенпик Красная Поляна" },
                    { new Guid("ff700dc8-0e0f-41e0-9a4d-746b25296d11"), "HB769", "Новотель Фит Красная Поляна" }
                });
        }
    }
}
