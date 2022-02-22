using Microsoft.EntityFrameworkCore.Migrations;

namespace Coursework_Server.Migrations
{
    public partial class Add_Services_More_Correctly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserService_Service_ServiceId",
                table: "UserService");

            migrationBuilder.DropForeignKey(
                name: "FK_UserService_Users_UserId",
                table: "UserService");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTariffs_Service_ServiceId",
                table: "UserTariffs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserService",
                table: "UserService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Service",
                table: "Service");

            migrationBuilder.RenameTable(
                name: "UserService",
                newName: "UserServices");

            migrationBuilder.RenameTable(
                name: "Service",
                newName: "Services");

            migrationBuilder.RenameIndex(
                name: "IX_UserService_UserId",
                table: "UserServices",
                newName: "IX_UserServices_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserService_ServiceId",
                table: "UserServices",
                newName: "IX_UserServices_ServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserServices",
                table: "UserServices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Services",
                table: "Services",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserServices_Services_ServiceId",
                table: "UserServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserServices_Users_UserId",
                table: "UserServices",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTariffs_Services_ServiceId",
                table: "UserTariffs",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserServices_Services_ServiceId",
                table: "UserServices");

            migrationBuilder.DropForeignKey(
                name: "FK_UserServices_Users_UserId",
                table: "UserServices");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTariffs_Services_ServiceId",
                table: "UserTariffs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserServices",
                table: "UserServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Services",
                table: "Services");

            migrationBuilder.RenameTable(
                name: "UserServices",
                newName: "UserService");

            migrationBuilder.RenameTable(
                name: "Services",
                newName: "Service");

            migrationBuilder.RenameIndex(
                name: "IX_UserServices_UserId",
                table: "UserService",
                newName: "IX_UserService_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserServices_ServiceId",
                table: "UserService",
                newName: "IX_UserService_ServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserService",
                table: "UserService",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Service",
                table: "Service",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserService_Service_ServiceId",
                table: "UserService",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserService_Users_UserId",
                table: "UserService",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTariffs_Service_ServiceId",
                table: "UserTariffs",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
