using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarrosAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddImagemToModeloCarro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "ModelosCarros",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "ModelosCarros");
        }
    }
}
