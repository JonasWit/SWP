using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SWP.DataBase.Migrations
{
    public partial class Log : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogRecords_LicensedUsers_UserDataId",
                table: "LogRecords");

            migrationBuilder.DropTable(
                name: "License");

            migrationBuilder.DropTable(
                name: "LicensedUsers");

            migrationBuilder.DropIndex(
                name: "IX_LogRecords_UserDataId",
                table: "LogRecords");

            migrationBuilder.DropColumn(
                name: "UserDataId",
                table: "LogRecords");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "LogRecords",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "StackTrace",
                table: "LogRecords",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LogRecords",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StackTrace",
                table: "LogRecords");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LogRecords");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "LogRecords",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserDataId",
                table: "LogRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LicensedUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicensedUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "License",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LicenseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_License", x => x.Id);
                    table.ForeignKey(
                        name: "FK_License_LicensedUsers_UserDataId",
                        column: x => x.UserDataId,
                        principalTable: "LicensedUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogRecords_UserDataId",
                table: "LogRecords",
                column: "UserDataId");

            migrationBuilder.CreateIndex(
                name: "IX_License_UserDataId",
                table: "License",
                column: "UserDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogRecords_LicensedUsers_UserDataId",
                table: "LogRecords",
                column: "UserDataId",
                principalTable: "LicensedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
