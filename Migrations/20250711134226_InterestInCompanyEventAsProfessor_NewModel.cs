using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class InterestInCompanyEventAsProfessor_NewModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InterestInCompanyEventsAsProfessor",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "CompanyDepartmentShowInterestAsProfessor",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "CompanyEmailShowInterestAsProfessor",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "CompanyEventStatusShowInterestAsProfessorAtTheProfessor_InterestInCompanyEventAsProfessor",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "CompanyEventStatusShowInterestAtTheCompanyByCompany_InterestInCompanyEventAsProfessor",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "CompanyEventTitleShowInterestAsProfessor",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "CompanyNameShowInterestAsProfessor",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "ProfessorGeneralFieldOfWorkShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "ProfessorImageInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "ProfessorLinkedInProfileShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "ProfessorNameShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "ProfessorOrchidProfileShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "ProfessorPersonalDescriptionShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "ProfessorPersonalTelephoneShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "ProfessorPersonalWebsiteShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "ProfessorPhoneVisibilityInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "ProfessorScholarProfileShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "ProfessorSurnameShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "ProfessorUniversityDepartmentShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "ProfessorUniversityShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "ProfessorVathmidaDEPInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropColumn(
                name: "ProfessorWorkTelephoneShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.RenameTable(
                name: "InterestInCompanyEventsAsProfessor",
                newName: "InterestInCompanyEventAsProfessor");

            migrationBuilder.RenameColumn(
                name: "RNGForCompanyEventShowInterestAsProfessor",
                table: "InterestInCompanyEventAsProfessor",
                newName: "RNGForCompanyEventInterestAsProfessor");

            migrationBuilder.AlterColumn<string>(
                name: "ProfessorEmailShowInterestForCompanyEvent",
                table: "InterestInCompanyEventAsProfessor",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEmailWhereProfessorShowedInterest",
                table: "InterestInCompanyEventAsProfessor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyEventStatus_ShowInterestAsProfessor_AtCompanySide",
                table: "InterestInCompanyEventAsProfessor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyEventStatus_ShowInterestAsProfessor_AtProfessorSide",
                table: "InterestInCompanyEventAsProfessor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyUniqueIDWhereProfessorShowedInterest",
                table: "InterestInCompanyEventAsProfessor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorUniqueIDShowInterestForCompanyEvent",
                table: "InterestInCompanyEventAsProfessor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RNGForCompanyEventInterestAsProfessor_HashedAsUniqueID",
                table: "InterestInCompanyEventAsProfessor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InterestInCompanyEventAsProfessor",
                table: "InterestInCompanyEventAsProfessor",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "InterestInCompanyEventAsProfessor_CompanyDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CompanyUniqueIDWhereProfessorShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyEmailWhereProfessorShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestInCompanyEventAsProfessor_CompanyDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestInCompanyEventAsProfessor_CompanyDetails_InterestInCompanyEventAsProfessor_Id",
                        column: x => x.Id,
                        principalTable: "InterestInCompanyEventAsProfessor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterestInCompanyEventAsProfessor_ProfessorDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ProfessorUniqueIDShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfessorEmailShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateTimeProfessorShowInterestForCompanyEvent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RNGForCompanyEventShowInterestAsProfessor_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestInCompanyEventAsProfessor_ProfessorDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestInCompanyEventAsProfessor_ProfessorDetails_InterestInCompanyEventAsProfessor_Id",
                        column: x => x.Id,
                        principalTable: "InterestInCompanyEventAsProfessor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterestInCompanyEventAsProfessor_ProfessorEmailShowInterestForCompanyEvent_RNGForCompanyEventInterestAsProfessor",
                table: "InterestInCompanyEventAsProfessor",
                columns: new[] { "ProfessorEmailShowInterestForCompanyEvent", "RNGForCompanyEventInterestAsProfessor" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterestInCompanyEventAsProfessor_CompanyDetails_CompanyEmailWhereProfessorShowInterestForCompanyEvent",
                table: "InterestInCompanyEventAsProfessor_CompanyDetails",
                column: "CompanyEmailWhereProfessorShowInterestForCompanyEvent");

            migrationBuilder.CreateIndex(
                name: "IX_InterestInCompanyEventAsProfessor_ProfessorDetails_ProfessorEmailShowInterestForCompanyEvent",
                table: "InterestInCompanyEventAsProfessor_ProfessorDetails",
                column: "ProfessorEmailShowInterestForCompanyEvent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterestInCompanyEventAsProfessor_CompanyDetails");

            migrationBuilder.DropTable(
                name: "InterestInCompanyEventAsProfessor_ProfessorDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InterestInCompanyEventAsProfessor",
                table: "InterestInCompanyEventAsProfessor");

            migrationBuilder.DropIndex(
                name: "IX_InterestInCompanyEventAsProfessor_ProfessorEmailShowInterestForCompanyEvent_RNGForCompanyEventInterestAsProfessor",
                table: "InterestInCompanyEventAsProfessor");

            migrationBuilder.DropColumn(
                name: "CompanyEmailWhereProfessorShowedInterest",
                table: "InterestInCompanyEventAsProfessor");

            migrationBuilder.DropColumn(
                name: "CompanyEventStatus_ShowInterestAsProfessor_AtCompanySide",
                table: "InterestInCompanyEventAsProfessor");

            migrationBuilder.DropColumn(
                name: "CompanyEventStatus_ShowInterestAsProfessor_AtProfessorSide",
                table: "InterestInCompanyEventAsProfessor");

            migrationBuilder.DropColumn(
                name: "CompanyUniqueIDWhereProfessorShowedInterest",
                table: "InterestInCompanyEventAsProfessor");

            migrationBuilder.DropColumn(
                name: "ProfessorUniqueIDShowInterestForCompanyEvent",
                table: "InterestInCompanyEventAsProfessor");

            migrationBuilder.DropColumn(
                name: "RNGForCompanyEventInterestAsProfessor_HashedAsUniqueID",
                table: "InterestInCompanyEventAsProfessor");

            migrationBuilder.RenameTable(
                name: "InterestInCompanyEventAsProfessor",
                newName: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.RenameColumn(
                name: "RNGForCompanyEventInterestAsProfessor",
                table: "InterestInCompanyEventsAsProfessor",
                newName: "RNGForCompanyEventShowInterestAsProfessor");

            migrationBuilder.AlterColumn<string>(
                name: "ProfessorEmailShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CompanyDepartmentShowInterestAsProfessor",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEmailShowInterestAsProfessor",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEventStatusShowInterestAsProfessorAtTheProfessor_InterestInCompanyEventAsProfessor",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEventStatusShowInterestAtTheCompanyByCompany_InterestInCompanyEventAsProfessor",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEventTitleShowInterestAsProfessor",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyNameShowInterestAsProfessor",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorGeneralFieldOfWorkShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfessorImageInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorLinkedInProfileShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorNameShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorOrchidProfileShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorPersonalDescriptionShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorPersonalTelephoneShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorPersonalWebsiteShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ProfessorPhoneVisibilityInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorScholarProfileShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorSurnameShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorUniversityDepartmentShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorUniversityShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorVathmidaDEPInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorWorkTelephoneShowInterestForCompanyEvent",
                table: "InterestInCompanyEventsAsProfessor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_InterestInCompanyEventsAsProfessor",
                table: "InterestInCompanyEventsAsProfessor",
                column: "Id");
        }
    }
}
