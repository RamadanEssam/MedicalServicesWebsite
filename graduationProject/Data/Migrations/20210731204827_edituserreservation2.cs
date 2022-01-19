using Microsoft.EntityFrameworkCore.Migrations;

namespace graduationProject.Data.Migrations
{
    public partial class edituserreservation2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReservations_Users_User_Id",
                table: "UserReservations");

            migrationBuilder.AlterColumn<string>(
                name: "User_Id",
                table: "UserReservations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserReservations_Users_User_Id",
                table: "UserReservations",
                column: "User_Id",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReservations_Users_User_Id",
                table: "UserReservations");

            migrationBuilder.AlterColumn<string>(
                name: "User_Id",
                table: "UserReservations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReservations_Users_User_Id",
                table: "UserReservations",
                column: "User_Id",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
