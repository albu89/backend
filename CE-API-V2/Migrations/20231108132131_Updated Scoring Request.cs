using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CE_API_V2.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedScoringRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClinicalSetting",
                table: "ScoringRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ClinicalSettingDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClinicalSettingUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClinicalSetting",
                table: "ScoringRequests");

            migrationBuilder.DropColumn(
                name: "ClinicalSettingDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "ClinicalSettingUnit",
                table: "Biomarkers");
        }
    }
}
