using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workoutapp.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkoutDays_CalendarDate",
                table: "WorkoutDays");

            migrationBuilder.DropIndex(
                name: "IX_CalendarDays_CalendarDate",
                table: "CalendarDays");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDays_CalendarDate",
                table: "WorkoutDays",
                column: "CalendarDate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalendarDays_CalendarDate",
                table: "CalendarDays",
                column: "CalendarDate",
                unique: true);
        }
    }
}
