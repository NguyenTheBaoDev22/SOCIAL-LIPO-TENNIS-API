using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatMerchantActiveCallbackUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActiveCallbackUrl",
                table: "PaymentTerminals",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActiveCallbackUrl",
                table: "MerchantBranches",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveCallbackUrl",
                table: "PaymentTerminals");

            migrationBuilder.DropColumn(
                name: "ActiveCallbackUrl",
                table: "MerchantBranches");
        }
    }
}
