using Microsoft.EntityFrameworkCore.Migrations;

namespace Coursework_Server.Migrations
{
    public partial class Add_Tariffs_Type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Tariffs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Tariffs");
        }
    }
}
