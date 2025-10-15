using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class UpdateCompanyJobAppliedRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("BEGIN TRANSACTION");

            try
            {
                // 1. Drop foreign key constraints
                migrationBuilder.DropForeignKey(
                    name: "FK_CompanyJobApplied_CompanyDetails_CompanyJobsApplied_CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                    table: "CompanyJobApplied_CompanyDetails");

                migrationBuilder.DropForeignKey(
                    name: "FK_CompanyJobApplied_StudentDetails_CompanyJobsApplied_StudentUniqueIDAppliedForCompanyJob",
                    table: "CompanyJobApplied_StudentDetails");

                // 2. Drop primary keys
                migrationBuilder.DropPrimaryKey(
                    name: "PK_CompanyJobApplied_StudentDetails",
                    table: "CompanyJobApplied_StudentDetails");

                migrationBuilder.DropPrimaryKey(
                    name: "PK_CompanyJobApplied_CompanyDetails",
                    table: "CompanyJobApplied_CompanyDetails");

                // 3. Drop other constraints
                migrationBuilder.DropUniqueConstraint(
                    name: "AK_CompanyJobsApplied_CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                    table: "CompanyJobsApplied");

                migrationBuilder.DropUniqueConstraint(
                    name: "AK_CompanyJobsApplied_StudentUniqueIDAppliedForCompanyJob",
                    table: "CompanyJobsApplied");

                migrationBuilder.DropIndex(
                    name: "IX_CompanyJobApplied_StudentDetails_StudentUniqueIDAppliedForCompanyJob",
                    table: "CompanyJobApplied_StudentDetails");

                migrationBuilder.DropIndex(
                    name: "IX_CompanyJobApplied_CompanyDetails_CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                    table: "CompanyJobApplied_CompanyDetails");

                // 4. Create temp columns and migrate data
                migrationBuilder.AddColumn<int>(
                    name: "TempId",
                    table: "CompanyJobApplied_StudentDetails",
                    nullable: false);

                migrationBuilder.AddColumn<int>(
                    name: "TempId",
                    table: "CompanyJobApplied_CompanyDetails",
                    nullable: false);

                migrationBuilder.Sql(@"
                    UPDATE CompanyJobApplied_StudentDetails SET TempId = Id;
                    UPDATE CompanyJobApplied_CompanyDetails SET TempId = Id;
                ");

                // 5. Drop and recreate Id columns
                migrationBuilder.DropColumn(
                    name: "Id",
                    table: "CompanyJobApplied_StudentDetails");

                migrationBuilder.DropColumn(
                    name: "Id",
                    table: "CompanyJobApplied_CompanyDetails");

                migrationBuilder.AddColumn<int>(
                    name: "Id",
                    table: "CompanyJobApplied_StudentDetails",
                    nullable: false);

                migrationBuilder.AddColumn<int>(
                    name: "Id",
                    table: "CompanyJobApplied_CompanyDetails",
                    nullable: false);

                migrationBuilder.Sql(@"
                    UPDATE CompanyJobApplied_StudentDetails SET Id = TempId;
                    UPDATE CompanyJobApplied_CompanyDetails SET Id = TempId;
                ");

                // 6. Recreate primary keys
                migrationBuilder.AddPrimaryKey(
                    name: "PK_CompanyJobApplied_StudentDetails",
                    table: "CompanyJobApplied_StudentDetails",
                    column: "Id");

                migrationBuilder.AddPrimaryKey(
                    name: "PK_CompanyJobApplied_CompanyDetails",
                    table: "CompanyJobApplied_CompanyDetails",
                    column: "Id");

                // 7. Clean up temp columns
                migrationBuilder.DropColumn(
                    name: "TempId",
                    table: "CompanyJobApplied_StudentDetails");

                migrationBuilder.DropColumn(
                    name: "TempId",
                    table: "CompanyJobApplied_CompanyDetails");

                // 8. Change column types
                migrationBuilder.AlterColumn<string>(
                    name: "StudentUniqueIDAppliedForCompanyJob",
                    table: "CompanyJobsApplied",
                    type: "nvarchar(max)",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(450)");

                migrationBuilder.AlterColumn<string>(
                    name: "CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                    table: "CompanyJobsApplied",
                    type: "nvarchar(max)",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(450)");

                // 9. Add new foreign keys
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

                migrationBuilder.Sql("COMMIT");
            }
            catch
            {
                migrationBuilder.Sql("ROLLBACK");
                throw;
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("BEGIN TRANSACTION");

            try
            {
                // 1. Drop new foreign keys
                migrationBuilder.DropForeignKey(
                    name: "FK_CompanyJobApplied_CompanyDetails_CompanyJobsApplied_Id",
                    table: "CompanyJobApplied_CompanyDetails");

                migrationBuilder.DropForeignKey(
                    name: "FK_CompanyJobApplied_StudentDetails_CompanyJobsApplied_Id",
                    table: "CompanyJobApplied_StudentDetails");

                // 2. Drop primary keys
                migrationBuilder.DropPrimaryKey(
                    name: "PK_CompanyJobApplied_StudentDetails",
                    table: "CompanyJobApplied_StudentDetails");

                migrationBuilder.DropPrimaryKey(
                    name: "PK_CompanyJobApplied_CompanyDetails",
                    table: "CompanyJobApplied_CompanyDetails");

                // 3. Create temp columns for reverting
                migrationBuilder.AddColumn<int>(
                    name: "TempId",
                    table: "CompanyJobApplied_StudentDetails",
                    nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1");

                migrationBuilder.AddColumn<int>(
                    name: "TempId",
                    table: "CompanyJobApplied_CompanyDetails",
                    nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1");

                migrationBuilder.Sql(@"
                    UPDATE CompanyJobApplied_StudentDetails SET TempId = Id;
                    UPDATE CompanyJobApplied_CompanyDetails SET TempId = Id;
                ");

                // 4. Drop and recreate Id columns with IDENTITY
                migrationBuilder.DropColumn(
                    name: "Id",
                    table: "CompanyJobApplied_StudentDetails");

                migrationBuilder.DropColumn(
                    name: "Id",
                    table: "CompanyJobApplied_CompanyDetails");

                migrationBuilder.AddColumn<int>(
                    name: "Id",
                    table: "CompanyJobApplied_StudentDetails",
                    nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1");

                migrationBuilder.AddColumn<int>(
                    name: "Id",
                    table: "CompanyJobApplied_CompanyDetails",
                    nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1");

                migrationBuilder.Sql(@"
                    UPDATE CompanyJobApplied_StudentDetails SET Id = TempId;
                    UPDATE CompanyJobApplied_CompanyDetails SET Id = TempId;
                ");

                // 5. Recreate primary keys
                migrationBuilder.AddPrimaryKey(
                    name: "PK_CompanyJobApplied_StudentDetails",
                    table: "CompanyJobApplied_StudentDetails",
                    column: "Id");

                migrationBuilder.AddPrimaryKey(
                    name: "PK_CompanyJobApplied_CompanyDetails",
                    table: "CompanyJobApplied_CompanyDetails",
                    column: "Id");

                // 6. Clean up temp columns
                migrationBuilder.DropColumn(
                    name: "TempId",
                    table: "CompanyJobApplied_StudentDetails");

                migrationBuilder.DropColumn(
                    name: "TempId",
                    table: "CompanyJobApplied_CompanyDetails");

                // 7. Restore column types
                migrationBuilder.AlterColumn<string>(
                    name: "StudentUniqueIDAppliedForCompanyJob",
                    table: "CompanyJobsApplied",
                    type: "nvarchar(450)",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(max)");

                migrationBuilder.AlterColumn<string>(
                    name: "CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                    table: "CompanyJobsApplied",
                    type: "nvarchar(450)",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "nvarchar(max)");

                // 8. Recreate constraints and indexes
                migrationBuilder.AddUniqueConstraint(
                    name: "AK_CompanyJobsApplied_CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                    table: "CompanyJobsApplied",
                    column: "CompanysUniqueIDWhereStudentAppliedForCompanyJob");

                migrationBuilder.AddUniqueConstraint(
                    name: "AK_CompanyJobsApplied_StudentUniqueIDAppliedForCompanyJob",
                    table: "CompanyJobsApplied",
                    column: "StudentUniqueIDAppliedForCompanyJob");

                migrationBuilder.CreateIndex(
                    name: "IX_CompanyJobApplied_StudentDetails_StudentUniqueIDAppliedForCompanyJob",
                    table: "CompanyJobApplied_StudentDetails",
                    column: "StudentUniqueIDAppliedForCompanyJob",
                    unique: true);

                migrationBuilder.CreateIndex(
                    name: "IX_CompanyJobApplied_CompanyDetails_CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                    table: "CompanyJobApplied_CompanyDetails",
                    column: "CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                    unique: true);

                // 9. Restore original foreign keys
                migrationBuilder.AddForeignKey(
                    name: "FK_CompanyJobApplied_CompanyDetails_CompanyJobsApplied_CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                    table: "CompanyJobApplied_CompanyDetails",
                    column: "CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                    principalTable: "CompanyJobsApplied",
                    principalColumn: "CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                    onDelete: ReferentialAction.Cascade);

                migrationBuilder.AddForeignKey(
                    name: "FK_CompanyJobApplied_StudentDetails_CompanyJobsApplied_StudentUniqueIDAppliedForCompanyJob",
                    table: "CompanyJobApplied_StudentDetails",
                    column: "StudentUniqueIDAppliedForCompanyJob",
                    principalTable: "CompanyJobsApplied",
                    principalColumn: "StudentUniqueIDAppliedForCompanyJob",
                    onDelete: ReferentialAction.Cascade);

                migrationBuilder.Sql("COMMIT");
            }
            catch
            {
                migrationBuilder.Sql("ROLLBACK");
                throw;
            }
        }
    }
}