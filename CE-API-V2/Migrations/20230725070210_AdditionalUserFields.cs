using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CE_API_V2.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalUserFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EMailAdress",
                table: "Users",
                newName: "ZipCode");

            migrationBuilder.AddColumn<string>(
                name: "EMailAddress",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EMailAddress",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "Users",
                newName: "EMailAdress");
        }
    }
}
