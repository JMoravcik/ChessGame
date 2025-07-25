using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChessGame.AccessService.Api.Migrations
{
    /// <inheritdoc />
    public partial class GameRecord_IsFinished : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "GameRecords",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "GameRecords");
        }
    }
}
