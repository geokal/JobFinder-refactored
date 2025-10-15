using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class CompanyThesis_NewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyNameUploadedThesis",
                table: "CompanyTheses");

            migrationBuilder.DropColumn(
                name: "ProfDepartmentInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.DropColumn(
                name: "ProfGeneralFieldOfWorkInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.DropColumn(
                name: "ProfGeneralSkillsInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.DropColumn(
                name: "ProfImageInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.DropColumn(
                name: "ProfLinkedInSiteInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.DropColumn(
                name: "ProfOrchidProfileInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.DropColumn(
                name: "ProfPersonalDescriptionInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.DropColumn(
                name: "ProfPersonalTelephoneInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.DropColumn(
                name: "ProfPersonalTelephoneVisibilityInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.DropColumn(
                name: "ProfPersonalWebsiteInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.DropColumn(
                name: "ProfScholarProfileInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.DropColumn(
                name: "ProfUniversityInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.DropColumn(
                name: "ProfVahmidaDEPInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.DropColumn(
                name: "ProfWorkTelephoneInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.DropColumn(
                name: "ProfessorNameInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.DropColumn(
                name: "ProfessorSurnNameInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.AlterColumn<string>(
                name: "ProfEmail",
                table: "Professors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RNGForThesisUploadedAsCompany_HashedAsUniqueID",
                table: "CompanyTheses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfessorEmailInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyEmailUsedToUploadThesis",
                table: "CompanyTheses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Professors_ProfEmail",
                table: "Professors",
                column: "ProfEmail");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTheses_CompanyEmailUsedToUploadThesis",
                table: "CompanyTheses",
                column: "CompanyEmailUsedToUploadThesis");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTheses_ProfessorEmailInterestedInCompanyThesis",
                table: "CompanyTheses",
                column: "ProfessorEmailInterestedInCompanyThesis");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTheses_RNGForThesisUploadedAsCompany_HashedAsUniqueID",
                table: "CompanyTheses",
                column: "RNGForThesisUploadedAsCompany_HashedAsUniqueID",
                unique: true,
                filter: "[RNGForThesisUploadedAsCompany_HashedAsUniqueID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyTheses_Companies_CompanyEmailUsedToUploadThesis",
                table: "CompanyTheses",
                column: "CompanyEmailUsedToUploadThesis",
                principalTable: "Companies",
                principalColumn: "CompanyEmail",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyTheses_Professors_ProfessorEmailInterestedInCompanyThesis",
                table: "CompanyTheses",
                column: "ProfessorEmailInterestedInCompanyThesis",
                principalTable: "Professors",
                principalColumn: "ProfEmail",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyTheses_Companies_CompanyEmailUsedToUploadThesis",
                table: "CompanyTheses");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyTheses_Professors_ProfessorEmailInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Professors_ProfEmail",
                table: "Professors");

            migrationBuilder.DropIndex(
                name: "IX_CompanyTheses_CompanyEmailUsedToUploadThesis",
                table: "CompanyTheses");

            migrationBuilder.DropIndex(
                name: "IX_CompanyTheses_ProfessorEmailInterestedInCompanyThesis",
                table: "CompanyTheses");

            migrationBuilder.DropIndex(
                name: "IX_CompanyTheses_RNGForThesisUploadedAsCompany_HashedAsUniqueID",
                table: "CompanyTheses");

            migrationBuilder.AlterColumn<string>(
                name: "ProfEmail",
                table: "Professors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "RNGForThesisUploadedAsCompany_HashedAsUniqueID",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfessorEmailInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyEmailUsedToUploadThesis",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyNameUploadedThesis",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfDepartmentInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfGeneralFieldOfWorkInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfGeneralSkillsInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfImageInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfLinkedInSiteInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfOrchidProfileInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfPersonalDescriptionInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfPersonalTelephoneInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ProfPersonalTelephoneVisibilityInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProfPersonalWebsiteInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfScholarProfileInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfUniversityInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfVahmidaDEPInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfWorkTelephoneInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorNameInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorSurnNameInterestedInCompanyThesis",
                table: "CompanyTheses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
