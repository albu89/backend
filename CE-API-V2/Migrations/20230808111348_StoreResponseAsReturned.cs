using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CE_API_V2.Migrations
{
    /// <inheritdoc />
    public partial class StoreResponseAsReturned : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Recommendation",
                table: "ScoringResponses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecommendationLong",
                table: "ScoringResponses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Risk",
                table: "ScoringResponses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RiskClass",
                table: "ScoringResponses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Score",
                table: "ScoringResponses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Recommendation",
                table: "ScoringResponses");

            migrationBuilder.DropColumn(
                name: "RecommendationLong",
                table: "ScoringResponses");

            migrationBuilder.DropColumn(
                name: "Risk",
                table: "ScoringResponses");

            migrationBuilder.DropColumn(
                name: "RiskClass",
                table: "ScoringResponses");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "ScoringResponses");
        }
    }
}
