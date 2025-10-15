using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class UpdateCompanyJobAppliedConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StudentEmailAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // Check if index exists before creating it
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes 
                               WHERE name = 'IX_CompanyJobApplied_StudentDetails_StudentEmailAppliedForCompanyJob' 
                               AND object_id = OBJECT_ID('CompanyJobApplied_StudentDetails'))
                BEGIN
                    CREATE INDEX [IX_CompanyJobApplied_StudentDetails_StudentEmailAppliedForCompanyJob] 
                    ON [CompanyJobApplied_StudentDetails] ([StudentEmailAppliedForCompanyJob]);
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyJobApplied_StudentDetails_StudentEmailAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails");

            migrationBuilder.AlterColumn<string>(
                name: "StudentEmailAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}