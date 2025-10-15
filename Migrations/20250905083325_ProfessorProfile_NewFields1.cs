using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class ProfessorProfile_NewFields1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfFEK",
                table: "Professors",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfGnostikoAntikeimeno",
                table: "Professors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfLab",
                table: "Professors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfLabFEK",
                table: "Professors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfLabFEK_AttachmentFile",
                table: "Professors",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfResearchGroup",
                table: "Professors",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfFEK",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "ProfGnostikoAntikeimeno",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "ProfLab",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "ProfLabFEK",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "ProfLabFEK_AttachmentFile",
                table: "Professors");

            migrationBuilder.DropColumn(
                name: "ProfResearchGroup",
                table: "Professors");
        }
    }
}
