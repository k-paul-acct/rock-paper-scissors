using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RockPaperScissors.Api.Migrations
{
    /// <inheritdoc />
    public partial class BotType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Players");
        }
    }
}
