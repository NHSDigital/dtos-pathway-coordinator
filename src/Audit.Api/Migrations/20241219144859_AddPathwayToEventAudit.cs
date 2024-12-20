using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Audit.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPathwayToEventAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pathway",
                table: "EventAudits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pathway",
                table: "EventAudits");
        }
    }
}
