using Microsoft.EntityFrameworkCore.Migrations;

namespace SWP.DataBase.Migrations
{
    public partial class ArchiveFlagAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "TimeRecords",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Patients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "CashMovements",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Appointments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "TimeRecords");

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
    }
}
