using Microsoft.EntityFrameworkCore.Migrations;

namespace graduationProject.Data.Migrations
{
    public partial class edituserreservation1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReservations_Users_User_Id",
                table: "UserReservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserReservations",
                table: "UserReservations");

            migrationBuilder.AlterColumn<string>(
                name: "User_Id",
                table: "UserReservations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserReservations",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserReservations",
                table: "UserReservations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserReservations_User_Id",
                table: "UserReservations",
                column: "User_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReservations_Users_User_Id",
                table: "UserReservations",
                column: "User_Id",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReservations_Users_User_Id",
                table: "UserReservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserReservations",
                table: "UserReservations");

            migrationBuilder.DropIndex(
                name: "IX_UserReservations_User_Id",
                table: "UserReservations");

            migrationBuilder.DropColumn(
                name: "Id",
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

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserReservations",
                table: "UserReservations",
                columns: new[] { "User_Id", "Hos_Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserReservations_Users_User_Id",
                table: "UserReservations",
                column: "User_Id",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
