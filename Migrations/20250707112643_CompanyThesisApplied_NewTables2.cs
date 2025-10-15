using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class CompanyThesisApplied_NewTables2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyNameWhereStudentAppliedForInternship",
                table: "InternshipApplied_CompanyDetails");

            migrationBuilder.DropColumn(
                name: "InternshipPositionInCompanyApplied",
                table: "InternshipApplied_CompanyDetails");

            migrationBuilder.DropColumn(
                name: "InternshipPositionTypeApplied",
                table: "InternshipApplied_CompanyDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyNameWhereStudentAppliedForInternship",
                table: "InternshipApplied_CompanyDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InternshipPositionInCompanyApplied",
                table: "InternshipApplied_CompanyDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InternshipPositionTypeApplied",
                table: "InternshipApplied_CompanyDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
