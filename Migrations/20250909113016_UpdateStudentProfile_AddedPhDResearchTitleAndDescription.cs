using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class UpdateStudentProfile_AddedPhDResearchTitleAndDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WhenStudentIsPostDocOrPhD_ResearchDescription",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WhenStudentIsPostDocOrPhD_ResearchTitle",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhenStudentIsPostDocOrPhD_ResearchDescription",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "WhenStudentIsPostDocOrPhD_ResearchTitle",
                table: "Students");
        }
    }
}
