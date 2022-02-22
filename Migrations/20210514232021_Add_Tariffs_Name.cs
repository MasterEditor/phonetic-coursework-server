using Microsoft.EntityFrameworkCore.Migrations;

namespace Coursework_Server.Migrations
{
    public partial class Add_Tariffs_Name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Tariffs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Tariffs");
        }
    }
}
