using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class ProfessorInternship_NewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailUsedToUploadProfessorInternship",
                table: "ProfessorInternships");

            migrationBuilder.DropColumn(
                name: "ProfVahmidaDEPToUploadInternship",
                table: "ProfessorInternships");

            migrationBuilder.DropColumn(
                name: "ProfessorNameUsedToUploadInternship",
                table: "ProfessorInternships");

            migrationBuilder.DropColumn(
                name: "ProfessorProfGeneralFieldOfWorkToUploadInternship",
                table: "ProfessorInternships");

            migrationBuilder.DropColumn(
                name: "ProfessorSurnameUsedToUploadInternship",
                table: "ProfessorInternships");

            migrationBuilder.DropColumn(
                name: "ProfessorUniversityDepartmentToUploadInternship",
                table: "ProfessorInternships");

            migrationBuilder.DropColumn(
                name: "ProfessorUniversityToUploadInternship",
                table: "ProfessorInternships");

            migrationBuilder.DropColumn(
                name: "RNGForInternshipAppliedInProfessorInternship_HashedAsUniqueID",
                table: "ProfessorInternships");

            migrationBuilder.RenameColumn(
                name: "RNGForInternshipAppliedInProfessorInternship",
                table: "ProfessorInternships",
                newName: "RNGForInternshipUploadedAsProfessor");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEmailUsedToUploadInternship",
                table: "ProfessorInternships",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RNGForInternshipUploadedAsProfessor_HashedAsUniqueID",
                table: "ProfessorInternships",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorInternships_ProfessorEmailUsedToUploadInternship",
                table: "ProfessorInternships",
                column: "ProfessorEmailUsedToUploadInternship");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessorInternships_RNGForInternshipUploadedAsProfessor_HashedAsUniqueID",
                table: "ProfessorInternships",
                column: "RNGForInternshipUploadedAsProfessor_HashedAsUniqueID",
                unique: true,
                filter: "[RNGForInternshipUploadedAsProfessor_HashedAsUniqueID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessorInternships_Professors_ProfessorEmailUsedToUploadInternship",
                table: "ProfessorInternships",
                column: "ProfessorEmailUsedToUploadInternship",
                principalTable: "Professors",
                principalColumn: "ProfEmail",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfessorInternships_Professors_ProfessorEmailUsedToUploadInternship",
                table: "ProfessorInternships");

            migrationBuilder.DropIndex(
                name: "IX_ProfessorInternships_ProfessorEmailUsedToUploadInternship",
                table: "ProfessorInternships");

            migrationBuilder.DropIndex(
                name: "IX_ProfessorInternships_RNGForInternshipUploadedAsProfessor_HashedAsUniqueID",
                table: "ProfessorInternships");

            migrationBuilder.DropColumn(
                name: "ProfessorEmailUsedToUploadInternship",
                table: "ProfessorInternships");

            migrationBuilder.DropColumn(
                name: "RNGForInternshipUploadedAsProfessor_HashedAsUniqueID",
                table: "ProfessorInternships");

            migrationBuilder.RenameColumn(
                name: "RNGForInternshipUploadedAsProfessor",
                table: "ProfessorInternships",
                newName: "RNGForInternshipAppliedInProfessorInternship");

            migrationBuilder.AddColumn<string>(
                name: "EmailUsedToUploadProfessorInternship",
                table: "ProfessorInternships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfVahmidaDEPToUploadInternship",
                table: "ProfessorInternships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorNameUsedToUploadInternship",
                table: "ProfessorInternships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorProfGeneralFieldOfWorkToUploadInternship",
                table: "ProfessorInternships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorSurnameUsedToUploadInternship",
                table: "ProfessorInternships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorUniversityDepartmentToUploadInternship",
                table: "ProfessorInternships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorUniversityToUploadInternship",
                table: "ProfessorInternships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RNGForInternshipAppliedInProfessorInternship_HashedAsUniqueID",
                table: "ProfessorInternships",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
