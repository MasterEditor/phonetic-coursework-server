using Microsoft.EntityFrameworkCore.Migrations;

namespace Coursework_Server.Migrations
{
    public partial class Add_Services_Correctly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceUser");

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "UserTariffs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsedMinutes = table.Column<int>(type: "int", nullable: false),
                    UsedInternet = table.Column<int>(type: "int", nullable: false),
                    UsedSMS = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserService_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserService_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTariffs_ServiceId",
                table: "UserTariffs",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserService_ServiceId",
                table: "UserService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserService_UserId",
                table: "UserService",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTariffs_Service_ServiceId",
                table: "UserTariffs",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTariffs_Service_ServiceId",
                table: "UserTariffs");

            migrationBuilder.DropTable(
                name: "UserService");

            migrationBuilder.DropIndex(
                name: "IX_UserTariffs_ServiceId",
                table: "UserTariffs");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "UserTariffs");

            migrationBuilder.CreateTable(
                name: "ServiceUser",
                columns: table => new
                {
                    ServicesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceUser", x => new { x.ServicesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ServiceUser_Service_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceUser_UsersId",
                table: "ServiceUser",
                column: "UsersId");
        }
    }
}
