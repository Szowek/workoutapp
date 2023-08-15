using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace workoutapp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExerciseName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    BodyPart = table.Column<string>(type: "text", nullable: false),
                    NumberOfSeries = table.Column<int>(type: "integer", nullable: true),
                    NumberOfRepeats = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.ExerciseId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    CalendarId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.CalendarId);
                    table.ForeignKey(
                        name: "FK_Calendars_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutPlans",
                columns: table => new
                {
                    WorkoutPlanId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    isPreferred = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutPlans", x => x.WorkoutPlanId);
                    table.ForeignKey(
                        name: "FK_WorkoutPlans_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    NoteId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NoteName = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    WorkoutPlanId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_Notes_WorkoutPlans_WorkoutPlanId",
                        column: x => x.WorkoutPlanId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "WorkoutPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutDays",
                columns: table => new
                {
                    WorkoutDayId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkoutPlanId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutDays", x => x.WorkoutDayId);
                    table.ForeignKey(
                        name: "FK_WorkoutDays_WorkoutPlans_WorkoutPlanId",
                        column: x => x.WorkoutPlanId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "WorkoutPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalendarDays",
                columns: table => new
                {
                    CalendarDayId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CalendarDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CalendarId = table.Column<int>(type: "integer", nullable: false),
                    WorkoutDayId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarDays", x => x.CalendarDayId);
                    table.ForeignKey(
                        name: "FK_CalendarDays_Calendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendars",
                        principalColumn: "CalendarId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalendarDays_WorkoutDays_WorkoutDayId",
                        column: x => x.WorkoutDayId,
                        principalTable: "WorkoutDays",
                        principalColumn: "WorkoutDayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserExercises",
                columns: table => new
                {
                    UserExerciseId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExerciseName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    BodyPart = table.Column<string>(type: "text", nullable: false),
                    NumberOfSeries = table.Column<int>(type: "integer", nullable: true),
                    NumberOfRepeats = table.Column<int>(type: "integer", nullable: true),
                    WorkoutDayId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExercises", x => x.UserExerciseId);
                    table.ForeignKey(
                        name: "FK_UserExercises_WorkoutDays_WorkoutDayId",
                        column: x => x.WorkoutDayId,
                        principalTable: "WorkoutDays",
                        principalColumn: "WorkoutDayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "ExerciseId", "BodyPart", "Description", "ExerciseName", "NumberOfRepeats", "NumberOfSeries" },
                values: new object[,]
                {
                    { 1, "Shoulders", "Pull-ups are a bodyweight exercise where you grip an overhead bar with palms facing away. By pulling your body upward, you target your upper back muscles, especially the latissimus dorsi. This exercise also engages your biceps and core, providing a well-rounded upper body workout.", "Pull ups", null, null },
                    { 2, "Chest", "Pin bench press is a weightlifting move. Barbell starts on safety pins, adding dead-stop challenge. Lift and press for chest, triceps, and shoulder strength. Targets bench press weak points.", "Pin bench press", null, null },
                    { 3, "Shoulders", "Face pull is an exercise using a cable machine and rope attachment. Pull the rope towards your face, keeping elbows high, to work rear deltoids and upper back. Enhances shoulder stability and posture.", "Face pull", null, null },
                    { 4, "Triceps", "Skull Crusher Pullover combines two exercises. Begin with a weightlifting barbell, lower it to your forehead (skull crusher), then move it over and behind your head (pullover). This engages triceps, chest, and lats for a comprehensive upper body workout.", "Skull crusher pullover", null, null },
                    { 5, "Biceps", "Barbell biceps curl is a classic exercise for building arm strength. Hold a barbell with an underhand grip, palms facing up, and curl it upward using your biceps. Lower the barbell with control. This targets the biceps muscles, helping to develop arm definition and strength.", "Barbell biceps curl", null, null },
                    { 6, "Core", "The Swiss ball plank is an effective core exercise. Start in a plank position with your forearms on a Swiss ball and toes on the ground. Maintain a straight line from head to heels, engaging your core and stabilizing muscles. This exercise helps improve core strength, stability, and balance.\"", "Swiss ball plank", null, null },
                    { 7, "Calf", "The standing barbell calf raise is a calf-strengthening exercise. Stand upright with a barbell resting on your upper back. Rise onto your toes by pushing through the balls of your feet, lifting your heels as high as possible. Lower your heels back down for a full range of motion. This exercise targets the calf muscles, enhancing lower leg strength and definition.", "Standing barbell calf raise", null, null },
                    { 8, "Quadriceps thighs ", "Reverse lunges with a barbell are a lower body exercise. Hold a barbell on your upper back, step back with one leg into a lunge, lowering your back knee toward the ground. Push through your front heel to return to the standing position. This targets the quadriceps, hamstrings, and glutes, enhancing leg strength and stability.", "Reverse lunges with barbell", null, null },
                    { 9, "Lower back", "The elevated deadlift is a weightlifting exercise that involves lifting a barbell from an elevated platform. Stand on weight plates or blocks, grasp the barbell with an overhand grip, and lift by extending your hips and knees. Lower the barbell back down with control. This exercise targets the hamstrings, glutes, lower back, and core, promoting overall strength and muscle development.", "Elevated deadlift", null, null },
                    { 10, "Biceps", "Dumbbell supinated biceps curls are a biceps-strengthening exercise. Hold dumbbells with palms facing up (supinated grip) and curl them upward while contracting your biceps. Lower the dumbbells with control. This exercise effectively isolates and builds the biceps muscles, enhancing arm strength and definition.", "Dumbbell supinated biceps curls", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarDays_CalendarId",
                table: "CalendarDays",
                column: "CalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarDays_WorkoutDayId",
                table: "CalendarDays",
                column: "WorkoutDayId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_UserId",
                table: "Calendars",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_WorkoutPlanId",
                table: "Notes",
                column: "WorkoutPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExercises_WorkoutDayId",
                table: "UserExercises",
                column: "WorkoutDayId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDays_WorkoutPlanId",
                table: "WorkoutDays",
                column: "WorkoutPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlans_UserId",
                table: "WorkoutPlans",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarDays");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "UserExercises");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "WorkoutDays");

            migrationBuilder.DropTable(
                name: "WorkoutPlans");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
