using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.DataAccess.Migrations
{
    public partial class RoleUserRoleCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2022, 1, 10, 8, 14, 44, 316, DateTimeKind.Local).AddTicks(5529), new DateTime(2022, 1, 10, 8, 14, 44, 316, DateTimeKind.Local).AddTicks(5797) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2022, 1, 10, 8, 14, 44, 316, DateTimeKind.Local).AddTicks(6273), new DateTime(2022, 1, 10, 8, 14, 44, 316, DateTimeKind.Local).AddTicks(6274) });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "d83fe835-39ed-4dcd-869a-06c1e98f8e8e", "Admin", "ADMIN" },
                    { 2, "24cde150-a089-4bd9-8813-880d6d5a2ff5", "Edıtor", "EDITOR" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Picture", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 0, "418a47bc-724e-4986-a312-272f9a4bbbe4", "admin@gmail.com", true, false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEHTfbCRpbToU+vWtw5QuuqUCC3DUjZSb89nwMLgZYuNdt/snboEB3O5XRBS+mDqulw==", "+905555556678", true, "default.png", "1dbdde56-0b8d-42b1-9115-68f11a51d466", false, "Admin" },
                    { 2, 0, "efce6ea7-b394-4320-ae4a-46ae73a9e161", "editor@gmail.com", true, false, null, "EDITOR@GMAIL.COM", "EDITOR", "AQAAAAEAACcQAAAAEMSFBddn9+dGL4ZxviK+Exl6DZzoB302WiMl0thvBkvPKmHT/Q0qwV+ku/IJoT5YCQ==", "+905555556678", true, "default.png", "6d6a5b47-490e-4ea1-accc-af9aef392088", false, "Editör" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 2, 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2022, 1, 7, 6, 56, 25, 800, DateTimeKind.Local).AddTicks(6972), new DateTime(2022, 1, 7, 6, 56, 25, 800, DateTimeKind.Local).AddTicks(7322) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2022, 1, 7, 6, 56, 25, 800, DateTimeKind.Local).AddTicks(7931), new DateTime(2022, 1, 7, 6, 56, 25, 800, DateTimeKind.Local).AddTicks(7932) });
        }
    }
}
