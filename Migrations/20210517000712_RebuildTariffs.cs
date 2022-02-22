using Microsoft.EntityFrameworkCore.Migrations;

namespace Coursework_Server.Migrations
{
    public partial class RebuildTariffs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tariffs_TariffId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TariffId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TariffId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserTariffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsedMinutes = table.Column<int>(type: "int", nullable: false),
                    UsedInternet = table.Column<int>(type: "int", nullable: false),
                    UsedSMS = table.Column<int>(type: "int", nullable: false),
                    TariffId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTariffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTariffs_Tariffs_TariffId",
                        column: x => x.TariffId,
                        principalTable: "Tariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTariffs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTariffs_TariffId",
                table: "UserTariffs",
                column: "TariffId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTariffs_UserId",
                table: "UserTariffs",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTariffs");

            migrationBuilder.AddColumn<int>(
                name: "TariffId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TariffId",
                table: "Users",
                column: "TariffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tariffs_TariffId",
                table: "Users",
                column: "TariffId",
                principalTable: "Tariffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
