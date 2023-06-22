using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.DataContext.Migrations
{
    public partial class ImageNameRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "UserImages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "UserImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
