using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kp.Ms.Oxi.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexHotelCodeAndInterfaceName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "pre",
                table: "transaction",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.CreateIndex(
                name: "ix_transaction_hotel_code_interface_name",
                table: "transaction",
                columns: new[] { "hotel_code", "interface_name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_transaction_hotel_code_interface_name",
                table: "transaction");

            migrationBuilder.AlterColumn<bool>(
                name: "pre",
                table: "transaction",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);
        }
    }
}
