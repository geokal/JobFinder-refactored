using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class CompanyThesisApplied_NewTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyAppliedForInternship",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "CompanyEmailAppliedForInternship",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "InternshipPositionInCompanyApplied",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "InternshipPositionTypeApplied",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "InternshipStatusAppliedAtTheCompanyByCompany",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "InternshipStatusAppliedAtTheCompanyByStudent",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "InternsnipTitleAppliedAtTheCompany",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "RNGForInternshipApplied_HashedAsUniqueID",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "StudentCVAppliedForInternship",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "StudentImageAppliedForInternship",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "StudentNameAppliedForInternship",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "StudentRegNumberAppliedForInternship",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "StudentStudyYearAppliedForInternship",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "StudentSurnameAppliedForInternship",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "StudentTelephoneAppliedForInternship",
                table: "InternshipsApplied");

            migrationBuilder.AlterColumn<string>(
                name: "StudentEmailAppliedForInternship",
                table: "InternshipsApplied",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEmailWhereStudentAppliedForInternship",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyUniqueIDWhereStudentAppliedForInternship",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InternshipStatusAppliedAtTheCompanySide",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InternshipStatusAppliedAtTheStudentSide",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RNGForInternshipAppliedAsStudent_HashedAsUniqueID",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentUniqueIDAppliedForInternship",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "InternshipApplied_CompanyDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CompanyUniqueIDWhereStudentAppliedForInternship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyEmailWhereStudentAppliedForInternship = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyNameWhereStudentAppliedForInternship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternshipPositionInCompanyApplied = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternshipPositionTypeApplied = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipApplied_CompanyDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternshipApplied_CompanyDetails_InternshipsApplied_Id",
                        column: x => x.Id,
                        principalTable: "InternshipsApplied",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InternshipApplied_StudentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    StudentUniqueIDAppliedForInternship = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentEmailAppliedForInternship = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateTimeStudentAppliedForInternship = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RNGForInternshipAppliedAsStudent_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipApplied_StudentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternshipApplied_StudentDetails_InternshipsApplied_Id",
                        column: x => x.Id,
                        principalTable: "InternshipsApplied",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InternshipsApplied_StudentEmailAppliedForInternship_RNGForInternshipApplied",
                table: "InternshipsApplied",
                columns: new[] { "StudentEmailAppliedForInternship", "RNGForInternshipApplied" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternshipApplied_CompanyDetails_CompanyEmailWhereStudentAppliedForInternship",
                table: "InternshipApplied_CompanyDetails",
                column: "CompanyEmailWhereStudentAppliedForInternship");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipApplied_StudentDetails_StudentEmailAppliedForInternship",
                table: "InternshipApplied_StudentDetails",
                column: "StudentEmailAppliedForInternship");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternshipApplied_CompanyDetails");

            migrationBuilder.DropTable(
                name: "InternshipApplied_StudentDetails");

            migrationBuilder.DropIndex(
                name: "IX_InternshipsApplied_StudentEmailAppliedForInternship_RNGForInternshipApplied",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "CompanyEmailWhereStudentAppliedForInternship",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "CompanyUniqueIDWhereStudentAppliedForInternship",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "InternshipStatusAppliedAtTheCompanySide",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "InternshipStatusAppliedAtTheStudentSide",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "RNGForInternshipAppliedAsStudent_HashedAsUniqueID",
                table: "InternshipsApplied");

            migrationBuilder.DropColumn(
                name: "StudentUniqueIDAppliedForInternship",
                table: "InternshipsApplied");

            migrationBuilder.AlterColumn<string>(
                name: "StudentEmailAppliedForInternship",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CompanyAppliedForInternship",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEmailAppliedForInternship",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternshipPositionInCompanyApplied",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternshipPositionTypeApplied",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternshipStatusAppliedAtTheCompanyByCompany",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternshipStatusAppliedAtTheCompanyByStudent",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternsnipTitleAppliedAtTheCompany",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RNGForInternshipApplied_HashedAsUniqueID",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "StudentCVAppliedForInternship",
                table: "InternshipsApplied",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "StudentImageAppliedForInternship",
                table: "InternshipsApplied",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentNameAppliedForInternship",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StudentRegNumberAppliedForInternship",
                table: "InternshipsApplied",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "StudentStudyYearAppliedForInternship",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentSurnameAppliedForInternship",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentTelephoneAppliedForInternship",
                table: "InternshipsApplied",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
