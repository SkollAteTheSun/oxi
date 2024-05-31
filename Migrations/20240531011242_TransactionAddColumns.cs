using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kp.Ms.Oxi.Migrations
{
    /// <inheritdoc />
    public partial class TransactionAddColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "sended",
                table: "transaction",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "url",
                table: "transaction",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sended",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "url",
                table: "transaction");
        }
    }
}
