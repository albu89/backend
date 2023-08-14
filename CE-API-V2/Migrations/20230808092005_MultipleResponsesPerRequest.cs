using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CE_API_V2.Migrations
{
    /// <inheritdoc />
    public partial class MultipleResponsesPerRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ScoringResponses_RequestId",
                table: "ScoringResponses");

            migrationBuilder.AddColumn<Guid>(
                name: "BiomarkersId",
                table: "ScoringResponses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScoringResponses_BiomarkersId",
                table: "ScoringResponses",
                column: "BiomarkersId",
                unique: true,
                filter: "[BiomarkersId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ScoringResponses_RequestId",
                table: "ScoringResponses",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScoringResponses_Biomarkers_BiomarkersId",
                table: "ScoringResponses",
                column: "BiomarkersId",
                principalTable: "Biomarkers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScoringResponses_Biomarkers_BiomarkersId",
                table: "ScoringResponses");

            migrationBuilder.DropIndex(
                name: "IX_ScoringResponses_BiomarkersId",
                table: "ScoringResponses");

            migrationBuilder.DropIndex(
                name: "IX_ScoringResponses_RequestId",
                table: "ScoringResponses");

            migrationBuilder.DropColumn(
                name: "BiomarkersId",
                table: "ScoringResponses");

            migrationBuilder.CreateIndex(
                name: "IX_ScoringResponses_RequestId",
                table: "ScoringResponses",
                column: "RequestId",
                unique: true);
        }
    }
}
