using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AslWebApi.Migrations
{
    public partial class V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CLogs",
                columns: table => new
                {
                    ClogID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    LogType = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false),
                    LogData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogTime = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Ltude = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    UserPC = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    IPAddress = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLogs", x => x.ClogID);
                });

            migrationBuilder.CreateTable(
                name: "ScreenShots",
                columns: table => new
                {
                    ScreenShotID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    DirPath = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    FileName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    InUserID = table.Column<int>(type: "int", nullable: true),
                    UpUserID = table.Column<int>(type: "int", nullable: true),
                    InTime = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    UpTime = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    InLtude = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    UpLtude = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    InUserPC = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    UpUserPC = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    InIPAddress = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    UpIPAddress = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScreenShots", x => x.ScreenShotID);
                });

            migrationBuilder.CreateTable(
                name: "UserStates",
                columns: table => new
                {
                    UserStateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    CurrentState = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    TimeFrom = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    TimeTo = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InUserID = table.Column<int>(type: "int", nullable: true),
                    UpUserID = table.Column<int>(type: "int", nullable: true),
                    InTime = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    UpTime = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    InLtude = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    UpLtude = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    InUserPC = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    UpUserPC = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    InIPAddress = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    UpIPAddress = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStates", x => x.UserStateId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserStates_UserID",
                table: "UserStates",
                column: "UserID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CLogs");

            migrationBuilder.DropTable(
                name: "ScreenShots");

            migrationBuilder.DropTable(
                name: "UserStates");
        }
    }
}
