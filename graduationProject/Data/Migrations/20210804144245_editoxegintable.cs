using Microsoft.EntityFrameworkCore.Migrations;

namespace graduationProject.Data.Migrations
{
    public partial class editoxegintable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BloodBanks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BloodBanks_UserId",
                table: "BloodBanks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BloodBanks_Users_UserId",
                table: "BloodBanks",
                column: "UserId",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BloodBanks_Users_UserId",
                table: "BloodBanks");

            migrationBuilder.DropIndex(
                name: "IX_BloodBanks_UserId",
                table: "BloodBanks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BloodBanks");
        }
    }
}
