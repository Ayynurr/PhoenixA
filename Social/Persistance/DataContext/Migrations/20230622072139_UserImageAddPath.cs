using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.DataContext.Migrations
{
    public partial class UserImageAddPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "UserImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "UserImages");
        }
    }
}
