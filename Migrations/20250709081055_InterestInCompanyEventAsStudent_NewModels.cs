using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class InterestInCompanyEventAsStudent_NewModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyDepartmentShowInterestAsStudent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "CompanyEmailShowInterestAsStudent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "CompanyEventStatusShowInterestAsStudentAtTheStudent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "CompanyEventStatusShowInterestAtTheCompanyByCompany",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "CompanyEventTitleShowInterestAsStudent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "CompanyNameShowInterestAsStudent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "RNGForCompanyEventShowInterestAsStudent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "RNGForCompanyEventShowInterestAsStudent_HashedAsUniqueID",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentAreasOfExpertiseInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentDepartmenInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentEmailShowInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentImageInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentKeywordsInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentLevelOfDegreeInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentLinkedInProfileInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentNameShowInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentPermanentAddressInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentPermanentPCInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentPermanentRegionInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentPermanentTownInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentPersonalWebsiteInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentPhoneVisibilityInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentStudyYearShowInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentSurnameShowInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentTelephoneShowInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentUniversityDepartmentInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentUniversityInterestForCompanyEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.RenameColumn(
                name: "StudentRegNumberShowInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                newName: "RNGForCompanyEventInterest");

            migrationBuilder.RenameColumn(
                name: "DateTimeStudentShowInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                newName: "DateTimeStudentShowInterest");

            migrationBuilder.AddColumn<string>(
                name: "CompanyEmailWhereStudentShowedInterest",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyEventStatusAtCompanySide",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyEventStatusAtStudentSide",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyUniqueIDWhereStudentShowedInterest",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RNGForCompanyEventInterest_HashedAsUniqueID",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentEmailShowInterestForEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentUniqueIDShowInterestForEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "InterestInCompanyEvent_CompanyDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CompanyUniqueIDWhereStudentShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyEmailWhereStudentShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestInCompanyEvent_CompanyDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestInCompanyEvent_CompanyDetails_InterestInCompanyEvents_Id",
                        column: x => x.Id,
                        principalTable: "InterestInCompanyEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterestInCompanyEvent_StudentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    StudentUniqueIDShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentEmailShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateTimeStudentShowInterestForCompanyEvent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RNGForCompanyEventShowInterestAsStudent_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestInCompanyEvent_StudentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestInCompanyEvent_StudentDetails_InterestInCompanyEvents_Id",
                        column: x => x.Id,
                        principalTable: "InterestInCompanyEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterestInCompanyEvents_StudentEmailShowInterestForEvent_RNGForCompanyEventInterest",
                table: "InterestInCompanyEvents",
                columns: new[] { "StudentEmailShowInterestForEvent", "RNGForCompanyEventInterest" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterestInCompanyEvent_CompanyDetails_CompanyEmailWhereStudentShowInterestForCompanyEvent",
                table: "InterestInCompanyEvent_CompanyDetails",
                column: "CompanyEmailWhereStudentShowInterestForCompanyEvent");

            migrationBuilder.CreateIndex(
                name: "IX_InterestInCompanyEvent_StudentDetails_StudentEmailShowInterestForCompanyEvent",
                table: "InterestInCompanyEvent_StudentDetails",
                column: "StudentEmailShowInterestForCompanyEvent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterestInCompanyEvent_CompanyDetails");

            migrationBuilder.DropTable(
                name: "InterestInCompanyEvent_StudentDetails");

            migrationBuilder.DropIndex(
                name: "IX_InterestInCompanyEvents_StudentEmailShowInterestForEvent_RNGForCompanyEventInterest",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "CompanyEmailWhereStudentShowedInterest",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "CompanyEventStatusAtCompanySide",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "CompanyEventStatusAtStudentSide",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "CompanyUniqueIDWhereStudentShowedInterest",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "RNGForCompanyEventInterest_HashedAsUniqueID",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentEmailShowInterestForEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.DropColumn(
                name: "StudentUniqueIDShowInterestForEvent",
                table: "InterestInCompanyEvents");

            migrationBuilder.RenameColumn(
                name: "RNGForCompanyEventInterest",
                table: "InterestInCompanyEvents",
                newName: "StudentRegNumberShowInterestForCompanyEvent");

            migrationBuilder.RenameColumn(
                name: "DateTimeStudentShowInterest",
                table: "InterestInCompanyEvents",
                newName: "DateTimeStudentShowInterestForCompanyEvent");

            migrationBuilder.AddColumn<string>(
                name: "CompanyDepartmentShowInterestAsStudent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEmailShowInterestAsStudent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEventStatusShowInterestAsStudentAtTheStudent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEventStatusShowInterestAtTheCompanyByCompany",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEventTitleShowInterestAsStudent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyNameShowInterestAsStudent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RNGForCompanyEventShowInterestAsStudent",
                table: "InterestInCompanyEvents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "RNGForCompanyEventShowInterestAsStudent_HashedAsUniqueID",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentAreasOfExpertiseInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentDepartmenInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentEmailShowInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "StudentImageInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentKeywordsInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentLevelOfDegreeInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentLinkedInProfileInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentNameShowInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentPermanentAddressInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StudentPermanentPCInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "StudentPermanentRegionInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentPermanentTownInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentPersonalWebsiteInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "StudentPhoneVisibilityInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StudentStudyYearShowInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentSurnameShowInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentTelephoneShowInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentUniversityDepartmentInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentUniversityInterestForCompanyEvent",
                table: "InterestInCompanyEvents",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
