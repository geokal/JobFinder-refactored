using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class FixCompanyJobAppliedRelationshipsFinalSolution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. First create new non-IDENTITY columns as temporary holders
            migrationBuilder.AddColumn<int>(
                name: "TempId",
                table: "CompanyJobApplied_CompanyDetails",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "TempId",
                table: "CompanyJobApplied_StudentDetails",
                nullable: false);

            // 2. Copy data from old IDENTITY columns to new temp columns
            migrationBuilder.Sql(@"
        UPDATE CompanyJobApplied_CompanyDetails 
        SET TempId = Id");

            migrationBuilder.Sql(@"
        UPDATE CompanyJobApplied_StudentDetails 
        SET TempId = Id");

            // 3. Drop the old IDENTITY columns and constraints
            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyJobApplied_CompanyDetails",
                table: "CompanyJobApplied_CompanyDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyJobApplied_StudentDetails",
                table: "CompanyJobApplied_StudentDetails");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CompanyJobApplied_CompanyDetails");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CompanyJobApplied_StudentDetails");

            // 4. Rename temp columns to Id and make them primary keys
            migrationBuilder.RenameColumn(
                name: "TempId",
                table: "CompanyJobApplied_CompanyDetails",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "TempId",
                table: "CompanyJobApplied_StudentDetails",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyJobApplied_CompanyDetails",
                table: "CompanyJobApplied_CompanyDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyJobApplied_StudentDetails",
                table: "CompanyJobApplied_StudentDetails",
                column: "Id");

            // 5. Recreate relationships using the new Id columns
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

            // 6. Add the composite unique constraint
            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobsApplied_StudentEmail_RNG",
                table: "CompanyJobsApplied",
                columns: new[] { "StudentEmailAppliedForCompanyJob", "RNGForCompanyJobApplied" },
                unique: true,
                filter: "[StudentEmailAppliedForCompanyJob] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyJobApplied_CompanyDetails_CompanyJobsApplied_Id",
                table: "CompanyJobApplied_CompanyDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyJobApplied_StudentDetails_CompanyJobsApplied_Id",
                table: "CompanyJobApplied_StudentDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails");

            migrationBuilder.AlterColumn<string>(
                name: "CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "StudentEmailAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CompanyJobApplied_StudentDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CompanyJobApplied_CompanyDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CompanyJobsApplied_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                column: "CompanysEmailWhereStudentAppliedForCompanyJob");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CompanyJobsApplied_StudentEmailAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                column: "StudentEmailAppliedForCompanyJob");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_StudentDetails_StudentEmailAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails",
                column: "StudentEmailAppliedForCompanyJob",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails",
                column: "CompanysEmailWhereStudentAppliedForCompanyJob",
                unique: true);

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
