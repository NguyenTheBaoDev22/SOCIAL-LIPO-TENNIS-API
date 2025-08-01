using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PaymentTerminal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IPAddress",
                table: "PaymentTerminals",
                newName: "DeviceId");

            migrationBuilder.AddColumn<string>(
                name: "IMEI",
                table: "PaymentTerminals",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IMEI",
                table: "PaymentTerminals");

            migrationBuilder.RenameColumn(
                name: "DeviceId",
                table: "PaymentTerminals",
                newName: "IPAddress");
        }
    }
}
