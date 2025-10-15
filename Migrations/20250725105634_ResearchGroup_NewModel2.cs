using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class ResearchGroup_NewModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResearchGroupAcronym",
                table: "ResearchGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ResearchGroupTeamImage",
                table: "ResearchGroups",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchGroupTelephoneNumber_EmbeddedPromoVideo",
                table: "ResearchGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchGroupTelephoneNumber_YouTubeChannel",
                table: "ResearchGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchGroup_Facebook",
                table: "ResearchGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchGroup_LinkedIn",
                table: "ResearchGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ResearchGroup_PresentationAttachment",
                table: "ResearchGroups",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchGroup_Twitter",
                table: "ResearchGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResearchGroup_Website",
                table: "ResearchGroups",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResearchGroupAcronym",
                table: "ResearchGroups");

            migrationBuilder.DropColumn(
                name: "ResearchGroupTeamImage",
                table: "ResearchGroups");

            migrationBuilder.DropColumn(
                name: "ResearchGroupTelephoneNumber_EmbeddedPromoVideo",
                table: "ResearchGroups");

            migrationBuilder.DropColumn(
                name: "ResearchGroupTelephoneNumber_YouTubeChannel",
                table: "ResearchGroups");

            migrationBuilder.DropColumn(
                name: "ResearchGroup_Facebook",
                table: "ResearchGroups");

            migrationBuilder.DropColumn(
                name: "ResearchGroup_LinkedIn",
                table: "ResearchGroups");

            migrationBuilder.DropColumn(
                name: "ResearchGroup_PresentationAttachment",
                table: "ResearchGroups");

            migrationBuilder.DropColumn(
                name: "ResearchGroup_Twitter",
                table: "ResearchGroups");

            migrationBuilder.DropColumn(
                name: "ResearchGroup_Website",
                table: "ResearchGroups");
        }
    }
}
