using Microsoft.EntityFrameworkCore.Migrations;

namespace graduationProject.Data.Migrations
{
    public partial class editoxegintable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "OxygenTubes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OxygenTubes_UserId",
                table: "OxygenTubes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OxygenTubes_Users_UserId",
                table: "OxygenTubes",
                column: "UserId",
                principalSchema: "security",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OxygenTubes_Users_UserId",
                table: "OxygenTubes");

            migrationBuilder.DropIndex(
                name: "IX_OxygenTubes_UserId",
                table: "OxygenTubes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OxygenTubes");
        }
    }
}
