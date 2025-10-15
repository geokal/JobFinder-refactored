using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizManager.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnnouncementsAsCompany",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyAnnouncementRNG = table.Column<long>(type: "bigint", nullable: true),
                    CompanyAnnouncementRNG_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAnnouncementTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAnnouncementDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAnnouncementStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAnnouncementUploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyAnnouncementCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAnnouncementCompanyEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAnnouncementTimeToBeActive = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyAnnouncementAttachmentFile = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementsAsCompany", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementsAsProfessor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfessorAnnouncementRNG = table.Column<long>(type: "bigint", nullable: true),
                    ProfessorAnnouncementTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorAnnouncementDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorAnnouncementStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorAnnouncementUploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfessorAnnouncementProfessorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorAnnouncementProfessorSurname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorAnnouncementProfessorEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorAnnouncementTimeToBeActive = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfessorAnnouncementAttachmentFile = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ProfessorAnnouncementRNG_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementsAsProfessor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AreaName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaSubFields = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyLogo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyNameENG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyActivity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyTaxID = table.Column<long>(type: "bigint", nullable: true),
                    CompanyTaxOffice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyTelephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyWebsite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyPresentationEmbeddedVideo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyWebsiteAnnouncements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyWebsiteJobs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AtlasID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SvseID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SvseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyPC = table.Column<long>(type: "bigint", nullable: true),
                    CompanyRegions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyTown = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAreas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyDesiredSkills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyCEOName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyCEOSurname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyCEOTaxID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyHRName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyHRSurname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyHREmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyHRTelephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAdminName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAdminSurname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAdminEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAdminTelephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEmployees = table.Column<int>(type: "int", nullable: true),
                    CompanEmployeesLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyTurnover = table.Column<double>(type: "float", nullable: true),
                    CompanyTurnoverLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyExportCountriesNumber = table.Column<int>(type: "int", nullable: true),
                    CompanyExportCountriesNumberLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyExportCountries = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyExportCountriesLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyVisibleActivity = table.Column<bool>(type: "bit", nullable: false),
                    CompanyAcceptRules = table.Column<bool>(type: "bit", nullable: false),
                    CompanyDepartment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AskForExperience = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RNGForEventUploadedAsCompany = table.Column<long>(type: "bigint", nullable: false),
                    CompanyEventType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventOtherOrganizerToBeVisible = table.Column<bool>(type: "bit", nullable: false),
                    CompanyEventOtherOrganizer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventAreasOfInterest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventResponsiblePerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventResponsiblePersonEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventResponsiblePersonTelephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventCompanyDepartment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventCompanyLogo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CompanyEventCompanyEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventUploadedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyEventActiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyEventPerifereiaLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventDimosLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventPlaceLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CompanyEventPostalCodeLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventAttachmentFile = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CompanyEventOfferingTransportToEventLocation = table.Column<bool>(type: "bit", nullable: true),
                    CompanyEventStartingPointLocationToTransportPeopleToEvent1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventStartingPointLocationToTransportPeopleToEvent2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventStartingPointLocationToTransportPeopleToEvent3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RNGForEventUploadedAsCompany_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyInternships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RNGForInternshipAppliedInCompanyInternship = table.Column<long>(type: "bigint", nullable: false),
                    CompanyInternshipESPA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInternshipType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInternshipTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInternshipForeas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInternshipContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInternshipContactTelephonePerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInternshipAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInternshipPerifereiaLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInternshipDimosLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInternshipPostalCodeLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInternshipTransportOffer = table.Column<bool>(type: "bit", nullable: false),
                    CompanyInternshipAreas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInternshipActivePeriod = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyInternshipFinishEstimation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyInternshipLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyInternshipDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInternshipAttachment = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CompanyUploadedInternshipStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInternshipEKPASupervisor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailUsedToUploadInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyNameUsedToUploadInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInternshipUploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RNGForInternshipAppliedInCompanyInternship_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyInternships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UploadDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmailUsedToUploadJobs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionForeas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionContactTelephonePerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionAddressLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionPerifereiaLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionDimosLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionPostalCodeLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionTransportOffer = table.Column<bool>(type: "bit", nullable: false),
                    PositionAreas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionActivePeriod = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PositionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionDepartment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    RNGForPositionUploaded = table.Column<long>(type: "bigint", nullable: false),
                    PositionAttachment = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    TimesUpdated = table.Column<int>(type: "int", nullable: false),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyNameUploadJob = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RNGForPositionUploaded_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyJobsApplied",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RNGForPositionApplied = table.Column<long>(type: "bigint", nullable: false),
                    DateTimeStudentAppliedForPosition = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PositionTitleAppliedAtTheCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentNameApplied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentSurnameApplied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentRegNumberApplied = table.Column<long>(type: "bigint", nullable: false),
                    StudentStudyYearAppliedForJob = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentCVApplied = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    StudentImageApplied = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    StudentTelephoneAppliedForJob = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentEmailApplied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAppliedForPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEmailAppliedForPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PositionInCompanyApplied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyPositionTypeApplied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyPositionStatusApplied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyPositionStatusAppliedAtTheCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RNGForPositionApplied_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyJobsApplied", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyTheses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RNGForThesisUploadedAsCompany = table.Column<long>(type: "bigint", nullable: false),
                    CompanyNameUploadedThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEmailUsedToUploadThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyThesisTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyThesisCompanySupervisorFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyThesisDescriptionsUploaded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyThesisAreasUpload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyThesisSkillsNeeded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyThesisStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyThesisDepartment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyThesisContactPersonEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyThesisContactPersonTelephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyThesisUploadDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyThesisUpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyThesisStartingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyThesisTimesUpdated = table.Column<int>(type: "int", nullable: false),
                    CompanyThesisAttachmentUpload = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ThesisType = table.Column<int>(type: "int", nullable: false),
                    IsProfessorInteresetedInCompanyThesis = table.Column<bool>(type: "bit", nullable: false),
                    IsProfessorInterestedInCompanyThesisStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorNameInterestedInCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorSurnNameInterestedInCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEmailInterestedInCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfImageInterestedInCompanyThesis = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ProfUniversityInterestedInCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfDepartmentInterestedInCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfVahmidaDEPInterestedInCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfWorkTelephoneInterestedInCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfPersonalTelephoneInterestedInCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfPersonalTelephoneVisibilityInterestedInCompanyThesis = table.Column<bool>(type: "bit", nullable: false),
                    ProfPersonalWebsiteInterestedInCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfLinkedInSiteInterestedInCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfScholarProfileInterestedInCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfOrchidProfileInterestedInCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfGeneralFieldOfWorkInterestedInCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfGeneralSkillsInterestedInCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfPersonalDescriptionInterestedInCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RNGForThesisUploadedAsCompany_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTheses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyThesesApplied",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RNGForCompanyThesisApplied = table.Column<long>(type: "bigint", nullable: false),
                    DateTimeStudentAppliedForCompanyThesis = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThesisTitleAppliedAtTheCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentNameAppliedForCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentStudyYearAppliedForCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentTelephoneAppliedForCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentSurnameAppliedForCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentRegNumberAppliedForCompanyThesis = table.Column<long>(type: "bigint", nullable: false),
                    StudentCVAppliedForCompanyThesis = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    StudentImageAppliedForCompanyThesis = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    StudentEmailAppliedForCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyNameAppliedForCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyDepartmentAppliedForCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEmailAppliedForCompanyThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyThesisStatusAppliedAtTheCompanyByStudent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyThesisStatusAppliedAtTheCompanyByCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RNGForCompanyThesisApplied_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyThesesApplied", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterestInCompanyEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RNGForCompanyEventShowInterestAsStudent = table.Column<long>(type: "bigint", nullable: false),
                    CompanyEventTitleShowInterestAsStudent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimeStudentShowInterestForCompanyEvent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentNameShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentSurnameShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentStudyYearShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentTelephoneShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentRegNumberShowInterestForCompanyEvent = table.Column<long>(type: "bigint", nullable: false),
                    StudentEmailShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentTransportNeedWhenShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentTransportChosenLocationWhenShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentPhoneVisibilityInterestForCompanyEvent = table.Column<bool>(type: "bit", nullable: false),
                    StudentImageInterestForCompanyEvent = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    StudentUniversityDepartmentInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentPermanentAddressInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentPermanentPCInterestForCompanyEvent = table.Column<long>(type: "bigint", nullable: false),
                    StudentPermanentRegionInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentPermanentTownInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentLinkedInProfileInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentPersonalWebsiteInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentUniversityInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentDepartmenInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentAreasOfExpertiseInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentKeywordsInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentLevelOfDegreeInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyNameShowInterestAsStudent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyDepartmentShowInterestAsStudent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEmailShowInterestAsStudent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventStatusShowInterestAsStudentAtTheStudent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventStatusShowInterestAtTheCompanyByCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RNGForCompanyEventShowInterestAsStudent_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestInCompanyEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterestInCompanyEventsAsProfessor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RNGForCompanyEventShowInterestAsProfessor = table.Column<long>(type: "bigint", nullable: false),
                    CompanyEventTitleShowInterestAsProfessor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimeProfessorShowInterestForCompanyEvent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfessorImageInterestForCompanyEvent = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ProfessorVathmidaDEPInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorNameShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorSurnameShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorPhoneVisibilityInterestForCompanyEvent = table.Column<bool>(type: "bit", nullable: false),
                    ProfessorPersonalTelephoneShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorWorkTelephoneShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEmailShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorUniversityShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorUniversityDepartmentShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorLinkedInProfileShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorPersonalWebsiteShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorScholarProfileShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorOrchidProfileShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorGeneralFieldOfWorkShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorPersonalDescriptionShowInterestForCompanyEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyNameShowInterestAsProfessor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyDepartmentShowInterestAsProfessor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEmailShowInterestAsProfessor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventStatusShowInterestAsProfessorAtTheProfessor_InterestInCompanyEventAsProfessor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEventStatusShowInterestAtTheCompanyByCompany_InterestInCompanyEventAsProfessor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestInCompanyEventsAsProfessor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterestInProfessorEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RNGForProfessorEventShowInterestAsStudent = table.Column<long>(type: "bigint", nullable: false),
                    ProfessorEventTitleShowInterestAsStudent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimeStudentShowInterestForProfessorEvent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentNameShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentSurnameShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentStudyYearShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentTelephoneShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentRegNumberShowInterestForProfessorEvent = table.Column<long>(type: "bigint", nullable: false),
                    StudentEmailShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentTransportNeedWhenShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentTransportChosenLocationWhenShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentPhoneVisibilityInterestForProfessorEvent = table.Column<bool>(type: "bit", nullable: false),
                    StudentImageInterestForProfessorEvent = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    StudentUniversityDepartmentInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentPermanentAddressInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentPermanentPCInterestForProfessorEvent = table.Column<long>(type: "bigint", nullable: false),
                    StudentPermanentRegionInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentPermanentTownInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentLinkedInProfileInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentPersonalWebsiteInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentUniversityInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentDepartmenInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentAreasOfExpertiseInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentKeywordsInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentLevelOfDegreeInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorNameShowInterestAsStudent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorSurnameShowInterestAsStudent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorUniversityShowInterestAsStudent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorUniversityDepartmentShowInterestAsStudent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEmailShowInterestAsStudent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventStatusShowInterestAsStudentAtTheStudent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventStatusShowInterestAtTheProfessorByProfessor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RNGForProfessorEventShowInterestAsStudent_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestInProfessorEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterestInProfessorEventsAsCompany",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RNGForProfessorEventShowInterestAsCompany = table.Column<long>(type: "bigint", nullable: false),
                    ProfessorNameShowInterestForHisEventAsCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorSurnameShowInterestForHisEventAsCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventTitleShowInterestAsCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEmailShowInterestForHisEventAsAsCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTimeCompanyShowInterestForProfessorEvent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyLogoInterestForProfessorEvent = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CompanyTypeShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyNameShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyShortNameShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyDescriptionInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEmailShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyPhoneVisibilityInterestForProfessorEvent = table.Column<bool>(type: "bit", nullable: false),
                    CompanyTelephoneShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyWebsiteShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyActivityShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyCountryShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyLocationShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyPermanentPCInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyRegionsInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyTownInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAreasInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyHRNameInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyHRSurnameInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyHREmailInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyHRTelephoneInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAdminNameInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAdminSurnameInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAdminEmailInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAdminTelephoneInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyTransportNeedWhenShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyNumberOfPeopleToShowUpWhenShowInterestForProfessorEvent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventStatusShowInterestAsCompanyAtTheCompany_InterestInProfessorEventAsCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventStatusShowInterestAtTheProfessorByProfessor_InterestInProfessorEventAsCompany = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestInProfessorEventsAsCompany", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InternshipsApplied",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RNGForInternshipApplied = table.Column<long>(type: "bigint", nullable: false),
                    DateTimeStudentAppliedForInternship = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InternsnipTitleAppliedAtTheCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentNameAppliedForInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentStudyYearAppliedForInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentTelephoneAppliedForInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentSurnameAppliedForInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentRegNumberAppliedForInternship = table.Column<long>(type: "bigint", nullable: false),
                    StudentCVAppliedForInternship = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    StudentImageAppliedForInternship = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    StudentEmailAppliedForInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAppliedForInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEmailAppliedForInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternshipPositionInCompanyApplied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternshipPositionTypeApplied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternshipStatusAppliedAtTheCompanyByStudent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternshipStatusAppliedAtTheCompanyByCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RNGForInternshipApplied_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipsApplied", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlatformActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserRole_PerformedAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForWhat_PerformedAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HashedPositionRNG_PerformedAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfAction_PerformedAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime_PerformedAction = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfessorEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RNGForEventUploadedAsProfessor = table.Column<long>(type: "bigint", nullable: false),
                    ProfessorEventType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventOtherOrganizerToBeVisible = table.Column<bool>(type: "bit", nullable: false),
                    ProfessorEventOtherOrganizer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventAreasOfInterest = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventProfessorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventProfessorSurName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventResponsiblePerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventResponsiblePersonEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventResponsiblePersonTelephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventUniversity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventUniversityDepartment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventProfessorImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ProfessorEventProfessorEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventUploadedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfessorEventActiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfessorEventPerifereiaLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventDimosLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventPlaceLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ProfessorEventPostalCodeLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventAttachmentFile = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ProfessorEventOfferingTransportToEventLocation = table.Column<bool>(type: "bit", nullable: true),
                    ProfessorEventStartingPointLocationToTransportPeopleToEvent1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventStartingPointLocationToTransportPeopleToEvent2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEventStartingPointLocationToTransportPeopleToEvent3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RNGForEventUploadedAsProfessor_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessorEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfessorInternships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RNGForInternshipAppliedInProfessorInternship = table.Column<long>(type: "bigint", nullable: false),
                    ProfessorInternshipESPA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorInternshipType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorInternshipTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorInternshipForeas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorInternshipContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorInternshipContactTelephonePerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorInternshipAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorInternshipPerifereiaLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorInternshipDimosLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorInternshipPostalCodeLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorInternshipTransportOffer = table.Column<bool>(type: "bit", nullable: false),
                    ProfessorInternshipAreas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorInternshipActivePeriod = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfessorInternshipFinishEstimation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfessorInternshipLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfessorInternshipDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorInternshipAttachment = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ProfessorUploadedInternshipStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorInternshipEKPASupervisor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailUsedToUploadProfessorInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfVahmidaDEPToUploadInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorNameUsedToUploadInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorSurnameUsedToUploadInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorUniversityToUploadInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorUniversityDepartmentToUploadInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorProfGeneralFieldOfWorkToUploadInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorInternshipUploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RNGForInternshipAppliedInProfessorInternship_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessorInternships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfessorInternshipsApplied",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RNGForProfessorInternshipApplied = table.Column<long>(type: "bigint", nullable: false),
                    DateTimeStudentAppliedForProfessorInternship = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InternsnipTitleAppliedAtTheProfessor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentNameAppliedForProfessorInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentStudyYearAppliedForProfessorInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentTelephoneAppliedForProfessorInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentSurnameAppliedForProfessorInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentRegNumberAppliedForProfessorInternship = table.Column<long>(type: "bigint", nullable: false),
                    StudentCVAppliedForProfessorInternship = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    StudentImageAppliedForProfessorInternship = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    StudentEmailAppliedForProfessorInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorNameAppliedForInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorSurnameAppliedForInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEmailAppliedForInternship = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorInternshipPositionTypeApplied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternshipStatusAppliedAtTheProfessorByStudent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternshipStatusAppliedAtTheProfessorByProfessor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RNGForProfessorInternshipApplied_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessorInternshipsApplied", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Professors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ProfName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfSurname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfUniversity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfDepartment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfVahmidaDEP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfWorkTelephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfPersonalTelephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfPersonalTelephoneVisibility = table.Column<bool>(type: "bit", nullable: false),
                    ProfPersonalWebsite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfLinkedInSite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfScholarProfile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfOrchidProfile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfGeneralFieldOfWork = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfGeneralSkills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfPersonalDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfCVAttachment = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ProfRegistryNumber = table.Column<long>(type: "bigint", nullable: true),
                    ProfCourses = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfessorTheses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RNGForThesisUploaded = table.Column<long>(type: "bigint", nullable: false),
                    ThesisTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThesisDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThesisAreas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThesisSkills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThesisAttachment = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    EmailUsedToUploadThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThesisUploadDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThesisActivePeriod = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThesisUpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThesisStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThesisTimesUpdated = table.Column<int>(type: "int", nullable: false),
                    ProfessorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorSurnname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorDepartment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThesisType = table.Column<int>(type: "int", nullable: false),
                    IsCompanyInteresetedInProfessorThesis = table.Column<bool>(type: "bit", nullable: false),
                    IsCompanyInterestedInProfessorThesisStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyLogoInterestedInProfessorThesis = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CompanyNameENGInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyEmailInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyTypeInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyActivityInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyTelephoneInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyWebsiteInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyWebsiteAnnouncementsInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyWebsiteJobsInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyCountryInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyLocationInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyPCInterestedInProfessorThesis = table.Column<long>(type: "bigint", nullable: true),
                    CompanyRegionsInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyTownInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyDescriptionInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAreasInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyDesiredSkillsInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyHRNameInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyHRSurnameInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyHREmailInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyHRTelephoneInterestedInProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RNGForThesisUploaded_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessorTheses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfessorThesesApplied",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RNGForProfessorThesisApplied = table.Column<long>(type: "bigint", nullable: false),
                    DateTimeStudentAppliedForProfessorThesis = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThesisTitleAppliedAtTheProfessor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentNameAppliedForProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentSurnameAppliedForProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentRegNumberAppliedForProfessorThesis = table.Column<long>(type: "bigint", nullable: false),
                    StudentStudyYearAppliedForProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentCVAppliedForProfessorThesis = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    StudentImageAppliedForProfessorThesis = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    StudentTelephoneAppliedForProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentEmailAppliedForProfessorThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorAppliedForThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorEmailAppliedForThesis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThesisInProfessorApplied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorThesisTypeApplied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorThesisStatusApplied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfessorThesisStatusAppliedAtTheProfessor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RNGForProfessorThesisApplied_HashedAsUniqueID = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessorThesesApplied", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResearchGroup_NonFacultyMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PK_ResearchGroupEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PK_NonFacultyMemberEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfRegistrationOnResearchGroup_ForNonFacultyMember = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PK_NonFacultyMemberLevelOfStudies = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchGroup_NonFacultyMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResearchGroup_Professors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PK_ResearchGroupEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PK_ProfessorEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfRegistrationOnResearchGroup_ForProfessorMember = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PK_ProfessorRole = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchGroup_Professors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResearchGroup_Publications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PK_ResearchGroupEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PK_ResearchGroupMemberEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PK_ResearchGroupMemberPublication_Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PK_ResearchGroupMemberPublication_Authors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PK_ResearchGroupMemberPublication_Journal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PK_ResearchGroupMemberPublication_CitedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PK_ResearchGroupMemberPublication_Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PK_ResearchGroupMemberPublication_Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchGroup_Publications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResearchGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResearchGroupEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroup_UniqueID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResearchGroupImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ResearchGroupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroupUniversity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroupSchool = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroupUniversityDepartment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroupLab = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroupFEK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroupContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroupPostalAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResearchGroupTelephoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneVisibility = table.Column<bool>(type: "bit", nullable: false),
                    PermanentAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PermanentPC = table.Column<long>(type: "bigint", nullable: false),
                    PermanentRegion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PermanentTown = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeVisibility = table.Column<bool>(type: "bit", nullable: false),
                    Attachment = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LinkedInProfile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonalWebsite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Transport = table.Column<bool>(type: "bit", nullable: false),
                    RegNumber = table.Column<long>(type: "bigint", nullable: false),
                    University = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnrollmentDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudyYear = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpectedGraduationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedECTS = table.Column<int>(type: "int", nullable: false),
                    Grades = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    InternshipStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThesisStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TechnicalSkills = table.Column<bool>(type: "bit", nullable: false),
                    Programming = table.Column<bool>(type: "bit", nullable: false),
                    MachineLearning = table.Column<bool>(type: "bit", nullable: false),
                    NetworksAndTelecom = table.Column<bool>(type: "bit", nullable: false),
                    Databases = table.Column<bool>(type: "bit", nullable: false),
                    AreasOfExpertise = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelfAssesmentAreas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetAreas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Keywords = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelfAssesmentSkills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetSkills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverLetter = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PreferredTownsBoolean = table.Column<bool>(type: "bit", nullable: false),
                    PreferedRegions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreferredTowns = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastProfileUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastCVUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LevelOfDegree = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasFinishedStudies = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThesisApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RNGForThesisUploaded = table.Column<long>(type: "bigint", nullable: false),
                    DateTimeApplied = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentRegNumber = table.Column<long>(type: "bigint", nullable: false),
                    StudentCV = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    StudentImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    SupervisorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupervisorSurname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupervisorEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThesisTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThesisApplications", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnouncementsAsCompany");

            migrationBuilder.DropTable(
                name: "AnnouncementsAsProfessor");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "CompanyEvents");

            migrationBuilder.DropTable(
                name: "CompanyInternships");

            migrationBuilder.DropTable(
                name: "CompanyJobs");

            migrationBuilder.DropTable(
                name: "CompanyJobsApplied");

            migrationBuilder.DropTable(
                name: "CompanyTheses");

            migrationBuilder.DropTable(
                name: "CompanyThesesApplied");

            migrationBuilder.DropTable(
                name: "InterestInCompanyEvents");

            migrationBuilder.DropTable(
                name: "InterestInCompanyEventsAsProfessor");

            migrationBuilder.DropTable(
                name: "InterestInProfessorEvents");

            migrationBuilder.DropTable(
                name: "InterestInProfessorEventsAsCompany");

            migrationBuilder.DropTable(
                name: "InternshipsApplied");

            migrationBuilder.DropTable(
                name: "PlatformActions");

            migrationBuilder.DropTable(
                name: "ProfessorEvents");

            migrationBuilder.DropTable(
                name: "ProfessorInternships");

            migrationBuilder.DropTable(
                name: "ProfessorInternshipsApplied");

            migrationBuilder.DropTable(
                name: "Professors");

            migrationBuilder.DropTable(
                name: "ProfessorTheses");

            migrationBuilder.DropTable(
                name: "ProfessorThesesApplied");

            migrationBuilder.DropTable(
                name: "ResearchGroup_NonFacultyMembers");

            migrationBuilder.DropTable(
                name: "ResearchGroup_Professors");

            migrationBuilder.DropTable(
                name: "ResearchGroup_Publications");

            migrationBuilder.DropTable(
                name: "ResearchGroups");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "ThesisApplications");
        }
    }
}
