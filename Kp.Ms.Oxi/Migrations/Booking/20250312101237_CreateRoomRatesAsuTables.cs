using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kp.Ms.Oxi.Migrations.Booking
{
    /// <inheritdoc />
    public partial class CreateRoomRatesAsuTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "room_rates_1_asu",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hotel_id = table.Column<long>(type: "bigint", nullable: false),
                    room_type_id = table.Column<long>(type: "bigint", nullable: false),
                    rate_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    adults = table.Column<byte>(type: "tinyint", nullable: false),
                    children = table.Column<byte>(type: "tinyint", nullable: false),
                    babies = table.Column<byte>(type: "tinyint", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: 1),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    extra_bed = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    craddle = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    close_sale = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    used = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: 0),
                    base_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    craddle_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    adult_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room_rates_1_asu", x => x.id).Annotation("MySql:IndexType", "BTREE");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .Annotation("MySql:RowFormat", "DYNAMIC");

            migrationBuilder.CreateIndex(
                name: "unique_room_rates_1",
                table: "room_rates_1_asu",
                columns: new[] { "hotel_id", "room_type_id", "rate_id", "adults", "children", "babies", "extra_bed", "craddle", "date" },
                unique: true
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "room_rates_1_adults_IDX",
                table: "room_rates_1_asu",
                columns: new[] { "date", "adults", "children", "babies", "active" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "idx_room_rates_1",
                table: "room_rates_1_asu",
                columns: new[] { "room_type_id", "rate_id", "date", "adults", "children", "babies", "extra_bed", "craddle" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateTable(
                name: "room_rates_2_asu",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hotel_id = table.Column<long>(type: "bigint", nullable: false),
                    room_type_id = table.Column<long>(type: "bigint", nullable: false),
                    rate_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    adults = table.Column<byte>(type: "tinyint", nullable: false),
                    children = table.Column<byte>(type: "tinyint", nullable: false),
                    babies = table.Column<byte>(type: "tinyint", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: 1),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    extra_bed = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    craddle = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    close_sale = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    used = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: 0),
                    base_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    craddle_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    adult_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room_rates_2_asu", x => x.id).Annotation("MySql:IndexType", "BTREE");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .Annotation("MySql:RowFormat", "DYNAMIC");

            migrationBuilder.CreateIndex(
                name: "unique_room_rates_2",
                table: "room_rates_2_asu",
                columns: new[] { "hotel_id", "room_type_id", "rate_id", "adults", "children", "babies", "extra_bed", "craddle", "date" },
                unique: true
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "room_rates_2_adults_IDX",
                table: "room_rates_2_asu",
                columns: new[] { "date", "adults", "children", "babies", "active" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "idx_room_rates_2",
                table: "room_rates_2_asu",
                columns: new[] { "room_type_id", "rate_id", "date", "adults", "children", "babies", "extra_bed", "craddle" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateTable(
                name: "room_rates_3_asu",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hotel_id = table.Column<long>(type: "bigint", nullable: false),
                    room_type_id = table.Column<long>(type: "bigint", nullable: false),
                    rate_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    adults = table.Column<byte>(type: "tinyint", nullable: false),
                    children = table.Column<byte>(type: "tinyint", nullable: false),
                    babies = table.Column<byte>(type: "tinyint", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: 1),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    extra_bed = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    craddle = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    close_sale = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    used = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: 0),
                    base_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    craddle_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    adult_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room_rates_3_asu", x => x.id).Annotation("MySql:IndexType", "BTREE");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .Annotation("MySql:RowFormat", "DYNAMIC");

            migrationBuilder.CreateIndex(
                name: "unique_room_rates_3",
                table: "room_rates_3_asu",
                columns: new[] { "hotel_id", "room_type_id", "rate_id", "adults", "children", "babies", "extra_bed", "craddle", "date" },
                unique: true
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "room_rates_3_adults_IDX",
                table: "room_rates_3_asu",
                columns: new[] { "date", "adults", "children", "babies", "active" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "idx_room_rates_3",
                table: "room_rates_3_asu",
                columns: new[] { "room_type_id", "rate_id", "date", "adults", "children", "babies", "extra_bed", "craddle" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateTable(
                name: "room_rates_4_asu",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hotel_id = table.Column<long>(type: "bigint", nullable: false),
                    room_type_id = table.Column<long>(type: "bigint", nullable: false),
                    rate_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    adults = table.Column<byte>(type: "tinyint", nullable: false),
                    children = table.Column<byte>(type: "tinyint", nullable: false),
                    babies = table.Column<byte>(type: "tinyint", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: 1),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    extra_bed = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    craddle = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    close_sale = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    used = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: 0),
                    base_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    craddle_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    adult_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room_rates_4_asu", x => x.id).Annotation("MySql:IndexType", "BTREE");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .Annotation("MySql:RowFormat", "DYNAMIC");

            migrationBuilder.CreateIndex(
                name: "unique_room_rates_4",
                table: "room_rates_4_asu",
                columns: new[] { "hotel_id", "room_type_id", "rate_id", "adults", "children", "babies", "extra_bed", "craddle", "date" },
                unique: true
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "room_rates_4_adults_IDX",
                table: "room_rates_4_asu",
                columns: new[] { "date", "adults", "children", "babies", "active" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "idx_room_rates_4",
                table: "room_rates_4_asu",
                columns: new[] { "room_type_id", "rate_id", "date", "adults", "children", "babies", "extra_bed", "craddle" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateTable(
                name: "room_rates_5_asu",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hotel_id = table.Column<long>(type: "bigint", nullable: false),
                    room_type_id = table.Column<long>(type: "bigint", nullable: false),
                    rate_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    adults = table.Column<byte>(type: "tinyint", nullable: false),
                    children = table.Column<byte>(type: "tinyint", nullable: false),
                    babies = table.Column<byte>(type: "tinyint", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: 1),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    extra_bed = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    craddle = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    close_sale = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    used = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: 0),
                    base_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    craddle_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    adult_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room_rates_5_asu", x => x.id).Annotation("MySql:IndexType", "BTREE");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .Annotation("MySql:RowFormat", "DYNAMIC");

            migrationBuilder.CreateIndex(
                name: "unique_room_rates_5",
                table: "room_rates_5_asu",
                columns: new[] { "hotel_id", "room_type_id", "rate_id", "adults", "children", "babies", "extra_bed", "craddle", "date" },
                unique: true
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "room_rates_5_adults_IDX",
                table: "room_rates_5_asu",
                columns: new[] { "date", "adults", "children", "babies", "active" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "idx_room_rates_5",
                table: "room_rates_5_asu",
                columns: new[] { "room_type_id", "rate_id", "date", "adults", "children", "babies", "extra_bed", "craddle" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateTable(
                name: "room_rates_6_asu",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hotel_id = table.Column<long>(type: "bigint", nullable: false),
                    room_type_id = table.Column<long>(type: "bigint", nullable: false),
                    rate_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    adults = table.Column<byte>(type: "tinyint", nullable: false),
                    children = table.Column<byte>(type: "tinyint", nullable: false),
                    babies = table.Column<byte>(type: "tinyint", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: 1),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    extra_bed = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    craddle = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    close_sale = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    used = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: 0),
                    base_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    craddle_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    adult_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room_rates_6_asu", x => x.id).Annotation("MySql:IndexType", "BTREE");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .Annotation("MySql:RowFormat", "DYNAMIC");

            migrationBuilder.CreateIndex(
                name: "unique_room_rates_6",
                table: "room_rates_6_asu",
                columns: new[] { "hotel_id", "room_type_id", "rate_id", "adults", "children", "babies", "extra_bed", "craddle", "date" },
                unique: true
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "room_rates_6_adults_IDX",
                table: "room_rates_6_asu",
                columns: new[] { "date", "adults", "children", "babies", "active" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "idx_room_rates_6",
                table: "room_rates_6_asu",
                columns: new[] { "room_type_id", "rate_id", "date", "adults", "children", "babies", "extra_bed", "craddle" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateTable(
                name: "room_rates_7_asu",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hotel_id = table.Column<long>(type: "bigint", nullable: false),
                    room_type_id = table.Column<long>(type: "bigint", nullable: false),
                    rate_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    adults = table.Column<byte>(type: "tinyint", nullable: false),
                    children = table.Column<byte>(type: "tinyint", nullable: false),
                    babies = table.Column<byte>(type: "tinyint", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: 1),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    extra_bed = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    craddle = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    close_sale = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    used = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: 0),
                    base_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    craddle_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    adult_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room_rates_7_asu", x => x.id).Annotation("MySql:IndexType", "BTREE");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .Annotation("MySql:RowFormat", "DYNAMIC");

            migrationBuilder.CreateIndex(
                name: "unique_room_rates_7",
                table: "room_rates_7_asu",
                columns: new[] { "hotel_id", "room_type_id", "rate_id", "adults", "children", "babies", "extra_bed", "craddle", "date" },
                unique: true
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "room_rates_7_adults_IDX",
                table: "room_rates_7_asu",
                columns: new[] { "date", "adults", "children", "babies", "active" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "idx_room_rates_7",
                table: "room_rates_7_asu",
                columns: new[] { "room_type_id", "rate_id", "date", "adults", "children", "babies", "extra_bed", "craddle" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateTable(
                name: "room_rates_8_asu",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hotel_id = table.Column<long>(type: "bigint", nullable: false),
                    room_type_id = table.Column<long>(type: "bigint", nullable: false),
                    rate_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    adults = table.Column<byte>(type: "tinyint", nullable: false),
                    children = table.Column<byte>(type: "tinyint", nullable: false),
                    babies = table.Column<byte>(type: "tinyint", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: 1),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    extra_bed = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    craddle = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    close_sale = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    used = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: 0),
                    base_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    craddle_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    adult_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room_rates_8_asu", x => x.id).Annotation("MySql:IndexType", "BTREE");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .Annotation("MySql:RowFormat", "DYNAMIC");

            migrationBuilder.CreateIndex(
                name: "unique_room_rates_8",
                table: "room_rates_8_asu",
                columns: new[] { "hotel_id", "room_type_id", "rate_id", "adults", "children", "babies", "extra_bed", "craddle", "date" },
                unique: true
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "room_rates_8_adults_IDX",
                table: "room_rates_8_asu",
                columns: new[] { "date", "adults", "children", "babies", "active" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "idx_room_rates_8",
                table: "room_rates_8_asu",
                columns: new[] { "room_type_id", "rate_id", "date", "adults", "children", "babies", "extra_bed", "craddle" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateTable(
                name: "room_rates_9_asu",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hotel_id = table.Column<long>(type: "bigint", nullable: false),
                    room_type_id = table.Column<long>(type: "bigint", nullable: false),
                    rate_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    adults = table.Column<byte>(type: "tinyint", nullable: false),
                    children = table.Column<byte>(type: "tinyint", nullable: false),
                    babies = table.Column<byte>(type: "tinyint", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: 1),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    extra_bed = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    craddle = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    close_sale = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    used = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: 0),
                    base_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    craddle_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    adult_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room_rates_9_asu", x => x.id).Annotation("MySql:IndexType", "BTREE");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .Annotation("MySql:RowFormat", "DYNAMIC");

            migrationBuilder.CreateIndex(
                name: "unique_room_rates_9",
                table: "room_rates_9_asu",
                columns: new[] { "hotel_id", "room_type_id", "rate_id", "adults", "children", "babies", "extra_bed", "craddle", "date" },
                unique: true
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "room_rates_9_adults_IDX",
                table: "room_rates_9_asu",
                columns: new[] { "date", "adults", "children", "babies", "active" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "idx_room_rates_9",
                table: "room_rates_9_asu",
                columns: new[] { "room_type_id", "rate_id", "date", "adults", "children", "babies", "extra_bed", "craddle" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateTable(
                name: "room_rates_10_asu",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hotel_id = table.Column<long>(type: "bigint", nullable: false),
                    room_type_id = table.Column<long>(type: "bigint", nullable: false),
                    rate_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    adults = table.Column<byte>(type: "tinyint", nullable: false),
                    children = table.Column<byte>(type: "tinyint", nullable: false),
                    babies = table.Column<byte>(type: "tinyint", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: 1),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    extra_bed = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    craddle = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    close_sale = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    used = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: 0),
                    base_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    craddle_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    adult_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room_rates_10_asu", x => x.id).Annotation("MySql:IndexType", "BTREE");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .Annotation("MySql:RowFormat", "DYNAMIC");

            migrationBuilder.CreateIndex(
                name: "unique_room_rates_10",
                table: "room_rates_10_asu",
                columns: new[] { "hotel_id", "room_type_id", "rate_id", "adults", "children", "babies", "extra_bed", "craddle", "date" },
                unique: true
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "room_rates_10_adults_IDX",
                table: "room_rates_10_asu",
                columns: new[] { "date", "adults", "children", "babies", "active" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "idx_room_rates_10",
                table: "room_rates_10_asu",
                columns: new[] { "room_type_id", "rate_id", "date", "adults", "children", "babies", "extra_bed", "craddle" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateTable(
                name: "room_rates_11_asu",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hotel_id = table.Column<long>(type: "bigint", nullable: false),
                    room_type_id = table.Column<long>(type: "bigint", nullable: false),
                    rate_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    adults = table.Column<byte>(type: "tinyint", nullable: false),
                    children = table.Column<byte>(type: "tinyint", nullable: false),
                    babies = table.Column<byte>(type: "tinyint", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: 1),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    extra_bed = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    craddle = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    close_sale = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    used = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: 0),
                    base_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    craddle_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    adult_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room_rates_11_asu", x => x.id).Annotation("MySql:IndexType", "BTREE");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .Annotation("MySql:RowFormat", "DYNAMIC");

            migrationBuilder.CreateIndex(
                name: "unique_room_rates_11",
                table: "room_rates_11_asu",
                columns: new[] { "hotel_id", "room_type_id", "rate_id", "adults", "children", "babies", "extra_bed", "craddle", "date" },
                unique: true
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "room_rates_11_adults_IDX",
                table: "room_rates_11_asu",
                columns: new[] { "date", "adults", "children", "babies", "active" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "idx_room_rates_11",
                table: "room_rates_11_asu",
                columns: new[] { "room_type_id", "rate_id", "date", "adults", "children", "babies", "extra_bed", "craddle" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateTable(
                name: "room_rates_12_asu",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    hotel_id = table.Column<long>(type: "bigint", nullable: false),
                    room_type_id = table.Column<long>(type: "bigint", nullable: false),
                    rate_id = table.Column<long>(type: "bigint", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    adults = table.Column<byte>(type: "tinyint", nullable: false),
                    children = table.Column<byte>(type: "tinyint", nullable: false),
                    babies = table.Column<byte>(type: "tinyint", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: 1),
                    amount = table.Column<long>(type: "bigint", nullable: false),
                    extra_bed = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    craddle = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    close_sale = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: 0),
                    used = table.Column<byte>(type: "tinyint", nullable: true, defaultValue: 0),
                    base_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    craddle_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    baby_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    children_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    adult_price_on_beds = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room_rates_12_asu", x => x.id).Annotation("MySql:IndexType", "BTREE");
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("MySql:Collation", "utf8mb4_general_ci")
                .Annotation("MySql:RowFormat", "DYNAMIC");

            migrationBuilder.CreateIndex(
                name: "unique_room_rates_12",
                table: "room_rates_12_asu",
                columns: new[] { "hotel_id", "room_type_id", "rate_id", "adults", "children", "babies", "extra_bed", "craddle", "date" },
                unique: true
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "room_rates_12_adults_IDX",
                table: "room_rates_12_asu",
                columns: new[] { "date", "adults", "children", "babies", "active" }
            ).Annotation("MySql:IndexType", "BTREE");

            migrationBuilder.CreateIndex(
                name: "idx_room_rates_12",
                table: "room_rates_12_asu",
                columns: new[] { "room_type_id", "rate_id", "date", "adults", "children", "babies", "extra_bed", "craddle" }
            ).Annotation("MySql:IndexType", "BTREE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "room_rates_1_asu");
            migrationBuilder.DropTable(name: "room_rates_2_asu");
            migrationBuilder.DropTable(name: "room_rates_3_asu");
            migrationBuilder.DropTable(name: "room_rates_4_asu");
            migrationBuilder.DropTable(name: "room_rates_5_asu");
            migrationBuilder.DropTable(name: "room_rates_6_asu");
            migrationBuilder.DropTable(name: "room_rates_7_asu");
            migrationBuilder.DropTable(name: "room_rates_8_asu");
            migrationBuilder.DropTable(name: "room_rates_9_asu");
            migrationBuilder.DropTable(name: "room_rates_10_asu");
            migrationBuilder.DropTable(name: "room_rates_11_asu");
            migrationBuilder.DropTable(name: "room_rates_12_asu");
        }
    }
}