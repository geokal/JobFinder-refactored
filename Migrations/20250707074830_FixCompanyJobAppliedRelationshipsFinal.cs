using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class FixCompanyJobAppliedRelationshipsFinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. First drop ALL dependent foreign keys
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyJobApplied_CompanyDetails_CompanyJobsApplied_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyJobApplied_StudentDetails_CompanyJobsApplied_StudentEmailAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails");

            // 2. Drop ALL problematic constraints
            migrationBuilder.DropUniqueConstraint(
                name: "AK_CompanyJobsApplied_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobsApplied");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CompanyJobsApplied_StudentEmailAppliedForCompanyJob",
                table: "CompanyJobsApplied");

            // 3. Recreate the relationships using proper keys
            migrationBuilder.AddForeignKey(
                name: "FK_CompanyJobApplied_CompanyDetails_CompanyJobsApplied_Id",
                table: "CompanyJobApplied_CompanyDetails",
                column: "Id",
                principalTable: "CompanyJobsApplied",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyJobApplied_StudentDetails_CompanyJobsApplied_Id",
                table: "CompanyJobApplied_StudentDetails",
                column: "Id",
                principalTable: "CompanyJobsApplied",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // 4. Add ONLY the composite unique constraint we actually want
            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobsApplied_StudentEmail_RNG",
                table: "CompanyJobsApplied",
                columns: new[] { "StudentEmailAppliedForCompanyJob", "RNGForCompanyJobApplied" },
                unique: true,
                filter: "[StudentEmailAppliedForCompanyJob] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyJobsApplied_StudentEmail_RNG",
                table: "CompanyJobsApplied");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyJobApplied_CompanyDetails_CompanyJobsApplied_Id",
                table: "CompanyJobApplied_CompanyDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyJobApplied_StudentDetails_CompanyJobsApplied_Id",
                table: "CompanyJobApplied_StudentDetails");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CompanyJobsApplied_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                column: "CompanysEmailWhereStudentAppliedForCompanyJob");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CompanyJobsApplied_StudentEmailAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                column: "StudentEmailAppliedForCompanyJob");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyJobApplied_CompanyDetails_CompanyJobsApplied_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails",
                column: "CompanysEmailWhereStudentAppliedForCompanyJob",
                principalTable: "CompanyJobsApplied",
                principalColumn: "CompanysEmailWhereStudentAppliedForCompanyJob",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyJobApplied_StudentDetails_CompanyJobsApplied_StudentEmailAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails",
                column: "StudentEmailAppliedForCompanyJob",
                principalTable: "CompanyJobsApplied",
                principalColumn: "StudentEmailAppliedForCompanyJob",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
