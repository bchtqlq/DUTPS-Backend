using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUTPS.Databases.Migrations
{
    public partial class UpdateFaculty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "Faculties",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                comment: "Id of faculty",
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldMaxLength: 3,
                oldComment: "Id of faculty");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "Faculties",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                comment: "Id of faculty",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldComment: "Id of faculty");
        }
    }
}
