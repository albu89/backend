using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CE_API_V2.Migrations
{
    /// <inheritdoc />
    public partial class FixedBiomarkersTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UricAcidUnit",
                table: "Biomarkers",
                newName: "UricacidUnit");

            migrationBuilder.RenameColumn(
                name: "UricAcid",
                table: "Biomarkers",
                newName: "Uricacid");

            migrationBuilder.RenameColumn(
                name: "TcAggInhibitor",
                table: "Biomarkers",
                newName: "Tcagginhibitor");

            migrationBuilder.RenameColumn(
                name: "HsTroponinTUnit",
                table: "Biomarkers",
                newName: "HstroponintUnit");

            migrationBuilder.RenameColumn(
                name: "HsTroponinT",
                table: "Biomarkers",
                newName: "Hstroponint");

            migrationBuilder.RenameColumn(
                name: "ChestPain",
                table: "Biomarkers",
                newName: "Chestpain");

            migrationBuilder.RenameColumn(
                name: "AceInhibitor",
                table: "Biomarkers",
                newName: "Aceinhibitor");

            migrationBuilder.RenameColumn(
                name: "SystolicBloodPressureUnit",
                table: "Biomarkers",
                newName: "SystolicbpUnit");

            migrationBuilder.RenameColumn(
                name: "SystolicBloodPressure",
                table: "Biomarkers",
                newName: "Systolicbp");

            migrationBuilder.RenameColumn(
                name: "RestingECG",
                table: "Biomarkers",
                newName: "Qwave");

            migrationBuilder.RenameColumn(
                name: "PancreaticAmylaseUnit",
                table: "Biomarkers",
                newName: "LeukocyteUnit");

            migrationBuilder.RenameColumn(
                name: "PancreaticAmylase",
                table: "Biomarkers",
                newName: "Leukocyte");

            migrationBuilder.RenameColumn(
                name: "OganicNitrate",
                table: "Biomarkers",
                newName: "Nitrate");

            migrationBuilder.RenameColumn(
                name: "LeukocytesUnit",
                table: "Biomarkers",
                newName: "DiastolicbpUnit");

            migrationBuilder.RenameColumn(
                name: "Leukocytes",
                table: "Biomarkers",
                newName: "Amylasep");

            migrationBuilder.RenameColumn(
                name: "DiastolicBloodPressureUnit",
                table: "Biomarkers",
                newName: "AmylasepUnit");

            migrationBuilder.RenameColumn(
                name: "DiastolicBloodPressure",
                table: "Biomarkers",
                newName: "Diastolicbp");

            migrationBuilder.RenameColumn(
                name: "CaAntagonist",
                table: "Biomarkers",
                newName: "Calciumant");

            migrationBuilder.RenameColumn(
                name: "AlkalinePhosphate",
                table: "Biomarkers",
                newName: "Alkaline");

            migrationBuilder.RenameColumn(
                name: "AlkalinePhosphataseUnit",
                table: "Biomarkers",
                newName: "AlkalineUnit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UricacidUnit",
                table: "Biomarkers",
                newName: "UricAcidUnit");

            migrationBuilder.RenameColumn(
                name: "Uricacid",
                table: "Biomarkers",
                newName: "UricAcid");

            migrationBuilder.RenameColumn(
                name: "Tcagginhibitor",
                table: "Biomarkers",
                newName: "TcAggInhibitor");

            migrationBuilder.RenameColumn(
                name: "HstroponintUnit",
                table: "Biomarkers",
                newName: "HsTroponinTUnit");

            migrationBuilder.RenameColumn(
                name: "Hstroponint",
                table: "Biomarkers",
                newName: "HsTroponinT");

            migrationBuilder.RenameColumn(
                name: "Chestpain",
                table: "Biomarkers",
                newName: "ChestPain");

            migrationBuilder.RenameColumn(
                name: "Aceinhibitor",
                table: "Biomarkers",
                newName: "AceInhibitor");

            migrationBuilder.RenameColumn(
                name: "SystolicbpUnit",
                table: "Biomarkers",
                newName: "SystolicBloodPressureUnit");

            migrationBuilder.RenameColumn(
                name: "Systolicbp",
                table: "Biomarkers",
                newName: "SystolicBloodPressure");

            migrationBuilder.RenameColumn(
                name: "Qwave",
                table: "Biomarkers",
                newName: "RestingECG");

            migrationBuilder.RenameColumn(
                name: "Nitrate",
                table: "Biomarkers",
                newName: "OganicNitrate");

            migrationBuilder.RenameColumn(
                name: "LeukocyteUnit",
                table: "Biomarkers",
                newName: "PancreaticAmylaseUnit");

            migrationBuilder.RenameColumn(
                name: "Leukocyte",
                table: "Biomarkers",
                newName: "PancreaticAmylase");

            migrationBuilder.RenameColumn(
                name: "DiastolicbpUnit",
                table: "Biomarkers",
                newName: "LeukocytesUnit");

            migrationBuilder.RenameColumn(
                name: "Diastolicbp",
                table: "Biomarkers",
                newName: "DiastolicBloodPressure");

            migrationBuilder.RenameColumn(
                name: "Calciumant",
                table: "Biomarkers",
                newName: "CaAntagonist");

            migrationBuilder.RenameColumn(
                name: "AmylasepUnit",
                table: "Biomarkers",
                newName: "DiastolicBloodPressureUnit");

            migrationBuilder.RenameColumn(
                name: "Amylasep",
                table: "Biomarkers",
                newName: "Leukocytes");

            migrationBuilder.RenameColumn(
                name: "AlkalineUnit",
                table: "Biomarkers",
                newName: "AlkalinePhosphataseUnit");

            migrationBuilder.RenameColumn(
                name: "Alkaline",
                table: "Biomarkers",
                newName: "AlkalinePhosphate");
        }
    }
}
