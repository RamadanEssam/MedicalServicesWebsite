using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace graduationProject.Data.Migrations
{
    public partial class addReservationTubesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserReservationOxegins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Oxygen_Id = table.Column<int>(type: "int", nullable: false),
                    Res_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Num_tubes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReservationOxegins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserReservationOxegins_OxygenTubes_Oxygen_Id",
                        column: x => x.Oxygen_Id,
                        principalTable: "OxygenTubes",
                        principalColumn: "OxgnId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserReservationOxegins_Users_User_Id",
                        column: x => x.User_Id,
                        principalSchema: "security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserReservationOxegins_Oxygen_Id",
                table: "UserReservationOxegins",
                column: "Oxygen_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserReservationOxegins_User_Id",
                table: "UserReservationOxegins",
                column: "User_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserReservationOxegins");
        }
    }
}
