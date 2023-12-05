using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CE_API_V2.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedFaultyUserInputMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_BillingId",
                table: "Users",
                column: "BillingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Billings_BillingId",
                table: "Users",
                column: "BillingId",
                principalTable: "Billings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Billings_BillingId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_BillingId",
                table: "Users");
        }
    }
}
