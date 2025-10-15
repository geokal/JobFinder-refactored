using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class UpdatedResearchGroupProfile_AreasAndSkillsBoxes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResearchGroupAreas",
                table: "ResearchGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchGroupSkills",
                table: "ResearchGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemberType",
                table: "ResearchGroup_Publications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResearchGroupAreas",
                table: "ResearchGroups");

            migrationBuilder.DropColumn(
                name: "ResearchGroupSkills",
                table: "ResearchGroups");

            migrationBuilder.DropColumn(
                name: "MemberType",
                table: "ResearchGroup_Publications");
        }
    }
}
