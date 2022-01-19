using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace graduationProject.Data.Migrations
{
    public partial class adduserreservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserReservations",
                columns: table => new
                {
                    User_Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Hos_Id = table.Column<int>(type: "int", nullable: false),
                    Res_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Num_Incubators = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReservations", x => new { x.User_Id, x.Hos_Id });
                    table.ForeignKey(
                        name: "FK_UserReservations_Hospitals_Hos_Id",
                        column: x => x.Hos_Id,
                        principalTable: "Hospitals",
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
                name: "IX_UserReservations_Hos_Id",
                table: "UserReservations",
                column: "Hos_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserReservations");
        }
    }
}
