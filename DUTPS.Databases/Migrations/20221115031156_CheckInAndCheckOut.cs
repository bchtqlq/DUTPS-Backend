using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DUTPS.Databases.Migrations
{
    public partial class CheckInAndCheckOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicals",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "primary key of table and auto increase")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LicensePlate = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "the time that the record was inserted"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "record's last update time"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "the time that the record was deleted"),
                    del_flag = table.Column<bool>(type: "boolean", nullable: false, comment: "true = deleted; false = available")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicals", x => x.id);
                    table.ForeignKey(
                        name: "FK_Vehicals_Users_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CheckIns",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "primary key of table and auto increase")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateOfCheckIn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsCheckOut = table.Column<bool>(type: "boolean", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    StaffId = table.Column<long>(type: "bigint", nullable: false),
                    VehicalId = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "the time that the record was inserted"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "record's last update time"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "the time that the record was deleted"),
                    del_flag = table.Column<bool>(type: "boolean", nullable: false, comment: "true = deleted; false = available")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckIns", x => x.id);
                    table.ForeignKey(
                        name: "FK_CheckIns_Users_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CheckIns_Users_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CheckIns_Vehicals_VehicalId",
                        column: x => x.VehicalId,
                        principalTable: "Vehicals",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CheckOuts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "primary key of table and auto increase")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateOfCheckOut = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StaffId = table.Column<long>(type: "bigint", nullable: false),
                    CheckInId = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "the time that the record was inserted"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "record's last update time"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "the time that the record was deleted"),
                    del_flag = table.Column<bool>(type: "boolean", nullable: false, comment: "true = deleted; false = available")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckOuts", x => x.id);
                    table.ForeignKey(
                        name: "FK_CheckOuts_CheckIns_CheckInId",
                        column: x => x.CheckInId,
                        principalTable: "CheckIns",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CheckOuts_Users_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_CustomerId",
                table: "CheckIns",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_StaffId",
                table: "CheckIns",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_VehicalId",
                table: "CheckIns",
                column: "VehicalId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckOuts_CheckInId",
                table: "CheckOuts",
                column: "CheckInId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckOuts_StaffId",
                table: "CheckOuts",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicals_CustomerId",
                table: "Vehicals",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckOuts");

            migrationBuilder.DropTable(
                name: "CheckIns");

            migrationBuilder.DropTable(
                name: "Vehicals");
        }
    }
}
