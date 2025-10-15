using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class InterestInProfessorEventAsStudent_NewModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfessorEmailShowInterestAsStudent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "ProfessorEventStatusShowInterestAsStudentAtTheStudent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "ProfessorEventStatusShowInterestAtTheProfessorByProfessor",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "ProfessorEventTitleShowInterestAsStudent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "ProfessorNameShowInterestAsStudent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "ProfessorSurnameShowInterestAsStudent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "ProfessorUniversityDepartmentShowInterestAsStudent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "ProfessorUniversityShowInterestAsStudent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "RNGForProfessorEventShowInterestAsStudent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "RNGForProfessorEventShowInterestAsStudent_HashedAsUniqueID",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentAreasOfExpertiseInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentDepartmenInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentEmailShowInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentImageInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentKeywordsInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentLevelOfDegreeInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentLinkedInProfileInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentNameShowInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentPermanentAddressInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentPermanentPCInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentPermanentRegionInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentPermanentTownInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentPersonalWebsiteInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentPhoneVisibilityInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentStudyYearShowInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentSurnameShowInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentTelephoneShowInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentUniversityDepartmentInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentUniversityInterestForProfessorEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.RenameColumn(
                name: "StudentRegNumberShowInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                newName: "RNGForProfessorEventInterest");

            migrationBuilder.RenameColumn(
                name: "DateTimeStudentShowInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                newName: "DateTimeStudentShowInterest");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEmailWhereStudentShowedInterest",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEventStatusAtProfessorSide",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEventStatusAtStudentSide",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorUniqueIDWhereStudentShowedInterest",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RNGForProfessorEventInterest_HashedAsUniqueID",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentEmailShowInterestForEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentUniqueIDShowInterestForEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "InterestInProfessorEvent_ProfessorDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ProfessorUniqueIDWhereStudentShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfessorEmailWhereStudentShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestInProfessorEvent_ProfessorDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestInProfessorEvent_ProfessorDetails_InterestInProfessorEvents_Id",
                        column: x => x.Id,
                        principalTable: "InterestInProfessorEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterestInProfessorEvent_StudentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    StudentUniqueIDShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentEmailShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateTimeStudentShowInterestForProfessorEvent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RNGForProfessorEventShowInterestAsStudent_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestInProfessorEvent_StudentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestInProfessorEvent_StudentDetails_InterestInProfessorEvents_Id",
                        column: x => x.Id,
                        principalTable: "InterestInProfessorEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterestInProfessorEvents_StudentEmailShowInterestForEvent_RNGForProfessorEventInterest",
                table: "InterestInProfessorEvents",
                columns: new[] { "StudentEmailShowInterestForEvent", "RNGForProfessorEventInterest" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterestInProfessorEvent_ProfessorDetails_ProfessorEmailWhereStudentShowInterestForProfessorEvent",
                table: "InterestInProfessorEvent_ProfessorDetails",
                column: "ProfessorEmailWhereStudentShowInterestForProfessorEvent");

            migrationBuilder.CreateIndex(
                name: "IX_InterestInProfessorEvent_StudentDetails_StudentEmailShowInterestForProfessorEvent",
                table: "InterestInProfessorEvent_StudentDetails",
                column: "StudentEmailShowInterestForProfessorEvent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterestInProfessorEvent_ProfessorDetails");

            migrationBuilder.DropTable(
                name: "InterestInProfessorEvent_StudentDetails");

            migrationBuilder.DropIndex(
                name: "IX_InterestInProfessorEvents_StudentEmailShowInterestForEvent_RNGForProfessorEventInterest",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "ProfessorEmailWhereStudentShowedInterest",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "ProfessorEventStatusAtProfessorSide",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "ProfessorEventStatusAtStudentSide",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "ProfessorUniqueIDWhereStudentShowedInterest",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "RNGForProfessorEventInterest_HashedAsUniqueID",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentEmailShowInterestForEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.DropColumn(
                name: "StudentUniqueIDShowInterestForEvent",
                table: "InterestInProfessorEvents");

            migrationBuilder.RenameColumn(
                name: "RNGForProfessorEventInterest",
                table: "InterestInProfessorEvents",
                newName: "StudentRegNumberShowInterestForProfessorEvent");

            migrationBuilder.RenameColumn(
                name: "DateTimeStudentShowInterest",
                table: "InterestInProfessorEvents",
                newName: "DateTimeStudentShowInterestForProfessorEvent");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEmailShowInterestAsStudent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEventStatusShowInterestAsStudentAtTheStudent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEventStatusShowInterestAtTheProfessorByProfessor",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEventTitleShowInterestAsStudent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorNameShowInterestAsStudent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorSurnameShowInterestAsStudent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorUniversityDepartmentShowInterestAsStudent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorUniversityShowInterestAsStudent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RNGForProfessorEventShowInterestAsStudent",
                table: "InterestInProfessorEvents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "RNGForProfessorEventShowInterestAsStudent_HashedAsUniqueID",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentAreasOfExpertiseInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentDepartmenInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentEmailShowInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "StudentImageInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentKeywordsInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentLevelOfDegreeInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentLinkedInProfileInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentNameShowInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentPermanentAddressInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StudentPermanentPCInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "StudentPermanentRegionInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentPermanentTownInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentPersonalWebsiteInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "StudentPhoneVisibilityInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StudentStudyYearShowInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentSurnameShowInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentTelephoneShowInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentUniversityDepartmentInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentUniversityInterestForProfessorEvent",
                table: "InterestInProfessorEvents",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
