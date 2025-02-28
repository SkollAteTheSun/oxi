using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kp.Ms.Oxi.Migrations.Booking
{
    /// <inheritdoc />
    public partial class AddAsuReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reservations_asu",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    number = table.Column<string>(type: "varchar(255)", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    hotel_id = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                    room_type_id = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                    rate_id = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                    guest_id = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                    room_quantity = table.Column<byte>(type: "tinyint(3) unsigned", nullable: true),
                    total = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                    date_in = table.Column<DateOnly>(type: "date", nullable: true),
                    date_out = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "enum('new','approved','paid','cancelled','closed','refund')", nullable: true, defaultValueSql: "'new'", collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    data = table.Column<string>(type: "json", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    comment_manager = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    payment_method_id = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "'1'"),
                    order_number = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    adults = table.Column<sbyte>(type: "tinyint(4)", nullable: true, defaultValueSql: "'0'"),
                    children = table.Column<sbyte>(type: "tinyint(4)", nullable: true, defaultValueSql: "'0'"),
                    babies = table.Column<sbyte>(type: "tinyint(4)", nullable: true, defaultValueSql: "'0'"),
                    infant = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    extra_bed = table.Column<sbyte>(type: "tinyint(4)", nullable: true, defaultValueSql: "'0'"),
                    craddle = table.Column<sbyte>(type: "tinyint(4)", nullable: true, defaultValueSql: "'0'"),
                    is_check_out = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    check_out_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    mindbox_points_date = table.Column<DateOnly>(type: "date", nullable: true),
                    promo_id = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true),
                    is_changed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    is_change_processed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    changed_by = table.Column<int>(type: "int(11)", nullable: true),
                    price_changing = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "'0'"),
                    promo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    room_rates = table.Column<string>(type: "text", nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    has_rate_own_quotas = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    points_used = table.Column<ulong>(type: "bigint(20) unsigned", nullable: true, defaultValueSql: "'0'"),
                    set_points = table.Column<int>(type: "int(11)", nullable: true),
                    guaranteed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    need_link = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    combined_rates = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    order_source = table.Column<string>(type: "enum('oob','mobile','other','site_mantera','oob_mantera','mobile_mantera')", nullable: true, defaultValueSql: "'other'", collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_cancelled = table.Column<DateTime>(type: "datetime", nullable: true),
                    date_guaranteed = table.Column<DateTime>(type: "datetime", nullable: true),
                    external_id = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, defaultValueSql: "''", collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dont_send_pms = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'"),
                    status_loyalty = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, collation: "utf8mb4_unicode_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.CreateIndex(
                name: "ix_reservations_asu_date_in",
                table: "reservations_asu",
                column: "date_in");

            migrationBuilder.CreateIndex(
                name: "ix_reservations_asu_date_out",
                table: "reservations_asu",
                column: "date_out");

            migrationBuilder.CreateIndex(
                name: "ix_reservations_asu_guest_id",
                table: "reservations_asu",
                column: "guest_id");

            migrationBuilder.CreateIndex(
                name: "ix_reservations_asu_hotel_id",
                table: "reservations_asu",
                column: "hotel_id");

            migrationBuilder.CreateIndex(
                name: "ix_reservations_asu_hotel_id_external_id",
                table: "reservations_asu",
                columns: new[] { "hotel_id", "external_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_reservations_asu_number",
                table: "reservations_asu",
                column: "number");

            migrationBuilder.CreateIndex(
                name: "ix_reservations_asu_order_source",
                table: "reservations_asu",
                column: "order_source");

            migrationBuilder.CreateIndex(
                name: "ix_reservations_asu_payment_method_id",
                table: "reservations_asu",
                column: "payment_method_id");

            migrationBuilder.CreateIndex(
                name: "ix_reservations_asu_rate_id",
                table: "reservations_asu",
                column: "rate_id");

            migrationBuilder.CreateIndex(
                name: "ix_reservations_asu_room_type_id",
                table: "reservations_asu",
                column: "room_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_reservations_asu_room_type_id_date_in_date_out_status",
                table: "reservations_asu",
                columns: new[] { "room_type_id", "date_in", "date_out", "status" });

            migrationBuilder.CreateIndex(
                name: "ix_reservations_asu_room_type_id_date_in_date_out_status_rate_id",
                table: "reservations_asu",
                columns: new[] { "room_type_id", "date_in", "date_out", "status", "rate_id" });

            migrationBuilder.CreateIndex(
                name: "ix_reservations_asu_status",
                table: "reservations_asu",
                column: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservations_asu");
        }
    }
}
