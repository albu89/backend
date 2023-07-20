using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CE_API_V2.Migrations
{
    /// <inheritdoc />
    public partial class BiomarkersUnits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AgeUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AlatUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AlbuminUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AlkalinePhosphataseUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BilirubinUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CholesterolUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DiastolicBloodPressureUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GlucoseUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HdlUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HeightUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HsTroponinTUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LdlUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LeukocytesUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MchcUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PancreaticAmylaseUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProteinUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SystolicBloodPressureUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UreaUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UricAcidUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WeightUnit",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgeUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "AlatUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "AlbuminUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "AlkalinePhosphataseUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "BilirubinUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "CholesterolUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "DiastolicBloodPressureUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "GlucoseUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "HdlUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "HeightUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "HsTroponinTUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "LdlUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "LeukocytesUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "MchcUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "PancreaticAmylaseUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "ProteinUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "SystolicBloodPressureUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "UreaUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "UricAcidUnit",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "WeightUnit",
                table: "Biomarkers");
        }
    }
}
