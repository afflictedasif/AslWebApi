using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AslWebApi.Migrations
{
    public partial class UserInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInfos",
                columns: table => new
                {
                    UserInfoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    MobNo = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    EmailID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    LoginID = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TimeFr = table.Column<TimeSpan>(type: "time", maxLength: 10, nullable: false),
                    TimeTo = table.Column<TimeSpan>(type: "time", maxLength: 10, nullable: false),
                    Status = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: false),
                    LoginBy = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    LoginPW = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
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
                    table.PrimaryKey("PK_UserInfos", x => x.UserInfoID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInfos_EmailID",
                table: "UserInfos",
                column: "EmailID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInfos_LoginID",
                table: "UserInfos",
                column: "LoginID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInfos_MobNo",
                table: "UserInfos",
                column: "MobNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInfos_UserID",
                table: "UserInfos",
                column: "UserID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInfos");
        }
    }
}
