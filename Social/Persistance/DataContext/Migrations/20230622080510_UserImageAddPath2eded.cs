using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.DataContext.Migrations
{
    public partial class UserImageAddPath2eded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "UserImages",
                newName: "PathProfile");

            migrationBuilder.AddColumn<string>(
                name: "PathBack",
                table: "UserImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PathBack",
                table: "UserImages");

            migrationBuilder.RenameColumn(
                name: "PathProfile",
                table: "UserImages",
                newName: "Path");
        }
    }
}
