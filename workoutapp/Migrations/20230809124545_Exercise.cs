using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workoutapp.Migrations
{
    public partial class Exercise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_WorkoutDays_WorkoutDayId",
                table: "Exercise");

            migrationBuilder.AlterColumn<int>(
                name: "WorkoutDayId",
                table: "Exercise",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BodyPart",
                table: "Exercise",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Exercise",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Exercise",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExerciseName",
                table: "Exercise",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRepeats",
                table: "Exercise",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfSeries",
                table: "Exercise",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkoutPlanId",
                table: "Exercise",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_WorkoutPlanId",
                table: "Exercise",
                column: "WorkoutPlanId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_WorkoutDays_WorkoutDayId",
                table: "Exercise");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_WorkoutPlans_WorkoutPlanId",
                table: "Exercise");

            migrationBuilder.DropIndex(
                name: "IX_Exercise_WorkoutPlanId",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "BodyPart",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "ExerciseName",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "NumberOfRepeats",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "NumberOfSeries",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "WorkoutPlanId",
                table: "Exercise");

            migrationBuilder.AlterColumn<int>(
                name: "WorkoutDayId",
                table: "Exercise",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_WorkoutDays_WorkoutDayId",
                table: "Exercise",
                column: "WorkoutDayId",
                principalTable: "WorkoutDays",
                principalColumn: "WorkoutDayId");
        }
    }
}
