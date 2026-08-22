using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RafeekRoomMate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class favoritestables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomFavorites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomFavorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomFavorites_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomFavorites_RoomId",
                table: "RoomFavorites",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomFavorites_UserId_RoomId",
                table: "RoomFavorites",
                columns: new[] { "UserId", "RoomId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomFavorites");
        }
    }
}
