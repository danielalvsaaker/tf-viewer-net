using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Infrastructure.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Picture = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ActivityId = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TotalTimerTime = table.Column<decimal>(type: "numeric", nullable: false),
                    BoundingBox = table.Column<Geometry>(type: "geometry", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => new { x.UserId, x.ActivityId });
                    table.ForeignKey(
                        name: "FK_Activities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Followers",
                columns: table => new
                {
                    FollowersUserId = table.Column<string>(type: "text", nullable: false),
                    FollowingUserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Followers", x => new { x.FollowersUserId, x.FollowingUserId });
                    table.ForeignKey(
                        name: "FK_Followers_Users_FollowersUserId",
                        column: x => x.FollowersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Followers_Users_FollowingUserId",
                        column: x => x.FollowingUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lap",
                columns: table => new
                {
                    ActivityId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StartPosition = table.Column<Point>(type: "geometry (point)", nullable: true),
                    EndPosition = table.Column<Point>(type: "geometry (point)", nullable: true),
                    Duration = table.Column<decimal>(type: "numeric", nullable: false),
                    DurationActive = table.Column<decimal>(type: "numeric", nullable: false),
                    Distance = table.Column<decimal>(type: "numeric", nullable: true),
                    CadenceAverage = table.Column<decimal>(type: "numeric", nullable: true),
                    CadenceMax = table.Column<decimal>(type: "numeric", nullable: true),
                    HeartRateAverage = table.Column<long>(type: "bigint", nullable: true),
                    HeartRateMax = table.Column<long>(type: "bigint", nullable: true),
                    SpeedAverage = table.Column<decimal>(type: "numeric", nullable: true),
                    SpeedMax = table.Column<decimal>(type: "numeric", nullable: true),
                    PowerAverage = table.Column<decimal>(type: "numeric", nullable: true),
                    PowerMax = table.Column<decimal>(type: "numeric", nullable: true),
                    Ascent = table.Column<decimal>(type: "numeric", nullable: true),
                    Descent = table.Column<decimal>(type: "numeric", nullable: true),
                    Calories = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lap", x => new { x.UserId, x.ActivityId, x.StartTime });
                    table.ForeignKey(
                        name: "FK_Lap_Activities_UserId_ActivityId",
                        columns: x => new { x.UserId, x.ActivityId },
                        principalTable: "Activities",
                        principalColumns: new[] { "UserId", "ActivityId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Record",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ActivityId = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Position = table.Column<Point>(type: "geometry (point)", nullable: true),
                    Cadence = table.Column<decimal>(type: "numeric", nullable: true),
                    Distance = table.Column<decimal>(type: "numeric", nullable: true),
                    Altitude = table.Column<decimal>(type: "numeric", nullable: true),
                    Speed = table.Column<decimal>(type: "numeric", nullable: true),
                    HeartRate = table.Column<long>(type: "bigint", nullable: true),
                    Power = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Record", x => new { x.UserId, x.ActivityId, x.Timestamp });
                    table.ForeignKey(
                        name: "FK_Record_Activities_UserId_ActivityId",
                        columns: x => new { x.UserId, x.ActivityId },
                        principalTable: "Activities",
                        principalColumns: new[] { "UserId", "ActivityId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ActivityId = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    BoundingBox = table.Column<Geometry>(type: "geometry", nullable: false),
                    Duration = table.Column<decimal>(type: "numeric", nullable: false),
                    DurationActive = table.Column<decimal>(type: "numeric", nullable: false),
                    Distance = table.Column<decimal>(type: "numeric", nullable: true),
                    CadenceAverage = table.Column<decimal>(type: "numeric", nullable: true),
                    CadenceMax = table.Column<decimal>(type: "numeric", nullable: true),
                    HeartRateAverage = table.Column<long>(type: "bigint", nullable: true),
                    HeartRateMax = table.Column<long>(type: "bigint", nullable: true),
                    SpeedAverage = table.Column<decimal>(type: "numeric", nullable: true),
                    SpeedMax = table.Column<decimal>(type: "numeric", nullable: true),
                    PowerAverage = table.Column<decimal>(type: "numeric", nullable: true),
                    PowerMax = table.Column<decimal>(type: "numeric", nullable: true),
                    Ascent = table.Column<decimal>(type: "numeric", nullable: true),
                    Descent = table.Column<decimal>(type: "numeric", nullable: true),
                    Calories = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => new { x.UserId, x.ActivityId, x.StartTime });
                    table.ForeignKey(
                        name: "FK_Session_Activities_UserId_ActivityId",
                        columns: x => new { x.UserId, x.ActivityId },
                        principalTable: "Activities",
                        principalColumns: new[] { "UserId", "ActivityId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Followers_FollowingUserId",
                table: "Followers",
                column: "FollowingUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Followers");

            migrationBuilder.DropTable(
                name: "Lap");

            migrationBuilder.DropTable(
                name: "Record");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
