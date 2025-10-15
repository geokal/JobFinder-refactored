using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class ResearchGroup_ResearchActions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResearchGroup_ResearchActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResearchGroup_ProjectStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroupEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_UniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_ProjectFramework = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_ProjectTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_ProjectAcronym = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_ProjectGrantAgreementNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_ProjectStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResearchGroup_ProjectEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResearchGroup_ProjectTotalCost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_ProjectTotalEUContribution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_ProjectCoordinator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_ProjectKeywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_ProjectProgramme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_ProjectTopic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_ProjectDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_ProjectELKECode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_ProjectScientificResponsibleEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_EuropaCordisWebsite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_ProjectWebsite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_OurProjectBudget = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchGroup_ResearchActions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResearchGroup_ResearchActions");
        }
    }
}
