using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PartnerOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartnerMerchantStatusCallbackLogs_PartnerOrder_PartnerOrder~",
                table: "PartnerMerchantStatusCallbackLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerOrder_Partner_PartnerId",
                table: "PartnerOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerTransactionCallbackLogs_PartnerOrder_PartnerOrderId",
                table: "PartnerTransactionCallbackLogs");

            migrationBuilder.DropIndex(
                name: "IX_PartnerMerchantStatusCallbackLogs_PartnerOrderId",
                table: "PartnerMerchantStatusCallbackLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartnerOrder",
                table: "PartnerOrder");

            migrationBuilder.DropColumn(
                name: "PartnerOrderId",
                table: "PartnerMerchantStatusCallbackLogs");

            migrationBuilder.RenameTable(
                name: "PartnerOrder",
                newName: "PartnerOrders");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerOrder_PartnerId",
                table: "PartnerOrders",
                newName: "IX_PartnerOrders_PartnerId");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerOrder_PartnerCode_OrderCode",
                table: "PartnerOrders",
                newName: "IX_PartnerOrders_PartnerCode_OrderCode");

            migrationBuilder.AlterColumn<string>(
                name: "UserAgent",
                table: "PartnerOrders",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PartnerOrders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "SourceIpAddress",
                table: "PartnerOrders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PurposeOfTransaction",
                table: "PartnerOrders",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PartnerId",
                table: "PartnerOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "PartnerCode",
                table: "PartnerOrders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "OrderCode",
                table: "PartnerOrders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ipn",
                table: "PartnerOrders",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "IpnLastAttemptAt",
                table: "PartnerOrders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IpnRetryCount",
                table: "PartnerOrders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IpnSent",
                table: "PartnerOrders",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IpnSuccess",
                table: "PartnerOrders",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartnerOrders",
                table: "PartnerOrders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerOrders_Partner_PartnerId",
                table: "PartnerOrders",
                column: "PartnerId",
                principalTable: "Partner",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerTransactionCallbackLogs_PartnerOrders_PartnerOrderId",
                table: "PartnerTransactionCallbackLogs",
                column: "PartnerOrderId",
                principalTable: "PartnerOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartnerOrders_Partner_PartnerId",
                table: "PartnerOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PartnerTransactionCallbackLogs_PartnerOrders_PartnerOrderId",
                table: "PartnerTransactionCallbackLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartnerOrders",
                table: "PartnerOrders");

            migrationBuilder.DropColumn(
                name: "IpnLastAttemptAt",
                table: "PartnerOrders");

            migrationBuilder.DropColumn(
                name: "IpnRetryCount",
                table: "PartnerOrders");

            migrationBuilder.DropColumn(
                name: "IpnSent",
                table: "PartnerOrders");

            migrationBuilder.DropColumn(
                name: "IpnSuccess",
                table: "PartnerOrders");

            migrationBuilder.RenameTable(
                name: "PartnerOrders",
                newName: "PartnerOrder");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerOrders_PartnerId",
                table: "PartnerOrder",
                newName: "IX_PartnerOrder_PartnerId");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerOrders_PartnerCode_OrderCode",
                table: "PartnerOrder",
                newName: "IX_PartnerOrder_PartnerCode_OrderCode");

            migrationBuilder.AddColumn<Guid>(
                name: "PartnerOrderId",
                table: "PartnerMerchantStatusCallbackLogs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserAgent",
                table: "PartnerOrder",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PartnerOrder",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SourceIpAddress",
                table: "PartnerOrder",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PurposeOfTransaction",
                table: "PartnerOrder",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PartnerId",
                table: "PartnerOrder",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PartnerCode",
                table: "PartnerOrder",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrderCode",
                table: "PartnerOrder",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ipn",
                table: "PartnerOrder",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartnerOrder",
                table: "PartnerOrder",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerMerchantStatusCallbackLogs_PartnerOrderId",
                table: "PartnerMerchantStatusCallbackLogs",
                column: "PartnerOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerMerchantStatusCallbackLogs_PartnerOrder_PartnerOrder~",
                table: "PartnerMerchantStatusCallbackLogs",
                column: "PartnerOrderId",
                principalTable: "PartnerOrder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerOrder_Partner_PartnerId",
                table: "PartnerOrder",
                column: "PartnerId",
                principalTable: "Partner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerTransactionCallbackLogs_PartnerOrder_PartnerOrderId",
                table: "PartnerTransactionCallbackLogs",
                column: "PartnerOrderId",
                principalTable: "PartnerOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
