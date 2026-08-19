using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RafeekRoomMate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class adduserprofile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Bio = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    Occupation = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Area = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    MonthlyBudget = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MoveInDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsSmoking = table.Column<bool>(type: "boolean", nullable: false),
                    HasPets = table.Column<bool>(type: "boolean", nullable: false),
                    LikesGuests = table.Column<bool>(type: "boolean", nullable: true),
                    PreferredRoommateGender = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    PreferredAgeMin = table.Column<int>(type: "integer", nullable: true),
                    PreferredAgeMax = table.Column<int>(type: "integer", nullable: true),
                    ProfileImageKey = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_City",
                table: "UserProfiles",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_MonthlyBudget",
                table: "UserProfiles",
                column: "MonthlyBudget");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfiles");
        }
    }
}
