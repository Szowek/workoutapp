using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workoutapp.Migrations
{
    public partial class UserExerciseFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserExercises_Users_CreatedById",
                table: "UserExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExercises_WorkoutPlans_WorkoutPlanId",
                table: "UserExercises");

            migrationBuilder.DropIndex(
                name: "IX_UserExercises_CreatedById",
                table: "UserExercises");

            migrationBuilder.DropIndex(
                name: "IX_UserExercises_WorkoutPlanId",
                table: "UserExercises");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "UserExercises");

            migrationBuilder.DropColumn(
                name: "WorkoutPlanId",
                table: "UserExercises");

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfSeries",
                table: "UserExercises",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfRepeats",
                table: "UserExercises",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_UserExercises_WorkoutDayId",
                table: "UserExercises",
                column: "WorkoutDayId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExercises_WorkoutDays_WorkoutDayId",
                table: "UserExercises",
                column: "WorkoutDayId",
                principalTable: "WorkoutDays",
                principalColumn: "WorkoutDayId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserExercises_WorkoutDays_WorkoutDayId",
                table: "UserExercises");

            migrationBuilder.DropIndex(
                name: "IX_UserExercises_WorkoutDayId",
                table: "UserExercises");

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfSeries",
                table: "UserExercises",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfRepeats",
                table: "UserExercises",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "UserExercises",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkoutPlanId",
                table: "UserExercises",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserExercises_CreatedById",
                table: "UserExercises",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserExercises_WorkoutPlanId",
                table: "UserExercises",
                column: "WorkoutPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExercises_Users_CreatedById",
                table: "UserExercises",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExercises_WorkoutPlans_WorkoutPlanId",
                table: "UserExercises",
                column: "WorkoutPlanId",
                principalTable: "WorkoutPlans",
                principalColumn: "WorkoutPlanId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
