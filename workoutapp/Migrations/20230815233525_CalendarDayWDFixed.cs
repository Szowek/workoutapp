using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workoutapp.Migrations
{
    public partial class CalendarDayWDFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "WorkoutDays",
                newName: "CalendarDate");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "CalendarDays",
                newName: "CalendarDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CalendarDate",
                table: "WorkoutDays",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CalendarDate",
                table: "CalendarDays",
                newName: "Date");
        }
    }
}
