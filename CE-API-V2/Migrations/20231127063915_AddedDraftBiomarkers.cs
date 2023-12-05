using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CE_API_V2.Migrations
{
    /// <inheritdoc />
    public partial class AddedDraftBiomarkers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                           name: "BiomarkersDraft",
                           columns: table => new
                           {
                               Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                               RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                               ResponseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                               ClinicalSetting = table.Column<int>(type: "int", nullable: true),
                               ClinicalSettingUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               ClinicalSettingDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               PriorCAD = table.Column<bool>(type: "bit", nullable: true),
                               PriorCADDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Age = table.Column<int>(type: "int", nullable: true),
                               AgeUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               AgeDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Sex = table.Column<int>(type: "int", nullable: true),
                               SexDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Height = table.Column<int>(type: "int", nullable: true),
                               HeightUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               HeightDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Weight = table.Column<int>(type: "int", nullable: true),
                               WeightUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               WeightDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Chestpain = table.Column<int>(type: "int", nullable: true),
                               ChestpainDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Nicotine = table.Column<int>(type: "int", nullable: true),
                               NicotineDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Diabetes = table.Column<int>(type: "int", nullable: true),
                               DiabetesDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Statin = table.Column<bool>(type: "bit", nullable: true),
                               StatinDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Tcagginhibitor = table.Column<bool>(type: "bit", nullable: true),
                               TcagginhibitorDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Aceinhibitor = table.Column<bool>(type: "bit", nullable: true),
                               AceinhibitorDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Calciumant = table.Column<bool>(type: "bit", nullable: true),
                               CalciumantDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Betablocker = table.Column<bool>(type: "bit", nullable: true),
                               BetablockerDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Diuretic = table.Column<bool>(type: "bit", nullable: true),
                               DiureticDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Nitrate = table.Column<bool>(type: "bit", nullable: true),
                               NitrateDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Systolicbp = table.Column<int>(type: "int", nullable: true),
                               SystolicbpUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               SystolicbpDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Diastolicbp = table.Column<int>(type: "int", nullable: true),
                               DiastolicbpUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               DiastolicbpDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Qwave = table.Column<int>(type: "int", nullable: true),
                               QwaveDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Amylasep = table.Column<float>(type: "real", nullable: true),
                               AmylasepUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               AmylasepDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Alkaline = table.Column<float>(type: "real", nullable: true),
                               AlkalineUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               AlkalineDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Hstroponint = table.Column<float>(type: "real", nullable: true),
                               HstroponintUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               HstroponintDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Alat = table.Column<float>(type: "real", nullable: true),
                               AlatUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               AlatDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Glucose = table.Column<float>(type: "real", nullable: true),
                               GlucoseUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               GlucoseDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Bilirubin = table.Column<float>(type: "real", nullable: true),
                               BilirubinUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               BilirubinDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Urea = table.Column<float>(type: "real", nullable: true),
                               UreaUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               UreaDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Uricacid = table.Column<float>(type: "real", nullable: true),
                               UricacidUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               UricacidDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Cholesterol = table.Column<float>(type: "real", nullable: true),
                               CholesterolUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               CholesterolDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Hdl = table.Column<float>(type: "real", nullable: true),
                               HdlUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               HdlDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Ldl = table.Column<float>(type: "real", nullable: true),
                               LdlUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               LdlDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Protein = table.Column<float>(type: "real", nullable: true),
                               ProteinUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               ProteinDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Albumin = table.Column<float>(type: "real", nullable: true),
                               AlbuminUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               AlbuminDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Leukocyte = table.Column<float>(type: "real", nullable: true),
                               LeukocyteUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               LeukocyteDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               Mchc = table.Column<float>(type: "real", nullable: true),
                               MchcUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               MchcDisplayValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                               CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "getdate()"),
                               UpdatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "getdate()")
                           },
                           constraints: table =>
                           {
                               table.PrimaryKey("PK_BiomarkersDraft", x => x.Id);
                               table.ForeignKey(
                                   name: "FK_BiomarkersDraft_ScoringRequests_RequestId",
                                   column: x => x.RequestId,
                                   principalTable: "ScoringRequests",
                                   principalColumn: "Id",
                                   onDelete: ReferentialAction.Cascade);
                               table.ForeignKey(
                                   name: "FK_BiomarkersDraft_ScoringResponses_ResponseId",
                                   column: x => x.ResponseId,
                                   principalTable: "ScoringResponses",
                                   principalColumn: "Id");
                           });

            migrationBuilder.CreateIndex(
                name: "IX_BiomarkersDraft_RequestId",
                table: "BiomarkersDraft",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_BiomarkersDraft_ResponseId",
                table: "BiomarkersDraft",
                column: "ResponseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BiomarkersDraft");
        }
    }
}
