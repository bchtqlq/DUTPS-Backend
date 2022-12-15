using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUTPS.Databases.Migrations
{
    public partial class RoleForUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "role",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "role of account of user");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "Users");
        }
    }
}
