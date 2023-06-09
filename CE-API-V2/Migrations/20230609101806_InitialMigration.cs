using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CE_API_V2.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScoringRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Biomarkers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicalSetting = table.Column<int>(type: "int", nullable: false),
                    PriorCAD = table.Column<bool>(type: "bit", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    ChestPain = table.Column<int>(type: "int", nullable: false),
                    Nicotine = table.Column<int>(type: "int", nullable: false),
                    Diabetes = table.Column<int>(type: "int", nullable: false),
                    Statin = table.Column<bool>(type: "bit", nullable: false),
                    TcAggInhibitor = table.Column<bool>(type: "bit", nullable: false),
                    AceInhibitor = table.Column<bool>(type: "bit", nullable: false),
                    CaAntagonist = table.Column<bool>(type: "bit", nullable: false),
                    Betablocker = table.Column<bool>(type: "bit", nullable: false),
                    Diuretic = table.Column<bool>(type: "bit", nullable: false),
                    OganicNitrate = table.Column<bool>(type: "bit", nullable: false),
                    SystolicBloodPressure = table.Column<float>(type: "real", nullable: false),
                    DiastolicBloodPressure = table.Column<float>(type: "real", nullable: false),
                    RestingECG = table.Column<int>(type: "int", nullable: false),
                    PancreaticAmylase = table.Column<float>(type: "real", nullable: false),
                    AlkalinePhosphate = table.Column<float>(type: "real", nullable: false),
                    HsTroponinT = table.Column<float>(type: "real", nullable: false),
                    Alat = table.Column<float>(type: "real", nullable: false),
                    Glucose = table.Column<float>(type: "real", nullable: false),
                    Bilirubin = table.Column<float>(type: "real", nullable: false),
                    Urea = table.Column<float>(type: "real", nullable: false),
                    UricAcid = table.Column<float>(type: "real", nullable: false),
                    Cholesterol = table.Column<float>(type: "real", nullable: false),
                    Hdl = table.Column<float>(type: "real", nullable: false),
                    Ldl = table.Column<float>(type: "real", nullable: false),
                    Protein = table.Column<float>(type: "real", nullable: false),
                    Albumin = table.Column<float>(type: "real", nullable: false),
                    Leukocytes = table.Column<float>(type: "real", nullable: false),
                    Mchc = table.Column<float>(type: "real", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biomarkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Biomarkers_ScoringRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "ScoringRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScoringResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    classifier_class = table.Column<int>(type: "int", nullable: true),
                    classifier_score = table.Column<double>(type: "float", nullable: true),
                    classifier_sign = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "getdate()"),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoringResponses_ScoringRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "ScoringRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Biomarkers_RequestId",
                table: "Biomarkers",
                column: "RequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScoringResponses_RequestId",
                table: "ScoringResponses",
                column: "RequestId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Biomarkers");

            migrationBuilder.DropTable(
                name: "ScoringResponses");

            migrationBuilder.DropTable(
                name: "ScoringRequests");
        }
    }
}
