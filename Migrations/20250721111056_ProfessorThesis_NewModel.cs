using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class ProfessorThesis_NewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyActivityInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyAreasInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyCountryInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyDescriptionInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyDesiredSkillsInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyHREmailInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyHRNameInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyHRSurnameInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyHRTelephoneInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyLocationInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyLogoInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyNameENGInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyPCInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyRegionsInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyTelephoneInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyTownInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyTypeInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyWebsiteAnnouncementsInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyWebsiteInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "CompanyWebsiteJobsInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "EmailUsedToUploadThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "ProfessorDepartment",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "ProfessorName",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "ProfessorSurnname",
                table: "ProfessorTheses");

            migrationBuilder.AlterColumn<string>(
                name: "RNGForThesisUploaded_HashedAsUniqueID",
                table: "ProfessorTheses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyEmailInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEmailUsedToUploadThesis",
                table: "ProfessorTheses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorTheses_CompanyEmailInterestedInProfessorThesis",
                table: "ProfessorTheses",
                column: "CompanyEmailInterestedInProfessorThesis");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorTheses_ProfessorEmailUsedToUploadThesis",
                table: "ProfessorTheses",
                column: "ProfessorEmailUsedToUploadThesis");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorTheses_RNGForThesisUploaded_HashedAsUniqueID",
                table: "ProfessorTheses",
                column: "RNGForThesisUploaded_HashedAsUniqueID",
                unique: true,
                filter: "[RNGForThesisUploaded_HashedAsUniqueID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessorTheses_Companies_CompanyEmailInterestedInProfessorThesis",
                table: "ProfessorTheses",
                column: "CompanyEmailInterestedInProfessorThesis",
                principalTable: "Companies",
                principalColumn: "CompanyEmail",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessorTheses_Professors_ProfessorEmailUsedToUploadThesis",
                table: "ProfessorTheses",
                column: "ProfessorEmailUsedToUploadThesis",
                principalTable: "Professors",
                principalColumn: "ProfEmail",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfessorTheses_Companies_CompanyEmailInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessorTheses_Professors_ProfessorEmailUsedToUploadThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropIndex(
                name: "IX_ProfessorTheses_CompanyEmailInterestedInProfessorThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropIndex(
                name: "IX_ProfessorTheses_ProfessorEmailUsedToUploadThesis",
                table: "ProfessorTheses");

            migrationBuilder.DropIndex(
                name: "IX_ProfessorTheses_RNGForThesisUploaded_HashedAsUniqueID",
                table: "ProfessorTheses");

            migrationBuilder.DropColumn(
                name: "ProfessorEmailUsedToUploadThesis",
                table: "ProfessorTheses");

            migrationBuilder.AlterColumn<string>(
                name: "RNGForThesisUploaded_HashedAsUniqueID",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyEmailInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyActivityInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyAreasInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCountryInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyDescriptionInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyDesiredSkillsInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyHREmailInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyHRNameInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyHRSurnameInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyHRTelephoneInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyLocationInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "CompanyLogoInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyNameENGInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CompanyPCInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyRegionsInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyTelephoneInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyTownInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyTypeInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyWebsiteAnnouncementsInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyWebsiteInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyWebsiteJobsInterestedInProfessorThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailUsedToUploadThesis",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorDepartment",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorName",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorSurnname",
                table: "ProfessorTheses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
