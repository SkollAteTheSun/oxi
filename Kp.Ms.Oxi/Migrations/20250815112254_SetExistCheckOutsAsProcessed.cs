using Microsoft.EntityFrameworkCore.Migrations;
using static System.Runtime.InteropServices.JavaScript.JSType;

#nullable disable

namespace Kp.Ms.Oxi.Migrations
{
    /// <inheritdoc />
    public partial class SetExistCheckOutsAsProcessed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var date = DateTime.UtcNow.AddMonths(-2);
            migrationBuilder.Sql($"UPDATE check_out SET processed = true WHERE check_out_date < date('{date.ToString("yyyy-MM-dd")}')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE check_out SET processed = false");
        }
    }
}
