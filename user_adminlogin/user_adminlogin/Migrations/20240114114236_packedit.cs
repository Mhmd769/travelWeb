using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace user_adminlogin.Migrations
{
    public partial class packedit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "bookedPackage",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "total_Price",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bookedPackage",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "total_Price",
                table: "Packages");
        }
    }
}
