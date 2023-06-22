using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.Datacontext.Migrations
{
    public partial class ProfileImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBacroundImage",
                table: "UserImages");

            migrationBuilder.DropColumn(
                name: "IsProfileImage",
                table: "UserImages");

            migrationBuilder.AddColumn<string>(
                name: "BackraundImageName",
                table: "UserImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageName",
                table: "UserImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackraundImageName",
                table: "UserImages");

            migrationBuilder.DropColumn(
                name: "ProfileImageName",
                table: "UserImages");

            migrationBuilder.AddColumn<bool>(
                name: "IsBacroundImage",
                table: "UserImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsProfileImage",
                table: "UserImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
