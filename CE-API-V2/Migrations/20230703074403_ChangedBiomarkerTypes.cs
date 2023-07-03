using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CE_API_V2.Migrations
{
    /// <inheritdoc />
    public partial class ChangedBiomarkerTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SystolicBloodPressure",
                table: "Biomarkers",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<int>(
                name: "DiastolicBloodPressure",
                table: "Biomarkers",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "SystolicBloodPressure",
                table: "Biomarkers",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "DiastolicBloodPressure",
                table: "Biomarkers",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
