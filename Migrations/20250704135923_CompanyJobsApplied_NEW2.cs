using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class CompanyJobsApplied_NEW2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyPositionStatusAppliedAtTheStudentSide",
                table: "CompanyJobApplied_StudentDetails");

            migrationBuilder.DropColumn(
                name: "CompanyPositionStatusAppliedAtTheCompanySide",
                table: "CompanyJobApplied_CompanyDetails");

            migrationBuilder.AddColumn<string>(
                name: "CompanyPositionStatusAppliedAtTheCompanySide",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyPositionStatusAppliedAtTheStudentSide",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyPositionStatusAppliedAtTheCompanySide",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "CompanyPositionStatusAppliedAtTheStudentSide",
                table: "CompanyJobsApplied");

            migrationBuilder.AddColumn<string>(
                name: "CompanyPositionStatusAppliedAtTheStudentSide",
                table: "CompanyJobApplied_StudentDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyPositionStatusAppliedAtTheCompanySide",
                table: "CompanyJobApplied_CompanyDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
