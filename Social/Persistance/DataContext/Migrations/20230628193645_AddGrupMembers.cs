using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistance.DataContext.Migrations
{
    public partial class AddGrupMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembership_AspNetUsers_UserId",
                table: "GroupMembership");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembership_Groups_GroupId",
                table: "GroupMembership");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupMembership",
                table: "GroupMembership");

            migrationBuilder.RenameTable(
                name: "GroupMembership",
                newName: "GroupMemberships");

            migrationBuilder.RenameIndex(
                name: "IX_GroupMembership_GroupId",
                table: "GroupMemberships",
                newName: "IX_GroupMemberships_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupMemberships",
                table: "GroupMemberships",
                columns: new[] { "UserId", "GroupId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMemberships_AspNetUsers_UserId",
                table: "GroupMemberships",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMemberships_Groups_GroupId",
                table: "GroupMemberships",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMemberships_AspNetUsers_UserId",
                table: "GroupMemberships");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMemberships_Groups_GroupId",
                table: "GroupMemberships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupMemberships",
                table: "GroupMemberships");

            migrationBuilder.RenameTable(
                name: "GroupMemberships",
                newName: "GroupMembership");

            migrationBuilder.RenameIndex(
                name: "IX_GroupMemberships_GroupId",
                table: "GroupMembership",
                newName: "IX_GroupMembership_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupMembership",
                table: "GroupMembership",
                columns: new[] { "UserId", "GroupId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembership_AspNetUsers_UserId",
                table: "GroupMembership",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembership_Groups_GroupId",
                table: "GroupMembership",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
