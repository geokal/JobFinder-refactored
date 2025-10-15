using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class UpdateCompanyJobAppliedRelationships2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. First drop all dependencies in the correct order

            // Drop foreign keys that reference these tables
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyJobApplied_CompanyDetails_CompanyJobsApplied_Id",
                table: "CompanyJobApplied_CompanyDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyJobApplied_StudentDetails_CompanyJobsApplied_Id",
                table: "CompanyJobApplied_StudentDetails");

            // Drop indexes that depend on these columns
            migrationBuilder.DropIndex(
                name: "IX_CompanyJobApplied_StudentDetails_StudentEmailAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails");

            // Drop primary key constraints before modifying the columns
            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyJobApplied_StudentDetails",
                table: "CompanyJobApplied_StudentDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyJobApplied_CompanyDetails",
                table: "CompanyJobApplied_CompanyDetails");

            // 2. Now modify the tables

            // First modify CompanyJobsApplied to prepare for new relationships
            migrationBuilder.AlterColumn<string>(
                name: "StudentEmailAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // Handle CompanyJobApplied_StudentDetails
            migrationBuilder.DropColumn(
                name: "Id",
                table: "CompanyJobApplied_StudentDetails");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CompanyJobApplied_StudentDetails",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "StudentUniqueIDAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // Handle CompanyJobApplied_CompanyDetails
            migrationBuilder.DropColumn(
                name: "Id",
                table: "CompanyJobApplied_CompanyDetails");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CompanyJobApplied_CompanyDetails",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            // 3. Recreate constraints and relationships

            // Add primary keys back
            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyJobApplied_StudentDetails",
                table: "CompanyJobApplied_StudentDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyJobApplied_CompanyDetails",
                table: "CompanyJobApplied_CompanyDetails",
                column: "Id");

            // Add alternate keys to CompanyJobsApplied for the new relationships
            migrationBuilder.AddUniqueConstraint(
                name: "AK_CompanyJobsApplied_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                column: "CompanysEmailWhereStudentAppliedForCompanyJob");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CompanyJobsApplied_StudentEmailAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                column: "StudentEmailAppliedForCompanyJob");

            // Create new indexes
            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_StudentDetails_StudentEmailAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails",
                column: "StudentEmailAppliedForCompanyJob",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_StudentDetails_StudentUniqueIDAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails",
                column: "StudentUniqueIDAppliedForCompanyJob");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails",
                column: "CompanysEmailWhereStudentAppliedForCompanyJob",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails",
                column: "CompanysUniqueIDWhereStudentAppliedForCompanyJob");

            // Add new foreign key relationships
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Drop all new dependencies first

            // Drop foreign keys
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyJobApplied_CompanyDetails_CompanyJobsApplied_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyJobApplied_StudentDetails_CompanyJobsApplied_StudentEmailAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails");

            // Drop unique constraints
            migrationBuilder.DropUniqueConstraint(
                name: "AK_CompanyJobsApplied_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobsApplied");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CompanyJobsApplied_StudentEmailAppliedForCompanyJob",
                table: "CompanyJobsApplied");

            // Drop indexes
            migrationBuilder.DropIndex(
                name: "IX_CompanyJobApplied_StudentDetails_StudentEmailAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompanyJobApplied_StudentDetails_StudentUniqueIDAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails");

            // Drop primary keys
            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyJobApplied_StudentDetails",
                table: "CompanyJobApplied_StudentDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyJobApplied_CompanyDetails",
                table: "CompanyJobApplied_CompanyDetails");

            // 2. Revert column changes

            // Revert CompanyJobsApplied columns
            migrationBuilder.AlterColumn<string>(
                name: "StudentEmailAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            // Revert CompanyJobApplied_StudentDetails
            migrationBuilder.DropColumn(
                name: "Id",
                table: "CompanyJobApplied_StudentDetails");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CompanyJobApplied_StudentDetails",
                type: "int",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "StudentUniqueIDAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            // Revert CompanyJobApplied_CompanyDetails
            migrationBuilder.DropColumn(
                name: "Id",
                table: "CompanyJobApplied_CompanyDetails");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CompanyJobApplied_CompanyDetails",
                type: "int",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            // 3. Recreate original constraints and relationships

            // Add primary keys back
            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyJobApplied_StudentDetails",
                table: "CompanyJobApplied_StudentDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyJobApplied_CompanyDetails",
                table: "CompanyJobApplied_CompanyDetails",
                column: "Id");

            // Recreate indexes
            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_StudentDetails_StudentEmailAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails",
                column: "StudentEmailAppliedForCompanyJob");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails",
                column: "CompanysEmailWhereStudentAppliedForCompanyJob");

            // Recreate original foreign keys
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
        }
    }
}