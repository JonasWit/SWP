using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SWP.DataBase.Migrations
{
    public partial class TimespanRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordedTime",
                table: "TimeRecords");

            migrationBuilder.AddColumn<int>(
                name: "Hours",
                table: "TimeRecords",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Minutes",
                table: "TimeRecords",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hours",
                table: "TimeRecords");

            migrationBuilder.DropColumn(
                name: "Minutes",
                table: "TimeRecords");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "RecordedTime",
                table: "TimeRecords",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
