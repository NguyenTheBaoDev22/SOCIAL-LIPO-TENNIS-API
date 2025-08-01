using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMerchantBranch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BranchAddress",
                table: "MerchantBranches",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "SequenceNumber",
                table: "MerchantBranches",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "MerchantBranchCode",
                table: "MerchantBranches",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                computedColumnSql: "ufn_generate_merchant_branch_code(\"MerchantId\", \"SequenceNumber\")",
                stored: true,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SequenceNumber",
                table: "MerchantBranches");

            migrationBuilder.AlterColumn<string>(
                name: "MerchantBranchCode",
                table: "MerchantBranches",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3,
                oldComputedColumnSql: "ufn_generate_merchant_branch_code(\"MerchantId\", \"SequenceNumber\")");

            migrationBuilder.AlterColumn<string>(
                name: "BranchAddress",
                table: "MerchantBranches",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
