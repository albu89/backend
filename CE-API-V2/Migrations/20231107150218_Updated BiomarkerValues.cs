using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CE_API_V2.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedBiomarkerValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "AceinhibitorDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AgeDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AlatDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AlbuminDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AlkalineDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AmylasepDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BetablockerDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BilirubinDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CalciumantDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChestpainDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CholesterolDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DiabetesDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DiastolicbpDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DiureticDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GlucoseDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HdlDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HeightDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HstroponintDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LdlDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LeukocyteDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MchcDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NicotineDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NitrateDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PriorCADDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProteinDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QwaveDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SexDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StatinDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SystolicbpDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TcagginhibitorDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UreaDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UricacidDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WeightDisplayValue",
                table: "Biomarkers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AceinhibitorDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "AgeDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "AlatDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "AlbuminDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "AlkalineDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "AmylasepDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "BetablockerDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "BilirubinDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "CalciumantDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "ChestpainDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "CholesterolDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "DiabetesDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "DiastolicbpDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "DiureticDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "GlucoseDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "HdlDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "HeightDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "HstroponintDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "LdlDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "LeukocyteDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "MchcDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "NicotineDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "NitrateDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "PriorCADDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "ProteinDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "QwaveDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "SexDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "StatinDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "SystolicbpDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "TcagginhibitorDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "UreaDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "UricacidDisplayValue",
                table: "Biomarkers");

            migrationBuilder.DropColumn(
                name: "WeightDisplayValue",
                table: "Biomarkers");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);
        }
    }
}
