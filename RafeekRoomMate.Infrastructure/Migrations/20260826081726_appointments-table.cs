using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RafeekRoomMate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class appointmentstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomAppointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RequestedStartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    RequestedEndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RejectionReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomAppointments_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomAppointments_RoomId",
                table: "RoomAppointments",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomAppointments_RoomId_UserId_Status",
                table: "RoomAppointments",
                columns: new[] { "RoomId", "UserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_RoomAppointments_UserId",
                table: "RoomAppointments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomAppointments");
        }
    }
}
