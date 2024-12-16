﻿// <auto-generated />
using System;
using HR4You.Contexts.WorkTime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HR4You.Migrations.WorkTime
{
    [DbContext(typeof(WorkTimeContext))]
    [Migration("20241212074525_ChangedToFloatWorkTime")]
    partial class ChangedToFloatWorkTime
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("HR4You.Model.Base.Models.WorkTime.WorkTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Holidays")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<float>("MinFriHours")
                        .HasColumnType("float");

                    b.Property<float>("MinMonHours")
                        .HasColumnType("float");

                    b.Property<float>("MinSatHours")
                        .HasColumnType("float");

                    b.Property<float>("MinSunHours")
                        .HasColumnType("float");

                    b.Property<float>("MinThuHours")
                        .HasColumnType("float");

                    b.Property<float>("MinTueHours")
                        .HasColumnType("float");

                    b.Property<float>("MinWedHours")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("hr4you_worktime");
                });
#pragma warning restore 612, 618
        }
    }
}
