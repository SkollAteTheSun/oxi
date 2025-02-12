using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kp.Ms.Oxi.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckOutAndHotelTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "check_out",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pms_id = table.Column<string>(type: "text", nullable: false),
                    external_id = table.Column<string>(type: "text", nullable: false),
                    check_out_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    hotel_code = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_check_out", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hotel",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hotel", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_check_out_external_id",
                table: "check_out",
                column: "external_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_hotel_code",
                table: "hotel",
                column: "code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "check_out");

            migrationBuilder.DropTable(
                name: "hotel");
        }
    }
}
