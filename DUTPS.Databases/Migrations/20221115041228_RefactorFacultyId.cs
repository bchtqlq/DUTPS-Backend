using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUTPS.Databases.Migrations
{
    public partial class RefactorFacultyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "faculty_id",
                table: "UserInfos",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                comment: "Faculty Id Of User",
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3,
                oldNullable: true,
                oldComment: "Faculty Id Of User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "faculty_id",
                table: "UserInfos",
                type: "character varying(3)",
                maxLength: 3,
                nullable: true,
                comment: "Faculty Id Of User",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true,
                oldComment: "Faculty Id Of User");
        }
    }
}
