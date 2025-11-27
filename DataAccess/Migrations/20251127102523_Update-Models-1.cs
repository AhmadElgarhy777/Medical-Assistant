using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NuresId",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NuresId",
                table: "Chats",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_NuresId",
                table: "Ratings",
                column: "NuresId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_NuresId",
                table: "Chats",
                column: "NuresId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Nures_NuresId",
                table: "Chats",
                column: "NuresId",
                principalTable: "Nures",
                principalColumn: "NuresId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Nures_NuresId",
                table: "Ratings",
                column: "NuresId",
                principalTable: "Nures",
                principalColumn: "NuresId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Nures_NuresId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Nures_NuresId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_NuresId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Chats_NuresId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "NuresId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "NuresId",
                table: "Chats");
        }
    }
}
