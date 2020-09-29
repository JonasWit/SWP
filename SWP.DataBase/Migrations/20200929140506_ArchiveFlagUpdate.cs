using Microsoft.EntityFrameworkCore.Migrations;

namespace SWP.DataBase.Migrations
{
    public partial class ArchiveFlagUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "TimeRecords");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "CashMovements");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Appointments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "TimeRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Reminders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "CashMovements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
