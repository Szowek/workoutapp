using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workoutapp.Migrations
{
    public partial class CreatedExercise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_WorkoutDays_WorkoutDayId",
                table: "Exercise");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_WorkoutPlans_WorkoutPlanId",
                table: "Exercise");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercise",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Exercise");

            migrationBuilder.RenameTable(
                name: "Exercise",
                newName: "Exercises");

            migrationBuilder.RenameIndex(
                name: "IX_Exercise_WorkoutPlanId",
                table: "Exercises",
                newName: "IX_Exercises_WorkoutPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Exercise_WorkoutDayId",
                table: "Exercises",
                newName: "IX_Exercises_WorkoutDayId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises",
                column: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_WorkoutDays_WorkoutDayId",
                table: "Exercises",
                column: "WorkoutDayId",
                principalTable: "WorkoutDays",
                principalColumn: "WorkoutDayId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_WorkoutPlans_WorkoutPlanId",
                table: "Exercises",
                column: "WorkoutPlanId",
                principalTable: "WorkoutPlans",
                principalColumn: "WorkoutPlanId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_WorkoutDays_WorkoutDayId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_WorkoutPlans_WorkoutPlanId",
                table: "Exercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises");

            migrationBuilder.RenameTable(
                name: "Exercises",
                newName: "Exercise");

            migrationBuilder.RenameIndex(
                name: "IX_Exercises_WorkoutPlanId",
                table: "Exercise",
                newName: "IX_Exercise_WorkoutPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Exercises_WorkoutDayId",
                table: "Exercise",
                newName: "IX_Exercise_WorkoutDayId");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Exercise",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercise",
                table: "Exercise",
                column: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_WorkoutDays_WorkoutDayId",
                table: "Exercise",
                column: "WorkoutDayId",
                principalTable: "WorkoutDays",
                principalColumn: "WorkoutDayId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_WorkoutPlans_WorkoutPlanId",
                table: "Exercise",
                column: "WorkoutPlanId",
                principalTable: "WorkoutPlans",
                principalColumn: "WorkoutPlanId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
