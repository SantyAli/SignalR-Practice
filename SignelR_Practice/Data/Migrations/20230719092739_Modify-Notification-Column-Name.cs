using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SignelR_Practice.Data.Migrations
{
    public partial class ModifyNotificationColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "createdBy",
                table: "Notifications",
                newName: "SenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Notifications",
                newName: "createdBy");
        }
    }
}
