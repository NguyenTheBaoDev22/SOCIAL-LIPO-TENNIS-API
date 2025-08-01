using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPartnerCallbackLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MerchantSource",
                table: "Merchants",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "PartnerId",
                table: "Merchants",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedAt",
                table: "Merchants",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerifiedBy",
                table: "Merchants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PartnerId",
                table: "ClientCredentials",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Partner",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CallbackIpnUrl = table.Column<string>(type: "text", nullable: true),
                    PublicKey = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    TraceId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "partner_orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PartnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    PartnerCode = table.Column<string>(type: "text", nullable: false),
                    MerchantCode = table.Column<string>(type: "text", nullable: false),
                    MerchantBranchCode = table.Column<string>(type: "text", nullable: false),
                    PaymentTerminalCode = table.Column<string>(type: "text", nullable: false),
                    QrType = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    PurposeOfTransaction = table.Column<string>(type: "text", nullable: true),
                    OrderCode = table.Column<string>(type: "text", nullable: true),
                    Ipn = table.Column<string>(type: "text", nullable: false),
                    QrContent = table.Column<string>(type: "text", nullable: true),
                    QrExpiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TransactionId = table.Column<string>(type: "text", nullable: true),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    SourceIpAddress = table.Column<string>(type: "text", nullable: true),
                    UserAgent = table.Column<string>(type: "text", nullable: true),
                    RequestRawJson = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    TraceId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partner_orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_partner_orders_Partner_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "partner_merchant_status_callback_logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CallbackUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    HttpStatusCode = table.Column<int>(type: "integer", nullable: false),
                    ResponseContent = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    IsSuccess = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    PartnerOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    TraceId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partner_merchant_status_callback_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_partner_merchant_status_callback_logs_partner_orders_Partne~",
                        column: x => x.PartnerOrderId,
                        principalTable: "partner_orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "partner_transaction_callback_logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PartnerOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CallbackUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    HttpStatusCode = table.Column<int>(type: "integer", nullable: false),
                    ResponseContent = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    IsSuccess = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    TraceId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partner_transaction_callback_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_partner_transaction_callback_logs_partner_orders_PartnerOrd~",
                        column: x => x.PartnerOrderId,
                        principalTable: "partner_orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_PartnerId",
                table: "Merchants",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientCredentials_PartnerId",
                table: "ClientCredentials",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_partner_merchant_status_callback_logs_PartnerOrderId",
                table: "partner_merchant_status_callback_logs",
                column: "PartnerOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_partner_orders_PartnerCode_OrderCode",
                table: "partner_orders",
                columns: new[] { "PartnerCode", "OrderCode" });

            migrationBuilder.CreateIndex(
                name: "IX_partner_orders_PartnerId",
                table: "partner_orders",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_partner_transaction_callback_logs_PartnerOrderId",
                table: "partner_transaction_callback_logs",
                column: "PartnerOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCredentials_Partner_PartnerId",
                table: "ClientCredentials",
                column: "PartnerId",
                principalTable: "Partner",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Merchants_Partner_PartnerId",
                table: "Merchants",
                column: "PartnerId",
                principalTable: "Partner",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientCredentials_Partner_PartnerId",
                table: "ClientCredentials");

            migrationBuilder.DropForeignKey(
                name: "FK_Merchants_Partner_PartnerId",
                table: "Merchants");

            migrationBuilder.DropTable(
                name: "partner_merchant_status_callback_logs");

            migrationBuilder.DropTable(
                name: "partner_transaction_callback_logs");

            migrationBuilder.DropTable(
                name: "partner_orders");

            migrationBuilder.DropTable(
                name: "Partner");

            migrationBuilder.DropIndex(
                name: "IX_Merchants_PartnerId",
                table: "Merchants");

            migrationBuilder.DropIndex(
                name: "IX_ClientCredentials_PartnerId",
                table: "ClientCredentials");

            migrationBuilder.DropColumn(
                name: "MerchantSource",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "VerifiedBy",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "ClientCredentials");
        }
    }
}
