using Microsoft.EntityFrameworkCore.Migrations;

namespace graduationProject.Data.Migrations
{
    public partial class addBloodAndOxeygen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BloodBanks",
                columns: table => new
                {
                    BloodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BloodType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodCost = table.Column<int>(type: "int", nullable: true),
                    BloodPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BloodDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dept_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodBanks", x => x.BloodId);
                    table.ForeignKey(
                        name: "FK_BloodBanks_Departments_Dept_Id",
                        column: x => x.Dept_Id,
                        principalTable: "Departments",
                        principalColumn: "Dept_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OxygenTubes",
                columns: table => new
                {
                    OxgnId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OxgnType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OxgnAmount = table.Column<int>(type: "int", nullable: true),
                    OxgnCost = table.Column<int>(type: "int", nullable: true),
                    OxgnPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OxgnLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OxgnDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dept_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OxygenTubes", x => x.OxgnId);
                    table.ForeignKey(
                        name: "FK_OxygenTubes_Departments_Dept_Id",
                        column: x => x.Dept_Id,
                        principalTable: "Departments",
                        principalColumn: "Dept_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BloodBanks_Dept_Id",
                table: "BloodBanks",
                column: "Dept_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OxygenTubes_Dept_Id",
                table: "OxygenTubes",
                column: "Dept_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloodBanks");

            migrationBuilder.DropTable(
                name: "OxygenTubes");
        }
    }
}
