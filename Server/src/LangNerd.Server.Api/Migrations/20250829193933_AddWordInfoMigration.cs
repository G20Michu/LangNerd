using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LangNerd.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddWordInfoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WordInfo",
                columns: table => new
                {
                    FileHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordInfo", x => x.FileHash);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordInfo");
        }
    }
}
