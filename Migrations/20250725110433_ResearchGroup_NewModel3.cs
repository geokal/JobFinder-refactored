using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class ResearchGroup_NewModel3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResearchGroupTelephoneNumber_YouTubeChannel",
                table: "ResearchGroups",
                newName: "ResearchGroup_YouTubeChannel");

            migrationBuilder.RenameColumn(
                name: "ResearchGroupTelephoneNumber_EmbeddedPromoVideo",
                table: "ResearchGroups",
                newName: "ResearchGroup_EmbeddedPromoVideo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResearchGroup_YouTubeChannel",
                table: "ResearchGroups",
                newName: "ResearchGroupTelephoneNumber_YouTubeChannel");

            migrationBuilder.RenameColumn(
                name: "ResearchGroup_EmbeddedPromoVideo",
                table: "ResearchGroups",
                newName: "ResearchGroupTelephoneNumber_EmbeddedPromoVideo");
        }
    }
}
