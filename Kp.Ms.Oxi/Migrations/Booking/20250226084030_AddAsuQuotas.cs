using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kp.Ms.Oxi.Migrations.Booking
{
    /// <inheritdoc />
    public partial class AddAsuQuotas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                 name: "amount_asu",
                 table: "room_quotas",
                 type: "BIGINT",
                 nullable: false,
                 defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                 name: "reserved_asu",
                 table: "room_quotas",
                 type: "BIGINT",
                 nullable: false,
                 defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "amount_asu",
                table: "room_quotas");
            migrationBuilder.DropColumn(
                name: "reserved_asu",
                table: "room_quotas");
        }
    }
}
