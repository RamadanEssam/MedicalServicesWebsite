using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace graduationProject.Data.Migrations
{
    public partial class editimagehospital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "hos_pic",
                table: "Hospitals",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hos_pic",
                table: "Hospitals");
        }
    }
}
