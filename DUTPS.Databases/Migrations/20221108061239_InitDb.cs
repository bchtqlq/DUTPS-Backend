using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DUTPS.Databases.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false, comment: "Id of faculty"),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "Name of Faculty"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "the time that the record was inserted"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "record's last update time"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "the time that the record was deleted"),
                    del_flag = table.Column<bool>(type: "boolean", nullable: false, comment: "true = deleted; false = available")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "primary key of table and auto increase")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false, comment: "username of user (sv id)"),
                    email = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false, comment: "email"),
                    password_hash = table.Column<byte[]>(type: "bytea", nullable: true, comment: "password hash"),
                    password_salt = table.Column<byte[]>(type: "bytea", nullable: true, comment: "password salt"),
                    status = table.Column<int>(type: "integer", nullable: false, comment: "status of account of user"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "the time that the record was inserted"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "record's last update time"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "the time that the record was deleted"),
                    del_flag = table.Column<bool>(type: "boolean", nullable: false, comment: "true = deleted; false = available")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserInfos",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false, comment: "primary key to identity the user"),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "name of user"),
                    gender = table.Column<int>(type: "integer", nullable: true, comment: "gender of user"),
                    birthday = table.Column<DateTime>(type: "date", nullable: true, comment: "birthday of user"),
                    phoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "phone number of user"),
                    @class = table.Column<string>(name: "class", type: "character varying(50)", maxLength: 50, nullable: true, comment: "class name of user"),
                    qrcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Link to QR Code Of User"),
                    faculty_id = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true, comment: "Faculty Id Of User"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "the time that the record was inserted"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "record's last update time"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "the time that the record was deleted"),
                    del_flag = table.Column<bool>(type: "boolean", nullable: false, comment: "true = deleted; false = available")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfos", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_UserInfos_Faculties_faculty_id",
                        column: x => x.faculty_id,
                        principalTable: "Faculties",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_UserInfos_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInfos_faculty_id",
                table: "UserInfos",
                column: "faculty_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInfos");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
