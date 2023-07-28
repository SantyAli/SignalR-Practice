using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignelR_Practice.Data.Migrations
{
    public partial class mg3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User",
                table: "ChatMessages");

            migrationBuilder.AddColumn<Guid>(
                name: "Reciever",
                table: "ChatMessages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reciever",
                table: "ChatMessages");

            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
