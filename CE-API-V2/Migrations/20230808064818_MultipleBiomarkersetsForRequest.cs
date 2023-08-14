using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CE_API_V2.Migrations
{
    /// <inheritdoc />
    public partial class MultipleBiomarkersetsForRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Biomarkers_RequestId",
                table: "Biomarkers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ScoringRequests",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ScoringRequests_UserId",
                table: "ScoringRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Biomarkers_RequestId",
                table: "Biomarkers",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScoringRequests_Users_UserId",
                table: "ScoringRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoringRequests_Users_UserId",
                table: "ScoringRequests");

            migrationBuilder.DropIndex(
                name: "IX_ScoringRequests_UserId",
                table: "ScoringRequests");

            migrationBuilder.DropIndex(
                name: "IX_Biomarkers_RequestId",
                table: "Biomarkers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ScoringRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Biomarkers_RequestId",
                table: "Biomarkers",
                column: "RequestId",
                unique: true);
        }
    }
}
