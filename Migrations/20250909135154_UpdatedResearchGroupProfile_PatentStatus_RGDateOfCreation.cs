using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class UpdatedResearchGroupProfile_PatentStatus_RGDateOfCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ResearchGroup_DateOfCreation",
                table: "ResearchGroups",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ResearchGroup_Patent_PatentStatus",
                table: "ResearchGroup_Patents",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResearchGroup_DateOfCreation",
                table: "ResearchGroups");

            migrationBuilder.DropColumn(
                name: "ResearchGroup_Patent_PatentStatus",
                table: "ResearchGroup_Patents");
        }
    }
}
