using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class CompanyJobsApplied_NEW3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyJobApplied_StudentDetails_RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID",
                table: "CompanyJobApplied_StudentDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID",
                table: "CompanyJobApplied_CompanyDetails");

            migrationBuilder.DropColumn(
                name: "RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID",
                table: "CompanyJobApplied_CompanyDetails");

            migrationBuilder.AlterColumn<string>(
                name: "StudentEmailAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID",
                table: "CompanyJobApplied_StudentDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_StudentDetails_StudentEmailAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails",
                column: "StudentEmailAppliedForCompanyJob");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails",
                column: "CompanysEmailWhereStudentAppliedForCompanyJob");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyJobApplied_StudentDetails_StudentEmailAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails");

            migrationBuilder.AlterColumn<string>(
                name: "StudentEmailAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID",
                table: "CompanyJobApplied_StudentDetails",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID",
                table: "CompanyJobApplied_CompanyDetails",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_StudentDetails_RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID",
                table: "CompanyJobApplied_StudentDetails",
                column: "RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID",
                table: "CompanyJobApplied_CompanyDetails",
                column: "RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID");
        }
    }
}
