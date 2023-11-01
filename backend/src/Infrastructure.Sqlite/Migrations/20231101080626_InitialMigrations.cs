using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Infrastructure.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Sqlite:InitSpatialMetaData", true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Picture = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ActivityId = table.Column<string>(type: "TEXT", nullable: false),
                    StartTime = table.Column<long>(type: "INTEGER", nullable: false),
                    TotalTimerTime = table.Column<decimal>(type: "TEXT", nullable: false),
                    BoundingBox = table.Column<Geometry>(type: "GEOMETRY", nullable: false)
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
                    FollowersUserId = table.Column<string>(type: "TEXT", nullable: false),
                    FollowingUserId = table.Column<string>(type: "TEXT", nullable: false)
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
                    ActivityId = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    StartTime = table.Column<long>(type: "INTEGER", nullable: false),
                    StartPosition = table.Column<Point>(type: "POINT", nullable: true),
                    EndPosition = table.Column<Point>(type: "POINT", nullable: true),
                    Duration = table.Column<decimal>(type: "TEXT", nullable: false),
                    DurationActive = table.Column<decimal>(type: "TEXT", nullable: false),
                    Distance = table.Column<decimal>(type: "TEXT", nullable: true),
                    CadenceAverage = table.Column<decimal>(type: "TEXT", nullable: true),
                    CadenceMax = table.Column<decimal>(type: "TEXT", nullable: true),
                    HeartRateAverage = table.Column<uint>(type: "INTEGER", nullable: true),
                    HeartRateMax = table.Column<uint>(type: "INTEGER", nullable: true),
                    SpeedAverage = table.Column<decimal>(type: "TEXT", nullable: true),
                    SpeedMax = table.Column<decimal>(type: "TEXT", nullable: true),
                    PowerAverage = table.Column<decimal>(type: "TEXT", nullable: true),
                    PowerMax = table.Column<decimal>(type: "TEXT", nullable: true),
                    Ascent = table.Column<decimal>(type: "TEXT", nullable: true),
                    Descent = table.Column<decimal>(type: "TEXT", nullable: true),
                    Calories = table.Column<uint>(type: "INTEGER", nullable: true)
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
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ActivityId = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<long>(type: "INTEGER", nullable: false),
                    Position = table.Column<Point>(type: "POINT", nullable: true),
                    Cadence = table.Column<decimal>(type: "TEXT", nullable: true),
                    Distance = table.Column<decimal>(type: "TEXT", nullable: true),
                    Altitude = table.Column<decimal>(type: "TEXT", nullable: true),
                    Speed = table.Column<decimal>(type: "TEXT", nullable: true),
                    HeartRate = table.Column<uint>(type: "INTEGER", nullable: true),
                    Power = table.Column<decimal>(type: "TEXT", nullable: true)
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
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ActivityId = table.Column<string>(type: "TEXT", nullable: false),
                    StartTime = table.Column<long>(type: "INTEGER", nullable: false),
                    BoundingBox = table.Column<Geometry>(type: "GEOMETRY", nullable: false),
                    Duration = table.Column<decimal>(type: "TEXT", nullable: false),
                    DurationActive = table.Column<decimal>(type: "TEXT", nullable: false),
                    Distance = table.Column<decimal>(type: "TEXT", nullable: true),
                    CadenceAverage = table.Column<decimal>(type: "TEXT", nullable: true),
                    CadenceMax = table.Column<decimal>(type: "TEXT", nullable: true),
                    HeartRateAverage = table.Column<uint>(type: "INTEGER", nullable: true),
                    HeartRateMax = table.Column<uint>(type: "INTEGER", nullable: true),
                    SpeedAverage = table.Column<decimal>(type: "TEXT", nullable: true),
                    SpeedMax = table.Column<decimal>(type: "TEXT", nullable: true),
                    PowerAverage = table.Column<decimal>(type: "TEXT", nullable: true),
                    PowerMax = table.Column<decimal>(type: "TEXT", nullable: true),
                    Ascent = table.Column<decimal>(type: "TEXT", nullable: true),
                    Descent = table.Column<decimal>(type: "TEXT", nullable: true),
                    Calories = table.Column<uint>(type: "INTEGER", nullable: true)
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
