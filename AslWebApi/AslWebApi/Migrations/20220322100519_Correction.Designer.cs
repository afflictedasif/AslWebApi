﻿// <auto-generated />
using System;
using AslWebApi.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AslWebApi.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20220322100519_Correction")]
    partial class Correction
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AslWebApi.DAL.Models.CLog", b =>
                {
                    b.Property<long>("ClogID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ClogID"), 1L, 1);

                    b.Property<string>("IPAddress")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LogData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LogTime")
                        .IsRequired()
                        .HasColumnType("smalldatetime");

                    b.Property<string>("LogType")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("varchar(6)");

                    b.Property<string>("Ltude")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<string>("UserPC")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("ClogID");

                    b.ToTable("CLogs");
                });

            modelBuilder.Entity("AslWebApi.DAL.Models.ScreenShot", b =>
                {
                    b.Property<long>("ScreenShotID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("ScreenShotID"), 1L, 1);

                    b.Property<string>("DirPath")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("InIPAddress")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("InLtude")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime?>("InTime")
                        .HasColumnType("smalldatetime");

                    b.Property<int?>("InUserID")
                        .HasColumnType("int");

                    b.Property<string>("InUserPC")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("UpIPAddress")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("UpLtude")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime?>("UpTime")
                        .HasColumnType("smalldatetime");

                    b.Property<int?>("UpUserID")
                        .HasColumnType("int");

                    b.Property<string>("UpUserPC")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("ScreenShotID");

                    b.ToTable("ScreenShots");
                });

            modelBuilder.Entity("AslWebApi.DAL.Models.UserInfo", b =>
                {
                    b.Property<int>("UserInfoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserInfoID"), 1L, 1);

                    b.Property<string>("Address")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("EmailID")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("InIPAddress")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("InLtude")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime?>("InTime")
                        .HasColumnType("smalldatetime");

                    b.Property<int?>("InUserID")
                        .HasColumnType("int");

                    b.Property<string>("InUserPC")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LoginBy")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<string>("LoginID")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LoginPW")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("MobNo")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("varchar(1)");

                    b.Property<TimeSpan>("TimeFr")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("TimeTo")
                        .HasColumnType("time");

                    b.Property<string>("UpIPAddress")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("UpLtude")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime?>("UpTime")
                        .HasColumnType("smalldatetime");

                    b.Property<int?>("UpUserID")
                        .HasColumnType("int");

                    b.Property<string>("UpUserPC")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("UserInfoID");

                    b.HasIndex("EmailID")
                        .IsUnique();

                    b.HasIndex("LoginID")
                        .IsUnique();

                    b.HasIndex("MobNo")
                        .IsUnique();

                    b.HasIndex("UserID")
                        .IsUnique();

                    b.ToTable("UserInfos");
                });

            modelBuilder.Entity("AslWebApi.DAL.Models.UserState", b =>
                {
                    b.Property<int>("UserStateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserStateId"), 1L, 1);

                    b.Property<string>("CurrentState")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("InIPAddress")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("InLtude")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime?>("InTime")
                        .HasColumnType("smalldatetime");

                    b.Property<int?>("InUserID")
                        .HasColumnType("int");

                    b.Property<string>("InUserPC")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Remarks")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("TimeFrom")
                        .HasColumnType("smalldatetime");

                    b.Property<DateTime?>("TimeTo")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("UpIPAddress")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("UpLtude")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime?>("UpTime")
                        .HasColumnType("smalldatetime");

                    b.Property<int?>("UpUserID")
                        .HasColumnType("int");

                    b.Property<string>("UpUserPC")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("UserStateId");

                    b.HasIndex("UserID")
                        .IsUnique();

                    b.ToTable("UserStates");
                });
#pragma warning restore 612, 618
        }
    }
}