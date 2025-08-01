using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamePartnerCallbackTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_partner_merchant_status_callback_logs_partner_orders_Partne~",
                table: "partner_merchant_status_callback_logs");

            migrationBuilder.DropForeignKey(
                name: "FK_partner_orders_Partner_PartnerId",
                table: "partner_orders");

            migrationBuilder.DropForeignKey(
                name: "FK_partner_transaction_callback_logs_partner_orders_PartnerOrd~",
                table: "partner_transaction_callback_logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_partner_transaction_callback_logs",
                table: "partner_transaction_callback_logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_partner_orders",
                table: "partner_orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_partner_merchant_status_callback_logs",
                table: "partner_merchant_status_callback_logs");

            migrationBuilder.RenameTable(
                name: "partner_transaction_callback_logs",
                newName: "PartnerTransactionCallbackLogs");

            migrationBuilder.RenameTable(
                name: "partner_orders",
                newName: "PartnerOrder");

            migrationBuilder.RenameTable(
                name: "partner_merchant_status_callback_logs",
                newName: "PartnerMerchantStatusCallbackLogs");

            migrationBuilder.RenameIndex(
                name: "IX_partner_transaction_callback_logs_PartnerOrderId",
                table: "PartnerTransactionCallbackLogs",
                newName: "IX_PartnerTransactionCallbackLogs_PartnerOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_partner_orders_PartnerId",
                table: "PartnerOrder",
                newName: "IX_PartnerOrder_PartnerId");

            migrationBuilder.RenameIndex(
                name: "IX_partner_orders_PartnerCode_OrderCode",
                table: "PartnerOrder",
                newName: "IX_PartnerOrder_PartnerCode_OrderCode");

            migrationBuilder.RenameIndex(
                name: "IX_partner_merchant_status_callback_logs_PartnerOrderId",
                table: "PartnerMerchantStatusCallbackLogs",
                newName: "IX_PartnerMerchantStatusCallbackLogs_PartnerOrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartnerTransactionCallbackLogs",
                table: "PartnerTransactionCallbackLogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartnerOrder",
                table: "PartnerOrder",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartnerMerchantStatusCallbackLogs",
                table: "PartnerMerchantStatusCallbackLogs",
                column: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartnerTransactionCallbackLogs",
                table: "PartnerTransactionCallbackLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartnerOrder",
                table: "PartnerOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartnerMerchantStatusCallbackLogs",
                table: "PartnerMerchantStatusCallbackLogs");

            migrationBuilder.RenameTable(
                name: "PartnerTransactionCallbackLogs",
                newName: "partner_transaction_callback_logs");

            migrationBuilder.RenameTable(
                name: "PartnerOrder",
                newName: "partner_orders");

            migrationBuilder.RenameTable(
                name: "PartnerMerchantStatusCallbackLogs",
                newName: "partner_merchant_status_callback_logs");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerTransactionCallbackLogs_PartnerOrderId",
                table: "partner_transaction_callback_logs",
                newName: "IX_partner_transaction_callback_logs_PartnerOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerOrder_PartnerId",
                table: "partner_orders",
                newName: "IX_partner_orders_PartnerId");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerOrder_PartnerCode_OrderCode",
                table: "partner_orders",
                newName: "IX_partner_orders_PartnerCode_OrderCode");

            migrationBuilder.RenameIndex(
                name: "IX_PartnerMerchantStatusCallbackLogs_PartnerOrderId",
                table: "partner_merchant_status_callback_logs",
                newName: "IX_partner_merchant_status_callback_logs_PartnerOrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_partner_transaction_callback_logs",
                table: "partner_transaction_callback_logs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_partner_orders",
                table: "partner_orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_partner_merchant_status_callback_logs",
                table: "partner_merchant_status_callback_logs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_partner_merchant_status_callback_logs_partner_orders_Partne~",
                table: "partner_merchant_status_callback_logs",
                column: "PartnerOrderId",
                principalTable: "partner_orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_partner_orders_Partner_PartnerId",
                table: "partner_orders",
                column: "PartnerId",
                principalTable: "Partner",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_partner_transaction_callback_logs_partner_orders_PartnerOrd~",
                table: "partner_transaction_callback_logs",
                column: "PartnerOrderId",
                principalTable: "partner_orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
