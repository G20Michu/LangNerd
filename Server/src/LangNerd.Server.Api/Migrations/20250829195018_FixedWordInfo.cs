using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LangNerd.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixedWordInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WordInfo",
                table: "WordInfo");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "WordInfo",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WordInfo",
                table: "WordInfo",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WordInfo",
                table: "WordInfo");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WordInfo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WordInfo",
                table: "WordInfo",
                column: "FileHash");
        }
    }
}
