using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Kp.Ms.Oxi.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionControlFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "xml",
                table: "transaction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Relational:ColumnOrder", 11);

            migrationBuilder.AlterColumn<string>(
                name: "url",
                table: "transaction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "transaction",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone")
                .OldAnnotation("Relational:ColumnOrder", 9);

            migrationBuilder.AlterColumn<string>(
                name: "transaction_id",
                table: "transaction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "transaction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Relational:ColumnOrder", 6);

            migrationBuilder.AlterColumn<bool>(
                name: "sended",
                table: "transaction",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false)
                .OldAnnotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<string>(
                name: "request",
                table: "transaction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Relational:ColumnOrder", 10);

            migrationBuilder.AlterColumn<string>(
                name: "interface_name",
                table: "transaction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "hotel_code",
                table: "transaction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "transaction",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone")
                .OldAnnotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "transaction",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<bool>(
                name: "received",
                table: "transaction",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "received_at",
                table: "transaction",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "sended_at",
                table: "transaction",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "received",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "received_at",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "sended_at",
                table: "transaction");

            migrationBuilder.AlterColumn<string>(
                name: "xml",
                table: "transaction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Relational:ColumnOrder", 11);

            migrationBuilder.AlterColumn<string>(
                name: "url",
                table: "transaction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "transaction",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone")
                .Annotation("Relational:ColumnOrder", 9);

            migrationBuilder.AlterColumn<string>(
                name: "transaction_id",
                table: "transaction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "transaction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Relational:ColumnOrder", 6);

            migrationBuilder.AlterColumn<bool>(
                name: "sended",
                table: "transaction",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false)
                .Annotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<string>(
                name: "request",
                table: "transaction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Relational:ColumnOrder", 10);

            migrationBuilder.AlterColumn<string>(
                name: "interface_name",
                table: "transaction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "hotel_code",
                table: "transaction",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "transaction",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone")
                .Annotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "transaction",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 1)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
