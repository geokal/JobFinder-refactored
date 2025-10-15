using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class RemoveCompanyEmailUniqueConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Drop the problematic unique index
            migrationBuilder.DropIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails");

            // 2. Optionally create a non-unique index if you need it for query performance
            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails",
                column: "CompanysEmailWhereStudentAppliedForCompanyJob",
                unique: false); // This is now non-unique
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // For rollback, recreate the unique index
            migrationBuilder.DropIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails",
                column: "CompanysEmailWhereStudentAppliedForCompanyJob",
                unique: true);
        }
    }
}
