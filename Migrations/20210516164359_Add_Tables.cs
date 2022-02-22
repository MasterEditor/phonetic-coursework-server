using Microsoft.EntityFrameworkCore.Migrations;

namespace Coursework_Server.Migrations
{
    public partial class Add_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consumption_Users_UserId",
                table: "Consumption");

            migrationBuilder.DropForeignKey(
                name: "FK_Operation_Users_UserId",
                table: "Operation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Operation",
                table: "Operation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Consumption",
                table: "Consumption");

            migrationBuilder.RenameTable(
                name: "Operation",
                newName: "Operations");

            migrationBuilder.RenameTable(
                name: "Consumption",
                newName: "Consumptions");

            migrationBuilder.RenameIndex(
                name: "IX_Operation_UserId",
                table: "Operations",
                newName: "IX_Operations_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Consumption_UserId",
                table: "Consumptions",
                newName: "IX_Consumptions_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Operations",
                table: "Operations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Consumptions",
                table: "Consumptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consumptions_Users_UserId",
                table: "Consumptions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_Users_UserId",
                table: "Operations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consumptions_Users_UserId",
                table: "Consumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Operations_Users_UserId",
                table: "Operations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Operations",
                table: "Operations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Consumptions",
                table: "Consumptions");

            migrationBuilder.RenameTable(
                name: "Operations",
                newName: "Operation");

            migrationBuilder.RenameTable(
                name: "Consumptions",
                newName: "Consumption");

            migrationBuilder.RenameIndex(
                name: "IX_Operations_UserId",
                table: "Operation",
                newName: "IX_Operation_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Consumptions_UserId",
                table: "Consumption",
                newName: "IX_Consumption_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Operation",
                table: "Operation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Consumption",
                table: "Consumption",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consumption_Users_UserId",
                table: "Consumption",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Operation_Users_UserId",
                table: "Operation",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
