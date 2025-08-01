using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatMerchant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Merchants",
                newName: "PrimaryPhone");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Merchants",
                newName: "PrimaryEmail");

            migrationBuilder.RenameIndex(
                name: "IX_Merchants_Email",
                table: "Merchants",
                newName: "IX_Merchants_PrimaryEmail");

            migrationBuilder.AddColumn<string>(
                name: "OwnerIdCardBackUrl",
                table: "Merchants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerIdCardFrontUrl",
                table: "Merchants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerIdCardNumber",
                table: "Merchants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryTaxNumber",
                table: "Merchants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "BranchEmail",
                table: "MerchantBranches",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountHolder",
                table: "MerchantBranches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "MerchantBranches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "MerchantBranches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BranchPhone",
                table: "MerchantBranches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BranchTaxNumber",
                table: "MerchantBranches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsHeadOffice",
                table: "MerchantBranches",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MerchantCategoryCode",
                table: "MerchantBranches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SignedForm01AUrl",
                table: "MerchantBranches",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerIdCardBackUrl",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "OwnerIdCardFrontUrl",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "OwnerIdCardNumber",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "PrimaryTaxNumber",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "BankAccountHolder",
                table: "MerchantBranches");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "MerchantBranches");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "MerchantBranches");

            migrationBuilder.DropColumn(
                name: "BranchPhone",
                table: "MerchantBranches");

            migrationBuilder.DropColumn(
                name: "BranchTaxNumber",
                table: "MerchantBranches");

            migrationBuilder.DropColumn(
                name: "IsHeadOffice",
                table: "MerchantBranches");

            migrationBuilder.DropColumn(
                name: "MerchantCategoryCode",
                table: "MerchantBranches");

            migrationBuilder.DropColumn(
                name: "SignedForm01AUrl",
                table: "MerchantBranches");

            migrationBuilder.RenameColumn(
                name: "PrimaryPhone",
                table: "Merchants",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "PrimaryEmail",
                table: "Merchants",
                newName: "Email");

            migrationBuilder.RenameIndex(
                name: "IX_Merchants_PrimaryEmail",
                table: "Merchants",
                newName: "IX_Merchants_Email");

            migrationBuilder.AlterColumn<string>(
                name: "BranchEmail",
                table: "MerchantBranches",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);
        }
    }
}
