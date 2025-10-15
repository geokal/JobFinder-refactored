using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class CompanyThesisApplied_UpdatedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyDepartmentAppliedForCompanyThesis",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "CompanyEmailAppliedForCompanyThesis",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "CompanyNameAppliedForCompanyThesis",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "CompanyThesisStatusAppliedAtTheCompanyByCompany",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "CompanyThesisStatusAppliedAtTheCompanyByStudent",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "RNGForCompanyThesisApplied_HashedAsUniqueID",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentCVAppliedForCompanyThesis",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentEmailAppliedForCompanyThesis",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentImageAppliedForCompanyThesis",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentNameAppliedForCompanyThesis",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentRegNumberAppliedForCompanyThesis",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentStudyYearAppliedForCompanyThesis",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentSurnameAppliedForCompanyThesis",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentTelephoneAppliedForCompanyThesis",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "ThesisTitleAppliedAtTheCompany",
                table: "CompanyThesesApplied");

            migrationBuilder.RenameColumn(
                name: "DateTimeStudentAppliedForCompanyThesis",
                table: "CompanyThesesApplied",
                newName: "DateTimeStudentAppliedForThesis");

            migrationBuilder.AddColumn<string>(
                name: "CompanyEmailWhereStudentAppliedForThesis",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyThesisStatusAppliedAtCompanySide",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyThesisStatusAppliedAtStudentSide",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyUniqueIDWhereStudentAppliedForThesis",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RNGForCompanyThesisAppliedAsStudent_HashedAsUniqueID",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentEmailAppliedForThesis",
                table: "CompanyThesesApplied",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentUniqueIDAppliedForThesis",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CompanyThesisApplied_CompanyDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CompanyUniqueIDWhereStudentAppliedForThesis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyEmailWhereStudentAppliedForThesis = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyThesisApplied_CompanyDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyThesisApplied_CompanyDetails_CompanyThesesApplied_Id",
                        column: x => x.Id,
                        principalTable: "CompanyThesesApplied",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyThesisApplied_StudentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    StudentUniqueIDAppliedForThesis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentEmailAppliedForThesis = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateTimeStudentAppliedForThesis = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RNGForCompanyThesisAppliedAsStudent_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyThesisApplied_StudentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyThesisApplied_StudentDetails_CompanyThesesApplied_Id",
                        column: x => x.Id,
                        principalTable: "CompanyThesesApplied",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyThesesApplied_StudentEmailAppliedForThesis_RNGForCompanyThesisApplied",
                table: "CompanyThesesApplied",
                columns: new[] { "StudentEmailAppliedForThesis", "RNGForCompanyThesisApplied" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyThesisApplied_CompanyDetails_CompanyEmailWhereStudentAppliedForThesis",
                table: "CompanyThesisApplied_CompanyDetails",
                column: "CompanyEmailWhereStudentAppliedForThesis");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyThesisApplied_StudentDetails_StudentEmailAppliedForThesis",
                table: "CompanyThesisApplied_StudentDetails",
                column: "StudentEmailAppliedForThesis");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyThesisApplied_CompanyDetails");

            migrationBuilder.DropTable(
                name: "CompanyThesisApplied_StudentDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompanyThesesApplied_StudentEmailAppliedForThesis_RNGForCompanyThesisApplied",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "CompanyEmailWhereStudentAppliedForThesis",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "CompanyThesisStatusAppliedAtCompanySide",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "CompanyThesisStatusAppliedAtStudentSide",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "CompanyUniqueIDWhereStudentAppliedForThesis",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "RNGForCompanyThesisAppliedAsStudent_HashedAsUniqueID",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentEmailAppliedForThesis",
                table: "CompanyThesesApplied");

            migrationBuilder.DropColumn(
                name: "StudentUniqueIDAppliedForThesis",
                table: "CompanyThesesApplied");

            migrationBuilder.RenameColumn(
                name: "DateTimeStudentAppliedForThesis",
                table: "CompanyThesesApplied",
                newName: "DateTimeStudentAppliedForCompanyThesis");

            migrationBuilder.AddColumn<string>(
                name: "CompanyDepartmentAppliedForCompanyThesis",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEmailAppliedForCompanyThesis",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyNameAppliedForCompanyThesis",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyThesisStatusAppliedAtTheCompanyByCompany",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyThesisStatusAppliedAtTheCompanyByStudent",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RNGForCompanyThesisApplied_HashedAsUniqueID",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "StudentCVAppliedForCompanyThesis",
                table: "CompanyThesesApplied",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentEmailAppliedForCompanyThesis",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "StudentImageAppliedForCompanyThesis",
                table: "CompanyThesesApplied",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentNameAppliedForCompanyThesis",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StudentRegNumberAppliedForCompanyThesis",
                table: "CompanyThesesApplied",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "StudentStudyYearAppliedForCompanyThesis",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentSurnameAppliedForCompanyThesis",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentTelephoneAppliedForCompanyThesis",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThesisTitleAppliedAtTheCompany",
                table: "CompanyThesesApplied",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
