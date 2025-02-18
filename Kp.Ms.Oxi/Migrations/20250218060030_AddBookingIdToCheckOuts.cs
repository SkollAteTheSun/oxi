using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kp.Ms.Oxi.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingIdToCheckOuts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "external_id",
                table: "check_out",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "booking_id",
                table: "check_out",
                type: "text",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "ix_check_out_hotel_code_pms_id",
                table: "check_out",
                columns: new[] { "hotel_code", "pms_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_check_out_hotel_code_pms_id",
                table: "check_out");

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

            migrationBuilder.DropColumn(
                name: "booking_id",
                table: "check_out");

            migrationBuilder.AlterColumn<string>(
                name: "external_id",
                table: "check_out",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

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
    }
}
