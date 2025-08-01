using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommuneCode",
                table: "MerchantBranches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommuneName",
                table: "MerchantBranches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProvinceCode",
                table: "MerchantBranches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProvinceName",
                table: "MerchantBranches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTerminals_MerchantCode_MerchantBranchCode_TerminalCo~",
                table: "PaymentTerminals",
                columns: new[] { "MerchantCode", "MerchantBranchCode", "TerminalCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_BusinessRegistrationNo",
                table: "Merchants",
                column: "BusinessRegistrationNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MerchantBranches_MerchantCode_MerchantBranchCode",
                table: "MerchantBranches",
                columns: new[] { "MerchantCode", "MerchantBranchCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentTerminals_MerchantCode_MerchantBranchCode_TerminalCo~",
                table: "PaymentTerminals");

            migrationBuilder.DropIndex(
                name: "IX_Merchants_BusinessRegistrationNo",
                table: "Merchants");

            migrationBuilder.DropIndex(
                name: "IX_MerchantBranches_MerchantCode_MerchantBranchCode",
                table: "MerchantBranches");

            migrationBuilder.DropColumn(
                name: "CommuneCode",
                table: "MerchantBranches");

            migrationBuilder.DropColumn(
                name: "CommuneName",
                table: "MerchantBranches");

            migrationBuilder.DropColumn(
                name: "ProvinceCode",
                table: "MerchantBranches");

            migrationBuilder.DropColumn(
                name: "ProvinceName",
                table: "MerchantBranches");
        }
    }
}
