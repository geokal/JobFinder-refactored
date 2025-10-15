using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class CompanyJobsApplied_NEW1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyAppliedForPosition",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "CompanyEmailAppliedForPosition",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "CompanyPositionStatusApplied",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "CompanyPositionStatusAppliedAtTheCompany",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "CompanyPositionTypeApplied",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "PositionInCompanyApplied",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "PositionTitleAppliedAtTheCompany",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "RNGForPositionApplied",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "RNGForPositionApplied_HashedAsUniqueID",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "StudentCVApplied",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "StudentEmailApplied",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "StudentImageApplied",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "StudentNameApplied",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "StudentStudyYearAppliedForJob",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "StudentSurnameApplied",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "StudentTelephoneAppliedForJob",
                table: "CompanyJobsApplied");

            migrationBuilder.RenameColumn(
                name: "StudentRegNumberApplied",
                table: "CompanyJobsApplied",
                newName: "RNGForCompanyJobApplied");

            migrationBuilder.RenameColumn(
                name: "DateTimeStudentAppliedForPosition",
                table: "CompanyJobsApplied",
                newName: "DateTimeStudentAppliedForCompanyJob");

            migrationBuilder.AddColumn<string>(
                name: "CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentEmailAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentUniqueIDAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CompanyJobsApplied_CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                column: "CompanysUniqueIDWhereStudentAppliedForCompanyJob");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CompanyJobsApplied_StudentUniqueIDAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                column: "StudentUniqueIDAppliedForCompanyJob");

            migrationBuilder.CreateTable(
                name: "CompanyJobApplied_CompanyDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanysUniqueIDWhereStudentAppliedForCompanyJob = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanysEmailWhereStudentAppliedForCompanyJob = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyPositionStatusAppliedAtTheCompanySide = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyJobApplied_CompanyDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyJobApplied_CompanyDetails_CompanyJobsApplied_CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                        column: x => x.CompanysUniqueIDWhereStudentAppliedForCompanyJob,
                        principalTable: "CompanyJobsApplied",
                        principalColumn: "CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyJobApplied_StudentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentUniqueIDAppliedForCompanyJob = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentEmailAppliedForCompanyJob = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyPositionStatusAppliedAtTheStudentSide = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTimeStudentAppliedForCompanyJob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyJobApplied_StudentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyJobApplied_StudentDetails_CompanyJobsApplied_StudentUniqueIDAppliedForCompanyJob",
                        column: x => x.StudentUniqueIDAppliedForCompanyJob,
                        principalTable: "CompanyJobsApplied",
                        principalColumn: "StudentUniqueIDAppliedForCompanyJob",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                table: "CompanyJobApplied_CompanyDetails",
                column: "CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_CompanyDetails_RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID",
                table: "CompanyJobApplied_CompanyDetails",
                column: "RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_StudentDetails_RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID",
                table: "CompanyJobApplied_StudentDetails",
                column: "RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyJobApplied_StudentDetails_StudentUniqueIDAppliedForCompanyJob",
                table: "CompanyJobApplied_StudentDetails",
                column: "StudentUniqueIDAppliedForCompanyJob",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyJobApplied_CompanyDetails");

            migrationBuilder.DropTable(
                name: "CompanyJobApplied_StudentDetails");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CompanyJobsApplied_CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                table: "CompanyJobsApplied");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CompanyJobsApplied_StudentUniqueIDAppliedForCompanyJob",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "CompanysEmailWhereStudentAppliedForCompanyJob",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "CompanysUniqueIDWhereStudentAppliedForCompanyJob",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "StudentEmailAppliedForCompanyJob",
                table: "CompanyJobsApplied");

            migrationBuilder.DropColumn(
                name: "StudentUniqueIDAppliedForCompanyJob",
                table: "CompanyJobsApplied");

            migrationBuilder.RenameColumn(
                name: "RNGForCompanyJobApplied",
                table: "CompanyJobsApplied",
                newName: "StudentRegNumberApplied");

            migrationBuilder.RenameColumn(
                name: "DateTimeStudentAppliedForCompanyJob",
                table: "CompanyJobsApplied",
                newName: "DateTimeStudentAppliedForPosition");

            migrationBuilder.AddColumn<string>(
                name: "CompanyAppliedForPosition",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEmailAppliedForPosition",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyPositionStatusApplied",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyPositionStatusAppliedAtTheCompany",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyPositionTypeApplied",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PositionInCompanyApplied",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PositionTitleAppliedAtTheCompany",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RNGForPositionApplied",
                table: "CompanyJobsApplied",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "RNGForPositionApplied_HashedAsUniqueID",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "StudentCVApplied",
                table: "CompanyJobsApplied",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentEmailApplied",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "StudentImageApplied",
                table: "CompanyJobsApplied",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentNameApplied",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentStudyYearAppliedForJob",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentSurnameApplied",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentTelephoneAppliedForJob",
                table: "CompanyJobsApplied",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
