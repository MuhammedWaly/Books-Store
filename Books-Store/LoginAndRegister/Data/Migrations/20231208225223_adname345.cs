using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoginAndRegister.Data.Migrations
{
    /// <inheritdoc />
    public partial class adname345 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AddProductViewModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte>(
                name: "genreId",
                table: "AddProductViewModel",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "AddProductViewModel");

            migrationBuilder.DropColumn(
                name: "genreId",
                table: "AddProductViewModel");
        }
    }
}
