using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DUTPS.Databases.Migrations
{
    public partial class RemoveQRCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "qrcode",
                table: "UserInfos");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "UserInfos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "name of user",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldComment: "name of user");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "UserInfos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                comment: "name of user",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "name of user");

            migrationBuilder.AddColumn<string>(
                name: "qrcode",
                table: "UserInfos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                comment: "Link to QR Code Of User");
        }
    }
}
