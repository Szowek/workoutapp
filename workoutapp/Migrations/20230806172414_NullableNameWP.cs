using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workoutapp.Migrations
{
    public partial class NullableNameWP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "WorkoutPlans",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "WorkoutPlans");
        }
    }
}
