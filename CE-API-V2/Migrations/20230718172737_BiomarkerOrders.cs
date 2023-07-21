using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CE_API_V2.Migrations
{
    /// <inheritdoc />
    public partial class BiomarkerOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BiomarkerOrders",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BiomarkerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    PreferredUnit = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiomarkerOrders", x => new { x.UserId, x.BiomarkerId });
                    table.ForeignKey(
                        name: "FK_BiomarkerOrders_User",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BiomarkerOrders");
        }
    }
}
