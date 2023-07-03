using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CE_API_V2.Migrations
{
    /// <inheritdoc />
    public partial class RemovedOrganisation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveDirectoryID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Organisation",
                table: "Users");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedOn",
                table: "Users",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AddColumn<string>(
                name: "ActiveDirectoryID",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Organisation",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
