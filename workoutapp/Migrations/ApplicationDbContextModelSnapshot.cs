﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using workoutapp.DAL;

#nullable disable

namespace workoutapp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("workoutapp.Models.Calendar", b =>
                {
                    b.Property<int>("CalendarId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CalendarId"));

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("CalendarId");

                    b.HasIndex("UserId");

                    b.ToTable("Calendars");
                });

            modelBuilder.Entity("workoutapp.Models.CalendarDay", b =>
                {
                    b.Property<int>("CalendarDayId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CalendarDayId"));

                    b.Property<string>("CalendarDate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CalendarId")
                        .HasColumnType("integer");

                    b.HasKey("CalendarDayId");

                    b.HasIndex("CalendarId");

                    b.ToTable("CalendarDays");
                });

            modelBuilder.Entity("workoutapp.Models.Exercise", b =>
                {
                    b.Property<int>("ExerciseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ExerciseId"));

                    b.Property<string>("BodyPart")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ExerciseName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("NumberOfLoad")
                        .HasColumnType("integer");

                    b.Property<int?>("NumberOfRepeats")
                        .HasColumnType("integer");

                    b.Property<int?>("NumberOfSeries")
                        .HasColumnType("integer");

                    b.HasKey("ExerciseId");

                    b.ToTable("Exercises");

                    b.HasData(
                        new
                        {
                            ExerciseId = 1,
                            BodyPart = "Plecy",
                            Description = "Podciąganie na drążku to ćwiczenie wykonywane przy użyciu wagi ciała, polegające na chwyceniu drążka nad głową, dłonie skierowane na zewnątrz. Poprzez unoszenie ciała ku górze skupiasz się na mięśniach górnej części pleców, szczególnie na mięśniu czworobocznym. Ćwiczenie to również angażuje bicepsy i mięśnie brzucha, zapewniając wszechstronny trening górnej części ciała.",
                            ExerciseName = "Podciąganie na drążku"
                        },
                        new
                        {
                            ExerciseId = 2,
                            BodyPart = "Klatka piersiowa",
                            Description = "Wyciskanie sztangi to ruch wykonywany przy użyciu ciężarów. Sztanga startuje na kołkach bezpieczeństwa, co dodaje wyzwań związanych z bezruchem. Unoszenie i wyciskanie wzmacnia mięśnie klatki piersiowej, tricepsów i ramion. Ćwiczenie to koncentruje się na osłabionych punktach w wyciskaniu sztangi.",
                            ExerciseName = "Wyciskanie sztangi"
                        },
                        new
                        {
                            ExerciseId = 3,
                            BodyPart = "Barki",
                            Description = "Podciąganie liny do twarzy to ćwiczenie wykonywane przy użyciu maszyny z linką i końcówką w kształcie liny. Pociągnij linkę w kierunku twarzy, trzymając łokcie wysoko, aby zaangażować mięśnie naramienne tyłowe oraz górną część pleców. Ćwiczenie to poprawia stabilność ramion i postawę ciała.",
                            ExerciseName = "Podciąganie liny do twarzy"
                        },
                        new
                        {
                            ExerciseId = 4,
                            BodyPart = "Mięsień trójgłowy ramienia",
                            Description = "Połączenie ćwiczeń Skull Crusher i Pullover. Zacznij od wyciągnięcia sztangi na wyciskanie nad czoło (skull crusher), a następnie przenieś ją nad głowę (pullover). Zaangażowana zostaje trójgłowa część ramienia, klatka piersiowa i mięśnie pleców, zapewniając kompleksowy trening górnej części ciała.",
                            ExerciseName = "Skull crusher i pullover"
                        },
                        new
                        {
                            ExerciseId = 5,
                            BodyPart = "Biceps",
                            Description = "Uginanie ramion ze sztangą to klasyczne ćwiczenie wzmacniające mięśnie ramion. Trzymaj sztangę podchwytem, dłonie skierowane do góry, i unosząc ją ku górze, wykorzystaj mięśnie bicepasa. Opuszczaj sztangę z kontrolą. Ćwiczenie to koncentruje się na mięśniach bicepsa, pomagając rozwijać ich definicję i siłę.",
                            ExerciseName = "Uginanie ramion ze sztangą"
                        },
                        new
                        {
                            ExerciseId = 6,
                            BodyPart = "Brzuch",
                            Description = "Plank na piłce szwajcarskiej to efektywne ćwiczenie dla mięśni brzucha. Rozpocznij w pozycji plank na przedramionach, piłka szwajcarska pod brzuchem, a palce stóp na podłodze. Utrzymuj prostą linię od głowy do pięt, angażując mięśnie brzucha i stabilizujące. Ćwiczenie to pomaga poprawić siłę, stabilność i równowagę mięśni brzucha.",
                            ExerciseName = "Plank na piłce szwajcarskiej"
                        },
                        new
                        {
                            ExerciseId = 7,
                            BodyPart = "Łydki",
                            Description = "Podnoszenie pięt ze sztangą w staniu to ćwiczenie wzmacniające łydki. Stań prosto, ze sztangą opierającą się na górnej części pleców. Wstawaj na palce poprzez unoszenie pięt i przesuwanie wagi ciała na przednią część stóp. Opuszczaj pięty z kontrolą, wykonując pełen zakres ruchu. Ćwiczenie to koncentruje się na mięśniach łydek, poprawiając ich siłę i wyrazistość.",
                            ExerciseName = "Podnoszenie pięt ze sztangą w staniu"
                        },
                        new
                        {
                            ExerciseId = 8,
                            BodyPart = "Czworogłowe uda",
                            Description = "Wypady z sztangą nad plecami to ćwiczenie dolnej partii ciała. Trzymając sztangę na górnej części pleców, cofnij się jedną nogą do wypadu, obniżając kolano tylnej nogi w kierunku podłoża. Napnij przednią nogę, aby wrócić do pozycji wyjściowej. Ćwiczenie to angażuje mięśnie czworogłowe uda, mięśnie dwugłowe uda i pośladki, poprawiając siłę i stabilność nóg.",
                            ExerciseName = "Wypady z sztangą nad plecami"
                        },
                        new
                        {
                            ExerciseId = 9,
                            BodyPart = "Dolny odcinek pleców",
                            Description = "Martwy ciąg na podwyższeniu to ćwiczenie z ciężarami, polegające na podnoszeniu sztangi z podwyższenia. Stań na podkładkach lub blokach, chwytając sztangę podchwytem, i unosząc ją poprzez wyprostowanie bioder i kolan. Opuszczaj sztangę z kontrolą. Ćwiczenie to angażuje mięśnie dwugłowe uda, pośladki, dolny odcinek pleców i mięśnie brzucha, wspierając ogólną siłę i rozwój mięśniowy.",
                            ExerciseName = "Martwy ciąg na podwyższeniu"
                        },
                        new
                        {
                            ExerciseId = 10,
                            BodyPart = "Biceps",
                            Description = "Uginanie ramion z hantlami w supinacji to ćwiczenie wzmacniające mięśnie bicepsa. Trzymaj hantle w dłoniach ze zwiniętymi do góry dłońmi (supinacja) i unieś je ku górze, skręcając bicepsy. Opuszczaj hantle z kontrolą. Ćwiczenie to efektywnie izoluje i rozwija mięśnie bicepsa, poprawiając siłę i definicję ramion.",
                            ExerciseName = "Uginanie ramion z hantlami w supinacji"
                        });
                });

            modelBuilder.Entity("workoutapp.Models.Meal", b =>
                {
                    b.Property<int>("MealId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("MealId"));

                    b.Property<int>("CalendarDayId")
                        .HasColumnType("integer");

                    b.Property<string>("MealName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<float>("TotalCarbs")
                        .HasColumnType("real");

                    b.Property<float>("TotalFat")
                        .HasColumnType("real");

                    b.Property<int>("TotalKcal")
                        .HasColumnType("integer");

                    b.Property<float>("TotalProtein")
                        .HasColumnType("real");

                    b.HasKey("MealId");

                    b.HasIndex("CalendarDayId");

                    b.ToTable("Meals");
                });

            modelBuilder.Entity("workoutapp.Models.Note", b =>
                {
                    b.Property<int>("NoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("NoteId"));

                    b.Property<string>("NoteName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("WorkoutPlanId")
                        .HasColumnType("integer");

                    b.HasKey("NoteId");

                    b.HasIndex("WorkoutPlanId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("workoutapp.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductId"));

                    b.Property<int>("MealId")
                        .HasColumnType("integer");

                    b.Property<float>("ProductCarbs")
                        .HasColumnType("real");

                    b.Property<int>("ProductCategoryId")
                        .HasColumnType("integer");

                    b.Property<string>("ProductCategoryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("ProductFat")
                        .HasColumnType("real");

                    b.Property<int>("ProductKcal")
                        .HasColumnType("integer");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("ProductProtein")
                        .HasColumnType("real");

                    b.Property<int>("ProductWeight")
                        .HasColumnType("integer");

                    b.HasKey("ProductId");

                    b.HasIndex("MealId");

                    b.HasIndex("ProductCategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("workoutapp.Models.ProductCategory", b =>
                {
                    b.Property<int>("ProductCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductCategoryId"));

                    b.Property<string>("ProductCategoryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ProductCategoryId");

                    b.ToTable("ProductCategories");

                    b.HasData(
                        new
                        {
                            ProductCategoryId = 1,
                            ProductCategoryName = "Owoce"
                        },
                        new
                        {
                            ProductCategoryId = 2,
                            ProductCategoryName = "Warzywa"
                        },
                        new
                        {
                            ProductCategoryId = 3,
                            ProductCategoryName = "Mięso"
                        },
                        new
                        {
                            ProductCategoryId = 4,
                            ProductCategoryName = "Nabiał"
                        },
                        new
                        {
                            ProductCategoryId = 5,
                            ProductCategoryName = "Pieczywo"
                        },
                        new
                        {
                            ProductCategoryId = 6,
                            ProductCategoryName = "Produkty zbożowe"
                        },
                        new
                        {
                            ProductCategoryId = 7,
                            ProductCategoryName = "Ryby i owoce morza"
                        },
                        new
                        {
                            ProductCategoryId = 8,
                            ProductCategoryName = "Słodycze"
                        },
                        new
                        {
                            ProductCategoryId = 9,
                            ProductCategoryName = "Napoje"
                        },
                        new
                        {
                            ProductCategoryId = 10,
                            ProductCategoryName = "Sosy i przyprawy"
                        },
                        new
                        {
                            ProductCategoryId = 11,
                            ProductCategoryName = "Przekąski"
                        },
                        new
                        {
                            ProductCategoryId = 12,
                            ProductCategoryName = "Zupy"
                        },
                        new
                        {
                            ProductCategoryId = 13,
                            ProductCategoryName = "Napoje alkoholowe"
                        },
                        new
                        {
                            ProductCategoryId = 14,
                            ProductCategoryName = "Produkty bezglutenowe"
                        },
                        new
                        {
                            ProductCategoryId = 15,
                            ProductCategoryName = "Kawy i herbaty"
                        },
                        new
                        {
                            ProductCategoryId = 16,
                            ProductCategoryName = "Inne"
                        });
                });

            modelBuilder.Entity("workoutapp.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("workoutapp.Models.UserExercise", b =>
                {
                    b.Property<int>("UserExerciseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserExerciseId"));

                    b.Property<string>("BodyPart")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ExerciseName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("NumberOfLoad")
                        .HasColumnType("integer");

                    b.Property<int?>("NumberOfRepeats")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int?>("NumberOfSeries")
                        .IsRequired()
                        .HasColumnType("integer");

                    b.Property<int>("WorkoutDayId")
                        .HasColumnType("integer");

                    b.HasKey("UserExerciseId");

                    b.HasIndex("WorkoutDayId");

                    b.ToTable("UserExercises");
                });

            modelBuilder.Entity("workoutapp.Models.WorkoutDay", b =>
                {
                    b.Property<int>("WorkoutDayId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("WorkoutDayId"));

                    b.Property<string>("CalendarDate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CalendarDayId")
                        .HasColumnType("integer");

                    b.Property<int>("WorkoutPlanId")
                        .HasColumnType("integer");

                    b.HasKey("WorkoutDayId");

                    b.HasIndex("CalendarDayId")
                        .IsUnique();

                    b.HasIndex("WorkoutPlanId");

                    b.ToTable("WorkoutDays");
                });

            modelBuilder.Entity("workoutapp.Models.WorkoutPlan", b =>
                {
                    b.Property<int>("WorkoutPlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("WorkoutPlanId"));

                    b.Property<long>("DaysCount")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<bool?>("isPreferred")
                        .HasColumnType("boolean");

                    b.HasKey("WorkoutPlanId");

                    b.HasIndex("UserId");

                    b.ToTable("WorkoutPlans");
                });

            modelBuilder.Entity("workoutapp.Models.Calendar", b =>
                {
                    b.HasOne("workoutapp.Models.User", "User")
                        .WithMany("Calendars")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("workoutapp.Models.CalendarDay", b =>
                {
                    b.HasOne("workoutapp.Models.Calendar", "Calendar")
                        .WithMany("CalendarDays")
                        .HasForeignKey("CalendarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Calendar");
                });

            modelBuilder.Entity("workoutapp.Models.Meal", b =>
                {
                    b.HasOne("workoutapp.Models.CalendarDay", "CalendarDay")
                        .WithMany("Meals")
                        .HasForeignKey("CalendarDayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CalendarDay");
                });

            modelBuilder.Entity("workoutapp.Models.Note", b =>
                {
                    b.HasOne("workoutapp.Models.WorkoutPlan", "WorkoutPlan")
                        .WithMany("Notes")
                        .HasForeignKey("WorkoutPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkoutPlan");
                });

            modelBuilder.Entity("workoutapp.Models.Product", b =>
                {
                    b.HasOne("workoutapp.Models.Meal", "Meal")
                        .WithMany("Products")
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("workoutapp.Models.ProductCategory", "ProductCategory")
                        .WithMany()
                        .HasForeignKey("ProductCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meal");

                    b.Navigation("ProductCategory");
                });

            modelBuilder.Entity("workoutapp.Models.UserExercise", b =>
                {
                    b.HasOne("workoutapp.Models.WorkoutDay", "WorkoutDay")
                        .WithMany("UserExercises")
                        .HasForeignKey("WorkoutDayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkoutDay");
                });

            modelBuilder.Entity("workoutapp.Models.WorkoutDay", b =>
                {
                    b.HasOne("workoutapp.Models.CalendarDay", "CalendarDay")
                        .WithOne("WorkoutDay")
                        .HasForeignKey("workoutapp.Models.WorkoutDay", "CalendarDayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("workoutapp.Models.WorkoutPlan", "WorkoutPlan")
                        .WithMany("WorkoutDays")
                        .HasForeignKey("WorkoutPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CalendarDay");

                    b.Navigation("WorkoutPlan");
                });

            modelBuilder.Entity("workoutapp.Models.WorkoutPlan", b =>
                {
                    b.HasOne("workoutapp.Models.User", "User")
                        .WithMany("WorkoutPlans")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("workoutapp.Models.Calendar", b =>
                {
                    b.Navigation("CalendarDays");
                });

            modelBuilder.Entity("workoutapp.Models.CalendarDay", b =>
                {
                    b.Navigation("Meals");

                    b.Navigation("WorkoutDay")
                        .IsRequired();
                });

            modelBuilder.Entity("workoutapp.Models.Meal", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("workoutapp.Models.User", b =>
                {
                    b.Navigation("Calendars");

                    b.Navigation("WorkoutPlans");
                });

            modelBuilder.Entity("workoutapp.Models.WorkoutDay", b =>
                {
                    b.Navigation("UserExercises");
                });

            modelBuilder.Entity("workoutapp.Models.WorkoutPlan", b =>
                {
                    b.Navigation("Notes");

                    b.Navigation("WorkoutDays");
                });
#pragma warning restore 612, 618
        }
    }
}
