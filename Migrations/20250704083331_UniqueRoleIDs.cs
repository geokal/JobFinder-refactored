using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class UniqueRoleIDs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Student_UniqueID",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Professor_UniqueID",
                table: "Professors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Company_UniqueID",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Student_UniqueID",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Professor_UniqueID",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "Company_UniqueID",
                table: "Companies");
        }
    }
}
