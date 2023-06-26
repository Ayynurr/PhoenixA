using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.DataContext.Migrations
{
    public partial class FriendStatusAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "UserFriends",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserFriends");
        }
    }
}
