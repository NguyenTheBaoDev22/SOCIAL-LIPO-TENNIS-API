using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "PaymentTerminals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Merchants",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "MerchantBranches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTerminals_MerchantId",
                table: "PaymentTerminals",
                column: "MerchantId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTerminals_Merchants_MerchantId",
                table: "PaymentTerminals",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTerminals_Merchants_MerchantId",
                table: "PaymentTerminals");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTerminals_MerchantId",
                table: "PaymentTerminals");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "PaymentTerminals");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "MerchantBranches");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                table: "Merchants",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
