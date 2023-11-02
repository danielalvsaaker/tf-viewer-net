﻿// <auto-generated />
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;

#nullable disable

namespace Infrastructure.Sqlite.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0-rc.2.23480.1");

            modelBuilder.Entity("Core.Activity", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ActivityId")
                        .HasColumnType("TEXT");

                    b.Property<Geometry>("BoundingBox")
                        .IsRequired()
                        .HasColumnType("GEOMETRY");

                    b.Property<long>("StartTime")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("TotalTimerTime")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "ActivityId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("Core.Lap", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ActivityId")
                        .HasColumnType("TEXT");

                    b.Property<long>("StartTime")
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("Ascent")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("CadenceAverage")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("CadenceMax")
                        .HasColumnType("TEXT");

                    b.Property<uint?>("Calories")
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("Descent")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Distance")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Duration")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("DurationActive")
                        .HasColumnType("TEXT");

                    b.Property<Point>("EndPosition")
                        .HasColumnType("POINT");

                    b.Property<uint?>("HeartRateAverage")
                        .HasColumnType("INTEGER");

                    b.Property<uint?>("HeartRateMax")
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("PowerAverage")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("PowerMax")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("SpeedAverage")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("SpeedMax")
                        .HasColumnType("TEXT");

                    b.Property<Point>("StartPosition")
                        .HasColumnType("POINT");

                    b.HasKey("UserId", "ActivityId", "StartTime");

                    b.ToTable("Lap");
                });

            modelBuilder.Entity("Core.Record", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ActivityId")
                        .HasColumnType("TEXT");

                    b.Property<long>("Timestamp")
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("Altitude")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Cadence")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Distance")
                        .HasColumnType("TEXT");

                    b.Property<uint?>("HeartRate")
                        .HasColumnType("INTEGER");

                    b.Property<Point>("Position")
                        .HasColumnType("POINT");

                    b.Property<decimal?>("Power")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Speed")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "ActivityId", "Timestamp");

                    b.ToTable("Record");
                });

            modelBuilder.Entity("Core.Session", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ActivityId")
                        .HasColumnType("TEXT");

                    b.Property<long>("StartTime")
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("Ascent")
                        .HasColumnType("TEXT");

                    b.Property<Geometry>("BoundingBox")
                        .IsRequired()
                        .HasColumnType("GEOMETRY");

                    b.Property<decimal?>("CadenceAverage")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("CadenceMax")
                        .HasColumnType("TEXT");

                    b.Property<uint?>("Calories")
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("Descent")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Distance")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Duration")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("DurationActive")
                        .HasColumnType("TEXT");

                    b.Property<uint?>("HeartRateAverage")
                        .HasColumnType("INTEGER");

                    b.Property<uint?>("HeartRateMax")
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("PowerAverage")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("PowerMax")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("SpeedAverage")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("SpeedMax")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "ActivityId", "StartTime");

                    b.ToTable("Session");
                });

            modelBuilder.Entity("Core.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Picture")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.Property<string>("FollowersUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("FollowingUserId")
                        .HasColumnType("TEXT");

                    b.HasKey("FollowersUserId", "FollowingUserId");

                    b.HasIndex("FollowingUserId");

                    b.ToTable("Followers", (string)null);
                });

            modelBuilder.Entity("Core.Activity", b =>
                {
                    b.HasOne("Core.User", null)
                        .WithMany("Activities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Lap", b =>
                {
                    b.HasOne("Core.Activity", null)
                        .WithMany("Laps")
                        .HasForeignKey("UserId", "ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Record", b =>
                {
                    b.HasOne("Core.Activity", null)
                        .WithMany("Records")
                        .HasForeignKey("UserId", "ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Session", b =>
                {
                    b.HasOne("Core.Activity", null)
                        .WithMany("Sessions")
                        .HasForeignKey("UserId", "ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.HasOne("Core.User", null)
                        .WithMany()
                        .HasForeignKey("FollowersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.User", null)
                        .WithMany()
                        .HasForeignKey("FollowingUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Activity", b =>
                {
                    b.Navigation("Laps");

                    b.Navigation("Records");

                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("Core.User", b =>
                {
                    b.Navigation("Activities");
                });
#pragma warning restore 612, 618
        }
    }
}