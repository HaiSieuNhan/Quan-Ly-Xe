using Microsoft.EntityFrameworkCore.Migrations;

namespace VroomDb.Migrations
{
    public partial class AddColumImageName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Bikes");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Bikes",
                type: "nvarchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Bikes");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Bikes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
