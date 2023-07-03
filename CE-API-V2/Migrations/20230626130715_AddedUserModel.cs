using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CE_API_V2.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActiveDirectoryID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salutation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfessionalSpecialisation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Organisation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreferredLab = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRole = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelephoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EMailAdress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitLabValues = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClinicalSetting = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
