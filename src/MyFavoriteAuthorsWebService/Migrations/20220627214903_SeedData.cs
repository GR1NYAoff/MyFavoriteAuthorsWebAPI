using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFavoriteAuthorsWebService.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Name", "Password", "PasswordKey", "Role" },
                values: new object[] { new Guid("685c6cae-0c3c-4842-94d1-f57aac21cb25"), "AppAdminAccount", "XLa59OaVBkf1aQOQETkadO6n14+mc2A8K6z2yK7R0FM=", new byte[] { 60, 229, 31, 119, 105, 69, 163, 235, 190, 86, 167, 108, 13, 194, 91, 145 }, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("685c6cae-0c3c-4842-94d1-f57aac21cb25"));
        }
    }
}
