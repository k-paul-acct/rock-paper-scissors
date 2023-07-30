﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RockPaperScissors.Api.Data;

#nullable disable

namespace RockPaperScissors.Api.Migrations
{
    [DbContext(typeof(GameContext))]
    [Migration("20230730095248_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.9");

            modelBuilder.Entity("RockPaperScissors.Api.Data.Models.Game", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("RoundsNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoundsPassed")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("RockPaperScissors.Api.Data.Models.Move", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("GameId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("MoveType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PlayerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("RoundIndex")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Moves");
                });

            modelBuilder.Entity("RockPaperScissors.Api.Data.Models.Player", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("GameId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Score")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("RockPaperScissors.Api.Data.Models.RestartRequest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("GameId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.ToTable("RestartRequests");
                });

            modelBuilder.Entity("RockPaperScissors.Api.Data.Models.Move", b =>
                {
                    b.HasOne("RockPaperScissors.Api.Data.Models.Game", "Game")
                        .WithMany("Moves")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RockPaperScissors.Api.Data.Models.Player", "Player")
                        .WithMany("Moves")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("RockPaperScissors.Api.Data.Models.Player", b =>
                {
                    b.HasOne("RockPaperScissors.Api.Data.Models.Game", "Type")
                        .WithMany("Players")
                        .HasForeignKey("GameId");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("RockPaperScissors.Api.Data.Models.RestartRequest", b =>
                {
                    b.HasOne("RockPaperScissors.Api.Data.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RockPaperScissors.Api.Data.Models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("RockPaperScissors.Api.Data.Models.Game", b =>
                {
                    b.Navigation("Moves");

                    b.Navigation("Players");
                });

            modelBuilder.Entity("RockPaperScissors.Api.Data.Models.Player", b =>
                {
                    b.Navigation("Moves");
                });
#pragma warning restore 612, 618
        }
    }
}
