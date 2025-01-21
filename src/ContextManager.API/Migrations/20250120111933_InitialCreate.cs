using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContextManager.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    NhsNumber = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.NhsNumber);
                });

            migrationBuilder.CreateTable(
                name: "Pathways",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParticipantNhsNumber = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pathways", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Pathways_Participants_ParticipantNhsNumber",
                        column: x => x.ParticipantNhsNumber,
                        principalTable: "Participants",
                        principalColumn: "NhsNumber");
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pathway = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NhsNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PathwayName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Pathways_PathwayName",
                        column: x => x.PathwayName,
                        principalTable: "Pathways",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_PathwayName",
                table: "Events",
                column: "PathwayName");

            migrationBuilder.CreateIndex(
                name: "IX_Pathways_ParticipantNhsNumber",
                table: "Pathways",
                column: "ParticipantNhsNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Pathways");

            migrationBuilder.DropTable(
                name: "Participants");
        }
    }
}
