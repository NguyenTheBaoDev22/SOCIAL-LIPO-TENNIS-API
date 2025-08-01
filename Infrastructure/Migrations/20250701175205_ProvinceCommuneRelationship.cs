using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProvinceCommuneRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communes_Provinces_ProvinceId1",
                table: "Communes");

            migrationBuilder.DropIndex(
                name: "IX_Communes_ProvinceId1",
                table: "Communes");

            migrationBuilder.DropColumn(
                name: "ProvinceId1",
                table: "Communes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProvinceId1",
                table: "Communes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Communes_ProvinceId1",
                table: "Communes",
                column: "ProvinceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Communes_Provinces_ProvinceId1",
                table: "Communes",
                column: "ProvinceId1",
                principalTable: "Provinces",
                principalColumn: "Id");
        }
    }
}
