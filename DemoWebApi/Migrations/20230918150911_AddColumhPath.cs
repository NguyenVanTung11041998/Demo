using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoWebApi.Migrations
{
    public partial class AddColumhPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Avatar",
                table: "Users",
                newName: "Path");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Users",
                newName: "Avatar");
        }
    }
}
