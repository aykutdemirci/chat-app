﻿// <auto-generated />
using System;
using ChatApp.Db.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChatApp.Db.Migrations
{
    [DbContext(typeof(ChatAppDbContext))]
    partial class ChatAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.13");

            modelBuilder.Entity("ChatApp.Dto.ChatRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ChatRooms");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Default"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Oda-1"
                        });
                });

            modelBuilder.Entity("ChatApp.Dto.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("MessageText")
                        .HasColumnType("text");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("ChatApp.Dto.Message", b =>
                {
                    b.HasOne("ChatApp.Dto.ChatRoom", "Room")
                        .WithMany("Messages")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("ChatApp.Dto.ChatRoom", b =>
                {
                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}