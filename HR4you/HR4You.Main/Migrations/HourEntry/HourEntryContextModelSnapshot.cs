﻿// <auto-generated />
using System;
using HR4You.Contexts.HourEntry;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HR4You.Migrations.HourEntry
{
    [DbContext(typeof(HourEntryContext))]
    partial class HourEntryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("HR4You.Model.Base.Models.HourEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<int?>("Duration")
                        .HasColumnType("int");

                    b.Property<TimeOnly?>("EndTime")
                        .HasColumnType("time(6)");

                    b.Property<int?>("FlagId")
                        .HasColumnType("int");

                    b.Property<bool>("IsBillable")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsHoliday")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<TimeOnly?>("StartTime")
                        .HasColumnType("time(6)");

                    b.Property<int>("TaskId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("hr4you_HourEntry", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
