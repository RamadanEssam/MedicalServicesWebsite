using Microsoft.EntityFrameworkCore.Migrations;

namespace graduationProject.Data.Migrations
{
    public partial class addstatustouser3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status",
                schema: "security",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                schema: "security",
                table: "Users");
        }
    }
}
