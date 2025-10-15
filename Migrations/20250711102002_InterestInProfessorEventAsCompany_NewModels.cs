using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class InterestInProfessorEventAsCompany_NewModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyActivityShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyAdminEmailInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyAdminNameInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyAdminSurnameInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyAdminTelephoneInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyAreasInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyCountryShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyDescriptionInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyHREmailInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyHRNameInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyHRSurnameInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyHRTelephoneInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyLocationShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyLogoInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyNameShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyPermanentPCInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyPhoneVisibilityInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyRegionsInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyShortNameShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyTelephoneShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyTownInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyTransportNeedWhenShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyTypeShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyWebsiteShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "ProfessorEmailShowInterestForHisEventAsAsCompany",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "ProfessorEventStatusShowInterestAsCompanyAtTheCompany_InterestInProfessorEventAsCompany",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "ProfessorEventStatusShowInterestAtTheProfessorByProfessor_InterestInProfessorEventAsCompany",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "ProfessorEventTitleShowInterestAsCompany",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "ProfessorNameShowInterestForHisEventAsCompany",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "ProfessorSurnameShowInterestForHisEventAsCompany",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.RenameColumn(
                name: "RNGForProfessorEventShowInterestAsCompany",
                table: "InterestInProfessorEventsAsCompany",
                newName: "RNGForProfessorEventInterestAsCompany");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyEmailShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyUniqueIDShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEmailWhereCompanyShowedInterest",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEventStatus_ShowInterestAsCompany_AtCompanySide",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEventStatus_ShowInterestAsCompany_AtProfessorSide",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfessorUniqueIDWhereCompanyShowedInterest",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RNGForProfessorEventInterestAsCompany_HashedAsUniqueID",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "InterestInProfessorEventAsCompany_CompanyDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CompanyUniqueIDShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyEmailShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateTimeCompanyShowInterestForProfessorEvent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RNGForProfessorEventShowInterestAsCompany_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestInProfessorEventAsCompany_CompanyDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestInProfessorEventAsCompany_CompanyDetails_InterestInProfessorEventsAsCompany_Id",
                        column: x => x.Id,
                        principalTable: "InterestInProfessorEventsAsCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InterestInProfessorEventAsCompany_ProfessorDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ProfessorUniqueIDWhereCompanyShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfessorEmailWhereCompanyShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestInProfessorEventAsCompany_ProfessorDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestInProfessorEventAsCompany_ProfessorDetails_InterestInProfessorEventsAsCompany_Id",
                        column: x => x.Id,
                        principalTable: "InterestInProfessorEventsAsCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterestInProfessorEventsAsCompany_CompanyEmailShowInterestForProfessorEvent_RNGForProfessorEventInterestAsCompany",
                table: "InterestInProfessorEventsAsCompany",
                columns: new[] { "CompanyEmailShowInterestForProfessorEvent", "RNGForProfessorEventInterestAsCompany" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InterestInProfessorEventAsCompany_CompanyDetails_CompanyEmailShowInterestForProfessorEvent",
                table: "InterestInProfessorEventAsCompany_CompanyDetails",
                column: "CompanyEmailShowInterestForProfessorEvent");

            migrationBuilder.CreateIndex(
                name: "IX_InterestInProfessorEventAsCompany_ProfessorDetails_ProfessorEmailWhereCompanyShowInterestForProfessorEvent",
                table: "InterestInProfessorEventAsCompany_ProfessorDetails",
                column: "ProfessorEmailWhereCompanyShowInterestForProfessorEvent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterestInProfessorEventAsCompany_CompanyDetails");

            migrationBuilder.DropTable(
                name: "InterestInProfessorEventAsCompany_ProfessorDetails");

            migrationBuilder.DropIndex(
                name: "IX_InterestInProfessorEventsAsCompany_CompanyEmailShowInterestForProfessorEvent_RNGForProfessorEventInterestAsCompany",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "CompanyUniqueIDShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "ProfessorEmailWhereCompanyShowedInterest",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "ProfessorEventStatus_ShowInterestAsCompany_AtCompanySide",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "ProfessorEventStatus_ShowInterestAsCompany_AtProfessorSide",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "ProfessorUniqueIDWhereCompanyShowedInterest",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropColumn(
                name: "RNGForProfessorEventInterestAsCompany_HashedAsUniqueID",
                table: "InterestInProfessorEventsAsCompany");

            migrationBuilder.RenameColumn(
                name: "RNGForProfessorEventInterestAsCompany",
                table: "InterestInProfessorEventsAsCompany",
                newName: "RNGForProfessorEventShowInterestAsCompany");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyEmailShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CompanyActivityShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyAdminEmailInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyAdminNameInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyAdminSurnameInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyAdminTelephoneInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyAreasInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCountryShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyDescriptionInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyHREmailInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyHRNameInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyHRSurnameInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyHRTelephoneInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyLocationShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "CompanyLogoInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyNameShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyPermanentPCInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CompanyPhoneVisibilityInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CompanyRegionsInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyShortNameShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyTelephoneShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyTownInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyTransportNeedWhenShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyTypeShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyWebsiteShowInterestForProfessorEvent",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEmailShowInterestForHisEventAsAsCompany",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEventStatusShowInterestAsCompanyAtTheCompany_InterestInProfessorEventAsCompany",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEventStatusShowInterestAtTheProfessorByProfessor_InterestInProfessorEventAsCompany",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorEventTitleShowInterestAsCompany",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorNameShowInterestForHisEventAsCompany",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorSurnameShowInterestForHisEventAsCompany",
                table: "InterestInProfessorEventsAsCompany",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
