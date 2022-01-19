using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace graduationProject.Data.Migrations
{
    public partial class intial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserReservations");

            migrationBuilder.DropTable(
                name: "Hospital");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "security",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Gov_Name",
                table: "Governorates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "security",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Gov_Name",
                table: "Governorates",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Hospital",
                columns: table => new
                {
                    Hos_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayPrice = table.Column<double>(type: "float", nullable: false),
                    Dept_Id = table.Column<int>(type: "int", nullable: false),
                    Hos_Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hos_Incubators = table.Column<int>(type: "int", nullable: false),
                    Hos_Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hos_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hos_Phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospital", x => x.Hos_Id);
                    table.ForeignKey(
                        name: "FK_Hospital_Departments_Dept_Id",
                        column: x => x.Dept_Id,
                        principalTable: "Departments",
                        principalColumn: "Dept_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserReservations",
                columns: table => new
                {
                    User_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Hos_Id = table.Column<int>(type: "int", nullable: false),
                    Num_Incubators = table.Column<int>(type: "int", nullable: false),
                    Res_Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReservations", x => new { x.User_Id, x.Hos_Id });
                    table.ForeignKey(
                        name: "FK_UserReservations_Hospital_Hos_Id",
                        column: x => x.Hos_Id,
                        principalTable: "Hospital",
                        principalColumn: "Hos_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserReservations_Users_User_Id",
                        column: x => x.User_Id,
                        principalSchema: "security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hospital_Dept_Id",
                table: "Hospital",
                column: "Dept_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserReservations_Hos_Id",
                table: "UserReservations",
                column: "Hos_Id");
        }
    }
}
