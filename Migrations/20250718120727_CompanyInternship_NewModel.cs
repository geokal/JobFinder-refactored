using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class CompanyInternship_NewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyNameUsedToUploadInternship",
                table: "CompanyInternships");

            migrationBuilder.DropColumn(
                name: "EmailUsedToUploadInternship",
                table: "CompanyInternships");

            migrationBuilder.DropColumn(
                name: "RNGForInternshipAppliedInCompanyInternship_HashedAsUniqueID",
                table: "CompanyInternships");

            migrationBuilder.RenameColumn(
                name: "RNGForInternshipAppliedInCompanyInternship",
                table: "CompanyInternships",
                newName: "RNGForInternshipUploadedAsCompany");

            migrationBuilder.AddColumn<string>(
                name: "CompanyEmailUsedToUploadInternship",
                table: "CompanyInternships",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RNGForInternshipUploadedAsCompany_HashedAsUniqueID",
                table: "CompanyInternships",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyInternships_CompanyEmailUsedToUploadInternship",
                table: "CompanyInternships",
                column: "CompanyEmailUsedToUploadInternship");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyInternships_RNGForInternshipUploadedAsCompany_HashedAsUniqueID",
                table: "CompanyInternships",
                column: "RNGForInternshipUploadedAsCompany_HashedAsUniqueID",
                unique: true,
                filter: "[RNGForInternshipUploadedAsCompany_HashedAsUniqueID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyInternships_Companies_CompanyEmailUsedToUploadInternship",
                table: "CompanyInternships",
                column: "CompanyEmailUsedToUploadInternship",
                principalTable: "Companies",
                principalColumn: "CompanyEmail",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyInternships_Companies_CompanyEmailUsedToUploadInternship",
                table: "CompanyInternships");

            migrationBuilder.DropIndex(
                name: "IX_CompanyInternships_CompanyEmailUsedToUploadInternship",
                table: "CompanyInternships");

            migrationBuilder.DropIndex(
                name: "IX_CompanyInternships_RNGForInternshipUploadedAsCompany_HashedAsUniqueID",
                table: "CompanyInternships");

            migrationBuilder.DropColumn(
                name: "CompanyEmailUsedToUploadInternship",
                table: "CompanyInternships");

            migrationBuilder.DropColumn(
                name: "RNGForInternshipUploadedAsCompany_HashedAsUniqueID",
                table: "CompanyInternships");

            migrationBuilder.RenameColumn(
                name: "RNGForInternshipUploadedAsCompany",
                table: "CompanyInternships",
                newName: "RNGForInternshipAppliedInCompanyInternship");

            migrationBuilder.AddColumn<string>(
                name: "CompanyNameUsedToUploadInternship",
                table: "CompanyInternships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailUsedToUploadInternship",
                table: "CompanyInternships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RNGForInternshipAppliedInCompanyInternship_HashedAsUniqueID",
                table: "CompanyInternships",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
