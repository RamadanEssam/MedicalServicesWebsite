using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace graduationProject.Data.Migrations
{
    public partial class addHospital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hospitals",
                columns: table => new
                {
                    Hos_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hos_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hos_Incubators = table.Column<int>(type: "int", nullable: false),
                    DayPrice = table.Column<float>(type: "real", nullable: false),
                    Hos_Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hos_Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hos_Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Dept_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.Hos_Id);
                    table.ForeignKey(
                        name: "FK_Hospitals_Departments_Dept_Id",
                        column: x => x.Dept_Id,
                        principalTable: "Departments",
                        principalColumn: "Dept_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_Dept_Id",
                table: "Hospitals",
                column: "Dept_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hospitals");
        }
    }
}
