using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using LinqKit;
using Newtonsoft.Json;
using QuizManager.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using QuizManager.Models;
using System.Net.Mail;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Text.Json;

namespace QuizManager.Shared
{
    public partial class MainLayout
    {
    [Inject] private IServiceProvider ServiceProvider { get; set; }
    [Inject] private FileUploadService FileUploadService { get; set; }
    [Inject] private Data.AppDbContext dbContext { get; set; }
    [Inject] private Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private InternshipEmailService InternshipEmailService { get; set; }
    [Inject] private IJSRuntime JS { get; set; }
    [Inject] private IAuth0Service Auth0Service { get; set; }
    [Inject] private GoogleScholarService GoogleScholarService { get; set; }

    // Layout parameter - Body is inherited from LayoutComponentBase

    // Fields for the refactored components
    private bool isStudentInitialized, isCompanyInitialized, isProfessorInitialized, isResearchGroupInitialized;
    private bool isStudentRegistered, isCompanyRegistered, isProfessorRegistered, isResearchGroupRegistered;

    // Properties for Research Group search
    private string searchResearchGroupNameAsCompanyToFindResearchGroup = "";
    private string searchResearchGroupSchoolAsCompanyToFindResearchGroup = "";
    private string searchResearchGroupUniversityDepartmentAsCompanyToFindResearchGroup = "";
    private string searchResearchGroupAreasAsCompanyToFindResearchGroup = "";
    private string searchResearchGroupSkillsAsCompanyToFindResearchGroup = "";
    private string searchResearchGroupKeywordsAsCompanyToFindResearchGroup = "";
    private bool hasSearchedForResearchGroups = false;


    private List<string> researchgroupNameSuggestions = new List<string>();
    private List<string> researchGroupAreasSuggestions = new List<string>();
    private List<string> researchGroupSkillsSuggestions = new List<string>();
    private List<string> researchGroupKeywordsSuggestions = new List<string>();

    private List<string> selectedResearchGroupAreas = new List<string>();
    private List<string> selectedResearchGroupSkills = new List<string>();
    private List<string> selectedResearchGroupKeywords = new List<string>();

    private Dictionary<string, List<string>> universityDepartments = new()
    {
        ["ΑΓΡΟΤΙΚΗΣ ΑΝΑΠΤΥΞΗΣ, ΔΙΑΤΡΟΦΗΣ ΚΑΙ ΑΕΙΦΟΡΙΑΣ"] = new List<string>
        {
            "ΤΜΗΜΑ ΑΓΡΟΤΙΚΗΣ ΑΝΑΠΤΥΞΗΣ, ΑΓΡΟΔΙΑΤΡΟΦΗΣ ΚΑΙ ΔΙΑΧΕΙΡΙΣΗΣ ΦΥΣΙΚΩΝ ΠΟΡΩΝ"
        },
        ["ΕΠΙΣΤΗΜΩΝ ΑΓΩΓΗΣ"] = new List<string>
        {
            "ΠΑΙΔΑΓΩΓΙΚΟ ΤΜΗΜΑ ΔΗΜΟΤΙΚΗΣ ΕΚΠΑΙΔΕΥΣΗΣ",
            "ΤΜΗΜΑ ΕΚΠΑΙΔΕΥΣΗΣ ΚΑΙ ΑΓΩΓΗΣ ΣΤΗΝ ΠΡΟΣΧΟΛΙΚΗ ΗΛΙΚΙΑ"
        },
        ["ΕΠΙΣΤΗΜΩΝ ΥΓΕΙΑΣ"] = new List<string>
        {
            "ΤΜΗΜΑ ΙΑΤΡΙΚΗΣ",
            "ΤΜΗΜΑ ΝΟΣΗΛΕΥΤΙΚΗΣ",
            "ΤΜΗΜΑ ΟΔΟΝΤΙΑΤΡΙΚΗΣ",
            "ΤΜΗΜΑ ΦΑΡΜΑΚΕΥΤΙΚΗΣ"
        },
        ["ΕΠΙΣΤΗΜΗΣ ΦΥΣΙΚΗΣ ΑΓΩΓΗΣ ΚΑΙ ΑΘΛΗΤΙΣΜΟΥ"] = new List<string>
        {
            "ΤΜΗΜΑ ΕΠΙΣΤΗΜΗΣ ΦΥΣΙΚΗΣ ΑΓΩΓΗΣ ΚΑΙ ΑΘΛΗΤΙΣΜΟΥ"
        },
        ["ΘΕΟΛΟΓΙΚΗ"] = new List<string>
        {
            "ΤΜΗΜΑ ΘΕΟΛΟΓΙΑΣ",
            "ΤΜΗΜΑ ΚΟΙΝΩΝΙΚΗΣ ΘΕΟΛΟΓΙΑΣ ΚΑΙ ΘΡΗΣΚΕΙΟΛΟΓΙΑΣ"
        },
        ["ΘΕΤΙΚΩΝ ΕΠΙΣΤΗΜΩΝ"] = new List<string>
        {
            "ΤΜΗΜΑ ΑΕΡΟΔΙΑΣΤΗΜΙΚΗΣ ΕΠΙΣΤΗΜΗΣ ΚΑΙ ΤΕΧΝΟΛΟΓΙΑΣ",
            "ΤΜΗΜΑ ΒΙΟΛΟΓΙΑΣ",
            "ΤΜΗΜΑ ΓΕΩΛΟΓΙΑΣ ΚΑΙ ΓΕΩΠΕΡΙΒΑΛΛΟΝΤΟΣ",
            "ΤΜΗΜΑ ΙΣΤΟΡΙΑΣ ΚΑΙ ΦΙΛΟΣΟΦΙΑΣ ΤΗΣ ΕΠΙΣΤΗΜΗΣ",
            "ΤΜΗΜΑ ΜΑΘΗΜΑΤΙΚΩΝ",
            "ΤΜΗΜΑ ΠΛΗΡΟΦΟΡΙΚΗΣ ΚΑΙ ΤΗΛΕΠΙΚΟΙΝΩΝΙΩΝ",
            "ΤΜΗΜΑ ΤΕΧΝΟΛΟΓΙΩΝ ΨΗΦΙΑΚΗΣ ΒΙΟΜΗΧΑΝΙΑΣ",
            "ΤΜΗΜΑ ΦΥΣΙΚΗΣ",
            "ΤΜΗΜΑ ΧΗΜΕΙΑΣ"
        },
        ["ΝΟΜΙΚΗ"] = new List<string>
        {
            "ΝΟΜΙΚΗ ΣΧΟΛΗ"
        },
        ["ΟΙΚΟΝΟΜΙΚΩΝ ΚΑΙ ΠΟΛΙΤΙΚΩΝ ΕΠΙΣΤΗΜΩΝ"] = new List<string>
        {
            "ΤΜΗΜΑ ΔΙΑΧΕΙΡΙΣΗΣ ΛΙΜΕΝΩΝ ΚΑΙ ΝΑΥΤΙΛΙΑΣ",
            "ΤΜΗΜΑ ΕΠΙΚΟΙΝΩΝΙΑΣ ΚΑΙ ΜΕΣΩΝ ΜΑΖΙΚΗΣ ΕΝΗΜΕΡΩΣΗΣ",
            "ΤΜΗΜΑ ΟΙΚΟΝΟΜΙΚΩΝ ΕΠΙΣΤΗΜΩΝ",
            "ΤΜΗΜΑ ΠΟΛΙΤΙΚΗΣ ΕΠΙΣΤΗΜΗΣ ΚΑΙ ΔΗΜΟΣΙΑΣ ΔΙΟΙΚΗΣΗΣ",
            "ΤΜΗΜΑ ΤΟΥΡΚΙΚΩΝ ΣΠΟΥΔΩΝ ΚΑΙ ΣΥΓΧΡΟΝΩΝ ΑΣΙΑΤΙΚΩΝ ΣΠΟΥΔΩΝ",
            "ΤΜΗΜΑ ΔΙΟΙΚΗΣΗΣ ΕΠΙΧΕΙΡΗΣΕΩΝ ΚΑΙ ΟΡΓΑΝΙΣΜΩΝ",
            "ΤΜΗΜΑ ΚΟΙΝΩΝΙΟΛΟΓΙΑΣ",
            "ΤΜΗΜΑ ΨΗΦΙΑΚΩΝ ΤΕΧΝΩΝ ΚΑΙ ΚΙΝΗΜΑΤΟΓΡΑΦΟΥ"
        },
        ["ΦΙΛΟΣΟΦΙΚΗ"] = new List<string>
        {
            "ΠΑΙΔΑΓΩΓΙΚΟ ΤΜΗΜΑ ΔΕΥΤΕΡΟΒΑΘΜΙΑΣ ΕΚΠΑΙΔΕΥΣΗΣ",
            "ΤΜΗΜΑ ΑΓΓΛΙΚΗΣ ΓΛΩΣΣΑΣ ΚΑΙ ΦΙΛΟΛΟΓΙΑΣ",
            "ΤΜΗΜΑ ΓΑΛΛΙΚΗΣ ΓΛΩΣΣΑΣ ΚΑΙ ΦΙΛΟΛΟΓΙΑΣ",
            "ΤΜΗΜΑ ΓΕΡΜΑΝΙΚΗΣ ΓΛΩΣΣΑΣ ΚΑΙ ΦΙΛΟΛΟΓΙΑΣ",
            "ΤΜΗΜΑ ΘΕΑΤΡΙΚΩΝ ΣΠΟΥΔΩΝ",
            "ΤΜΗΜΑ ΙΣΠΑΝΙΚΗΣ ΓΛΩΣΣΑΣ ΚΑΙ ΦΙΛΟΛΟΓΙΑΣ",
            "ΤΜΗΜΑ ΙΣΤΟΡΙΑΣ ΚΑΙ ΑΡΧΑΙΟΛΟΓΙΑΣ",
            "ΤΜΗΜΑ ΙΤΑΛΙΚΗΣ ΓΛΩΣΣΑΣ ΚΑΙ ΦΙΛΟΛΟΓΙΑΣ",
            "ΤΜΗΜΑ ΜΟΥΣΙΚΩΝ ΣΠΟΥΔΩΝ",
            "ΤΜΗΜΑ ΡΩΣΙΚΗΣ ΓΛΩΣΣΑΣ ΚΑΙ ΦΙΛΟΛΟΓΙΑΣ ΚΑΙ ΣΛΑΒΙΚΩΝ ΣΠΟΥΔΩΝ",
            "ΤΜΗΜΑ ΦΙΛΟΛΟΓΙΑΣ",
            "ΤΜΗΜΑ ΦΙΛΟΣΟΦΙΑΣ",
            "ΤΜΗΜΑ ΨΥΧΟΛΟΓΙΑΣ"
        }
    };

    // Property to get the list of schools for the dropdown
    private List<string> researchGroupSchools => universityDepartments.Keys.ToList();

    // Property to get the filtered departments based on selected school
    private List<string> filteredDepartments => 
        string.IsNullOrEmpty(searchResearchGroupSchoolAsCompanyToFindResearchGroup) 
            ? new List<string>() 
            : universityDepartments.ContainsKey(searchResearchGroupSchoolAsCompanyToFindResearchGroup)
                ? universityDepartments[searchResearchGroupSchoolAsCompanyToFindResearchGroup]
                : new List<string>();

    // Method to handle school selection change
    private async Task OnSchoolSelectionChanged(ChangeEventArgs e)
    {
        searchResearchGroupSchoolAsCompanyToFindResearchGroup = e.Value?.ToString() ?? "";
        // Clear department selection when school changes
        searchResearchGroupUniversityDepartmentAsCompanyToFindResearchGroup = "";
        await InvokeAsync(StateHasChanged);
    }
    // Search results and pagination
    private List<ResearchGroup> searchResultsAsCompanyToFindResearchGroup = new List<ResearchGroup>();
    private ResearchGroup selectedResearchGroupWhenSearchForResearchGroupsAsCompany;
    private bool showResearchGroupDetailsModalWhenSearchForResearchGroupsAsCompany = false;

    private int currentResearchGroupPage_SearchForResearchGroupsAsCompany = 1;
    private int ResearchGroupsPerPage_SearchForResearchGroupsAsCompany = 5;
    private List<int> pageSizeOptions_SearchForResearchGroupsAsCompany = new List<int> { 5, 10, 20 };

    private bool isStatisticsVisible = false;
    private int? numberOfFacultyMembers;
    private int? numberOfCollaborators;
    private int? numberOfTotalPublications;
    private int? numberOfRecentPublications;
    private int? numberOfActiveResearchActions;
    private int? numberOfInactiveResearchActions;
    private int? numberOfActivePatents;
    private int? numberOfInactivePatents;

    // Add this method to toggle visibility
    private async Task ToggleStatisticsVisibility()
    {
        isStatisticsVisible = !isStatisticsVisible;
        await LoadResearchGroupStatistics();
        StateHasChanged();
    }


    private List<StudentWithAuth0Details> StudentsWithAuth0Details { get; set; } = new();
    private string UserRole = "";
    bool isInitializedAsStudentUser = false;
    bool isInitializedAsCompanyUser = false;
    bool isInitializedAsProfessorUser = false;
    string CurrentUserEmail = "";
    private List<Student> Students = new List<Student>();
    bool isInitializedAsResearchGroupUser = false;

    private bool ShouldShowAdminTable()
    {
        return UserRole == "Admin" && !NavigationManager.Uri.Contains("/profile", StringComparison.OrdinalIgnoreCase);
    }

    private async Task LoadStudentsAsync()
    {
        // First get all students from your Students table
        Students = await dbContext.Students.ToListAsync();
    }


    bool showThesisApplications = false;
    bool showInternships = false;
    bool showJobApplications = false;
    private Student userData;
    private List<ProfessorThesisApplied> thesisApplications;
    private List<ProfessorThesisApplied> professorthesisApplications;
    private List<CompanyThesisApplied> companythesisApplications;
    private List<CompanyJobApplied> jobApplications;
    List<CompanyJob> jobs = new List<CompanyJob>();
    List<CompanyThesis> companytheses = new List<CompanyThesis>();
    List<ProfessorThesis> professortheses = new List<ProfessorThesis>();
    List<AnnouncementAsCompany> announcements = new List<AnnouncementAsCompany>();
    List<ProfessorThesis> theses = new List<ProfessorThesis>();
    List<CompanyInternship> companyinternships = new List<CompanyInternship>();
    private bool ShowStudentRegistrationButton = false;
    private bool ShowCompanyRegistrationButton = false;
    private bool ShowProfessorRegistrationButton = false;
    private bool ShowAdminRegistrationButton = false;
    private bool isStudentStatsFormVisibleToShowStudentStatsAsAdmin = false;
    private bool isAnalyticsVisible = false;

    string thesisSearch = "";
    string jobSearch = "";
    string companyinternshipSearch = "";

    string companyinternshipSearchByTitle = "";
    string companyinternshipSearchByType = "";
    string companyinternshipSearchByESPA = "";
    string companyinternshipSearchByRegion = "";
    string jobSearchByTown = "";
    string jobSearchByRegion = "";
    string companyjobSearchByRegion = "";
    private string companyinternshipSearchByTown = "";
    private bool companyinternshipSearchByTransportOffer;
    private bool companyjobSearchByTransportOffer;
    string companyinternshipSearchByArea = "";
    private List<Area> Areas = new();
    private List<Skill> Skills = new();
    private DateTime? internshipSearchByActiveStartDate;
    private string startDateInput;
    private string searchMonthInput;
    private string searchYearInput;
    private int? searchMonth;
    private int? searchYear;
    private int? selectedMonth;

    private bool IsCompanyRegistrationPage => NavigationManager.Uri.Contains("/companyRegistration");

    private string emailUsedToUploadThesis = "";
    private string professorNameSearch = "";
    private string professorSurnameSearch = "";
    private DateTime? thesisUploadDateTime;
    private int? thesisUploadMonth;
    bool showStudentThesisApplications = false;
    bool showStudentJobApplications = false;

    private CompanyJob job = new CompanyJob();
    private CompanyThesis thesis = new CompanyThesis();
    private AnnouncementAsCompany announcement = new AnnouncementAsCompany();
    private AnnouncementAsProfessor professorannouncement = new AnnouncementAsProfessor();
    private ProfessorThesis professorthesis = new ProfessorThesis();
    private IBrowserFile? uploadedFile;
    private bool showSuccessMessage = false;
    private bool showErrorMessage = false;

    private bool showErrorMessagesForAreasWhenUploadJobAsCompany = false;
    private bool showErrorMessagesForAreasWhenUploadInternshipAsCompany = false;
    private bool showErrorMessagesForSkillsWhenUploadThesisAsCompany = false;

    private bool showErrorMessageforUploadingjobsAsCompany = false;
    private bool showErrorMessageforUploadinginternshipsAsProfessor = false;
    private bool showErrorMessageforUploadingthesisAsCompany = false;
    private bool showErrorMessageforUploadingannouncementsAsCompany = false;
    private bool showErrorMessageforUploadingannouncementsAsProfessor = false;
    private bool showErrorMessageforUploadingThesisAsProfessor = false;
    private bool showErrorMessageForUploadingCompanyEvent = false;
    private bool showErrorMessageForUploadingProfessorEvent = false;
    private bool showSuccessUpdateMessage = false;
    private bool isEditing = false;
    private string? companyName;
    private string? companyAreas;
    private string? companyTelephone;
    private string? companyWebsite;
    private byte[]? companyLogo;
    private string? companyDescription;
    private string? companyShortName;
    private string? companyType;
    private string? companyActivity;
    private string? companyCountry;
    private string? companyLocation;
    private long? companyPermanentPC;
    private string? companyRegions;
    private string? companyTown;
    private string? companyHRName;
    private string? companyHRSurname;
    private string? companyHREmail;
    private string? companyHRTelephone;
    private string? companyAdminName;
    private string? companyAdminSurname;
    private string? companyAdminEmail;
    private string? companyAdminTelephone;
    private Dictionary<long, int> numberOfCompanyPeopleInputWhenCompanyShowsInterestInProfessorEvent = new Dictionary<long, int>();

    private string? professorName;
    private string? professorUniversity;
    private string? professorUniversityDepartment;
    private byte[]? professorImage;
    private string? professorSurname;
    private string? professorVathmidaDEP;
    private string? companyEmail;
    private string? professorPersonalTelephone;
    private string? professorWorkTelephone;
    private string? professorDepartment;
    private string? professorGeneralFieldOfWork;
    private string? professorGeneralSkills;
    private string? professorPersonalDescription;

    private string? professorLinkedInProfile;
    private string? professorPersonalWebsite;
    private string? professorScholarProfile;
    private string? professorOrchidProfile;

    private bool isForm1Visible = false;
    private bool isAnnouncementsFormVisible = false;
    private bool isAnnouncementsFormVisibleToShowGeneralAnnouncementsAndEventsAsCompany = false;
    private bool isAnnouncementsFormVisibleToShowGeneralAnnouncementsAndEventsAsRG = false;
    private bool isProfessorAnnouncementsFormVisible = false;
    private bool isProfessorThesisFormVisible = false;
    private bool isProfessorInternshipFormVisible = false;
    private bool isProfessorSearchStudentFormVisible = false;
    private bool isProfessorSearchCompanyFormVisible = false;
    private bool isRGSearchCompanyFormVisible = false;
    private bool isForm2Visible = false;
    private bool isShowActiveThesesAsCompanyFormVisible = false;
    private bool isThesisApplicationsVisible = false;
    private bool isAnnouncementsAsStudentVisible = false;
    private bool isAnnouncementsAsRGVisible = false;
    private bool isAnnouncementsAsProfessorVisible = false;
    private bool isSearchInternshipsAsStudentFiltersVisible = false;
    private bool isJobApplicationsAsStudentVisible  = false;
    private bool isJobPositionAsStudentFiltersVisible = false;
    private bool isInternshipApplicationsAsStudentVisible = false;
    private bool isInternshipSearchAsStudentFiltersVisible = false;
    private bool isEventSearchAsStudentVisible = false;

    private Dictionary<int, bool> positionDetails = new Dictionary<int, bool>();
    private List<CompanyJob> companyJobs;

    bool showApplications = false;
    private List<CompanyJobApplied> jobApplicationsmadeToCompany = new List<CompanyJobApplied>(); // List for job applications
    private Company companyData;

    private Company companyDatas = new Company();

    private long? selectedPositionId = null;

    private int totalStudentsCount = 0;
    private bool isDoughnutChartVisible = false;
    private bool isDepartmentDistributionChartVisible = false;
    private Dictionary<string, int> departmentDistribution = new Dictionary<string, int>();
    private Dictionary<string, int> skillDistribution = new Dictionary<string, int>();
    private bool hasReadAsCompanyPermission = false;

    private bool isUploadCompanyInternshipsFormVisible = false;
    private bool isUploadCompanyThesisFormVisible = false;
    private bool isShowActiveInternshipsAsCompanyFormVisible = false;
    private bool isShowActiveInternshipsAsProfessorFormVisible = false;
    private bool isUploadCompanyEventFormVisible = false;

    private bool isShowActiveJobsAsCompanyFormVisible = false;
    private bool isUploadProfessorEventFormVisible = false;

    private CompanyInternship companyInternship = new CompanyInternship();
    private CompanyThesis companyThesis = new CompanyThesis();

    private string selectedRegion = "";
    private string selectedTown = "";
    private Dictionary<string, bool> selectedTownsDictionary = new Dictionary<string, bool>();
    private HashSet<string> selectedRegions = new HashSet<string>();
    private Dictionary<string, bool> selectedRegionsDictionary = new Dictionary<string, bool>();
    private List<Area> availableAreas = new List<Area>();
    private List<Skill> availableSkills = new List<Skill>();
    private List<string> expandedAreas = new List<string>();
    private List<SelectedArea> selectedAreasForAssessment = new List<SelectedArea>();

    private string companyNameSearch { get; set; }
    private string emailSearch { get; set; }
    private string positionTypeSearch { get; set; }

    private string companyThesisSearch { get; set; }
    private string companyEmailSearch { get; set; }
    private string normalizedThesisSearch { get; set; }
    private string normalizedEmailSearch { get; set; }
    private string normalizedProfessorNameSearch { get; set; }
    private string normalizedProfessorSurnameSearch { get; set; }


    private DateTime? uploadDateSearch { get; set; }

    private CompanyInternship companyinternship = new CompanyInternship();
    private string selectedRegionForInternship;
    private List<string> filteredTownsForInternship;

    private List<CompanyInternship> internships = new List<CompanyInternship>(); 
    private List<ProfessorInternship> professorInternships = new List<ProfessorInternship>();


    private List<string> InternshipAsCompanyStatusOptions = new List<string> { "Δημοσιευμένη", "Μη Δημοσιευμένη", "Αποσυρμένη" };

    private string selectedStatusFilterForInternships = "Όλα";
    private string selectedStatusFilterForProfessorInternships = "Όλα";
    private string selectedStatusFilterForCompanyTheses = "Όλα";
    private string selectedStatusFilterForJobs = "Όλα";
    private string selectedStatusFilterForAnnouncements = "Όλα";
    private string selectedStatusFilterForAnnouncementsAsProfessor = "Όλα";
    private string selectedStatusFilterForEventsAsCompany = "Όλα";
    private string selectedStatusFilterForEventsAsProfessor = "Όλα";
    private string selectedStatusFilterForThesesAsProfessor = "Όλα";

    private CompanyInternship selectedInternship;
    private ProfessorInternship selectedProfessorInternship;
    private CompanyJob selectedJob;
    private CompanyThesis selectedCompanyThesis;
    private bool isEditPopupVisibleForInternships = false;
    private bool isEditPopupVisibleForProfessorInternships = false;
    private bool isEditPopupVisibleForJobs = false;
    private bool isEditPopupVisibleForCompanyThesis = false;
    private ThesisApplication selectedThesisApplication;
    private CompanyThesisApplied selectedCompanyThesisApplicationToShowAsStudent;
    private ProfessorThesisApplied selectedProfessorThesisApplicationToShowAsStudent;
    private ProfessorThesis selectedProfessorThesis;

    private bool showStudentInternshipApplications = false;

    private List<InternshipApplied> internshipApplications = new List<InternshipApplied>();
    private List<ProfessorInternshipApplied> professorInternshipApplications = new List<ProfessorInternshipApplied>();

    private CompanyJobApplied selectedJobApplication;
    private CompanyInternshipAreasForCheckboxes companyInternshipforcheckboxes = new CompanyInternshipAreasForCheckboxes();

    private List<Area> SelectedAreasWhenUploadInternshipAsCompany = new(); 
    private List<Area> SelectedAreasWhenUploadThesisAsCompany = new();
    private List<Area> SelectedAreasWhenUploadJobAsCompany = new();
    private List<Area> SelectedAreasWhenUploadEventAsCompany = new();
    private List<Skill> SelectedSkillsWhenUploadThesisAsCompany = new(); 

    private List<Area> SelectedAreasWhenUploadInternshipAsProfessor = new(); 
    private List<Area> SelectedAreasWhenUploadThesisAsProfessor = new();
    private List<Area> SelectedAreasWhenUploadJobAsProfessor = new();
    private List<Area> SelectedAreasWhenUploadEventAsProfessor = new();


    private List<Area> SelectedAreasToEditForCompanyJob = new List<Area>();
    private List<Area> SelectedAreasToEditForCompanyInternship = new List<Area>();
    private List<Area> SelectedAreasToEditForCompanyThesis = new List<Area>();
    private List<Skill> SelectedSkillsToEditForCompanyThesis  = new List<Skill>();
    private List<Skill> SelectedSkills = new(); 

    private List<Professor> professors = new List<Professor>();
    private int? selectedProfessorId;
    private int? selectedCompanyId;
    private string statusToEdit;

    private DateTime? selectedDateToSearchInternship { get; set; }
    private DateTime? finishEstimationDateToSearchInternship { get; set; }

    private DateTime? selectedDateToSearchJob { get; set; }

    private List<string> selectedAreas = new List<string>();
    private bool isInternshipAreasVisible = false;

    private Dictionary<long, bool> expandedInternships = new Dictionary<long, bool>();
    private Dictionary<long, bool> expandedProfessorInternships = new Dictionary<long, bool>();

    private Dictionary<long, bool> expandedJobs = new Dictionary<long, bool>();
    private Dictionary<long, bool> expandedCompanyTheses = new Dictionary<long, bool>();
    private Dictionary<long, bool> expandedCompanyThesesForProfessorInterest = new Dictionary<long, bool>();
    private Dictionary<long, bool> expandedProfessorThesesForCompanyInterest = new Dictionary<long, bool>();
    private int remainingCharactersInInternshipFieldUploadAsCompany = 120;
    private int remainingCharactersInThesisFieldUploadAsCompany = 120;
    private int remainingCharactersInThesisFieldUploadAsProfessor = 120;
    private int remainingCharactersInAnnouncementFieldUploadAsCompany = 120;
    private int remainingCharactersInAnnouncementFieldUploadAsProfessor = 120;
    private int remainingCharactersInInternshipDescriptionUploadAsCompany = 1000;
    private int remainingCharactersInCompanyEventDescription = 1000;
    private int remainingCharactersInEventTitleField = 120;

    private int remainingCharactersInJobFieldUploadAsCompany = 120;
    private int remainingCharactersInJobDescriptionUploadAsCompany = 1000;
    private int remainingCharactersInThesisDescriptionUploadAsCompany = 1000;
    private int remainingCharactersInThesisDescriptionUploadAsProfessor = 1000;
    private int remainingCharactersInAnnouncementDescriptionUploadAsCompany = 1000;
    private int remainingCharactersInAnnouncementDescriptionUploadAsProfessor = 1000;

    private string selectedStatusFilterToCountInternships = "Όλα";
    private int totalCount, publishedCount, unpublishedCount, withdrawnCount;
    private int totalCountForCompanyTheses, publishedCountForCompanyTheses, unpublishedCountForCompanyTheses, withdrawnCountForCompanyTheses;
    private int totalCountJobs, publishedCountJobs, unpublishedCountJobs, withdrawnCountJobs;
    private int totalCountAnnouncements, publishedCountAnnouncements, unpublishedCountAnnouncements, withdrawnCountAnnouncements;
    private int totalCountAnnouncementsAsProfessor, publishedCountAnnouncementsAsProfessor, unpublishedCountAnnouncementsAsProfessor, withdrawnCountAnnouncementsAsProfessor;
    private int totalCountThesesAsProfessor, publishedCountThesesAsProfessor, unpublishedCountThesesAsProfessor, withdrawnCountThesesAsProfessor;
    private int totalCountEventsAsCompany, publishedCountEventsAsCompany, unpublishedCountEventsAsCompany, withdrawnCountEventsAsCompany;
    private int totalCountEventsAsProfessor, publishedCountEventsAsProfessor, unpublishedCountEventsAsProfessor, withdrawnCountEventsAsProfessor;

    private int totalProfessorInternshipsCount, publishedProfessorInternshipsCount, unpublishedProfessorInternshipsCount, withdrawnProfessorInternshipsCount;

    private bool actionsPerformedToAcceptorRejectInternshipsAsCompany = false;
    private bool actionsPerformedToAcceptorRejectInternshipsAsProfessor = false;
    private bool actionsPerformedToAcceptorRejectJobsAsCompany = false;
    private bool actionsPerformedToAcceptorRejectThesisAsCompany = false;
    private bool actionsPerformedToAcceptorRejectThesesAsProfessor = false;

    private List<Company> companies = new List<Company>(); // Initialize with an empty list
    private Company selectedCompany;
    private Student selectedStudent;
    private List<string> availableTowns;

    private bool isSaveAnnouncementAsCompanySuccessful = false;
    private string saveAnnouncementAsCompanyMessage = string.Empty;
    private bool isSaveAnnouncementAsProfessorSuccessful = false;
    private string saveAnnouncementAsProfessorMessage = string.Empty;
    private string saveEventAsCompanyMessage = string.Empty;
    private string saveEventAsProfessorMessage = string.Empty;
    private bool isSaveThesisAsProfessorSuccessful = false;
    private string saveThesisAsProfessorMessage = string.Empty;

    private bool isPositionAreasVisible = false;
    private bool isThesisAreasVisible = false;
    private bool isCompanySearchStudentVisible = false;
    private bool isCompanySearchProfessorVisible = false;
    private bool isCompanySearchResearchGroupVisible = false;
    private bool isRGSearchProfessorVisible = false;
    private List<string> selectedPositionAreas = new List<string>();
    private List<string> selectedThesisAreas = new List<string>();

    private bool isUploadedAnnouncementsVisible = false;
    private bool isUploadedEventsVisible = false;
    private bool isUploadedEventsVisibleAsProfessor = false;

    private bool isUploadedAnnouncementsVisibleAsProfessor = false;
    private bool isUploadedThesesVisibleAsProfessor = false;
    private bool isUploadedCompanyThesesVisibleAsProfessor = false;
    private bool isUploadedProfessorThesesVisibleAsCompany = false;
    private List<AnnouncementAsCompany> UploadedAnnouncements { get; set; } = new List<AnnouncementAsCompany>();
    private List<CompanyThesis> UploadedCompanyTheses { get; set; } = new List<CompanyThesis>();
    private List<AnnouncementAsProfessor> UploadedAnnouncementsAsProfessor { get; set; } = new List<AnnouncementAsProfessor>();
    private List<CompanyEvent> UploadedEventsAsCompany { get; set; } = new List<CompanyEvent>();
    private List<ProfessorEvent> UploadedEventsAsProfessor { get; set; } = new List<ProfessorEvent>();
    private List<ProfessorThesis> UploadedThesesAsProfessor { get; set; } = new List<ProfessorThesis>();
    private List<CompanyThesis> UploadedCompanyThesesToSeeAsProfessor { get; set; } = new List<CompanyThesis>();
    private bool isEditModalVisible = false;
    private bool isEditModalVisibleForAnnouncementsAsProfessor = false;
    private bool isEditModalVisibleForThesesAsProfessor = false;
    private bool isEditModalVisibleForEventsAsCompany = false;
    private bool isEditModalVisibleForEventsAsProfessor = false;
    private AnnouncementAsCompany currentAnnouncement;
    private CompanyEvent currentCompanyEvent;
    private ProfessorEvent currentProfessorEvent;
    private CompanyThesis currentThesis;
    private AnnouncementAsProfessor currentAnnouncementAsProfessor;
    private ProfessorThesis currentThesisAsProfessor;

    private List<AnnouncementAsCompany> FilteredAnnouncements { get; set; }
    private List<CompanyEvent> FilteredCompanyEvents { get; set; }
    private List<ProfessorEvent> FilteredProfessorEvents { get; set; }
    private List<CompanyThesis> FilteredCompanyTheses { get; set; }
    private List<AnnouncementAsProfessor> FilteredAnnouncementsAsProfessor { get; set; }
    private List<ProfessorInternship> FilteredInternshipsAsProfessor { get; set; }
    private List<ProfessorThesis> FilteredThesesAsProfessor { get; set; }
    private bool isModalVisibleForJobs = false;
    private bool isModalVisibleToShowProfessorThesisAsProfessor = false;
    private bool isModalVisibleForProfessorThesisToShowDetailsAsStudent = false;
    private bool isModalVisibleForCompanyThesisToShowDetailsAsStudent = false;
    private bool isModalVisibleToShowCompanyThesisDetails = false;
    private bool isModalVisibleToEditCompanyThesisDetails = false;
    private bool isModalVisibleToShowStudentDetailsAsCompanyFromTheirHyperlinkNameInCompanyInternships = false;
    private bool isModalVisibleToShowStudentDetailsAsProfessorFromTheirHyperlinkNameInProfessorInternships = false;
    private CompanyJob currentJob;
    private CompanyJob currentJobApplicationMadeAsStudent;

    private List<AllTheses> sumUpThesesFromBothCompanyAndProfessor = new List<AllTheses>();
    private List<AllInternships> sumUpInternshipsFromBothCompanyAndProfessor = new List<AllInternships>();

    private string thesisSearchForInternshipsAsStudent;
    private string professorNameSearchForInternshipsAsStudent;
    private string professorSurnameSearchForInternshipsAsStudent;
    private string companyNameSearchForInternshipsAsStudent;
    private int? thesisUploadMonthForInternshipsAsStudent;

    private string thesisSearchForThesesAsStudent;
    private string professorNameSearchForThesesAsStudent;
    private string professorSurnameSearchForThesesAsStudent;
    private string companyNameSearchForThesesAsStudent;
    private int? thesisUploadMonthForThesesAsStudent;
    private DateTime? thesisStartDateForThesesAsStudent;


    private List<Area> availableAreasForProfessorThesis = new List<Area>();
    private List<Skill> availableSkillsForProfessorThesis = new List<Skill>();
    private List<long> selectedAreasForProfessorThesis = new List<long>();
    private List<long> selectedSkillsForProfessorThesis = new List<long>();

    private ProfessorThesis currentProfessorThesis;
    private ProfessorThesisApplied currentProfessorThesisToShowDetailsAsStudent;
    private CompanyThesis currentCompanyThesisToShowDetailsAsStudent;
    private bool isModalVisibleToShowProfessorThesisDetails = false;

    private Dictionary<long, bool> expandedTheses = new Dictionary<long, bool>();

    private IEnumerable<CompanyThesisApplied> companyThesisApplications;
    private IEnumerable<ProfessorThesisApplied> professorThesisApplications;

    private ProfessorThesis selectedProfessorThesisDetails;
    private Professor selectedProfessorDetailsForHyperlinkNameInThesisAsStudent;
    private Company selectedCompanyDetailsForHyperlinkNameInThesisAsStudent;
    private CompanyThesis selectedCompanyThesisDetails;
    private CompanyInternship selectedCompanyInternshipDetails;
    private ProfessorInternship selectedProfessorInternshipDetails;

    private List<CompanyThesis> companyThesesResultsToFindThesesAsProfessor = new List<CompanyThesis>();
    private bool searchPerformedToFindThesesAsProfessor = false;

    private List<ProfessorThesis> professorThesesResultsToFindThesesAsCompany = new List<ProfessorThesis>();
    private bool searchPerformedToFindThesesAsCompany = false;

    private CompanyThesis selectedCompanyThesisToSeeDetailsOnEyeIconAsProfessor;

    private Company selectedCompanyToSeeDetailsOnExpandedInterestAsProfessor;

    private bool isThesisDetailEyeIconModalVisibleToSeeAsProfessor = false;

    private ProfessorThesis selectedProfessorThesisToSeeDetailsOnEyeIconAsCompany;
    private bool isThesisDetailEyeIconModalVisibleToSeeAsCompany = false;
    private bool isExpandedModalVisibleToSeeCompanyDetailsAsProfessor = false;

    private bool isCompanyDetailModalVisibleForHyperlinkNameToShowCompanyDetailsToTheProfessor = false;
    private Company selectedCompanyNameAsHyperlinkToShowDetailsToTheProfessor;

    // Search criteria
    private string searchCompanyNameToFindThesesAsProfessor;
    private string searchThesisTitleToFindThesesAsProfessor;
    private string searchSupervisorToFindThesesAsProfessor;
    private string searchDepartmentToFindThesesAsProfessor;
    private string searchSkillsToFindThesesAsProfessor;
    private DateTime? searchStartingDateToFindThesesAsProfessor;

    private string searchProfessorNameToFindThesesAsCompany;
    private string searchProfessorSurnameToFindThesesAsCompany;
    private string searchProfessorThesisTitleToFindThesesAsCompany;
    private string searchAreasToFindThesesAsCompany;
    private string searchSkillsToFindThesesAsCompany;
    private DateTime? searchStartingDateToFindThesesAsCompany;

    private Dictionary<long, bool> professorInterestStatus = new Dictionary<long, bool>();
    private List<ThesisWithInterestStatus> thesesWithInterestStatus;

    private List<ProfessorThesisWithInterestStatus> professorthesesWithInterestStatus;

    private Professor? selectedProfessor;
    private ProfessorThesis? selectedCompanyThesiss;

    private CompanyEvent companyEvent = new CompanyEvent();
    private ProfessorEvent professorEvent = new ProfessorEvent();

    private DotNetObjectReference<MainLayout> dotNetHelper;
    private List<string> suggestions = new List<string>();

    private string searchNameAsCompanyToFindStudent = string.Empty;
    private string searchSurnameAsCompanyToFindStudent = string.Empty;
    private string searchRegNumberAsCompanyToFindStudent = string.Empty;
    private string searchDepartmentAsCompanyToFindStudent = string.Empty;

    private string InternshipStatus = string.Empty;
    private string ThesisStatus = string.Empty;

    private List<Student> searchResultsAsCompanyToFindStudent;

    private List<CompanyEvent> companyEventsToSeeAsStudent;
    private List<ProfessorEvent> professorEventsToSeeAsStudent;
    private CompanyEvent selectedCompanyEventToSeeAsStudent;
    private bool isCompanyEventsVisibleToSeeAsStudent = false;
    private bool isProfessorEventsVisibleToSeeAsStudent = false;
    private bool isModalVisibleToSeeCompanyEventsAsStudent = false; 

    private Company? currentCompanyDetailsToShowOnHyperlinkAsStudentForCompanyEvents;
    private Professor? currentProfessorDetailsToShowOnHyperlinkAsStudentForProfessorEvents;
    private Dictionary<long, bool> needsTransportForCompanyEvent = new Dictionary<long, bool>(); // Use a suitable type for the event ID
    private Dictionary<long, bool> needsTransportForProfessorEvent = new Dictionary<long, bool>(); // Use a suitable type for the event ID

    private Dictionary<long, string> selectedStartingPoint = new Dictionary<long, string>();
    private List<CompanyEvent> companyEvents = new List<CompanyEvent>();

    private List<ProfessorEvent> professorEvents = new List<ProfessorEvent>();


    private bool isProfessorDetailModalVisible = false;

    private string searchEmailAsProfessorToFindStudent = string.Empty;
    private string searchNameAsProfessorToFindStudent = string.Empty;
    private string searchSurnameAsProfessorToFindStudent = string.Empty;
    private string searchRegNumberAsProfessorToFindStudent = string.Empty;
    private string searchDepartmentAsProfessorToFindStudent = string.Empty;
    private string searchAreasOfExpertiseAsProfessorToFindStudent = string.Empty;
    private string searchKeywordsAsProfessorToFindStudent = string.Empty;


    private string searchCompanyEmailAsProfessorToFindCompany = string.Empty;
    private string searchCompanyNameENGAsProfessorToFindCompany = string.Empty;
    private string searchCompanyTypeAsProfessorToFindCompany = string.Empty;
    private string searchCompanyActivityrAsProfessorToFindCompany = string.Empty;
    private string searchCompanyTownAsProfessorToFindCompany = string.Empty;
    private string searchCompanyAreasAsProfessorToFindCompany = string.Empty;
    private string searchCompanyDesiredSkillsAsProfessorToFindCompany = string.Empty;

    private string searchCompanyEmailAsRGToFindCompany = string.Empty;
    private string searchCompanyNameENGAsRGToFindCompany = string.Empty;
    private string searchCompanyTypeAsRGToFindCompany = string.Empty;
    private string searchCompanyActivityrAsRGToFindCompany = string.Empty;
    private string searchCompanyTownAsRGToFindCompany = string.Empty;
    private string searchCompanyAreasAsRGToFindCompany = string.Empty;
    private string searchCompanyDesiredSkillsAsRGToFindCompany = string.Empty;

    private List<Student> searchResultsAsProfessorToFindStudent;
    private bool showStudentDetailsModal = false;

    private List<Company> searchResultsAsProfessorToFindCompany;
    private List<Company> searchResultsAsRGToFindCompany;
    private bool showCompanyDetailsModal = false;

    private bool isModalVisibleToShowCompanyDetailsAtProfessorThesisInterest = false;
    private bool isModalVisibleToShowprofessorDetailsAtCompanyThesisInterest  = false;
    private Company currentCompanyDetails = new Company();

    private bool isModalVisibleToShowStudentDetailsInNameAsHyperlinkForProfessorThesis = false;
    private Student currentStudentDetails = new Student();

    private bool showStudentDetailsModalWhenSearchForStudentsAsCompany = false;
    private Student selectedStudentWhenSearchForStudentsAsCompany;

    private string searchNameAsCompanyToFindProfessor;
    private string searchSurnameAsCompanyToFindProfessor;
    private string searchDepartmentAsCompanyToFindProfessor;
    private string searchAreasOfInterestAsCompanyToFindProfessor;
    private List<Professor> searchResultsAsCompanyToFindProfessor;

    private bool showProfessorDetailsModalWhenSearchForProfessorsAsCompany = false;
    private bool showProfessorDetailsModalWhenSearchForProfessorsAsRG = false;
    private Professor selectedProfessorWhenSearchForProfessorsAsCompany;
    private Professor selectedProfessorWhenSearchForProfessorsAsRG;

    private Professor currentProfessorDetails = new Professor();

    private string searchNameOrSurname = string.Empty; 
    private List<string> nameSurnameSuggestions = new List<string>();

    private string searchNameSurnameAsCompanyToFindProfessor { get; set; }
    private string searchNameSurnameAsStudentToFindProfessor { get; set; }
    private List<string> professorNameSurnameSuggestions { get; set; } = new();
    private List<string> thesisTitleSuggestions { get; set; } = new();
    private List<string> companyNameSuggestionsWhenSearchForProfessorThesisAutocompleteNameAsStudent { get; set; } = new();

    ////////////////////////////////////////////////////////////////////////////////////////
    private string searchNameSurnameAsRGToFindProfessor { get; set; }
    private List<string> professorNameSurnameSuggestionsAsRG { get; set; } = new();
    private string searchDepartmentAsRGToFindProfessor;
    private string searchAreasOfInterestAsRGToFindProfessor;
    private List<Professor> searchResultsAsRGToFindProfessor;
    ////////////////////////////////////////////////////////////////////////////////////////


    private string searchAreasOfExpertise = string.Empty;
    private string searchAreasOfInterest = string.Empty;
    private List<string> areasOfExpertiseSuggestions = new List<string>();
    private List<string> areasOfInterestSuggestions = new List<string>();
    private string searchKeywords = string.Empty;
    private List<string> keywordsSuggestions = new List<string>();

    private string searchAreasOfExpertiseAsRG = string.Empty;
    private string searchAreasOfInterestAsRG = string.Empty;
    private List<string> areasOfExpertiseSuggestionsAsRG = new List<string>();
    private List<string> areasOfInterestSuggestionsAsRG = new List<string>();
    private string searchKeywordsAsRG = string.Empty;
    private List<string> keywordsSuggestionsAsRG = new List<string>();

    private string? selectedDegreeLevel { get; set; }

    private DateTime currentMonth = DateTime.Today;
    private string[] daysOfWeek = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
    private int firstDayOfMonth => (int)new DateTime(currentMonth.Year, currentMonth.Month, 1).DayOfWeek;

    private List<ProfessorEvent> selectedProfessorDateEvents = new List<ProfessorEvent>();
    private List<CompanyEvent> selectedDateEvents = new List<CompanyEvent>();

    private bool isModalVisibleToShowEventsOnCalendarForEachClickedDay = false;
    private int daysInCurrentMonth => DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
    private int totalCellsInGrid;
    private int remainingCells => totalCellsInGrid - (firstDayOfMonth + daysInCurrentMonth);
    private int remainingCellsValue; 
    private DateTime? selectedDate;

    private Dictionary<int, List<CompanyEvent>> eventsForDate = new Dictionary<int, List<CompanyEvent>>();
    private Dictionary<int, List<ProfessorEvent>> eventsForDateForProfessors = new Dictionary<int, List<ProfessorEvent>>();

    private int selectedDay = 0; // To store the selected day
    private int highlightedDay = 0; // To store the day that needs to be highlighted
    private int adjustedFirstDayOfMonth => (firstDayOfMonth == 0) ? 6 : firstDayOfMonth - 1; // Adjust Sunday (0) to Saturday (6) and Monday (1) to 0

    public List<CompanyEvent> CompanyEventsToShowAtFrontPage { get; set; }
    public List<ProfessorEvent> ProfessorEventsToShowAtFrontPage { get; set; }

    private List<InterestInCompanyEvent> InterestedStudents = new();
    private List<InterestInCompanyEventAsProfessor> InterestedProfessorsInCompanyEvent = new();
    private long? selectedEventIdForStudents;
    private long? selectedEventIdForProfessors;

    private List<InterestInProfessorEvent> InterestedStudentsForProfessorEvent = new();
    private long? selectedEventIdForStudentsWhenShowInterestForProfessorEvent;

    private bool showModal = false;
    private bool showProfessorModal = false;
    private bool showModalForStudentsAtProfessorEventInterest = false;
    private InterestInCompanyEvent selectedStudentToShowDetailsForInterestinCompanyEvent;
    private Professor selectedProfessorToShowDetailsForInterestinCompanyEvent;
    private InterestInProfessorEvent selectedStudentToShowDetailsForInterestinProfessorEvent;
    private IEnumerable<CompanyThesisApplied> companythesisapplicants;
    private bool isFormValidToSaveAnnouncementAsCompany = true;
    private bool showErrorMessageforUploadingAnnouncementAsCompany = false;
    private bool isFormValidToSaveEventAsCompany = true;
    private bool isFormValidToSaveEventAsProfessor = true;

    private List<string> selectedKeywords = new();
    private List<string> selectedAreasOfExpertise = new();
    private List<string> selectedAreasOfInterest = new();

    private List<string> selectedKeywordsAsRG = new();
    private List<string> selectedAreasOfExpertiseAsRG = new();
    private List<string> selectedAreasOfInterestAsRG = new();

    private bool showErrorMessageforPostalCode = false;

    private const long MaxFileSize = 10 * 1024 * 1024; //10MB FILE SIZE

    // Track current page and calculate the total number of pages
    private int currentPage = 1;
    private int totalPages => selectedDateEvents?.Count ?? 0;

    private CompanyEvent? eventToDisplay => selectedDateEvents?.ElementAtOrDefault(currentPage - 1);
    private ProfessorEvent? professoreventToDisplay => selectedProfessorDateEvents?.ElementAtOrDefault(currentPage - 1);

    private string selectedEventType = "all"; // Default to show all events
    private object selectedEvent = null;

    private string selectedEventFilter { get; set; } = "All";
    private List<CompanyEvent> filteredCompanyEvents => 
    selectedEventFilter == "All" || selectedEventFilter == "Company" 
        ? selectedDateEvents 
        : new List<CompanyEvent>();

    private List<ProfessorEvent> filteredProfessorEvents => 
    selectedEventFilter == "All" || selectedEventFilter == "Professor" 
        ? selectedProfessorDateEvents 
        : new List<ProfessorEvent>();

    // Get the event for the current page

    // Methods for pagination
    private void NextPage()
    {
        if (currentPage < totalPages)
            currentPage++;
    }

    private void PreviousPage()
    {
        if (currentPage > 1)
            currentPage--;
    }

    private DateTime _professorEventDate;
    private int existingEventsCountToCheckAsProfessor = 0;

    private bool isCompanyDetailsModalOpenForHyperlinkNameAsStudent = false;
    private bool isModalOpenForCompanyEventToSeeAsStudent = false;
    private bool isModalOpenForProfessorEventToSeeAsStudent = false;
    private bool isModalOpenForCompanyDetailsToSeeAsStudent = false;
    private bool isModalOpenForProfessorDetailsToSeeAsStudent = false;
    private bool isCompanyDetailsModal2Visible = false; 
    private bool isCompanyDetailsModal3Visible = false; 
    private bool isInternshipDetailsModalVisible = false;
    private bool isJobDetailsModal2Visible = false;
    private bool isCompanyDetailsModal4Visible = false;
    private bool iscompanyDetailsModalVisible = false;

    private List<InterestInCompanyEventAsProfessor> filteredProfessorInterestForCompanyEvents = new();
    private List<InterestInProfessorEventAsCompany> filteredCompanyInterestForProfessorEvents = new();
    private ProfessorInternship professorInternship = new ProfessorInternship();

    private bool isCompanyDetailsModalOpenForJobSearch = false;
    private bool isCompanyDetailsModalOpenForJobShow = false;
    private Company? selectedCompanyDetailsForJobSearch = null;
    private Company? selectedCompanyDetailsForJobShow = null;
    private bool isJobDetailsModalVisibleToSeeJobApplicationsAsStudent = false;

    private bool isModalOpenToSeeCompanyDetails_ThesisStudentApplicationsToShow = false;
    private Company selectedCompanyDetails_ThesisStudentApplicationsToShow = null;  
    private bool isModalOpenToSeeProfessorDetails_ThesisStudentApplicationsToShow = false;
    private bool isModalOpenToSeeProfessorDetails_InternshipStudentApplicationsToShow = false;
    private Professor selectedProfessorDetails_ThesisStudentApplicationsToShow = null;

    private bool isModalOpenToSeeCompanyThesisDetails_ThesisStudentApplicationsToShow = false;
    private CompanyThesis selectedCompanyThesisDetails_ThesisStudentApplicationsToShow = null;

    private bool isModalOpenToSeeProfessorThesisDetails_ThesisStudentApplicationsToShow = false;
    private ProfessorThesis selectedProfessorThesisDetails_ThesisStudentApplicationsToShow = null;

    private bool isProfessorDetailsModalVisible_StudentInternshipApplicationsShow = false;
    private Professor selectedProfessorDetails = null;
    private bool isInternshipDetailsModalVisible_StudentInternshipApplicationsShow = false;

    private bool showCompanyThesisApplications = true;
    private bool showProfessorThesisApplications = true;
    private bool isLoading = false;
    private string filterValue = "all";

    private List<CompanyThesisApplied> companyApplicationsToShowOnPagination_SeeMyThesisApplicationsAsStudent;
    private List<ProfessorThesisApplied> professorApplicationsToShowOnPagination_SeeMyThesisApplicationsAsStudent;
    private int itemsPerPageToShowOnPagination_SeeMyThesisApplicationsAsStudent;
    private int totalPagesToShowOnPagination_SeeMyThesisApplicationsAsStudent;
    private int currentPageToShowOnPagination_SeeMyThesisApplicationsAsStudent;
    private List<CompanyThesisApplied> currentCompanyApplicationsToShowOnPagination_SeeMyThesisApplicationsAsStudent;
    private List<ProfessorThesisApplied> currentProfessorApplicationsToShowOnPagination_SeeMyThesisApplicationsAsStudent;



    private bool showCompanyThesisApplicationsToSearchAsStudent = true;
    private bool showProfessorThesisApplicationsToSearchAsStudent = true;
    private string selectedThesisFilter = "all";
    private List<AllTheses> publishedTheses;
    private string dropdownState = "all";

    private bool showExpandedAreasInCompanyThesisEditModalAsCompany = false;
    private bool showExpandedSkillsInCompanyThesisEditModalAsCompany = false;
    private void ToggleAreasInEditCompanyThesisModalAsCompany() => showExpandedAreasInCompanyThesisEditModalAsCompany = !showExpandedAreasInCompanyThesisEditModalAsCompany;
    private void ToggleSkillsInEditCompanyThesisModalAsCompany() => showExpandedSkillsInCompanyThesisEditModalAsCompany = !showExpandedSkillsInCompanyThesisEditModalAsCompany; 
    private bool showExpandedAreasInCompanyInternshipEditModalAsCompany = false;
    private void ToggleAreasInEditCompanyInternshipModalAsCompany() => showExpandedAreasInCompanyInternshipEditModalAsCompany = !showExpandedAreasInCompanyInternshipEditModalAsCompany;
    private void ToggleAreasInEditCompanyEventModal() => showExpandedAreasInCompanyEventEditModal = !showExpandedAreasInCompanyEventEditModal;



    private List<Area> selectedThesisAreasForProfessor = new();
    private List<Skill> selectedThesisSkillsForProfessor = new();

    private CompanyThesis companythesis;
    private CompanyInternship companyinternshipp;
    private CompanyJob companyjobb;
    private ProfessorInternship professorinternship;

    private IEnumerable<CompanyThesisApplied> applicants;
    private IEnumerable<InternshipApplied> companyInternshipApplicants;
    private IEnumerable<CompanyJobApplied> companyJobApplicants;
    private IEnumerable<ProfessorInternshipApplied> professorInternshipApplicants;


    private bool showExpandedAreasInCompanyEventEditModal = false;

    private List<Area> SelectedAreasToEditForCompanyEvent = new List<Area>();

    private List<string> AvailableTowns = new List<string>();

    private Dictionary<long, IEnumerable<CompanyJobApplied>> jobApplicants = new Dictionary<long, IEnumerable<CompanyJobApplied>>();
    private Dictionary<long, IEnumerable<CompanyJobApplied>> jobApplicantsMap = new Dictionary<long, IEnumerable<CompanyJobApplied>>();
    private Dictionary<long, IEnumerable<InternshipApplied>> internshipApplicantsMap = new Dictionary<long, IEnumerable<InternshipApplied>>();
    private Dictionary<long, IEnumerable<ProfessorInternshipApplied>> professorInternshipApplicantsMap = new Dictionary<long, IEnumerable<ProfessorInternshipApplied>>();
    private Dictionary<long, IEnumerable<CompanyThesisApplied>> companyThesisApplicantsMap = new Dictionary<long, IEnumerable<CompanyThesisApplied>>();
    private Dictionary<long, IEnumerable<ProfessorThesisApplied>> professorThesisApplicantsMap = new Dictionary<long, IEnumerable<ProfessorThesisApplied>>();
    private Dictionary<long, IEnumerable<CompanyThesis>> companyThesesProfessorsMap = new Dictionary<long, IEnumerable<CompanyThesis>>();


    private List<string> jobTitleAutocompleteSuggestionsWhenSearchForCompanyJobsAsStudent { get; set; } = new();
    private List<string> companyNameAutocompleteSuggestionsWhenSearchForCompanyJobsAsStudent { get; set; } = new();
    private List<string> internshipTitleAutocompleteSuggestionsWhenSearchInternshipAsStudent { get; set; } = new();

    private int currentCompanyEventpageSize = 3;
    private int pageSize = 3;
    public List<AnnouncementAsProfessor> ProfessorAnnouncements { get; set; }
    public List<AnnouncementAsCompany> CompanyAnnouncements { get; set; }
    private int totalPagesForCompanyEvents => (int)Math.Ceiling((double)CompanyEventsToShowAtFrontPage.Where(a => a.CompanyEventStatus == "Δημοσιευμένη").Count() / currentCompanyEventpageSize);
    private int expandedCompanyEventId = -1;
    private string fetchError;
    private int expandedProfessorEventId = -1;
    private int expandedAnnouncementId = -1;
    private bool isHidden = false;
    private int expandedProfessorAnnouncementId = -1;
    private int totalPagesForProfessorAnnouncements => (int)Math.Ceiling((double)ProfessorAnnouncements.Where(a => a.ProfessorAnnouncementStatus == "Δημοσιευμένη").Count() / pageSize);
    private int currentPageForProfessorAnnouncements = 1;
    private int currentCompanyEventPage = 1;
    private int currentProfessorEventPage = 1;
    private int totalPagesForProfessorEvents => (int)Math.Ceiling((double)ProfessorEventsToShowAtFrontPage.Where(a => a.ProfessorEventStatus == "Δημοσιευμένη").Count() / currentProfessorEventpageSize);
    private int currentProfessorEventpageSize = 3;

    private int totalPagesForCompanyAnnouncements => (int)Math.Ceiling((double)CompanyAnnouncements.Where(a => a.CompanyAnnouncementStatus == "Δημοσιευμένη").Count() / pageSize);
    private int currentPageForCompanyAnnouncements = 1;

    private Professor selectedProfessorDetailsForHyperlinkNameInInternshipAsStudent;
    private Company selectedCompanyDetailsForHyperlinkNameInInternshipAsStudent;
    private bool isCompanyDetailsModalOpenForHyperlinkNameAsStudentForCompanyInternship;


    private List<NewsArticle> newsArticles;
    private List<NewsArticle> svseNewsArticles;
    public List<AnnouncementAsCompany> Announcements { get; set; }

    private bool showSuccessMessageWhenSaveInternshipAsCompany = false;

    private string searchAreasInputToFindThesesAsCompany = string.Empty;
    private List<string> areaSuggestionsToFindThesesAsCompany = new();
    private List<string> selectedAreasToFindThesesAsCompany = new();

    // For filtering
    private string filterValueForInternships = "all";
    private bool showCompanyInternshipApplications = true;
    private bool showProfessorInternshipApplications = true;

    // For pagination
    private int currentPageForInternshipsToSee = 1;
    private int pageSizeForInternshipsToSee = 3; // Adjust as needed
    private int totalInternshipCount = 0;
    private int totalPagesForInternshipsToSee = 1;

    private HashSet<long> interestedProfessorEventIds = new();
    private HashSet<long> alreadyInterestedCompanyEventIds = new();
    private HashSet<long> professorThesisIdsApplied = new();
    private HashSet<long> companyThesisIdsApplied = new();
    private HashSet<long> jobIdsApplied = new();
    private HashSet<long> internshipIdsApplied = new();
    private HashSet<long> professorInternshipIdsApplied = new();


    private int currentInternshipPage = 1;
    private int InternshipsPerPage = 3; // Set to show 3 internships per page
    private int totalInternshipPages => (int)Math.Ceiling((double)publishedInternships.Count / InternshipsPerPage);


    // STUDENT PAGINATION ON TABLE DROPDOWNS - ALL TABS
    private int[] pageSizeOptions_SeeMyThesisApplicationsAsStudent = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] pageSizeOptions_SearchForThesisAsStudent = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] pageSizeOptions_SeeMyJobApplicationsAsStudent = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] pageSizeOptions_SearchForJobsAsStudent = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] pageSizeOptions_SeeMyInternshipApplicationsAsStudent = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] pageSizeOptions_SearchForInternshipsAsStudent = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] pageSizeOptions_SearchForEventsAsStudent = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    // COMPANY PAGINATION ON TABLE DROPDOWNS - ALL TABS
    private int[] pageSizeOptions_SeeMyUploadedAnnouncementsAsCompany = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] pageSizeOptions_SeeMyUploadedJobsAsCompany = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] pageSizeOptions_SeeMyUploadedInternshipsAsCompany = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] pageSizeOptions_SeeMyUploadedThesesAsCompany = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] pageSizeOptions_SearchForProfessorThesesAsCompany = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] pageSizeOptions_SeeMyUploadedEventsAsCompany = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] pageSizeOptions_SearchForStudentsAsCompany = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] pageSizeOptions_SearchForProfessorsAsStudent = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    // SEARCH FOR PROFESSOR AS RG
    private int[] pageSizeOptions_SearchForProfessorsAsRG = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    // PROFESSOR PAGINATION ON TABLE DROPDOWNS - ALL TABS
    private int[] pageSizeOptions_SeeMyUploadedAnnouncementsAsProfessor = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] pageSizeOptions_SeeMyUploadedThesesAsProfessor = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] companyThesesPageSize_SearchForCompanyThesesAsProfessor = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] pageSizeOptions_SeeMyUploadedInternshipsAsProfessor = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] pageSizeOptions_SeeMyUploadedEventsAsProfessor = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] studentSearchPageSizeOptions = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    private int[] companySearchPageSizeOptions = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };


    private List<ProfessorInternshipApplied> _professorinternshipapplicants = new();
    private string uploadErrorMessage = string.Empty;
    private bool uploadSuccess = false;
    private ResearchGroup researchGroupData;

    private Student selectedStudentFromCache;

    private Dictionary<string, Student> studentDataCache = new Dictionary<string, Student>();
    private Dictionary<string, Company> companyDataCache = new Dictionary<string, Company>();
    private Dictionary<string, Professor> professorDataCache = new Dictionary<string, Professor>();
    private Dictionary<long, CompanyJob> jobDataCache = new Dictionary<long, CompanyJob>();
    private Dictionary<long, CompanyInternship> internshipDataCache = new Dictionary<long, CompanyInternship>();
    private Dictionary<long, ProfessorInternship> professorInternshipDataCache = new Dictionary<long, ProfessorInternship>();
    private Dictionary<long, CompanyThesis> thesisDataCache = new Dictionary<long, CompanyThesis>();
    private Dictionary<long, ProfessorThesis> professorThesisDataCache = new Dictionary<long, ProfessorThesis>();


    private bool hasExistingEventsOnSelectedDate = false;
    private int existingEventsCount = 0;

    private async Task CheckExistingEventsForDate()
    {
        if (companyEvent.CompanyEventActiveDate.Date > DateTime.Today.Date)
        {
            // Check for existing company events on this date
            var companyEventsCount = await dbContext.CompanyEvents
                .CountAsync(e => e.CompanyEventActiveDate.Date == companyEvent.CompanyEventActiveDate.Date && 
                                e.CompanyEventStatus == "Δημοσιευμένη");

            // Check for existing professor events on this date
            var professorEventsCount = await dbContext.ProfessorEvents
                .CountAsync(e => e.ProfessorEventActiveDate.Date == companyEvent.CompanyEventActiveDate.Date && 
                                e.ProfessorEventStatus == "Δημοσιευμένη");

            existingEventsCount = companyEventsCount + professorEventsCount;
            hasExistingEventsOnSelectedDate = existingEventsCount > 0;
        }
        else
        {
            hasExistingEventsOnSelectedDate = false;
        }

        StateHasChanged();
    }

    private async Task HandleDateChange(ChangeEventArgs e)
    {
        if (DateTime.TryParse(e.Value?.ToString(), out DateTime newDate))
        {
            companyEvent.CompanyEventActiveDate = newDate;
            await CheckExistingEventsForDate();
        }
    }


    private AnnouncementAsCompany? selectedCompanyAnnouncementToSeeDetailsAsCompany;
    private void OpenCompanyAnnouncementDetailsModal(AnnouncementAsCompany currentAnnouncement)
    {
        selectedCompanyAnnouncementToSeeDetailsAsCompany = currentAnnouncement;
    }

    private void CloseCompanyAnnouncementDetailsModal()
    {
        selectedCompanyAnnouncementToSeeDetailsAsCompany = null;
    }

    private List<string> DegreeLevel = new List<string>
    {
        "Προπτυχιακός Φοιτητής",
        "Μεταπτυχιακός Φοιτητής",
        "Υποψήφιος Διδάκτορας",
    };





    private List<string> ForeasType = new List<string>
    {
        "Ιδιωτικός Φορέας",
        "Δημόσιος Φορέας",
        "Μ.Κ.Ο.",
        "Άλλο"
    };

    private List<string> Regions = new List<string>
    {
        "Ανατολική Μακεδονία και Θράκη",
        "Κεντρική Μακεδονία",
        "Δυτική Μακεδονία",
        "Ήπειρος",
        "Θεσσαλία",
        "Ιόνια Νησιά",
        "Δυτική Ελλάδα",
        "Κεντρική Ελλάδα",
        "Αττική",
        "Πελοπόννησος",
        "Βόρειο Αιγαίο",
        "Νότιο Αιγαίο",
        "Κρήτη"
    };
    private Dictionary<string, List<string>> RegionToTownsMap = new Dictionary<string, List<string>>
    {
        {"Ανατολική Μακεδονία και Θράκη", new List<string> {"Κομοτηνή", "Αλεξανδρούπολη", "Καβάλα", "Ξάνθη", "Δράμα", "Ορεστιάδα", "Διδυμότειχο", "Ίασμος", "Νέα Βύσσα", "Φέρες"}},
        {"Κεντρική Μακεδονία", new List<string> {"Θεσσαλονίκη", "Κατερίνη", "Σέρρες", "Κιλκίς", "Πολύγυρος", "Ναούσα", "Έδεσσα", "Γιαννιτσά", "Καβάλα", "Άμφισσα"}},
        {"Δυτική Μακεδονία", new List<string> {"Κοζάνη", "Φλώρινα", "Καστοριά", "Γρεβενά"}},
        {"Ήπειρος", new List<string> {"Ιωάννινα", "Άρτα", "Πρέβεζα", "Ηγουμενίτσα"}},
        {"Θεσσαλία", new List<string> {"Λάρισα", "Βόλος", "Τρίκαλα", "Καρδίτσα"}},
        {"Ιόνια Νησιά", new List<string> {"Κέρκυρα", "Λευκάδα", "Κεφαλονιά", "Ζάκυνθος", "Ιθάκη", "Παξοί", "Κυθήρα"}},
        {"Δυτική Ελλάδα", new List<string> {"Πάτρα", "Μεσολόγγι", "Αμφιλοχία", "Πύργος", "Αιγίο", "Ναύπακτος"}},
        {"Κεντρική Ελλάδα", new List<string> {"Λαμία", "Χαλκίδα", "Λιβαδειά", "Θήβα", "Αλιάρτος", "Αμφίκλεια"}},
        {"Αττική", new List<string> {"Αθήνα", "Πειραιάς", "Κηφισιά", "Παλλήνη", "Αγία Παρασκευή", "Χαλάνδρι", "Καλλιθέα", "Γλυφάδα", "Περιστέρι", "Αιγάλεω"}},
        {"Πελοπόννησος", new List<string> {"Πάτρα", "Τρίπολη", "Καλαμάτα", "Κορίνθος", "Άργος", "Ναύπλιο", "Σπάρτη", "Κυπαρισσία", "Πύργος", "Μεσσήνη"}},
        {"Βόρειο Αιγαίο", new List<string> {"Μυτιλήνη", "Χίος", "Λήμνος", "Σάμος", "Ίκαρος", "Λέσβος", "Θάσος", "Σκύρος", "Ψαρά"}},
        {"Νότιο Αιγαίο", new List<string> {"Ρόδος", "Κως", "Κρήτη", "Κάρπαθος", "Σαντορίνη", "Μύκονος", "Νάξος", "Πάρος", "Σύρος", "Άνδρος"}},
        {"Κρήτη", new List<string> {"Ηράκλειο", "Χανιά", "Ρέθυμνο", "Αγία Νικόλαος", "Ιεράπετρα", "Σητεία", "Κίσαμος", "Παλαιόχωρα", "Αρχάνες", "Ανώγεια"}},
    };

    protected override async Task OnInitializedAsync()
    {
        LoadAnalytics();
        await LoadAllStudentData();
        await LoadCompanyJobData();
        //await LoadCompanyJobApplicantData();
        await ShowMyJobApplicationsAsStudent();

        await LoadStudentsWithAuth0DetailsAsync();
        await LoadStudentsAsync();
        await LoadInternships();
        await LoadProfessorInternships();
        await LoadJobs();
        await LoadThesesAsCompany();
        await LoadThesesAsProfessor();
        await LoadAreasAsync();
        await LoadSkillsAsync();
        await LoadProfessors();
        await LoadCompanies();
        await LoadUploadedAnnouncementsAsync();
        await LoadUploadedAnnouncementsAsProfessorAsync();
        await LoadUploadedThesesAsProfessorAsync();
        await CalculateStatusCountsForInternships();
        await CalculateStatusCountsForJobs();
        await CalculateStatusCountsForAnnouncements();
        await CalculateStatusCountsForAnnouncementsAsProfessor();
        await CalculateStatusCountsForThesesAsProfessor();
        await CalculateStatusCountsForCompanyTheses();
        await LoadToSeeUploadedCompanyThesesAsProfessorAsync();
        await LoadUploadedEventsAsync();
        await LoadUploadedEventsAsyncAsProfessor();
        await LoadThesisData();
        FilterAnnouncements();
        FilterAnnouncementsAsProfessor();
        FilterThesesAsProfessor();
        FilterProfessorInternships();
        //FilterThesesAsCompany();
        FilterCompanyEvents();
        FilterProfessorEvents();
        await LoadProfessorInterestsForCompanyEvents();
        professorthesisApplications = new List<ProfessorThesisApplied>();
        companythesisApplications = new List<CompanyThesisApplied>();
        UploadedCompanyTheses = await GetCompanyThesesAsync(); 
        companyThesesResultsToFindThesesAsProfessor = await dbContext.CompanyTheses.ToListAsync();
        //await SearchThesisApplicationsAsStudent();

        // Populate available schools and departments
        AvailableSchools = StudentsWithAuth0Details.Select(s => s.School).Distinct().ToList();
        AvailableDepartments = StudentsWithAuth0Details.Select(s => s.Department).Distinct().ToList();

        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        UserRole = user.FindFirst(ClaimTypes.Role)?.Value; // Get user's role
        var userEmail = user.FindFirst("name")?.Value; // Assuming "name" claim contains the user's email
        var userSignUpDate = user.FindFirst("created_at")?.Value;
        var userLatestLoginDate = user.FindFirst("last_login")?.Value;
        Console.WriteLine($"Sign Up Date: {userSignUpDate}");
        Console.WriteLine($"Latest Login Date: {userLatestLoginDate}");



        jobs = await dbContext.CompanyJobs
            .Include(j => j.Company)  // Just add this line
            .ToListAsync(); 

        totalStudentsCount = await dbContext.Students.CountAsync();

        await LoadSkillDistributionAsync();
        await LoadDepartmentDistributionAsync();

        companyInternship.CompanyInternshipActivePeriod = DateTime.Now;
        companyInternship.CompanyInternshipFinishEstimation = DateTime.Now;
        companyInternship.CompanyInternshipUploadDate = DateTime.Now;
        announcement.CompanyAnnouncementUploadDate = DateTime.Now;
        companyInternship.CompanyUploadedInternshipStatus = "Μη Δημοσιευμένη";

        newsArticles = await FetchNewsArticlesAsync();
        svseNewsArticles = await FetchSVSENewsArticlesAsync();
        Announcements = await FetchAnnouncementsAsync();
        ProfessorAnnouncements = await FetchProfessorAnnouncementsAsync();
        CompanyAnnouncements = await FetchCompanyAnnouncementsAsync();

        CompanyEventsToShowAtFrontPage = await FetchCompanyEventsAsync();
        ProfessorEventsToShowAtFrontPage = await FetchProfessorEventsAsync();


        thesisStartDateForThesesAsStudent  = DateTime.Now;
        companyEvent.CompanyEventActiveDate = DateTime.Now;

        job.PositionActivePeriod  = DateTime.Now;
        job.PositionStatus = "Μη Δημοσιευμένη";


        announcement.CompanyAnnouncementTimeToBeActive = DateTime.Now;
        professorannouncement.ProfessorAnnouncementTimeToBeActive = DateTime.Now;
        thesis.CompanyThesisStartingDate = DateTime.Now;

        availableAreasForProfessorThesis = await dbContext.Areas.ToListAsync();
        availableSkillsForProfessorThesis = await dbContext.Skills.ToListAsync();
        professorthesis.ThesisActivePeriod = DateTime.Now;

        searchStartingDateToFindThesesAsProfessor  = DateTime.Now;


        // Ensure you're observing the entire state to understand if something is affecting the rendering
        Console.WriteLine("Interest Status Dictionary: " + string.Join(", ", professorInterestStatus.Select(kv => $"{kv.Key}: {kv.Value}")));

        //dotNetHelper = DotNetObjectReference.Create(this);

        CompanyEventsToShowAtFrontPage = await FetchCompanyEventsAsync();
        ProfessorEventsToShowAtFrontPage = await FetchProfessorEventsAsync();

        LoadEventsForCalendar();
        CalculateRemainingCells();



        if (user.Identity.IsAuthenticated)
        {
            var roleClaim = user.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            if (roleClaim != null)
            {
                companyName = user.Identity?.Name ?? "Anonymous User";
                var userRole = roleClaim.Value;
                hasReadAsCompanyPermission = userRole == "Company";
            }
        }

        // Check if the user has the Student role
        if (user.IsInRole("Student"))
        {
            ShowStudentRegistrationButton = true;
        }
        // Check if the user has the Company role
        if (user.IsInRole("Company"))
        {
            ShowCompanyRegistrationButton = true;
        }
        // Check if the user has the Professor role
        if (user.IsInRole("Professor"))
        {
            ShowProfessorRegistrationButton = true;
        }
        if (user.IsInRole("Admin"))
        {
            ShowAdminRegistrationButton = true;
        }

        if (user.IsInRole("ResearchGroup"))
        {
            ShowAdminRegistrationButton = true;
        }

        if (!string.IsNullOrEmpty(userEmail))
        {
            CurrentUserEmail = userEmail; // Assign userEmail to CurrentUserEmail

            // Check if student is registered based on email
            try
            {
                using (var scope = ServiceProvider.CreateScope())
                {
                    var scopedDbContext = scope.ServiceProvider.GetRequiredService<Data.AppDbContext>();
                    isStudentRegistered = await scopedDbContext.Students.AnyAsync(s => s.Email == CurrentUserEmail);
                }
                isStudentInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking student registration: {ex.Message}");
                isStudentInitialized = true;
            }

            // Check if company is registered based on email
            try
            {
                using (var scope = ServiceProvider.CreateScope())
                {
                    var scopedDbContext = scope.ServiceProvider.GetRequiredService<Data.AppDbContext>();
                    isCompanyRegistered = await scopedDbContext.Companies.AnyAsync(s => s.CompanyEmail == CurrentUserEmail);

                    // Fetch company data
                    companyData = await scopedDbContext.Companies
                        .FirstOrDefaultAsync(c => c.CompanyEmail == CurrentUserEmail);

                    if (companyData == null)
                    {
                        Console.WriteLine($"Company with email {CurrentUserEmail} not found.");
                    }
                }
                isCompanyInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking company registration: {ex.Message}");
                isCompanyInitialized = true;
            }

            // Check if professor is registered based on email
            try
            {
                using (var scope = ServiceProvider.CreateScope())
                {
                    var scopedDbContext = scope.ServiceProvider.GetRequiredService<Data.AppDbContext>();
                    isProfessorRegistered = await scopedDbContext.Professors.AnyAsync(s => s.ProfEmail == CurrentUserEmail);
                }
                isProfessorInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking professor registration: {ex.Message}");
                isProfessorInitialized = true;
            }
            // Load user data
            try
            {
                using (var scope = ServiceProvider.CreateScope())
                {
                    var scopedDbContext = scope.ServiceProvider.GetRequiredService<Data.AppDbContext>();
                    userData = await scopedDbContext.Students.FirstOrDefaultAsync(s => s.Email == userEmail);

                if (userData == null)
                {
                    Console.WriteLine($"User with email {userEmail} not found.");
                }
                else
                {
                    //interest in professor events as student
                    interestedProfessorEventIds = (await scopedDbContext.InterestInProfessorEvents
                        .Where(e => e.StudentUniqueIDShowInterestForEvent == userData.Student_UniqueID && 
                                    e.StudentEmailShowInterestForEvent == userData.Email)
                        .Select(e => e.RNGForProfessorEventInterest)
                        .ToListAsync())
                        .ToHashSet();

                    //interest in company events as student
                    alreadyInterestedCompanyEventIds = (await scopedDbContext.InterestInCompanyEvents
                        .Where(e => e.StudentUniqueIDShowInterestForEvent == userData.Student_UniqueID && 
                                    e.StudentEmailShowInterestForEvent == userData.Email)
                        .Select(e => e.RNGForCompanyEventInterest)
                        .ToListAsync())
                        .ToHashSet();

                    // Get applied professor theses
                    professorThesisIdsApplied = scopedDbContext.ProfessorThesesApplied
                        .Where(x => x.StudentUniqueIDAppliedForProfessorThesis == userData.Student_UniqueID && 
                                    x.StudentEmailAppliedForProfessorThesis == userData.Email)
                        .Select(x => x.RNGForProfessorThesisApplied)
                        .ToHashSet();

                    // Get applied company theses
                    companyThesisIdsApplied =  scopedDbContext.CompanyThesesApplied
                        .Where(x => x.StudentUniqueIDAppliedForThesis == userData.Student_UniqueID && 
                                    x.StudentEmailAppliedForThesis == userData.Email)
                        .Select(x => x.RNGForCompanyThesisApplied)
                        .ToHashSet();

                    // Get applied company applied jobs
                    jobIdsApplied = scopedDbContext.CompanyJobsApplied
                        .Where(x => x.StudentUniqueIDAppliedForCompanyJob == userData.Student_UniqueID && 
                                    x.StudentEmailAppliedForCompanyJob == userData.Email)
                        .Select(x => x.RNGForCompanyJobApplied)
                        .ToHashSet();

                    // Get applied company internships
                    var companyApplied = scopedDbContext.InternshipsApplied
                        .Include(x => x.StudentDetails)
                        .Where(x => x.StudentDetails.StudentUniqueIDAppliedForInternship == userData.Student_UniqueID)
                        .Select(x => x.RNGForInternshipApplied)
                        .ToHashSet();

                    // Get applied professor internships 
                    var professorApplied = scopedDbContext.ProfessorInternshipsApplied
                        .Include(x => x.StudentDetails)
                        .Where(x => x.StudentDetails.StudentUniqueIDAppliedForProfessorInternship == userData.Student_UniqueID)
                        .Select(x => x.RNGForProfessorInternshipApplied)
                        .ToHashSet();

                    // Combine both sets
                    internshipIdsApplied = companyApplied;
                    professorInternshipIdsApplied = professorApplied;
                }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading user data: {ex.Message}");
            }
            // Check if ResearchGroup is registered based on email
            try
            {
                using (var scope = ServiceProvider.CreateScope())
                {
                    var scopedDbContext = scope.ServiceProvider.GetRequiredService<Data.AppDbContext>();
                    isResearchGroupRegistered = await scopedDbContext.ResearchGroups.AnyAsync(s => s.ResearchGroupEmail == CurrentUserEmail);

                    // Fetch research group data
                    researchGroupData = await scopedDbContext.ResearchGroups
                        .FirstOrDefaultAsync(c => c.ResearchGroupEmail == CurrentUserEmail);

                    if (researchGroupData == null)
                    {
                        Console.WriteLine($"ResearchGroup with email {CurrentUserEmail} not found.");
                    }
                }
                isResearchGroupInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking research group registration: {ex.Message}");
                isResearchGroupInitialized = true;
            }

        }

        if (!string.IsNullOrEmpty(userEmail))
        {
            // First get the company details from the Company table
            var company = await dbContext.Companies
                .FirstOrDefaultAsync(c => c.CompanyEmail == userEmail);

            if (company != null)
            {
                companyName = company.CompanyName; // Get name from Company model

                // Now get all jobs for this company
                jobs = await dbContext.CompanyJobs
                    .Where(job => job.EmailUsedToUploadJobs == userEmail)
                    .Include(job => job.Company) // Optional: include company details if needed
                    .ToListAsync();
            }
        }



        var professor = await dbContext.Professors.FirstOrDefaultAsync(c => c.ProfEmail == userEmail);
        if (professor != null)
        {
            professorName = professor.ProfName;
            professorSurname = professor.ProfSurname;
            professorDepartment = professor.ProfDepartment;
            professorImage = professor.ProfImage;
            professorUniversity = professor.ProfUniversity;
            professorVathmidaDEP = professor.ProfVahmidaDEP;
            professorPersonalTelephone = professor.ProfPersonalTelephone;
            professorWorkTelephone = professor.ProfWorkTelephone;
            professorLinkedInProfile = professor.ProfLinkedInSite;
            professorPersonalWebsite = professor.ProfPersonalWebsite;
            professorScholarProfile = professor.ProfScholarProfile;
            professorOrchidProfile = professor.ProfOrchidProfile;
            professorGeneralFieldOfWork = professor.ProfGeneralFieldOfWork;
            professorGeneralSkills = professor.ProfGeneralSkills;
            professorPersonalDescription = professor.ProfPersonalDescription;


        }

        foreach (var companyEvent in companyEvents)
        {
            if (!selectedStartingPoint.ContainsKey(companyEvent.RNGForEventUploadedAsCompany))
            {
                // Initialize the value to null or a default starting point
                selectedStartingPoint[companyEvent.RNGForEventUploadedAsCompany] = null;
            }
        }

        companyEvents = await dbContext.CompanyEvents.ToListAsync();
        foreach (var companyEvent in companyEvents)
        {
            if (!selectedStartingPoint.ContainsKey(companyEvent.RNGForEventUploadedAsCompany))
            {
                selectedStartingPoint[companyEvent.RNGForEventUploadedAsCompany] = null;
            }
        }

        companythesis = await GetCompanyThesisAsync();
        companyinternshipp = await GetCompanyInternshipsAsync();
        companyjobb = await GetCompanyJobsAsync();

        job.PositionContactPerson = companyData?.CompanyHREmail;
        job.PositionAddressLocation = companyData?.CompanyLocation;
        job.PositionContactTelephonePerson = companyData?.CompanyHRTelephone;
        job.PositionPerifereiaLocation = companyData?.CompanyRegions;
        job.PositionDimosLocation = companyData?.CompanyTown;
        job.PositionPostalCodeLocation = companyData?.CompanyPC.ToString();


        companyInternship.CompanyInternshipContactPerson = companyData?.CompanyHREmail;
        companyInternship.CompanyInternshipAddress = companyData?.CompanyLocation;
        companyInternship.CompanyInternshipContactTelephonePerson = companyData?.CompanyHRTelephone;
        companyInternship.CompanyInternshipPerifereiaLocation = companyData?.CompanyRegions;
        companyInternship.CompanyInternshipDimosLocation = companyData?.CompanyTown;
        companyInternship.CompanyInternshipPostalCodeLocation = companyData?.CompanyPC.ToString();


        professorEvents = await dbContext.ProfessorEvents.ToListAsync();
        // Before assigning values, ensure Professor exists
        if (professorEvent.Professor == null)
        {
            professorEvent.Professor = new Professor();
        }

        // Now you can safely assign
        professorEvent.Professor.ProfUniversity = professorUniversity;
        professorEvent.ProfessorEventUniversityDepartment = professorDepartment;
        ProfessorEventDate = DateTime.Today;

        companyLogo = companyData?.CompanyLogo;
        companyTelephone = companyData?.CompanyTelephone;
        companyWebsite = companyData?.CompanyWebsite;
        companyAreas = companyData?.CompanyAreas;
        companyDescription = companyData?.CompanyDescription;
        companyShortName = companyData?.CompanyShortName;
        companyType = companyData?.CompanyType;
        companyActivity = companyData?.CompanyActivity;
        companyCountry = companyData?.CompanyCountry;
        companyLocation = companyData?.CompanyLocation;
        companyPermanentPC = companyData?.CompanyPC;
        companyRegions = companyData?.CompanyRegions;
        companyTown = companyData?.CompanyTown;
        companyHRName = companyData?.CompanyHRName;
        companyHRSurname = companyData?.CompanyHRSurname;
        companyHREmail = companyData?.CompanyHREmail;
        companyHRTelephone = companyData?.CompanyHRTelephone;
        companyAdminName = companyData?.CompanyAdminName;
        companyAdminSurname = companyData?.CompanyAdminSurname;
        companyAdminEmail = companyData?.CompanyAdminEmail;
        companyAdminTelephone = companyData?.CompanyAdminTelephone;
        numberOfCompanyPeopleInputWhenCompanyShowsInterestInProfessorEvent[professorEvent.RNGForEventUploadedAsProfessor] = 1; 

        professorInternship.ProfessorInternshipActivePeriod = DateTime.Today;
        professorInternship.ProfessorInternshipUploadDate = DateTime.Today;
        professorInternship.ProfessorInternshipFinishEstimation = DateTime.Today;
        professorInternship.ProfessorInternshipContactPerson = professorName +" "+ professorSurname;
        professorInternship.ProfessorInternshipContactTelephonePerson = professorPersonalTelephone;
        professorInternship.ProfessorEmailUsedToUploadInternship = userEmail;

        var companyjob = await dbContext.CompanyJobs.FirstOrDefaultAsync(c => c.Id == job.Id);
        SelectedAreasToEditForCompanyJob = companyjob?.PositionAreas?
            .Split(',')
            .Select(area => new Area { AreaName = area })
            .ToList() ?? new List<Area>();

        searchStartingDateToFindThesesAsCompany = DateTime.Today;


        if (companythesis != null)
        {
            applicants = await GetApplicantsForCompanyThesis(companythesis.RNGForThesisUploadedAsCompany);
        }
        else
        {
            // Handle the case where companythesis is null (maybe show a message or log an error)
            applicants = Enumerable.Empty<CompanyThesisApplied>(); // Or handle differently
        }

        if (companyinternshipp != null)
        {
            companyInternshipApplicants = await GetApplicants(companyinternshipp.RNGForInternshipUploadedAsCompany);
        }
        else
        {
            // Handle the case where companyinternshipp is null
            companyInternshipApplicants = Enumerable.Empty<InternshipApplied>();

        }

        if (companyjobb != null)
        {
            // Only call GetApplicantsForJobs if companyjobb is not null
            companyJobApplicants = await GetApplicantsForJobs(companyjobb.RNGForPositionUploaded);
        }
        else
        {
            // Handle the case where companyjobb is null (maybe show a message or log an error)
            companyJobApplicants = Enumerable.Empty<CompanyJobApplied>(); // Or handle differently
        }

        if (professorinternship != null)
        {
            // Only call GetApplicantsForJobs if professorinternship is not null
            professorInternshipApplicants = await GetProfessorInternshipApplicants(professorinternship.RNGForInternshipUploadedAsProfessor);
        }
        else
        {
            professorInternshipApplicants = Enumerable.Empty<ProfessorInternshipApplied>(); // Or handle differently
        }

        // Initialize component states for testing
        isStudentInitialized = true;
        isCompanyInitialized = true;
        isProfessorInitialized = true;
        isResearchGroupInitialized = true;

    }

    private async Task<CompanyThesis> GetCompanyThesisAsync()
    {
        // Replace this with your actual logic to fetch the company thesis
        // Ensure this does not return null unless expected
        return await dbContext.CompanyTheses.FirstOrDefaultAsync(); // Example fetch
    }

    private async Task<CompanyInternship> GetCompanyInternshipsAsync()
    {
        // Replace this with your actual logic to fetch the company intern
        // Ensure this does not return null unless expected
        return await dbContext.CompanyInternships.FirstOrDefaultAsync(); // Example fetch
    }

    private async Task<CompanyJob> GetCompanyJobsAsync()
    {
        // Replace this with your actual logic to fetch the company intern
        // Ensure this does not return null unless expected
        return await dbContext.CompanyJobs.FirstOrDefaultAsync(); // Example fetch
        currentJobPage = 1;
    }

    private async Task<List<CompanyThesis>> GetCompanyThesesAsync()
    {
        return await dbContext.CompanyTheses.ToListAsync(); 
    }


    private async Task LoadAreasAsync()
    {
        Areas = await dbContext.Areas.ToListAsync();
    }

    private async Task LoadSkillsAsync()
    {
        Skills = await dbContext.Skills.ToListAsync();
    }



    private void NavigateToSearchJobs()
    {
        NavigationManager.NavigateTo("/searchjobs");
    }

    private void NavigateToSearchThesis()
    {
        NavigationManager.NavigateTo("/searchthesis");
    }


    private bool HideAllInitialCards()
    {
        var uri = NavigationManager.Uri;
        return uri.Contains("profile")
        || uri.Contains("settings")
        || uri.Contains("uploadjobs")
        || uri.Contains("searchjobs")
        || uri.Contains("companyRegistration")
        || uri.Contains("studentRegistration")
        || uri.Contains("uploadJobs")
        || uri.Contains("professorRegistration")
        || uri.Contains("uploadthesis")
        || uri.Contains("searchthesis")
        || uri.Contains("uploadinternship")
        || uri.Contains("researchGroupRegistration");

        //ALLAKSA AYTO GIA TO MAIN LAYOUT NA FIGOUN TA CARDS KAI EPISIS STO APP.RAZOR EVALA 3 FUNCTIONS GIA NA KANEI REDIRECT STO REGISTRAITON ANALOGA TO USER ROLE
        //AYTO PREPEI NA GINETAI MIA FORA STO PRWTO RENDER ALLIWS AN PATAW PANW STO X-TREND NA DEIXNEI TOUS CRAWLERS APO TO LANDING PAGE MAZI ME AYTA POU EIPE O DIONYSIS.
        //EPISIS NA GINEI TO UPDATE GIA TA STOIXEIA TOU USER STIN IDIA FORMA OPWS AKRIVWS GINETAI KAI META EDIT POSITIONS KAI EDIT THESIS
    }


    private async Task ShowMyThesisApplicationsAsStudent()
    {
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var userEmail = user.FindFirst("name")?.Value;
                if (string.IsNullOrEmpty(userEmail))
                {
                    Console.WriteLine("User email is null or empty.");
                    return;
                }

                if (userData == null)
                {
                    Console.WriteLine("userData is null.");
                    return;
                }

                if (string.IsNullOrEmpty(userData.Student_UniqueID))
                {
                    Console.WriteLine("userData.Student_UniqueID is not set.");
                    return;
                }

                showThesisApplications = true;

                // Retrieve thesis applications with includes and proper filtering
                thesisApplications = await dbContext.ProfessorThesesApplied
                    .Include(a => a.StudentDetails)
                    .Include(a => a.ProfessorDetails)
                    .Where(j => j.StudentEmailAppliedForProfessorThesis == userEmail && 
                               j.StudentUniqueIDAppliedForProfessorThesis == userData.Student_UniqueID)
                    .OrderByDescending(j => j.DateTimeStudentAppliedForProfessorThesis)
                    .ToListAsync();

                StateHasChanged();
            }
            else
            {
                Console.WriteLine("User is not authenticated.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private async Task ShowMyJobApplicationsAsStudent()
    {
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var userEmail = user.FindFirst("name")?.Value;
                if (!string.IsNullOrEmpty(userEmail))
                {
                    showJobApplications = true;

                    // Load applications with ALL related data
                    jobApplications = await dbContext.CompanyJobsApplied
                        .Where(j => j.StudentEmailAppliedForCompanyJob == userEmail)
                        .Include(a => a.StudentDetails)
                        .Include(a => a.CompanyDetails)
                        .ToListAsync();

                    // Load ALL related jobs WITH COMPANY DATA in one query
                    var jobIds = jobApplications.Select(a => a.RNGForCompanyJobApplied).Distinct().ToList();
                    var jobs = await dbContext.CompanyJobs
                        .Where(j => jobIds.Contains(j.RNGForPositionUploaded))
                        .Include(j => j.Company)  // Add this line to include Company data
                        .AsNoTracking()
                        .ToListAsync();

                    // Update cache
                    jobDataCache = jobs.ToDictionary(j => j.RNGForPositionUploaded, j => j);

                    StateHasChanged();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading applications: {ex.Message}");
            await JS.InvokeVoidAsync("alert", "Σφάλμα φόρτωσης αιτήσεων");
        }
    }



    private async Task DeleteProfessorThesisApplicationMadeAsStudent(long rngForThesisApplied)
    {
        try
        {
            // Find application by the RNG value
            var applicationToDelete = await dbContext.ProfessorThesesApplied
                .FirstOrDefaultAsync(app => app.RNGForProfessorThesisApplied == rngForThesisApplied);

            if (applicationToDelete != null)
            {
                // First delete related details if they exist
                var studentDetails = await dbContext.ProfessorThesisApplied_StudentDetails
                    .FirstOrDefaultAsync(s => s.StudentUniqueIDAppliedForProfessorThesis == applicationToDelete.StudentUniqueIDAppliedForProfessorThesis 
                                          && s.StudentEmailAppliedForProfessorThesis == applicationToDelete.StudentEmailAppliedForProfessorThesis);

                var professorDetails = await dbContext.ProfessorThesisApplied_ProfessorDetails
                    .FirstOrDefaultAsync(p => p.ProfessorUniqueIDWhereStudentAppliedForProfessorThesis == applicationToDelete.ProfessorUniqueIDWhereStudentAppliedForProfessorThesis
                                           && p.ProfessorEmailWhereStudentAppliedForProfessorThesis == applicationToDelete.ProfessorEmailWhereStudentAppliedForProfessorThesis);

                if (studentDetails != null)
                    dbContext.ProfessorThesisApplied_StudentDetails.Remove(studentDetails);

                if (professorDetails != null)
                    dbContext.ProfessorThesisApplied_ProfessorDetails.Remove(professorDetails);

                // Then delete the main application
                dbContext.ProfessorThesesApplied.Remove(applicationToDelete);
                await dbContext.SaveChangesAsync();

                // Update local list by RNG value
                professorthesisApplications.RemoveAll(t => t.RNGForProfessorThesisApplied == rngForThesisApplied);
            }
            else
            {
                Console.WriteLine($"No professor thesis application found with RNG: {rngForThesisApplied}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting Professor Thesis Application: {ex.Message}");
        }
        finally
        {
            StateHasChanged();
        }
    }



    private async Task DeleteCompanyThesisApplication(long rngForThesisApplied)
    {
        try
        {
            // Find application by the RNG value
            var applicationToDelete = await dbContext.CompanyThesesApplied
                .FirstOrDefaultAsync(app => app.RNGForCompanyThesisApplied == rngForThesisApplied);

            if (applicationToDelete != null)
            {
                // First delete related details if they exist
                var studentDetails = await dbContext.CompanyThesisApplied_StudentDetails
                    .FirstOrDefaultAsync(s => s.StudentUniqueIDAppliedForThesis == applicationToDelete.StudentUniqueIDAppliedForThesis 
                                          && s.StudentEmailAppliedForThesis == applicationToDelete.StudentEmailAppliedForThesis);

                var companyDetails = await dbContext.CompanyThesisApplied_CompanyDetails
                    .FirstOrDefaultAsync(c => c.CompanyUniqueIDWhereStudentAppliedForThesis == applicationToDelete.CompanyUniqueIDWhereStudentAppliedForThesis
                                           && c.CompanyEmailWhereStudentAppliedForThesis == applicationToDelete.CompanyEmailWhereStudentAppliedForThesis);

                if (studentDetails != null)
                    dbContext.CompanyThesisApplied_StudentDetails.Remove(studentDetails);

                if (companyDetails != null)
                    dbContext.CompanyThesisApplied_CompanyDetails.Remove(companyDetails);

                // Then delete the main application
                dbContext.CompanyThesesApplied.Remove(applicationToDelete);
                await dbContext.SaveChangesAsync();

                // Update local list if needed (now matching by RNG value)
                companythesisApplications.RemoveAll(t => t.RNGForCompanyThesisApplied == rngForThesisApplied);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting thesis application: {ex.Message}");
            // Consider adding user notification
        }
    }



    private async Task DeleteJobApplication(long rngForJobApplied)
    {
        try
        {
            // Find application by the RNG value
            var applicationToDelete = await dbContext.CompanyJobsApplied
                .FirstOrDefaultAsync(app => app.RNGForCompanyJobApplied == rngForJobApplied);

            if (applicationToDelete != null)
            {
                // First delete related details if they exist
                var studentDetails = await dbContext.CompanyJobApplied_StudentDetails
                    .FirstOrDefaultAsync(s => s.StudentUniqueIDAppliedForCompanyJob == applicationToDelete.StudentUniqueIDAppliedForCompanyJob 
                                          && s.StudentEmailAppliedForCompanyJob == applicationToDelete.StudentEmailAppliedForCompanyJob);

                var companyDetails = await dbContext.CompanyJobApplied_CompanyDetails
                    .FirstOrDefaultAsync(c => c.CompanysUniqueIDWhereStudentAppliedForCompanyJob == applicationToDelete.CompanysUniqueIDWhereStudentAppliedForCompanyJob
                                           && c.CompanysEmailWhereStudentAppliedForCompanyJob == applicationToDelete.CompanysEmailWhereStudentAppliedForCompanyJob);

                if (studentDetails != null)
                    dbContext.CompanyJobApplied_StudentDetails.Remove(studentDetails);

                if (companyDetails != null)
                    dbContext.CompanyJobApplied_CompanyDetails.Remove(companyDetails);

                // Then delete the main application
                dbContext.CompanyJobsApplied.Remove(applicationToDelete);
                await dbContext.SaveChangesAsync();

                // Update local list if needed (now matching by RNG value)
                jobApplications.RemoveAll(j => j.RNGForCompanyJobApplied == rngForJobApplied);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting job application: {ex.Message}");
            // Consider adding user notification
        }
    }






    public static class DiacriticsRemover
    {
        // Mapping Greek diacritics to their base characters
        private static readonly Dictionary<char, char> DiacriticRemovals = new Dictionary<char, char>
    {
        { 'ά', 'α' }, { 'έ', 'ε' }, { 'ή', 'η' }, { 'ί', 'ι' },
        { 'ϊ', 'ι' }, { 'ΐ', 'ι' }, { 'ό', 'ο' }, { 'ύ', 'υ' },
        { 'ϋ', 'υ' }, { 'ΰ', 'υ' }, { 'ώ', 'ω' }, { 'Ά', 'Α' },
        { 'Έ', 'Ε' }, { 'Ή', 'Η' }, { 'Ί', 'Ι' }, { 'Ϊ', 'Ι' },
        { 'Ό', 'Ο' }, { 'Ύ', 'Υ' }, { 'Ϋ', 'Υ' }, { 'Ώ', 'Ω' }
    };

        public static string RemoveDiacritics(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var normalizedString = new StringBuilder();

            foreach (var character in input)
            {
                if (DiacriticRemovals.TryGetValue(character, out var replacement))
                {
                    normalizedString.Append(replacement);
                }
                else
                {
                    normalizedString.Append(character);
                }
            }

            return normalizedString.ToString();
        }
    }

    //I methodos egine Update gia na tsekarei to department opws ginetai kai ,me ta Professor Internships (student.deparmtnet == professor.department) 
    private async Task SearchThesisApplicationsAsStudent()
    {
        dbContext.ChangeTracker.Clear();

        // Get the current student's department
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        var userEmail = user.Identity.Name;

        var currentStudent = await dbContext.Students
            .FirstOrDefaultAsync(s => s.Email == userEmail);

        var studentDepartment = currentStudent?.Department;

        var professorThesesQuery = dbContext.ProfessorTheses
            .Include(t => t.Professor)  // Load professor data
            .AsQueryable();

        var companyThesesQuery = dbContext.CompanyTheses
            .Include(t => t.Company)
            .AsQueryable();

        // Filter professor theses by department matching - only show theses where professor's department matches student's department
        if (!string.IsNullOrEmpty(studentDepartment))
        {
            professorThesesQuery = professorThesesQuery
                .Where(t => t.Professor != null && 
                           t.Professor.ProfDepartment == studentDepartment);
        }
        // Keep company theses as is - no department filtering applied

        // Filter by Thesis Title with exact match priority
        if (!string.IsNullOrEmpty(thesisSearchForThesesAsStudent))
        {
            // First try exact match using ToLower() for case-insensitive comparison
            var exactMatchProfessor = professorThesesQuery 
                .Where(t => t.ThesisTitle.ToLower() == thesisSearchForThesesAsStudent.ToLower());

            var exactMatchCompany = companyThesesQuery
                .Where(t => t.CompanyThesisTitle.ToLower() == thesisSearchForThesesAsStudent.ToLower());

            // Check if we have any exact matches
            var hasExactMatches = await exactMatchProfessor.AnyAsync() || await exactMatchCompany.AnyAsync();

            if (hasExactMatches)
            {
                professorThesesQuery = exactMatchProfessor;
                companyThesesQuery = exactMatchCompany;
            }
            else
            {
                // Fall back to contains search if no exact matches
                professorThesesQuery = professorThesesQuery
                    .Where(t => EF.Functions.Like(t.ThesisTitle, $"%{thesisSearchForThesesAsStudent}%"));
                companyThesesQuery = companyThesesQuery
                    .Where(t => EF.Functions.Like(t.CompanyThesisTitle, $"%{thesisSearchForThesesAsStudent}%"));
            }
        }

        // Filter by Professor Full Name (autocomplete field) - now using navigation properties
        if (!string.IsNullOrEmpty(searchNameSurnameAsStudentToFindProfessor))
        {
            var searchTerm = searchNameSurnameAsStudentToFindProfessor.Trim();
            var nameParts = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (nameParts.Length == 1)
            {
                professorThesesQuery = professorThesesQuery.Where(t =>
                    t.Professor != null && (
                        EF.Functions.Like(t.Professor.ProfName, $"%{nameParts[0]}%") ||
                        EF.Functions.Like(t.Professor.ProfSurname, $"%{nameParts[0]}%")
                    ));
            }
            else
            {
                professorThesesQuery = professorThesesQuery.Where(t =>
                    t.Professor != null && (
                        (EF.Functions.Like(t.Professor.ProfName, $"%{nameParts[0]}%") && 
                         EF.Functions.Like(t.Professor.ProfSurname, $"%{nameParts[1]}%")) ||
                        (EF.Functions.Like(t.Professor.ProfName, $"%{nameParts[1]}%") && 
                         EF.Functions.Like(t.Professor.ProfSurname, $"%{nameParts[0]}%"))
                    ));
            }
            companyThesesQuery = companyThesesQuery.Where(t => false);
        }
        // Keep existing separate name/surname filters - updated for navigation properties
        else if (!string.IsNullOrEmpty(professorNameSearchForThesesAsStudent))
        {
            professorThesesQuery = professorThesesQuery.Where(t =>
                t.Professor != null &&
                EF.Functions.Like(t.Professor.ProfName.ToLower(), $"%{professorNameSearchForThesesAsStudent.ToLower()}%"));
            companyThesesQuery = companyThesesQuery.Where(t => false);
        }
        else if (!string.IsNullOrEmpty(professorSurnameSearchForThesesAsStudent))
        {
            professorThesesQuery = professorThesesQuery.Where(t => 
                t.Professor != null &&
                EF.Functions.Like(t.Professor.ProfSurname, $"%{professorSurnameSearchForThesesAsStudent}%"));
            companyThesesQuery = companyThesesQuery.Where(t => false);
        }

        // Enhanced Filter by Company Name with exact match priority (unchanged)
        if (!string.IsNullOrEmpty(companyNameSearchForThesesAsStudent))
        {
            // First try exact match using ToLower() for case-insensitive comparison
            var exactMatchCompany = companyThesesQuery
                .Where(t => t.Company != null && 
                           t.Company.CompanyName.ToLower() == companyNameSearchForThesesAsStudent.ToLower());

            // Check if we have any exact matches
            var hasExactMatches = await exactMatchCompany.AnyAsync();

            if (hasExactMatches)
            {
                companyThesesQuery = exactMatchCompany;
            }
            else
            {
                // Fall back to contains search if no exact matches
                companyThesesQuery = companyThesesQuery
                    .Where(t => t.Company != null && 
                               EF.Functions.Like(t.Company.CompanyName, $"%{companyNameSearchForThesesAsStudent}%"));
            }
            professorThesesQuery = professorThesesQuery.Where(t => false);
        }

        // Filter by Thesis Starting Date in both Professor and Company Theses
        if (thesisStartDateForThesesAsStudent.HasValue)
        {
            var startOfDay = thesisStartDateForThesesAsStudent.Value.Date;
            professorThesesQuery = professorThesesQuery.Where(t => t.ThesisActivePeriod.Date >= startOfDay);
            companyThesesQuery = companyThesesQuery.Where(t => t.CompanyThesisStartingDate.Date >= startOfDay);
        }

        if (selectedThesisAreas.Any())
        {
            // For Professor Theses
            var professorPredicate = PredicateBuilder.New<ProfessorThesis>();
            foreach (var area in selectedThesisAreas)
            {
                var tempArea = area;
                professorPredicate = professorPredicate.Or(t => 
                    EF.Functions.Like(t.ThesisAreas, $"%{tempArea}%"));
            }
            professorThesesQuery = professorThesesQuery.Where(professorPredicate);

            // For Company Theses
            var companyPredicate = PredicateBuilder.New<CompanyThesis>();
            foreach (var area in selectedThesisAreas)
            {
                var tempArea = area;
                companyPredicate = companyPredicate.Or(t => 
                    EF.Functions.Like(t.CompanyThesisAreasUpload, $"%{tempArea}%"));
            }
            companyThesesQuery = companyThesesQuery.Where(companyPredicate);
        }

        // Apply dropdown state filters
        if (dropdownState == "Professor")
        {
            companyThesesQuery = companyThesesQuery.Where(t => false);
        }
        else if (dropdownState == "Company")
        {
            professorThesesQuery = professorThesesQuery.Where(t => false);
        }

        // Execute the queries
        var allProfessorTheses = await professorThesesQuery.ToListAsync();
        var allCompanyTheses = await companyThesesQuery.ToListAsync();

        // Process results - updated for navigation properties
        sumUpThesesFromBothCompanyAndProfessor.Clear();

        sumUpThesesFromBothCompanyAndProfessor.AddRange(allProfessorTheses.Select(t => new AllTheses
        {
            ThesisTitle = t.ThesisTitle,
            EmailUsedToUploadThesis = t.ProfessorEmailUsedToUploadThesis,
            ProfessorName = t.Professor?.ProfName ?? "Άγνωστο Όνομα", // Using navigation property with fallback
            ProfessorSurname = t.Professor?.ProfSurname ?? "Άγνωστο Επώνυμο",
            ThesisUploadDateTime = t.ThesisUploadDateTime,
            CompanyNameUploadedThesis = "",
            CompanyThesisAreasUpload = "",
            ProfessorThesisAreasUpload = t.ThesisAreas,
            RNGForProfessorThesisUploaded = t.RNGForThesisUploaded,
            ThesisType = ThesisType.Professor,
            ProfessorThesisStatus = t.ThesisStatus,
            ProfessorDepartment = t.Professor?.ProfDepartment ?? "Άγνωστο Τμήμα",
            RNGForProfessorThesisUploaded_HashedAsUniqueID = t.RNGForThesisUploaded_HashedAsUniqueID,
        }));

        // Only add company theses if not searching by professor name
        if (string.IsNullOrEmpty(searchNameSurnameAsStudentToFindProfessor) && 
            string.IsNullOrEmpty(professorNameSearchForThesesAsStudent) &&
            string.IsNullOrEmpty(professorSurnameSearchForThesesAsStudent))
        {
            sumUpThesesFromBothCompanyAndProfessor.AddRange(allCompanyTheses.Select(t => new AllTheses
            {
                ThesisTitle = t.CompanyThesisTitle,
                EmailUsedToUploadThesis = t.CompanyEmailUsedToUploadThesis,
                ProfessorName = "",
                ProfessorSurname = "",
                ThesisUploadDateTime = t.CompanyThesisUploadDateTime,
                CompanyNameUploadedThesis = t.Company?.CompanyName ?? "Άγνωστη Εταιρεία",
                CompanyThesisAreasUpload = t.CompanyThesisAreasUpload,
                ProfessorThesisAreasUpload = "",
                RNGForCompanyThesisUploaded = t.RNGForThesisUploadedAsCompany,
                ThesisType = ThesisType.Company,
                CompanyThesisStatus = t.CompanyThesisStatus,
                RNGForCompanyThesisUploaded_HashedAsUniqueID = t.RNGForThesisUploadedAsCompany_HashedAsUniqueID,
            }));
        }

        StateHasChanged();
        showThesisApplications = sumUpThesesFromBothCompanyAndProfessor.Any();
        showThesisApplications = true;
        await FilterThesisApplicationsToSearchAsStudent();  

        if (sumUpThesesFromBothCompanyAndProfessor?.Any() == false)
        {
            await Task.Delay(50);
            await JS.InvokeVoidAsync("scrollToElement", "noThesesFoundAlert");
        }
    }


    private async Task SearchJobApplicationsAsStudent()
    {
        try
        {
            dbContext.ChangeTracker.Clear();
            var query = dbContext.CompanyJobs
                .Include(j => j.Company)  // Add this line to include Company data
                .AsQueryable();

            // Filter by selected date (from the day chosen and later)
            if (selectedDateToSearchJob.HasValue)
            {
                var startOfDayForJobs = selectedDateToSearchJob.Value.Date;
                query = query.Where(j => j.PositionActivePeriod >= startOfDayForJobs);
            }

            // Filter by Position Title
            if (!string.IsNullOrEmpty(jobSearch))
            {
                query = query.Where(j => EF.Functions.Like(j.PositionTitle, $"%{jobSearch}%"));
            }

            // Filter by Company Name (now using the navigation property)
            if (!string.IsNullOrEmpty(companyNameSearch))
            {
                query = query.Where(j => j.Company != null && 
                       EF.Functions.Like(j.Company.CompanyName, $"%{companyNameSearch}%"));
            }

            // Rest of your filters remain the same...
            if (!string.IsNullOrEmpty(emailSearch))
            {
                query = query.Where(j => EF.Functions.Like(j.EmailUsedToUploadJobs, $"%{emailSearch}%"));
            }

            if (!string.IsNullOrEmpty(positionTypeSearch))
            {
                query = query.Where(j => EF.Functions.Like(j.PositionType, $"%{positionTypeSearch}%"));
            }

            if (uploadDateSearch != null)
            {
                query = query.Where(j => j.UploadDateTime.Date == uploadDateSearch.Value.Date);
            }

            if (!string.IsNullOrEmpty(jobSearchByRegion))
            {
                query = query.Where(j => j.PositionPerifereiaLocation == jobSearchByRegion);
            }

            if (!string.IsNullOrEmpty(jobSearchByTown))
            {
                query = query.Where(j => j.PositionDimosLocation == jobSearchByTown);
            }

            // Execute the query
            var allJobs = await query.ToListAsync();

            // Apply in-memory filters
            if (selectedPositionAreas.Any())
            {
                allJobs = allJobs.Where(j => j.PositionAreas?
                    .Split(',', StringSplitOptions.TrimEntries)
                    .Any(area => selectedPositionAreas.Contains(area, StringComparer.OrdinalIgnoreCase)) ?? false)
                    .ToList();
            }

            if (companyjobSearchByTransportOffer)
            {
                allJobs = allJobs.Where(j => j.PositionTransportOffer).ToList();
            }

            // Update results
            jobs = allJobs;
            showJobApplications = true; 
            StateHasChanged();

            // Handle empty results
            var publishedJobs = jobs?.Where(i => i.PositionStatus == "Δημοσιευμένη").ToList();
            if (publishedJobs?.Any() == false)
            {
                await Task.Delay(50);
                await JS.InvokeVoidAsync("scrollToElement", "noJobsFoundAlert");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Search error: {ex.Message}");
            await JS.InvokeVoidAsync("alert", "Σφάλμα κατά την αναζήτηση");
        }
    }

    private List<AllInternships> publishedInternships = new();
    private async Task SearchInternshipsAsStudent()
    {
        try
        {
            dbContext.ChangeTracker.Clear();

            // Initialize queries with Company and Professor included
            var companyInternshipsQuery = dbContext.CompanyInternships
                .Include(i => i.Company)
                .Where(i => i.CompanyUploadedInternshipStatus == "Δημοσιευμένη")
                .AsQueryable();

            var professorInternshipsQuery = dbContext.ProfessorInternships
                .Include(i => i.Professor) // Added navigation property
                .Where(i => i.ProfessorUploadedInternshipStatus == "Δημοσιευμένη")
                .AsQueryable();

            // Title filter
            if (!string.IsNullOrEmpty(companyinternshipSearch))
            {
                var searchTerm = $"%{companyinternshipSearch}%";
                companyInternshipsQuery = companyInternshipsQuery
                    .Where(j => EF.Functions.Like(j.CompanyInternshipTitle, searchTerm));
                professorInternshipsQuery = professorInternshipsQuery
                    .Where(j => EF.Functions.Like(j.ProfessorInternshipTitle, searchTerm));
            }

            // Type filter
            if (!string.IsNullOrEmpty(companyinternshipSearchByType))
            {
                companyInternshipsQuery = companyInternshipsQuery
                    .Where(j => j.CompanyInternshipType == companyinternshipSearchByType);
                professorInternshipsQuery = professorInternshipsQuery
                    .Where(j => j.ProfessorInternshipType == companyinternshipSearchByType);
            }

            // ESPA filter
            if (!string.IsNullOrEmpty(companyinternshipSearchByESPA))
            {
                companyInternshipsQuery = companyInternshipsQuery
                    .Where(j => j.CompanyInternshipESPA == companyinternshipSearchByESPA);
                professorInternshipsQuery = professorInternshipsQuery
                    .Where(j => j.ProfessorInternshipESPA == companyinternshipSearchByESPA);
            }

            // Date filters
            if (selectedDateToSearchInternship.HasValue)
            {
                var startOfDay = selectedDateToSearchInternship.Value.Date;
                companyInternshipsQuery = companyInternshipsQuery
                    .Where(j => j.CompanyInternshipActivePeriod >= startOfDay);
                professorInternshipsQuery = professorInternshipsQuery
                    .Where(j => j.ProfessorInternshipActivePeriod >= startOfDay);
            }

            if (finishEstimationDateToSearchInternship.HasValue)
            {
                var endOfDay = finishEstimationDateToSearchInternship.Value.Date;
                companyInternshipsQuery = companyInternshipsQuery
                    .Where(j => j.CompanyInternshipFinishEstimation <= endOfDay);
                professorInternshipsQuery = professorInternshipsQuery
                    .Where(j => j.ProfessorInternshipFinishEstimation <= endOfDay);
            }

            // Transport filter
            if (companyinternshipSearchByTransportOffer)
            {
                companyInternshipsQuery = companyInternshipsQuery
                    .Where(j => j.CompanyInternshipTransportOffer);
                professorInternshipsQuery = professorInternshipsQuery
                    .Where(j => j.ProfessorInternshipTransportOffer);
            }

            // Location filters
            if (!string.IsNullOrEmpty(companyinternshipSearchByTown))
            {
                companyInternshipsQuery = companyInternshipsQuery
                    .Where(j => j.CompanyInternshipDimosLocation == companyinternshipSearchByTown);
                professorInternshipsQuery = professorInternshipsQuery
                    .Where(j => j.ProfessorInternshipDimosLocation == companyinternshipSearchByTown);
            }
            else if (!string.IsNullOrEmpty(companyinternshipSearchByRegion))
            {
                companyInternshipsQuery = companyInternshipsQuery
                    .Where(j => j.CompanyInternshipPerifereiaLocation == companyinternshipSearchByRegion);
                professorInternshipsQuery = professorInternshipsQuery
                    .Where(j => j.ProfessorInternshipPerifereiaLocation == companyinternshipSearchByRegion);
            }

            var allCompanyInternships = await companyInternshipsQuery.ToListAsync();
            var allProfessorInternships = await professorInternshipsQuery.ToListAsync();

            // Area filter
            if (selectedAreas.Any())
            {
                allCompanyInternships = allCompanyInternships
                    .Where(j => !string.IsNullOrEmpty(j.CompanyInternshipAreas) &&
                        j.CompanyInternshipAreas.Split(',', StringSplitOptions.TrimEntries)
                        .Any(area => selectedAreas.Contains(area, StringComparer.OrdinalIgnoreCase)))
                    .ToList();

                allProfessorInternships = allProfessorInternships
                    .Where(j => !string.IsNullOrEmpty(j.ProfessorInternshipAreas) &&
                        j.ProfessorInternshipAreas.Split(',', StringSplitOptions.TrimEntries)
                        .Any(area => selectedAreas.Contains(area, StringComparer.OrdinalIgnoreCase)))
                    .ToList();
            }

            sumUpInternshipsFromBothCompanyAndProfessor.Clear();

            // Company Internship mapping (unchanged)
            sumUpInternshipsFromBothCompanyAndProfessor.AddRange(allCompanyInternships.Select(j => new AllInternships
            {
                InternshipTitle = j.CompanyInternshipTitle,
                CompanyName = j.Company?.CompanyName,
                CompanyInternshipUploadDate = j.CompanyInternshipUploadDate,
                InternshipStatus = j.CompanyUploadedInternshipStatus,
                InternshipAreas = j.CompanyInternshipAreas,
                InternshipType = "Company",
                InternshipActivePeriod = j.CompanyInternshipActivePeriod,
                InternshipFinishEstimation = j.CompanyInternshipFinishEstimation,
                RNGForCompanyInternship = j.RNGForInternshipUploadedAsCompany,
                CompanyEmail = j.CompanyEmailUsedToUploadInternship,
                InternshipFundingType = j.CompanyInternshipESPA,
                RNGForCompanyInternship_HashedAsUniqueID = j.RNGForInternshipUploadedAsCompany_HashedAsUniqueID
            }));

            // Updated Professor Internship mapping
            sumUpInternshipsFromBothCompanyAndProfessor.AddRange(allProfessorInternships.Select(j => new AllInternships
            {
                InternshipTitle = j.ProfessorInternshipTitle,
                ProfessorName = j.Professor?.ProfName, // From navigation property
                ProfessorSurname = j.Professor?.ProfSurname, // From navigation property
                ProfessorEmail = j.ProfessorEmailUsedToUploadInternship, // Updated property
                ProfessorInternshipUploadDate = j.ProfessorInternshipUploadDate,
                InternshipStatus = j.ProfessorUploadedInternshipStatus,
                InternshipAreas = j.ProfessorInternshipAreas,
                InternshipType = "Professor",
                InternshipActivePeriod = j.ProfessorInternshipActivePeriod,
                InternshipFinishEstimation = j.ProfessorInternshipFinishEstimation,
                RNGForProfessorInternship = j.RNGForInternshipUploadedAsProfessor, // Updated property
                InternshipFundingType = j.ProfessorInternshipESPA,
                RNGForProfessorInternship_HashedAsUniqueID = j.RNGForInternshipUploadedAsProfessor_HashedAsUniqueID, // Updated
                ProfessorDepartment = j.Professor?.ProfDepartment // From navigation property
            }));

            // Filter by department for professor internships only
            publishedInternships = sumUpInternshipsFromBothCompanyAndProfessor
                .Where(internship => 
                    (internship.InternshipType == "Professor" && 
                     internship.ProfessorDepartment == userData.Department) ||
                    internship.InternshipType == "Company")
                .ToList();

            showInternships = publishedInternships.Any();

            if (!publishedInternships.Any())
            {
                await Task.Delay(50); 
                await JS.InvokeVoidAsync("scrollToElement", "noInternshipsFoundAlert");
            }

            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching internships: {ex.Message}");
            await JS.InvokeVoidAsync("alert", "An error occurred while searching internships");
        }
    }

    private void NavigateToUploadThesis()
    {
        NavigationManager.NavigateTo("/uploadthesis"); 
    }
    private void NavigateToUploadInternship()
    {
        NavigationManager.NavigateTo("/uploadinternship"); 
    }
    private void NavigateToUploadJobs()
    {
        NavigationManager.NavigateTo("/uploadjobs"); 
    }


    void ClearSearchFieldsForInternshipsAsStudent()
    {
        companyinternshipSearch = string.Empty;                
        companyinternshipSearchByType = string.Empty;           
        companyinternshipSearchByRegion = string.Empty;        
        companyinternshipSearchByTransportOffer = false;  
        companyinternshipSearchByESPA = string.Empty;   
        selectedAreas.Clear(); 
        isInternshipAreasVisible = false;
        selectedMonth = null;                                   
        companyinternships = new List<CompanyInternship>();     
        showInternships = false;              
        StateHasChanged();                                      
    }




    void ClearSearchFieldsForJobApplicationsAsStudent()
    {
        jobSearch = string.Empty;
        companyNameSearch = string.Empty;
        emailSearch = string.Empty;
        positionTypeSearch = string.Empty;
        uploadDateSearch = null;
        jobSearchByRegion = string.Empty; 
        jobSearchByTown = string.Empty;
        companyjobSearchByTransportOffer = false; 
        selectedPositionAreas.Clear();
        isPositionAreasVisible = false; 
        companyJobs = new List<CompanyJob>(); 
        showJobApplications = false;
        StateHasChanged();
    }


    void ClearSearchFieldsForThesisAsStudent()
    {
        thesisSearchForThesesAsStudent = string.Empty;
        professorNameSearchForThesesAsStudent = string.Empty;
        professorSurnameSearchForThesesAsStudent = string.Empty;
        companyNameSearchForThesesAsStudent = string.Empty;
        thesisUploadMonthForThesesAsStudent = null;
        thesisStartDateForThesesAsStudent = DateTime.Now;

        // Clear the autocomplete fields and suggestions
        searchNameSurnameAsStudentToFindProfessor = string.Empty;
        professorNameSurnameSuggestions.Clear();
        thesisTitleSuggestions.Clear();

        // Clear area filters
        selectedThesisAreas.Clear();  
        isThesisAreasVisible = false;  

        // Reset results
        sumUpThesesFromBothCompanyAndProfessor.Clear();
        showThesisApplications = false; 

        StateHasChanged();
    }


    private async Task LoadUserThesisApplications()
    {
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            // Initialize lists and cache
            companyThesisApplications = new List<CompanyThesisApplied>();
            professorThesisApplications = new List<ProfessorThesisApplied>();
            thesisDataCache = new Dictionary<long, CompanyThesis>();
            professorThesisDataCache = new Dictionary<long, ProfessorThesis>(); 

            if (user.Identity.IsAuthenticated)
            {
                var userEmail = user.FindFirst("name")?.Value;

                if (!string.IsNullOrEmpty(userEmail))
                {
                    showStudentThesisApplications = true;

                    // Get student details
                    var student = await dbContext.Students
                        .FirstOrDefaultAsync(s => s.Email == userEmail);

                    if (student != null)
                    {
                        // Fetch company thesis applications
                        companyThesisApplications = await dbContext.CompanyThesesApplied
                            .Include(a => a.StudentDetails)
                            .Include(a => a.CompanyDetails)
                            .Where(app => app.StudentEmailAppliedForThesis == userEmail && 
                                        app.StudentUniqueIDAppliedForThesis == student.Student_UniqueID)
                            .OrderByDescending(app => app.DateTimeStudentAppliedForThesis)
                            .ToListAsync();

                        // Load all related theses in one query
                        var thesisRNGs = companyThesisApplications
                            .Select(a => a.RNGForCompanyThesisApplied)
                            .ToList();

                        var theses = await dbContext.CompanyTheses
                            .Include(t => t.Company) 
                            .Where(t => thesisRNGs.Contains(t.RNGForThesisUploadedAsCompany))
                            .ToListAsync();

                        // Populate cache
                        foreach (var thesis in theses)
                        {
                            thesisDataCache[thesis.RNGForThesisUploadedAsCompany] = thesis;
                        }

                        // Fetch professor thesis applications (updated to match company pattern)
                        professorThesisApplications = await dbContext.ProfessorThesesApplied
                            .Include(a => a.StudentDetails)
                            .Include(a => a.ProfessorDetails)
                            .Where(app => app.StudentEmailAppliedForProfessorThesis == userEmail && 
                                          app.StudentUniqueIDAppliedForProfessorThesis == student.Student_UniqueID)
                            .OrderByDescending(app => app.DateTimeStudentAppliedForProfessorThesis)
                            .ToListAsync();

                        // Load all related professor theses in one query
                        var professorThesisRNGs = professorThesisApplications
                            .Select(a => a.RNGForProfessorThesisApplied)
                            .ToList();

                        var professorTheses = await dbContext.ProfessorTheses
                            .Where(t => professorThesisRNGs.Contains(t.RNGForThesisUploaded))
                            .ToListAsync();

                        // Populate professor thesis cache if needed
                        foreach (var thesis in professorTheses)
                        {
                            professorThesisDataCache[thesis.RNGForThesisUploaded] = thesis;
                        }

                    }
                }
            }

            Console.WriteLine($"Company Thesis Applications: {companyThesisApplications.Count()}");
            Console.WriteLine($"Professor Thesis Applications: {professorThesisApplications.Count()}");
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading thesis applications: {ex.Message}");
            StateHasChanged();
        }
    }

    private async Task LoadUserJobApplications()
    {
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var userEmail = user.FindFirst("name")?.Value; // Get user's email from claim
                if (!string.IsNullOrEmpty(userEmail))
                {
                    showStudentJobApplications = true; // Set flag to indicate applications are loading

                    // Retrieve job applications using email and unique ID
                    jobApplications = await dbContext.CompanyJobsApplied
                        .Where(j => j.StudentEmailAppliedForCompanyJob == userEmail && 
                                   j.StudentUniqueIDAppliedForCompanyJob == userData.Student_UniqueID)
                        .OrderByDescending(j => j.DateTimeStudentAppliedForCompanyJob) // Added sorting
                        .ToListAsync();

                    StateHasChanged(); // Notify the UI to update
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading job applications: {ex.Message}");
            StateHasChanged();
        }
    }

    private async Task ToggleAndLoadStudentThesisApplications()
    {
        showStudentThesisApplications = !showStudentThesisApplications;

        if (showStudentThesisApplications)
        {
            await LoadUserThesisApplications();
        }
        StateHasChanged();
    }

    private async Task ToggleAndLoadStudentJobApplications()
    {
        showStudentJobApplications = !showStudentJobApplications;

        if (showStudentJobApplications)
        {
            await LoadUserJobApplications();
        }
    }

    private async Task ApplyForThesisAsStudent(AllTheses thesis)
    {
        // First ask for confirmation
        var confirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", 
            $"Πρόκεται να κάνετε Αίτηση για την Πτυχιακή Εργασία: {thesis.ThesisTitle}. Είστε σίγουρος/η;");
        if (!confirmed) return;

        // Status check remains the same for both types
        if (thesis.ThesisType == ThesisType.Professor)
        {
            var latestThesis = await dbContext.ProfessorTheses
                .AsNoTracking()
                .Where(t => t.RNGForThesisUploaded == thesis.RNGForProfessorThesisUploaded)
                .Select(t => new { t.ThesisStatus })
                .FirstOrDefaultAsync();

            if (latestThesis == null || latestThesis.ThesisStatus != "Δημοσιευμένη")
            {
                await JS.InvokeVoidAsync("confirmActionWithHTML2", "Η Πτυχιακή Εργασία του Καθηγητή Έχει Αποδημοσιευτεί. Παρακαλώ δοκιμάστε αργότερα");
                return;
            }
        }
        else if (thesis.ThesisType == ThesisType.Company)
        {
            var latestThesis = await dbContext.CompanyTheses
                .AsNoTracking()
                .Where(t => t.RNGForThesisUploadedAsCompany == thesis.RNGForCompanyThesisUploaded)
                .Select(t => new { t.CompanyThesisStatus })
                .FirstOrDefaultAsync();

            if (latestThesis == null || latestThesis.CompanyThesisStatus != "Δημοσιευμένη")
            {
                await JS.InvokeVoidAsync("confirmActionWithHTML2", "Η Πτυχιακή Εργασία της Εταιρείας Έχει Αποδημοσιευτεί. Παρακαλώ δοκιμάστε αργότερα");
                return;
            }
        }

        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (!user.Identity.IsAuthenticated) return;

        var student = await GetStudentDetails(user.Identity.Name);
        if (student == null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Δεν βρέθηκαν στοιχεία φοιτητή.");
            return;
        }

        if (thesis.ThesisType == ThesisType.Professor)
        {
            Console.WriteLine("Detected thesis as Professor.");

            // Check for existing application using email and RNG (like company version)
            var existingApplication = await dbContext.ProfessorThesesApplied
                .FirstOrDefaultAsync(app => 
                    app.StudentEmailAppliedForProfessorThesis == student.Email &&
                    app.RNGForProfessorThesisApplied == thesis.RNGForProfessorThesisUploaded);

            if (existingApplication != null)
            {
                await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    $"Έχετε ήδη κάνει Αίτηση για την Πτυχιακή Εργασία <span style='color: #8B0000; font-weight: bold;'>{thesis.ThesisTitle}</span> από τον Καθηγητή: <span style='color: #00008B; font-weight: bold;'>{thesis.ProfessorName} {thesis.ProfessorSurname}</span>!");
                return;
            }

            // Get professor data (like company version)
            var professor = await dbContext.Professors
                .FirstOrDefaultAsync(p => p.ProfEmail == thesis.EmailUsedToUploadThesis);

            if (professor == null)
            {
                await JS.InvokeVoidAsync("confirmActionWithHTML2", "Σφάλμα: Δεν βρέθηκε ο καθηγητής.");
                return;
            }

            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                // Create main application with details (like company version)
                var professorThesisApplication = new ProfessorThesisApplied
                {
                    RNGForProfessorThesisApplied = thesis.RNGForProfessorThesisUploaded,
                    DateTimeStudentAppliedForProfessorThesis = DateTime.UtcNow,
                    StudentEmailAppliedForProfessorThesis = student.Email,
                    StudentUniqueIDAppliedForProfessorThesis = student.Student_UniqueID,
                    ProfessorEmailWhereStudentAppliedForProfessorThesis = thesis.EmailUsedToUploadThesis,
                    ProfessorUniqueIDWhereStudentAppliedForProfessorThesis = professor.Professor_UniqueID,
                    ProfessorThesisStatusAppliedAtProfessorSide = "Σε Επεξεργασία",
                    ProfessorThesisStatusAppliedAtStudentSide = "Σε Επεξεργασία",
                    RNGForProfessorThesisApplied_HashedAsUniqueID = thesis.RNGForProfessorThesisUploaded_HashedAsUniqueID,

                    // Navigation properties (like company version)
                    StudentDetails = new ProfessorThesisApplied_StudentDetails
                    {
                        StudentEmailAppliedForProfessorThesis = student.Email,
                        StudentUniqueIDAppliedForProfessorThesis = student.Student_UniqueID,
                        DateTimeStudentAppliedForProfessorThesis = DateTime.UtcNow,
                        RNGForProfessorThesisApplied_HashedAsUniqueID = thesis.RNGForProfessorThesisUploaded_HashedAsUniqueID
                    },

                    ProfessorDetails = new ProfessorThesisApplied_ProfessorDetails
                    {
                        ProfessorEmailWhereStudentAppliedForProfessorThesis = thesis.EmailUsedToUploadThesis,
                        ProfessorUniqueIDWhereStudentAppliedForProfessorThesis = professor.Professor_UniqueID
                    }
                };

                dbContext.ProfessorThesesApplied.Add(professorThesisApplication);

                // Add platform action (like company version)
                dbContext.PlatformActions.Add(new PlatformActions
                {
                    UserRole_PerformedAction = "STUDENT",
                    ForWhat_PerformedAction = "PROFESSOR_THESIS",
                    HashedPositionRNG_PerformedAction = HashingHelper.HashLong(thesis.RNGForProfessorThesisUploaded),
                    TypeOfAction_PerformedAction = "APPLY",
                    DateTime_PerformedAction = DateTime.UtcNow
                });

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                // Send emails (existing logic remains)
                try
                {
                    await InternshipEmailService.SendThesisApplicationConfirmationToStudent(
                        student.Email,
                        student.Name,
                        student.Surname,
                        thesis.ThesisTitle,
                        thesis.RNGForProfessorThesisUploaded_HashedAsUniqueID,
                        $"{thesis.ProfessorName} {thesis.ProfessorSurname}");

                    await InternshipEmailService.SendThesisApplicationNotificationToProfessor(
                        thesis.EmailUsedToUploadThesis,
                        $"{thesis.ProfessorName} {thesis.ProfessorSurname}",
                        student.Name,
                        student.Surname,
                        student.Email,
                        student.Telephone,
                        student.StudyYear,
                        thesis.RNGForProfessorThesisUploaded_HashedAsUniqueID,
                        student.Attachment,
                        thesis.ThesisTitle);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Email error: {ex.Message}");
                }

                await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    $"Η αίτηση για την Πτυχιακή Εργασία {thesis.ThesisTitle} υποβλήθηκε επιτυχώς!");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Full error: {ex.ToString()}");
                await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    $"Σφάλμα κατά την υποβολή: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
        else if (thesis.ThesisType == ThesisType.Company)
        {
            Console.WriteLine("Detected thesis as Company.");

            // Check for existing application using email and RNG
            var existingApplication = await dbContext.CompanyThesesApplied
                .FirstOrDefaultAsync(app => 
                    app.StudentEmailAppliedForThesis == student.Email &&
                    app.RNGForCompanyThesisApplied == thesis.RNGForCompanyThesisUploaded);

            if (existingApplication != null)
            {
                await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    $"Έχετε ήδη κάνει Αίτηση για την Πτυχιακή Εργασία <span style='color: #8B0000; font-weight: bold;'>{thesis.ThesisTitle}</span> από την Εταιρεία: <span style='color: #00008B; font-weight: bold;'>{thesis.CompanyNameUploadedThesis}</span>!");
                return;
            }

            // Get company data
            var company = await dbContext.Companies
                .FirstOrDefaultAsync(c => c.CompanyEmail == thesis.EmailUsedToUploadThesis);

            if (company == null)
            {
                await JS.InvokeVoidAsync("confirmActionWithHTML2", "Σφάλμα: Δεν βρέθηκε η εταιρία.");
                return;
            }

            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                // Create main application with details
                var companyThesisApplication = new CompanyThesisApplied
                {
                    RNGForCompanyThesisApplied = thesis.RNGForCompanyThesisUploaded,
                    DateTimeStudentAppliedForThesis = DateTime.UtcNow,
                    StudentEmailAppliedForThesis = student.Email,
                    StudentUniqueIDAppliedForThesis = student.Student_UniqueID,
                    CompanyEmailWhereStudentAppliedForThesis = thesis.EmailUsedToUploadThesis,
                    CompanyUniqueIDWhereStudentAppliedForThesis = company.Company_UniqueID,
                    CompanyThesisStatusAppliedAtCompanySide = "Σε Επεξεργασία",
                    CompanyThesisStatusAppliedAtStudentSide = "Σε Επεξεργασία",
                    RNGForCompanyThesisAppliedAsStudent_HashedAsUniqueID = thesis.RNGForCompanyThesisUploaded_HashedAsUniqueID,

                    // Navigation properties
                    StudentDetails = new CompanyThesisApplied_StudentDetails
                    {
                        StudentEmailAppliedForThesis = student.Email,
                        StudentUniqueIDAppliedForThesis = student.Student_UniqueID,
                        DateTimeStudentAppliedForThesis = DateTime.UtcNow,
                        RNGForCompanyThesisAppliedAsStudent_HashedAsUniqueID = thesis.RNGForCompanyThesisUploaded_HashedAsUniqueID
                    },

                    CompanyDetails = new CompanyThesisApplied_CompanyDetails
                    {
                        CompanyEmailWhereStudentAppliedForThesis = thesis.EmailUsedToUploadThesis,
                        CompanyUniqueIDWhereStudentAppliedForThesis = company.Company_UniqueID
                    }
                };

                dbContext.CompanyThesesApplied.Add(companyThesisApplication);

                // Add platform action
                dbContext.PlatformActions.Add(new PlatformActions
                {
                    UserRole_PerformedAction = "STUDENT",
                    ForWhat_PerformedAction = "COMPANY_THESIS",
                    HashedPositionRNG_PerformedAction = HashingHelper.HashLong(thesis.RNGForCompanyThesisUploaded),
                    TypeOfAction_PerformedAction = "APPLY",
                    DateTime_PerformedAction = DateTime.UtcNow
                });

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                // Send emails
                try
                {
                    await InternshipEmailService.SendThesisApplicationConfirmationToStudent(
                        student.Email,
                        student.Name,
                        student.Surname,
                        thesis.ThesisTitle,
                        thesis.RNGForCompanyThesisUploaded_HashedAsUniqueID,
                        thesis.CompanyNameUploadedThesis);

                    await InternshipEmailService.SendThesisApplicationNotificationToCompany(
                        thesis.EmailUsedToUploadThesis,
                        thesis.CompanyNameUploadedThesis,
                        student.Name,
                        student.Surname,
                        student.Email,
                        student.Telephone,
                        student.StudyYear,
                        thesis.RNGForCompanyThesisUploaded_HashedAsUniqueID,
                        student.Attachment,
                        thesis.ThesisTitle);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Email error: {ex.Message}");
                }

                await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    $"Η αίτηση για την Πτυχιακή Εργασία {thesis.ThesisTitle} υποβλήθηκε επιτυχώς!");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Full error: {ex.ToString()}");
                await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    $"Σφάλμα κατά την υποβολή: {ex.InnerException?.Message ?? ex.Message}");
            }

            await RefreshStudentData();
        }
        else
        {
            await JS.InvokeVoidAsync("alert", "Δεν είναι γνωστός ο τύπος της Πτυχιακής Εργασίας.");
        }

        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }




    private async Task SaveChangesWithErrorHandling(DbContext context, string thesisTitle)
    {
        try
        {
            await context.SaveChangesAsync();
            await JS.InvokeVoidAsync("confirmActionWithHTML2", $"Η Αίτηση σας για την Πτυχιακή Εργασία: {thesisTitle} έχει πραγματοποιηθεί Επιτυχώς!");
        }
        catch (DbUpdateException ex)
        {
            var innerException = ex.InnerException?.Message ?? ex.Message;
            Console.WriteLine($"Error saving thesis application: {innerException}");
            await JS.InvokeVoidAsync("confirmActionWithHTML2", $"Σφάλμα κατά την υποβολή της αίτησης. Δοκιμάστε ξανά. Λεπτομέρειες: {innerException}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving thesis application: {ex.Message}");
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Σφάλμα κατά την υποβολή της αίτησης. Δοκιμάστε ξανά.");
        }
    }

    private async Task LoadCompanyJobApplicantData()
    {
        try 
        {
            // Get all unique student emails from applications
            var studentEmails = jobApplicantsMap.Values
                .SelectMany(x => x)
                .Select(a => a.StudentEmailAppliedForCompanyJob)
                .Distinct()
                .ToList();

            // Load ALL student fields needed for the modal
            var students = await dbContext.Students
                .Where(s => studentEmails.Contains(s.Email.ToLower()))
                .Select(s => new Student {
                    Id = s.Id,
                    Student_UniqueID = s.Student_UniqueID,
                    Email = s.Email,
                    Image = s.Image,
                    Name = s.Name,
                    Surname = s.Surname,
                    Telephone = s.Telephone,
                    PermanentAddress = s.PermanentAddress,
                    PermanentRegion = s.PermanentRegion,
                    PermanentTown = s.PermanentTown,
                    Attachment = s.Attachment,
                    LinkedInProfile = s.LinkedInProfile,
                    PersonalWebsite = s.PersonalWebsite,
                    Transport = s.Transport,
                    RegNumber = s.RegNumber,
                    University = s.University,
                    Department = s.Department,
                    EnrollmentDate = s.EnrollmentDate,
                    StudyYear = s.StudyYear,
                    LevelOfDegree = s.LevelOfDegree,
                    AreasOfExpertise = s.AreasOfExpertise,
                    Keywords = s.Keywords,
                    ExpectedGraduationDate = s.ExpectedGraduationDate,
                    CompletedECTS = s.CompletedECTS
                })
                .AsNoTracking()
                .ToListAsync();

            // Clear and repopulate cache
            //studentDataCache.Clear();
            foreach (var student in students)
            {
                studentDataCache[student.Email.ToLower()] = student;
            }

            Console.WriteLine($"Loaded {students.Count} student records with full details");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading student data: {ex.Message}");
        }
    }

    private async Task LoadCompanyInternshipApplicantData()
    {
        try 
        {
            var studentEmails = internshipApplicantsMap.Values
                .SelectMany(x => x)
                .Select(a => a.StudentEmailAppliedForInternship)
                .Distinct()
                .ToList();

            var students = await dbContext.Students
                .Where(s => studentEmails.Contains(s.Email.ToLower()))
                .Select(s => new Student {
                    Id = s.Id,
                    Student_UniqueID = s.Student_UniqueID,
                    Email = s.Email,
                    Image = s.Image,
                    Name = s.Name,
                    Surname = s.Surname,
                    Telephone = s.Telephone,
                    PermanentAddress = s.PermanentAddress,
                    PermanentRegion = s.PermanentRegion,
                    PermanentTown = s.PermanentTown,
                    Attachment = s.Attachment,
                    LinkedInProfile = s.LinkedInProfile,
                    PersonalWebsite = s.PersonalWebsite,
                    Transport = s.Transport,
                    RegNumber = s.RegNumber,
                    University = s.University,
                    Department = s.Department,
                    EnrollmentDate = s.EnrollmentDate,
                    StudyYear = s.StudyYear,
                    LevelOfDegree = s.LevelOfDegree,
                    AreasOfExpertise = s.AreasOfExpertise,
                    Keywords = s.Keywords,
                    ExpectedGraduationDate = s.ExpectedGraduationDate,
                    CompletedECTS = s.CompletedECTS
                })
                .AsNoTracking()
                .ToListAsync();

            //studentDataCache.Clear(); // Optional: keep if job & internship data shouldn't mix
            foreach (var student in students)
            {
                studentDataCache[student.Email.ToLower()] = student;
            }

            Console.WriteLine($"Loaded {students.Count} internship student records");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading internship student data: {ex.Message}");
        }
    }

    private async Task LoadProfessorInternshipApplicantData()
    {
        try 
        {
            var studentEmails = professorInternshipApplicantsMap.Values
                .SelectMany(x => x)
                .Select(a => a.StudentDetails.StudentEmailAppliedForProfessorInternship)
                .Distinct()
                .ToList();

            var students = await dbContext.Students
                .Where(s => studentEmails.Contains(s.Email.ToLower()))
                .Select(s => new Student {
                    Id = s.Id,
                    Student_UniqueID = s.Student_UniqueID,
                    Email = s.Email,
                    Image = s.Image,
                    Name = s.Name,
                    Surname = s.Surname,
                    Telephone = s.Telephone,
                    PermanentAddress = s.PermanentAddress,
                    PermanentRegion = s.PermanentRegion,
                    PermanentTown = s.PermanentTown,
                    Attachment = s.Attachment,
                    LinkedInProfile = s.LinkedInProfile,
                    PersonalWebsite = s.PersonalWebsite,
                    Transport = s.Transport,
                    RegNumber = s.RegNumber,
                    University = s.University,
                    Department = s.Department,
                    EnrollmentDate = s.EnrollmentDate,
                    StudyYear = s.StudyYear,
                    LevelOfDegree = s.LevelOfDegree,
                    AreasOfExpertise = s.AreasOfExpertise,
                    Keywords = s.Keywords,
                    ExpectedGraduationDate = s.ExpectedGraduationDate,
                    CompletedECTS = s.CompletedECTS
                })
                .AsNoTracking()
                .ToListAsync();

            //studentDataCache.Clear(); // Optional: keep if job & internship data shouldn't mix
            foreach (var student in students)
            {
                studentDataCache[student.Email.ToLower()] = student;
            }

            Console.WriteLine($"Loaded {students.Count} professor internship student records");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading professor internship student data: {ex.Message}");
        }
        StateHasChanged();
    }

    private async Task LoadCompanyThesisApplicantData()
    {
        try 
        {
            // Get all unique student emails from applications
            var studentEmails = companyThesisApplicantsMap.Values
                .SelectMany(x => x)
                .Select(a => a.StudentEmailAppliedForThesis)
                .Distinct()
                .ToList();

            // Load ALL student fields needed for the modal
            var students = await dbContext.Students
                .Where(s => studentEmails.Contains(s.Email.ToLower()))
                .Select(s => new Student {
                    Id = s.Id,
                    Student_UniqueID = s.Student_UniqueID,
                    Email = s.Email,
                    Image = s.Image,
                    Name = s.Name,
                    Surname = s.Surname,
                    Telephone = s.Telephone,
                    PermanentAddress = s.PermanentAddress,
                    PermanentRegion = s.PermanentRegion,
                    PermanentTown = s.PermanentTown,
                    Attachment = s.Attachment,
                    LinkedInProfile = s.LinkedInProfile,
                    PersonalWebsite = s.PersonalWebsite,
                    Transport = s.Transport,
                    RegNumber = s.RegNumber,
                    University = s.University,
                    Department = s.Department,
                    EnrollmentDate = s.EnrollmentDate,
                    StudyYear = s.StudyYear,
                    LevelOfDegree = s.LevelOfDegree,
                    AreasOfExpertise = s.AreasOfExpertise,
                    Keywords = s.Keywords,
                    ExpectedGraduationDate = s.ExpectedGraduationDate,
                    CompletedECTS = s.CompletedECTS
                })
                .AsNoTracking()
                .ToListAsync();

            // Clear and repopulate cache
            //studentDataCache.Clear();
            foreach (var student in students)
            {
                studentDataCache[student.Email.ToLower()] = student;
            }

            Console.WriteLine($"Loaded {students.Count} student records for thesis applicants");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading thesis applicant student data: {ex.Message}");
        }
    }



    private async Task<Student> GetStudentDetails(string email)
    {
        return await dbContext.Students
            .Where(s => s.Email.ToLower() == email.ToLower()) 
            .Select(s => new Student {
                Id = s.Id,
                Email = s.Email,
                Name = s.Name,
                Surname = s.Surname,
                StudyYear = s.StudyYear,
                Telephone = s.Telephone,
                Student_UniqueID = s.Student_UniqueID,
                RegNumber = s.RegNumber,
                Attachment = s.Attachment
            })
            .AsNoTracking() // Better performance for read-only
            .FirstOrDefaultAsync();
    }

    private async Task<Company> GetCompanyDetails(string email)
    {
        return await dbContext.Companies
            .Where(c => c.CompanyEmail.ToLower() == email.ToLower())
            .Select(c => new Company {
                CompanyEmail = c.CompanyEmail,
                Company_UniqueID = c.Company_UniqueID,
                CompanyLogo = c.CompanyLogo,
                CompanyName = c.CompanyName,
                CompanyShortName = c.CompanyShortName,
                CompanyType = c.CompanyType,
                CompanyActivity = c.CompanyActivity,
                CompanyTelephone = c.CompanyTelephone,
                CompanyWebsite = c.CompanyWebsite,
                CompanyCountry = c.CompanyCountry,
                CompanyLocation = c.CompanyLocation,
                CompanyPC = c.CompanyPC,
                CompanyRegions = c.CompanyRegions,
                CompanyTown = c.CompanyTown,
                CompanyDescription = c.CompanyDescription,
                CompanyAreas = c.CompanyAreas,
                CompanyHRName = c.CompanyHRName,
                CompanyHRSurname = c.CompanyHRSurname,
                CompanyHREmail = c.CompanyHREmail,
                CompanyHRTelephone = c.CompanyHRTelephone,
                CompanyAdminName = c.CompanyAdminName,
                CompanyAdminSurname = c.CompanyAdminSurname,
                CompanyAdminEmail = c.CompanyAdminEmail,
                CompanyAdminTelephone = c.CompanyAdminTelephone
            })
            .AsNoTracking() // Better performance for read-only
            .FirstOrDefaultAsync();
    }

    private async Task<Professor> GetProfessorDetails(string email)
    {
        return await dbContext.Professors
            .Where(p => p.ProfEmail.ToLower() == email.ToLower())
            .Select(p => new Professor {
                Id = p.Id,
                ProfEmail = p.ProfEmail,
                Professor_UniqueID = p.Professor_UniqueID,
                ProfName = p.ProfName,
                ProfSurname = p.ProfSurname,
                ProfUniversity = p.ProfUniversity,
                ProfDepartment = p.ProfDepartment,
                ProfWorkTelephone = p.ProfWorkTelephone,
                ProfPersonalTelephone = p.ProfPersonalTelephone,
                ProfPersonalTelephoneVisibility = p.ProfPersonalTelephoneVisibility,
                ProfPersonalWebsite = p.ProfPersonalWebsite,
                ProfGeneralFieldOfWork = p.ProfGeneralFieldOfWork,
                ProfGeneralSkills = p.ProfGeneralSkills
            })
            .AsNoTracking() // Better performance for read-only
            .FirstOrDefaultAsync();
    }

    private async Task ApplyForJobAsStudent(CompanyJob job)
    {
        // Retrieve the latest job status
        var latestJob = await dbContext.CompanyJobs
            .AsNoTracking()
            .Where(j => j.RNGForPositionUploaded == job.RNGForPositionUploaded)
            .Select(j => new { j.PositionStatus })
            .FirstOrDefaultAsync();

        if (latestJob == null || latestJob.PositionStatus != "Δημοσιευμένη")
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Η Θέση Εργασίας έχει Αποδημοσιευτεί. Παρακαλώ δοκιμάστε αργότερα.");
            return;
        }

        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (!user.Identity.IsAuthenticated) return;

        var student = await GetStudentDetails(user.Identity.Name);
        if (student == null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Δεν βρέθηκαν στοιχεία φοιτητή.");
            return;
        }

        // Check for existing application
        var existingApplication = await dbContext.CompanyJobsApplied
            .FirstOrDefaultAsync(app =>
                app.StudentEmailAppliedForCompanyJob == student.Email &&
                app.RNGForCompanyJobApplied == job.RNGForPositionUploaded);

        if (existingApplication != null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", $"Έχετε ήδη κάνει αίτηση για: {job.PositionTitle}!");
            return;
        }

        // Get company data
        var company = await dbContext.Companies
            .FirstOrDefaultAsync(c => c.CompanyEmail == job.EmailUsedToUploadJobs);

        if (company == null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Σφάλμα: Δεν βρέθηκε η εταιρία.");
            return;
        }

        using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            // Create main application with details
            var jobApplication = new CompanyJobApplied
            {
                RNGForCompanyJobApplied = job.RNGForPositionUploaded,
                DateTimeStudentAppliedForCompanyJob = DateTime.UtcNow,
                StudentEmailAppliedForCompanyJob = student.Email,
                StudentUniqueIDAppliedForCompanyJob = student.Student_UniqueID,
                CompanysEmailWhereStudentAppliedForCompanyJob = job.EmailUsedToUploadJobs,
                CompanysUniqueIDWhereStudentAppliedForCompanyJob = company.Company_UniqueID,
                CompanyPositionStatusAppliedAtTheCompanySide = "Σε Επεξεργασία",
                CompanyPositionStatusAppliedAtTheStudentSide = "Σε Επεξεργασία",
                RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID = job.RNGForPositionUploaded_HashedAsUniqueID,
            
                // Let EF Core handle the relationship and IDs
                StudentDetails = new CompanyJobApplied_StudentDetails
                {

                    StudentEmailAppliedForCompanyJob = student.Email,
                    StudentUniqueIDAppliedForCompanyJob = student.Student_UniqueID,
                    DateTimeStudentAppliedForCompanyJob = DateTime.UtcNow,
                    RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID = job.RNGForPositionUploaded_HashedAsUniqueID
                },
            
                CompanyDetails = new CompanyJobApplied_CompanyDetails
                {
                    CompanysEmailWhereStudentAppliedForCompanyJob = job.EmailUsedToUploadJobs,
                    CompanysUniqueIDWhereStudentAppliedForCompanyJob = company.Company_UniqueID
                }
            };

            dbContext.CompanyJobsApplied.Add(jobApplication);
        
            // Add platform action
            dbContext.PlatformActions.Add(new PlatformActions
            {
                UserRole_PerformedAction = "STUDENT",
                ForWhat_PerformedAction = "COMPANY_JOB",
                HashedPositionRNG_PerformedAction = HashingHelper.HashLong(job.RNGForPositionUploaded),
                TypeOfAction_PerformedAction = "APPLY",
                DateTime_PerformedAction = DateTime.UtcNow
            });

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            // Send emails
            try
            {
                // Make sure Company is loaded (if not already included in the query)
                if (job.Company == null)
                {
                    await dbContext.Entry(job)
                        .Reference(j => j.Company)
                        .LoadAsync();
                }

                await InternshipEmailService.SendJobApplicationConfirmationToStudent(
                    student.Email, student.Name, student.Surname,
                    job.PositionTitle, job.RNGForPositionUploaded_HashedAsUniqueID, 
                    job.Company?.CompanyName); // Get company name from navigation property

                await InternshipEmailService.SendJobApplicationNotificationToCompany(
                    job.EmailUsedToUploadJobs, job.Company?.CompanyName, // Get company name from navigation property
                    student.Name, student.Surname, student.Email,
                    student.Telephone, student.StudyYear, student.Attachment,
                    job.RNGForPositionUploaded_HashedAsUniqueID, job.PositionTitle);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
            }

            await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Η αίτηση για {job.PositionTitle} υποβλήθηκε επιτυχώς!");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Full error: {ex.ToString()}"); // Log full error
            await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Σφάλμα κατά την υποβολή: {ex.InnerException?.Message ?? ex.Message}");
        }

        await RefreshStudentData();
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }


    private async Task UploadJobAsCompany(bool publishJob = false)
    {
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (!string.IsNullOrEmpty(userEmail))
            {
                var company = await dbContext.Companies.FirstOrDefaultAsync(c => c.CompanyEmail == userEmail);
                if (company != null)
                {
                    companyName = company.CompanyName;
                }
            }

            // Check each required field and scroll to it if it is missing
            if (string.IsNullOrWhiteSpace(job.PositionType))
            {
                showErrorMessageforUploadingjobsAsCompany = true;
                await JS.InvokeVoidAsync("scrollToElementById", "positionType");
                return;
            }

            if (job.PositionActivePeriod.Date <= DateTime.Today)
            {
                showErrorMessageforUploadingjobsAsCompany = true;
                await JS.InvokeVoidAsync("scrollToElementById", "positionActivePeriod");
                return;
            }

            if (string.IsNullOrWhiteSpace(job.PositionTitle))
            {
                showErrorMessageforUploadingjobsAsCompany = true;
                await JS.InvokeVoidAsync("scrollToElementById", "positionTitle");
                return;
            }

            if (string.IsNullOrWhiteSpace(job.PositionContactPerson))
            {
                showErrorMessageforUploadingjobsAsCompany = true;
                await JS.InvokeVoidAsync("scrollToElementById", "positionContactPerson");
                return;
            }

            if (string.IsNullOrWhiteSpace(job.PositionPerifereiaLocation))
            {
                showErrorMessageforUploadingjobsAsCompany = true;
                await JS.InvokeVoidAsync("scrollToElementById", "positionPerifereia");
                return;
            }

            if (string.IsNullOrWhiteSpace(job.PositionDimosLocation))
            {
                showErrorMessageforUploadingjobsAsCompany = true;
                await JS.InvokeVoidAsync("scrollToElementById", "positionDimos");
                return;
            }

            if (string.IsNullOrWhiteSpace(job.PositionDescription))
            {
                showErrorMessageforUploadingjobsAsCompany = true;
                await JS.InvokeVoidAsync("scrollToElementById", "positionDescription");
                return;
            }

            if (string.IsNullOrWhiteSpace(job.PositionAddressLocation))
            {
                showErrorMessageforUploadingjobsAsCompany = true;
                await JS.InvokeVoidAsync("scrollToElementById", "positionAddress");
                return;
            }

            if (!SelectedAreasWhenUploadJobAsCompany.Any())
            {
                showErrorMessageforUploadingjobsAsCompany = true;
                await JS.InvokeVoidAsync("scrollToElementById", "positionAreas");
                return;
            }

            // All validations passed - proceed with saving
            job.RNGForPositionUploaded = new Random().NextInt64();
            job.RNGForPositionUploaded_HashedAsUniqueID = HashingHelper.HashLong(job.RNGForPositionUploaded);
            job.EmailUsedToUploadJobs = userEmail; // This links to the Company via CompanyEmail
            job.UploadDateTime = DateTime.Now;
            job.PositionForeas = companyData.CompanyType;
            job.PositionAreas = string.Join(",", SelectedAreasWhenUploadJobAsCompany.Select(a => a.AreaName));
            job.PositionStatus = publishJob ? "Δημοσιευμένη" : "Μη Δημοσιευμένη";

            // No need to set CompanyNameUploadJob as it's now handled via the Company relation

            dbContext.CompanyJobs.Add(job);
            await dbContext.SaveChangesAsync();

            showSuccessMessage = true;
            showErrorMessageforUploadingjobsAsCompany = false;

            // Clear form fields
            job = new CompanyJob();
            SelectedAreasWhenUploadJobAsCompany.Clear();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            showSuccessMessage = false;
            showErrorMessagesForAreasWhenUploadJobAsCompany = false;
            showErrorMessageforUploadingjobsAsCompany = true;
            Console.WriteLine($"Error uploading job: {ex.Message}");
        }
        StateHasChanged();
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }

    private async Task UploadThesisAsCompany(bool publishThesis = false)
    {
        try
        {
            // Initial debug logging
            Console.WriteLine("=== INITIAL FIELD VALUES ===");
            LogCurrentThesisState();
            Console.WriteLine("===========================");

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (!string.IsNullOrEmpty(userEmail))
            {
                var company = await dbContext.Companies.FirstOrDefaultAsync(c => c.CompanyEmail == userEmail);
                if (company != null)
                {
                    companyName = company.CompanyName;
                }
            }

            // Update collections before validation
            thesis.CompanyThesisSkillsNeeded = string.Join(",", SelectedSkillsWhenUploadThesisAsCompany.Select(a => a.SkillName));
            thesis.CompanyThesisAreasUpload = string.Join(",", SelectedAreasWhenUploadThesisAsCompany.Select(a => a.AreaName));

            // Debug logging before validation
            Console.WriteLine("=== FIELD VALUES BEFORE VALIDATION ===");
            LogCurrentThesisState();
            Console.WriteLine("=====================================");

            // Validation checks
            if (string.IsNullOrWhiteSpace(thesis.CompanyThesisTitle))
            {
                await HandleValidationError("companyThesisTitle");
                return;
            }

            if (string.IsNullOrWhiteSpace(thesis.CompanyThesisDescriptionsUploaded))
            {
                await HandleValidationError("CompanyThesisDescription");
                return;
            }

            if (string.IsNullOrWhiteSpace(thesis.CompanyThesisCompanySupervisorFullName))
            {
                await HandleValidationError("CompanyThesisCompanySupervisorFullName");
                return;
            }

            if (string.IsNullOrWhiteSpace(thesis.CompanyThesisContactPersonEmail))
            {
                await HandleValidationError("thesisContactPersonEmail");
                return;
            }

            if (thesis.CompanyThesisStartingDate.Date <= DateTime.Today)
            {
                await HandleValidationError("CompanyThesisStartingDate");
                return;
            }

            if (!SelectedAreasWhenUploadThesisAsCompany.Any() || string.IsNullOrWhiteSpace(thesis.CompanyThesisAreasUpload))
            {
                await HandleValidationError("toggleCheckboxesForThesisAreas");
                return;
            }

            if (!SelectedSkillsWhenUploadThesisAsCompany.Any() || string.IsNullOrWhiteSpace(thesis.CompanyThesisSkillsNeeded))
            {
                await HandleValidationError("toggleCheckboxesForThesisSkills");
                return;
            }

            // Final debug logging before save
            Console.WriteLine("=== FINAL VALUES BEFORE SAVE ===");
            LogCurrentThesisState();
            Console.WriteLine("===============================");

            // Prepare thesis for saving
            thesis.RNGForThesisUploadedAsCompany = new Random().NextInt64();
            thesis.RNGForThesisUploadedAsCompany_HashedAsUniqueID = HashingHelper.HashLong(thesis.RNGForThesisUploadedAsCompany);
            thesis.CompanyEmailUsedToUploadThesis = userEmail;  // This is now the foreign key
            thesis.CompanyThesisUploadDateTime = DateTime.Now;
            thesis.ThesisType = ThesisType.Company;
            thesis.IsProfessorInteresetedInCompanyThesis = false;
            thesis.IsProfessorInterestedInCompanyThesisStatus = "Δεν έχει γίνει Αποδοχή";
            thesis.CompanyThesisStatus = publishThesis ? "Δημοσιευμένη" : "Μη Δημοσιευμένη";

            // Save changes
            Console.WriteLine("Before saving changes...");
            dbContext.CompanyTheses.Add(thesis);

            // Log entity states
            var entries = dbContext.ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                Console.WriteLine($"{entry.Entity.GetType().Name} - {entry.State}");
            }

            await dbContext.SaveChangesAsync();
            Console.WriteLine("After saving changes...");

            showSuccessMessage = true;
            showErrorMessageforUploadingthesisAsCompany = false;

            // Clear form fields
            thesis = new CompanyThesis();
            Areas.Clear();
            Skills.Clear();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading thesis: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            showSuccessMessage = false;
            showErrorMessagesForSkillsWhenUploadThesisAsCompany = true;
            showErrorMessageforUploadingthesisAsCompany = true;
        }

        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        StateHasChanged();
    }

    // Helper method to log current thesis state
    private void LogCurrentThesisState()
    {
        Console.WriteLine($"Title: '{thesis?.CompanyThesisTitle}' (null/empty: {string.IsNullOrWhiteSpace(thesis?.CompanyThesisTitle)})");
        Console.WriteLine($"Description: '{thesis?.CompanyThesisDescriptionsUploaded}' (null/empty: {string.IsNullOrWhiteSpace(thesis?.CompanyThesisDescriptionsUploaded)})");
        Console.WriteLine($"Supervisor: '{thesis?.CompanyThesisCompanySupervisorFullName}' (null/empty: {string.IsNullOrWhiteSpace(thesis?.CompanyThesisCompanySupervisorFullName)})");
        Console.WriteLine($"Contact Email: '{thesis?.CompanyThesisContactPersonEmail}' (null/empty: {string.IsNullOrWhiteSpace(thesis?.CompanyThesisContactPersonEmail)})");
        Console.WriteLine($"Starting Date: {thesis?.CompanyThesisStartingDate} (valid: {thesis?.CompanyThesisStartingDate.Date > DateTime.Today})");
        Console.WriteLine($"Areas: '{thesis?.CompanyThesisAreasUpload}' (count: {SelectedAreasWhenUploadThesisAsCompany?.Count})");
        Console.WriteLine($"Skills: '{thesis?.CompanyThesisSkillsNeeded}' (count: {SelectedSkillsWhenUploadThesisAsCompany?.Count})");
    }

    // Helper method to handle validation errors
    private async Task HandleValidationError(string elementId)
    {
        showErrorMessageforUploadingthesisAsCompany = true;
        await JS.InvokeVoidAsync("scrollToElementById", elementId);
        Console.WriteLine($"Validation failed for element: {elementId}");
    }




    private async Task UploadInternshipAsCompany()
    {
        try
        {
            // All validation checks remain exactly the same
            if (string.IsNullOrWhiteSpace(companyInternship.CompanyInternshipTitle))
            {
                showErrorMessage = true;
                await JS.InvokeVoidAsync("scrollToElementById", "positionTitle");
                return;
            }

            if (string.IsNullOrWhiteSpace(companyInternship.CompanyInternshipType))
            {
                showErrorMessage = true;
                await JS.InvokeVoidAsync("scrollToElementById", "internshipType");
                return;
            }

            if (companyInternship.CompanyInternshipActivePeriod.Date <= DateTime.Today)
            {
                showErrorMessage = true;
                await JS.InvokeVoidAsync("scrollToElementById", "internshipActivePeriod");
                return;
            }

            if (companyInternship.CompanyInternshipFinishEstimation.Date <= DateTime.Today)
            {
                showErrorMessage = true;
                await JS.InvokeVoidAsync("scrollToElementById", "internshipFinishEstimation");
                return;
            }

            if (string.IsNullOrWhiteSpace(companyInternship.CompanyInternshipESPA))
            {
                showErrorMessage = true;
                await JS.InvokeVoidAsync("scrollToElementById", "internshipESPA");
                return;
            }

            if (string.IsNullOrWhiteSpace(companyInternship.CompanyInternshipContactPerson))
            {
                showErrorMessage = true;
                await JS.InvokeVoidAsync("scrollToElementById", "internshipContactPerson");
                return;
            }

            if (string.IsNullOrWhiteSpace(companyInternship.CompanyInternshipContactTelephonePerson))
            {
                showErrorMessage = true;
                await JS.InvokeVoidAsync("scrollToElementById", "internshipContactPhone");
                return;
            }

            if (string.IsNullOrWhiteSpace(companyInternship.CompanyInternshipPerifereiaLocation))
            {
                showErrorMessage = true;
                await JS.InvokeVoidAsync("scrollToElementById", "internshipPerifereia");
                return;
            }

            if (string.IsNullOrWhiteSpace(companyInternship.CompanyInternshipDimosLocation))
            {
                showErrorMessage = true;
                await JS.InvokeVoidAsync("scrollToElementById", "internshipDimos");
                return;
            }

            if (string.IsNullOrWhiteSpace(companyInternship.CompanyInternshipPostalCodeLocation))
            {
                showErrorMessage = true;
                await JS.InvokeVoidAsync("scrollToElementById", "internshipPostalCode");
                return;
            }

            if (string.IsNullOrWhiteSpace(companyInternship.CompanyInternshipDescription))
            {
                showErrorMessage = true;
                await JS.InvokeVoidAsync("scrollToElementById", "internshipDescription");
                return;
            }

            if (!SelectedAreasWhenUploadInternshipAsCompany.Any())
            {
                showErrorMessagesForAreasWhenUploadInternshipAsCompany = true;
                await JS.InvokeVoidAsync("scrollToElementById", "internshipAreas");
                return;
            }

            // Updated property names and added navigation property
            companyInternship.RNGForInternshipUploadedAsCompany = new Random().NextInt64();
            companyInternship.RNGForInternshipUploadedAsCompany_HashedAsUniqueID = 
                HashingHelper.HashLong(companyInternship.RNGForInternshipUploadedAsCompany);
            companyInternship.CompanyEmailUsedToUploadInternship = companyData.CompanyEmail;
        
            // Set the Company navigation property
            companyInternship.Company = await dbContext.Companies
                .FirstOrDefaultAsync(c => c.CompanyEmail == companyData.CompanyEmail);

            // Keep all other assignments exactly the same
            companyInternship.CompanyInternshipUploadDate = DateTime.Now;
            companyInternship.CompanyInternshipForeas = companyData.CompanyType;
            companyInternship.CompanyInternshipType = companyInternship.CompanyInternshipType;
            companyInternship.CompanyInternshipESPA = companyInternship.CompanyInternshipESPA;
            companyInternship.CompanyInternshipActivePeriod = companyInternship.CompanyInternshipActivePeriod;
            companyInternship.CompanyInternshipFinishEstimation = companyInternship.CompanyInternshipFinishEstimation;
            companyInternship.CompanyInternshipTitle = companyInternship.CompanyInternshipTitle;
            companyInternship.CompanyInternshipContactPerson = companyInternship.CompanyInternshipContactPerson;
            companyInternship.CompanyInternshipContactTelephonePerson = companyInternship.CompanyInternshipContactTelephonePerson;
            companyInternship.CompanyInternshipPerifereiaLocation = companyInternship.CompanyInternshipPerifereiaLocation;
            companyInternship.CompanyInternshipDimosLocation = companyInternship.CompanyInternshipDimosLocation;
            companyInternship.CompanyInternshipPostalCodeLocation = companyInternship.CompanyInternshipPostalCodeLocation;
            companyInternship.CompanyInternshipTransportOffer = companyInternship.CompanyInternshipTransportOffer;
            companyInternship.CompanyInternshipAreas = string.Join(",", SelectedAreasWhenUploadInternshipAsCompany.Select(a => a.AreaName));
            companyInternship.CompanyInternshipDescription = companyInternship.CompanyInternshipDescription;

            // EKPA supervisor logic remains exactly the same
            if (selectedProfessorId.HasValue)
            {
                var professor = await dbContext.Professors
                    .FirstOrDefaultAsync(p => p.Id == selectedProfessorId.Value);

                if (professor != null)
                {
                    companyInternship.CompanyInternshipEKPASupervisor = $"{professor.ProfName} {professor.ProfSurname}";
                }
                else
                {
                    companyInternship.CompanyInternshipEKPASupervisor = "Unknown Professor";
                }
            }
            else
            {
                companyInternship.CompanyInternshipEKPASupervisor = "Χωρίς Προτίμηση";
            }

            companyInternship.CompanyUploadedInternshipStatus = companyInternship.CompanyUploadedInternshipStatus;
            companyInternship.CompanyInternshipAttachment = companyInternship.CompanyInternshipAttachment;

            dbContext.CompanyInternships.Add(companyInternship);
            await dbContext.SaveChangesAsync();

            showSuccessMessageWhenSaveInternshipAsCompany = true;
            showErrorMessage = false;
            showErrorMessagesForAreasWhenUploadInternshipAsCompany = false;

            companyInternship = new CompanyInternship();
            SelectedAreasWhenUploadInternshipAsCompany.Clear();
            StateHasChanged();
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
        catch (Exception ex)
        {
            showSuccessMessageWhenSaveInternshipAsCompany = false;
            showErrorMessagesForAreasWhenUploadInternshipAsCompany = true;
            showErrorMessage = true;
            Console.WriteLine($"Error uploading internship: {ex.Message}");
        }
    }



    private async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file != null && file.ContentType == "application/pdf")
        {
            using var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms); // Copy file stream to memory stream
            job.PositionAttachment = ms.ToArray(); // Convert memory stream to byte array
        }
    }

    private async Task HandleFileSelectedForCompanyThesisAttachment(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file != null && file.ContentType == "application/pdf")
        {
            using var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms); // Copy file stream to memory stream
            thesis.CompanyThesisAttachmentUpload = ms.ToArray(); // Convert memory stream to byte array
        }
    }

    private async Task HandleFileSelectedForAnnouncementAttachment(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file != null && file.ContentType == "application/pdf")
        {
            using var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms); // Copy file stream to memory stream
            announcement.CompanyAnnouncementAttachmentFile = ms.ToArray(); // Convert memory stream to byte array
        }
    }

    private async Task HandleFileSelectedForAnnouncementAttachmentAsProfessor(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file != null && file.ContentType == "application/pdf")
        {
            using var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms); // Copy file stream to memory stream
            professorannouncement.ProfessorAnnouncementAttachmentFile = ms.ToArray(); // Convert memory stream to byte array
        }
    }

//SOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSO AYTO EDW LEITOURGEI KANONIKA. THA KANW TO IDIO GIA OLA TA ATTACHMENTS KAI EDW KAI STON HOST KAI STO ANEVASMA GIA KA8E ARXEIO 8ELEI ALLAGI SOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSO

    private async Task HandleFileSelectedForThesisAttachmentAsProfessor(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file != null && file.ContentType == "application/pdf")
        {
            using var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms); // Copy file stream to memory stream
            professorthesis.ThesisAttachment = ms.ToArray(); // Convert memory stream to byte array
        }
    }
//SOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSO AYTO EDW LEITOURGEI KANONIKA. THA KANW TO IDIO GIA OLA TA ATTACHMENTS KAI EDW KAI STON HOST KAI STO ANEVASMA GIA KA8E ARXEIO 8ELEI ALLAGI SOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSO



    private async Task HandleFileSelectedForUploadInternshipAsCompany(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file != null && file.ContentType == "application/pdf")
        {
            using var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms); // Copy file stream to memory stream
            companyInternship.CompanyInternshipAttachment = ms.ToArray(); // Convert memory stream to byte array
        }
    }

    private void ToggleFormVisibilityForUploadCompanyJobs()
    {
        isForm1Visible = !isForm1Visible;
        StateHasChanged();
    }

    private void ToggleFormVisibilityForUploadCompanyAnnouncements()
    {
        isAnnouncementsFormVisible = !isAnnouncementsFormVisible;
        StateHasChanged();
    }

    private void ToggleFormVisibilityToShowGeneralAnnouncementsAndEventsAsCompany()
    {
        isAnnouncementsFormVisibleToShowGeneralAnnouncementsAndEventsAsCompany = !isAnnouncementsFormVisibleToShowGeneralAnnouncementsAndEventsAsCompany;
        StateHasChanged();
    }

    private void ToggleFormVisibilityToShowGeneralAnnouncementsAndEventsAsRG()
    {
        isAnnouncementsFormVisibleToShowGeneralAnnouncementsAndEventsAsRG = !isAnnouncementsFormVisibleToShowGeneralAnnouncementsAndEventsAsRG;
        StateHasChanged();
    }

    private void ToggleFormVisibilityForUploadProfessorAnnouncements()
    {
        isProfessorAnnouncementsFormVisible = !isProfessorAnnouncementsFormVisible;
        StateHasChanged();
    }

    private void ToggleFormVisibilityForUploadProfessorThesis()
    {
        isProfessorThesisFormVisible = !isProfessorThesisFormVisible;
        StateHasChanged();
    }

    private void ToggleFormVisibilityForUploadProfessorInternship()
    {
        isProfessorInternshipFormVisible = !isProfessorInternshipFormVisible;
        StateHasChanged();
    }

    private void ToggleFormVisibilityForSearchStudentAsProfessor()
    {
        isProfessorSearchStudentFormVisible = !isProfessorSearchStudentFormVisible;
        StateHasChanged();
    }

    private void ToggleFormVisibilityForSearchCompanyAsProfessor()
    {
        isProfessorSearchCompanyFormVisible = !isProfessorSearchCompanyFormVisible;
        StateHasChanged();
    }

    private void ToggleFormVisibilityForSearchCompanyAsRG()
    {
        isRGSearchCompanyFormVisible = !isRGSearchCompanyFormVisible;
        StateHasChanged();
    }

    private async Task ToggleFormVisibilityToShowMyActiveJobsAsCompany()
    {
        isForm2Visible = !isForm2Visible;
        if (isForm2Visible)
        {
            
            await LoadJobs(); // Ensure jobs are loaded when the form is shown
        }
        StateHasChanged();

    }

    private async Task ToggleFormVisibilityToShowMyActiveThesesAsCompany()
    {
        isShowActiveThesesAsCompanyFormVisible = !isShowActiveThesesAsCompanyFormVisible;
        if (isShowActiveThesesAsCompanyFormVisible)
        {
            
            await LoadThesesAsCompany(); // Ensure jobs are loaded when the form is shown
        }
        StateHasChanged();

    }


    private async Task TogglePositionDetails(CompanyJob position)
    {
        if (positionDetails.ContainsKey(position.Id))
        {
            positionDetails[position.Id] = !positionDetails[position.Id];           

        }
        else
        {
            positionDetails[position.Id] = true; // Default to true if not found
        }
       await LoadWhoApplied(position.RNGForPositionUploaded);
       StateHasChanged();
    }

    private async Task DeleteJobPosition(int jobId)
    {
        // Show custom confirmation dialog with HTML styling
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
            "Πρόκειται να διαγράψετε οριστικά αυτή τη Θέση Εργασίας. <br>" +
            "<strong style='color: red;'>Η ενέργεια αυτή είναι μη αναστρέψιμη!</strong>"
        });

        // Proceed only if confirmed
        if (isConfirmed)
        {
            var job = await dbContext.CompanyJobs.FindAsync(jobId);
            if (job != null)
            {
                dbContext.CompanyJobs.Remove(job);
                await dbContext.SaveChangesAsync();
                await LoadJobs(); // Reload the jobs list
            }
            StateHasChanged();
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
    }




    private async Task DeleteCompanyThesis(int companythesisId)
    {
        // Show custom confirmation dialog with HTML styling
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
            "Πρόκειται να διαγράψετε οριστικά αυτή τη Πτυχιακή Εργασία. <br>" +
            "<strong style='color: red;'>Η ενέργεια αυτή είναι μη αναστρέψιμη!</strong>"
        });

        // Proceed only if confirmed
        if (isConfirmed)
        {
            var companytheses = await dbContext.CompanyTheses.FindAsync(companythesisId);
            if (companytheses != null)
            {
                dbContext.CompanyTheses.Remove(companytheses);
                await dbContext.SaveChangesAsync();
                await LoadThesesAsCompany(); // Reload the theses list
            }
            StateHasChanged();
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
    }



    private void EditJobPosition(int positionId)
    {
        // Check if 'companyJobs' is not null before attempting to access it
        if (jobs == null)
        {
            Console.WriteLine("The companyJobs list is null.");
            return;
        }

        // Find the job to edit
        var jobToEdit = jobs.FirstOrDefault(j => j.Id == positionId);
        if (jobToEdit != null)
        {
            isEditPopupVisibleForJobs = true;
            // Set the job object to be edited
            job = jobToEdit;
            // Perform any additional logic needed for editing in the same component
            // For example, setting flags or triggering UI updates
            isEditing = true;
            StateHasChanged();

        }
        else
        {
            // Handle the case when the job position with the given positionId is not found
            Console.WriteLine($"Job position with ID {positionId} not found.");
        }
    }


    private async Task DownloadAttachmentForCompanyJobs(int jobId)
    {
        var job = await dbContext.CompanyJobs.FindAsync(jobId);
        if (job != null && job.PositionAttachment != null)
        {
            var fileName = $"{job.PositionTitle}_Attachment.pdf"; // Ensure file name ends with .pdf
            var mimeType = "application/pdf"; // Correct MIME type for PDF
            await JS.InvokeVoidAsync("BlazorDownloadAttachmentPositionFile", fileName, mimeType, job.PositionAttachment);
        }
    }


    private async Task DownloadAttachmentForCompanyTheses(int companyThesisId)
    {
        var companythesis = await dbContext.CompanyTheses.FindAsync(companyThesisId);
        if (companythesis != null && companythesis.CompanyThesisAttachmentUpload != null)
        {
            var fileName = $"{companythesis.CompanyThesisTitle}_Attachment";
            var mimeType = "application/octet-stream"; // or set the appropriate MIME type if known
            var fileContent = new byte[companythesis.CompanyThesisAttachmentUpload.Length];
            companythesis.CompanyThesisAttachmentUpload.CopyTo(fileContent, 0);
            await JS.InvokeVoidAsync("BlazorDownloadAttachmentPositionFile", fileName, mimeType, fileContent);
        }
    }

    //SOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSO AYTO EDW LEITOURGEI KANONIKA. THA KANW TO IDIO GIA OLA TA ATTACHMENTS KAI EDW KAI STON HOST KAI STO ANEVASMA GIA KA8E ARXEIO 8ELEI ALLAGI SOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSO
    private async Task DownloadAttachmentForProfessorTheses(int professorThesisId)
    {
        var professorThesis = await dbContext.ProfessorTheses.FindAsync(professorThesisId);
        if (professorThesis != null && professorThesis.ThesisAttachment != null)
        {
            // Use the ThesisTitle for file naming and ensure .pdf extension
            var fileName = $"{professorThesis.ThesisTitle}_Attachment.pdf"; 
            var mimeType = "application/pdf"; 

            // Convert byte array to base64 string for JavaScript
            var fileContentBase64 = Convert.ToBase64String(professorThesis.ThesisAttachment); 
        
            // Invoke the JavaScript download function
            await JS.InvokeVoidAsync("BlazorDownloadAttachmentProfessorThesisFile", fileName, mimeType, fileContentBase64);
            Console.WriteLine($"File Size: {professorThesis.ThesisAttachment.Length}");

        }
    }
    //SOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSO AYTO EDW LEITOURGEI KANONIKA. THA KANW TO IDIO GIA OLA TA ATTACHMENTS KAI EDW KAI STON HOST KAI STO ANEVASMA GIA KA8E ARXEIO 8ELEI ALLAGI SOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSOSO





    private async Task DownloadAttachmentForCompanyInternships(int internshipId)
    {
        var internship = await dbContext.CompanyInternships.FindAsync(internshipId);
        if (internship != null && internship.CompanyInternshipAttachment != null)
        {
            var fileName = $"{internship.CompanyInternshipTitle}_Attachment.pdf"; // Ensure file name ends with .pdf
            var mimeType = "application/pdf"; // Correct MIME type for PDF
            await JS.InvokeVoidAsync("BlazorDownloadAttachmentPositionFile", fileName, mimeType, internship.CompanyInternshipAttachment);
        }
    }

    private async Task DownloadAttachmentForProfessorInternships(int internshipId)
    {
        var internship = await dbContext.ProfessorInternships.FindAsync(internshipId);
        if (internship != null && internship.ProfessorInternshipAttachment != null)
        {
            var fileName = $"{internship.ProfessorInternshipTitle}_Attachment.pdf"; // Ensure file name ends with .pdf
            var mimeType = "application/pdf"; // Correct MIME type for PDF
            await JS.InvokeVoidAsync("BlazorDownloadAttachmentPositionFile", fileName, mimeType, internship.ProfessorInternshipAttachment);
        }
    }


    private async Task UpdateJob()
    {
        try
        {
            dbContext.CompanyJobs.Update(job);
            job.TimesUpdated++;
            job.UpdateDateTime = DateTime.Now;
            await dbContext.SaveChangesAsync();


            showSuccessUpdateMessage = true;
            showErrorMessage = false;
        }
        catch (Exception)
        {
            showSuccessMessage = false;
            showErrorMessage = true;
            showSuccessUpdateMessage = false;
        }
        StateHasChanged();
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);

    }

    private async Task UpdateCompanyThesis()
    {
        try
        {
            dbContext.CompanyJobs.Update(job);
            job.TimesUpdated++;
            job.UpdateDateTime = DateTime.Now;
            await dbContext.SaveChangesAsync();


            showSuccessUpdateMessage = true;
            showErrorMessage = false;
        }
        catch (Exception)
        {
            showSuccessMessage = false;
            showErrorMessage = true;
            showSuccessUpdateMessage = false;
        }
        StateHasChanged();
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);

    }

    private async Task LoadUserApplications()
    {
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var userEmail = user.FindFirst("name")?.Value;
                if (!string.IsNullOrEmpty(userEmail))
                {
                    showApplications = true;
                
                    // Retrieve applications using email + unique ID instead of registration number
                    jobApplications = await dbContext.CompanyJobsApplied
                        .Where(j => j.StudentEmailAppliedForCompanyJob == userEmail && 
                                   j.StudentUniqueIDAppliedForCompanyJob == userData.Student_UniqueID)
                        .OrderByDescending(j => j.DateTimeStudentAppliedForCompanyJob)
                        .ToListAsync();

                    StateHasChanged();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading applications: {ex.Message}");
            StateHasChanged();
        }
    }

    private async Task LoadWhoApplied(long rngForPositionUploaded)
    {
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var companyEmail = user.FindFirst("name")?.Value;
            
                if (!string.IsNullOrEmpty(companyEmail))
                {
                    jobApplicationsmadeToCompany = await dbContext.CompanyJobsApplied
                        .Include(a => a.StudentDetails)
                        .Include(a => a.CompanyDetails)
                        .Where(j => j.CompanysEmailWhereStudentAppliedForCompanyJob == companyEmail &&
                                   j.RNGForCompanyJobApplied == rngForPositionUploaded)
                        .OrderByDescending(j => j.DateTimeStudentAppliedForCompanyJob)
                        .ToListAsync();

                    StateHasChanged();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading applications: {ex.Message}");
            StateHasChanged();
        }
    }


    private async Task AcceptApplicationAsCompany(CompanyJobApplied application)
    {
        try
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            // Update status in the main entity (since status fields were moved there)
            application.CompanyPositionStatusAppliedAtTheCompanySide = "Έχετε Αποδεχτεί";
            application.CompanyPositionStatusAppliedAtTheStudentSide = "Επιτυχής";

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            // Get required data for email
            var studentName = $"{userData.Name} {userData.Surname}";
            var positionTitle = companyjobb.PositionTitle;
            var companyName = companyData.CompanyName;

            // Send acceptance email
            var emailService = new InternshipEmailService(
                "kleapali70@gmail.com",
                "mbyuqdgdyrvtefan",
                "kleapali69@hotmail.com"
            );

            await emailService.SendVerificationEmail(
                application.StudentEmailAppliedForCompanyJob,
                studentName,
                positionTitle,
                companyName);

            await JS.InvokeVoidAsync("alert", 
                $"Application accepted and notification sent to {application.StudentEmailAppliedForCompanyJob}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accepting application: {ex.Message}");
            await JS.InvokeVoidAsync("alert", "Error processing application acceptance.");
        }
    }

    private async Task RejectApplicationAsCompany(CompanyJobApplied application)
    {
        try
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            // Update status in the main entity (since status fields were moved there)
            application.CompanyPositionStatusAppliedAtTheCompanySide = "Έχει Απορριφθεί";
            application.CompanyPositionStatusAppliedAtTheStudentSide = "Απορρίφθηκε";

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            // Get required data for email
            var studentName = $"{userData.Name} {userData.Surname}";
            var positionTitle = companyjobb.PositionTitle;
            var companyName = companyData.CompanyName;

            // Send rejection email
            var emailService = new InternshipEmailService(
                "kleapali70@gmail.com",
                "mbyuqdgdyrvtefan",
                "kleapali69@hotmail.com"
            );

            await emailService.SendRejectionEmail(
                application.StudentEmailAppliedForCompanyJob,
                studentName,
                positionTitle,
                companyName);

            await JS.InvokeVoidAsync("alert", 
                $"Application rejected and notification sent to {application.StudentEmailAppliedForCompanyJob}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error rejecting application: {ex.Message}");
            await JS.InvokeVoidAsync("alert", "Error processing application rejection.");
        }
    }

    private async Task ToggleDoughnutChart()
    {
        isDoughnutChartVisible = !isDoughnutChartVisible;
        if (isDoughnutChartVisible)
        {
            await JS.InvokeVoidAsync("showDoughnutChart", "doughnutChart");
        }
        else
        {
            await JS.InvokeVoidAsync("hideDoughnutChart", "doughnutChart");
        }
    }

    private async Task ToggleDepartmentDistributionChart()
    {
        isDepartmentDistributionChartVisible = !isDepartmentDistributionChartVisible;
        if (isDepartmentDistributionChartVisible)
        {
            await JS.InvokeVoidAsync("showDoughnutChart", "departmentDistributionChart");
        }
        else
        {
            await JS.InvokeVoidAsync("hideDoughnutChart", "departmentDistributionChart");
        }
    }

    private async Task LoadSkillDistributionAsync()
    {
        var students = await dbContext.Students.ToListAsync();
        foreach (var student in students)
        {
            var keywords = student.Keywords.Split(',');
            foreach (var keyword in keywords)
            {
                var trimmedKeyword = keyword.Trim();
                if (skillDistribution.ContainsKey(trimmedKeyword))
                {
                    skillDistribution[trimmedKeyword]++;
                }
                else
                {
                    skillDistribution[trimmedKeyword] = 1;
                }
            }
        }

        int totalSkills = skillDistribution.Values.Sum();
        foreach (var key in skillDistribution.Keys.ToList())
        {
            skillDistribution[key] = (skillDistribution[key] * 100) / totalSkills;
        }
    }

    private async Task LoadDepartmentDistributionAsync()
    {
        var students = await dbContext.Students.ToListAsync();
        foreach (var student in students)
        {
            var department = student.Department;
            if (departmentDistribution.ContainsKey(department))
            {
                departmentDistribution[department]++;
            }
            else
            {
                departmentDistribution[department] = 1;
            }
        }

        int totalDepartments = departmentDistribution.Values.Sum();
        foreach (var key in departmentDistribution.Keys.ToList())
        {
            departmentDistribution[key] = (departmentDistribution[key] * 100) / totalDepartments;
        }
    }

    // This method is called after the component has rendered. It is used to create the charts using JavaScript.
    // protected override async Task OnAfterRenderAsync(bool firstRender) //EINAI TSAPATSOULIA NA VRETHEI ALLOS TROPOS GIARI ME TO ASYNC CRASHAREI OTAN PAW STO PROFILE // TO EVALA SXOLIA GIA MIN CRASHAREI TO PROFILE TOY COMPANY
    // {
    //     if (hasReadAsCompanyPermission)
    //     {
    //         await JS.InvokeVoidAsync("createDoughnutChart", "doughnutChart", new
    //         {
    //             labels = skillDistribution.Keys.ToArray(),
    //             datasets = new[]
    //             {
    //                 new
    //                 {
    //                     data = skillDistribution.Values.ToArray(),
    //                     backgroundColor = new[]
    //                     {
    //                         "#FF6384",
    //                         "#36A2EB",
    //                         "#FFCE56",
    //                         "#4BC0C0",
    //                         "#9966FF",
    //                         "#FF9F40"
    //                     }
    //                 }
    //             }
    //         });

    //         await JS.InvokeVoidAsync("createDoughnutChart", "departmentDistributionChart", new
    //         {
    //             labels = departmentDistribution.Keys.ToArray(),
    //             datasets = new[]
    //             {
    //                 new
    //                 {
    //                     data = departmentDistribution.Values.ToArray(),
    //                     backgroundColor = new[]
    //                     {
    //                         "#FF6384",
    //                         "#36A2EB",
    //                         "#FFCE56",
    //                         "#4BC0C0",
    //                         "#9966FF",
    //                         "#FF9F40"
    //                     }
    //                 }
    //             }
    //         });
    //     }
    // }
    
    private void ToggleFormVisibilityForUploadCompanyInternships()
    {
        isUploadCompanyInternshipsFormVisible = !isUploadCompanyInternshipsFormVisible;
        StateHasChanged();
    }

    private void ToggleFormVisibilityForUploadCompanyThesis()
    {
        isUploadCompanyThesisFormVisible = !isUploadCompanyThesisFormVisible;
        StateHasChanged();
    }

    private void ToggleFormVisibilityForUploadCompanyEvent()
    {
        isUploadCompanyEventFormVisible = !isUploadCompanyEventFormVisible;
        StateHasChanged();
    }

    private void ToggleFormVisibilityForUploadProfessorEvent()
    {
        isUploadProfessorEventFormVisible = !isUploadProfessorEventFormVisible;
        StateHasChanged();
    }


    private void OnAreaChange(ChangeEventArgs e)
    {
        var selectedValue = e.Value.ToString();
        var selectedArea = availableAreas.FirstOrDefault(a => a.AreaName == selectedValue);
        if (selectedArea != null)
        {
            ToggleSubFields(selectedArea);
        }
    }

    private async Task MoveSelectedAreaToLeft()
    {
        var newlySelectedAreas = await GetSelectedAreasFromDOM("selectedAreas");

        foreach (var areaName in newlySelectedAreas)
        {
            var selectedArea = selectedAreasForAssessment.FirstOrDefault(sa => sa.AreaName == areaName);
            if (selectedArea != null)
            {
                selectedAreasForAssessment.Remove(selectedArea);

                if (!availableAreas.Any(a => a.AreaName == areaName))
                {
                    var areaToAdd = dbContext.Areas.FirstOrDefault(a => a.AreaName == areaName);
                    if (areaToAdd != null)
                    {
                        availableAreas.Add(areaToAdd);
                        Console.WriteLine($"Area {areaName} added back to availableAreas (Left List Box)");
                    }
                }
            }
        }

        StateHasChanged();
    }


    private async Task MoveSelectedAreaToRight()
    {
        var newlySelectedAreas = await GetSelectedAreasFromDOM("availableAreas");

        foreach (var areaName in newlySelectedAreas)
        {
            var areaToRemove = availableAreas.FirstOrDefault(a => a.AreaName == areaName);
            if (areaToRemove != null)
            {
                availableAreas.Remove(areaToRemove);

                if (!selectedAreasForAssessment.Any(sa => sa.AreaName == areaToRemove.AreaName))
                {
                    selectedAreasForAssessment.Add(new SelectedArea { AreaName = areaToRemove.AreaName });
                    Console.WriteLine($"Area {areaName} added to selectedAreas (Right List Box)");
                }
            }
        }

        StateHasChanged();
    }

    public class SelectedArea
    {
        public string AreaName { get; set; }
        public int Assessment { get; set; } = 1; // Default assessment value is 1
    }

    private async Task<List<string>> GetSelectedAreasFromDOM(string selectId)
    {
        var selectedAreas = await JS.InvokeAsync<List<string>>("getSelectedValues", new object[] { selectId });
        return selectedAreas;
    }

    private void ToggleSubFields(Area area)
    {
        if (expandedAreas.Contains(area.AreaName))
        {
            expandedAreas.Remove(area.AreaName);
        }
        else
        {
            expandedAreas.Add(area.AreaName);
        }
        StateHasChanged();
    }

    private List<string> GetTownsForRegion(string region)
    {
        if (string.IsNullOrEmpty(region) || !RegionToTownsMap.ContainsKey(region))
        {
            return new List<string>();
        }

        return RegionToTownsMap[region];
    }


    private void UpdateTransportOffer(bool offer)
    {
        companyInternship.CompanyInternshipTransportOffer = offer;
    }

    private void UpdateTransportOfferForProfessorInternship(bool offer)
    {
        professorInternship.ProfessorInternshipTransportOffer = offer;
    }

    private void ToggleFormVisibilityToShowMyActiveInternshipsAsCompany()
    {
        isShowActiveInternshipsAsCompanyFormVisible = !isShowActiveInternshipsAsCompanyFormVisible;
    }

    private void ToggleFormVisibilityToShowMyActiveInternshipsAsProfessor()
    {
        isShowActiveInternshipsAsProfessorFormVisible = !isShowActiveInternshipsAsProfessorFormVisible;
    }



    private void ToggleInternshipDetails(CompanyInternship internship)
    {
        if (positionDetails.ContainsKey(internship.Id))
        {
            positionDetails[internship.Id] = !positionDetails[internship.Id];
        }
        else
        {
            positionDetails[internship.Id] = true;
        }
    }

    private async Task DeleteInternship(int internshipId)
    {
        // Call JavaScript function for confirmation with HTML content and custom styling
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
            "Πρόκειται να διαγράψετε οριστικά αυτή την Θέση Πρακτικής. Είστε σίγουρος/η; <br>" +
            "<strong style='color: red;'>Η ενέργεια αυτή είναι μη αναστρέψιμη!</strong>"
        });

        if (isConfirmed)
        {
            var internship = await dbContext.CompanyInternships.FindAsync(internshipId);
            if (internship != null)
            {
                dbContext.CompanyInternships.Remove(internship);
                await dbContext.SaveChangesAsync();
                await LoadInternships(); // Reload the internships list
            }
        }
        StateHasChanged();
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }


    private async Task DeleteProfessorInternship(int internshipId)
    {
        var internship = await dbContext.ProfessorInternships.FindAsync(internshipId);
        if (internship != null)
        {
            dbContext.ProfessorInternships.Remove(internship);
            await dbContext.SaveChangesAsync();
            await LoadProfessorInternships(); // Reload the internships list
        }
    }

    private async Task LoadInternships()
    {
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (!string.IsNullOrEmpty(userEmail))
            {
                // Updated query to use new property name and include Company data
                internships = await dbContext.CompanyInternships
                    .Include(i => i.Company)  // Include company data
                    .Where(i => i.CompanyEmailUsedToUploadInternship == userEmail)  // Updated property name
                    .ToListAsync();

                // Check the loaded data
                if (internships == null || !internships.Any())
                {
                    Console.WriteLine("No internships found for this user.");
                }
                else
                {
                    // Example of accessing company data through navigation property
                    foreach (var internship in internships)
                    {
                        Console.WriteLine($"Internship {internship.Id} by {internship.Company?.CompanyName}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading internships: {ex.Message}");
        }
    }

    private async Task LoadProfessorInternships()
    {
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (!string.IsNullOrEmpty(userEmail))
            {
                // Fetch internships related to the current professor using the new property name
                professorInternships = await dbContext.ProfessorInternships
                    .Include(i => i.Professor) // Include professor navigation property
                    .Where(i => i.ProfessorEmailUsedToUploadInternship == userEmail)
                    .ToListAsync();

                // Check the loaded data
                if (professorInternships == null || !professorInternships.Any())
                {
                    Console.WriteLine("No internships found for this professor.");
                }
                else
                {
                    Console.WriteLine($"Found {professorInternships.Count} internships for professor {userEmail}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading professor internships: {ex.Message}");
        }
    }

    private async Task LoadJobs()
    {
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (!string.IsNullOrEmpty(userEmail))
            {
                // Fetch internships related to the current company
                jobs = await dbContext.CompanyJobs
                    .Where(i => i.EmailUsedToUploadJobs == userEmail)
                    .ToListAsync();

                // Check the loaded data
                if (jobs == null || !jobs.Any())
                {
                    Console.WriteLine("No internships found for this user.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading internships: {ex.Message}");
        }
        LogJobLoadingInfo();

    }

    private async Task LoadThesesAsCompany()
    {
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (!string.IsNullOrEmpty(userEmail))
            {
                // Fetch theses related to the current company
                companytheses = await dbContext.CompanyTheses
                    .Include(t => t.Company) // This ensures Company data is loaded
                    .Where(i => i.CompanyEmailUsedToUploadThesis == userEmail)
                    .ToListAsync();

                // Check the loaded data
                if (companytheses == null || !companytheses.Any())
                {
                    Console.WriteLine("No theses found for this company.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading company theses: {ex.Message}");
        }
    }

    private async Task LoadThesesAsProfessor()
    {
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (!string.IsNullOrEmpty(userEmail))
            {
                // Fetch theses related to the current professor using the navigation property
                professortheses = await dbContext.ProfessorTheses
                    .Include(t => t.Professor) // Include professor data if needed
                    .Where(t => t.ProfessorEmailUsedToUploadThesis == userEmail)
                    .ToListAsync();

                // Check the loaded data
                if (professortheses == null || !professortheses.Any())
                {
                    Console.WriteLine("No theses found for this Professor.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading theses for this Professor: {ex.Message}");
        }

        // Load filtered theses (if needed for other purposes)
        FilteredThesesAsProfessor = await dbContext.ProfessorTheses
            .Include(t => t.Professor) // Include professor data if needed
            .ToListAsync();
    
        StateHasChanged();
    }


    private async Task LoadProfessors()
    {
        try
        {
            // Fetch all professors from the database
            professors = await dbContext.Professors
            .AsNoTracking() //to avala 26/9 gia tin vasi multi threading
            .ToListAsync();

            // Check the loaded data
            if (professors == null || !professors.Any())
            {
                Console.WriteLine("No professors found.");
            }
            else
            {
                Console.WriteLine($"Found {professors.Count} professors.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }

    private async Task LoadCompanies()
    {
        try
        {
            // Fetch all professors from the database
            companies = await dbContext.Companies
            .AsNoTracking() //to avala 26/9 gia tin vasi multi threading
            .ToListAsync();

            // Check the loaded data
            if (companies == null || !companies.Any())
            {
                Console.WriteLine("No companies found.");
            }
            else
            {
                Console.WriteLine($"Found {companies.Count} companies.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }


    private bool isModalVisibleForInternships = false;
    private bool isModalVisibleForInternshipsAsStudent = false;
    private bool isModalVisibleForProfessorInternshipsAsStudent = false;
    private CompanyInternship currentInternship = null;
    private bool isModalVisibleForProfessorInternships = false;
    private ProfessorInternship currentProfessorInternship = null;

    private void ShowInternshipDetails(CompanyInternship internship)
    {
        currentInternship = internship;
        isModalVisibleForInternships = true;

    }


    private void ShowProfessorInternshipDetails(ProfessorInternship professorinternship)
    {
        currentProfessorInternship = professorinternship;
        isModalVisibleForProfessorInternships = true;

    }

    private void CloseModalForInternships()
    {
        isModalVisibleForInternships = false;
        selectedCompanyInternshipDetails = null;
    }

    private void CloseModalForProfessorInternships()
    {
        isModalVisibleForProfessorInternships = false;
        currentProfessorInternship = null;
    }

    // Method to show the edit popup with selected internship details
    private void EditInternshipDetails(CompanyInternship internship)
    {
        selectedInternship = internship;
        isEditPopupVisibleForInternships = true; // Show the edit popup
    }

    private void EditProfessorInternshipDetails(ProfessorInternship internship)
    {
        selectedProfessorInternship = internship;
        isEditPopupVisibleForProfessorInternships = true; // Show the edit popup
    }

    // Method to close the edit popup
    private void CloseEditPopupForInternships()
    {
        isEditPopupVisibleForInternships = false; // Hide the edit popup
    }

    // Method to save the edited internship details
    private async Task SaveEditedInternship()
    {
        try
        {
            // Check if required fields are filled
            if (string.IsNullOrWhiteSpace(selectedInternship.CompanyInternshipTitle) || string.IsNullOrWhiteSpace(selectedInternship.CompanyInternshipDescription))
            {
                showSuccessMessage = false;
                showErrorMessage = true;
                return; // Prevent saving if required fields are missing
            }

            // Ensure that SelectedAreasToEditForCompanyInternship contains the areas (even if no changes were made)
            if (SelectedAreasToEditForCompanyInternship == null || !SelectedAreasToEditForCompanyInternship.Any())
            {
                // If no areas are selected, you can set them based on the internship's current areas
                var currentAreas = selectedInternship.CompanyInternshipAreas.Split(",").ToList();
                SelectedAreasToEditForCompanyInternship = Areas
                    .Where(area => currentAreas.Contains(area.AreaName)) // Set the areas to the ones already selected
                    .ToList();
            }

            // Convert the selected areas to a comma-separated string
            selectedInternship.CompanyInternshipAreas = string.Join(",", SelectedAreasToEditForCompanyInternship.Select(area => area.AreaName));

            // Find the internship to update
            var internshipToUpdate = await dbContext.CompanyInternships.FindAsync(selectedInternship.Id);
            if (internshipToUpdate != null)
            {
                // Update internship properties
                internshipToUpdate.CompanyInternshipTitle = selectedInternship.CompanyInternshipTitle;
                internshipToUpdate.CompanyInternshipDescription = selectedInternship.CompanyInternshipDescription;
                internshipToUpdate.CompanyUploadedInternshipStatus = selectedInternship.CompanyUploadedInternshipStatus;
                internshipToUpdate.CompanyInternshipType = selectedInternship.CompanyInternshipType;
                internshipToUpdate.CompanyInternshipForeas = selectedInternship.CompanyInternshipForeas;
                internshipToUpdate.CompanyInternshipContactPerson = selectedInternship.CompanyInternshipContactPerson;
                internshipToUpdate.CompanyInternshipPerifereiaLocation = selectedInternship.CompanyInternshipPerifereiaLocation;
                internshipToUpdate.CompanyInternshipDimosLocation = selectedInternship.CompanyInternshipDimosLocation;
                internshipToUpdate.CompanyInternshipPostalCodeLocation = selectedInternship.CompanyInternshipPostalCodeLocation;
                internshipToUpdate.CompanyInternshipTransportOffer = selectedInternship.CompanyInternshipTransportOffer;
                internshipToUpdate.CompanyInternshipAreas = selectedInternship.CompanyInternshipAreas;
                internshipToUpdate.CompanyInternshipActivePeriod = selectedInternship.CompanyInternshipActivePeriod;
                internshipToUpdate.CompanyInternshipEKPASupervisor = selectedInternship.CompanyInternshipEKPASupervisor;
                internshipToUpdate.CompanyInternshipLastUpdate = selectedInternship.CompanyInternshipLastUpdate;

                // Only update CompanyInternshipAttachment if a new file was uploaded
                if (selectedInternship.CompanyInternshipAttachment != null && selectedInternship.CompanyInternshipAttachment.Length > 0)
                {
                    internshipToUpdate.CompanyInternshipAttachment = selectedInternship.CompanyInternshipAttachment;
                }

                // Save the changes to the database
                await dbContext.SaveChangesAsync();
                showSuccessMessage = true;
                showErrorMessage = false;
            }
            else
            {
                showSuccessMessage = false;
                showErrorMessage = true;
            }
        }
        catch (Exception ex)
        {
            showSuccessMessage = false;
            showErrorMessage = true;
            Console.Error.WriteLine($"Error saving internship: {ex.Message}");
        }
        finally
        {
            isEditPopupVisibleForInternships = false;
            StateHasChanged(); // Ensure the UI updates after saving
        }
    }




  

    private async Task ShowJobDetails(CompanyJob job)
    {
        if (job.Company == null)
        {
            await dbContext.Entry(job)
                .Reference(j => j.Company)
                .LoadAsync();
        }
    
        currentJob = job;
        isModalVisibleForJobs = true;
        StateHasChanged();
    }

    private void ShowProfessorThesisDetailsAsStudent(ProfessorThesis professorthesis)
    {
        currentProfessorThesis = professorthesis;
        isModalVisibleToShowProfessorThesisDetails = true;

    }

   private void ShowProfessorThesisDetailsAsProfessor(ProfessorThesis professorthesis)
   {
        currentProfessorThesis = professorthesis;
        isModalVisibleToShowProfessorThesisAsProfessor = true;
   }





    private void CloseModalForJobs()
    {
        isModalVisibleForJobs = false;
        currentJob = null;
    }

    private void EditJobDetails(CompanyJob job)
    {
        selectedJob = job;
        isEditPopupVisibleForJobs = true; // Show the edit popup
    }

    private void CloseEditPopupForJobs()
    {
        isEditPopupVisibleForJobs = false; // Hide the edit popup
    }


    private async Task SaveEditedJob()
    {
        try
        {
            // Check if required fields are filled
            if (string.IsNullOrWhiteSpace(selectedJob.PositionTitle) || string.IsNullOrWhiteSpace(selectedJob.PositionDescription))
            {
                showSuccessMessage = false;
                showErrorMessage = true;
                return; // Prevent saving if required fields are missing
            }

            // Ensure SelectedAreasToEditForCompanyJob contains all checked areas (even if no changes were made)
            if (SelectedAreasToEditForCompanyJob == null || !SelectedAreasToEditForCompanyJob.Any())
            {
                SelectedAreasToEditForCompanyJob = Areas.Where(area => selectedJob.PositionAreas.Contains(area.AreaName)).ToList();  // Fetch areas if none are selected
            }

            // Convert the selected areas to a comma-separated string
            selectedJob.PositionAreas = string.Join(",", SelectedAreasToEditForCompanyJob.Select(area => area.AreaName));  

            // Update job in database
            var jobToUpdate = await dbContext.CompanyJobs.FindAsync(selectedJob.Id);
            if (jobToUpdate != null)
            {
                jobToUpdate.PositionTitle = selectedJob.PositionTitle;
                jobToUpdate.PositionDescription = selectedJob.PositionDescription;
                jobToUpdate.PositionStatus = selectedJob.PositionStatus;
                jobToUpdate.PositionType = selectedJob.PositionType;
                jobToUpdate.PositionForeas = selectedJob.PositionForeas;
                jobToUpdate.PositionContactPerson = selectedJob.PositionContactPerson;
                jobToUpdate.PositionPerifereiaLocation = selectedJob.PositionPerifereiaLocation;
                jobToUpdate.PositionDimosLocation = selectedJob.PositionDimosLocation;
                jobToUpdate.PositionPostalCodeLocation = selectedJob.PositionPostalCodeLocation;
                jobToUpdate.PositionTransportOffer = selectedJob.PositionTransportOffer;
                jobToUpdate.PositionAreas = selectedJob.PositionAreas;
                jobToUpdate.PositionActivePeriod = selectedJob.PositionActivePeriod;
                jobToUpdate.UpdateDateTime = selectedJob.UpdateDateTime;

                if (selectedJob.PositionAttachment != null && selectedJob.PositionAttachment.Length > 0)
                {
                    jobToUpdate.PositionAttachment = selectedJob.PositionAttachment;
                }

                await dbContext.SaveChangesAsync();
                showSuccessMessage = true;
                showErrorMessage = false;
            }
            else
            {
                showSuccessMessage = false;
                showErrorMessage = true;
            }
        }
        catch (Exception ex)
        {
            showSuccessMessage = false;
            showErrorMessage = true;
            Console.Error.WriteLine($"Error saving job: {ex.Message}");
        }
        finally
        {
            isEditPopupVisibleForJobs = false;
            StateHasChanged();
        }
    }








    private void ShowCompanyThesisDetails(CompanyThesis companythesis)
    {
        currentThesis = companythesis;
        isModalVisibleToShowCompanyThesisDetails = true;

    }
    private void EditCompanyThesisDetails(CompanyThesis companythesis)
    {
        selectedCompanyThesis = companythesis;
        isModalVisibleToEditCompanyThesisDetails = true; // Show the edit popup
    }
    private void CloseModalForCompanyThesis()
    {
        isModalVisibleToShowCompanyThesisDetails = false;
        currentThesis = null;
    }

    private void CloseModalForProfessorDetails()
    {
        isModalVisibleToShowprofessorDetailsAtCompanyThesisInterest = false;
        currentProfessorDetails = null;
    }




    private async Task ShowCompanyThesisApplicationsAsStudent(CompanyThesisApplied thesis)
    {
        selectedCompanyThesisApplicationToShowAsStudent = thesis;
        await JS.InvokeVoidAsync("showModal", "thesisDetailsModal");
    }

    private async Task ShowProfessorThesisApplicationsAsStudent(ProfessorThesisApplied thesis)
    {
        selectedProfessorThesisApplicationToShowAsStudent = thesis;
        await JS.InvokeVoidAsync("showModal", "thesisDetailsModal");
    }

    private async Task ConfirmApplyForInternship(CompanyInternship internship)
    {
        var message = $"Πρόκεται να κάνετε αίτηση για την Θέση \"{internship.CompanyInternshipTitle}\". Είστε σίγουρος/η;";
    
        var confirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", message);

        if (confirmed)
        {
            await ApplyForInternshipAsStudent(internship);
        }
    }

    private async Task ApplyForInternshipAsStudent(CompanyInternship internship)
    {
        // Retrieve the latest internship status using updated property name
        var latestInternship = await dbContext.CompanyInternships
            .AsNoTracking()
            .Where(i => i.RNGForInternshipUploadedAsCompany == internship.RNGForInternshipUploadedAsCompany)
            .Select(i => new { i.CompanyUploadedInternshipStatus })
            .FirstOrDefaultAsync();

        if (latestInternship == null || latestInternship.CompanyUploadedInternshipStatus != "Δημοσιευμένη")
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Η Πρακτική Άσκηση δεν είναι πλέον διαθέσιμη. Παρακαλώ δοκιμάστε αργότερα.");
            return;
        }

        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (!user.Identity.IsAuthenticated) return;

        var student = await GetStudentDetails(user.Identity.Name);
        if (student == null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Δεν βρέθηκαν στοιχεία φοιτητή.");
            return;
        }

        // Check for existing application using updated property name
        var existingApplication = await dbContext.InternshipsApplied
            .FirstOrDefaultAsync(app =>
                app.StudentEmailAppliedForInternship == student.Email &&
                app.RNGForInternshipApplied == internship.RNGForInternshipUploadedAsCompany);

        if (existingApplication != null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", $"Έχετε ήδη κάνει αίτηση για: {internship.CompanyInternshipTitle}!");
            return;
        }

        // Get company data using updated property name
        var company = await dbContext.Companies
            .FirstOrDefaultAsync(c => c.CompanyEmail == internship.CompanyEmailUsedToUploadInternship);

        if (company == null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Σφάλμα: Δεν βρέθηκε η εταιρία.");
            return;
        }

        using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var internshipApplication = new InternshipApplied
            {
                RNGForInternshipApplied = internship.RNGForInternshipUploadedAsCompany,
                DateTimeStudentAppliedForInternship = DateTime.UtcNow,
                StudentEmailAppliedForInternship = student.Email,
                StudentUniqueIDAppliedForInternship = student.Student_UniqueID,
                CompanyEmailWhereStudentAppliedForInternship = internship.CompanyEmailUsedToUploadInternship,
                CompanyUniqueIDWhereStudentAppliedForInternship = company.Company_UniqueID,
                InternshipStatusAppliedAtTheCompanySide = "Σε Επεξεργασία",
                InternshipStatusAppliedAtTheStudentSide = "Σε Επεξεργασία",
                RNGForInternshipAppliedAsStudent_HashedAsUniqueID = internship.RNGForInternshipUploadedAsCompany_HashedAsUniqueID,

                StudentDetails = new InternshipApplied_StudentDetails
                {
                    StudentEmailAppliedForInternship = student.Email,
                    StudentUniqueIDAppliedForInternship = student.Student_UniqueID,
                    DateTimeStudentAppliedForInternship = DateTime.UtcNow,
                    RNGForInternshipAppliedAsStudent_HashedAsUniqueID = internship.RNGForInternshipUploadedAsCompany_HashedAsUniqueID
                },

                CompanyDetails = new InternshipApplied_CompanyDetails
                {
                    CompanyUniqueIDWhereStudentAppliedForInternship = company.Company_UniqueID,
                    CompanyEmailWhereStudentAppliedForInternship = internship.CompanyEmailUsedToUploadInternship
                }
            };

            dbContext.InternshipsApplied.Add(internshipApplication);

            // Add platform action using updated property name
            dbContext.PlatformActions.Add(new PlatformActions
            {
                UserRole_PerformedAction = "STUDENT",
                ForWhat_PerformedAction = "COMPANY_INTERNSHIP",
                HashedPositionRNG_PerformedAction = HashingHelper.HashLong(internship.RNGForInternshipUploadedAsCompany),
                TypeOfAction_PerformedAction = "APPLY",
                DateTime_PerformedAction = DateTime.UtcNow
            });

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            // Send emails - use company name from navigation property
            try
            {
                await InternshipEmailService.SendCompanyInternshipApplicationConfirmationToStudent(
                    student.Email, student.Name, student.Surname,
                    internship.CompanyInternshipTitle, 
                    internship.RNGForInternshipUploadedAsCompany_HashedAsUniqueID, 
                    internship.Company?.CompanyName ?? "Unknown Company");

                await InternshipEmailService.SendInternshipApplicationNotificationToCompany(
                    internship.CompanyEmailUsedToUploadInternship, 
                    internship.Company?.CompanyName ?? "Unknown Company",
                    student.Name, student.Surname, student.Email,
                    student.Telephone, student.StudyYear, student.Attachment,
                    internship.RNGForInternshipUploadedAsCompany_HashedAsUniqueID, 
                    internship.CompanyInternshipTitle);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
            }

            await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Η αίτηση για {internship.CompanyInternshipTitle} υποβλήθηκε επιτυχώς!");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Full error: {ex}");
            await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Σφάλμα κατά την υποβολή: {ex.InnerException?.Message ?? ex.Message}");
        }

        await RefreshStudentData();
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }


    private async Task ConfirmApplyForProfessorInternship(ProfessorInternship internship)
    {
        var message = $"Πρόκεται να κάνετε αίτηση για την Θέση \"{internship.ProfessorInternshipTitle}\". Είστε σίγουρος/η;";
    
        var confirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", message);

        if (confirmed)
        {
            await ApplyForProfessorInternshipAsStudent(internship);
        }
    }

    private async Task ApplyForProfessorInternshipAsStudent(ProfessorInternship internship)
    {
        // Retrieve the latest internship status using the new property name
        var latestInternship = await dbContext.ProfessorInternships
            .AsNoTracking()
            .Where(i => i.RNGForInternshipUploadedAsProfessor == internship.RNGForInternshipUploadedAsProfessor)
            .Select(i => new { i.ProfessorUploadedInternshipStatus })
            .FirstOrDefaultAsync();

        if (latestInternship == null || latestInternship.ProfessorUploadedInternshipStatus != "Δημοσιευμένη")
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Η Πρακτική Άσκηση δεν είναι πλέον διαθέσιμη. Παρακαλώ δοκιμάστε αργότερα.");
            return;
        }

        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (!user.Identity.IsAuthenticated) return;

        var student = await GetStudentDetails(user.Identity.Name);
        if (student == null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Δεν βρέθηκαν στοιχεία φοιτητή.");
            return;
        }

        // Check for existing application using new property names
        var existingApplication = await dbContext.ProfessorInternshipsApplied
            .FirstOrDefaultAsync(app =>
                app.StudentEmailAppliedForProfessorInternship == student.Email &&
                app.RNGForProfessorInternshipApplied == internship.RNGForInternshipUploadedAsProfessor);

        if (existingApplication != null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", $"Έχετε ήδη κάνει αίτηση για: {internship.ProfessorInternshipTitle}!");
            return;
        }

        // Get professor data using the new property name
        var professor = await dbContext.Professors
            .FirstOrDefaultAsync(p => p.ProfEmail == internship.ProfessorEmailUsedToUploadInternship);

        if (professor == null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Σφάλμα: Δεν βρέθηκε ο καθηγητής.");
            return;
        }

        using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var professorInternshipApplication = new ProfessorInternshipApplied
            {
                RNGForProfessorInternshipApplied = internship.RNGForInternshipUploadedAsProfessor,
                DateTimeStudentAppliedForProfessorInternship = DateTime.UtcNow,
                StudentEmailAppliedForProfessorInternship = student.Email,
                StudentUniqueIDAppliedForProfessorInternship = student.Student_UniqueID,
                ProfessorEmailWhereStudentAppliedForInternship = internship.ProfessorEmailUsedToUploadInternship,
                ProfessorUniqueIDWhereStudentAppliedForInternship = professor.Professor_UniqueID,
                InternshipStatusAppliedAtTheProfessorSide = "Σε Επεξεργασία",
                InternshipStatusAppliedAtTheStudentSide = "Σε Επεξεργασία",
                RNGForProfessorInternshipApplied_HashedAsUniqueID = internship.RNGForInternshipUploadedAsProfessor_HashedAsUniqueID,

                StudentDetails = new ProfessorInternshipsApplied_StudentDetails
                {
                    StudentUniqueIDAppliedForProfessorInternship = student.Student_UniqueID,
                    StudentEmailAppliedForProfessorInternship = student.Email,
                    DateTimeStudentAppliedForProfessorInternship = DateTime.UtcNow,
                    RNGForProfessorInternshipAppliedAsStudent_HashedAsUniqueID = internship.RNGForInternshipUploadedAsProfessor_HashedAsUniqueID
                },

                ProfessorDetails = new ProfessorInternshipsApplied_ProfessorDetails
                {
                    ProfessorUniqueIDWhereStudentAppliedForProfessorInternship = professor.Professor_UniqueID,
                    ProfessorEmailWhereStudentAppliedForProfessorInternship = internship.ProfessorEmailUsedToUploadInternship
                }
            };

            dbContext.ProfessorInternshipsApplied.Add(professorInternshipApplication);

            // Add platform action with updated property names
            dbContext.PlatformActions.Add(new PlatformActions
            {
                UserRole_PerformedAction = "STUDENT",
                ForWhat_PerformedAction = "PROFESSOR_INTERNSHIP",
                HashedPositionRNG_PerformedAction = HashingHelper.HashLong(internship.RNGForInternshipUploadedAsProfessor),
                TypeOfAction_PerformedAction = "APPLY",
                DateTime_PerformedAction = DateTime.UtcNow
            });

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            // Send emails using professor details from navigation property
            try
            {
                await InternshipEmailService.SendProfessorInternshipApplicationConfirmationToStudent(
                    student.Email,
                    student.Name,
                    student.Surname,
                    internship.ProfessorInternshipTitle,
                    internship.RNGForInternshipUploadedAsProfessor_HashedAsUniqueID,
                    $"{professor.ProfName} {professor.ProfSurname}");

                await InternshipEmailService.SendProfessorInternshipApplicationNotificationToProfessor(
                    internship.ProfessorEmailUsedToUploadInternship,
                    $"{professor.ProfName} {professor.ProfSurname}",
                    student.Name,
                    student.Surname,
                    student.Email,
                    student.Telephone,
                    student.StudyYear,
                    student.Attachment,
                    internship.RNGForInternshipUploadedAsProfessor_HashedAsUniqueID,
                    internship.ProfessorInternshipTitle);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
            }

            await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Η αίτηση για {internship.ProfessorInternshipTitle} υποβλήθηκε επιτυχώς!");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Full error: {ex.ToString()}");
            await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Σφάλμα κατά την υποβολή: {ex.InnerException?.Message ?? ex.Message}");
        }

        await RefreshStudentData();
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }


    private async Task ConfirmApplyForJob(CompanyJob job)
    {
        var message = $"Πρόκεται να κάνετε αίτηση για την Θέση \"{job.PositionTitle}\".Είστε σίγουρος/η;";
        var confirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", message);
    
        if (confirmed)
        {
            await ApplyForJobAsStudent(job);
        }
    }


    private async Task ToggleAndLoadStudentInternshipApplications()
    {
        showStudentInternshipApplications = !showStudentInternshipApplications;

        if (showStudentInternshipApplications)
        {
            await LoadUserInternshipApplications();
        }

        StateHasChanged(); // Ensure UI updates after toggling
    }


    private async Task LoadUserInternshipApplications()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        // Initialize lists and caches
        internshipApplications = new List<InternshipApplied>();
        professorInternshipApplications = new List<ProfessorInternshipApplied>();
        internshipDataCache = new Dictionary<long, CompanyInternship>();
        professorInternshipDataCache = new Dictionary<long, ProfessorInternship>();

        if (user.Identity.IsAuthenticated)
        {
            var userEmail = user.FindFirst("name")?.Value;

            if (!string.IsNullOrEmpty(userEmail))
            {
                // Get student details
                var student = await dbContext.Students.FirstOrDefaultAsync(s => s.Email == userEmail);

                if (student != null)
                {
                    // Fetch company internship applications with details (unchanged)
                    internshipApplications = await dbContext.InternshipsApplied
                        .Include(app => app.StudentDetails)
                        .Include(app => app.CompanyDetails)
                        .Where(app => app.StudentDetails.StudentUniqueIDAppliedForInternship == student.Student_UniqueID)
                        .ToListAsync();

                    // Load all related company internships in one query
                    var companyInternshipRNGs = internshipApplications.Select(a => a.RNGForInternshipApplied).ToList();
                    var companyInternships = await dbContext.CompanyInternships
                        .Include(i => i.Company)
                        .Where(i => companyInternshipRNGs.Contains(i.RNGForInternshipUploadedAsCompany))
                        .ToListAsync();

                    // Populate company internship cache
                    foreach (var internship in companyInternships)
                    {
                        internshipDataCache[internship.RNGForInternshipUploadedAsCompany] = internship;
                    }

                    // Fetch professor internship applications with details
                    professorInternshipApplications = await dbContext.ProfessorInternshipsApplied
                        .Include(app => app.StudentDetails)
                        .Include(app => app.ProfessorDetails)
                        .Where(app => app.StudentDetails.StudentUniqueIDAppliedForProfessorInternship == student.Student_UniqueID)
                        .ToListAsync();

                    // Load all related professor internships in one query with updated property names
                    var professorInternshipRNGs = professorInternshipApplications.Select(a => a.RNGForProfessorInternshipApplied).ToList();
                    var professorInternships = await dbContext.ProfessorInternships
                        .Include(i => i.Professor) // Include professor navigation property
                        .Where(i => professorInternshipRNGs.Contains(i.RNGForInternshipUploadedAsProfessor)) // Updated property
                        .ToListAsync();

                    // Populate professor internship cache with updated property names
                    foreach (var internship in professorInternships)
                    {
                        professorInternshipDataCache[internship.RNGForInternshipUploadedAsProfessor] = internship;
                    }
                }
            }
        }

        StateHasChanged();
    }




    private void ShowJobDetails(CompanyJobApplied jobApplication)
    {
        selectedJobApplication = jobApplication;
        ShowJobDetailsModal();
    }

    private async void ShowJobDetailsModal()
    {
        await JS.InvokeVoidAsync("ShowBootstrapModal", "#jobDetailsModal");
    }

    public class CompanyInternshipAreasForCheckboxes
    {
        public List<Area> CompanyInternshipAreas { get; set; } = new List<Area>();
    }

    private bool IsSelectedAreasWhenUploadJobAsCompany(Area area)
    {
        return SelectedAreasWhenUploadJobAsCompany.Contains(area);
    }

    private bool IsSelectedAreasWhenUploadThesisAsCompany(Area area)
    {
        return SelectedAreasWhenUploadThesisAsCompany.Contains(area);
    }

    private bool IsSelectedAreasWhenUploadInternshipAsCompany(Area area)
    {
        return SelectedAreasWhenUploadInternshipAsCompany.Contains(area);
    }

    private bool IsSelectedAreaForCompanyEvent(Area area)
    {
        return SelectedAreasWhenUploadEventAsCompany.Contains(area);
    }

    private bool IsSelectedAreaForProfessorEvent(Area area)
    {
        return SelectedAreasWhenUploadEventAsProfessor.Contains(area);
    }

    private bool IsSelectedAreaForProfessorInternship(Area area)
    {
        return SelectedAreasWhenUploadInternshipAsProfessor.Contains(area);
    }

    private bool IsSelectedForSkillsWhenUploadThesisAsCompany(Skill skill)
    {
        return SelectedSkillsWhenUploadThesisAsCompany.Contains(skill);
    }

    private void OnCheckedChangedAreasWhenUploadJobAsCompany(ChangeEventArgs e, Area area)
    {
        if (e.Value is bool isChecked)
        {
            if (isChecked)
            {
                if (!SelectedAreasWhenUploadJobAsCompany.Contains(area))
                {
                    SelectedAreasWhenUploadJobAsCompany.Add(area);
                }
            }
            else
            {
                SelectedAreasWhenUploadJobAsCompany.Remove(area);
            }

            if (SelectedAreasWhenUploadJobAsCompany.Any())
            {
                showErrorMessagesForAreasWhenUploadJobAsCompany = false;
            }

        }
    }

    private void OnCheckedChangedAreasWhenUploadInternshipAsCompany(ChangeEventArgs e, Area area)
    {
        if (e.Value is bool isChecked)
        {
            if (isChecked)
            {
                if (!SelectedAreasWhenUploadInternshipAsCompany.Contains(area))
                {
                    SelectedAreasWhenUploadInternshipAsCompany.Add(area);
                }
            }
            else
            {
                SelectedAreasWhenUploadInternshipAsCompany.Remove(area);
            }

            if (SelectedAreasWhenUploadInternshipAsCompany.Any())
            {
                showErrorMessagesForAreasWhenUploadInternshipAsCompany = false;
            }

        }
    }

    private void OnCheckedChangedAreasWhenUploadThesisAsCompany(ChangeEventArgs e, Area area)
    {
        if (e.Value is bool isChecked)
        {
            if (isChecked)
            {
                if (!SelectedAreasWhenUploadThesisAsCompany.Contains(area))
                {
                    SelectedAreasWhenUploadThesisAsCompany.Add(area);
                }
            }
            else
            {
                SelectedAreasWhenUploadThesisAsCompany.Remove(area);
            }

            if (SelectedAreasWhenUploadThesisAsCompany.Any())
            {
                showErrorMessage = false;
            }

        }
    }

    private void OnCheckedChangedAreasWhenUploadInternshipAsProfessor(ChangeEventArgs e, Area area)
    {
        if (e.Value is bool isChecked)
        {
            if (isChecked)
            {
                if (!SelectedAreasWhenUploadInternshipAsProfessor.Contains(area))
                {
                    SelectedAreasWhenUploadInternshipAsProfessor.Add(area);
                }
            }
            else
            {
                SelectedAreasWhenUploadInternshipAsProfessor.Remove(area);
            }

            if (SelectedAreasWhenUploadInternshipAsProfessor.Any())
            {
                showErrorMessage = false;
            }

        }
    }


    private void OnCheckedChangedForCompanyEvent(ChangeEventArgs e, Area area)
    {
        if (e.Value is bool isChecked)
        {
            if (isChecked)
            {
                if (!SelectedAreasWhenUploadEventAsCompany.Contains(area))
                {
                    SelectedAreasWhenUploadEventAsCompany.Add(area);
                }
            }
            else
            {
                SelectedAreasWhenUploadEventAsCompany.Remove(area);
            }
        }
    }

    private void OnCheckedChangedForProfessorEvent(ChangeEventArgs e, Area area)
    {
        if (e.Value is bool isChecked)
        {
            if (isChecked)
            {
                if (!SelectedAreasWhenUploadEventAsProfessor.Contains(area))
                {
                    SelectedAreasWhenUploadEventAsProfessor.Add(area);
                }
            }
            else
            {
                SelectedAreasWhenUploadEventAsProfessor.Remove(area);
            }
        }
    }

    private void OnCheckedChangedForSkillsWhenUploadThesisAsCompany(ChangeEventArgs e, Skill skill)
    {
        if (e.Value is bool isChecked)
        {
            if (isChecked)
            {
                if (!SelectedSkillsWhenUploadThesisAsCompany.Contains(skill))
                {
                    SelectedSkillsWhenUploadThesisAsCompany.Add(skill);
                }
            }
            else
            {
                SelectedSkillsWhenUploadThesisAsCompany.Remove(skill);
            }
            if (SelectedSkillsWhenUploadThesisAsCompany.Any())
            {
                showErrorMessagesForSkillsWhenUploadThesisAsCompany = false;
            }

        }
    }

    private async Task ToggleCheckboxesForCompanyInternship()
    {
        await JS.InvokeVoidAsync("toggleCompanyInternshipCheckboxes");
    }

    private async Task ToggleCheckboxesForProfessorInternship()
    {
        await JS.InvokeVoidAsync("toggleProfessorInternshipCheckboxes");
    }

    private async Task ToggleCheckboxesForAreasForCompanyThesis()
    {
        await JS.InvokeVoidAsync("toggleCompanyThesisAreasCheckboxes");
    }

    private async Task ToggleCheckboxesForAreasForCompanyEvent()
    {
        await JS.InvokeVoidAsync("toggleCompanyEventAreasCheckboxes");
    }

    private async Task ToggleCheckboxesForAreasForProfessorEvent()
    {
        await JS.InvokeVoidAsync("toggleProfessorEventAreasCheckboxes");
    }

    private async Task ToggleCheckboxesForSkillsForCompanyThesis()
    {
        await JS.InvokeVoidAsync("toggleCompanyThesisSkillsCheckboxes");
    }

    private async Task ToggleCheckboxesForCompanyJob()
    {
        await JS.InvokeVoidAsync("toggleCompanyJobCheckboxes");
    }

   private async Task UpdateInternshipStatusAsCompany(int internshipId, string newStatus)
    {
        // Skip confirmation dialog if the new status is "Μη Δημοσιευμένη"
        if (newStatus != "Μη Δημοσιευμένη")
        {
            bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
                $"Πρόκειται να αλλάξετε την κατάσταση της Θέσης Πρακτικής σε '{newStatus}'. Είστε σίγουρος/η; <br>"
            });

            if (!isConfirmed)
                return;
        }

        // Retrieve the internship from the database
        var internship = await dbContext.CompanyInternships
            .FirstOrDefaultAsync(i => i.Id == internshipId);

        if (internship != null)
        {
            // Update the status
            internship.CompanyUploadedInternshipStatus = newStatus;

            // If the internship status is "Αποσυρμένη", update student applications
            if (newStatus == "Αποσυρμένη")
            {
                var rngForInternship = internship.RNGForInternshipUploadedAsCompany; // Updated property name

                // Retrieve all student applications for this internship
                var studentApplications = await dbContext.InternshipsApplied
                    .Where(a => a.RNGForInternshipApplied == rngForInternship)
                    .ToListAsync();

                // Update the status for each student application
                foreach (var application in studentApplications)
                {
                    application.InternshipStatusAppliedAtTheCompanySide = "Απορρίφθηκε (Απόσυρση Θέσεως Από Εταιρία)";
                    application.InternshipStatusAppliedAtTheStudentSide = "Απορρίφθηκε (Απόσυρση Θέσεως Από Εταιρία)";
                }
            }

            // Save all changes
            await dbContext.SaveChangesAsync();

            // Reload data and refresh UI
            await LoadInternships();
            var tabUrl = $"{NavigationManager.Uri.Split('?')[0]}#internships";
            NavigationManager.NavigateTo(tabUrl, true);
            await Task.Delay(500);
            await JS.InvokeVoidAsync("activateTab", "internships");
        }
    }



    private async Task UpdateInternshipStatusAsProfessor(int internshipId, string newStatus)
    {
        // Retrieve the internship from the database
        var internship = await dbContext.ProfessorInternships
            .FirstOrDefaultAsync(i => i.Id == internshipId);

        if (internship != null)
        {
            // Update the status
            internship.ProfessorUploadedInternshipStatus = newStatus;

            // If the internship status is "Αποσυρμένη", update student applications
            if (newStatus == "Αποσυρμένη")
            {
                var rngForInternship = internship.RNGForInternshipUploadedAsProfessor; // Updated property name

                // Retrieve all student applications for this internship
                var studentApplications = await dbContext.ProfessorInternshipsApplied
                    .Where(a => a.RNGForProfessorInternshipApplied == rngForInternship)
                    .ToListAsync();

                // Update the status for each student application
                foreach (var application in studentApplications)
                {
                    application.InternshipStatusAppliedAtTheProfessorSide = "Απορρίφθηκε (Απόσυρση Θέσεως Από Καθηγητή)";
                    application.InternshipStatusAppliedAtTheStudentSide = "Απορρίφθηκε (Απόσυρση Θέσεως Από Καθηγητή)";
                }
            }

            // Save all changes
            await dbContext.SaveChangesAsync();

            // Reload data and refresh UI
            await LoadProfessorInternships();
            var tabUrl = $"{NavigationManager.Uri.Split('?')[0]}#professor-internships";
            NavigationManager.NavigateTo(tabUrl, true);
            await Task.Delay(500);
            await JS.InvokeVoidAsync("activateTab", "professor-internships");
        }
    }


    private async Task UpdateJobStatusAsCompany(int jobId, string newStatus)
    {
        // Show confirmation dialog only if the new status is NOT "Μη Δημοσιευμένη"
        if (newStatus != "Μη Δημοσιευμένη")
        {
            var isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
                $"Πρόκειται να αλλάξετε την κατάσταση αυτής της Θέσης Εργασίας σε <strong>{newStatus}</strong>. <br><br>" +
                "<strong style='color: red;'>Είστε σίγουρος/η;</strong>"
            });

            if (!isConfirmed) return;
        }

        // Retrieve the job from the database
        var job = await dbContext.CompanyJobs
            .FirstOrDefaultAsync(i => i.Id == jobId);

        if (job != null)
        {
            // Update the job status
            job.PositionStatus = newStatus;

            // If the job status is "Αποσυρμένη", update student applications
            if (newStatus == "Αποσυρμένη")
            {
                var rngForJob = job.RNGForPositionUploaded;

                // Get all applications for this job
                var applications = await dbContext.CompanyJobsApplied
                    .Where(a => a.RNGForCompanyJobApplied == rngForJob)
                    .ToListAsync();

                foreach (var application in applications)
                {
                    // Update status directly on the main application entity
                    application.CompanyPositionStatusAppliedAtTheCompanySide = 
                        "Απορρίφθηκε (Απόσυρση Θέσεως Από Εταιρία)";
                    application.CompanyPositionStatusAppliedAtTheStudentSide = 
                        "Απορρίφθηκε (Απόσυρση Θέσεως Από Εταιρία)";
                }
            }

            await dbContext.SaveChangesAsync();
            await LoadJobs();

            // Refresh with tab focus
            var tabUrl = $"{NavigationManager.Uri.Split('?')[0]}#jobs";
            NavigationManager.NavigateTo(tabUrl, true);
            await Task.Delay(500);
            await JS.InvokeVoidAsync("activateTab", "jobs");
        }
    }




    private async Task UpdateThesisStatusAsCompany(int companythesisId, string newCompanyThesisStatus)
    {
        try
        {
            // Skip confirmation dialog if the new status is "Μη Δημοσιευμένη"
            if (newCompanyThesisStatus != "Μη Δημοσιευμένη")
            {
                bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
                    $"Πρόκειται να αλλάξετε την κατάσταση της Διπλωματικής Εργασίας σε '{newCompanyThesisStatus}'. Είστε σίγουρος/η; <br>"
                });

                if (!isConfirmed) return;
            }

            using var transaction = await dbContext.Database.BeginTransactionAsync();
        
            try
            {
                // Retrieve the thesis from the database
                var companyThesis = await dbContext.CompanyTheses
                    .FirstOrDefaultAsync(i => i.Id == companythesisId);

                if (companyThesis == null)
                {
                    await JS.InvokeVoidAsync("confirmActionWithHTML2", "Δεν βρέθηκε η διπλωματική εργασία.");
                    return;
                }

                // Update the status
                companyThesis.CompanyThesisStatus = newCompanyThesisStatus;
                await dbContext.SaveChangesAsync();

                // Handle withdrawn status
                if (newCompanyThesisStatus == "Αποσυρμένη")
                {
                    var studentApplications = await dbContext.CompanyThesesApplied
                        .Where(a => a.RNGForCompanyThesisApplied == companyThesis.RNGForThesisUploadedAsCompany)
                        .ToListAsync();

                    foreach (var application in studentApplications)
                    {
                        application.CompanyThesisStatusAppliedAtCompanySide = "Απορρίφθηκε (Απόσυρση Θέσεως Από Εταιρία)";
                        application.CompanyThesisStatusAppliedAtStudentSide = "Απορρίφθηκε (Απόσυρση Θέσεως Από Εταιρία)";
                    }

                    await dbContext.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                // Reload data and update UI
                await LoadThesesAsCompany();
                StateHasChanged();

                // Navigate to the specific tab
                var tabUrl = $"{NavigationManager.Uri.Split('?')[0]}#companythesis";
                NavigationManager.NavigateTo(tabUrl, true);
                await Task.Delay(500);
                await JS.InvokeVoidAsync("activateTab", "companythesis");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error updating thesis status: {ex}");
                await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    $"Σφάλμα κατά την ενημέρωση κατάστασης: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex}");
            await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                "Προέκυψε σφάλμα κατά την επεξεργασία της αίτησης.");
        }
    }





    private void OnAreaCheckboxChanged(ChangeEventArgs e, string areaName)
    {
        if (bool.TryParse(e.Value?.ToString(), out var isChecked))
        {
            if (isChecked)
            {
                if (!selectedAreas.Contains(areaName))
                {
                    selectedAreas.Add(areaName);
                }
            }
            else
            {
                selectedAreas.Remove(areaName);
            }
        }
    }

    private void ToggleInternshipAreasVisibility()
    {
        isInternshipAreasVisible = !isInternshipAreasVisible;
        StateHasChanged();
    }

    private async Task ToggleInternshipExpanded(long internshipId)
    {
        Console.WriteLine($"ToggleInternshipExpanded called for internship: {internshipId}");

        if (expandedInternships.ContainsKey(internshipId))
        {
            expandedInternships[internshipId] = !expandedInternships[internshipId];
        }
        else
        {
            expandedInternships[internshipId] = true;
        }

        if (expandedInternships[internshipId])
        {
            if (!internshipApplicantsMap.ContainsKey(internshipId))
            {
                internshipApplicantsMap[internshipId] = await GetApplicantsForInternships(internshipId);
                await LoadCompanyInternshipApplicantData();
            }
        }
        else
        {
            internshipApplicantsMap.Remove(internshipId);
        }

        StateHasChanged();
    }

    private async Task ToggleProfessorInternshipExpanded(long internshipId)
    {
        Console.WriteLine($"ToggleProfessorInternshipExpanded called for internship: {internshipId}");

        // Toggle expansion state
        if (expandedProfessorInternships.ContainsKey(internshipId))
        {
            expandedProfessorInternships[internshipId] = !expandedProfessorInternships[internshipId];
        }
        else
        {
            expandedProfessorInternships[internshipId] = true;
        }

        // Load data if expanding
        if (expandedProfessorInternships[internshipId])
        {
            if (!professorInternshipApplicantsMap.ContainsKey(internshipId))
            {
                professorInternshipApplicantsMap[internshipId] = await GetApplicantsForProfessorInternship(internshipId);
                await LoadProfessorInternshipApplicantData();
            }
        }
    
        StateHasChanged();
    }



    private async Task<IEnumerable<InternshipApplied>> GetApplicantsForInternships(long internshipId)
    {
        var internship = await dbContext.CompanyInternships
            .Where(i => i.RNGForInternshipUploadedAsCompany == internshipId) // Updated property name
            .FirstOrDefaultAsync();

        if (internship == null)
        {
            return Enumerable.Empty<InternshipApplied>();
        }

        return await dbContext.InternshipsApplied
            .Where(a => a.RNGForInternshipApplied == internshipId)
            .Include(a => a.StudentDetails) 
            .AsNoTracking()
            .ToListAsync();
    }



    private void ToggleProfessorInternshipApplicants(long professorinternshipRNG)
    {
        if (expandedProfessorInternships.ContainsKey(professorinternshipRNG))
        {
            expandedProfessorInternships[professorinternshipRNG] = !expandedProfessorInternships[professorinternshipRNG];
        }
        else
        {
            expandedProfessorInternships[professorinternshipRNG] = true;
        }
        StateHasChanged();
        Console.WriteLine($"Internship ID: {professorinternshipRNG}, Expanded: {expandedProfessorInternships[professorinternshipRNG]}");
    }



    private async Task ToggleProfessorThesisExpanded(long thesisRNG)
    {
        Console.WriteLine($"ToggleProfessorThesisExpanded called for thesis: {thesisRNG}");

        // Toggle expansion state
        if (expandedTheses.ContainsKey(thesisRNG))
        {
            expandedTheses[thesisRNG] = !expandedTheses[thesisRNG];
        }
        else
        {
            expandedTheses[thesisRNG] = true;
        }

        // Load data if expanding
        if (expandedTheses[thesisRNG])
        {
            if (!professorThesisApplicantsMap.ContainsKey(thesisRNG))
            {
                professorThesisApplicantsMap[thesisRNG] = await GetApplicantsForProfessorThesis(thesisRNG);
                await LoadProfessorThesisApplicantData();
            }
        }
    
        StateHasChanged();
    }

    private async Task LoadProfessorThesisApplicantData()
    {
        try 
        {
            var studentEmails = professorThesisApplicantsMap.Values
                .SelectMany(x => x)
                .Select(a => a.StudentEmailAppliedForProfessorThesis.ToLower())
                .Distinct()
                .ToList();

            var students = await dbContext.Students
                .Where(s => studentEmails.Contains(s.Email.ToLower()))
                .Select(s => new Student {
                    Id = s.Id,
                    Student_UniqueID = s.Student_UniqueID,
                    Email = s.Email,
                    Image = s.Image,
                    Name = s.Name,
                    Surname = s.Surname,
                    Telephone = s.Telephone,
                    PermanentAddress = s.PermanentAddress,
                    PermanentRegion = s.PermanentRegion,
                    PermanentTown = s.PermanentTown,
                    Attachment = s.Attachment,
                    LinkedInProfile = s.LinkedInProfile,
                    PersonalWebsite = s.PersonalWebsite,
                    Transport = s.Transport,
                    RegNumber = s.RegNumber,
                    University = s.University,
                    Department = s.Department,
                    EnrollmentDate = s.EnrollmentDate,
                    StudyYear = s.StudyYear,
                    LevelOfDegree = s.LevelOfDegree,
                    AreasOfExpertise = s.AreasOfExpertise,
                    Keywords = s.Keywords,
                    ExpectedGraduationDate = s.ExpectedGraduationDate,
                    CompletedECTS = s.CompletedECTS
                })
                .AsNoTracking()
                .ToListAsync();

            //studentDataCache.Clear(); TO EVGALA 11/09/2025 GIATI DEN EKANE RENDER TOUS APPLICANTS STO INTERNSHIP
            foreach (var student in students)
            {
                studentDataCache[student.Email.ToLower()] = student;
            }

            Console.WriteLine($"Loaded {students.Count} professor thesis student records");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading professor thesis student data: {ex.Message}");
        }
    }


    private async Task ToggleJobsExpanded(long positionId)
    {
        if (expandedJobs.ContainsKey(positionId))
        {
            expandedJobs[positionId] = !expandedJobs[positionId];
        }
        else
        {
            expandedJobs[positionId] = true;
        }

        if (expandedJobs[positionId])
        {
            if (!jobApplicantsMap.ContainsKey(positionId))
            {
                jobApplicantsMap[positionId] = await GetApplicantsForJobs(positionId);
                await LoadCompanyJobApplicantData(); 
            }
        }
        else
        {
            jobApplicantsMap.Remove(positionId);
        }

        StateHasChanged();
    }


    private async Task ToggleCompanyThesesExpanded(long companyThesisRNG)
    {
        if (expandedCompanyTheses.ContainsKey(companyThesisRNG))
        {
            expandedCompanyTheses[companyThesisRNG] = !expandedCompanyTheses[companyThesisRNG];
        }
        else
        {
            expandedCompanyTheses[companyThesisRNG] = true;
        }

        if (expandedCompanyTheses[companyThesisRNG])
        {
            if (!companyThesisApplicantsMap.ContainsKey(companyThesisRNG))
            {
                companyThesisApplicantsMap[companyThesisRNG] = await GetApplicantsForCompanyThesis(companyThesisRNG);
                await LoadCompanyThesisApplicantData(); // Add this equivalent method
            }
        }
        else
        {
            companyThesisApplicantsMap.Remove(companyThesisRNG);
        }

        StateHasChanged();
    }





    private async Task ToggleCompanyThesesExpandedForProfessorInterest(long companythesisRNG)
    {
        Console.WriteLine($"ToggleThesesExpandedProfessorInterest called for position: {companythesisRNG}");

        if (expandedCompanyThesesForProfessorInterest.ContainsKey(companythesisRNG))
        {
            expandedCompanyThesesForProfessorInterest[companythesisRNG] = !expandedCompanyThesesForProfessorInterest[companythesisRNG];
        }
        else
        {
            expandedCompanyThesesForProfessorInterest[companythesisRNG] = true;
        }

        if (expandedCompanyThesesForProfessorInterest[companythesisRNG])
        {
            if (!companyThesesProfessorsMap.ContainsKey(companythesisRNG))
            {
                // Load interested professors for this thesis
                var thesisWithProfessors = await dbContext.CompanyTheses
                    .Include(t => t.ProfessorInterested)
                    .Where(t => t.RNGForThesisUploadedAsCompany == companythesisRNG && 
                               t.IsProfessorInteresetedInCompanyThesis)
                    .ToListAsync();

                companyThesesProfessorsMap[companythesisRNG] = thesisWithProfessors;
            
                // Load professor data if not already cached
                await LoadProfessorDataWhenHeShowsInterestForCompanyTheses(thesisWithProfessors);
            }
        }

        StateHasChanged();
    }

    private async Task LoadProfessorDataWhenHeShowsInterestForCompanyTheses(List<CompanyThesis> theses)
    {
        try 
        {
            // Get all unique professor emails from interested theses
            var professorEmails = theses
                .Where(t => !string.IsNullOrEmpty(t.ProfessorEmailInterestedInCompanyThesis))
                .Select(t => t.ProfessorEmailInterestedInCompanyThesis.ToLower())
                .Distinct()
                .ToList();

            // Load ALL professor fields according to your model
            var professors = await dbContext.Professors
                .Where(p => professorEmails.Contains(p.ProfEmail.ToLower()))
                .Select(p => new Professor {
                    Id = p.Id,
                    ProfEmail = p.ProfEmail,
                    Professor_UniqueID = p.Professor_UniqueID,
                    ProfImage = p.ProfImage,
                    ProfName = p.ProfName,
                    ProfSurname = p.ProfSurname,
                    ProfUniversity = p.ProfUniversity,
                    ProfDepartment = p.ProfDepartment,
                    ProfVahmidaDEP = p.ProfVahmidaDEP,
                    ProfWorkTelephone = p.ProfWorkTelephone,
                    ProfPersonalTelephone = p.ProfPersonalTelephone,
                    ProfPersonalTelephoneVisibility = p.ProfPersonalTelephoneVisibility,
                    ProfPersonalWebsite = p.ProfPersonalWebsite,
                    ProfLinkedInSite = p.ProfLinkedInSite,
                    ProfScholarProfile = p.ProfScholarProfile,
                    ProfOrchidProfile = p.ProfOrchidProfile,
                    ProfGeneralFieldOfWork = p.ProfGeneralFieldOfWork,
                    ProfGeneralSkills = p.ProfGeneralSkills,
                    ProfPersonalDescription = p.ProfPersonalDescription,
                    ProfCVAttachment = p.ProfCVAttachment,
                    ProfRegistryNumber = p.ProfRegistryNumber,
                    ProfCourses = p.ProfCourses
                })
                .AsNoTracking()
                .ToListAsync();

            // Initialize cache if null
            professorDataCache ??= new Dictionary<string, Professor>();

            // Update cache
            foreach (var professor in professors)
            {
                professorDataCache[professor.ProfEmail.ToLower()] = professor;
            }

            Console.WriteLine($"Loaded {professors.Count} professor records for interested theses");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading professor data: {ex.Message}");
        }
    }


    private void ToggleProfessorThesesExpandedForCompanyInterest(long professorthesisRNG)
    {
        Console.WriteLine($"ToggleThesesExpandedCompanyInterest called for position: {professorthesisRNG}");

        if (expandedProfessorThesesForCompanyInterest.ContainsKey(professorthesisRNG))
        {
            expandedProfessorThesesForCompanyInterest[professorthesisRNG] = !expandedProfessorThesesForCompanyInterest[professorthesisRNG];
        }
        else
        {
            expandedProfessorThesesForCompanyInterest[professorthesisRNG] = true;
        }

        // Ensure the UI is updated
        StateHasChanged();
    }


    private async Task<IEnumerable<InternshipApplied>> GetApplicants(long companyInternshipRNG)
    {
        // Get the internship details including the position information
        var internship = await dbContext.CompanyInternships
            .Where(i => i.RNGForInternshipUploadedAsCompany == companyInternshipRNG) // Updated property
            .Select(i => new {
                i.CompanyInternshipTitle,
                RNGForInternshipUploadedAsCompany = i.RNGForInternshipUploadedAsCompany // Updated property
            })
            .FirstOrDefaultAsync();

        if (internship == null)
            return Enumerable.Empty<InternshipApplied>();

        // Get all applications for this internship (now matching by RNG)
        return await dbContext.InternshipsApplied
            .Include(a => a.StudentDetails)
            .Include(a => a.CompanyDetails)
            .Where(a => a.RNGForInternshipApplied == companyInternshipRNG)
            .ToListAsync();
    }

    private async Task<IEnumerable<ProfessorInternshipApplied>> GetProfessorInternshipApplicants(long professorInternshipRNG)
    {
        // Get the professor internship details including the position information
        var internship = await dbContext.ProfessorInternships
            .Include(i => i.Professor) // Include professor details
            .Where(i => i.RNGForInternshipUploadedAsProfessor == professorInternshipRNG) // Updated property name
            .Select(i => new {
                i.ProfessorInternshipTitle,
                i.RNGForInternshipUploadedAsProfessor, // Updated property name
                ProfessorName = i.Professor.ProfName, // From navigation property
                ProfessorSurname = i.Professor.ProfSurname // From navigation property
            })
            .FirstOrDefaultAsync();

        if (internship == null)
            return Enumerable.Empty<ProfessorInternshipApplied>();

        // Get all applications for this internship (matching by RNG)
        return await dbContext.ProfessorInternshipsApplied
            .Include(a => a.StudentDetails)
            .Include(a => a.ProfessorDetails)
            .Where(a => a.RNGForProfessorInternshipApplied == professorInternshipRNG)
            .ToListAsync();
    }



    private async Task<IEnumerable<CompanyJobApplied>> GetApplicantsForJobs(long positionId)
    {
        return await dbContext.CompanyJobsApplied
            .Where(a => a.RNGForCompanyJobApplied == positionId)
            .Include(a => a.StudentDetails)  // Make sure these are included
            .Include(a => a.CompanyDetails)
            .AsNoTracking()
            .ToListAsync();
    }


    private async Task<IEnumerable<CompanyThesisApplied>> GetApplicantsForCompanyThesis(long companyThesisRNG)
    {
        return await dbContext.CompanyThesesApplied
            .Where(a => a.RNGForCompanyThesisApplied == companyThesisRNG)
            .Include(a => a.StudentDetails)  // Include student details
            .Include(a => a.CompanyDetails)  // Include company details
            .AsNoTracking()  // Add no tracking for better performance
            .ToListAsync();
    }

    private async Task<List<ProfessorThesisApplied>> GetApplicantsForProfessorThesis(long thesisRNG)
    {
        return await dbContext.ProfessorThesesApplied
            .Where(a => a.RNGForProfessorThesisApplied == thesisRNG)
            .Include(a => a.StudentDetails)
            .Include(a => a.ProfessorDetails)
            .AsNoTracking() 
            .ToListAsync();
    }



    private async Task AcceptInternshipApplicationAsCompany_MadeByStudent(long internshipRNG, string studentUniqueID)
    {
        try
        {
            // Fetch application with related internship (no need to include details for status updates)
            var application = await dbContext.InternshipsApplied
                .Join(dbContext.CompanyInternships,
                    applied => applied.RNGForInternshipApplied,
                    internship => internship.RNGForInternshipUploadedAsCompany, // Updated property
                    (applied, internship) => new { Application = applied, Internship = internship })
                .FirstOrDefaultAsync(x => x.Application.RNGForInternshipApplied == internshipRNG &&
                                        x.Application.StudentUniqueIDAppliedForInternship == studentUniqueID);

            if (application == null)
            {
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Δεν βρέθηκε η Αίτηση ή ο Φοιτητής").AsTask());
                return;
            }

            // Update status directly on the main application entity
            application.Application.InternshipStatusAppliedAtTheCompanySide = "Επιτυχής";
            application.Application.InternshipStatusAppliedAtTheStudentSide = "Επιτυχής";

            await dbContext.SaveChangesAsync();

            try
            {
                // Get student details from your user service
                var student = await GetStudentDetails(application.Application.StudentEmailAppliedForInternship);

                // Send acceptance email to student
                await InternshipEmailService.SendAcceptanceEmailAsCompanyToStudentAfterHeAppliedForInternshipPosition(
                    application.Application.StudentEmailAppliedForInternship.Trim(),
                    student?.Name,
                    student?.Surname,
                    application.Internship.CompanyInternshipTitle,
                    application.Internship.Company?.CompanyName, // Updated to use navigation property with fallback
                    application.Application.RNGForInternshipAppliedAsStudent_HashedAsUniqueID,
                    student?.Attachment
                );

                // Send notification email to company
                await InternshipEmailService.SendAcceptanceConfirmationEmailToCompanyAfterStudentAppliedForInternshipPosition(
                    application.Application.CompanyEmailWhereStudentAppliedForInternship.Trim(),
                    application.Internship.Company?.CompanyName, // Updated to use navigation property with fallback
                    student?.Name,
                    student?.Surname,
                    application.Application.RNGForInternshipAppliedAsStudent_HashedAsUniqueID,
                    application.Internship.CompanyInternshipTitle
                );

                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    $"Ενημερώσεις Αποδοχής στάλθηκαν τόσο στον Φοιτητή " +
                    $"({student?.Name} {student?.Surname}) " +
                    $"όσο και στην Εταιρεία ({application.Internship.Company?.CompanyName})").AsTask());

                StateHasChanged();
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid email address format.");
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Μη έγκυρη διεύθυνση email.").AsTask());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accepting internship: {ex.Message} \n StackTrace: {ex.StackTrace}");
            await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Παρουσιάστηκε σφάλμα: {ex.Message}").AsTask());
        }
    }

    private async Task RejectInternshipApplicationAsCompany_MadeByStudent(long internshipRNG, string studentUniqueID)
    {
        try
        {
            // Fetch application with related internship
            var application = await dbContext.InternshipsApplied
                .Join(dbContext.CompanyInternships,
                    applied => applied.RNGForInternshipApplied,
                    internship => internship.RNGForInternshipUploadedAsCompany, // Updated property
                    (applied, internship) => new { Application = applied, Internship = internship })
                .FirstOrDefaultAsync(x => x.Application.RNGForInternshipApplied == internshipRNG &&
                                       x.Application.StudentUniqueIDAppliedForInternship == studentUniqueID);

            if (application == null)
            {
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Δεν βρέθηκε η Αίτηση ή ο Φοιτητής").AsTask());
                return;
            }

            // Update status directly on the main application entity
            application.Application.InternshipStatusAppliedAtTheCompanySide = "Απορρίφθηκε";
            application.Application.InternshipStatusAppliedAtTheStudentSide = "Απορρίφθηκε";

            await dbContext.SaveChangesAsync();

            try
            {
                // Get student details from your user service
                var student = await GetStudentDetails(application.Application.StudentEmailAppliedForInternship);

                // Send rejection email to student
                await InternshipEmailService.SendRejectionEmailAsCompanyToStudentAfterHeAppliedForInternshipPosition(
                    application.Application.StudentEmailAppliedForInternship.Trim(),
                    student?.Name,
                    student?.Surname,
                    application.Internship.CompanyInternshipTitle,
                    application.Internship.Company?.CompanyName, // Updated to use navigation property with fallback
                    application.Application.RNGForInternshipAppliedAsStudent_HashedAsUniqueID,
                    student?.Attachment
                );

                // Send notification email to company
                await InternshipEmailService.SendRejectionConfirmationEmailToCompanyAfterStudentAppliedForInternshipPosition(
                    application.Application.CompanyEmailWhereStudentAppliedForInternship.Trim(),
                    application.Internship.Company?.CompanyName, // Updated to use navigation property with fallback
                    student?.Name,
                    student?.Surname,
                    application.Application.RNGForInternshipAppliedAsStudent_HashedAsUniqueID,
                    application.Internship.CompanyInternshipTitle
                );

                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2",
                    $"Η Απόρριψη της Αίτησης κοινοποιήθηκε μέσω Email τόσο στον Φοιτητή " +
                    $"({student?.Name} {student?.Surname}) " +
                    $"όσο και στην Εταιρεία ({application.Internship.Company?.CompanyName})").AsTask());

                StateHasChanged();
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid email address format.");
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Μη έγκυρη διεύθυνση email.").AsTask());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error rejecting internship: {ex.Message} \n StackTrace: {ex.StackTrace}");
            await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Παρουσιάστηκε σφάλμα: {ex.Message}").AsTask());
        }
    }



    private async Task ConfirmAndAcceptInternship(long internshipRNG, string studentUniqueID)
    {
        bool isConfirmedForInternships = await JS.InvokeAsync<bool>(
            "confirmActionWithHTML", 
            "- ΑΠΟΔΟΧΗ ΑΙΤΗΣΗΣ - \n Η ενέργεια αυτή δεν θα μπορεί να αναιρεθεί. Είστε σίγουρος/η;"
        );
    
        if (isConfirmedForInternships)
        {
            await AcceptInternshipApplicationAsCompany_MadeByStudent(internshipRNG, studentUniqueID);
            actionsPerformedToAcceptorRejectInternshipsAsCompany = true;
        }
    }

    private async Task ConfirmAndRejectInternship(long internshipRNG, string studentUniqueID)
    {
        bool isConfirmedForInternships = await JS.InvokeAsync<bool>(
            "confirmActionWithHTML", 
            "- ΑΠΟΡΡΙΨΗ ΑΙΤΗΣΗΣ - \n Η ενέργεια αυτή δεν θα μπορεί να αναιρεθεί. Είστε σίγουρος/η;"
        );
    
        if (isConfirmedForInternships)
        {
            await RejectInternshipApplicationAsCompany_MadeByStudent(internshipRNG, studentUniqueID);
            actionsPerformedToAcceptorRejectInternshipsAsCompany = true; 
        }
    }

    private async Task AcceptJobApplicationAsCompany_MadeByStudent(long jobRNG, string studentUniqueID)
    {
        try
        {
            // Fetch application with related job (no need to include details for status updates)
            var application = await dbContext.CompanyJobsApplied
                .Join(dbContext.CompanyJobs,
                    applied => applied.RNGForCompanyJobApplied,
                    job => job.RNGForPositionUploaded,
                    (applied, job) => new { Application = applied, Job = job })
                .FirstOrDefaultAsync(x => x.Application.RNGForCompanyJobApplied == jobRNG &&
                                        x.Application.StudentUniqueIDAppliedForCompanyJob == studentUniqueID);

            if (application == null)
            {
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Δεν βρέθηκε η Αίτηση ή ο Φοιτητής").AsTask());
                return;
            }

            // Update status directly on the main application entity
            application.Application.CompanyPositionStatusAppliedAtTheCompanySide = "Επιτυχής";
            application.Application.CompanyPositionStatusAppliedAtTheStudentSide = "Επιτυχής";

            await dbContext.SaveChangesAsync();

            try
            {
                // Get student details from your user service
                var student = await GetStudentDetails(application.Application.StudentEmailAppliedForCompanyJob);
    
                // Ensure Company data is loaded
                if (application.Job.Company == null)
                {
                    await dbContext.Entry(application.Job)
                        .Reference(j => j.Company)
                        .LoadAsync();
                }

                // Send acceptance email to student
                await InternshipEmailService.SendAcceptanceEmailAsCompanyToStudentAfterHeAppliedForJobPosition(
                    application.Application.StudentEmailAppliedForCompanyJob.Trim(),
                    student?.Name,
                    student?.Surname,
                    application.Job.PositionTitle,
                    application.Job.Company?.CompanyName, // Get from Company navigation property
                    application.Application.RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID,
                    student?.Attachment
                );

                // Send notification email to company
                await InternshipEmailService.SendAcceptanceConfirmationEmailToCompanyAfterStudentAppliedForJobPosition(
                    application.Application.CompanysEmailWhereStudentAppliedForCompanyJob.Trim(),
                    application.Job.Company?.CompanyName, // Get from Company navigation property
                    student?.Name,
                    student?.Surname,
                    application.Application.RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID,
                    application.Job.PositionTitle
                );

                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    $"Ενημερώσεις Αποδοχής στάλθηκαν τόσο στον Φοιτητή " +
                    $"({student?.Name} {student?.Surname}) " +
                    $"όσο και στην Εταιρεία ({application.Job.Company?.CompanyName})").AsTask());

                StateHasChanged();
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid email address format.");
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Μη έγκυρη διεύθυνση email.").AsTask());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accepting job: {ex.Message} \n StackTrace: {ex.StackTrace}");
            await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Παρουσιάστηκε σφάλμα: {ex.Message}").AsTask());
        }
    }

    private async Task RejectJobApplicationAsCompany_MadeByStudent(long jobRNG, string studentUniqueID)
    {
        try
        {
            // Fetch application with related job (no need to include details for status updates)
            var application = await dbContext.CompanyJobsApplied
                .Join(dbContext.CompanyJobs,
                    applied => applied.RNGForCompanyJobApplied,
                    job => job.RNGForPositionUploaded,
                    (applied, job) => new { Application = applied, Job = job })
                .FirstOrDefaultAsync(x => x.Application.RNGForCompanyJobApplied == jobRNG &&
                                       x.Application.StudentUniqueIDAppliedForCompanyJob == studentUniqueID);

            if (application == null)
            {
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Δεν βρέθηκε η Αίτηση ή ο Φοιτητής").AsTask());
                return;
            }

            // Update status directly on the main application entity
            application.Application.CompanyPositionStatusAppliedAtTheCompanySide = "Απορρίφθηκε";
            application.Application.CompanyPositionStatusAppliedAtTheStudentSide = "Απορρίφθηκε";

            await dbContext.SaveChangesAsync();

            try
            {
                // Get student details from your user service
                var student = await GetStudentDetails(application.Application.StudentEmailAppliedForCompanyJob);
    
                // Ensure Company data is loaded if not already included
                if (application.Job.Company == null)
                {
                    await dbContext.Entry(application.Job)
                        .Reference(j => j.Company)
                        .LoadAsync();
                }

                // Send rejection email to student
                await InternshipEmailService.SendRejectionEmailAsCompanyToStudentAfterHeAppliedForJobPosition(
                    application.Application.StudentEmailAppliedForCompanyJob.Trim(),
                    student?.Name,
                    student?.Surname,
                    application.Job.PositionTitle,
                    application.Job.Company?.CompanyName, // From Company navigation property
                    application.Application.RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID,
                    student?.Attachment
                );

                // Send notification email to company
                await InternshipEmailService.SendRejectionConfirmationEmailToCompanyAfterStudentAppliedForJobPosition(
                    application.Application.CompanysEmailWhereStudentAppliedForCompanyJob.Trim(),
                    application.Job.Company?.CompanyName, // From Company navigation property
                    student?.Name,
                    student?.Surname,
                    application.Application.RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID,
                    application.Job.PositionTitle
                );

                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2",
                    $"Η Απόρριψη της Αίτησης κοινοποιήθηκε μέσω Email τόσο στον Φοιτητή " +
                    $"({student?.Name} {student?.Surname}) " +
                    $"όσο και στην Εταιρεία ({application.Job.Company?.CompanyName})").AsTask());

                StateHasChanged();
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid email address format.");
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Μη έγκυρη διεύθυνση email.").AsTask());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error rejecting job: {ex.Message} \n StackTrace: {ex.StackTrace}");
            await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Παρουσιάστηκε σφάλμα: {ex.Message}").AsTask());
        }
    }

    private async Task ConfirmAndAcceptJob(long jobRNG, string studentUniqueID)
    {
        bool isConfirmedForJobs = await JS.InvokeAsync<bool>("confirmActionWithHTML", 
            "- ΑΠΟΔΟΧΗ ΑΙΤΗΣΗΣ - \n Η ενέργεια αυτή δεν θα μπορεί να αναιρεθεί. Είστε σίγουρος/η;");
    
        if (isConfirmedForJobs)
        {
            await AcceptJobApplicationAsCompany_MadeByStudent(jobRNG, studentUniqueID);
            actionsPerformedToAcceptorRejectJobsAsCompany = true;
        }
    }

    private async Task ConfirmAndRejectJob(long jobRNG, string studentUniqueID)
    {
        bool isConfirmedForJobs = await JS.InvokeAsync<bool>("confirmActionWithHTML", 
            "- ΑΠΟΡΡΙΨΗ ΑΙΤΗΣΗΣ - \n Η ενέργεια αυτή δεν θα μπορεί να αναιρεθεί. Είστε σίγουρος/η;");
    
        if (isConfirmedForJobs)
        {
            await RejectJobApplicationAsCompany_MadeByStudent(jobRNG, studentUniqueID);
            actionsPerformedToAcceptorRejectJobsAsCompany = true;
        }
    }

    private async Task AcceptThesisApplicationAsCompany_MadeByStudent(long companythesisId, string studentUniqueID)
    {
        try
        {
            // Fetch application with related thesis (similar to job approach)
            var application = await dbContext.CompanyThesesApplied
                .Join(dbContext.CompanyTheses,
                    applied => applied.RNGForCompanyThesisApplied,
                    thesis => thesis.RNGForThesisUploadedAsCompany,
                    (applied, thesis) => new { Application = applied, Thesis = thesis })
                .FirstOrDefaultAsync(x => x.Application.RNGForCompanyThesisApplied == companythesisId &&
                                        x.Application.StudentUniqueIDAppliedForThesis == studentUniqueID);

            if (application == null)
            {
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Δεν βρέθηκε η Αίτηση ή ο Φοιτητής").AsTask());
                return;
            }

            // Update status directly on the main application entity
            application.Application.CompanyThesisStatusAppliedAtStudentSide = "Επιτυχής";
            application.Application.CompanyThesisStatusAppliedAtCompanySide = "Έχει γίνει Αποδοχή";

            await dbContext.SaveChangesAsync();

            try
            {
                // Get student details from your user service
                var student = await GetStudentDetails(application.Application.StudentEmailAppliedForThesis);
    
                // Get company name from navigation property with fallback
                var companyName = application.Thesis.Company?.CompanyName ?? "Άγνωστη Εταιρεία";

                // Send acceptance email to student
                await InternshipEmailService.SendAcceptanceEmailAsCompanyToStudentAfterHeAppliedForThesisPosition(
                    application.Application.StudentEmailAppliedForThesis.Trim(),
                    student?.Name,
                    student?.Surname,
                    application.Thesis.CompanyThesisTitle,
                    companyName,  // Using navigation property instead of CompanyNameUploadedThesis
                    application.Application.RNGForCompanyThesisAppliedAsStudent_HashedAsUniqueID,
                    student?.Attachment
                );

                // Send notification email to company
                await InternshipEmailService.SendAcceptanceConfirmationEmailToCompanyAfterStudentAppliedForThesisPosition(
                    application.Application.CompanyEmailWhereStudentAppliedForThesis.Trim(),
                    companyName,  // Using navigation property instead of CompanyNameUploadedThesis
                    student?.Name,
                    student?.Surname,
                    application.Application.RNGForCompanyThesisAppliedAsStudent_HashedAsUniqueID,
                    application.Thesis.CompanyThesisTitle
                );

                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    $"Ενημερώσεις Αποδοχής στάλθηκαν τόσο στον Φοιτητή " +
                    $"({student?.Name} {student?.Surname}) " +
                    $"όσο και στην Εταιρεία ({companyName})").AsTask());

                StateHasChanged();
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid email address format.");
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Μη έγκυρη διεύθυνση email.").AsTask());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accepting thesis: {ex.Message} \n StackTrace: {ex.StackTrace}");
            await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Παρουσιάστηκε σφάλμα: {ex.Message}").AsTask());
        }
    }

    private async Task RejectThesisApplicationAsCompany_MadeByStudent(long companythesisId, string studentUniqueID)
    {
        try
        {
            // Fetch application with related thesis (similar to job approach)
            var application = await dbContext.CompanyThesesApplied
                .Join(dbContext.CompanyTheses,
                    applied => applied.RNGForCompanyThesisApplied,
                    thesis => thesis.RNGForThesisUploadedAsCompany,
                    (applied, thesis) => new { Application = applied, Thesis = thesis })
                .FirstOrDefaultAsync(x => x.Application.RNGForCompanyThesisApplied == companythesisId &&
                                       x.Application.StudentUniqueIDAppliedForThesis == studentUniqueID);

            if (application == null)
            {
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Δεν βρέθηκε η Αίτηση ή ο Φοιτητής").AsTask());
                return;
            }

            // Update status directly on the main application entity
            application.Application.CompanyThesisStatusAppliedAtCompanySide = "Έχει Απορριφθεί";
            application.Application.CompanyThesisStatusAppliedAtStudentSide = "Απορρίφθηκε";

            await dbContext.SaveChangesAsync();

            try
            {
                // Get student details from your user service (like in job version)
                var student = await GetStudentDetails(application.Application.StudentEmailAppliedForThesis);
    
                // Get company name from navigation property (with fallback)
                var companyName = application.Thesis.Company?.CompanyName ?? "Άγνωστη Εταιρεία";

                // Send rejection email to student (using joined thesis data)
                await InternshipEmailService.SendRejectionEmailAsCompanyToStudentAfterHeAppliedForThesisPosition(
                    application.Application.StudentEmailAppliedForThesis.Trim(),
                    student?.Name,
                    student?.Surname,
                    application.Thesis.CompanyThesisTitle, // From joined CompanyThesis
                    application.Application.RNGForCompanyThesisAppliedAsStudent_HashedAsUniqueID,
                    companyName // Using navigation property with fallback
                );

                // Send notification email to company (using joined thesis data)
                await InternshipEmailService.SendRejectionConfirmationEmailToCompanyAfterStudentAppliedForThesisPosition(
                    application.Application.CompanyEmailWhereStudentAppliedForThesis.Trim(),
                    companyName, // Using navigation property with fallback
                    student?.Name,
                    student?.Surname,
                    application.Application.RNGForCompanyThesisAppliedAsStudent_HashedAsUniqueID,
                    application.Thesis.CompanyThesisTitle // From joined CompanyThesis
                );

                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2",
                    $"Η Απόρριψη της Αίτησης κοινοποιήθηκε μέσω Email τόσο στον Φοιτητή " +
                    $"({student?.Name} {student?.Surname}) " +
                    $"όσο και στην Εταιρεία ({companyName})").AsTask());

                StateHasChanged();
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid email address format.");
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Μη έγκυρη διεύθυνση email.").AsTask());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error rejecting thesis: {ex.Message} \n StackTrace: {ex.StackTrace}");
            await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Παρουσιάστηκε σφάλμα: {ex.Message}").AsTask());
        }
    }

    private async Task ConfirmAndAcceptStudentThesisApplicationAsCompany(long companythesisId, string studentUniqueID)
    {
        bool isConfirmedFortStudentThesisApplications = await JS.InvokeAsync<bool>("confirmActionWithHTML", "- ΑΠΟΔΟΧΗ ΑΙΤΗΣΗΣ - \n Η ενέργεια αυτή δεν θα μπορεί να αναιρεθεί. Είστε σίγουρος/η;");
        if (isConfirmedFortStudentThesisApplications)
        {
            await AcceptThesisApplicationAsCompany_MadeByStudent(companythesisId, studentUniqueID);
            actionsPerformedToAcceptorRejectThesisAsCompany = true;
        }
    }

    private async Task ConfirmAndRejectStudentThesisApplicationAsCompany(long companythesisId, string studentUniqueID)
    {
        bool isConfirmedFortStudentThesisApplication = await JS.InvokeAsync<bool>("confirmActionWithHTML", "- ΑΠΟΡΡΙΨΗ ΑΙΤΗΣΗΣ - \n Η ενέργεια αυτή δεν θα μπορεί να αναιρεθεί. Είστε σίγουρος/η;");
        if (isConfirmedFortStudentThesisApplication)
        {
            await RejectThesisApplicationAsCompany_MadeByStudent(companythesisId, studentUniqueID);
            actionsPerformedToAcceptorRejectThesisAsCompany = true;
        }
    }

    private async Task AcceptThesisApplicationAsProfessor_MadeByStudent(long professorThesisId, string studentUniqueID)
    {
        try
        {
            // Fetch application with related thesis using join (consistent with company version)
            var application = await dbContext.ProfessorThesesApplied
                .Join(dbContext.ProfessorTheses,
                    applied => applied.RNGForProfessorThesisApplied,
                    thesis => thesis.RNGForThesisUploaded,
                    (applied, thesis) => new { Application = applied, Thesis = thesis })
                .FirstOrDefaultAsync(x => x.Application.Id == professorThesisId &&
                                       x.Application.StudentUniqueIDAppliedForProfessorThesis == studentUniqueID);

            if (application == null)
            {
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Δεν βρέθηκε η Αίτηση ή ο Φοιτητής").AsTask());
                return;
            }

            // Update status directly on the application entity
            application.Application.ProfessorThesisStatusAppliedAtStudentSide = "Επιτυχής";
            application.Application.ProfessorThesisStatusAppliedAtProfessorSide = "Έχει γίνει Αποδοχή";

            await dbContext.SaveChangesAsync();

            try
            {
                // Get student details from navigation property with fallback
                var student = await GetStudentDetails(application.Application.StudentEmailAppliedForProfessorThesis);
            
                // Get professor name from navigation property with fallback
                var professorName = application.Thesis.Professor != null
                    ? $"{application.Thesis.Professor.ProfName} {application.Thesis.Professor.ProfSurname}"
                    : "Άγνωστος Καθηγητής";

                // Send acceptance email to student
                await InternshipEmailService.SendAcceptanceEmailAsProfessorToStudentAfterHeAppliedForThesisPosition(
                    application.Application.StudentEmailAppliedForProfessorThesis.Trim(),
                    student?.Name,
                    student?.Surname,
                    application.Thesis.ThesisTitle,
                    application.Application.RNGForProfessorThesisApplied_HashedAsUniqueID,
                    professorName
                );

                // Send notification email to professor
                await InternshipEmailService.SendAcceptanceConfirmationEmailToProfessorAfterStudentAppliedForThesisPosition(
                    application.Application.ProfessorEmailWhereStudentAppliedForProfessorThesis.Trim(),
                    professorName,
                    student?.Name,
                    student?.Surname,
                    application.Application.RNGForProfessorThesisApplied_HashedAsUniqueID,
                    application.Thesis.ThesisTitle
                );

                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    $"Ενημερώσεις Αποδοχής στάλθηκαν τόσο στον Φοιτητή " +
                    $"({student?.Name} {student?.Surname}) " +
                    $"όσο και στον Καθηγητή ({professorName})").AsTask());

                StateHasChanged();
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid email address format.");
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Μη έγκυρη διεύθυνση email.").AsTask());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Η αποδοχή καταγράφηκε, αλλά απέτυχε η αποστολή email").AsTask());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accepting thesis: {ex.Message}\n{ex.StackTrace}");
            await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Παρουσιάστηκε σφάλμα: {ex.Message}").AsTask());
        }
    }

    private async Task RejectThesisApplicationAsProfessor_MadeByStudent(long professorThesisId, string studentUniqueID)
    {
        try
        {
            // Fetch application with related thesis using join (consistent with company version)
            var application = await dbContext.ProfessorThesesApplied
                .Join(dbContext.ProfessorTheses,
                    applied => applied.RNGForProfessorThesisApplied,
                    thesis => thesis.RNGForThesisUploaded,
                    (applied, thesis) => new { Application = applied, Thesis = thesis })
                .FirstOrDefaultAsync(x => x.Application.Id == professorThesisId &&
                                       x.Application.StudentUniqueIDAppliedForProfessorThesis == studentUniqueID);

            if (application == null)
            {
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Δεν βρέθηκε η Αίτηση ή ο Φοιτητής").AsTask());
                return;
            }

            // Update status directly on the application entity
            application.Application.ProfessorThesisStatusAppliedAtProfessorSide = "Έχει Απορριφθεί";
            application.Application.ProfessorThesisStatusAppliedAtStudentSide = "Απορρίφθηκε";

            await dbContext.SaveChangesAsync();

            try
            {
                // Get student details from navigation property with fallback
                var student = await GetStudentDetails(application.Application.StudentEmailAppliedForProfessorThesis);
            
                // Get professor name from navigation property with fallback
                var professorName = application.Thesis.Professor != null
                    ? $"{application.Thesis.Professor.ProfName} {application.Thesis.Professor.ProfSurname}"
                    : "Άγνωστος Καθηγητής";

                // Send rejection email to student
                await InternshipEmailService.SendRejectionEmailAsProfessorToStudentAfterHeAppliedForThesisPosition(
                    application.Application.StudentEmailAppliedForProfessorThesis.Trim(),
                    student?.Name,
                    student?.Surname,
                    application.Thesis.ThesisTitle,
                    application.Application.RNGForProfessorThesisApplied_HashedAsUniqueID,
                    professorName
                );

                // Send notification email to professor
                await InternshipEmailService.SendRejectionConfirmationEmailToProfessorAfterStudentAppliedForThesisPosition(
                    application.Application.ProfessorEmailWhereStudentAppliedForProfessorThesis.Trim(),
                    professorName,
                    student?.Name,
                    student?.Surname,
                    application.Application.RNGForProfessorThesisApplied_HashedAsUniqueID,
                    application.Thesis.ThesisTitle
                );

                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2",
                    $"Η Απόρριψη της Αίτησης κοινοποιήθηκε μέσω Email τόσο στον Φοιτητή " +
                    $"({student?.Name} {student?.Surname}) " +
                    $"όσο και στον Καθηγητή ({professorName})").AsTask());

                StateHasChanged();
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid email address format.");
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Μη έγκυρη διεύθυνση email.").AsTask());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Η απόρριψη καταγράφηκε, αλλά απέτυχε η αποστολή email").AsTask());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error rejecting thesis: {ex.Message}\n{ex.StackTrace}");
            await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Παρουσιάστηκε σφάλμα: {ex.Message}").AsTask());
        }
    }

    private async Task ConfirmAndAcceptProfessorThesis(long professorthesisId, string studentUniqueID)
    {
        bool isConfirmedForProfessorTheses = await JS.InvokeAsync<bool>("confirmActionWithHTML", "- ΑΠΟΔΟΧΗ ΑΙΤΗΣΗΣ - \n Η ενέργεια αυτή δεν θα μπορεί να αναιρεθεί. Είστε σίγουρος/η;");
        if (isConfirmedForProfessorTheses)
        {
            await AcceptThesisApplicationAsProfessor_MadeByStudent(professorthesisId, studentUniqueID);
            actionsPerformedToAcceptorRejectThesesAsProfessor = true;
        }
    }

    private async Task ConfirmAndRejectProfessorThesis(long professorthesisId, string studentUniqueID)
    {
        bool isConfirmedForProfessorTheses = await JS.InvokeAsync<bool>("confirmActionWithHTML", "- ΑΠΟΡΡΙΨΗ ΑΙΤΗΣΗΣ - \n Η ενέργεια αυτή δεν θα μπορεί να αναιρεθεί. Είστε σίγουρος/η;");
        if (isConfirmedForProfessorTheses)
        {
            await RejectThesisApplicationAsProfessor_MadeByStudent(professorthesisId, studentUniqueID);
            actionsPerformedToAcceptorRejectThesesAsProfessor = true;
        }
    }
     


    private bool IsComponentActive()
    {
        return JS != null;
    }

    private async Task SafeInvokeJsAsync(Func<Task> jsAction)
    {
        try
        {
            if (IsComponentActive())
            {
                await jsAction();
            }
        }
        catch (JSDisconnectedException)
        {
            Console.WriteLine("JS interop call failed because the circuit is disconnected.");
        }
    }


    private void CheckCharacterLimitInInternshipFieldUploadAsCompany(ChangeEventArgs e)
    {
        // Calculate remaining characters
        var inputText = e.Value?.ToString() ?? string.Empty;
        remainingCharactersInInternshipFieldUploadAsCompany = 120 - inputText.Length;
    }

    private void CheckCharacterLimitInThesisFieldUploadAsCompany(ChangeEventArgs e)
    {
        // Calculate remaining characters
        var inputText = e.Value?.ToString() ?? string.Empty;
        remainingCharactersInThesisFieldUploadAsCompany = 120 - inputText.Length;
    }

    private void CheckCharacterLimitInThesisFieldUploadAsProfessor(ChangeEventArgs e)
    {
        // Calculate remaining characters
        var inputText = e.Value?.ToString() ?? string.Empty;
        remainingCharactersInThesisFieldUploadAsProfessor = 120 - inputText.Length;
    }

    private void CheckCharacterLimitInAnnouncementFieldUploadAsCompany(ChangeEventArgs e)
    {
        // Calculate remaining characters
        var inputText = e.Value?.ToString() ?? string.Empty;
        remainingCharactersInAnnouncementFieldUploadAsCompany = 120 - inputText.Length;
    }

    private void CheckCharacterLimitInAnnouncementFieldUploadAsProfessor(ChangeEventArgs e)
    {
        // Calculate remaining characters
        var inputText = e.Value?.ToString() ?? string.Empty;
        remainingCharactersInAnnouncementFieldUploadAsProfessor = 120 - inputText.Length;
    }

  

    private void CheckCharacterLimitInInternshipDescriptionUploadAsCompany(ChangeEventArgs e)
    {
        var inputText = e.Value?.ToString() ?? string.Empty;
        remainingCharactersInInternshipDescriptionUploadAsCompany = 1000 - inputText.Length;
    }

    private void CheckCharacterLimitInJobFieldUploadAsCompany(ChangeEventArgs e)
    {
        // Calculate remaining characters
        var inputText = e.Value?.ToString() ?? string.Empty;
        remainingCharactersInJobFieldUploadAsCompany = 120 - inputText.Length;
    }

    private void CheckCharacterLimitInJobDescriptionUploadAsCompany(ChangeEventArgs e)
    {
        var inputText = e.Value?.ToString() ?? string.Empty;
        remainingCharactersInJobDescriptionUploadAsCompany = 1000 - inputText.Length;
    }

    private void CheckCharacterLimitInThesisDescriptionUploadAsCompany(ChangeEventArgs e)
    {
        var inputText = e.Value?.ToString() ?? string.Empty;
        remainingCharactersInThesisDescriptionUploadAsCompany = 1000 - inputText.Length;
    }

    private void CheckCharacterLimitInAnnouncementDescriptionUploadAsCompany(ChangeEventArgs e)
    {
        var inputText = e.Value?.ToString() ?? string.Empty;
        remainingCharactersInAnnouncementDescriptionUploadAsCompany = 1000 - inputText.Length;
    }

    private void CheckCharacterLimitInThesisDescriptionUploadAsProfessor(ChangeEventArgs e)
    {
        var inputText = e.Value?.ToString() ?? string.Empty;
        remainingCharactersInThesisDescriptionUploadAsProfessor = 1000 - inputText.Length;
    }

    private void CheckCharacterLimitInAnnouncementDescriptionUploadAsProfessor(ChangeEventArgs e)
    {
        var inputText = e.Value?.ToString() ?? string.Empty;
        remainingCharactersInAnnouncementDescriptionUploadAsProfessor = 1000 - inputText.Length;
    }

    private async Task CalculateStatusCountsForInternships()
    {
        await LoadInternships();
        if (internships == null) return;

        // Filter internships based on selected status
        var filteredInternships = selectedStatusFilterForInternships == "Όλα"
            ? internships
            : internships.Where(i => i.CompanyUploadedInternshipStatus == selectedStatusFilterForInternships);

        // Calculate counts
        totalCount = internships.Count();
        publishedCount = internships.Count(i => i.CompanyUploadedInternshipStatus == "Δημοσιευμένη");
        unpublishedCount = internships.Count(i => i.CompanyUploadedInternshipStatus == "Μη Δημοσιευμένη");
        withdrawnCount = internships.Count(i => i.CompanyUploadedInternshipStatus == "Αποσυρμένη");

        // Trigger UI update
        StateHasChanged();
    }

    private async Task CalculateStatusCountsForCompanyTheses()
    {
        await LoadThesesAsCompany();
        if (companytheses == null) return;

        // Filter internships based on selected status
        var filteredCompanyTheses = selectedStatusFilterForCompanyTheses == "Όλα"
            ? companytheses
            : companytheses.Where(i => i.CompanyThesisStatus == selectedStatusFilterForCompanyTheses);

        // Calculate counts
        totalCountForCompanyTheses = companytheses.Count();
        publishedCountForCompanyTheses = companytheses.Count(i => i.CompanyThesisStatus == "Δημοσιευμένη");
        unpublishedCountForCompanyTheses = companytheses.Count(i => i.CompanyThesisStatus == "Μη Δημοσιευμένη");
        withdrawnCountForCompanyTheses = companytheses.Count(i => i.CompanyThesisStatus == "Αποσυρμένη");

        // Trigger UI update
        StateHasChanged();
    }


    private async Task CalculateStatusCountsForJobs()
    {
        await LoadJobs();
        if (jobs == null) return;

        // Filter jobs based on selected status
        var filteredJobs = selectedStatusFilterForJobs == "Όλα"
            ? jobs
            : jobs.Where(i => i.PositionStatus == selectedStatusFilterForJobs);

        // Calculate counts
        totalCountJobs = jobs.Count();
        publishedCountJobs = jobs.Count(i => i.PositionStatus == "Δημοσιευμένη");
        unpublishedCountJobs = jobs.Count(i => i.PositionStatus == "Μη Δημοσιευμένη");
        withdrawnCountJobs = jobs.Count(i => i.PositionStatus == "Αποσυρμένη");

        // Trigger UI update
        StateHasChanged();
    }

    private async Task CalculateStatusCountsForAnnouncements()
    {
        await LoadUploadedAnnouncementsAsync();

        // Apply your logic to filter announcements based on the selectedStatusFilterForAnnouncements
        var filteredAnnouncements = selectedStatusFilterForAnnouncements == "Όλα"
            ? UploadedAnnouncements
            : UploadedAnnouncements.Where(i => i.CompanyAnnouncementStatus == selectedStatusFilterForAnnouncements);

        // Update counts
        totalCountAnnouncements = UploadedAnnouncements.Count();
        publishedCountAnnouncements = UploadedAnnouncements.Count(i => i.CompanyAnnouncementStatus == "Δημοσιευμένη");
        unpublishedCountAnnouncements = UploadedAnnouncements.Count(i => i.CompanyAnnouncementStatus == "Μη Δημοσιευμένη");

        // Trigger UI update
        StateHasChanged();
    }

    private async Task CalculateStatusCountsForAnnouncementsAsProfessor()
    {
        await LoadUploadedAnnouncementsAsProfessorAsync();

        // Apply your logic to filter announcements based on the selectedStatusFilterForAnnouncements
        var filteredAnnouncementsAsProfessor = selectedStatusFilterForAnnouncementsAsProfessor == "Όλα"
            ? UploadedAnnouncementsAsProfessor
            : UploadedAnnouncementsAsProfessor.Where(i => i.ProfessorAnnouncementStatus == selectedStatusFilterForAnnouncementsAsProfessor);

        // Update counts
        totalCountAnnouncementsAsProfessor = UploadedAnnouncementsAsProfessor.Count();
        publishedCountAnnouncementsAsProfessor = UploadedAnnouncementsAsProfessor.Count(i => i.ProfessorAnnouncementStatus == "Δημοσιευμένη");
        unpublishedCountAnnouncementsAsProfessor = UploadedAnnouncementsAsProfessor.Count(i => i.ProfessorAnnouncementStatus == "Μη Δημοσιευμένη");

        // Trigger UI update
        StateHasChanged();
    }

    private async Task CalculateStatusCountsForThesesAsProfessor()
    {
        await LoadUploadedThesesAsProfessorAsync();

        // Apply your logic to filter announcements based on the selectedStatusFilterForAnnouncements
        var filteredThesesAsProfessor = selectedStatusFilterForThesesAsProfessor == "Όλα"
            ? UploadedThesesAsProfessor
            : UploadedThesesAsProfessor.Where(i => i.ThesisStatus == selectedStatusFilterForThesesAsProfessor);

        // Update counts
        totalCountThesesAsProfessor = UploadedThesesAsProfessor.Count();
        publishedCountThesesAsProfessor = UploadedThesesAsProfessor.Count(i => i.ThesisStatus == "Δημοσιευμένη");
        unpublishedCountThesesAsProfessor = UploadedThesesAsProfessor.Count(i => i.ThesisStatus == "Μη Δημοσιευμένη");
        withdrawnCountThesesAsProfessor = UploadedThesesAsProfessor.Count(i => i.ThesisStatus == "Αποσυρμένη");
        // Trigger UI update
        StateHasChanged();
    }


        // protected override async Task OnParametersSetAsync()
        // {
    //         await base.OnParametersSetAsync();
    //         CalculateStatusCountsForInternships();
    //         CalculateStatusCountsForJobs();
        // }


    private async Task UpdateStatusFilterToCountInternships()
    {
        await CalculateStatusCountsForInternships();
    }

    private async Task UpdateStatusFilterToCountJobs()
    {
        await CalculateStatusCountsForJobs();
    }

    private async Task UpdateStatusFilterToCountAnnouncements()
    {
        await CalculateStatusCountsForAnnouncements();
    }
    private async Task UpdateStatusFilterToCountCompanyTheses()
    {
        await CalculateStatusCountsForCompanyTheses();
    }





    private async Task AcceptInternshipApplicationAsProfessor_MadeByStudent(long internshipRNG, string studentUniqueID)
    {
        try
        {
            // Fetch application with related internship (updated property names)
            var application = await dbContext.ProfessorInternshipsApplied
                .Join(dbContext.ProfessorInternships.Include(i => i.Professor), // Include professor data
                    applied => applied.RNGForProfessorInternshipApplied,
                    internship => internship.RNGForInternshipUploadedAsProfessor, // Updated property
                    (applied, internship) => new { Application = applied, Internship = internship })
                .FirstOrDefaultAsync(x => x.Application.RNGForProfessorInternshipApplied == internshipRNG &&
                                        x.Application.StudentDetails.StudentUniqueIDAppliedForProfessorInternship == studentUniqueID);

            if (application == null)
            {
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Δεν βρέθηκε η Αίτηση ή ο Φοιτητής").AsTask());
                return;
            }

            // Update status directly on the main application entity
            application.Application.InternshipStatusAppliedAtTheProfessorSide = "Επιτυχής";
            application.Application.InternshipStatusAppliedAtTheStudentSide = "Επιτυχής";

            await dbContext.SaveChangesAsync();

            try
            {
                // Get student details from your user service
                var student = await GetStudentDetails(application.Application.StudentDetails.StudentEmailAppliedForProfessorInternship);

                // Send acceptance email to student (updated professor name source)
                await InternshipEmailService.SendAcceptanceEmailAsProfessorToStudentAfterHeAppliedForInternshipPosition(
                    application.Application.StudentDetails.StudentEmailAppliedForProfessorInternship.Trim(),
                    student?.Name,
                    student?.Surname,
                    application.Internship.ProfessorInternshipTitle,
                    application.Application.RNGForProfessorInternshipApplied_HashedAsUniqueID,
                    $"{application.Internship.Professor.ProfName} {application.Internship.Professor.ProfSurname}" // From navigation property
                );

                // Send notification email to professor (updated professor name source)
                await InternshipEmailService.SendAcceptanceConfirmationEmailToProfessorAfterStudentAppliedForInternshipPosition(
                    application.Application.ProfessorDetails.ProfessorEmailWhereStudentAppliedForProfessorInternship.Trim(),
                    $"{application.Internship.Professor.ProfName} {application.Internship.Professor.ProfSurname}", // From navigation property
                    student?.Name,
                    student?.Surname,
                    application.Application.RNGForProfessorInternshipApplied_HashedAsUniqueID,
                    application.Internship.ProfessorInternshipTitle
                );

                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    $"Ενημερώσεις Αποδοχής στάλθηκαν τόσο στον Φοιτητή " +
                    $"({student?.Name} {student?.Surname}) " +
                    $"όσο και στον Καθηγητή ({application.Internship.Professor.ProfName} {application.Internship.Professor.ProfSurname})").AsTask());

                StateHasChanged();
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid email address format.");
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Μη έγκυρη διεύθυνση email.").AsTask());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accepting internship: {ex.Message} \n StackTrace: {ex.StackTrace}");
            await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Παρουσιάστηκε σφάλμα: {ex.Message}").AsTask());
        }
    }

    private async Task RejectInternshipApplicationAsProfessor_MadeByStudent(long internshipRNG, string studentUniqueID)
    {
        try
        {
            // Fetch application with related internship (updated property names)
            var application = await dbContext.ProfessorInternshipsApplied
                .Join(dbContext.ProfessorInternships.Include(i => i.Professor), // Include professor data
                    applied => applied.RNGForProfessorInternshipApplied,
                    internship => internship.RNGForInternshipUploadedAsProfessor, // Updated property
                    (applied, internship) => new { Application = applied, Internship = internship })
                .FirstOrDefaultAsync(x => x.Application.RNGForProfessorInternshipApplied == internshipRNG &&
                                       x.Application.StudentDetails.StudentUniqueIDAppliedForProfessorInternship == studentUniqueID);

            if (application == null)
            {
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Δεν βρέθηκε η Αίτηση ή ο Φοιτητής").AsTask());
                return;
            }

            // Update status directly on the main application entity
            application.Application.InternshipStatusAppliedAtTheProfessorSide = "Απορρίφθηκε";
            application.Application.InternshipStatusAppliedAtTheStudentSide = "Απορρίφθηκε";

            await dbContext.SaveChangesAsync();

            try
            {
                // Get student details from your user service
                var student = await GetStudentDetails(application.Application.StudentDetails.StudentEmailAppliedForProfessorInternship);

                // Send rejection email to student (updated professor name source)
                await InternshipEmailService.SendRejectionEmailAsProfessorToStudentAfterHeAppliedForInternshipPosition(
                    application.Application.StudentDetails.StudentEmailAppliedForProfessorInternship.Trim(),
                    student?.Name,
                    student?.Surname,
                    application.Internship.ProfessorInternshipTitle,
                    application.Application.RNGForProfessorInternshipApplied_HashedAsUniqueID,
                    $"{application.Internship.Professor.ProfName} {application.Internship.Professor.ProfSurname}" // From navigation property
                );

                // Send notification email to professor (updated professor name source)
                await InternshipEmailService.SendRejectionConfirmationEmailToProfessorAfterStudentAppliedForInternshipPosition(
                    application.Application.ProfessorDetails.ProfessorEmailWhereStudentAppliedForProfessorInternship.Trim(),
                    $"{application.Internship.Professor.ProfName} {application.Internship.Professor.ProfSurname}", // From navigation property
                    student?.Name,
                    student?.Surname,
                    application.Application.RNGForProfessorInternshipApplied_HashedAsUniqueID,
                    application.Internship.ProfessorInternshipTitle
                );

                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2",
                    $"Η Απόρριψη της Αίτησης κοινοποιήθηκε μέσω Email τόσο στον Φοιτητή " +
                    $"({student?.Name} {student?.Surname}) " +
                    $"όσο και στον Καθηγητή ({application.Internship.Professor.ProfName} {application.Internship.Professor.ProfSurname})").AsTask());

                StateHasChanged();
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid email address format.");
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                    "Μη έγκυρη διεύθυνση email.").AsTask());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error rejecting internship: {ex.Message} \n StackTrace: {ex.StackTrace}");
            await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Παρουσιάστηκε σφάλμα: {ex.Message}").AsTask());
        }
    }


    private async Task ConfirmAndAcceptProfessorInternship(long professorInternshipId, string studentUniqueID)
    {
        bool isConfirmedForProfessorInternships = await JS.InvokeAsync<bool>(
            "confirmActionWithHTML", 
            "- ΑΠΟΔΟΧΗ ΑΙΤΗΣΗΣ - \n Η ενέργεια αυτή δεν θα μπορεί να αναιρεθεί. Είστε σίγουρος/η;"
        );

        if (isConfirmedForProfessorInternships)
        {
            await AcceptInternshipApplicationAsProfessor_MadeByStudent(professorInternshipId, studentUniqueID);
            actionsPerformedToAcceptorRejectInternshipsAsProfessor = true;
        }
    }

    private async Task ConfirmAndRejectProfessorInternship(long professorInternshipId, string studentUniqueID)
    {
        bool isConfirmedForProfessorInternships = await JS.InvokeAsync<bool>(
            "confirmActionWithHTML", 
            "- ΑΠΟΡΡΙΨΗ ΑΙΤΗΣΗΣ - \n Η ενέργεια αυτή δεν θα μπορεί να αναιρεθεί. Είστε σίγουρος/η;"
        );

        if (isConfirmedForProfessorInternships)
        {     
            await RejectInternshipApplicationAsProfessor_MadeByStudent(professorInternshipId, studentUniqueID);
            actionsPerformedToAcceptorRejectInternshipsAsProfessor = true;
        }
    }



    private async Task ShowCompanyDetailsinTitleAsHyperlink(string companyEmail)
    {
        try 
        {
            // Fetch company details by email (more reliable than name)
            selectedCompany = await dbContext.Companies
                .FirstOrDefaultAsync(c => c.CompanyEmail == companyEmail);

            if (selectedCompany != null)
            {
                iscompanyDetailsModalVisible = true;
                StateHasChanged();
            }
            else
            {
                await JS.InvokeVoidAsync("alert", "Δεν βρέθηκαν στοιχεία εταιρίας");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading company details: {ex.Message}");
            await JS.InvokeVoidAsync("alert", "Σφάλμα κατά τη φόρτωση των στοιχείων της εταιρίας");
        }
    }

    private async Task ShowProfessorDetailsinTitleAsHyperlink_StudentInternshipApplicationsShow(string professorEmail)
    {
        try
        {
            // Fetch professor details by email with all necessary fields
            selectedProfessor = await dbContext.Professors
                .Where(p => p.ProfEmail == professorEmail)
                .Select(p => new Professor {
                    Professor_UniqueID = p.Professor_UniqueID,
                    ProfEmail = p.ProfEmail,
                    ProfName = p.ProfName,
                    ProfSurname = p.ProfSurname,
                    ProfUniversity = p.ProfUniversity,
                    ProfDepartment = p.ProfDepartment,
                    ProfVahmidaDEP = p.ProfVahmidaDEP,
                    ProfWorkTelephone = p.ProfWorkTelephone,
                    ProfPersonalTelephone = p.ProfPersonalTelephoneVisibility ? p.ProfPersonalTelephone : null,
                    ProfPersonalWebsite = p.ProfPersonalWebsite,
                    ProfLinkedInSite = p.ProfLinkedInSite,
                    ProfScholarProfile = p.ProfScholarProfile,
                    ProfOrchidProfile = p.ProfOrchidProfile,
                    ProfGeneralFieldOfWork = p.ProfGeneralFieldOfWork,
                    ProfGeneralSkills = p.ProfGeneralSkills,
                    ProfPersonalDescription = p.ProfPersonalDescription,
                    ProfImage = p.ProfImage,
                    ProfCVAttachment = p.ProfCVAttachment,
                    ProfRegistryNumber = p.ProfRegistryNumber,
                    ProfCourses = p.ProfCourses
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (selectedProfessor != null)
            {
                isProfessorDetailsModalVisible_StudentInternshipApplicationsShow = true;
                StateHasChanged();
            }
            else
            {
                await JS.InvokeVoidAsync("alert", "Δεν βρέθηκαν στοιχεία καθηγητή");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching professor details: {ex.Message}");
            await JS.InvokeVoidAsync("alert", $"Σφάλμα κατά την ανάκτηση των στοιχείων: {ex.Message}");
        }
    }


    // private async Task ShowProfessorDetailsinTitleAsHyperlink_StudentInternshipApplicationsShow(string professorName)
    // {
    //     // Fetch professor details by professorName from dbContext
    //     selectedProfessor = await dbContext.Professors.FirstOrDefaultAsync(p => p.ProfName == professorName);

    //     if (selectedProfessor != null)
    //     {
    //         // Set modal visibility to true
    //         isProfessorDetailsModalVisible_StudentInternshipApplicationsShow = true;
    //         StateHasChanged();
    //     }
    // }


    private void CloseModalforHyperLinkTitle()
    {
        iscompanyDetailsModalVisible = false;
        StateHasChanged(); // Ensure UI is updated
    }


    private async Task ShowStudentDetailsInNameAsHyperlink(string studentUniqueId)
    {
        // First try to find in cache
        selectedStudentFromCache = studentDataCache.Values
            .FirstOrDefault(s => s.Student_UniqueID == studentUniqueId);

        if (selectedStudentFromCache == null)
        {
            Console.WriteLine($"Student with ID {studentUniqueId} not found in cache - loading from DB");
        
            selectedStudentFromCache = await dbContext.Students
                .Where(s => s.Student_UniqueID == studentUniqueId)
                .Select(s => new Student {
                    Id = s.Id,
                    Student_UniqueID = s.Student_UniqueID,
                    Email = s.Email,
                    Image = s.Image,
                    Name = s.Name,
                    Surname = s.Surname,
                    Telephone = s.Telephone,
                    PermanentAddress = s.PermanentAddress,
                    PermanentRegion = s.PermanentRegion,
                    PermanentTown = s.PermanentTown,
                    Attachment = s.Attachment,
                    LinkedInProfile = s.LinkedInProfile,
                    PersonalWebsite = s.PersonalWebsite,
                    Transport = s.Transport,
                    RegNumber = s.RegNumber,
                    University = s.University,
                    Department = s.Department,
                    EnrollmentDate = s.EnrollmentDate,
                    StudyYear = s.StudyYear,
                    LevelOfDegree = s.LevelOfDegree,
                    AreasOfExpertise = s.AreasOfExpertise,
                    Keywords = s.Keywords
                })
                .FirstOrDefaultAsync();

            if (selectedStudentFromCache != null)
            {
                studentDataCache[selectedStudentFromCache.Email.ToLower()] = selectedStudentFromCache;
            }
        }

        isModalVisibleToShowStudentDetailsAsCompanyFromTheirHyperlinkNameInCompanyInternships = true;
        StateHasChanged();
    }

    private async Task ShowStudentDetailsInNameAsHyperlink_StudentAppliedinternshipsAtProfessor(string studentUniqueId)
    {
        try
        {
            // First try to find in cache
            selectedStudent = studentDataCache.Values
                .FirstOrDefault(s => s.Student_UniqueID == studentUniqueId);

            if (selectedStudent == null)
            {
                Console.WriteLine($"Student with ID {studentUniqueId} not found in cache - loading from DB");
            
                selectedStudent = await dbContext.Students
                    .Where(s => s.Student_UniqueID == studentUniqueId)
                    .Select(s => new Student {
                        Id = s.Id,
                        Student_UniqueID = s.Student_UniqueID,
                        Email = s.Email,
                        Image = s.Image,
                        Name = s.Name,
                        Surname = s.Surname,
                        Telephone = s.Telephone,
                        PermanentAddress = s.PermanentAddress,
                        PermanentRegion = s.PermanentRegion,
                        PermanentTown = s.PermanentTown,
                        Attachment = s.Attachment,
                        LinkedInProfile = s.LinkedInProfile,
                        PersonalWebsite = s.PersonalWebsite,
                        Transport = s.Transport,
                        RegNumber = s.RegNumber,
                        University = s.University,
                        Department = s.Department,
                        EnrollmentDate = s.EnrollmentDate,
                        StudyYear = s.StudyYear,
                        LevelOfDegree = s.LevelOfDegree,
                        AreasOfExpertise = s.AreasOfExpertise,
                        Keywords = s.Keywords,
                        ExpectedGraduationDate = s.ExpectedGraduationDate,
                        CompletedECTS = s.CompletedECTS,
                        InternshipStatus = s.InternshipStatus,
                        ThesisStatus = s.ThesisStatus,
                        PreferedRegions = s.PreferedRegions,
                        PreferredTowns = s.PreferredTowns
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (selectedStudent != null)
                {
                    // Add to cache if found
                    studentDataCache[selectedStudent.Email.ToLower()] = selectedStudent;
                }
            }

            isModalVisibleToShowStudentDetailsAsProfessorFromTheirHyperlinkNameInProfessorInternships = true;
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading student details: {ex.Message}");
            // Optionally show error to user
            await JS.InvokeVoidAsync("alert", "Σφάλμα κατά την φόρτωση των στοιχείων του φοιτητή");
        }
    }




    private void CloseModalforHyperLinkTitleStudentName()
    {
        isModalVisibleToShowStudentDetailsAsCompanyFromTheirHyperlinkNameInCompanyInternships = false;
        selectedStudentFromCache = null;
        StateHasChanged();
    }
    private void CloseModalforHyperLinkTitleStudentName_StudentAppliedinternshipsAtProfessor()
    {
        isModalVisibleToShowStudentDetailsAsProfessorFromTheirHyperlinkNameInProfessorInternships = false;
        JS.InvokeVoidAsync("eval", "$('#studentDetailsModal').modal('hide')");
    }

    private async Task WithdrawInternshipApplicationMadeByStudent(InternshipApplied application)
    {
        try
        {
            // First ask for confirmation
            var confirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", 
                $"Πρόκεται να αποσύρετε την Αίτησή σας για την Πρακτική Άσκηση. Είστε σίγουρος/η;");
            if (!confirmed) return;

            // Get the related internship details first - updated property name
            var internship = await dbContext.CompanyInternships
                .Include(i => i.Company) // Include company data
                .FirstOrDefaultAsync(i => i.RNGForInternshipUploadedAsCompany == application.RNGForInternshipApplied);

            if (internship == null)
            {
                await JS.InvokeVoidAsync("alert", "Δεν βρέθηκε η πρακτική άσκηση.");
                return;
            }

            // Update status directly on the main application entity
            application.InternshipStatusAppliedAtTheCompanySide = "Αποσύρθηκε από τον φοιτητή";
            application.InternshipStatusAppliedAtTheStudentSide = "Αποσύρθηκε από τον φοιτητή";

            var platformAction = new PlatformActions
            {
                UserRole_PerformedAction = "STUDENT",
                ForWhat_PerformedAction = "COMPANY_INTERNSHIP",
                HashedPositionRNG_PerformedAction = HashingHelper.HashLong(application.RNGForInternshipApplied),
                TypeOfAction_PerformedAction = "SELFWITHDRAW",
                DateTime_PerformedAction = DateTime.UtcNow
            };

            dbContext.PlatformActions.Add(platformAction);
            await dbContext.SaveChangesAsync();

            // Get student details from your user service
            var student = await GetStudentDetails(application.StudentEmailAppliedForInternship);

            if (student == null)
            {
                await JS.InvokeVoidAsync("alert", "Δεν βρέθηκαν στοιχεία φοιτητή.");
                return;
            }

            // Send notifications - updated to use navigation property with fallback
            await InternshipEmailService.SendInternshipWithdrawalNotificationToCompany_AsStudent(
                application.CompanyEmailWhereStudentAppliedForInternship,
                internship.Company?.CompanyName,
                student.Name,
                student.Surname,
                internship.CompanyInternshipTitle,
                application.RNGForInternshipAppliedAsStudent_HashedAsUniqueID);

            await InternshipEmailService.SendInternshipWithdrawalConfirmationToStudent_AsCompany(
                application.StudentEmailAppliedForInternship,
                student.Name,
                student.Surname,
                internship.CompanyInternshipTitle,
                application.RNGForInternshipAppliedAsStudent_HashedAsUniqueID,
                internship.Company?.CompanyName);

            // Refresh the page to show updated status
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving withdrawal: {ex.Message}");
            await JS.InvokeVoidAsync("alert", "Σφάλμα κατά την αποθήκευση της απόσυρσης.");
        }
    }

    private async Task WithdrawProfessorInternshipApplicationMadeByStudent(ProfessorInternshipApplied application)
    {
        try
        {
            // First ask for confirmation
            var confirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", 
                $"Πρόκεται να αποσύρετε την Αίτησή σας για την Πρακτική Άσκηση. Είστε σίγουρος/η;");
            if (!confirmed) return;

            // Get the related internship details with professor info
            var internship = await dbContext.ProfessorInternships
                .Include(i => i.Professor) // Include professor data
                .FirstOrDefaultAsync(i => i.RNGForInternshipUploadedAsProfessor == application.RNGForProfessorInternshipApplied); // Updated property

            if (internship == null)
            {
                await JS.InvokeVoidAsync("alert", "Δεν βρέθηκε η πρακτική άσκηση.");
                return;
            }

            // Update status directly on the main application entity
            application.InternshipStatusAppliedAtTheProfessorSide = "Αποσύρθηκε από τον φοιτητή";
            application.InternshipStatusAppliedAtTheStudentSide = "Αποσύρθηκε από τον φοιτητή";

            var platformAction = new PlatformActions
            {
                UserRole_PerformedAction = "STUDENT",
                ForWhat_PerformedAction = "PROFESSOR_INTERNSHIP",
                HashedPositionRNG_PerformedAction = HashingHelper.HashLong(application.RNGForProfessorInternshipApplied),
                TypeOfAction_PerformedAction = "SELFWITHDRAW",
                DateTime_PerformedAction = DateTime.UtcNow
            };

            dbContext.PlatformActions.Add(platformAction);
            await dbContext.SaveChangesAsync();

            // Get student details from your user service
            var student = await GetStudentDetails(application.StudentDetails.StudentEmailAppliedForProfessorInternship);

            if (student == null)
            {
                await JS.InvokeVoidAsync("alert", "Δεν βρέθηκαν στοιχεία φοιτητή.");
                return;
            }

            // Send notifications (updated to use professor navigation property)
            await InternshipEmailService.SendProfessorInternshipWithdrawalNotificationToProfessor(
                application.ProfessorDetails.ProfessorEmailWhereStudentAppliedForProfessorInternship,
                $"{internship.Professor.ProfName} {internship.Professor.ProfSurname}", // From navigation property
                student.Name,
                student.Surname,
                internship.ProfessorInternshipTitle,
                application.RNGForProfessorInternshipApplied_HashedAsUniqueID);

            await InternshipEmailService.SendProfessorInternshipWithdrawalConfirmationToStudent(
                application.StudentDetails.StudentEmailAppliedForProfessorInternship,
                student.Name,
                student.Surname,
                internship.ProfessorInternshipTitle,
                application.RNGForProfessorInternshipApplied_HashedAsUniqueID,
                $"{internship.Professor.ProfName} {internship.Professor.ProfSurname}"); // From navigation property

            // Refresh the page to show updated status
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving withdrawal: {ex.Message}");
            await JS.InvokeVoidAsync("alert", "Σφάλμα κατά την αποθήκευση της απόσυρσης.");
        }
    }

    private async Task WithdrawJobApplicationMadeByStudent(CompanyJobApplied application)
    {
        try
        {
            // First ask for confirmation - using job RNG in message since PositionTitle isn't available
            var confirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", 
                $"Πρόκεται να αποσύρετε την Αίτησή σας για την Θέση Εργασίας. Είστε σίγουρος/η;");
            if (!confirmed) return;

            // Get the related job details first
            var job = await dbContext.CompanyJobs
                .FirstOrDefaultAsync(j => j.RNGForPositionUploaded == application.RNGForCompanyJobApplied);

            if (job == null)
            {
                await JS.InvokeVoidAsync("alert", "Δεν βρέθηκε η θέση εργασίας.");
                return;
            }

            // Update status directly on the main application entity
            application.CompanyPositionStatusAppliedAtTheCompanySide = "Αποσύρθηκε από τον φοιτητή";
            application.CompanyPositionStatusAppliedAtTheStudentSide = "Αποσύρθηκε από τον φοιτητή";

            var platformAction = new PlatformActions
            {
                UserRole_PerformedAction = "STUDENT",
                ForWhat_PerformedAction = "COMPANY_JOB",
                HashedPositionRNG_PerformedAction = HashingHelper.HashLong(application.RNGForCompanyJobApplied),
                TypeOfAction_PerformedAction = "SELFWITHDRAW",
                DateTime_PerformedAction = DateTime.UtcNow
            };

            dbContext.PlatformActions.Add(platformAction);
            await dbContext.SaveChangesAsync();

            // Get student details from your user service
            var student = await GetStudentDetails(application.StudentEmailAppliedForCompanyJob);
    
            if (student == null)
            {
                await JS.InvokeVoidAsync("alert", "Δεν βρέθηκαν στοιχεία φοιτητή.");
                return;
            }

            // Send notifications
            await InternshipEmailService.SendJobWithdrawalNotificationToCompany_AsStudent(
                application.CompanysEmailWhereStudentAppliedForCompanyJob,
                job.Company?.CompanyName,  // From Company navigation property
                student.Name,
                student.Surname,
                job.PositionTitle,
                application.RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID);

            await InternshipEmailService.SendJobWithdrawalConfirmationToStudent_AsCompany(
                application.StudentEmailAppliedForCompanyJob,
                student.Name,
                student.Surname,
                job.PositionTitle,
                application.RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID,
                job.Company?.CompanyName);  // From Company navigation property

            // Refresh the page to show updated status
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving withdrawal: {ex.Message}");
            await JS.InvokeVoidAsync("alert", "Σφάλμα κατά την αποθήκευση της απόσυρσης.");
        }
    }

    private async Task WithdrawCompanyThesisApplicationMadeByStudent(CompanyThesisApplied companythesis)
    {
        companythesis.CompanyThesisStatusAppliedAtStudentSide = "Αποσύρθηκε από τον φοιτητή";
        companythesis.CompanyThesisStatusAppliedAtCompanySide = "Αποσύρθηκε από τον φοιτητή"; 
        await dbContext.SaveChangesAsync();
    }

    private async Task WithdrawProfessorThesisApplicationMadeByStudent(ProfessorThesisApplied professorthesis)
    {
        professorthesis.ProfessorThesisStatusAppliedAtStudentSide = "Αποσύρθηκε από τον φοιτητή";
        professorthesis.ProfessorThesisStatusAppliedAtProfessorSide = "Αποσύρθηκε από τον φοιτητή"; 
        await dbContext.SaveChangesAsync();

    }

    private async Task DownloadStudentCVFromCompanyInternships(string studentEmail)
    {
        try
        {
            // First try to find in cache
            var student = studentDataCache.Values.FirstOrDefault(s => s.Email == studentEmail);
    
            // If not in cache, try database
            if (student == null)
            {
                student = await dbContext.Students
                    .FirstOrDefaultAsync(s => s.Email == studentEmail);
            }

            if (student?.Attachment == null)
            {
                await JS.InvokeVoidAsync("alert", "Δεν βρέθηκε βιογραφικό για αυτόν τον φοιτητή");
                return;
            }

            var fileName = $"CV_{student.Name}_{student.Surname}.pdf";
            var mimeType = "application/pdf";
        
            // Correct parameter order to match the working example
            await JS.InvokeVoidAsync("downloadFile", fileName, mimeType, Convert.ToBase64String(student.Attachment));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading CV: {ex.Message}");
            await JS.InvokeVoidAsync("alert", "Σφάλμα κατά τη λήψη του βιογραφικού");
        }
    }

    private async Task ShowCompanyDetailsAsAHyperlinkInInternshipSearchAsStudent(string companyId)
    {
        // Fetch the company details based on the Company Name (ENG)
        selectedCompany = await dbContext.Companies.FirstOrDefaultAsync(c => c.CompanyName == companyId);

        if (selectedCompany != null)
        {
            isCompanyDetailsModal2Visible = true;
            await JS.InvokeVoidAsync("eval", "$('#companyDetailsModal2').modal('show')");
        }
    }

    private async Task CloseModalforHyperLinkTitleInSearch()
    {
        // Close the modal and set the flag to false
        isCompanyDetailsModal2Visible = false;
        await JS.InvokeVoidAsync("eval", "$('#companyDetailsModal2').modal('hide')");
    }

    private async Task ShowCompanyDetailsAsAHyperlinkInJobSearchAsStudent(string companyName)
    {
        // Fetch company details from database
        selectedCompanyDetailsForJobSearch = await dbContext.Companies
            .FirstOrDefaultAsync(c => c.CompanyName == companyName);

        if (selectedCompanyDetailsForJobSearch != null)
        {
            isCompanyDetailsModalOpenForJobSearch = true;
            StateHasChanged();  // Refresh UI
        }
        else
        {
            await JS.InvokeVoidAsync("alert", "Company details not found.");
        }
    }

    private void CloseModalForCompanyNameHyperlinkDetailsInJobSearch()
    {
        isCompanyDetailsModalOpenForJobSearch = false;
    }



    private async Task ShowCompanyDetailsAsAHyperlinkInShowJobsAsStudent(string companyEmail)
    {
        try
        {
            // First try to find in job cache
            var cachedCompany = jobDataCache.Values
                .FirstOrDefault(j => j.EmailUsedToUploadJobs.Equals(companyEmail, StringComparison.OrdinalIgnoreCase));
        
            if (cachedCompany != null)
            {
                selectedCompanyDetailsForJobShow = await dbContext.Companies
                    .FirstOrDefaultAsync(c => c.CompanyEmail == cachedCompany.EmailUsedToUploadJobs);
            }
            else
            {
                // Fallback to direct lookup
                selectedCompanyDetailsForJobShow = await dbContext.Companies
                    .FirstOrDefaultAsync(c => c.CompanyEmail == companyEmail);
            }

            isCompanyDetailsModalOpenForJobShow = selectedCompanyDetailsForJobShow != null;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error showing company details: {ex.Message}");
            await JS.InvokeVoidAsync("alert", "Σφάλμα φόρτωσης στοιχείων εταιρείας");
        }
    }

    private void CloseModalForCompanyNameHyperlinkDetailsInJobShow()
    {
        isCompanyDetailsModalOpenForJobShow = false;
    }



    private async Task ShowInternshipDetailsInInternshipTitleAsHyperlink(long internshipId)
    {
        currentInternship = await dbContext.CompanyInternships
            .Include(i => i.Company) // Include company data
            .FirstOrDefaultAsync(i => i.RNGForInternshipUploadedAsCompany == internshipId); // Updated property name

        if (currentInternship != null)
        {
            isInternshipDetailsModalVisible = true;
            StateHasChanged();
        }
    }

    private async Task CloseInternshipDetailsModal()
    {
        // Set the flag to hide the modal
        isInternshipDetailsModalVisible = false;
        StateHasChanged();
    }

    private async Task ShowProfessorInternshipDetailsInInternshipTitleAsHyperlink_StudentInternshipApplicationsShow(long internshipId)
    {
        // Fetch the internship details from the database with professor information
        currentProfessorInternship = await dbContext.ProfessorInternships
            .Include(i => i.Professor) // Include professor data
            .FirstOrDefaultAsync(i => i.RNGForInternshipUploadedAsProfessor == internshipId); // Updated property name

        if (currentProfessorInternship != null)
        {
            // Set the flag to show the modal
            isInternshipDetailsModalVisible_StudentInternshipApplicationsShow = true;

            // Trigger modal visibility change via JavaScript
            await JS.InvokeVoidAsync("eval", 
                $"$('#internshipDetailsModal').modal('{(isInternshipDetailsModalVisible_StudentInternshipApplicationsShow ? "show" : "hide")}')");
        }
    }

    private async Task CloseProfessorInternshipDetailsModal_StudentInternshipApplicationsShow()
    {
        // Set the flag to hide the modal
        isInternshipDetailsModalVisible_StudentInternshipApplicationsShow = false;
        StateHasChanged();
    }

    private async Task ShowJobDetailsInJobTitleAsHyperlink_StudentJobApplications(long jobId)
    {
        // Fetch the job details from the database with Company included
        currentJobApplicationMadeAsStudent = await dbContext.CompanyJobs
            .Include(j => j.Company) // Include the Company data
            .FirstOrDefaultAsync(i => i.RNGForPositionUploaded == jobId);

        if (currentJobApplicationMadeAsStudent != null)
        {
            isJobDetailsModalVisibleToSeeJobApplicationsAsStudent = true;
            StateHasChanged(); // Update UI
        }
        else
        {
            await JS.InvokeVoidAsync("alert", "Job details not found.");
        }
    }

    private void CloseJobDetailsModalInJobTitleAsHyperlink_StudentJobApplications()
    {
        isJobDetailsModalVisibleToSeeJobApplicationsAsStudent = false;
        StateHasChanged();
    }



    private async Task CloseJobDetailsModal()
    {
        isJobDetailsModal2Visible = false;
        await  JS.InvokeVoidAsync("eval", "$('#jobDetailsModal2').modal('hide')");
    }

    private async Task DownloadInternshipAttachmentAsStudent(long internshipId)
    {
        var internship = dbContext.CompanyInternships.FirstOrDefault(i => i.Id == internshipId);
        if (internship == null)
        {
            Console.WriteLine("Internship not found.");
            return;
        }

        if (internship.CompanyInternshipAttachment == null)
        {
            Console.WriteLine("No attachment found.");
            return;
        }

        Console.WriteLine($"Attachment found for internship {internshipId}, size: {internship.CompanyInternshipAttachment.Length} bytes");

        // Convert byte[] to base64
        string base64String = Convert.ToBase64String(internship.CompanyInternshipAttachment);
        string fileName = $"Internship_Attachment_{internshipId}.pdf"; // Adjust file extension if needed

        // Invoke JS to download file
        await JS.InvokeVoidAsync("downloadInternshipAttachmentAsStudent", fileName, base64String);
    }

    private async Task HandleSaveClickToSaveInternshipsAsCompany()
    {
        // Call JavaScript function for confirmation with custom HTML and styling
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] {
            "Είστε σίγουροι πως θέλετε να υποβάλλετε Νέα Θέση Πρακτικής Άσκησης; Η Θέση θα καταχωρηθεί ως 'Μη Δημοσιευμένη' στο Ιστορικό Θέσεων Πρακτικής Άσκησης. " +
            "Αν επιθυμείτε να την Δημοσιεύσετε απαιτούνται επιπλέον ενέργειες! <br>" +
            "<strong style='color: red;'>Παρακαλώ επιβεβαιώστε την ενέργειά σας.</strong>"
    });

        if (isConfirmed)
        {
            // Proceed with the form submission
            await UploadInternshipAsCompany();
        }
    }



    private async Task ChangeInternshipStatusToUnpublished(int internshipId)
    {
        // Show custom confirmation dialog with formatted text
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML",
            "Προχωράτε στην <strong style='color: red;'>Αποδημοσίευση</strong> της Θέσης.<br><br>" + 
            "<strong style='color: red;'>ΔΕΝ θα είναι ορατή σε νέους υποψηφίους</strong>.<br><br>" + 
            "Η κατάσταση των αιτήσεων παλαιότερων υποψηφίων θα παραμείνει ως έχει.<br><br>" + 
            "Θέλετε σίγουρα να συνεχίσετε;");

        if (isConfirmed)
        {
            // Proceed with status update to "Μη Δημοσιευμένη"
            await UpdateInternshipStatusAsCompany(internshipId, "Μη Δημοσιευμένη");
        }
    }



    private async Task ChangeProfessorInternshipStatusToUnpublished(int internshipId)
    {
        // Show confirmation dialog
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML",
            "Προχωράτε στην Αποδημοσίευση της Θέσης. Η Θέση μετά από αυτήν την ενέργεια ΔΕΝ θα είναι ορατή σε νέους υποψηφίους. Η κατάσταση των αιτήσεων παλαιότερων υποψηφίων θα παραμείνει ως έχει. Θέλετε σίγουρα να συνεχίσετε ;");

        if (isConfirmed)
        {
            // Proceed with status update
            await UpdateInternshipStatusAsProfessor(internshipId, "Μη Δημοσιευμένη");
        }
    }

    private async Task ChangeJobStatusToUnpublished(int jobId)
    {
        // Show custom confirmation dialog with formatted text
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML",
            "Προχωράτε στην <strong style='color: red;'>Αποδημοσίευση</strong> της Θέσης.<br><br>" + 
            "<strong style='color: red;'>ΔΕΝ θα είναι ορατή σε νέους υποψηφίους</strong>.<br><br>" + 
            "Η κατάσταση των αιτήσεων παλαιότερων υποψηφίων θα παραμείνει ως έχει.<br><br>" + 
            "Θέλετε σίγουρα να συνεχίσετε;");

        if (isConfirmed)
        {
            // Proceed with status update
            await UpdateJobStatusAsCompany(jobId, "Μη Δημοσιευμένη");
        }
    }



    private async Task ChangeCompanyThesisStatusToUnpublished(int companyThesisId)
    {
        // Show custom confirmation dialog with formatted text
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML",
            "Προχωράτε στην <strong style='color: red;'>Αποδημοσίευση</strong> της Πτυχιακής Εργασίας.<br><br>" + 
            "<strong style='color: red;'>ΔΕΝ θα είναι ορατή σε νέους υποψηφίους</strong>.<br><br>" + 
            "Η κατάσταση των αιτήσεων παλαιότερων υποψηφίων θα παραμείνει ως έχει.<br><br>" + 
            "Θέλετε σίγουρα να συνεχίσετε;");

        if (isConfirmed)
        {
            // Proceed with status update
            await UpdateThesisStatusAsCompany(companyThesisId, "Μη Δημοσιευμένη");
        }
    }


    private async Task HandlePublishClickToSaveInternshipsAsCompany()
    {
        // Call JavaScript function for confirmation with custom HTML and styling
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] {
            "Είστε σίγουροι πως θέλετε να υποβάλλετε Νέα Θέση Πρακτικής Άσκησης; Η Θέση θα καταχωρηθεί ως 'Δημοσιευμένη' στο Ιστορικό Θέσεων Πρακτικής Άσκησης. " +
            "Αν επιθυμείτε να την Αποδημοσιεύσετε απαιτούνται επιπλέον ενέργειες! <br>" +
            "<strong style='color: red;'>Παρακαλώ επιβεβαιώστε την ενέργειά σας.</strong>"
    });

        if (isConfirmed)
        {
            // Set status to Δημοσιευμένη
            companyInternship.CompanyUploadedInternshipStatus = "Δημοσιευμένη";

            // Proceed with the form submission
            await UploadInternshipAsCompany();
        }
    }



    private void UpdateTownsForSelectedRegion(ChangeEventArgs e)
    {
        selectedRegion = e.Value?.ToString();
        if (!string.IsNullOrEmpty(selectedRegion) && RegionToTownsMap.ContainsKey(selectedRegion))
        {
            availableTowns = RegionToTownsMap[selectedRegion];
        }
        else
        {
            availableTowns = null; // Clear the town selection if no region is selected
        }
    }

    private void OnTownChangeForInternships(ChangeEventArgs e)
    {
        selectedInternship.CompanyInternshipDimosLocation = e.Value?.ToString();
    }

    private void LogJobLoadingInfo()
    {
        var message = $"Loaded jobs for company: {companyName} \n" +
                      $"Total Jobs: {totalCountJobs} \n" +
                      $"Published Jobs: {publishedCountJobs} \n" +
                      $"Unpublished Jobs: {unpublishedCountJobs} \n" +
                      $"Withdrawn Jobs: {withdrawnCountJobs}";

        Console.WriteLine(message); // This writes to the console
    }

    private async Task HandleTemporarySaveJobAsCompany()
    {
        // Call JavaScript function for confirmation with HTML content and custom styling
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
            "Είστε σίγουροι πως θέλετε να υποβάλλετε Νέα Θέση Εργασίας; Η Θέση θα καταχωρηθεί ως 'Μη Δημοσιευμένη' στο Ιστορικό Θέσεων Εργασίας. <br>" +
            "<strong style='color: red;'>Αν επιθυμείτε να την Δημοσιεύσετε απαιτούνται επιπλέον ενέργειες!</strong>"
    });

        if (isConfirmed)
        {
            // Proceed with the temporary save
            await UploadJobAsCompany(false);
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
         StateHasChanged();
    }

    private async Task HandlePublishSaveJobAsCompany()
    {
        // Call JavaScript function for confirmation with HTML content and custom styling
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
            "Είστε σίγουροι πως θέλετε να υποβάλλετε Νέα Θέση Εργασίας; Η Θέση θα καταχωρηθεί ως 'Δημοσιευμένη' στο Ιστορικό Θέσεων Εργασίας. <br>" +
            "<strong style='color: red;'>Αν επιθυμείτε να την Αποδημοσιεύσετε απαιτούνται επιπλέον ενέργειες!</strong>"
    });

        if (isConfirmed)
        {
            // Proceed with publishing the job
            await UploadJobAsCompany(true);
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
        StateHasChanged();
    }


    private async Task HandleTemporarySaveThesisAsCompany()
    {
        // Show custom confirmation dialog with formatted text
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML",
            "Είστε σίγουροι πως θέλετε να <strong style='color: blue;'>Υποβάλετε</strong> Νέα Πτυχιακή Εργασία;<br><br>" +
            "Η Εργασία θα καταχωρηθεί ως <strong style='color: red;'>'Μη Δημοσιευμένη'</strong> στο Ιστορικό Θέσεων Πτυχιακών Εργασιών.<br><br>" +
            "Αν επιθυμείτε να την Δημοσιεύσετε απαιτούνται επιπλέον ενέργειες!<br><br>" +
            "Θέλετε να συνεχίσετε;");

        if (isConfirmed)
        {
            // Proceed with the temporary save
            await UploadThesisAsCompany(false);
        }
        StateHasChanged();
    }

    private async Task HandlePublishSaveThesisAsCompany()
    {
        // Show custom confirmation dialog with formatted text
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML",
            "Είστε σίγουροι πως θέλετε να <strong style='color: green;'>Υποβάλετε</strong> Νέα Πτυχιακή Εργασία;<br><br>" +
            "Η Εργασία θα καταχωρηθεί ως <strong style='color: green;'>'Δημοσιευμένη'</strong> στο Ιστορικό Θέσεων Πτυχιακών Εργασιών.<br><br>" +
            "Αν επιθυμείτε να την <strong style='color: red;'>Αποδημοσιεύσετε</strong> απαιτούνται επιπλέον ενέργειες!<br><br>" +
            "Θέλετε να συνεχίσετε;");

        if (isConfirmed)
        {
            // Proceed with publishing the thesis
            await UploadThesisAsCompany(true);
        }
        StateHasChanged();
    }



    private async Task SaveAnnouncementAsPublished()
    {
        // Validate mandatory fields
        isFormValidToSaveAnnouncementAsCompany = ValidateMandatoryFieldsForUploadAnnouncementAsCompany();

        if (!isFormValidToSaveAnnouncementAsCompany)
            return;

        // Show custom confirmation dialog with HTML styling
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
            $"Πρόκεται να Δημιουργήσετε μια Ανακοίνωση με Τίτλο: <strong>{announcement.CompanyAnnouncementTitle}</strong> ως '<strong>Δημοσιευμένη</strong>'.<br><br>" +
            "<strong style='color: red;'>Είστε σίγουρος/η;</strong>"
        });

        if (!isConfirmed)
            return;

        announcement.CompanyAnnouncementStatus = "Δημοσιευμένη";
        announcement.CompanyAnnouncementUploadDate = DateTime.Now;
        announcement.CompanyAnnouncementCompanyEmail = CurrentUserEmail;
        announcement.CompanyAnnouncementRNG = new Random().NextInt64();
        announcement.CompanyAnnouncementRNG_HashedAsUniqueID = HashingHelper.HashLong(announcement.CompanyAnnouncementRNG ?? 0);
        await SaveAnnouncementToDatabase();
    }

    private async Task SaveAnnouncementAsUnpublished()
    {
        // Validate mandatory fields
        isFormValidToSaveAnnouncementAsCompany = ValidateMandatoryFieldsForUploadAnnouncementAsCompany();

        if (!isFormValidToSaveAnnouncementAsCompany)
            return;

        // Show custom confirmation dialog with HTML styling
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
            $"Πρόκεται να Δημιουργήσετε μια Ανακοίνωση με Τίτλο: <strong>{announcement.CompanyAnnouncementTitle}</strong> ως '<strong>Μη Δημοσιευμένη (Προσωρινή Αποθήκευση)</strong>'.<br><br>" +
            "<strong style='color: red;'>Είστε σίγουρος/η;</strong>"
        });

        if (!isConfirmed)
            return;

        announcement.CompanyAnnouncementStatus = "Μη Δημοσιευμένη";
        announcement.CompanyAnnouncementUploadDate = DateTime.Now;
        announcement.CompanyAnnouncementCompanyEmail = CurrentUserEmail;
        announcement.CompanyAnnouncementRNG = new Random().NextInt64();
        announcement.CompanyAnnouncementRNG_HashedAsUniqueID = HashingHelper.HashLong(announcement.CompanyAnnouncementRNG ?? 0);
        await SaveAnnouncementToDatabase();
    }



    // Method to validate mandatory fields
    private bool ValidateMandatoryFieldsForUploadAnnouncementAsCompany()
    {
        // Check if mandatory fields are filled, including date validation for today's date
        if (string.IsNullOrWhiteSpace(announcement.CompanyAnnouncementTitle) || 
            string.IsNullOrWhiteSpace(announcement.CompanyAnnouncementDescription) ||
            announcement.CompanyAnnouncementTimeToBeActive.Date == DateTime.Today) // Ensure date is not today's date
        {
            // Trigger error message and return false for form validity
            showErrorMessageforUploadingAnnouncementAsCompany = true;
            return false;
        }

        // No errors, form is valid
        showErrorMessageforUploadingAnnouncementAsCompany = false;
        return true;
    }



    private async Task SaveAnnouncementToDatabase()
    {
        try
        {
            // Check each required field and scroll to it if it is missing
            if (string.IsNullOrWhiteSpace(announcement.CompanyAnnouncementTitle))
            {
                showErrorMessageforUploadingAnnouncementAsCompany = true;
                await JS.InvokeVoidAsync("scrollToElementById", "announcementTitle");
                return;
            }

            if (string.IsNullOrWhiteSpace(announcement.CompanyAnnouncementDescription))
            {
                showErrorMessageforUploadingAnnouncementAsCompany = true;
                await JS.InvokeVoidAsync("scrollToElementById", "announcementDescription");
                return;
            }

            if (announcement.CompanyAnnouncementTimeToBeActive.Date <= DateTime.Today)
            {
                showErrorMessageforUploadingAnnouncementAsCompany = true;
                await JS.InvokeVoidAsync("scrollToElementById", "announcementActiveDate");
                return;
            }

            // All validations passed - proceed with saving
            dbContext.AnnouncementsAsCompany.Add(announcement);
            await dbContext.SaveChangesAsync();
    
            isSaveAnnouncementAsCompanySuccessful = true;
            saveAnnouncementAsCompanyMessage = "Η Ανακοίνωση Δημιουργήθηκε Επιτυχώς";
    
            // Clear form or reset as needed
            announcement = new AnnouncementAsCompany();
        }
        catch (Exception ex)
        {
            isSaveAnnouncementAsCompanySuccessful = false;
            saveAnnouncementAsCompanyMessage = "Κάποιο πρόβλημα παρουσιάστηκε με την Δημιουργία της Ανακοίνωσης! Ανανεώστε την σελίδα και προσπαθήστε ξανά";
            Console.WriteLine($"Error saving announcement: {ex.Message}");
        }

        StateHasChanged();
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }



    private void TogglePositionAreasVisibility()
    {
        isPositionAreasVisible = !isPositionAreasVisible;
    }

    private void ToggleThesisAreasVisibility()
    {
        isThesisAreasVisible = !isThesisAreasVisible;
    }

    private void OnPositionAreaCheckboxChanged(ChangeEventArgs e, string areaName)
    {
        if (e.Value != null && bool.TryParse(e.Value.ToString(), out bool isChecked))
        {
            if (isChecked)
            {
                if (!selectedPositionAreas.Contains(areaName))
                {
                    selectedPositionAreas.Add(areaName);
                }
            }
            else
            {
                selectedPositionAreas.Remove(areaName);
            }
        }
    }

    private void OnThesisAreaCheckboxChanged(ChangeEventArgs e, string areaName)
    {
        if (e.Value != null && bool.TryParse(e.Value.ToString(), out bool isChecked))
        {
            if (isChecked)
            {
                if (!selectedThesisAreas.Contains(areaName))
                {
                    selectedThesisAreas.Add(areaName);
                }
            }
            else
            {
                selectedThesisAreas.Remove(areaName);
            }
        }
    }

    public async Task DeleteExpiredAnnouncements()
    {
        var expiredAnnouncements = await dbContext.AnnouncementsAsCompany
            .Where(a => a.CompanyAnnouncementTimeToBeActive < DateTime.Now)
            .ToListAsync();

        dbContext.AnnouncementsAsCompany.RemoveRange(expiredAnnouncements);
        await dbContext.SaveChangesAsync();
    }

    private async Task ToggleUploadedAnnouncementsVisibility()
    {
        isUploadedAnnouncementsVisible = !isUploadedAnnouncementsVisible;

        if (isUploadedAnnouncementsVisible)
        {
            // Load announcements only when the section is visible
            await LoadUploadedAnnouncementsAsync();
        }

        StateHasChanged();
    }

    private async Task ToggleUploadedCompanyEventsVisibility()
    {
        isUploadedEventsVisible = !isUploadedEventsVisible;

        if (isUploadedEventsVisible)
        {
            // Load announcements only when the section is visible
            await LoadUploadedEventsAsync();
        }

        StateHasChanged();
    }

    private async Task ToggleUploadedProfessorEventsVisibility()
    {
        isUploadedEventsVisibleAsProfessor = !isUploadedEventsVisibleAsProfessor;

        if (isUploadedEventsVisibleAsProfessor)
        {
            // Load announcements only when the section is visible
            await LoadUploadedEventsAsyncAsProfessor();
        }

        StateHasChanged();
    }

    private async Task ToggleUploadedAnnouncementsVisibilityAsProfessor()
    {
        isUploadedAnnouncementsVisibleAsProfessor = !isUploadedAnnouncementsVisibleAsProfessor;

        if (isUploadedAnnouncementsVisibleAsProfessor)
        {
            // Load announcements only when the section is visible
            await LoadUploadedAnnouncementsAsProfessorAsync();
        }

        StateHasChanged();
    }

    private async Task ToggleCompanySearchStudentVisible()
    {
        isCompanySearchStudentVisible = !isCompanySearchStudentVisible;
        StateHasChanged();
    }

    private async Task ToggleCompanySearchProfessorVisible()
    {
        isCompanySearchProfessorVisible = !isCompanySearchProfessorVisible;
        StateHasChanged();
    }

    private async Task ToggleCompanySearchResearchGroupVisible()
    {
        isCompanySearchResearchGroupVisible = !isCompanySearchResearchGroupVisible;
        StateHasChanged();
    }

    private async Task ToggleRGSearchProfessorVisible()
    {
        isRGSearchProfessorVisible = !isRGSearchProfessorVisible;
        StateHasChanged();
    }


    private async Task ToggleUploadedThesesVisibilityAsProfessor()
    {
        isUploadedThesesVisibleAsProfessor = !isUploadedThesesVisibleAsProfessor;

        if (isUploadedThesesVisibleAsProfessor)
        {
            // Load announcements only when the section is visible
            await LoadUploadedThesesAsProfessorAsync();
        }

        StateHasChanged();
    }

    private void ToggleToSearchForUploadedCompanyThesesAsProfessor()
    {
        isUploadedCompanyThesesVisibleAsProfessor = !isUploadedCompanyThesesVisibleAsProfessor;

        // Clear search results if toggling off the search form
        if (!isUploadedCompanyThesesVisibleAsProfessor)
        {
            companyThesesResultsToFindThesesAsProfessor = null;
            searchPerformedToFindThesesAsProfessor = false;
        }
    }

    private void ToggleToSearchForUploadedProfessorThesesAsCompany()
    {
        isUploadedProfessorThesesVisibleAsCompany = !isUploadedProfessorThesesVisibleAsCompany;

        // Clear search results if toggling off the search form
        if (!isUploadedProfessorThesesVisibleAsCompany)
        {
            professorThesesResultsToFindThesesAsCompany = null;
            searchPerformedToFindThesesAsCompany = false;
        }
    }

    private async Task<List<AnnouncementAsCompany>> GetUploadedAnnouncements()
    {
        return await dbContext.AnnouncementsAsCompany
            .Where(a => a.CompanyAnnouncementCompanyEmail == CurrentUserEmail)
            .ToListAsync();
    }

    private async Task<List<CompanyEvent>> GetUploadedCompanyEvents()
    {
        return await dbContext.CompanyEvents
            .Include(e => e.Company) // Include the Company data
            .Where(e => e.CompanyEmailUsedToUploadEvent == CurrentUserEmail)
            .ToListAsync();
    }

    private async Task<List<ProfessorEvent>> GetUploadedProfessorEvents()
    {
        return await dbContext.ProfessorEvents
            .Include(pe => pe.Professor) // Include the Professor data
            .Where(pe => pe.ProfessorEmailUsedToUploadEvent == CurrentUserEmail)
            .ToListAsync();
    }

    private async Task<List<AnnouncementAsProfessor>> GetUploadedAnnouncementsAsProfessor()
    {
        return await dbContext.AnnouncementsAsProfessor
            .Where(a => a.ProfessorAnnouncementProfessorEmail == CurrentUserEmail)
            .ToListAsync();
    }

    private async Task DeleteAnnouncement(int announcementId)
    {
        // Show custom confirmation dialog with formatted text
        var isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML",
            "Πρόκειται να διαγράψετε οριστικά αυτή την Ανακοίνωση.<br><br>" + 
            "<strong style='color: red;'>Είστε σίγουρος/η;</strong>");

        if (isConfirmed)
        {
            // Proceed with deletion
            var announcement = await dbContext.AnnouncementsAsCompany.FindAsync(announcementId);
            if (announcement != null)
            {
                dbContext.AnnouncementsAsCompany.Remove(announcement);
                await dbContext.SaveChangesAsync();

                // Refresh the list after deletion
                UploadedAnnouncements = await GetUploadedAnnouncements();
            }

            // Trigger UI refresh
            StateHasChanged();
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
    }



    private async Task DeleteCompanyEvent(int companyeventId)
    {
        // Show custom confirmation dialog with formatted text
        var isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML",
            "<strong style='color: red;'>Προσοχή!</strong><br><br>" +
            "Πρόκειται να <strong style='color: red;'>διαγράψετε οριστικά</strong> αυτή την Εκδήλωση.<br><br>" +
            "Αυτή η ενέργεια <strong>δεν μπορεί να αναιρεθεί</strong>.<br><br>" +
            "Είστε σίγουρος/η;");

        if (isConfirmed)
        {
            // Proceed with deletion
            var companyevent = await dbContext.CompanyEvents.FindAsync(companyeventId);
            if (companyevent != null)
            {
                dbContext.CompanyEvents.Remove(companyevent);
                await dbContext.SaveChangesAsync();

                // Refresh the lists
                UploadedEventsAsCompany = await GetUploadedCompanyEvents();
                FilteredCompanyEvents = UploadedEventsAsCompany.ToList();
            }

            // Trigger UI refresh
            StateHasChanged();
        }
    }


    private async Task DeleteProfessorEvent(int professoreventId)
    {
        // Show a confirmation dialog
        var isConfirmed = await JS.InvokeAsync<bool>("confirm", new object[] { "Πρόκειται να διαγράψετε οριστικά αυτή την Εκδήλωση είστε σίγουρος/η;" });

        if (isConfirmed)
        {
            // Proceed with deletion
            var professorevent = await dbContext.ProfessorEvents.FindAsync(professoreventId);
            if (professorevent != null)
            {
                dbContext.ProfessorEvents.Remove(professorevent);
                await dbContext.SaveChangesAsync();

                // Refresh the lists
                UploadedEventsAsProfessor = await GetUploadedProfessorEvents();
                FilteredProfessorEvents = UploadedEventsAsProfessor.ToList();
            }

            // Trigger UI refresh
            StateHasChanged();
        }
    }


    private async Task DeleteAnnouncementAsProfessor(int professorannouncementId)
    {
        // Delete the selected announcement from the database
        var professorannouncement = await dbContext.AnnouncementsAsProfessor.FindAsync(professorannouncementId);
        if (professorannouncement != null)
        {
            dbContext.AnnouncementsAsProfessor.Remove(professorannouncement);
            await dbContext.SaveChangesAsync();

            // Refresh the list after deletion
            UploadedAnnouncementsAsProfessor = await GetUploadedAnnouncementsAsProfessor();
        }
        StateHasChanged();
    }

    private async Task LoadUploadedAnnouncementsAsync()
    {
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (!string.IsNullOrEmpty(userEmail))
            {
                // Fetch announcements related to the current company
                UploadedAnnouncements = await dbContext.AnnouncementsAsCompany
                    .Where(i => i.CompanyAnnouncementCompanyEmail == userEmail)
                    .ToListAsync();

                // Check the loaded data
                if (UploadedAnnouncements == null || !UploadedAnnouncements.Any())
                {
                    Console.WriteLine("No Announcements found for this user.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading Announcements: {ex.Message}");
        }
        StateHasChanged();
    }

    private async Task LoadUploadedAnnouncementsAsProfessorAsync()
    {
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (!string.IsNullOrEmpty(userEmail))
            {
                // Fetch announcements related to the current company
                UploadedAnnouncementsAsProfessor = await dbContext.AnnouncementsAsProfessor
                    .Where(i => i.ProfessorAnnouncementProfessorEmail == userEmail)
                    .ToListAsync();

                // Check the loaded data
                if (UploadedAnnouncementsAsProfessor == null || !UploadedAnnouncementsAsProfessor.Any())
                {
                    Console.WriteLine("No Announcements found for this user.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading Announcements: {ex.Message}");
        }
        StateHasChanged();
    }

    private async Task LoadUploadedEventsAsync()
    {
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (!string.IsNullOrEmpty(userEmail))
            {
                // Fetch events related to the current company with Company data included
                UploadedEventsAsCompany = await dbContext.CompanyEvents
                    .Include(e => e.Company) // Include the Company data
                    .Where(e => e.CompanyEmailUsedToUploadEvent == userEmail)
                    .ToListAsync();

                // Check the loaded data
                if (UploadedEventsAsCompany == null || !UploadedEventsAsCompany.Any())
                {
                    Console.WriteLine("No Events found for this user.");
                }
                else
                {
                    Console.WriteLine($"Found {UploadedEventsAsCompany.Count} events for company: {userEmail}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading Events: {ex.Message}");
        }
        StateHasChanged();
    }

    private async Task LoadUploadedEventsAsyncAsProfessor()
    {
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (!string.IsNullOrEmpty(userEmail))
            {
                // Fetch events related to the current professor
                UploadedEventsAsProfessor = await dbContext.ProfessorEvents
                    .Include(pe => pe.Professor) // Include professor data
                    .Where(pe => pe.ProfessorEmailUsedToUploadEvent == userEmail)
                    .ToListAsync();

                // Check the loaded data
                if (UploadedEventsAsProfessor == null || !UploadedEventsAsProfessor.Any())
                {
                    Console.WriteLine("No Events found for this professor.");
                }
                else
                {
                    // Example of accessing professor data through navigation property
                    foreach (var ev in UploadedEventsAsProfessor)
                    {
                        Console.WriteLine($"Event {ev.Id} by {ev.Professor?.ProfName} from {ev.Professor?.ProfUniversity}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading Events: {ex.Message}");
        }
        StateHasChanged();
    }

    private async Task LoadUploadedThesesAsProfessorAsync()
    {
        try
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (!string.IsNullOrEmpty(userEmail))
            {
                // Fetch theses with professor details using navigation property
                UploadedThesesAsProfessor = await dbContext.ProfessorTheses
                    .Include(t => t.Professor) // Include professor details
                    .Where(t => t.ProfessorEmailUsedToUploadThesis == userEmail)
                    .OrderByDescending(t => t.ThesisUploadDateTime) // Newest first
                    .ToListAsync();

                if (UploadedThesesAsProfessor == null || !UploadedThesesAsProfessor.Any())
                {
                    Console.WriteLine("No theses found for this professor.");
                }
                else
                {
                    // Log loaded theses count for debugging
                    Console.WriteLine($"Loaded {UploadedThesesAsProfessor.Count} theses for professor {userEmail}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading professor theses: {ex.Message}");
            // Consider adding user-friendly error notification
            await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("showErrorToast", 
                "Σφάλμα φόρτωσης πτυχιακών").AsTask());
        }
        finally
        {
            StateHasChanged();
        }
    }

    private async Task LoadToSeeUploadedCompanyThesesAsProfessorAsync()
    {
        try
        {
            // Load all company theses for the professor to see
            UploadedCompanyThesesToSeeAsProfessor = await dbContext.CompanyTheses.ToListAsync();

            // Check if any theses were found
            if (UploadedCompanyThesesToSeeAsProfessor == null || !UploadedCompanyThesesToSeeAsProfessor.Any())
            {
                Console.WriteLine("No Company Theses found for the Professor To See");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading Company Theses: {ex.Message}");
        }
        StateHasChanged();
    }

    private async Task ChangeAnnouncementStatus(int announcementId, string newStatus)
    {
        // Show custom confirmation dialog with formatted text
        var isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML",
            $"Πρόκειται να αλλάξετε την κατάσταση αυτής της Ανακοίνωσης σε <strong>{newStatus}</strong>.<br><br>" +
            "<strong style='color: red;'>Είστε σίγουρος/η;</strong>");

        // Proceed only if confirmed
        if (isConfirmed)
        {
            // Find the announcement by ID and update its status
            var announcement = UploadedAnnouncements.FirstOrDefault(a => a.Id == announcementId);
            if (announcement != null)
            {
                announcement.CompanyAnnouncementStatus = newStatus;
                // Update the status in the database
                await dbContext.SaveChangesAsync();
            }

            // Refresh UI
            StateHasChanged();
        }
    }



    private async Task ChangeCompanyEventStatus(int companyeventId, string newStatus)
    {
        // Show custom confirmation dialog with formatted text
        var isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML",
            $"Πρόκειται να αλλάξετε την κατάσταση αυτής της Εκδήλωσης σε " +
            $"<strong style='color: {(newStatus == "Δημοσιευμένη" ? "green" : "red")};'>{newStatus}</strong>.<br><br>" +
            "Είστε σίγουρος/η;");

        // Proceed only if confirmed
        if (isConfirmed)
        {
            // Find the event by ID and update its status
            var companyevent = UploadedEventsAsCompany.FirstOrDefault(a => a.Id == companyeventId);
            if (companyevent != null)
            {
                companyevent.CompanyEventStatus = newStatus;
                // Update the status in the database
                await dbContext.SaveChangesAsync();

                // Optionally, refresh FilteredCompanyEvents if necessary
                FilteredCompanyEvents = UploadedEventsAsCompany.ToList();
            }

            StateHasChanged();
        }
    }


    private async Task ChangeProfessorEventStatus(int professoreventId, string newStatus)
    {
        // Show confirmation dialog
        var isConfirmed = await JS.InvokeAsync<bool>("confirm", new object[] 
        { 
            $"Πρόκειται να αλλάξετε την κατάσταση αυτής της Εκδήλωσης σε '{newStatus}'. Είστε σίγουρος/η;" 
        });

        // Proceed only if confirmed
        if (isConfirmed)
        {
            // Find the announcement by ID and update its status
            var professorevent = UploadedEventsAsProfessor.FirstOrDefault(a => a.Id == professoreventId);
            if (professorevent != null)
            {
                professorevent.ProfessorEventStatus = newStatus;
                // Update the status in the database as well
                await dbContext.SaveChangesAsync();

                // Optionally, refresh FilteredCompanyEvents if necessary
                FilteredProfessorEvents = UploadedEventsAsProfessor.ToList();
            }

            StateHasChanged();
        }
    }


    private async Task ChangeAnnouncementStatusAsProfessor(int professorannouncementId, string professorannouncementnewStatus)
    {
        // Find the announcement by ID and update its status
        var professorannouncement = UploadedAnnouncementsAsProfessor.FirstOrDefault(a => a.Id == professorannouncementId);
        if (announcement != null)
        {
            professorannouncement.ProfessorAnnouncementStatus = professorannouncementnewStatus;
            // Update the status in the database as well
            await dbContext.SaveChangesAsync();
        }
        StateHasChanged();
    }

    private async Task ChangeThesisStatusAsProfessor(int professorthesisId, string professorthesisnewStatus)
    {
        // Find the announcement by ID and update its status
        var professorthesis = UploadedThesesAsProfessor.FirstOrDefault(a => a.Id == professorthesisId);
        if (thesis != null)
        {
            professorthesis.ThesisStatus = professorthesisnewStatus;
            await dbContext.SaveChangesAsync();
        }
        StateHasChanged();
    }

    private void OpenEditModal(AnnouncementAsCompany announcement)
    {
        currentAnnouncement = new AnnouncementAsCompany
        {
            Id = announcement.Id,
            CompanyAnnouncementTitle = announcement.CompanyAnnouncementTitle,
            CompanyAnnouncementDescription = announcement.CompanyAnnouncementDescription,
            CompanyAnnouncementUploadDate = announcement.CompanyAnnouncementUploadDate,
            CompanyAnnouncementCompanyEmail = announcement.CompanyAnnouncementCompanyEmail,
            CompanyAnnouncementTimeToBeActive = announcement.CompanyAnnouncementTimeToBeActive,
            CompanyAnnouncementAttachmentFile = announcement.CompanyAnnouncementAttachmentFile
        };
        isEditModalVisible = true;
    }

    private void OpenEditModalForCompanyEvent(CompanyEvent companyEvent)
    {
        currentCompanyEvent = new CompanyEvent
        {
            Id = companyEvent.Id,
            RNGForEventUploadedAsCompany = companyEvent.RNGForEventUploadedAsCompany,
            CompanyEventType = companyEvent.CompanyEventType,
            CompanyEventOtherOrganizerToBeVisible = companyEvent.CompanyEventOtherOrganizerToBeVisible,
            CompanyEventOtherOrganizer = companyEvent.CompanyEventOtherOrganizer,
            CompanyEventAreasOfInterest = companyEvent.CompanyEventAreasOfInterest,
            CompanyEventTitle = companyEvent.CompanyEventTitle,
            CompanyEventDescription = companyEvent.CompanyEventDescription,
            CompanyEventStatus = companyEvent.CompanyEventStatus,
            CompanyEventResponsiblePerson = companyEvent.CompanyEventResponsiblePerson,
            CompanyEventResponsiblePersonEmail = companyEvent.CompanyEventResponsiblePersonEmail,
            CompanyEventResponsiblePersonTelephone = companyEvent.CompanyEventResponsiblePersonTelephone,
            CompanyEventCompanyDepartment = companyEvent.CompanyEventCompanyDepartment,
            CompanyEventUploadedDate = companyEvent.CompanyEventUploadedDate,
            CompanyEventActiveDate = companyEvent.CompanyEventActiveDate,
            CompanyEventPerifereiaLocation = companyEvent.CompanyEventPerifereiaLocation,
            CompanyEventDimosLocation = companyEvent.CompanyEventDimosLocation,
            CompanyEventPlaceLocation = companyEvent.CompanyEventPlaceLocation,
            CompanyEventTime = companyEvent.CompanyEventTime, // TimeSpan property
            CompanyEventPostalCodeLocation = companyEvent.CompanyEventPostalCodeLocation,
            CompanyEventAttachmentFile = companyEvent.CompanyEventAttachmentFile,
            CompanyEventOfferingTransportToEventLocation = companyEvent.CompanyEventOfferingTransportToEventLocation,
            CompanyEventStartingPointLocationToTransportPeopleToEvent1 = companyEvent.CompanyEventStartingPointLocationToTransportPeopleToEvent1,
            CompanyEventStartingPointLocationToTransportPeopleToEvent2 = companyEvent.CompanyEventStartingPointLocationToTransportPeopleToEvent2,
            CompanyEventStartingPointLocationToTransportPeopleToEvent3 = companyEvent.CompanyEventStartingPointLocationToTransportPeopleToEvent3,
            RNGForEventUploadedAsCompany_HashedAsUniqueID = companyEvent.RNGForEventUploadedAsCompany_HashedAsUniqueID,
            CompanyEmailUsedToUploadEvent = companyEvent.CompanyEmailUsedToUploadEvent
        };

        // Initialize SelectedAreasToEditForCompanyEvent
        SelectedAreasToEditForCompanyEvent = string.IsNullOrEmpty(companyEvent.CompanyEventAreasOfInterest) 
            ? new List<Area>() 
            : companyEvent.CompanyEventAreasOfInterest
                .Split(',')
                .Select(areaName => Areas.FirstOrDefault(area => area.AreaName.Trim() == areaName.Trim())) 
                .Where(area => area != null)
                .ToList();

        // Ensure towns are loaded when opening the modal
        AvailableTownsForEditCompanyEvent = GetTownsForRegionForEditCompanyEvent(companyEvent.CompanyEventPerifereiaLocation);

        isEditModalVisibleForEventsAsCompany = true;
    }


    private List<string> GetTownsForRegionForEditProfessorEvent(string region)
    {
        return RegionToTownsMap.ContainsKey(region) 
            ? RegionToTownsMap[region] 
            : new List<string>();
    }


    private List<Area> SelectedAreasToEditForProfessorEvent = new();
    private List<string> AvailableTownsForEditProfessorEvent = new();
   private void OpenEditModalForProfessorEvent(ProfessorEvent professorevent)
    {
        // Make sure to include Professor when loading the event
        var eventWithProfessor = dbContext.ProfessorEvents
            .Include(pe => pe.Professor)
            .FirstOrDefault(pe => pe.Id == professorevent.Id);

        if (eventWithProfessor == null) return;

        currentProfessorEvent = new ProfessorEvent
        {
            Id = eventWithProfessor.Id,
            ProfessorEventTitle = eventWithProfessor.ProfessorEventTitle,
            ProfessorEventDescription = eventWithProfessor.ProfessorEventDescription,
            ProfessorEventType = eventWithProfessor.ProfessorEventType,
            ProfessorEventOtherOrganizerToBeVisible = eventWithProfessor.ProfessorEventOtherOrganizerToBeVisible,
            ProfessorEventOtherOrganizer = eventWithProfessor.ProfessorEventOtherOrganizer,
            ProfessorEventAreasOfInterest = eventWithProfessor.ProfessorEventAreasOfInterest,
            ProfessorEventStatus = eventWithProfessor.ProfessorEventStatus,
            ProfessorEventResponsiblePerson = eventWithProfessor.ProfessorEventResponsiblePerson,
            ProfessorEventResponsiblePersonEmail = eventWithProfessor.ProfessorEventResponsiblePersonEmail,
            ProfessorEventResponsiblePersonTelephone = eventWithProfessor.ProfessorEventResponsiblePersonTelephone,
            // University and department now come from Professor navigation property
            Professor = new Professor 
            {
                ProfUniversity = eventWithProfessor.Professor?.ProfUniversity,
                ProfDepartment = eventWithProfessor.Professor?.ProfDepartment,
                ProfImage = eventWithProfessor.Professor?.ProfImage
            },
            ProfessorEventPerifereiaLocation = eventWithProfessor.ProfessorEventPerifereiaLocation,
            ProfessorEventDimosLocation = eventWithProfessor.ProfessorEventDimosLocation,
            ProfessorEventPlaceLocation = eventWithProfessor.ProfessorEventPlaceLocation,
            ProfessorEventActiveDate = eventWithProfessor.ProfessorEventActiveDate,
            ProfessorEventTime = eventWithProfessor.ProfessorEventTime,
            ProfessorEventPostalCodeLocation = eventWithProfessor.ProfessorEventPostalCodeLocation,
            ProfessorEventAttachmentFile = eventWithProfessor.ProfessorEventAttachmentFile,
            ProfessorEventOfferingTransportToEventLocation = eventWithProfessor.ProfessorEventOfferingTransportToEventLocation,
            ProfessorEventStartingPointLocationToTransportPeopleToEvent1 = eventWithProfessor.ProfessorEventStartingPointLocationToTransportPeopleToEvent1,
            ProfessorEventStartingPointLocationToTransportPeopleToEvent2 = eventWithProfessor.ProfessorEventStartingPointLocationToTransportPeopleToEvent2,
            ProfessorEventStartingPointLocationToTransportPeopleToEvent3 = eventWithProfessor.ProfessorEventStartingPointLocationToTransportPeopleToEvent3,
            ProfessorEmailUsedToUploadEvent = eventWithProfessor.ProfessorEmailUsedToUploadEvent
        };

        // Initialize SelectedAreasToEditForProfessorEvent
        SelectedAreasToEditForProfessorEvent = string.IsNullOrEmpty(eventWithProfessor.ProfessorEventAreasOfInterest) 
            ? new List<Area>() 
            : eventWithProfessor.ProfessorEventAreasOfInterest
                .Split(',')
                .Select(areaName => Areas.FirstOrDefault(area => area.AreaName.Trim() == areaName.Trim()))
                .Where(area => area != null)
                .ToList();

        // Ensure towns are loaded when opening the modal
        AvailableTownsForEditProfessorEvent = GetTownsForRegionForEditProfessorEvent(eventWithProfessor.ProfessorEventPerifereiaLocation);

        isEditModalVisibleForEventsAsProfessor = true;
    }

    private void OpenEditModalAsProfessor(AnnouncementAsProfessor professorAnnouncement)
    {
        currentAnnouncementAsProfessor = new AnnouncementAsProfessor
        {
            Id = professorAnnouncement.Id,
            ProfessorAnnouncementRNG = professorAnnouncement.ProfessorAnnouncementRNG,
            ProfessorAnnouncementRNG_HashedAsUniqueID = professorAnnouncement.ProfessorAnnouncementRNG_HashedAsUniqueID,
            ProfessorAnnouncementTitle = professorAnnouncement.ProfessorAnnouncementTitle,
            ProfessorAnnouncementDescription = professorAnnouncement.ProfessorAnnouncementDescription,
            ProfessorAnnouncementStatus = professorAnnouncement.ProfessorAnnouncementStatus,
            ProfessorAnnouncementUploadDate = professorAnnouncement.ProfessorAnnouncementUploadDate,
            ProfessorAnnouncementProfessorEmail = professorAnnouncement.ProfessorAnnouncementProfessorEmail,
            ProfessorAnnouncementTimeToBeActive = professorAnnouncement.ProfessorAnnouncementTimeToBeActive,
            ProfessorAnnouncementAttachmentFile = professorAnnouncement.ProfessorAnnouncementAttachmentFile
        };

        // Initialize navigation property with all Professor details
        if (professorAnnouncement.Professor != null)
        {
            currentAnnouncementAsProfessor.Professor = new Professor
            {
                Id = professorAnnouncement.Professor.Id,
                ProfEmail = professorAnnouncement.Professor.ProfEmail,
                Professor_UniqueID = professorAnnouncement.Professor.Professor_UniqueID,
                ProfImage = professorAnnouncement.Professor.ProfImage,
                ProfName = professorAnnouncement.Professor.ProfName,
                ProfSurname = professorAnnouncement.Professor.ProfSurname,
                ProfUniversity = professorAnnouncement.Professor.ProfUniversity,
                ProfDepartment = professorAnnouncement.Professor.ProfDepartment,
                ProfVahmidaDEP = professorAnnouncement.Professor.ProfVahmidaDEP,
                ProfWorkTelephone = professorAnnouncement.Professor.ProfWorkTelephone,
                ProfPersonalTelephone = professorAnnouncement.Professor.ProfPersonalTelephone,
                ProfPersonalTelephoneVisibility = professorAnnouncement.Professor.ProfPersonalTelephoneVisibility,
                ProfPersonalWebsite = professorAnnouncement.Professor.ProfPersonalWebsite,
                ProfLinkedInSite = professorAnnouncement.Professor.ProfLinkedInSite,
                ProfScholarProfile = professorAnnouncement.Professor.ProfScholarProfile,
                ProfOrchidProfile = professorAnnouncement.Professor.ProfOrchidProfile,
                ProfGeneralFieldOfWork = professorAnnouncement.Professor.ProfGeneralFieldOfWork,
                ProfGeneralSkills = professorAnnouncement.Professor.ProfGeneralSkills,
                ProfPersonalDescription = professorAnnouncement.Professor.ProfPersonalDescription,
                ProfCVAttachment = professorAnnouncement.Professor.ProfCVAttachment,
                ProfRegistryNumber = professorAnnouncement.Professor.ProfRegistryNumber,
                ProfCourses = professorAnnouncement.Professor.ProfCourses
            };
        }

        isEditModalVisibleForAnnouncementsAsProfessor = true;
    }


    private void OpenEditModalForThesisAsProfessor(ProfessorThesis professorthesis)
    {
        try
        {
            // Create new instance with navigation properties
            currentThesisAsProfessor = new ProfessorThesis
            {
                Id = professorthesis.Id,
                ThesisTitle = professorthesis.ThesisTitle,
                ThesisDescription = professorthesis.ThesisDescription,
                ThesisType = professorthesis.ThesisType,
                ThesisStatus = professorthesis.ThesisStatus,
                ThesisActivePeriod = professorthesis.ThesisActivePeriod,
                ThesisAreas = professorthesis.ThesisAreas,
                ThesisSkills = professorthesis.ThesisSkills,
                ThesisAttachment = professorthesis.ThesisAttachment,
                ProfessorEmailUsedToUploadThesis = professorthesis.ProfessorEmailUsedToUploadThesis,
                ThesisUploadDateTime = professorthesis.ThesisUploadDateTime,
                ThesisUpdateDateTime = professorthesis.ThesisUpdateDateTime,
                ThesisTimesUpdated = professorthesis.ThesisTimesUpdated,
                // Include professor details if needed
                Professor = professorthesis.Professor != null ? new Professor
                {
                    ProfName = professorthesis.Professor.ProfName,
                    ProfSurname = professorthesis.Professor.ProfSurname,
                    ProfDepartment = professorthesis.Professor.ProfDepartment
                } : null
            };

            // Initialize selected areas
            SelectedAreasToEditForProfessorThesis = new List<Area>();
            if (!string.IsNullOrEmpty(professorthesis.ThesisAreas))
            {
                var currentAreas = professorthesis.ThesisAreas.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => a.Trim())
                    .Where(a => !string.IsNullOrEmpty(a));
            
                SelectedAreasToEditForProfessorThesis = Areas
                    .Where(area => currentAreas.Contains(area.AreaName))
                    .ToList();
            }

            // Initialize selected skills with improved null handling
            SelectedSkillsToEditForProfessorThesis = new List<Skill>();
            if (!string.IsNullOrEmpty(professorthesis.ThesisSkills))
            {
                var currentSkills = professorthesis.ThesisSkills.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .Where(s => !string.IsNullOrEmpty(s));
                
                SelectedSkillsToEditForProfessorThesis = Skills
                    .Where(skill => currentSkills.Contains(skill.SkillName))
                    .ToList();
            }

            isEditModalVisibleForThesesAsProfessor = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening edit modal: {ex.Message}");
            // Consider adding user notification
            SafeInvokeJsAsync(() => JS.InvokeVoidAsync("showErrorToast", 
                "Σφάλμα ανοίγματος επεξεργασίας").AsTask());
        }
        finally
        {
            StateHasChanged();
        }
    }

    private void CloseEditModal()
    {
        isEditModalVisible = false;
    }

    private void CloseEditModalForCompanyEvent()
    {
        isEditModalVisibleForEventsAsCompany = false;
    }

    private void CloseEditModalForProfessorEvent()
    {
        isEditModalVisibleForEventsAsProfessor = false;
    }

    private void CloseEditModalForAnnouncementsAsProfessor()
    {
        isEditModalVisibleForAnnouncementsAsProfessor = false;
    }

    private void CloseEditModalForThesesAsProfessor()
    {
        isEditModalVisibleForThesesAsProfessor = false;
    }

    private async Task UpdateAnnouncement(AnnouncementAsCompany updatedAnnouncement)
    {
        var existingAnnouncement = await dbContext.AnnouncementsAsCompany.FindAsync(updatedAnnouncement.Id);

        if (existingAnnouncement != null)
        {
            existingAnnouncement.CompanyAnnouncementTitle = updatedAnnouncement.CompanyAnnouncementTitle;
            existingAnnouncement.CompanyAnnouncementDescription = updatedAnnouncement.CompanyAnnouncementDescription;
            existingAnnouncement.CompanyAnnouncementAttachmentFile = updatedAnnouncement.CompanyAnnouncementAttachmentFile; // Update file attachment
            await dbContext.SaveChangesAsync();
            CloseEditModal();
        }
    }


    private async Task UpdateCompanyEvent(CompanyEvent updatedCompanyEvent)
    {
        var existingCompanyEvent = await dbContext.CompanyEvents.FindAsync(updatedCompanyEvent.Id);

        if (existingCompanyEvent != null)
        {
            existingCompanyEvent.CompanyEventTitle = updatedCompanyEvent.CompanyEventTitle;
            existingCompanyEvent.CompanyEventDescription = updatedCompanyEvent.CompanyEventDescription;
            existingCompanyEvent.CompanyEventType = updatedCompanyEvent.CompanyEventType;
            existingCompanyEvent.CompanyEventResponsiblePerson = updatedCompanyEvent.CompanyEventResponsiblePerson;
            existingCompanyEvent.CompanyEventResponsiblePersonEmail = updatedCompanyEvent.CompanyEventResponsiblePersonEmail;
            existingCompanyEvent.CompanyEventResponsiblePersonTelephone = updatedCompanyEvent.CompanyEventResponsiblePersonTelephone;
            existingCompanyEvent.CompanyEventCompanyDepartment = updatedCompanyEvent.CompanyEventCompanyDepartment;
            existingCompanyEvent.CompanyEventPerifereiaLocation = updatedCompanyEvent.CompanyEventPerifereiaLocation;
            existingCompanyEvent.CompanyEventDimosLocation = updatedCompanyEvent.CompanyEventDimosLocation;
            existingCompanyEvent.CompanyEventPlaceLocation = updatedCompanyEvent.CompanyEventPlaceLocation;
            existingCompanyEvent.CompanyEventPostalCodeLocation = updatedCompanyEvent.CompanyEventPostalCodeLocation;
            existingCompanyEvent.CompanyEventActiveDate = updatedCompanyEvent.CompanyEventActiveDate;
            existingCompanyEvent.CompanyEventTime = updatedCompanyEvent.CompanyEventTime;

            // Ensure areas are saved as a comma-separated string (or any other format you prefer)
            existingCompanyEvent.CompanyEventAreasOfInterest = string.Join(",", SelectedAreasToEditForCompanyEvent.Select(area => area.AreaName));

            existingCompanyEvent.CompanyEventOfferingTransportToEventLocation = updatedCompanyEvent.CompanyEventOfferingTransportToEventLocation;
            existingCompanyEvent.CompanyEventStartingPointLocationToTransportPeopleToEvent1 = updatedCompanyEvent.CompanyEventStartingPointLocationToTransportPeopleToEvent1;
            existingCompanyEvent.CompanyEventStartingPointLocationToTransportPeopleToEvent2 = updatedCompanyEvent.CompanyEventStartingPointLocationToTransportPeopleToEvent2;
            existingCompanyEvent.CompanyEventStartingPointLocationToTransportPeopleToEvent3 = updatedCompanyEvent.CompanyEventStartingPointLocationToTransportPeopleToEvent3;

            // Save the changes to the database
            await dbContext.SaveChangesAsync();
            CloseEditModalForCompanyEvent();
        }
    }



    private async Task UpdateProfessorEvent(ProfessorEvent updatedProfessorEvent)
    {
        var existingProfessorEvent = await dbContext.ProfessorEvents.FindAsync(updatedProfessorEvent.Id);

        if (existingProfessorEvent != null)
        {
            existingProfessorEvent.ProfessorEventTitle = updatedProfessorEvent.ProfessorEventTitle;
            existingProfessorEvent.ProfessorEventDescription = updatedProfessorEvent.ProfessorEventDescription;
            await dbContext.SaveChangesAsync();
            CloseEditModalForProfessorEvent();
        }
    }

    private async Task UpdateAnnouncementAsProfessor(AnnouncementAsProfessor updatedAnnouncementasProfessor)
    {
        var existingAnnouncementasProfessor = await dbContext.AnnouncementsAsProfessor.FindAsync(updatedAnnouncementasProfessor.Id);

        if (existingAnnouncementasProfessor != null)
        {
            existingAnnouncementasProfessor.ProfessorAnnouncementTitle = updatedAnnouncementasProfessor.ProfessorAnnouncementTitle;
            existingAnnouncementasProfessor.ProfessorAnnouncementDescription = updatedAnnouncementasProfessor.ProfessorAnnouncementDescription;
            existingAnnouncementasProfessor.ProfessorAnnouncementAttachmentFile = updatedAnnouncementasProfessor.ProfessorAnnouncementAttachmentFile;
            await dbContext.SaveChangesAsync();
            CloseEditModalForAnnouncementsAsProfessor();
        }
    }

    private async Task UpdateThesisAsProfessor(ProfessorThesis updatedThesisProfessor)
    {
        try
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(currentThesisAsProfessor.ThesisTitle) || 
                string.IsNullOrWhiteSpace(currentThesisAsProfessor.ThesisDescription))
            {
                showSuccessMessage = false;
                showErrorMessage = true;
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("showErrorToast", 
                    "Παρακαλώ συμπληρώστε όλα τα απαραίτητα πεδία").AsTask());
                return;
            }

            // Initialize selected areas if null/empty
            SelectedAreasToEditForProfessorThesis ??= new List<Area>();
            if (!SelectedAreasToEditForProfessorThesis.Any())
            {
                var currentAreas = currentThesisAsProfessor.ThesisAreas?
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => a.Trim())
                    .ToList() ?? new List<string>();
            
                SelectedAreasToEditForProfessorThesis = Areas
                    .Where(area => currentAreas.Contains(area.AreaName))
                    .ToList();
            }

            // Initialize selected skills if null/empty
            SelectedSkillsToEditForProfessorThesis ??= new List<Skill>();
            if (!SelectedSkillsToEditForProfessorThesis.Any())
            {
                var currentSkills = currentThesisAsProfessor.ThesisSkills?
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList() ?? new List<string>();
            
                SelectedSkillsToEditForProfessorThesis = Skills
                    .Where(skill => currentSkills.Contains(skill.SkillName))
                    .ToList();
            }

            // Update areas and skills
            currentThesisAsProfessor.ThesisAreas = string.Join(",", 
                SelectedAreasToEditForProfessorThesis.Select(a => a.AreaName));
            currentThesisAsProfessor.ThesisSkills = string.Join(",", 
                SelectedSkillsToEditForProfessorThesis.Select(s => s.SkillName));

            // Find and update thesis
            var thesisToUpdate = await dbContext.ProfessorTheses
                .Include(t => t.Professor) // Include professor if needed
                .FirstOrDefaultAsync(t => t.Id == currentThesisAsProfessor.Id);

            if (thesisToUpdate == null)
            {
                showSuccessMessage = false;
                showErrorMessage = true;
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("showErrorToast", 
                    "Δεν βρέθηκε η πτυχιακή εργασία").AsTask());
                return;
            }

            // Update thesis properties
            thesisToUpdate.ThesisTitle = currentThesisAsProfessor.ThesisTitle;
            thesisToUpdate.ThesisDescription = currentThesisAsProfessor.ThesisDescription;
            thesisToUpdate.ThesisType = currentThesisAsProfessor.ThesisType;
            thesisToUpdate.ThesisStatus = currentThesisAsProfessor.ThesisStatus;
            thesisToUpdate.ThesisActivePeriod = currentThesisAsProfessor.ThesisActivePeriod;
            thesisToUpdate.ThesisAreas = currentThesisAsProfessor.ThesisAreas;
            thesisToUpdate.ThesisSkills = currentThesisAsProfessor.ThesisSkills;

            // Update attachment if new file was uploaded
            if (currentThesisAsProfessor.ThesisAttachment != null && 
                currentThesisAsProfessor.ThesisAttachment.Length > 0)
            {
                thesisToUpdate.ThesisAttachment = currentThesisAsProfessor.ThesisAttachment;
            }

            // Update metadata
            thesisToUpdate.ThesisUpdateDateTime = DateTime.Now;
            thesisToUpdate.ThesisTimesUpdated++;

            // Save changes
            await dbContext.SaveChangesAsync();
        
            showSuccessMessage = true;
            showErrorMessage = false;
            await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("showSuccessToast", 
                "Επιτυχής ενημέρωση πτυχιακής εργασίας").AsTask());
        }
        catch (Exception ex)
        {
            showSuccessMessage = false;
            showErrorMessage = true;
            Console.Error.WriteLine($"Error saving professor thesis: {ex.Message}");
            await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("showErrorToast", 
                $"Σφάλμα ενημέρωσης: {ex.Message}").AsTask());
        }
        finally
        {
            isEditModalVisibleForThesesAsProfessor = false;
            await LoadUploadedThesesAsProfessorAsync(); // Refresh the list
            StateHasChanged();
        }
    }

    private void FilterAnnouncements()
    {
        // Filter the announcements based on the selected filter
        if (selectedStatusFilterForAnnouncements == "Όλα")
        {
            FilteredAnnouncements = UploadedAnnouncements;
        }
        else if (selectedStatusFilterForAnnouncements == "Δημοσιευμένη")
        {
            FilteredAnnouncements = UploadedAnnouncements
                .Where(a => a.CompanyAnnouncementStatus == "Δημοσιευμένη").ToList();
        }
        else if (selectedStatusFilterForAnnouncements == "Μη Δημοσιευμένη")
        {
            FilteredAnnouncements = UploadedAnnouncements
                .Where(a => a.CompanyAnnouncementStatus == "Μη Δημοσιευμένη").ToList();
        }

        // Update counts
        totalCountAnnouncements = UploadedAnnouncements.Count;
        publishedCountAnnouncements = UploadedAnnouncements
            .Count(a => a.CompanyAnnouncementStatus == "Δημοσιευμένη");
        unpublishedCountAnnouncements = UploadedAnnouncements
            .Count(a => a.CompanyAnnouncementStatus == "Μη Δημοσιευμένη");

        // Refresh UI
        StateHasChanged();
    }

    private void FilterAnnouncementsAsProfessor()
    {
        // Filter the announcements based on the selected filter
        if (selectedStatusFilterForAnnouncementsAsProfessor == "Όλα")
        {
            FilteredAnnouncementsAsProfessor = UploadedAnnouncementsAsProfessor;
        }
        else if (selectedStatusFilterForAnnouncementsAsProfessor == "Δημοσιευμένη")
        {
            FilteredAnnouncementsAsProfessor = UploadedAnnouncementsAsProfessor
                .Where(a => a.ProfessorAnnouncementStatus == "Δημοσιευμένη").ToList();
        }
        else if (selectedStatusFilterForAnnouncementsAsProfessor == "Μη Δημοσιευμένη")
        {
            FilteredAnnouncementsAsProfessor = UploadedAnnouncementsAsProfessor
                .Where(a => a.ProfessorAnnouncementStatus == "Μη Δημοσιευμένη").ToList();
        }

        // Update counts
        totalCountAnnouncementsAsProfessor = UploadedAnnouncementsAsProfessor.Count;
        publishedCountAnnouncementsAsProfessor = UploadedAnnouncementsAsProfessor
            .Count(a => a.ProfessorAnnouncementStatus == "Δημοσιευμένη");
        unpublishedCountAnnouncementsAsProfessor = UploadedAnnouncementsAsProfessor
            .Count(a => a.ProfessorAnnouncementStatus == "Μη Δημοσιευμένη");

        // Refresh UI
        StateHasChanged();
    }

    private void FilterThesesAsProfessor()
    {
        // Filter the announcements based on the selected filter
        if (selectedStatusFilterForThesesAsProfessor == "Όλα")
        {
            FilteredThesesAsProfessor = UploadedThesesAsProfessor;
        }
        else
        {
            FilteredThesesAsProfessor = UploadedThesesAsProfessor
                .Where(a => a.ThesisStatus == selectedStatusFilterForThesesAsProfessor)
                .ToList();
        }
        Console.WriteLine($"Filtered Theses Count: {FilteredThesesAsProfessor.Count}");

        // Update counts
        totalCountThesesAsProfessor = UploadedThesesAsProfessor.Count;
        publishedCountThesesAsProfessor = UploadedThesesAsProfessor
            .Count(a => a.ThesisStatus == "Δημοσιευμένη");
        unpublishedCountThesesAsProfessor = UploadedThesesAsProfessor
            .Count(a => a.ThesisStatus == "Μη Δημοσιευμένη");
        withdrawnCountThesesAsProfessor = UploadedThesesAsProfessor
            .Count(a => a.ThesisStatus == "Αποσυρμένη");

        Console.WriteLine($"Total Count: {totalCountThesesAsProfessor}");
        Console.WriteLine($"Published Count: {publishedCountThesesAsProfessor}");
        Console.WriteLine($"Unpublished Count: {unpublishedCountThesesAsProfessor}");
        Console.WriteLine($"Withdrawn Count: {withdrawnCountThesesAsProfessor}");

        // Refresh UI
        StateHasChanged();
    }


    // private void FilterThesesAsCompany()
    // {
    //     // Apply filter based on selected status
    //     if (selectedStatusFilterForCompanyTheses == "Όλα")
    //     {
    //         FilteredCompanyTheses = UploadedCompanyTheses;
    //     }
    //     else
    //     {
    //         FilteredCompanyTheses = UploadedCompanyTheses
    //             .Where(a => a.CompanyThesisStatus == selectedStatusFilterForCompanyTheses)
    //             .ToList();
    //     }

    //     // Debugging: Check if filtering works
    //     Console.WriteLine($"Filtered Theses Count: {FilteredCompanyTheses.Count}");

    //     // Recalculate counts
    //     totalCountForCompanyTheses = UploadedCompanyTheses.Count; // Total count of filtered theses
    //     publishedCountForCompanyTheses = UploadedCompanyTheses
    //         .Count(a => a.CompanyThesisStatus == "Δημοσιευμένη"); // Count from original list
    //     unpublishedCountForCompanyTheses = UploadedCompanyTheses
    //         .Count(a => a.CompanyThesisStatus == "Μη Δημοσιευμένη"); // Count from original list
    //     withdrawnCountForCompanyTheses = UploadedCompanyTheses
    //         .Count(a => a.CompanyThesisStatus == "Αποσυρμένη"); // Count from original list

    //     // Log the calculated counts to ensure they are correct
    //     Console.WriteLine($"Total Count: {totalCountForCompanyTheses}");
    //     Console.WriteLine($"Published Count: {publishedCountForCompanyTheses}");
    //     Console.WriteLine($"Unpublished Count: {unpublishedCountForCompanyTheses}");
    //     Console.WriteLine($"Withdrawn Count: {withdrawnCountForCompanyTheses}");

    //     // Refresh the UI
    //     StateHasChanged();
    // }

    private void FilterCompanyEvents()
    {
        // Filter the announcements based on the selected filter
        if (selectedStatusFilterForEventsAsCompany == "Όλα")
        {
            FilteredCompanyEvents = UploadedEventsAsCompany;
        }
        else if (selectedStatusFilterForEventsAsCompany == "Δημοσιευμένη")
        {
            FilteredCompanyEvents = UploadedEventsAsCompany
                .Where(a => a.CompanyEventStatus == "Δημοσιευμένη").ToList();
        }
        else if (selectedStatusFilterForEventsAsCompany == "Μη Δημοσιευμένη")
        {
            FilteredCompanyEvents = UploadedEventsAsCompany
                .Where(a => a.CompanyEventStatus == "Μη Δημοσιευμένη").ToList();
        }

        // Update counts
        totalCountEventsAsCompany = UploadedEventsAsCompany.Count;
        publishedCountEventsAsCompany = UploadedEventsAsCompany
            .Count(a => a.CompanyEventStatus == "Δημοσιευμένη");
        unpublishedCountEventsAsCompany = UploadedEventsAsCompany
            .Count(a => a.CompanyEventStatus == "Μη Δημοσιευμένη");

        // Refresh UI
        StateHasChanged();
    }


    private void FilterProfessorEvents()
    {
        // Filter the announcements based on the selected filter
        if (selectedStatusFilterForEventsAsProfessor == "Όλα")
        {
            FilteredProfessorEvents = UploadedEventsAsProfessor;
        }
        else if (selectedStatusFilterForEventsAsProfessor == "Δημοσιευμένη")
        {
            FilteredProfessorEvents = UploadedEventsAsProfessor
                .Where(a => a.ProfessorEventStatus == "Δημοσιευμένη").ToList();
        }
        else if (selectedStatusFilterForEventsAsProfessor == "Μη Δημοσιευμένη")
        {
            FilteredProfessorEvents = UploadedEventsAsProfessor
                .Where(a => a.ProfessorEventStatus == "Μη Δημοσιευμένη").ToList();
        }

        // Update counts
        totalCountEventsAsProfessor = UploadedEventsAsProfessor.Count;
        publishedCountEventsAsProfessor = UploadedEventsAsProfessor
            .Count(a => a.ProfessorEventStatus == "Δημοσιευμένη");
        unpublishedCountEventsAsProfessor = UploadedEventsAsProfessor
            .Count(a => a.ProfessorEventStatus == "Μη Δημοσιευμένη");

        // Refresh UI
        StateHasChanged();
    }



    private void HandleStatusFilterChange(ChangeEventArgs e)
    {
        selectedStatusFilterForAnnouncements = e.Value.ToString();
        FilterAnnouncements(); // Call your filtering logic
    }

    private void HandleStatusFilterChangeForCompanyEvents(ChangeEventArgs e)
    {
        selectedStatusFilterForEventsAsCompany = e.Value.ToString();
        FilterCompanyEvents(); // Call your filtering logic
    }

    private void HandleStatusFilterChangeForProfessorEvents(ChangeEventArgs e)
    {
        selectedStatusFilterForEventsAsProfessor = e.Value.ToString();
        FilterProfessorEvents(); // Call your filtering logic
    }

    private void HandleStatusFilterChangeForAnnouncementsAsProfessor(ChangeEventArgs e)
    {
        selectedStatusFilterForAnnouncementsAsProfessor = e.Value.ToString();
        FilterAnnouncementsAsProfessor(); // Call your filtering logic
    }

    private void HandleStatusFilterChangeForThesesAsProfessor(ChangeEventArgs e)
    {
        selectedStatusFilterForThesesAsProfessor = e.Value.ToString();
        FilterThesesAsProfessor(); // Call your filtering logic
    }

    private void HandleStatusFilterChangeForThesesAsCompany(ChangeEventArgs e)
    {
        selectedStatusFilterForCompanyTheses = e.Value.ToString();
        //FilterThesesAsCompany(); // Trigger filtering and recalculation of counts
    }



private async Task SaveAnnouncementAsPublishedAsProfessor()
{
    // Show custom confirmation dialog with HTML styling
    bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
        $"Πρόκεται να Δημιουργήσετε μια Ανακοίνωση με Τίτλο: <strong>{professorannouncement.ProfessorAnnouncementTitle}</strong> ως '<strong>Δημοσιευμένη</strong>'.<br><br>" +
        "<strong style='color: red;'>Είστε σίγουρος/η;</strong>"
    });

    if (!isConfirmed)
        return;

    professorannouncement.ProfessorAnnouncementStatus = "Δημοσιευμένη";
    professorannouncement.ProfessorAnnouncementUploadDate = DateTime.Now;
    professorannouncement.ProfessorAnnouncementProfessorEmail = CurrentUserEmail;
    professorannouncement.ProfessorAnnouncementRNG = new Random().NextInt64();
    professorannouncement.ProfessorAnnouncementRNG_HashedAsUniqueID = HashingHelper.HashLong(professorannouncement.ProfessorAnnouncementRNG ?? 0);
    
    // Initialize Professor navigation property if needed
    professorannouncement.Professor = new Professor
    {
        ProfEmail = CurrentUserEmail,
        ProfName = professorName,
        ProfSurname = professorSurname
    };
    
    await SaveAnnouncementToDatabaseAsProfessor();
}

    private async Task SaveAnnouncementAsUnpublishedAsProfessor()
    {
        // Show custom confirmation dialog with HTML styling
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
            $"Πρόκεται να Δημιουργήσετε μια Ανακοίνωση με Τίτλο: <strong>{professorannouncement.ProfessorAnnouncementTitle}</strong> ως '<strong>Μη Δημοσιευμένη (Προσωρινή Αποθήκευση)</strong>'.<br><br>" +
            "<strong style='color: red;'>Είστε σίγουρος/η;</strong>"
        });

        if (!isConfirmed)
            return;

        professorannouncement.ProfessorAnnouncementStatus = "Μη Δημοσιευμένη";
        professorannouncement.ProfessorAnnouncementUploadDate = DateTime.Now;
        professorannouncement.ProfessorAnnouncementProfessorEmail = CurrentUserEmail;
        professorannouncement.ProfessorAnnouncementRNG = new Random().NextInt64();
        professorannouncement.ProfessorAnnouncementRNG_HashedAsUniqueID = HashingHelper.HashLong(professorannouncement.ProfessorAnnouncementRNG ?? 0);
    
        // Initialize Professor navigation property if needed
        professorannouncement.Professor = new Professor
        {
            ProfEmail = CurrentUserEmail,
            ProfName = professorName,
            ProfSurname = professorSurname
        };
    
        await SaveAnnouncementToDatabaseAsProfessor();
    }


    private async Task SaveThesisAsPublishedAsProfessor()
    {
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
            $"Πρόκεται να Δημιουργήσετε μια Πτυχιακή Εργασία με Τίτλο: <strong>{professorthesis.ThesisTitle}</strong> ως '<strong>Δημοσιευμένη</strong>'.<br><br>" +
            "<strong style='color: red;'>Είστε σίγουρος/η;</strong>"
        });

        if (!isConfirmed) return;

        // Initialize new thesis
        var newThesis = new ProfessorThesis
        {
            ThesisTitle = professorthesis.ThesisTitle,
            ThesisDescription = professorthesis.ThesisDescription,
            ThesisAttachment = professorthesis.ThesisAttachment,
            ThesisStatus = "Δημοσιευμένη",
            ThesisUploadDateTime = DateTime.Now,
            ThesisActivePeriod = DateTime.Now.AddYears(1),
            ProfessorEmailUsedToUploadThesis = CurrentUserEmail,
            RNGForThesisUploaded = new Random().NextInt64(),
            RNGForThesisUploaded_HashedAsUniqueID = HashingHelper.HashLong(new Random().NextInt64()),
            ThesisAreas = string.Join(",", selectedThesisAreasForProfessor.Select(a => a.AreaName)),
            ThesisSkills = string.Join(",", selectedThesisSkillsForProfessor.Select(s => s.SkillName)),
            ThesisType = ThesisType.Professor,
            IsCompanyInteresetedInProfessorThesis = false,
            IsCompanyInterestedInProfessorThesisStatus = "Δεν έχει γίνει Αποδοχή",
            ThesisUpdateDateTime = DateTime.Now
        };
    
        professorthesis = newThesis;
        await SaveThesisToDatabaseAsProfessor();
    }

    private async Task SaveThesisAsUnpublishedAsProfessor()
    {
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
            $"Πρόκεται να Δημιουργήσετε μια Πτυχιακή Εργασία με Τίτλο: <strong>{professorthesis.ThesisTitle}</strong> ως '<strong>Μη Δημοσιευμένη</strong>'.<br><br>" +
            "<strong style='color: red;'>Είστε σίγουρος/η;</strong>"
        });

        if (!isConfirmed) return;

        var newThesis = new ProfessorThesis
        {
            ThesisTitle = professorthesis.ThesisTitle,
            ThesisDescription = professorthesis.ThesisDescription,
            ThesisAttachment = professorthesis.ThesisAttachment,
            ThesisStatus = "Μη Δημοσιευμένη",
            ThesisUploadDateTime = DateTime.Now,
            ThesisActivePeriod = DateTime.Now.AddYears(1),
            ProfessorEmailUsedToUploadThesis = CurrentUserEmail,
            RNGForThesisUploaded = new Random().NextInt64(),
            RNGForThesisUploaded_HashedAsUniqueID = HashingHelper.HashLong(new Random().NextInt64()),
            ThesisAreas = string.Join(",", selectedThesisAreasForProfessor.Select(a => a.AreaName)),
            ThesisSkills = string.Join(",", selectedThesisSkillsForProfessor.Select(s => s.SkillName)),
            ThesisType = ThesisType.Professor,
            IsCompanyInteresetedInProfessorThesis = false,
            IsCompanyInterestedInProfessorThesisStatus = "Δεν έχει γίνει Αποδοχή",
            ThesisUpdateDateTime = DateTime.Now
        };
    
        professorthesis = newThesis;
        await SaveThesisToDatabaseAsProfessor();
    }



    private async Task SaveAnnouncementToDatabaseAsProfessor()
    {
        // Validation logic for professor announcement form
        if (string.IsNullOrWhiteSpace(professorannouncement.ProfessorAnnouncementTitle) ||
            string.IsNullOrWhiteSpace(professorannouncement.ProfessorAnnouncementDescription) ||
            professorannouncement.ProfessorAnnouncementTimeToBeActive.Date == DateTime.Today)
        {
            showErrorMessageforUploadingannouncementsAsProfessor = true;
            return;
        }

        try
        {
            // Get existing professor from database instead of creating new one
            var existingProfessor = await dbContext.Professors
                .FirstOrDefaultAsync(p => p.ProfEmail == CurrentUserEmail);

            if (existingProfessor != null)
            {
                // Associate existing professor with announcement
                professorannouncement.Professor = existingProfessor;
                professorannouncement.ProfessorAnnouncementProfessorEmail = existingProfessor.ProfEmail;
            }
            else
            {
                // Handle case where professor doesn't exist
                isSaveAnnouncementAsProfessorSuccessful = false;
                saveAnnouncementAsProfessorMessage = "Ο Καθηγητής δεν βρέθηκε στο σύστημα";
                return;
            }

            dbContext.AnnouncementsAsProfessor.Add(professorannouncement);
            await dbContext.SaveChangesAsync();
        
            isSaveAnnouncementAsProfessorSuccessful = true;
            saveAnnouncementAsProfessorMessage = "Η Ανακοίνωση Δημιουργήθηκε Επιτυχώς";
        }
        catch (Exception ex)
        {
            isSaveAnnouncementAsProfessorSuccessful = false;
            saveAnnouncementAsProfessorMessage = "Κάποιο πρόβλημα παρουσιάστηκε με την Δημιουργία της Ανακοίνωσης! Ανανεώστε την σελίδα και προσπαθήστε ξανά";
            Console.WriteLine($"Error saving announcement: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    
        StateHasChanged();
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }

   private async Task SaveThesisToDatabaseAsProfessor()
    {
        // Validate form data
        if (string.IsNullOrWhiteSpace(professorthesis.ThesisTitle) ||
            string.IsNullOrWhiteSpace(professorthesis.ThesisDescription))
        {
            showErrorMessageforUploadingThesisAsProfessor = true;
            return;
        }

        try
        {
            // Check if professor exists or create new
            var professor = await dbContext.Professors
                .FirstOrDefaultAsync(p => p.ProfEmail == CurrentUserEmail);
            
            if (professor == null)
            {
                professor = new Professor 
                {
                    ProfEmail = CurrentUserEmail,
                    ProfName = professorName,
                    ProfSurname = professorSurname,
                    ProfDepartment = professorDepartment
                };
                dbContext.Professors.Add(professor);
                await dbContext.SaveChangesAsync();
            }
        
            // Set required fields
            professorthesis.ThesisUpdateDateTime = DateTime.Now;
            professorthesis.ThesisActivePeriod = DateTime.Now.AddYears(1); // Example: active for 1 year
            professorthesis.Professor = professor;
        
            // Save the thesis
            dbContext.ProfessorTheses.Add(professorthesis);
            await dbContext.SaveChangesAsync();

            isSaveThesisAsProfessorSuccessful = true;
            saveThesisAsProfessorMessage = "Η Πτυχιακή Εργασία Δημιουργήθηκε Επιτυχώς";
        }
        catch (Exception ex)
        {
            isSaveThesisAsProfessorSuccessful = false;
            saveThesisAsProfessorMessage = "Κάποιο πρόβλημα παρουσιάστηκε με την Δημιουργία της Πτυχιακής Εργασίας! Ανανεώστε την σελίδα και προσπαθήστε ξανά";
            Console.WriteLine($"Πρόβλημα Δημιουργίας/Αποθήκευσης: {ex.Message}\n{ex.StackTrace}");
        }

        StateHasChanged();
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }

    // Method to toggle Area selection
    private void ToggleAreaSelectionForThesisUploadAsProfessor(long areaId, object isChecked)
    {
        if ((bool)isChecked)
        {
            if (!selectedAreasForProfessorThesis.Contains(areaId))
                selectedAreasForProfessorThesis.Add(areaId);
        }
        else
        {
            selectedAreasForProfessorThesis.Remove(areaId);
        }
        professorthesis.ThesisAreas = string.Join(",", selectedAreas); 
    }

    // Method to toggle Skill selection
    private void ToggleSkillSelectionForThesisUploadAsProfessor(long skillId, object isChecked)
    {
        if ((bool)isChecked)
        {
            if (!selectedSkillsForProfessorThesis.Contains(skillId))
                selectedSkillsForProfessorThesis.Add(skillId);
        }
        else
        {
            selectedSkillsForProfessorThesis.Remove(skillId);
        }
        professorthesis.ThesisSkills = string.Join(",", selectedSkillsForProfessorThesis); 
    }

    private async Task ToggleCheckboxesForThesisAreasAsProfessor()
    {
        await JS.InvokeVoidAsync("toggleProfessorThesisAreasCheckboxes");
    }

    private async Task ToggleCheckboxesForThesisSkillsAsProfessor()
    {
        await JS.InvokeVoidAsync("toggleProfessorThesisSkillsCheckboxes");
    }

    private void OnCheckedChangedForThesisAreasAsProfessor(ChangeEventArgs e, Area area)
    {
        if ((bool)e.Value) // If checked
        {
            if (!selectedThesisAreasForProfessor.Contains(area))
            {
                selectedThesisAreasForProfessor.Add(area);
            }
        }
        else // If unchecked
        {
            selectedThesisAreasForProfessor.Remove(area);
        }
    }


    private void OnCheckedChangedForThesisSkillsAsProfessor(ChangeEventArgs e, Skill skill)
    {
        if ((bool)e.Value) // If checked
        {
            if (!selectedThesisSkillsForProfessor.Contains(skill))
            {
                selectedThesisSkillsForProfessor.Add(skill);
            }
        }
        else // If unchecked
        {
            selectedThesisSkillsForProfessor.Remove(skill);
        }
    }


    private bool IsSelectedForThesisAreasAsProfessor(Area area)
    {
        return selectedAreasForProfessorThesis.Contains(area.Id);
    }

    private bool IsSelectedForThesisSkillsAsProfessor(Skill skill)
    {
        return selectedSkillsForProfessorThesis.Contains(skill.Id);
    }

    private async Task DeleteThesisAsProfessor(int professorThesisId)
    {
        // Find the thesis by ID
        var professorthesis = await dbContext.ProfessorTheses.FindAsync(professorThesisId);

        // Check if the thesis exists
        if (professorthesis != null)
        {
            // Remove the thesis from the database
            dbContext.ProfessorTheses.Remove(professorthesis);
            await dbContext.SaveChangesAsync();

            // Reload the theses to reflect the deletion
            await LoadThesesAsProfessor();
        }
        StateHasChanged();
    }

    private void CloseModalForProfessorThesis()
    {
        isModalVisibleToShowProfessorThesisDetails = false; // Hide the modal
    }  


    private void ShowProfessorThesisDetailsAsStudent(ProfessorThesisApplied professorthesisdetails)
    {
        currentProfessorThesisToShowDetailsAsStudent = professorthesisdetails;
        isModalVisibleForProfessorThesisToShowDetailsAsStudent = true;
    }

    private void ShowCompanyThesisDetailsAsStudent(CompanyThesis companythesisdetails)
    {
        currentCompanyThesisToShowDetailsAsStudent = companythesisdetails;
        isModalVisibleForCompanyThesisToShowDetailsAsStudent = true;
    }

    private async Task ShowProfessorThesisDetailsAsStudent(long thesisId)
    {
        // Fetch the thesis details based on the thesisId
        selectedProfessorThesisDetails = await dbContext.ProfessorTheses
            .FirstOrDefaultAsync(t => t.RNGForThesisUploaded == thesisId);

        // Show the modal after fetching the details
        StateHasChanged();
        await JS.InvokeVoidAsync("showProfessorThesisDetailsModal"); // Show the modal using JS
    }

    private async Task ShowCompanyThesisDetailsAsStudent(long thesisId)
    {
        // Fetch the company thesis details based on the thesisId
        selectedCompanyThesisDetails = await dbContext.CompanyTheses
            .FirstOrDefaultAsync(t => t.RNGForThesisUploadedAsCompany == thesisId);

        // Show the modal after fetching the details
        StateHasChanged();
        await JS.InvokeVoidAsync("showCompanyThesisDetailsModal"); // Show the modal using JS
    }

    private async Task ShowCompanyInternshipDetailsAsStudent(long thesisId)
    {
        isModalVisibleForInternshipsAsStudent = true;
        selectedCompanyInternshipDetails = await dbContext.CompanyInternships
            .Include(i => i.Company) // Include company data
            .FirstOrDefaultAsync(t => t.RNGForInternshipUploadedAsCompany == thesisId); // Updated property name

        // Assign selectedCompanyInternshipDetails to currentInternship
        currentInternship = selectedCompanyInternshipDetails;

        // Show the modal after fetching the details
        StateHasChanged();
        await JS.InvokeVoidAsync("showCompanyInternshipDetailsModal"); // Show the modal using JS
    }

    private async Task ShowProfessorInternshipDetailsAsStudent(long thesisId)
    {
        try
        {
            isModalVisibleForProfessorInternshipsAsStudent = true;
        
            // Fetch internship details with professor information
            selectedProfessorInternshipDetails = await dbContext.ProfessorInternships
                .Include(i => i.Professor)  // Include professor details
                .FirstOrDefaultAsync(t => t.RNGForInternshipUploadedAsProfessor == thesisId);  // Updated property name

            // Assign selectedProfessorInternshipDetails to currentProfessorInternship
            currentProfessorInternship = selectedProfessorInternshipDetails;

            if (currentProfessorInternship == null)
            {
                await JS.InvokeVoidAsync("alert", "Internship details not found");
                isModalVisibleForProfessorInternshipsAsStudent = false;
                return;
            }

            // Show the modal after fetching the details
            StateHasChanged();
            await JS.InvokeVoidAsync("showProfessorInternshipDetailsModal"); // Show the modal using JS
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading internship details: {ex.Message}");
            await JS.InvokeVoidAsync("alert", "Error loading internship details");
            isModalVisibleForProfessorInternshipsAsStudent = false;
        }
    }

    private void CloseProfessorInternshipDetailsModal()
    {
        isModalVisibleForProfessorInternshipsAsStudent = false;
        StateHasChanged();
    }


    private async Task CloseModalForProfessorThesisDetails()
    {
        selectedProfessorThesisDetails = null; // Reset the selected thesis details
        await JS.InvokeVoidAsync("hideProfessorThesisDetailsModal"); // Close the modal using JS
        StateHasChanged(); // Update the UI
    }

    private async Task CloseModalForCompanyThesisDetails()
    {
        selectedCompanyThesisDetails = null; // Reset the selected thesis details
        await JS.InvokeVoidAsync("hideCompanyThesisDetailsModal"); // Close the modal using JS
        StateHasChanged(); // Update the UI
    }

    private async Task CloseModalForCompanyInternshipDetails()
    {
        isModalVisibleForInternships = false;
        selectedCompanyInternshipDetails = null; // Reset the selected thesis details
        await JS.InvokeVoidAsync("hideCompanyInternshipDetailsModal"); // Close the modal using JS
        StateHasChanged(); // Update the UI
    }


    private async Task ShowProfessorHyperlinkNameDetailsModalInStudentThesis(string professorEmail)
    {
        // Fetch professor details based on the professorId
        selectedProfessorDetailsForHyperlinkNameInThesisAsStudent = await dbContext.Professors
            .FirstOrDefaultAsync(p => p.ProfEmail == professorEmail);

        // Show the modal after fetching the details
        StateHasChanged();
        await JS.InvokeVoidAsync("showProfessorDetailsModal"); // Show the modal using JS
    }

    private async Task ShowCompanyHyperlinkNameDetailsModalInStudentThesis(string companyEmail)
    {
        selectedCompanyDetailsForHyperlinkNameInThesisAsStudent = await dbContext.Companies
            .FirstOrDefaultAsync(c => c.CompanyEmail == companyEmail);

        if (selectedCompanyDetailsForHyperlinkNameInThesisAsStudent != null)
        {
            isCompanyDetailsModalOpenForHyperlinkNameAsStudent = true;
            StateHasChanged();
            await JS.InvokeVoidAsync("showCompanyDetailsModal"); // Show the modal using JS
        }
    }


    void CloseModalForProfessorNameHyperlinkDetails()
    {
        selectedProfessorDetailsForHyperlinkNameInThesisAsStudent = null;
        StateHasChanged(); // Ensure the Razor component updates
        JS.InvokeVoidAsync("hideProfessorDetailsModal"); // Call JS to hide the modal
    }

    void CloseModalForCompanyNameHyperlinkDetails()
    {
        isCompanyDetailsModalOpenForHyperlinkNameAsStudent = false;
        selectedCompanyDetailsForHyperlinkNameInThesisAsStudent = null;
        StateHasChanged(); // Ensure the Razor component updates
        JS.InvokeVoidAsync("hideCompanyDetailsModal"); // Call JS to hide the modal
    }

    private string ShowProfileImage(byte[] imageBytes)
    {
        if (imageBytes != null)
        {
            var base64Image = Convert.ToBase64String(imageBytes);
            return $"data:image/png;base64,{base64Image}"; // Assuming the image is in PNG format
        }
        return string.Empty;
    }

    private async Task SearchCompanyThesesAsProfessor()
    {
        companyThesesResultsToFindThesesAsProfessor = await dbContext.CompanyTheses
            .Include(t => t.Company) // Include Company for navigation property access
            .Where(t =>
                t.CompanyThesisStatus == "Δημοσιευμένη" && // Filter for published theses
                (t.IsProfessorInterestedInCompanyThesisStatus == "Δεν έχει γίνει Αποδοχή" || 
                 t.IsProfessorInterestedInCompanyThesisStatus == "Έχετε Αποδεχτεί") && // Include both statuses
                (string.IsNullOrEmpty(searchCompanyNameToFindThesesAsProfessor) || 
                 (t.Company != null && EF.Functions.Like(t.Company.CompanyName, $"%{searchCompanyNameToFindThesesAsProfessor}%"))) &&
                (string.IsNullOrEmpty(searchThesisTitleToFindThesesAsProfessor) || 
                 EF.Functions.Like(t.CompanyThesisTitle, $"%{searchThesisTitleToFindThesesAsProfessor}%")) &&
                (string.IsNullOrEmpty(searchSupervisorToFindThesesAsProfessor) || 
                 EF.Functions.Like(t.CompanyThesisCompanySupervisorFullName, $"%{searchSupervisorToFindThesesAsProfessor}%")) &&
                (string.IsNullOrEmpty(searchDepartmentToFindThesesAsProfessor) || 
                 EF.Functions.Like(t.CompanyThesisDepartment, $"%{searchDepartmentToFindThesesAsProfessor}%")) &&
                (string.IsNullOrEmpty(searchSkillsToFindThesesAsProfessor) || 
                 EF.Functions.Like(t.CompanyThesisSkillsNeeded, $"%{searchSkillsToFindThesesAsProfessor}%")) &&
                (!searchStartingDateToFindThesesAsProfessor.HasValue || 
                 t.CompanyThesisStartingDate.Date >= searchStartingDateToFindThesesAsProfessor.Value.Date)
            )
            .OrderByDescending(t => t.CompanyThesisUploadDateTime) // Added sorting
            .ToListAsync();

        searchPerformedToFindThesesAsProfessor = true;
        StateHasChanged();
    }



    private async Task SearchProfessorThesesAsCompany()
    {
        var query = dbContext.ProfessorTheses
            .Include(t => t.Professor) // Include professor details
            .Where(t =>
                t.ThesisStatus == "Δημοσιευμένη" &&
                (t.IsCompanyInterestedInProfessorThesisStatus == "Δεν έχει γίνει Αποδοχή" ||
                 t.IsCompanyInterestedInProfessorThesisStatus == "Έχετε Αποδεχτεί"));

        // Apply search filters
        if (!string.IsNullOrEmpty(searchProfessorNameToFindThesesAsCompany))
        {
            query = query.Where(t => t.Professor != null && 
                                   t.Professor.ProfName.Contains(searchProfessorNameToFindThesesAsCompany));
        }

        if (!string.IsNullOrEmpty(searchProfessorThesisTitleToFindThesesAsCompany))
        {
            query = query.Where(t => t.ThesisTitle.Contains(searchProfessorThesisTitleToFindThesesAsCompany));
        }

        if (!string.IsNullOrEmpty(searchProfessorSurnameToFindThesesAsCompany))
        {
            query = query.Where(t => t.Professor != null && 
                                   t.Professor.ProfSurname.Contains(searchProfessorSurnameToFindThesesAsCompany));
        }

        if (!string.IsNullOrEmpty(searchSkillsToFindThesesAsCompany))
        {
            query = query.Where(t => t.ThesisSkills.Contains(searchSkillsToFindThesesAsCompany));
        }

        if (searchStartingDateToFindThesesAsCompany.HasValue)
        {
            query = query.Where(t => t.ThesisActivePeriod.Date >= searchStartingDateToFindThesesAsCompany.Value.Date);
        }

        // Execute the query
        var initialQuery = await query.ToListAsync();

        // Apply in-memory filtering for selected areas if any
        if (selectedAreasToFindThesesAsCompany.Any())
        {
            initialQuery = initialQuery
                .Where(t => selectedAreasToFindThesesAsCompany.All(area => 
                    t.ThesisAreas != null && t.ThesisAreas.Contains(area)))
                .ToList();
        }

        professorThesesResultsToFindThesesAsCompany = initialQuery;
        searchPerformedToFindThesesAsCompany = true;
        StateHasChanged();
    }







    private void ClearSearchFieldsForSearchCompanyThesesAsProfessor()
    {
        // Reset search input fields
        searchCompanyNameToFindThesesAsProfessor = string.Empty;
        searchThesisTitleToFindThesesAsProfessor = string.Empty;
        searchSupervisorToFindThesesAsProfessor = string.Empty;
        searchDepartmentToFindThesesAsProfessor = string.Empty;
        searchAreasToFindThesesAsCompany = string.Empty;
        searchSkillsToFindThesesAsProfessor = string.Empty;
        searchStartingDateToFindThesesAsProfessor = null;

        // Clear search results
        companyThesesResultsToFindThesesAsProfessor.Clear();
        searchPerformedToFindThesesAsProfessor = false;

        // Trigger UI update
        StateHasChanged();
    }

    private void ShowThesisDetails(CompanyThesis thesis)
    {
        selectedCompanyThesisToSeeDetailsOnEyeIconAsProfessor = thesis;
        isThesisDetailEyeIconModalVisibleToSeeAsProfessor = true; // Assume this boolean controls the modal visibility
    }

    private void ShowThesisDetailsAsCompany(ProfessorThesis thesis)
    {
        selectedProfessorThesisToSeeDetailsOnEyeIconAsCompany = thesis;
        isThesisDetailEyeIconModalVisibleToSeeAsCompany = true; // Assume this boolean controls the modal visibility
    }


    private async Task ShowCompanyDetailsFromHyperlinkNameToTheProfessor(string companyEmail)
    {
        try
        {
            // Load the company details from database
            selectedCompanyToSeeDetailsOnExpandedInterestAsProfessor = await dbContext.Companies
                .FirstOrDefaultAsync(c => c.CompanyEmail == companyEmail);

            if (selectedCompanyToSeeDetailsOnExpandedInterestAsProfessor != null)
            {
                // Update Blazor state
                isExpandedModalVisibleToSeeCompanyDetailsAsProfessor = true;
                StateHasChanged();
            
                // Call JavaScript to show the modal
                await JS.InvokeVoidAsync("showCompanyDetailsAsProfessorModal", "companyDetailsWhenSearchAsProfessorThesisDetailsModal");
            }
            else
            {
                Console.WriteLine("Company not found with email: " + companyEmail);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error showing company details: {ex.Message}");
        }
    }

    private async Task CloseCompanyModalToShowCompanyDetailsFromHyperlinkNameToTheProfessor()
    {
        try
        {
            // Close the modal via JavaScript first
            await JS.InvokeVoidAsync("hideCompanyDetailsAsProfessorModal", "companyDetailsWhenSearchAsProfessorThesisDetailsModal");
        
            // Then reset the state
            isExpandedModalVisibleToSeeCompanyDetailsAsProfessor = false;
            selectedCompanyToSeeDetailsOnExpandedInterestAsProfessor = null;
        
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error closing modal: {ex.Message}");
        }
    }

    //EDW PREPEI NA PAIRNEI KAI TA ALLA INFO APO PROFESSOR/TO MODELO EXEI GINIE IDI UPDATE 27/11
    private async Task MarkInterestInThesisCompanyThesisAsProfessor(CompanyThesis thesis)
    {
        // First ask for confirmation
        var companyName = thesis.Company?.CompanyName ?? "Άγνωστη Εταιρεία";
        var confirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", 
            $"Πρόκεται να δείξετε ενδιαφέρον για την Πτυχιακή Εργασία: {thesis.CompanyThesisTitle} της Εταιρείας {companyName}. Είστε σίγουρος/η;");

        if (!confirmed) return;

        try
        {
            Console.WriteLine($"RNGForThesisUploadedAsCompany: {thesis.RNGForThesisUploadedAsCompany}");
            Console.WriteLine($"CurrentUserEmail: {CurrentUserEmail}");

            // Check if ANY professor already showed interest for this thesis
            if (thesis.IsProfessorInteresetedInCompanyThesis && 
                !string.IsNullOrWhiteSpace(thesis.ProfessorEmailInterestedInCompanyThesis))
            {
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("showToast", 
                    "error", 
                    "Η πτυχιακή εργασία δεν είναι πλέον διαθέσιμη. Έχει ήδη εκδηλωθεί ενδιαφέρον από άλλο καθηγητή.").AsTask());
                return;
            }

            // Check if THIS specific professor already showed interest
            if (thesis.ProfessorEmailInterestedInCompanyThesis == CurrentUserEmail && 
                thesis.IsProfessorInteresetedInCompanyThesis)
            {
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("showToast", 
                    "error", 
                    "Έχετε ήδη δείξει ενδιαφέρον για αυτήν την πτυχιακή εργασία").AsTask());
                return;
            }

            // Get current professor details
            var professor = await dbContext.Professors
                .FirstOrDefaultAsync(p => p.ProfEmail == CurrentUserEmail);
    
            if (professor == null)
            {
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("showToast", 
                    "error", 
                    "Δεν βρέθηκαν στοιχεία καθηγητή").AsTask());
                return;
            }

            // Update thesis with professor's interest
            thesis.IsProfessorInteresetedInCompanyThesis = true;
            thesis.ProfessorEmailInterestedInCompanyThesis = CurrentUserEmail;
            thesis.IsProfessorInterestedInCompanyThesisStatus = "Έχετε Αποδεχτεί";
            thesis.ProfessorInterested = professor; // Set navigation property

            // Create platform action
            var platformAction = new PlatformActions
            {
                UserRole_PerformedAction = "PROFESSOR",
                ForWhat_PerformedAction = "COMPANY_THESIS",
                HashedPositionRNG_PerformedAction = HashingHelper.HashLong(thesis.RNGForThesisUploadedAsCompany),
                TypeOfAction_PerformedAction = "SHOW_INTEREST",
                DateTime_PerformedAction = DateTime.UtcNow
            };

            dbContext.PlatformActions.Add(platformAction);
            dbContext.CompanyTheses.Update(thesis);
            await dbContext.SaveChangesAsync();

            // Send notifications
            await InternshipEmailService.SendCompanyThesisInterestNotificationToCompany(
                thesis.CompanyEmailUsedToUploadThesis,
                companyName,
                professor.ProfName,
                professor.ProfSurname,
                professor.ProfUniversity,
                professor.ProfDepartment,
                professor.ProfWorkTelephone,
                professor.ProfPersonalTelephoneVisibility ? professor.ProfPersonalTelephone : null,
                professor.ProfPersonalWebsite,
                thesis.CompanyThesisTitle,
                thesis.RNGForThesisUploadedAsCompany_HashedAsUniqueID);

            await InternshipEmailService.SendCompanyThesisInterestConfirmationToProfessor(
                CurrentUserEmail,
                $"{professor.ProfName} {professor.ProfSurname}",
                thesis.CompanyThesisTitle,
                thesis.RNGForThesisUploadedAsCompany_HashedAsUniqueID,
                companyName);

            await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                "success", 
                "Επιτυχής ένδειξη ενδιαφέροντος").AsTask());
    
            await LoadThesisData();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving interest: {ex.Message}");
            await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                "error", 
                "Σφάλμα κατά την αποθήκευση").AsTask());
        }
    }



    private async Task MarkInterestInProfessorThesis(ProfessorThesis thesis)
    {
        // First ask for confirmation
        var professorName = $"{thesis.Professor?.ProfName} {thesis.Professor?.ProfSurname}" ?? "Άγνωστος Καθηγητής";
        var confirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", 
            $"Πρόκεται να δείξετε ενδιαφέρον για την Πτυχιακή Εργασία: {thesis.ThesisTitle} του/της Καθηγητή/τριας {professorName}. Είστε σίγουρος/η;");
    
        if (!confirmed)
        {
            return; // User cancelled the action
        }

        try
        {
            Console.WriteLine($"RNGForThesisUploaded: {thesis.RNGForThesisUploaded}");
            Console.WriteLine($"CurrentUserEmail: {CurrentUserEmail}");

            // Check if ANY company already showed interest for this thesis
            if (thesis.IsCompanyInteresetedInProfessorThesis && 
                !string.IsNullOrWhiteSpace(thesis.CompanyEmailInterestedInProfessorThesis))
            {
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("showToast", 
                    "error", 
                    "Η πτυχιακή εργασία δεν είναι πλέον διαθέσιμη. Έχει ήδη εκδηλωθεί ενδιαφέρον από άλλη εταιρεία.").AsTask());
                return;
            }

            // Check if THIS specific company already showed interest
            if (thesis.CompanyEmailInterestedInProfessorThesis == CurrentUserEmail && 
                thesis.IsCompanyInteresetedInProfessorThesis)
            {
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("showToast", 
                    "error", 
                    "Έχετε ήδη δείξει ενδιαφέρον για αυτήν την πτυχιακή εργασία").AsTask());
                return;
            }

            // Get existing company data instead of creating new
            var company = await dbContext.Companies
                .FirstOrDefaultAsync(c => c.CompanyEmail == CurrentUserEmail);

            if (company == null)
            {
                Console.WriteLine("Company not found in database");
                await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("showToast", 
                    "error", 
                    "Δεν βρέθηκαν στοιχεία εταιρείας").AsTask());
                return;
            }

            // Update thesis with company interest
            thesis.IsCompanyInteresetedInProfessorThesis = true;
            thesis.IsCompanyInterestedInProfessorThesisStatus = "Έχετε Αποδεχτεί";
            thesis.CompanyEmailInterestedInProfessorThesis = CurrentUserEmail;
            thesis.CompanyInterested = company;  // Set navigation property
            thesis.ThesisUpdateDateTime = DateTime.Now;

            // Update the existing record in the database
            dbContext.ProfessorTheses.Update(thesis);

            // Create platform action
            var platformAction = new PlatformActions
            {
                UserRole_PerformedAction = "COMPANY",
                ForWhat_PerformedAction = "PROFESSOR_THESIS",
                HashedPositionRNG_PerformedAction = thesis.RNGForThesisUploaded_HashedAsUniqueID,
                TypeOfAction_PerformedAction = "SHOW_INTEREST",
                DateTime_PerformedAction = DateTime.UtcNow
            };
            dbContext.PlatformActions.Add(platformAction);

            // Save changes to the database
            await dbContext.SaveChangesAsync();

            // Send notifications
            await InternshipEmailService.SendProfessorThesisInterestNotificationToProfessor(
                thesis.ProfessorEmailUsedToUploadThesis,
                professorName,
                company.CompanyName,
                company.CompanyHRName,
                company.CompanyHRSurname,
                company.CompanyHREmail,
                company.CompanyHRTelephone,
                thesis.ThesisTitle,
                thesis.RNGForThesisUploaded_HashedAsUniqueID);

            await InternshipEmailService.SendProfessorThesisInterestConfirmationToCompany(
                CurrentUserEmail,
                company.CompanyName,
                company.CompanyHRName,
                company.CompanyHRSurname,
                thesis.ThesisTitle,
                thesis.RNGForThesisUploaded_HashedAsUniqueID,
                professorName);

            await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                "success", 
                "Επιτυχής ένδειξη ενδιαφέροντος").AsTask());

            // Refresh the thesis data to reflect updates
            await LoadThesisData();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving interest: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            await SafeInvokeJsAsync(() => JS.InvokeVoidAsync("confirmActionWithHTML2", 
                "error", 
                "Σφάλμα κατά την αποθήκευση").AsTask());
        }
    }





    private async Task<bool> IsProfessorInterestedInThesis(CompanyThesis thesis)
    {
        var isInterested = await dbContext.CompanyTheses
            .AnyAsync(x => x.ProfessorEmailInterestedInCompanyThesis == CurrentUserEmail 
                        && x.RNGForThesisUploadedAsCompany == thesis.RNGForThesisUploadedAsCompany);

        // Log the executed condition
        Console.WriteLine($"Checking Thesis ID: {thesis.RNGForThesisUploadedAsCompany}, " +
                          $"Professor Email: {CurrentUserEmail}, Result: {isInterested}");

        return isInterested;
    }

    public class ThesisWithInterestStatus
    {
        public CompanyThesis Thesis { get; set; }
        public bool IsInterested { get; set; }
    }

    public class ProfessorThesisWithInterestStatus
    {
        public ProfessorThesis ProfessorThesis { get; set; }
        public bool IsInterested { get; set; }
    }

    private async Task LoadThesisData()
    {
        var theses = await dbContext.CompanyTheses.ToListAsync();

        // Create a list to store the status for each thesis
        thesesWithInterestStatus = theses.Select(thesis => new ThesisWithInterestStatus
        {
            Thesis = thesis,
            IsInterested = thesis.ProfessorEmailInterestedInCompanyThesis == CurrentUserEmail
        }).ToList();
    }

    private async Task LoadProfessorThesisData()
    {
        var theses = await dbContext.ProfessorTheses.ToListAsync();

        // Create a list to store the status for each thesis
        professorthesesWithInterestStatus = theses.Select(thesis => new ProfessorThesisWithInterestStatus
        {
            ProfessorThesis = thesis,
            IsInterested = thesis.CompanyEmailInterestedInProfessorThesis == CurrentUserEmail
        }).ToList();
    }

    private async Task ShowProfessorDetailsAtCompanyThesisInterest(CompanyThesis companyThesis)
    {
        try
        {
            // Try to get professor from navigation property first
            if (companyThesis.ProfessorInterested == null && 
                !string.IsNullOrEmpty(companyThesis.ProfessorEmailInterestedInCompanyThesis))
            {
                // Explicitly load the professor if not already loaded
                await dbContext.Entry(companyThesis)
                    .Reference(t => t.ProfessorInterested)
                    .LoadAsync();
            }

            // Use the navigation property if available
            currentProfessorDetails = companyThesis.ProfessorInterested;

            if (currentProfessorDetails == null)
            {
                // Create minimal professor object with just the email if no professor found
                currentProfessorDetails = new Professor
                {
                    ProfEmail = companyThesis.ProfessorEmailInterestedInCompanyThesis
                };
            }

            isModalVisibleToShowprofessorDetailsAtCompanyThesisInterest = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading professor details: {ex.Message}");
            currentProfessorDetails = new Professor();
            isModalVisibleToShowprofessorDetailsAtCompanyThesisInterest = true;
        }
    }


    private void ShowCompanyDetailsAtProfessorThesisInterestFromHyperlinkCompanyName(ProfessorThesis professorThesis)
    {
        if (professorThesis?.CompanyInterested != null)
        {
            currentCompanyDetails = new Company
            {
                CompanyLogo = professorThesis.CompanyInterested.CompanyLogo,
                CompanyNameENG = professorThesis.CompanyInterested.CompanyName,
                CompanyEmail = professorThesis.CompanyInterested.CompanyEmail,
                CompanyType = professorThesis.CompanyInterested.CompanyType,
                CompanyActivity = professorThesis.CompanyInterested.CompanyActivity,
                CompanyTelephone = professorThesis.CompanyInterested.CompanyTelephone,
                CompanyWebsite = professorThesis.CompanyInterested.CompanyWebsite,
                CompanyWebsiteAnnouncements = professorThesis.CompanyInterested.CompanyWebsiteAnnouncements,
                CompanyWebsiteJobs = professorThesis.CompanyInterested.CompanyWebsiteJobs,
                CompanyCountry = professorThesis.CompanyInterested.CompanyCountry,
                CompanyLocation = professorThesis.CompanyInterested.CompanyLocation,
                CompanyPC = professorThesis.CompanyInterested.CompanyPC,
                CompanyRegions = professorThesis.CompanyInterested.CompanyRegions,
                CompanyTown = professorThesis.CompanyInterested.CompanyTown,
                CompanyDescription = professorThesis.CompanyInterested.CompanyDescription,
                CompanyAreas = professorThesis.CompanyInterested.CompanyAreas,
                CompanyDesiredSkills = professorThesis.CompanyInterested.CompanyDesiredSkills,
                CompanyHRName = professorThesis.CompanyInterested.CompanyHRName,
                CompanyHRSurname = professorThesis.CompanyInterested.CompanyHRSurname,
                CompanyHREmail = professorThesis.CompanyInterested.CompanyHREmail,
                CompanyHRTelephone = professorThesis.CompanyInterested.CompanyHRTelephone
            };

            isModalVisibleToShowCompanyDetailsAtProfessorThesisInterest = true;
        }
    }


    private async Task CloseModalWhichShowsProfessorDetailsAtCompanyThesisInterest()
    {
        await JS.InvokeVoidAsync("hideProfessorDetailsModalForThesisInterest");
    }

    private async Task CloseModalWhichShowsCompanyDetailsAtProfessorThesisInterest()
    {
        await JS.InvokeVoidAsync("hideCompanyDetailsModalForThesisInterest");
    }

    private void ClearSearchFieldsForSearchProfessorThesesAsCompany()
    {
        searchProfessorNameToFindThesesAsCompany = string.Empty;
        searchProfessorThesisTitleToFindThesesAsCompany = string.Empty;
        searchProfessorSurnameToFindThesesAsCompany = string.Empty;
        searchSkillsToFindThesesAsCompany = string.Empty;
        searchStartingDateToFindThesesAsCompany = DateTime.Now;
        searchAreasInputToFindThesesAsCompany = string.Empty;

        selectedAreasToFindThesesAsCompany.Clear(); 
        
        professorThesesResultsToFindThesesAsCompany = null;
        searchPerformedToFindThesesAsCompany = false;
    }




    private void CheckCharacterLimitInCompanyEventDescription(ChangeEventArgs e)
    {
        var inputText = e.Value?.ToString() ?? string.Empty;
        remainingCharactersInCompanyEventDescription = 1000 - inputText.Length;
    }

    private void CheckCharacterLimitInEventTitleField(ChangeEventArgs e)
    {
        var inputText = e.Value?.ToString() ?? string.Empty;
        remainingCharactersInEventTitleField = 120 - inputText.Length;
    }

    private async Task HandleTemporarySaveCompanyEvent()
    {
        // Show custom confirmation dialog with formatted text
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML",
            "Είστε σίγουροι πως θέλετε να <strong style='color: blue;'>Υποβάλετε</strong> την Εκδήλωση;<br><br>" +
            "Η εκδήλωση θα καταχωρηθεί ως <strong style='color: red;'>'Μη Δημοσιευμένη'</strong>.<br><br>" +
            "Θέλετε να συνεχίσετε;");

        if (isConfirmed)
        {
            await SaveCompanyEvent(false); 
        }
    }

    private async Task HandlePublishSaveCompanyEvent()
    {
        // Show custom confirmation dialog with formatted text
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML",
            "Είστε σίγουροι πως θέλετε να <strong style='color: green;'>Υποβάλετε</strong> την Εκδήλωση;<br><br>" +
            "Η εκδήλωση θα καταχωρηθεί ως <strong style='color: green;'>'Δημοσιευμένη'</strong>.<br><br>" +
            "Θέλετε να συνεχίσετε;");

        if (isConfirmed)
        {
            await SaveCompanyEvent(true); 
        }
    }


    private async Task SaveCompanyEvent(bool publishEvent)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(companyEvent.CompanyEventTitle) || 
                string.IsNullOrWhiteSpace(companyEvent.CompanyEventDescription) || 
                companyEvent.CompanyEventActiveDate <= DateTime.Today ||
                companyEvent.CompanyEventTimeOnly == TimeOnly.MinValue ||
                companyEvent.CompanyEventOfferingTransportToEventLocation == null ||
                !SelectedAreasWhenUploadEventAsCompany.Any())
            {
                showErrorMessageForUploadingCompanyEvent = true;
                isFormValidToSaveEventAsCompany = false;
                return;
            }

            companyEvent.CompanyEventAreasOfInterest = string.Join(",", SelectedAreasWhenUploadEventAsCompany.Select(a => a.AreaName));

            if (dbContext.Entry(companyEvent).State == EntityState.Detached)
            {
                dbContext.CompanyEvents.Add(companyEvent);
            }

            companyEvent.RNGForEventUploadedAsCompany = new Random().NextInt64();
            companyEvent.RNGForEventUploadedAsCompany_HashedAsUniqueID = HashingHelper.HashLong(companyEvent.RNGForEventUploadedAsCompany);
            companyEvent.CompanyEventStatus = publishEvent ? "Δημοσιευμένη" : "Μη Δημοσιευμένη";
            companyEvent.CompanyEventUploadedDate = DateTime.Now;
            companyEvent.CompanyEventTime = companyEvent.CompanyEventTimeOnly.ToTimeSpan();
        
            // Set contact information (kept exactly as before)
            companyEvent.CompanyEventResponsiblePerson = $"{companyData.CompanyHRName} {companyData.CompanyHRSurname}";
            companyEvent.CompanyEventResponsiblePersonEmail = companyData.CompanyHREmail;
            companyEvent.CompanyEventResponsiblePersonTelephone = companyData.CompanyHRTelephone;
            companyEvent.CompanyEventCompanyDepartment = companyEvent.CompanyEventCompanyDepartment;
        
            // Set foreign key using companyData instead of Company
            companyEvent.CompanyEmailUsedToUploadEvent = companyData.CompanyEmail;

            await dbContext.SaveChangesAsync();
            saveEventAsCompanyMessage = "Η Εκδήλωση Δημιουργήθηκε Επιτυχώς";
            showSuccessMessage = true;
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Κάποιο Πρόβλημα Παρουσιάστηκε, Προσπαθήστε ξανά: {ex.Message}");
            Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
            showSuccessMessage = false;
            showErrorMessageForUploadingCompanyEvent = true;
        }
    }




    protected override async Task OnAfterRenderAsync(bool firstRender)
    {

        await Task.Delay(200); 
        await JS.InvokeVoidAsync("initializeAutocomplete");
        await JS.InvokeVoidAsync("initializeAutocomplete2");
        await JS.InvokeVoidAsync("initializeAutocomplete3");
        await JS.InvokeVoidAsync("initializeAutocomplete4");

        await JS.InvokeVoidAsync("initializeAutocomplete6");
        await JS.InvokeVoidAsync("initializeAutocomplete7");
        await JS.InvokeVoidAsync("initializeAutocomplete8");

    }

    [JSInvokable]
    public static Task UpdatePlaceDetails(List<GooglePlaceComponent> addressComponents)
    {
        return Task.CompletedTask;
    }

    public class GooglePlaceComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

    private void SearchStudentsAsCompanyToFindStudent()
    {
        var combinedSearchAreas = new List<string>();

        if (selectedAreasOfExpertise != null && selectedAreasOfExpertise.Any())
            combinedSearchAreas.AddRange(selectedAreasOfExpertise);

        if (!string.IsNullOrWhiteSpace(searchAreasOfExpertise))
            combinedSearchAreas.Add(searchAreasOfExpertise);

        var normalizedSearchAreas = combinedSearchAreas
            .SelectMany(area => area.Split('/', StringSplitOptions.RemoveEmptyEntries))
            .Select(area => area.Trim().ToLower())
            .Distinct()
            .ToList();

        var combinedSearchKeywords = new List<string>();

        if (selectedKeywords != null && selectedKeywords.Any())
            combinedSearchKeywords.AddRange(selectedKeywords);

        if (!string.IsNullOrWhiteSpace(searchKeywords))
            combinedSearchKeywords.Add(searchKeywords);

        var normalizedSearchKeywords = combinedSearchKeywords
            .Select(keyword => keyword.Trim().ToLower())
            .Distinct()
            .ToList();

        searchResultsAsCompanyToFindStudent = dbContext.Students
            .AsEnumerable()
            .Where(s =>
            {
                var normalizedStudentAreas = NormalizeAreas(s.AreasOfExpertise);

                var normalizedKeywords = s.Keywords?
                    .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(k => k.Trim().ToLower())
                    .Distinct()
                    .ToList() ?? new List<string>();

                // Debugging
                Console.WriteLine($"Student: {s.Name} {s.Surname}");
                Console.WriteLine($"Student Areas: {string.Join(", ", normalizedStudentAreas)}");
                Console.WriteLine($"Search Areas: {string.Join(", ", normalizedSearchAreas)}");
                Console.WriteLine($"Student Keywords: {string.Join(", ", normalizedKeywords)}");
                Console.WriteLine($"Search Keywords: {string.Join(", ", normalizedSearchKeywords)}");

                // Filter by school if selected
                bool schoolMatch = true;
                if (!string.IsNullOrEmpty(searchSchoolAsCompanyToFindStudent))
                {
                    var schoolDepartments = universityDepartments[searchSchoolAsCompanyToFindStudent];
                    schoolMatch = schoolDepartments.Contains(s.Department);
                }

                var areaMatch = !normalizedSearchAreas.Any() ||
                    normalizedStudentAreas.Any(studentArea =>
                        normalizedSearchAreas.Contains(studentArea));

                var keywordMatch = !normalizedSearchKeywords.Any() ||
                    normalizedKeywords.Any(studentKeyword =>
                        normalizedSearchKeywords.Contains(studentKeyword));

                return (string.IsNullOrEmpty(searchNameOrSurname) ||
                            (s.Name + " " + s.Surname).Contains(searchNameOrSurname, StringComparison.OrdinalIgnoreCase)) &&
                       (string.IsNullOrEmpty(searchRegNumberAsCompanyToFindStudent) ||
                            s.RegNumber.ToString().Contains(searchRegNumberAsCompanyToFindStudent)) &&
                       (string.IsNullOrEmpty(searchSchoolAsCompanyToFindStudent) || schoolMatch) &&
                       (string.IsNullOrEmpty(searchDepartmentAsCompanyToFindStudent) ||
                            s.Department == searchDepartmentAsCompanyToFindStudent) &&
                       (string.IsNullOrEmpty(InternshipStatus) ||
                            s.InternshipStatus == InternshipStatus) &&
                       (string.IsNullOrEmpty(ThesisStatus) ||
                            s.ThesisStatus == ThesisStatus) &&
                       areaMatch &&
                       keywordMatch &&
                       (string.IsNullOrEmpty(selectedDegreeLevel) ||
                            s.LevelOfDegree == selectedDegreeLevel);
            })
            .ToList();
    }




    private IEnumerable<string> NormalizeAreas(string Areas)
    {
        if (string.IsNullOrWhiteSpace(Areas))
            return Array.Empty<string>();

        return Areas
            .Split(new string[] { ",", "/", ", ", " / ", " ," }, StringSplitOptions.RemoveEmptyEntries)
            .Select(area => area.Trim().ToLower()) 
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Where(area => !string.IsNullOrEmpty(area)); 
    }

    // Normalize Keywords (convert to lowercase)
    private IEnumerable<string> NormalizeKeywords(string keywords)
    {
        if (string.IsNullOrWhiteSpace(keywords))
            return Array.Empty<string>();

        return keywords
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(keyword => keyword.Trim().ToLower())  
            .Distinct(StringComparer.OrdinalIgnoreCase) 
            .Where(keyword => !string.IsNullOrEmpty(keyword)); 
    }



    private void ClearSearchFieldsAsCompanyToFindStudent()
    {
        searchNameOrSurname = string.Empty;
        searchRegNumberAsCompanyToFindStudent = string.Empty;
        searchSchoolAsCompanyToFindStudent = string.Empty;
        searchDepartmentAsCompanyToFindStudent = string.Empty;
        InternshipStatus = string.Empty;
        ThesisStatus = string.Empty;
        searchAreasOfExpertise = string.Empty;
        searchKeywords = string.Empty;
        selectedDegreeLevel = string.Empty;
        searchResultsAsCompanyToFindStudent = null;

        nameSurnameSuggestions.Clear();
        areasOfExpertiseSuggestions.Clear();
        keywordsSuggestions.Clear();

        selectedKeywords.Clear();
        selectedAreasOfExpertise.Clear();

        StateHasChanged();
    }



    private void OnTransportOptionChange(ChangeEventArgs e)
    {
        if (bool.TryParse(e.Value?.ToString(), out var result))
        {
            companyEvent.CompanyEventOfferingTransportToEventLocation = result;
        }
    }

    private void OnTransportOptionChangeForProfessorEvent(ChangeEventArgs e)
    {
        if (bool.TryParse(e.Value?.ToString(), out var result))
        {
            professorEvent.ProfessorEventOfferingTransportToEventLocation = result;
        }
    }

    private async Task UploadCompanyEventAttachmentFile(InputFileChangeEventArgs e)
    {
        if (e.File != null)
        {
            using var memoryStream = new MemoryStream();
            await e.File.OpenReadStream().CopyToAsync(memoryStream);
            companyEvent.CompanyEventAttachmentFile = memoryStream.ToArray();
        }
    }

    private async Task UploadProfessorEventAttachmentFile(InputFileChangeEventArgs e)
    {
        if (e.File != null)
        {
            using var memoryStream = new MemoryStream();
            await e.File.OpenReadStream().CopyToAsync(memoryStream);
            professorEvent.ProfessorEventAttachmentFile = memoryStream.ToArray();
        }
    }

    private async Task ToggleAndLoadCompanyEventsAsStudent()
    {
        isCompanyEventsVisibleToSeeAsStudent = !isCompanyEventsVisibleToSeeAsStudent;
        Console.WriteLine("Toggle button clicked. Visibility: " + isCompanyEventsVisibleToSeeAsStudent);

        if (isCompanyEventsVisibleToSeeAsStudent && (companyEventsToSeeAsStudent == null || !companyEventsToSeeAsStudent.Any()))
        {
            try
            {
                companyEventsToSeeAsStudent = await dbContext.CompanyEvents.ToListAsync();
                Console.WriteLine("Events loaded: " + companyEventsToSeeAsStudent.Count); // Log event count

                StateHasChanged(); // Ensure UI refresh
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching company events: " + ex.Message);
            }
        }
    }


    private void ShowCompanyEventDetails(CompanyEvent eventDetails)
    {
        currentCompanyEvent = eventDetails;
        isModalOpenForCompanyEventToSeeAsStudent = true;  // Set the modal open state to true
        StateHasChanged();  // To trigger a re-render and show the modal
    }

    private void CloseModalForCompanyEventToSeeAsStudent()
    {
        currentCompanyEvent = null;
        isModalOpenForCompanyEventToSeeAsStudent = false; // Set the modal open state to false
        StateHasChanged(); // To trigger a re-render and hide the modal
    }



    private void ShowProfessorEventDetails(ProfessorEvent eventDetails)
    {
        currentProfessorEvent = eventDetails;
        isModalOpenForProfessorEventToSeeAsStudent = true;  // Set the modal open state to true
        StateHasChanged();  // To trigger a re-render and show the modal
    }

    private void CloseModalForProfessorEventToSeeAsStudent()
    {
        currentProfessorEvent = null;
        isModalOpenForProfessorEventToSeeAsStudent = false; // Set the modal open state to false
        StateHasChanged(); // To trigger a re-render and hide the modal
    }


    private async Task ShowCompanyDetailsModalAtEventsAsStudent(string companyEmail)
    {
        currentCompanyDetailsToShowOnHyperlinkAsStudentForCompanyEvents = 
            await dbContext.Companies.FirstOrDefaultAsync(c => c.CompanyEmail == companyEmail);

        if (currentCompanyDetailsToShowOnHyperlinkAsStudentForCompanyEvents != null)
        {
            isModalOpenForCompanyDetailsToSeeAsStudent = true;
            await JS.InvokeVoidAsync("showCompanyDetailsModalForEventsAsStudent");
        }
        else
        {
            Console.WriteLine("Company details not found for company name with email: " + companyEmail);
        }
    }

    private void CloseCompanyDetailsModalAtEventsAsStudent()
    {
        currentCompanyDetailsToShowOnHyperlinkAsStudentForCompanyEvents = null;
        isModalOpenForCompanyDetailsToSeeAsStudent = false;
        JS.InvokeVoidAsync("hideCompanyDetailsModalForEventsAsStudent");
    }

    private async Task ShowProfessorDetailsModalAtEventsAsStudent(string professorEmail)
    {
        currentProfessorDetailsToShowOnHyperlinkAsStudentForProfessorEvents = 
            await dbContext.Professors.FirstOrDefaultAsync(c => c.ProfEmail == professorEmail);

        if (currentProfessorDetailsToShowOnHyperlinkAsStudentForProfessorEvents != null)
        {
            isModalOpenForProfessorDetailsToSeeAsStudent = true;
            await JS.InvokeVoidAsync("showProfessorDetailsModalForEventsAsStudent");
        }
        else
        {
            Console.WriteLine("Professor details not found for company name with email: " + professorEmail);
        }
    }

    
    private void CloseProfessorDetailsModalAtEventsAsStudent()
    {
        currentProfessorDetailsToShowOnHyperlinkAsStudentForProfessorEvents = null;
        isModalOpenForProfessorDetailsToSeeAsStudent = false;
        JS.InvokeVoidAsync("hideProfessorDetailsModalForEventsAsStudent");
    }




    private async Task<bool> ShowInterestInCompanyEvent(CompanyEvent companyEvent, bool needsTransportForCompanyEvent)
    {
        // First ask for confirmation
        var confirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", 
            $"Πρόκεται να δείξετε Ενδιαφέρον για την Εκδήλωση: {companyEvent.CompanyEventTitle} της εταιρείας {companyEvent.Company?.CompanyName}. Είστε σίγουρος/η;");
        if (!confirmed) return false;

        // Retrieve the latest event status
        var latestEvent = await dbContext.CompanyEvents
            .AsNoTracking()
            .Where(e => e.RNGForEventUploadedAsCompany == companyEvent.RNGForEventUploadedAsCompany)
            .Select(e => new { e.CompanyEventStatus })
            .FirstOrDefaultAsync();

        if (latestEvent == null || latestEvent.CompanyEventStatus != "Δημοσιευμένη")
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Η Εκδήλωση έχει Αποδημοσιευτεί. Παρακαλώ δοκιμάστε αργότερα.");
            return false;
        }

        var student = await GetStudentDetails(CurrentUserEmail);
        if (student == null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Δεν βρέθηκαν στοιχεία φοιτητή.");
            return false;
        }

        // Check for existing interest
        var existingInterest = await dbContext.InterestInCompanyEvents
            .FirstOrDefaultAsync(i => 
                i.StudentEmailShowInterestForEvent == student.Email &&
                i.RNGForCompanyEventInterest == companyEvent.RNGForEventUploadedAsCompany);

        if (existingInterest != null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", $"Έχετε ήδη δείξει ενδιαφέρον για: {companyEvent.CompanyEventTitle}!");
            return false;
        }

        if (!selectedStartingPoint.TryGetValue(companyEvent.RNGForEventUploadedAsCompany, out var chosenLocation))
        {
            await ShowAlert("Παρακαλώ επιλέξτε μια τοποθεσία μετακίνησης πριν δείξετε ενδιαφέρον.");
            return false;
        }

        // Get company data
        var company = await dbContext.Companies
            .FirstOrDefaultAsync(c => c.CompanyEmail == companyEvent.CompanyEmailUsedToUploadEvent);

        if (company == null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Σφάλμα: Δεν βρέθηκε η εταιρία.");
            return false;
        }

        using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            // Create main interest record with details
            var interest = new InterestInCompanyEvent
            {
                RNGForCompanyEventInterest = companyEvent.RNGForEventUploadedAsCompany,
                RNGForCompanyEventInterest_HashedAsUniqueID = companyEvent.RNGForEventUploadedAsCompany_HashedAsUniqueID,
                DateTimeStudentShowInterest = DateTime.UtcNow,
                CompanyEventStatusAtStudentSide = "Έχετε Δείξει Ενδιαφέρον",
                CompanyEventStatusAtCompanySide = "Προς Επεξεργασία",
                CompanyEmailWhereStudentShowedInterest = companyEvent.CompanyEmailUsedToUploadEvent,
                CompanyUniqueIDWhereStudentShowedInterest = company.Company_UniqueID,
                StudentEmailShowInterestForEvent = student.Email,
                StudentUniqueIDShowInterestForEvent = student.Student_UniqueID,
                StudentTransportNeedWhenShowInterestForCompanyEvent = needsTransportForCompanyEvent ? "Ναι" : "Όχι",
                StudentTransportChosenLocationWhenShowInterestForCompanyEvent = chosenLocation,
        
                StudentDetails = new InterestInCompanyEvent_StudentDetails
                {
                    StudentUniqueIDShowInterestForCompanyEvent = student.Student_UniqueID,
                    StudentEmailShowInterestForCompanyEvent = student.Email,
                    DateTimeStudentShowInterestForCompanyEvent = DateTime.UtcNow,
                    RNGForCompanyEventShowInterestAsStudent_HashedAsUniqueID = companyEvent.RNGForEventUploadedAsCompany_HashedAsUniqueID
                },
        
                CompanyDetails = new InterestInCompanyEvent_CompanyDetails
                {
                    CompanyUniqueIDWhereStudentShowInterestForCompanyEvent = company.Company_UniqueID,
                    CompanyEmailWhereStudentShowInterestForCompanyEvent = companyEvent.CompanyEmailUsedToUploadEvent
                }
            };

            dbContext.InterestInCompanyEvents.Add(interest);
    
            // Add platform action
            dbContext.PlatformActions.Add(new PlatformActions
            {
                UserRole_PerformedAction = "STUDENT",
                ForWhat_PerformedAction = "COMPANY_EVENT",
                HashedPositionRNG_PerformedAction = HashingHelper.HashLong(companyEvent.RNGForEventUploadedAsCompany),
                TypeOfAction_PerformedAction = "SHOW_INTEREST",
                DateTime_PerformedAction = DateTime.UtcNow
            });

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            // Send emails
            try
            {
                await InternshipEmailService.SendConfirmationToStudentForInterestInCompanyEvent(
                    student.Email,
                    student.Name,
                    student.Surname,
                    companyEvent.CompanyEventTitle,
                    company.CompanyName,
                    companyEvent.RNGForEventUploadedAsCompany_HashedAsUniqueID,
                    needsTransportForCompanyEvent,
                    chosenLocation);

                await InternshipEmailService.SendNotificationToCompanyForStudentInterestForCompanyEvent(
                    companyEvent.CompanyEmailUsedToUploadEvent,
                    company.CompanyName,
                    student.Name,
                    student.Surname,
                    student.Email,
                    student.Telephone,
                    student.StudyYear,
                    companyEvent.CompanyEventTitle,
                    companyEvent.RNGForEventUploadedAsCompany_HashedAsUniqueID,
                    needsTransportForCompanyEvent,
                    chosenLocation);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
            }

            await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Η έκφραση ενδιαφέροντος για την εκδήλωση {companyEvent.CompanyEventTitle} υποβλήθηκε επιτυχώς!");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Full error: {ex}");
            await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Σφάλμα κατά την υποβολή: {ex.InnerException?.Message ?? ex.Message}");
            return false;
        }

        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        return false;
    }

    private async Task<bool> ShowInterestInProfessorEvent(ProfessorEvent professorEvent, bool needsTransportForProfessorEvent)
    {
        // First ask for confirmation - now accessing professor name through navigation property
        var confirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", 
            $"Πρόκεται να δείξετε Ενδιαφέρον για την Εκδήλωση: {professorEvent.ProfessorEventTitle} του Καθηγητή {professorEvent.Professor?.ProfName} {professorEvent.Professor?.ProfSurname}. Είστε σίγουρος/η;");
        if (!confirmed) return false;

        // Retrieve the latest event status with professor included
        var latestEvent = await dbContext.ProfessorEvents
            .Include(e => e.Professor)
            .AsNoTracking()
            .Where(e => e.RNGForEventUploadedAsProfessor == professorEvent.RNGForEventUploadedAsProfessor)
            .Select(e => new { e.ProfessorEventStatus })
            .FirstOrDefaultAsync();

        if (latestEvent == null || latestEvent.ProfessorEventStatus != "Δημοσιευμένη")
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Η Εκδήλωση έχει Αποδημοσιευτεί. Παρακαλώ δοκιμάστε αργότερα.");
            return false;
        }

        var student = await GetStudentDetails(CurrentUserEmail);
        if (student == null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Δεν βρέθηκαν στοιχεία φοιτητή.");
            return false;
        }

        // Check for existing interest
        var existingInterest = await dbContext.InterestInProfessorEvents
            .FirstOrDefaultAsync(i => 
                i.StudentEmailShowInterestForEvent == student.Email &&
                i.RNGForProfessorEventInterest == professorEvent.RNGForEventUploadedAsProfessor);

        if (existingInterest != null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", $"Έχετε ήδη δείξει ενδιαφέρον για: {professorEvent.ProfessorEventTitle}!");
            return false;
        }

        if (!selectedStartingPoint.TryGetValue(professorEvent.RNGForEventUploadedAsProfessor, out var chosenLocation))
        {
            await ShowAlert("Παρακαλώ επιλέξτε μια τοποθεσία μετακίνησης πριν δείξετε ενδιαφέρον.");
            return false;
        }

        // Get professor data - now using the navigation property or foreign key
        var professor = professorEvent.Professor ?? await dbContext.Professors
            .FirstOrDefaultAsync(p => p.ProfEmail == professorEvent.ProfessorEmailUsedToUploadEvent);

        if (professor == null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Σφάλμα: Δεν βρέθηκε ο καθηγητής.");
            return false;
        }

        using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            // Create main interest record with details
            var interest = new InterestInProfessorEvent
            {
                RNGForProfessorEventInterest = professorEvent.RNGForEventUploadedAsProfessor,
                RNGForProfessorEventInterest_HashedAsUniqueID = professorEvent.RNGForEventUploadedAsProfessor_HashedAsUniqueID,
                DateTimeStudentShowInterest = DateTime.UtcNow,
                ProfessorEventStatusAtStudentSide = "Έχετε Δείξει Ενδιαφέρον",
                ProfessorEventStatusAtProfessorSide = "Προς Επεξεργασία",
                ProfessorEmailWhereStudentShowedInterest = professorEvent.ProfessorEmailUsedToUploadEvent, // Updated to use foreign key
                ProfessorUniqueIDWhereStudentShowedInterest = professor.Professor_UniqueID,
                StudentEmailShowInterestForEvent = student.Email,
                StudentUniqueIDShowInterestForEvent = student.Student_UniqueID,
                StudentTransportNeedWhenShowInterestForProfessorEvent = needsTransportForProfessorEvent ? "Ναι" : "Όχι",
                StudentTransportChosenLocationWhenShowInterestForProfessorEvent = chosenLocation,
    
                StudentDetails = new InterestInProfessorEvent_StudentDetails
                {
                    StudentUniqueIDShowInterestForProfessorEvent = student.Student_UniqueID,
                    StudentEmailShowInterestForProfessorEvent = student.Email,
                    DateTimeStudentShowInterestForProfessorEvent = DateTime.UtcNow,
                    RNGForProfessorEventShowInterestAsStudent_HashedAsUniqueID = professorEvent.RNGForEventUploadedAsProfessor_HashedAsUniqueID
                },
    
                ProfessorDetails = new InterestInProfessorEvent_ProfessorDetails
                {
                    ProfessorUniqueIDWhereStudentShowInterestForProfessorEvent = professor.Professor_UniqueID,
                    ProfessorEmailWhereStudentShowInterestForProfessorEvent = professorEvent.ProfessorEmailUsedToUploadEvent // Updated to use foreign key
                }
            };

            dbContext.InterestInProfessorEvents.Add(interest);

            // Add platform action
            dbContext.PlatformActions.Add(new PlatformActions
            {
                UserRole_PerformedAction = "STUDENT",
                ForWhat_PerformedAction = "PROFESSOR_EVENT",
                HashedPositionRNG_PerformedAction = HashingHelper.HashLong(professorEvent.RNGForEventUploadedAsProfessor),
                TypeOfAction_PerformedAction = "SHOW_INTEREST",
                DateTime_PerformedAction = DateTime.UtcNow
            });

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            // Send emails - updated to use professor navigation properties
            try
            {
                await InternshipEmailService.SendConfirmationToStudentForInterestInProfessorEvent(
                    student.Email,
                    student.Name,
                    student.Surname,
                    professorEvent.ProfessorEventTitle,
                    professor.ProfName,
                    professor.ProfSurname,
                    professorEvent.RNGForEventUploadedAsProfessor_HashedAsUniqueID,
                    needsTransportForProfessorEvent,
                    chosenLocation);

                await InternshipEmailService.SendNotificationToProfessorForStudentInterestForProfessorEvent(
                    professor.ProfEmail,
                    professor.ProfName,
                    professor.ProfSurname,
                    student.Name,
                    student.Surname,
                    student.Email,
                    student.Telephone,
                    student.StudyYear,
                    professorEvent.ProfessorEventTitle,
                    professorEvent.RNGForEventUploadedAsProfessor_HashedAsUniqueID,
                    needsTransportForProfessorEvent,
                    chosenLocation);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
            }

            await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Η έκφραση ενδιαφέροντος για την εκδήλωση {professorEvent.ProfessorEventTitle} υποβλήθηκε επιτυχώς!");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Full error: {ex}");
            await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Σφάλμα κατά την υποβολή: {ex.InnerException?.Message ?? ex.Message}");
            return false;
        }

        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        return false;
    }




    private async Task ShowAlert(string message)
    {
        await JS.InvokeVoidAsync("alert", message);
    }


    private async Task ShowProfessorDetailsFromHyperlinkName(string professorEmail)
    {
        try
        {
            selectedProfessorDetails = await dbContext.Professors
                .FirstOrDefaultAsync(p => p.ProfEmail == professorEmail);

            if (selectedProfessorDetails != null)
            {
                isProfessorDetailModalVisible = true;
            }
            else
            {
                Console.WriteLine("Professor not found");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching professor details: {ex.Message}");
        }
    }

    private void CloseProfessorModalFromHyperlinkName()
    {
        isProfessorDetailModalVisible = false;
        selectedProfessorDetails = null;
    }

    private string GetImageSource(byte[] imageBytes)
    {      
        return "data:image/png;base64," + Convert.ToBase64String(imageBytes);       
    }      

    private void SearchStudentsAsProfessorToFindStudent()
    {
        var query = dbContext.Students.AsQueryable();

        query = query.Where(s =>
            (string.IsNullOrEmpty(searchEmailAsProfessorToFindStudent) || s.Email.Contains(searchEmailAsProfessorToFindStudent)) &&
            (string.IsNullOrEmpty(searchNameAsProfessorToFindStudent) || s.Name.Contains(searchNameAsProfessorToFindStudent)) &&
            (string.IsNullOrEmpty(searchSurnameAsProfessorToFindStudent) || s.Surname.Contains(searchSurnameAsProfessorToFindStudent)) &&
            (string.IsNullOrEmpty(searchRegNumberAsProfessorToFindStudent) || s.RegNumber.ToString().Contains(searchRegNumberAsProfessorToFindStudent)) &&
            (string.IsNullOrEmpty(searchSchoolAsProfessorToFindStudent) || 
                universityDepartments.ContainsKey(searchSchoolAsProfessorToFindStudent) &&
                universityDepartments[searchSchoolAsProfessorToFindStudent].Contains(s.Department)) &&
            (string.IsNullOrEmpty(searchDepartmentAsProfessorToFindStudent) || s.Department.Contains(searchDepartmentAsProfessorToFindStudent))
        );

        if (!string.IsNullOrEmpty(searchAreasOfExpertiseAsProfessorToFindStudent))
        {
            query = query.Where(s => s.AreasOfExpertise.Contains(searchAreasOfExpertiseAsProfessorToFindStudent));
        }

        if (!string.IsNullOrEmpty(searchKeywordsAsProfessorToFindStudent))
        {
            query = query.Where(s => s.Keywords.Contains(searchKeywordsAsProfessorToFindStudent));
        }

        searchResultsAsProfessorToFindStudent = query.ToList();
    }


    private void ClearSearchFieldsAsProfessorToFindStudent()
    {
        // Clear all search fields
        searchEmailAsProfessorToFindStudent = string.Empty;
        searchNameAsProfessorToFindStudent = string.Empty;
        searchSurnameAsProfessorToFindStudent = string.Empty;
        searchRegNumberAsProfessorToFindStudent = string.Empty;
        searchSchoolAsProfessorToFindStudent = string.Empty;
        searchDepartmentAsProfessorToFindStudent = string.Empty;
        searchAreasOfExpertiseAsProfessorToFindStudent = string.Empty;  // Clear AreasOfExpertise search field
        searchKeywordsAsProfessorToFindStudent = string.Empty;  // Clear Keywords search field

        searchResultsAsProfessorToFindStudent = null;
    }


    private void SearchCompaniesAsProfessor()
    {
        var query = dbContext.Companies.AsQueryable();

        // Apply basic filters
        query = query.Where(c =>
            (string.IsNullOrEmpty(searchCompanyEmailAsProfessorToFindCompany) || c.CompanyEmail.Contains(searchCompanyEmailAsProfessorToFindCompany)) &&
            (string.IsNullOrEmpty(searchCompanyNameENGAsProfessorToFindCompany) || c.CompanyNameENG.Contains(searchCompanyNameENGAsProfessorToFindCompany)) &&
            (string.IsNullOrEmpty(searchCompanyTypeAsProfessorToFindCompany) || c.CompanyType.Contains(searchCompanyTypeAsProfessorToFindCompany)) &&
            (string.IsNullOrEmpty(searchCompanyActivityrAsProfessorToFindCompany) || c.CompanyActivity.Contains(searchCompanyActivityrAsProfessorToFindCompany)) &&
            (string.IsNullOrEmpty(searchCompanyTownAsProfessorToFindCompany) || c.CompanyTown.Contains(searchCompanyTownAsProfessorToFindCompany))
        );

        // Apply areas filter only if search term exists
        if (!string.IsNullOrEmpty(searchCompanyAreasAsProfessorToFindCompany))
        {
            query = query.Where(c => c.CompanyAreas.Contains(searchCompanyAreasAsProfessorToFindCompany));
        }

        // Apply skills filter only if search term exists
        if (!string.IsNullOrEmpty(searchCompanyDesiredSkillsAsProfessorToFindCompany))
        {
            query = query.Where(c => c.CompanyDesiredSkills.Contains(searchCompanyDesiredSkillsAsProfessorToFindCompany));
        }

        searchResultsAsProfessorToFindCompany = query.ToList();
    }

    private void ClearSearchFieldsAsProfessorToFindCompany()
    {
        // Clear all search fields
        searchCompanyEmailAsProfessorToFindCompany = string.Empty;
        searchCompanyNameENGAsProfessorToFindCompany = string.Empty;
        searchCompanyTypeAsProfessorToFindCompany = string.Empty;
        searchCompanyActivityrAsProfessorToFindCompany = string.Empty;
        searchCompanyTownAsProfessorToFindCompany = string.Empty;
        searchCompanyAreasAsProfessorToFindCompany = string.Empty;
        searchCompanyDesiredSkillsAsProfessorToFindCompany = string.Empty;

        // Clear search results
        searchResultsAsProfessorToFindCompany = null;
    }

    private void SearchCompaniesAsRG()
    {
        var query = dbContext.Companies.AsQueryable();

        // Apply basic filters
        query = query.Where(c =>
            (string.IsNullOrEmpty(searchCompanyEmailAsRGToFindCompany) || c.CompanyEmail.Contains(searchCompanyEmailAsRGToFindCompany)) &&
            (string.IsNullOrEmpty(searchCompanyNameENGAsRGToFindCompany) || c.CompanyNameENG.Contains(searchCompanyNameENGAsRGToFindCompany)) &&
            (string.IsNullOrEmpty(searchCompanyTypeAsRGToFindCompany) || c.CompanyType.Contains(searchCompanyTypeAsRGToFindCompany)) &&
            (string.IsNullOrEmpty(searchCompanyActivityrAsRGToFindCompany) || c.CompanyActivity.Contains(searchCompanyActivityrAsRGToFindCompany)) &&
            (string.IsNullOrEmpty(searchCompanyTownAsRGToFindCompany) || c.CompanyTown.Contains(searchCompanyTownAsRGToFindCompany))
        );

        // Apply areas filter only if search term exists
        if (!string.IsNullOrEmpty(searchCompanyAreasAsRGToFindCompany))
        {
            query = query.Where(c => c.CompanyAreas.Contains(searchCompanyAreasAsRGToFindCompany));
        }

        // Apply skills filter only if search term exists
        if (!string.IsNullOrEmpty(searchCompanyDesiredSkillsAsRGToFindCompany))
        {
            query = query.Where(c => c.CompanyDesiredSkills.Contains(searchCompanyDesiredSkillsAsRGToFindCompany));
        }

        searchResultsAsRGToFindCompany = query.ToList();
    }

    private void ClearSearchFieldsAsRGToFindCompany()
    {
        // Clear all search fields
        searchCompanyEmailAsRGToFindCompany = string.Empty;
        searchCompanyNameENGAsRGToFindCompany = string.Empty;
        searchCompanyTypeAsRGToFindCompany = string.Empty;
        searchCompanyActivityrAsRGToFindCompany = string.Empty;
        searchCompanyTownAsRGToFindCompany = string.Empty;
        searchCompanyAreasAsRGToFindCompany = string.Empty;
        searchCompanyDesiredSkillsAsRGToFindCompany = string.Empty;

        // Clear search results
        searchResultsAsRGToFindCompany = null;
    }

 /////////////////

    private void ShowStudentDetailsWhenSearchAsProfessor(Student student)
    {
        selectedStudent = student;
        showStudentDetailsModal = true;
    }

    private void CloseStudentDetailsModalWhenSearchAsProfessor()
    {
        showStudentDetailsModal = false;
        selectedStudent = null;
    }


    private void ShowCompanyDetailsWhenSearchAsProfessor(Company company)
    {
        selectedCompany = company;
        showCompanyDetailsModal = true;
    }

    private void ShowCompanyDetailsWhenSearchAsRG(Company company)
    {
        selectedCompany = company;
        showCompanyDetailsModal = true;
    }

    private void CloseCompanyDetailsModalWhenSearchAsProfessor()
    {
        showCompanyDetailsModal = false;
        selectedCompany = null;
    }

    private async Task ShowStudentDetailsInNameAsHyperlinkForProfessorThesis(string studentUniqueID)
    {
        try
        {
            // Fetch the student details using the unique ID from cache first
            if (!studentDataCache.TryGetValue(studentUniqueID, out currentStudentDetails))
            {
                // If not in cache, fetch from database
                currentStudentDetails = await dbContext.Students
                    .FirstOrDefaultAsync(s => s.Student_UniqueID == studentUniqueID);
            
                if (currentStudentDetails != null)
                {
                    // Add to cache
                    studentDataCache[studentUniqueID] = currentStudentDetails;
                }
            }

            if (currentStudentDetails != null)
            {
                isModalVisibleToShowStudentDetailsInNameAsHyperlinkForProfessorThesis = true;
            }
            else
            {
                await JS.InvokeVoidAsync("alert", "Δεν βρέθηκαν στοιχεία φοιτητή");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading student details: {ex.Message}");
            await JS.InvokeVoidAsync("alert", "Σφάλμα κατά την φόρτωση των στοιχείων του φοιτητή");
        }
    }

    private void UpdateOrganizerVisibility(bool value)
    {
        companyEvent.CompanyEventOtherOrganizerToBeVisible = value;
        StateHasChanged(); // Ensure the UI is updated
    }

    private void UpdateOrganizerVisibilityForProfessorEvents(bool value)
    {
        professorEvent.ProfessorEventOtherOrganizerToBeVisible = value;
        StateHasChanged(); // Ensure the UI is updated
    }

    private void ShowStudentDetailsOnEyeIconWhenSearchForStudentsAsCompany(Student student)
    {
        selectedStudentWhenSearchForStudentsAsCompany = student;
        showStudentDetailsModalWhenSearchForStudentsAsCompany = true;
    }

    private void CloseModalStudentDetailsOnEyeIconWhenSearchForStudentsAsCompany()
    {
        showStudentDetailsModalWhenSearchForStudentsAsCompany = false;
        selectedStudentWhenSearchForStudentsAsCompany = null;
    }

    private void SearchProfessorsAsCompanyToFindProfessor()
    {
        var professorsQuery = dbContext.Professors.AsQueryable();

        if (!string.IsNullOrEmpty(searchNameSurnameAsCompanyToFindProfessor))
        {
            professorsQuery = professorsQuery.Where(p =>
                (p.ProfName + " " + p.ProfSurname).Contains(searchNameSurnameAsCompanyToFindProfessor));
        }

        if (!string.IsNullOrEmpty(searchSchoolAsCompanyToFindProfessor))
        {
            // Filter by school - you might need to add a School property to your Professor model
            // If you don't have a School property, you can filter by departments that belong to the selected school
            var schoolDepartments = universityDepartments[searchSchoolAsCompanyToFindProfessor];
            professorsQuery = professorsQuery.Where(p => schoolDepartments.Contains(p.ProfDepartment));
        }

        if (!string.IsNullOrEmpty(searchDepartmentAsCompanyToFindProfessor))
        {
            professorsQuery = professorsQuery.Where(p => p.ProfDepartment == searchDepartmentAsCompanyToFindProfessor);
        }

        var professorsList = professorsQuery.ToList();

        searchResultsAsCompanyToFindProfessor = professorsList
            .Where(p =>
                string.IsNullOrEmpty(searchAreasOfInterestAsCompanyToFindProfessor) ||
                (!string.IsNullOrEmpty(p.ProfGeneralFieldOfWork) &&
                    (
                        selectedAreasOfInterest.Any(area => p.ProfGeneralFieldOfWork.Contains(area)) ||
                        p.ProfGeneralFieldOfWork.Contains(searchAreasOfInterestAsCompanyToFindProfessor)
                    )
                )
            )
            .ToList();
    }


    private void ClearSearchFieldsAsCompanyToFindProfessor()
    {
        searchNameSurnameAsCompanyToFindProfessor = string.Empty; 
        searchSchoolAsCompanyToFindProfessor = string.Empty;
        searchDepartmentAsCompanyToFindProfessor = string.Empty;   
        searchAreasOfInterestAsCompanyToFindProfessor = string.Empty; 
        searchResultsAsCompanyToFindProfessor = null; 
        areasOfInterestSuggestions.Clear();
        selectedAreasOfInterest.Clear(); 
    }




    private void ShowProfessorDetailsOnEyeIconWhenSearchForProfessorAsCompany(Professor professor)
    {
        selectedProfessorWhenSearchForProfessorsAsCompany = professor;
        showProfessorDetailsModalWhenSearchForProfessorsAsCompany = true;
    }

    private void CloseModalProfessorDetailsOnEyeIconWhenSearchForProfessorsAsCompany()
    {
        showProfessorDetailsModalWhenSearchForProfessorsAsCompany = false;
        selectedProfessorWhenSearchForProfessorsAsCompany = null;
    }

    private string SearchNameOrSurname
    {
        get => searchNameOrSurname;
        set
        {
            searchNameOrSurname = value;
            UpdateNameSurnameSuggestions(); 
        }
    }

    private void UpdateNameSurnameSuggestions()
    {
        if (!string.IsNullOrWhiteSpace(searchNameOrSurname) && searchNameOrSurname.Length > 1)
        {
            // Fetch suggestions dynamically
            nameSurnameSuggestions = dbContext.Students
                .Where(s => 
                    s.Name.Contains(searchNameOrSurname) || 
                    s.Surname.Contains(searchNameOrSurname))
                .Select(s => s.Name + " " + s.Surname) // Combine Name and Surname
                .Distinct()
                .Take(10) // Limit results
                .ToList();
        }
        else
        {
            // Clear suggestions when input is empty
            nameSurnameSuggestions.Clear();
        }
    }


    private void SelectNameSurnameSuggestion(string suggestion)
    {
        if (!string.IsNullOrWhiteSpace(suggestion))
        {
            searchNameOrSurname = suggestion; // Populate input with the selected suggestion
            nameSurnameSuggestions.Clear();  // Clear the suggestions list
        }
    }


    private async Task HandleInput(ChangeEventArgs e)
    {
        searchNameOrSurname = e.Value?.ToString().Trim() ?? string.Empty;

        if (searchNameOrSurname.Length >= 2)
        {
            try
            {
                nameSurnameSuggestions = await Task.Run(() =>
                    dbContext.Students
                        .Where(s => 
                            s.Name.Contains(searchNameOrSurname) || 
                            s.Surname.Contains(searchNameOrSurname))
                        .Select(s => s.Name + " " + s.Surname)
                        .Distinct()
                        .Take(10)
                        .ToList());
            }
            catch (Exception ex)
            {
                // Log or handle the error as appropriate
                Console.WriteLine($"Error fetching student suggestions: {ex.Message}");
            }
        }
        else
        {
            nameSurnameSuggestions.Clear();
        }

        StateHasChanged();
    }


    private void HandleProfessorInput(ChangeEventArgs e)
    {
        searchNameSurnameAsCompanyToFindProfessor = e.Value?.ToString();
        if (!string.IsNullOrWhiteSpace(searchNameSurnameAsCompanyToFindProfessor) && searchNameSurnameAsCompanyToFindProfessor.Length >= 2)
        {
            professorNameSurnameSuggestions = dbContext.Professors
                .Where(p => (p.ProfName + " " + p.ProfSurname).Contains(searchNameSurnameAsCompanyToFindProfessor))
                .Select(p => p.ProfName + " " + p.ProfSurname) // Concatenate Name and Surname
                .Distinct()
                .ToList();
        }
        else
        {
            professorNameSurnameSuggestions.Clear();
        }
    }

    private void HandleResearchGroupInput(ChangeEventArgs e)
    {
        searchResearchGroupNameAsCompanyToFindResearchGroup = e.Value?.ToString();
        if (!string.IsNullOrWhiteSpace(searchResearchGroupNameAsCompanyToFindResearchGroup) && searchResearchGroupNameAsCompanyToFindResearchGroup.Length >= 2)
        {
            researchgroupNameSuggestions = dbContext.ResearchGroups
                .Where(p => (p.ResearchGroupName).Contains(searchResearchGroupNameAsCompanyToFindResearchGroup))
                .Select(p => p.ResearchGroupName) 
                .Distinct()
                .ToList();
        }
        else
        {
            researchgroupNameSuggestions.Clear();
        }
    }

////////////////////////////////////////////////////////////////////////////////////////////////////
    private void HandleProfessorInputWhenSearchForProfessorAsRG(ChangeEventArgs e)
    {
        searchNameSurnameAsRGToFindProfessor = e.Value?.ToString();
        if (!string.IsNullOrWhiteSpace(searchNameSurnameAsRGToFindProfessor) && searchNameSurnameAsRGToFindProfessor.Length >= 2)
        {
            professorNameSurnameSuggestionsAsRG = dbContext.Professors
                .Where(p => (p.ProfName + " " + p.ProfSurname).Contains(searchNameSurnameAsRGToFindProfessor))
                .Select(p => p.ProfName + " " + p.ProfSurname) // Concatenate Name and Surname
                .Distinct()
                .ToList();
        }
        else
        {
            professorNameSurnameSuggestionsAsRG.Clear();
        }
    }

    private void SelectProfessorNameSurnameSuggestionAsRG(string suggestion)
    {
        searchNameSurnameAsRGToFindProfessor = suggestion;
        professorNameSurnameSuggestionsAsRG.Clear();
    }

    private async Task HandleAreasOfInterestInputAsRG(ChangeEventArgs e)
    {
        searchAreasOfInterestAsRG = e.Value?.ToString().Trim() ?? string.Empty;

        // Ensure areasOfInterestSuggestions is never null
        areasOfInterestSuggestionsAsRG = new List<string>();

        if (searchAreasOfInterestAsRG.Length >= 1)
        {
            try
            {
                // Fetch suggestions for Areas of Interest with 1+ characters
                areasOfInterestSuggestionsAsRG = await Task.Run(() =>
                    dbContext.Areas
                        .Where(a => a.AreaName.Contains(searchAreasOfInterestAsRG)) // Assuming the entity has InterestName
                        .Select(a => a.AreaName) // Extract only the InterestName
                        .Distinct()
                        .Take(10) // Limit suggestions to 10
                        .ToList());
            }
            catch (Exception ex)
            {
                // Log the error for debugging purposes
                Console.WriteLine($"Πρόβλημα στην Ανάκτηση Περιοχών Ενδιαφέροντος από την Βάση: {ex.Message}");
                areasOfInterestSuggestionsAsRG = new List<string>();  // Fallback to empty list
            }
        }
        else
        {
            areasOfInterestSuggestions.Clear(); // Clear suggestions for fewer than 1 character
        }

        // Trigger UI refresh
        StateHasChanged();
    }

    private void SelectAreasOfInterestSuggestionAsRG(string suggestion)
    {
        if (!string.IsNullOrWhiteSpace(suggestion) && !selectedAreasOfInterestAsRG.Contains(suggestion))
        {
            selectedAreasOfInterestAsRG.Add(suggestion);
            areasOfInterestSuggestionsAsRG.Clear(); // Clear suggestions
            searchAreasOfInterestAsRG = string.Empty; // Clear input field
        }
    }

    private void RemoveSelectedAreaOfInterestAsRG(string area)
    {
        selectedAreasOfInterestAsRG.Remove(area); // Remove area
        StateHasChanged(); // Refresh UI
    }

    private void SearchProfessorsAsRGToFindProfessor()
    {
        var professorsQuery = dbContext.Professors.AsQueryable();

        if (!string.IsNullOrEmpty(searchNameSurnameAsRGToFindProfessor))
        {
            professorsQuery = professorsQuery.Where(p =>
                (p.ProfName + " " + p.ProfSurname).Contains(searchNameSurnameAsRGToFindProfessor));
        }

        if (!string.IsNullOrEmpty(searchSchoolAsRGToFindProfessor))
        {
            // Filter by departments that belong to the selected school
            var schoolDepartments = universityDepartments[searchSchoolAsRGToFindProfessor];
            professorsQuery = professorsQuery.Where(p => schoolDepartments.Contains(p.ProfDepartment));
        }

        if (!string.IsNullOrEmpty(searchDepartmentAsRGToFindProfessor))
        {
            professorsQuery = professorsQuery.Where(p => p.ProfDepartment == searchDepartmentAsRGToFindProfessor);
        }

        var professorsList = professorsQuery.ToList();

        searchResultsAsRGToFindProfessor = professorsList
            .Where(p =>
                string.IsNullOrEmpty(searchAreasOfInterestAsRGToFindProfessor) ||
                (!string.IsNullOrEmpty(p.ProfGeneralFieldOfWork) &&
                    (
                        selectedAreasOfInterestAsRG.Any(area => p.ProfGeneralFieldOfWork.Contains(area)) ||
                        p.ProfGeneralFieldOfWork.Contains(searchAreasOfInterestAsRGToFindProfessor)
                    )
                )
            )
            .ToList();
    }

    private void ClearSearchFieldsAsRGToFindProfessor()
    {
        searchNameSurnameAsRGToFindProfessor = string.Empty; 
        searchSchoolAsRGToFindProfessor = string.Empty;
        searchDepartmentAsRGToFindProfessor = string.Empty;   
        searchAreasOfInterestAsRGToFindProfessor = string.Empty; 
        searchResultsAsRGToFindProfessor = null; 
        areasOfInterestSuggestionsAsRG.Clear();
        selectedAreasOfInterestAsRG.Clear(); 
    }


    private int ProfessorsPerPage_SearchForProfessorsAsRG = 3; // Default value
    private int currentProfessorPage_SearchForProfessorsAsRG = 1;
    private void OnPageSizeChange_SearchForProfessorsAsRG(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            ProfessorsPerPage_SearchForProfessorsAsRG = newSize;
            currentProfessorPage_SearchForProfessorsAsRG = 1;
            StateHasChanged();
        }
    }

    private IEnumerable<Professor> GetPaginatedProfessorResultsAsRG()
    {
        return searchResultsAsRGToFindProfessor?
            .Skip((currentProfessorPage_SearchForProfessorsAsRG - 1) * ProfessorsPerPage_SearchForProfessorsAsRG)
            .Take(ProfessorsPerPage_SearchForProfessorsAsRG) 
            ?? Enumerable.Empty<Professor>();
    }

    private void ShowProfessorDetailsOnEyeIconWhenSearchForProfessorAsRG(Professor professor)
    {
        selectedProfessorWhenSearchForProfessorsAsRG = professor;
        showProfessorDetailsModalWhenSearchForProfessorsAsRG = true;
    }

    private void GoToFirstProfessorPageAsRG()
    {
        currentProfessorPage_SearchForProfessorsAsRG = 1;
        StateHasChanged();
    }

    private int totalProfessorPages_SearchForProfessorsAsRG => searchResultsAsRGToFindProfessor != null 
            ? (int)Math.Ceiling((double)searchResultsAsRGToFindProfessor.Count / ProfessorsPerPage_SearchForProfessorsAsRG) 
            : 1;
    private List<int> GetVisibleProfessorPagesAsRG()
    {
        var pagesAsRG = new List<int>();
        int currentPageAsRG = currentProfessorPage_SearchForProfessorsAsRG;
        int totalPagesAsRG = totalProfessorPages_SearchForProfessorsAsRG;
    
        // Always show first page
        pagesAsRG.Add(1);
    
        // Show pages around current page
        if (currentPageAsRG > 3) pagesAsRG.Add(-1); // Ellipsis
    
        int start = Math.Max(2, currentPageAsRG - 1);
        int end = Math.Min(totalPages - 1, currentPageAsRG + 1);
    
        for (int i = start; i <= end; i++)
        {
            pagesAsRG.Add(i);
        }
    
        if (currentPageAsRG < totalPagesAsRG - 2) pagesAsRG.Add(-1); // Ellipsis
    
        // Always show last page if different from first
        if (totalPagesAsRG > 1) pagesAsRG.Add(totalPagesAsRG);
    
        return pagesAsRG;
    }

    private void GoToProfessorPageAsRG(int pageNumberAsRG)
    {
        if (pageNumberAsRG >= 1 && pageNumberAsRG <= totalProfessorPages_SearchForProfessorsAsRG)
        {
            currentProfessorPage_SearchForProfessorsAsRG = pageNumberAsRG;
            StateHasChanged();
        }
    }

    private void PreviousProfessorPageAsRG()
    {
        if (currentProfessorPage_SearchForProfessorsAsRG > 1)
        {
            currentProfessorPage_SearchForProfessorsAsRG--;
            StateHasChanged();
        }
    }

    private void NextProfessorPageAsRG()
    {
        if (currentProfessorPage_SearchForProfessorsAsRG < totalProfessorPages_SearchForProfessorsAsRG)
        {
            currentProfessorPage_SearchForProfessorsAsRG++;
            StateHasChanged();
        }
    }

    private void GoToLastProfessorPageAsRG()
    {
        currentProfessorPage_SearchForProfessorsAsRG = totalProfessorPages_SearchForProfessorsAsRG;
        StateHasChanged();
    }


    private void CloseModalProfessorDetailsOnEyeIconWhenSearchForProfessorsAsRG()
    {
        showProfessorDetailsModalWhenSearchForProfessorsAsRG = false;
        selectedProfessorWhenSearchForProfessorsAsRG = null;
    }
////////////////////////////////////////////////////////////////////////////////////////////////////

        private void HandleProfessorInputWhenSearchForProfessorThesisAutocompleteNameAsStudent(ChangeEventArgs e)
        {
            searchNameSurnameAsStudentToFindProfessor = e.Value?.ToString();
            if (!string.IsNullOrWhiteSpace(searchNameSurnameAsStudentToFindProfessor) && searchNameSurnameAsStudentToFindProfessor.Length >= 2)
            {
                professorNameSurnameSuggestions = dbContext.Professors
                    .Where(p => (p.ProfName + " " + p.ProfSurname).Contains(searchNameSurnameAsStudentToFindProfessor))
                    .Select(p => p.ProfName + " " + p.ProfSurname) // Concatenate Name and Surname
                    .Distinct()
                    .ToList();
            }
            else
            {
                professorNameSurnameSuggestions.Clear();
            }
        }

    // Select a suggestion for name and surname
    private void SelectProfessorNameSurnameSuggestionWhenSearchForProfessorThesisAutocompleteNameAsStudent(string suggestion)
    {
        searchNameSurnameAsStudentToFindProfessor = suggestion;
        professorNameSurnameSuggestions.Clear();
    }

    private void SelectProfessorNameSurnameSuggestion(string suggestion)
    {
        searchNameSurnameAsCompanyToFindProfessor = suggestion;
        professorNameSurnameSuggestions.Clear();
    }



    private async Task HandleAreasOfExpertiseInput(ChangeEventArgs e)
    {
        searchAreasOfExpertise = e.Value?.ToString().Trim() ?? string.Empty;

        // Ensure areasOfExpertiseSuggestions is never null
        areasOfExpertiseSuggestions = new List<string>();

        if (searchAreasOfExpertise.Length >= 1)
        {
            try
            {
                // Fetch suggestions for Areas of Expertise with 1+ characters
                areasOfExpertiseSuggestions = await Task.Run(() =>
                    dbContext.Areas
                        .Where(a => a.AreaName.Contains(searchAreasOfExpertise))
                        .Select(a => a.AreaName) // Extract only the AreaName
                        .Distinct()
                        .Take(10) // Limit suggestions to 10
                        .ToList()); // No need for ?? here, since ToList() will always return a List<string>
            }
            catch (Exception ex)
            {
                // Log the error for debugging purposes
                Console.WriteLine($"Πρόβλημα στην Ανάκτηση Περιοχών Εξειδίκευσης από την Βάση: {ex.Message}");
                areasOfExpertiseSuggestions = new List<string>();  // Fallback to empty list
            }
        }
        else
        {
            areasOfExpertiseSuggestions.Clear(); // Clear suggestions for fewer than 1 character
        }

        // Trigger UI refresh
        StateHasChanged();
    }

    private async Task HandleAreasOfInterestInput(ChangeEventArgs e)
    {
        searchAreasOfInterest = e.Value?.ToString().Trim() ?? string.Empty;

        // Ensure areasOfInterestSuggestions is never null
        areasOfInterestSuggestions = new List<string>();

        if (searchAreasOfInterest.Length >= 1)
        {
            try
            {
                // Fetch suggestions for Areas of Interest with 1+ characters
                areasOfInterestSuggestions = await Task.Run(() =>
                    dbContext.Areas
                        .Where(a => a.AreaName.Contains(searchAreasOfInterest)) // Assuming the entity has InterestName
                        .Select(a => a.AreaName) // Extract only the InterestName
                        .Distinct()
                        .Take(10) // Limit suggestions to 10
                        .ToList());
            }
            catch (Exception ex)
            {
                // Log the error for debugging purposes
                Console.WriteLine($"Πρόβλημα στην Ανάκτηση Περιοχών Ενδιαφέροντος από την Βάση: {ex.Message}");
                areasOfInterestSuggestions = new List<string>();  // Fallback to empty list
            }
        }
        else
        {
            areasOfInterestSuggestions.Clear(); // Clear suggestions for fewer than 1 character
        }

        // Trigger UI refresh
        StateHasChanged();
    }



    private void SelectAreasOfExpertiseSuggestion(string suggestion)
    {
        if (!string.IsNullOrWhiteSpace(suggestion) && !selectedAreasOfExpertise.Contains(suggestion))
        {
            selectedAreasOfExpertise.Add(suggestion);
            areasOfExpertiseSuggestions.Clear(); // Clear suggestions
            searchAreasOfExpertise = string.Empty; // Clear input field
        }
    }

    private void SelectAreasOfInterestSuggestion(string suggestion)
    {
        if (!string.IsNullOrWhiteSpace(suggestion) && !selectedAreasOfInterest.Contains(suggestion))
        {
            selectedAreasOfInterest.Add(suggestion);
            areasOfInterestSuggestions.Clear(); // Clear suggestions
            searchAreasOfInterest = string.Empty; // Clear input field
        }
    }

    private void RemoveSelectedAreaOfExpertise(string area)
    {
        selectedAreasOfExpertise.Remove(area); // Remove area
        StateHasChanged(); // Refresh UI
    }

    private void RemoveSelectedAreaOfInterest(string area)
    {
        selectedAreasOfInterest.Remove(area); // Remove area
        StateHasChanged(); // Refresh UI
    }


    private async Task HandleKeywordsInput(ChangeEventArgs e)
    {
        searchKeywords = e.Value?.ToString().Trim() ?? string.Empty;

        // Ensure keywordsSuggestions is never null
        keywordsSuggestions = new List<string>();

        if (searchKeywords.Length >= 1)
        {
            try
            {
                // Fetch suggestions for Keywords/Skills with 1+ characters
                keywordsSuggestions = await Task.Run(() =>
                    dbContext.Skills
                        .Where(k => k.SkillName.Contains(searchKeywords))
                        .Select(k => k.SkillName)
                        .Distinct()
                        .Take(10) // Limit suggestions to 10
                        .ToList() ?? new List<string>()); // Ensure empty list if null
            }
            catch (Exception ex)
            {
                // Log the error for debugging purposes
                Console.WriteLine($"Πρόβλημα στην Ανάκτηση Ικανοτήτων από την Βάση: {ex.Message}");
                keywordsSuggestions = new List<string>();  // Fallback to empty list
            }
        }
        else
        {
            keywordsSuggestions.Clear();  // Clear suggestions for fewer than 1 character
        }

        // Trigger UI refresh
        StateHasChanged();
    }

    private void SelectKeywordsSuggestion(string suggestion)
    {
        if (!string.IsNullOrWhiteSpace(suggestion) && !selectedKeywords.Contains(suggestion))
        {
            selectedKeywords.Add(suggestion);  // Add to selected keywords
            searchKeywords = string.Empty;    // Clear input field
            keywordsSuggestions.Clear();      // Clear the suggestions list
        }
    }

    private void RemoveKeyword(string keyword)
    {
        if (selectedKeywords.Contains(keyword))
        {
            selectedKeywords.Remove(keyword);  // Remove from selected keywords
        }
    }



    private async Task DownloadStudentAttachmentAsCompanyInSearchForStudents(long studentId)
    {
        var student = await dbContext.Students
            .Where(s => s.Id == (int)studentId)
            .FirstOrDefaultAsync();

        if (student?.Attachment != null)
        {
            string fileName = $"{student.Name}_{student.Surname}_CV.pdf";
            string mimeType = "application/pdf";
            string base64Data = Convert.ToBase64String(student.Attachment);
            await JS.InvokeVoidAsync("downloadFile", fileName, mimeType, base64Data);
        }
    }

    private void LoadEventsForCalendar()
    {
        eventsForDate.Clear();
        eventsForDateForProfessors.Clear();
        int currentYear = currentMonth.Year;
        int currentMonthNumber = currentMonth.Month;

        // Loop through the events for the current month
        foreach (var eventItem in CompanyEventsToShowAtFrontPage)
        {
            if (eventItem.CompanyEventActiveDate.Year == currentYear &&
                eventItem.CompanyEventActiveDate.Month == currentMonthNumber)
            {
                int eventDay = eventItem.CompanyEventActiveDate.Day;
                if (!eventsForDate.ContainsKey(eventDay))
                {
                    eventsForDate[eventDay] = new List<CompanyEvent>();
                }
                eventsForDate[eventDay].Add(eventItem);
            }
        }


        //mpike gia ta professor events 22/1
        foreach (var eventProfessorItem in ProfessorEventsToShowAtFrontPage)
        {
            if (eventProfessorItem.ProfessorEventActiveDate.Year == currentYear &&
                eventProfessorItem.ProfessorEventActiveDate.Month == currentMonthNumber)
            {
                int eventDay = eventProfessorItem.ProfessorEventActiveDate.Day;
                if (!eventsForDateForProfessors.ContainsKey(eventDay))
                {
                    eventsForDateForProfessors[eventDay] = new List<ProfessorEvent>();
                }
                eventsForDateForProfessors[eventDay].Add(eventProfessorItem);
            }
        }

        // If highlighted day is not valid for this month, reset it
        if (highlightedDay != 0 && !eventsForDate.ContainsKey(highlightedDay))
        {
            highlightedDay = 0; // Reset it if there's no event for the day in the current month
        }

        // After loading events, ensure the selected and highlighted day is respected
        if (selectedDay != 0 && eventsForDate.ContainsKey(selectedDay))
        {
            highlightedDay = selectedDay; // Keep the selected day highlighted if valid
        }


        // GIA PROFESSORS EVENTS
        if (highlightedDay != 0 && !eventsForDateForProfessors.ContainsKey(highlightedDay))
        {
            highlightedDay = 0; // Reset it if there's no event for the day in the current month
        }

        // GIA PROFESSORS EVENTS
        if (selectedDay != 0 && eventsForDateForProfessors.ContainsKey(selectedDay))
        {
            highlightedDay = selectedDay; // Keep the selected day highlighted if valid
        }

        StateHasChanged();
    }



    private void ShowPreviousMonth()
    {
        currentMonth = currentMonth.AddMonths(-1);
        LoadEventsForCalendar();
        CalculateRemainingCells();  // Recalculate when changing the month
        StateHasChanged();

    }

    private void ShowNextMonth()
    {
        currentMonth = currentMonth.AddMonths(1);
        LoadEventsForCalendar();
        CalculateRemainingCells();  
        StateHasChanged();

    }

    private void OnDateClicked(DateTime clickedDate)
    {
        selectedDay = clickedDate.Day;  
        highlightedDay = selectedDay;
        selectedDate = clickedDate; // Make sure to set this for the modal display

        // Filter company events by status "Δημοσιευμένη" AND the specific date
        selectedDateEvents = dbContext.CompanyEvents
            .Include(e => e.Company)
            .Where(e => e.CompanyEventStatus == "Δημοσιευμένη" && 
                       e.CompanyEventActiveDate.Date == clickedDate.Date)
            .ToList();

        // Filter professor events by status "Δημοσιευμένη" AND the specific date
        selectedProfessorDateEvents = dbContext.ProfessorEvents
            .Include(e => e.Professor)
            .Where(e => e.ProfessorEventStatus == "Δημοσιευμένη" && 
                       e.ProfessorEventActiveDate.Date == clickedDate.Date)
            .ToList();

        // Only show modal if there are published events for this specific date
        if (selectedDateEvents.Any() || selectedProfessorDateEvents.Any())
        {
            isModalVisibleToShowEventsOnCalendarForEachClickedDay = true;
        }
        else
        {
            // Optional: Show a message that no events exist for this date
            // You could set a flag to display a message in your modal
        }
    
        StateHasChanged();
    }



    private void CloseModalShowingTheEventsOnCalendar()
    {
        isModalVisibleToShowEventsOnCalendarForEachClickedDay = false;
        selectedDateEvents.Clear();
        selectedProfessorDateEvents.Clear();


        // Re-render the calendar and highlight the selected day
        LoadEventsForCalendar();  // Reload events for the current month
        StateHasChanged();
    }



    private void ShowEventsForDate(List<CompanyEvent> events, List<ProfessorEvent> professorevents)
    {
        selectedDateEvents = events;
        selectedProfessorDateEvents = professorevents;

        isModalVisibleToShowEventsOnCalendarForEachClickedDay = true;
        StateHasChanged();

    }

    private void CalculateRemainingCells()
    {
        remainingCellsValue = totalCellsInGrid - (firstDayOfMonth + daysInCurrentMonth);
    }

    private void ChangeMonth(int monthChange)
    {
        currentMonth = currentMonth.AddMonths(monthChange);

        // Reset the highlighted day if it's invalid for the new month
        LoadEventsForCalendar();  // This will handle the resetting of highlightedDay
    }

    private async Task<List<CompanyEvent>> FetchCompanyEventsAsync()
    {
        var companyevents = await dbContext.CompanyEvents.AsNoTracking().ToListAsync(); 
        return companyevents;
    }

    private async Task<List<ProfessorEvent>> FetchProfessorEventsAsync()
    {
        var professorevents = await dbContext.ProfessorEvents.AsNoTracking().ToListAsync(); 
        return professorevents;
    }

    private async Task ShowInterestedStudentsInCompanyEvent(long eventRNG)
    {
        // Toggle the selectedEventIdForStudents to either show or hide the table
        if (selectedEventIdForStudents == eventRNG)
        {
            // Close the table by clearing the InterestedStudents
            selectedEventIdForStudents = null;
            InterestedStudents.Clear();
        }
        else
        {
            // Show the table and fetch the interested students for the selected event
            selectedEventIdForStudents = eventRNG;

            // Fetch the interested students with their details
            var interestedStudentsWithDetails = await dbContext.InterestInCompanyEvents
                .Include(x => x.StudentDetails)
                .Where(x => x.RNGForCompanyEventInterest == eventRNG)
                .Select(x => new 
                {
                    Application = x,
                    Student = dbContext.Students.FirstOrDefault(s => s.Student_UniqueID == x.StudentUniqueIDShowInterestForEvent)
                })
                .ToListAsync();

            // Convert to the expected type if needed
            InterestedStudents = interestedStudentsWithDetails.Select(x => x.Application).ToList();

            // ✅ Load full student data into cache (as in Job logic)
            var studentEmails = interestedStudentsWithDetails
                .Where(x => x.Student != null)
                .Select(x => x.Student.Email.ToLower())
                .Distinct()
                .ToList();

            var students = await dbContext.Students
                .Where(s => studentEmails.Contains(s.Email.ToLower()))
                .Select(s => new Student
                {
                    Id = s.Id,
                    Student_UniqueID = s.Student_UniqueID,
                    Email = s.Email,
                    Image = s.Image,
                    Name = s.Name,
                    Surname = s.Surname,
                    Telephone = s.Telephone,
                    PermanentAddress = s.PermanentAddress,
                    PermanentRegion = s.PermanentRegion,
                    PermanentTown = s.PermanentTown,
                    Attachment = s.Attachment,
                    LinkedInProfile = s.LinkedInProfile,
                    PersonalWebsite = s.PersonalWebsite,
                    Transport = s.Transport,
                    RegNumber = s.RegNumber,
                    University = s.University,
                    Department = s.Department,
                    EnrollmentDate = s.EnrollmentDate,
                    StudyYear = s.StudyYear,
                    LevelOfDegree = s.LevelOfDegree,
                    AreasOfExpertise = s.AreasOfExpertise,
                    Keywords = s.Keywords,
                    ExpectedGraduationDate = s.ExpectedGraduationDate,
                    CompletedECTS = s.CompletedECTS
                })
                .AsNoTracking()
                .ToListAsync();

            studentDataCache.Clear();
            foreach (var student in students)
            {
                studentDataCache[student.Email.ToLower()] = student;
            }

            Console.WriteLine($"Fetched {InterestedStudents.Count} students for Event RNG: {eventRNG}");
            Console.WriteLine($"Loaded {students.Count} student records into cache.");
        }

        StateHasChanged(); // Refresh the UI
    }


    private long? selectedEventIdForProfessorsWhenShowInterestForCompanyEvent;
    private async Task ShowInterestedProfessorsInCompanyEvent(long companyeventRNG)
    {
        if (selectedEventIdForProfessors == companyeventRNG)
        {
            // Close the table
            selectedEventIdForProfessors = null;
            filteredProfessorInterestForCompanyEvents.Clear();
        }
        else
        {
            // Show the table
            selectedEventIdForProfessors = companyeventRNG;
    
            // Load interests with professor details
            filteredProfessorInterestForCompanyEvents = await dbContext.InterestInCompanyEventsAsProfessor
                .Include(i => i.ProfessorDetails)
                .Where(x => x.RNGForCompanyEventInterestAsProfessor == companyeventRNG)
                .OrderByDescending(x => x.DateTimeProfessorShowInterestForCompanyEvent)
                .AsNoTracking()
                .ToListAsync();

            // Get distinct professor emails not already in cache
            var emailsToFetch = filteredProfessorInterestForCompanyEvents
                .Select(i => i.ProfessorEmailShowInterestForCompanyEvent)
                .Distinct()
                .Where(email => !professorDataCache.ContainsKey(email))
                .ToList();

            // Fetch all needed professors in one query
            if (emailsToFetch.Any())
            {
                var professors = await dbContext.Professors
                    .Where(p => emailsToFetch.Contains(p.ProfEmail))
                    .AsNoTracking()
                    .ToListAsync();

                // Add fetched professors to cache
                foreach (var professor in professors)
                {
                    professorDataCache[professor.ProfEmail] = professor;
                }
            }
        }
        StateHasChanged();
    }

    private async Task ShowInterestedStudentsInProfessorEvent(long professoreventRNG)
    {
        // Toggle the selectedEventIdForStudents to either show or hide the table
        if (selectedEventIdForStudentsWhenShowInterestForProfessorEvent == professoreventRNG)
        {
            // Close the table by clearing the InterestedStudents
            selectedEventIdForStudentsWhenShowInterestForProfessorEvent = null;
            InterestedStudentsForProfessorEvent.Clear();
            studentDataCache.Clear();
        }
        else
        {
            // Show the table and fetch the interested students for the selected event
            selectedEventIdForStudentsWhenShowInterestForProfessorEvent = professoreventRNG;

            // Fetch the interested students with their details
            var interestedStudentsWithDetails = await dbContext.InterestInProfessorEvents
                .Include(x => x.StudentDetails)
                .Where(x => x.RNGForProfessorEventInterest == professoreventRNG)
                .Select(x => new 
                {
                    Application = x,
                    Student = dbContext.Students.FirstOrDefault(s => s.Student_UniqueID == x.StudentUniqueIDShowInterestForEvent)
                })
                .ToListAsync();

            // Convert to the expected type if needed
            InterestedStudentsForProfessorEvent = interestedStudentsWithDetails.Select(x => x.Application).ToList();

            // Load full student data into cache
            var studentEmails = interestedStudentsWithDetails
                .Where(x => x.Student != null)
                .Select(x => x.Student.Email.ToLower())
                .Distinct()
                .ToList();

            var students = await dbContext.Students
                .Where(s => studentEmails.Contains(s.Email.ToLower()))
                .Select(s => new Student
                {
                    Id = s.Id,
                    Student_UniqueID = s.Student_UniqueID,
                    Email = s.Email,
                    Image = s.Image,
                    Name = s.Name,
                    Surname = s.Surname,
                    Telephone = s.Telephone,
                    PermanentAddress = s.PermanentAddress,
                    PermanentRegion = s.PermanentRegion,
                    PermanentTown = s.PermanentTown,
                    Attachment = s.Attachment,
                    LinkedInProfile = s.LinkedInProfile,
                    PersonalWebsite = s.PersonalWebsite,
                    Transport = s.Transport,
                    RegNumber = s.RegNumber,
                    University = s.University,
                    Department = s.Department,
                    EnrollmentDate = s.EnrollmentDate,
                    StudyYear = s.StudyYear,
                    LevelOfDegree = s.LevelOfDegree,
                    AreasOfExpertise = s.AreasOfExpertise,
                    Keywords = s.Keywords,
                    ExpectedGraduationDate = s.ExpectedGraduationDate,
                    CompletedECTS = s.CompletedECTS
                })
                .AsNoTracking()
                .ToListAsync();

            studentDataCache.Clear();
            foreach (var student in students)
            {
                studentDataCache[student.Email.ToLower()] = student;
            }

            Console.WriteLine($"Fetched {InterestedStudentsForProfessorEvent.Count} students for Event RNG: {professoreventRNG}");
            Console.WriteLine($"Loaded {students.Count} student records into cache.");
        }

        StateHasChanged(); // Refresh the UI
    }

    private async Task ShowStudentDetailsAtCompanyEventInterest(InterestInCompanyEvent application)
    {
        // === STUDENT LOOKUP ===
        var studentUniqueId = application.StudentUniqueIDShowInterestForEvent;
        selectedStudentFromCache = studentDataCache.Values
            .FirstOrDefault(s => s.Student_UniqueID == studentUniqueId);

        if (selectedStudentFromCache == null)
        {
            Console.WriteLine($"Student with ID {studentUniqueId} not found in cache - loading from DB");

            selectedStudentFromCache = await dbContext.Students
                .Where(s => s.Student_UniqueID == studentUniqueId)
                .Select(s => new Student {
                    Id = s.Id,
                    Student_UniqueID = s.Student_UniqueID,
                    Email = s.Email,
                    Image = s.Image,
                    Name = s.Name,
                    Surname = s.Surname,
                    Telephone = s.Telephone,
                    PermanentAddress = s.PermanentAddress,
                    PermanentRegion = s.PermanentRegion,
                    PermanentTown = s.PermanentTown,
                    Attachment = s.Attachment,
                    LinkedInProfile = s.LinkedInProfile,
                    PersonalWebsite = s.PersonalWebsite,
                    Transport = s.Transport,
                    RegNumber = s.RegNumber,
                    University = s.University,
                    Department = s.Department,
                    EnrollmentDate = s.EnrollmentDate,
                    StudyYear = s.StudyYear,
                    LevelOfDegree = s.LevelOfDegree,
                    AreasOfExpertise = s.AreasOfExpertise,
                    Keywords = s.Keywords
                })
                .FirstOrDefaultAsync();

            if (selectedStudentFromCache != null)
            {
                studentDataCache[selectedStudentFromCache.Email.ToLower()] = selectedStudentFromCache;
            }
        }

        // === COMPANY LOOKUP ===
        var companyUniqueId = application.CompanyUniqueIDWhereStudentShowedInterest;
        Company selectedCompanyFromCache = null;

        if (!string.IsNullOrEmpty(companyUniqueId))
        {
            companyDataCache.TryGetValue(companyUniqueId, out selectedCompanyFromCache);

            if (selectedCompanyFromCache == null)
            {
                Console.WriteLine($"Company with ID {companyUniqueId} not found in cache - loading from DB");

                selectedCompanyFromCache = await dbContext.Companies
                    .Where(c => c.Company_UniqueID == companyUniqueId)
                    .FirstOrDefaultAsync();

                if (selectedCompanyFromCache != null)
                {
                    companyDataCache[companyUniqueId] = selectedCompanyFromCache;
                }
            }
        }

        // === ASSIGN FINAL COMPOSITE OBJECT ===
        selectedStudentToShowDetailsForInterestinCompanyEvent = new InterestInCompanyEvent
        {
            Id = application.Id,
            CompanyEmailWhereStudentShowedInterest = application.CompanyEmailWhereStudentShowedInterest,
            CompanyUniqueIDWhereStudentShowedInterest = application.CompanyUniqueIDWhereStudentShowedInterest,
            StudentEmailShowInterestForEvent = application.StudentEmailShowInterestForEvent,
            StudentUniqueIDShowInterestForEvent = application.StudentUniqueIDShowInterestForEvent,
            RNGForCompanyEventInterest = application.RNGForCompanyEventInterest,
            RNGForCompanyEventInterest_HashedAsUniqueID = application.RNGForCompanyEventInterest_HashedAsUniqueID,
            DateTimeStudentShowInterest = application.DateTimeStudentShowInterest,
            StudentTransportNeedWhenShowInterestForCompanyEvent = application.StudentTransportNeedWhenShowInterestForCompanyEvent,
            StudentTransportChosenLocationWhenShowInterestForCompanyEvent = application.StudentTransportChosenLocationWhenShowInterestForCompanyEvent,
            CompanyEventStatusAtStudentSide = application.CompanyEventStatusAtStudentSide,
            CompanyEventStatusAtCompanySide = application.CompanyEventStatusAtCompanySide,

            StudentDetails = new InterestInCompanyEvent_StudentDetails
            {
                StudentEmailShowInterestForCompanyEvent = selectedStudentFromCache?.Email,
                StudentUniqueIDShowInterestForCompanyEvent = selectedStudentFromCache?.Student_UniqueID,
                DateTimeStudentShowInterestForCompanyEvent = application.DateTimeStudentShowInterest,
                RNGForCompanyEventShowInterestAsStudent_HashedAsUniqueID = application.RNGForCompanyEventInterest_HashedAsUniqueID
            },

            CompanyDetails = new InterestInCompanyEvent_CompanyDetails
            {
                CompanyEmailWhereStudentShowInterestForCompanyEvent = selectedCompanyFromCache?.CompanyEmail,
                CompanyUniqueIDWhereStudentShowInterestForCompanyEvent = selectedCompanyFromCache?.Company_UniqueID
            }
        };

        showModal = true;
        StateHasChanged();
    }


    private void CloseStudentDetailsModal()
    {
        showModal = false;
    }

    private bool IsValidEmailForCompanyJobs(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        // Use simple regex for basic email validation
        return System.Text.RegularExpressions.Regex.IsMatch(email, 
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    private bool IsValidEmailForProfessorInternships(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        // Use simple regex for basic email validation
        return System.Text.RegularExpressions.Regex.IsMatch(email, 
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
        // Check if the input is null or empty
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        // Ensure it contains exactly 10 digits
        return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\d{10}$");
    }

    private void ValidatePostalCode(ChangeEventArgs e)
    {
        var inputValue = e.Value?.ToString() ?? string.Empty;
        showErrorMessageforPostalCode = !IsValidPostalCodeForCompanyJobs(inputValue);
    }

    private bool IsValidPostalCodeForCompanyJobs(string postalCode)
    {
        // Ensure it contains exactly 5 digits
        return !string.IsNullOrWhiteSpace(postalCode) &&
               System.Text.RegularExpressions.Regex.IsMatch(postalCode, @"^\d{5}$");
    }



    private async Task HandleTemporarySaveProfessorEvent()
    {
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
            "Είστε σίγουροι πως θέλετε να υποβάλλετε την <strong>Εκδήλωση</strong>;<br><br>" +
            "Η εκδήλωση θα καταχωρηθεί ως '<strong>Μη Δημοσιευμένη</strong>'."
        });

        if (!isConfirmed)
            return;

        await SaveProfessorEvent(false); 
    }

    private async Task HandlePublishSaveProfessorEvent()
    {
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
            "Είστε σίγουροι πως θέλετε να υποβάλλετε την <strong>Εκδήλωση</strong>;<br><br>" +
            "Η εκδήλωση θα καταχωρηθεί ως '<strong>Δημοσιευμένη</strong>'."
        });

        if (!isConfirmed)
            return;

        await SaveProfessorEvent(true); 
    }


    private async Task SaveProfessorEvent(bool publishEvent)
    {
        try
        {
            // Reset error states
            showErrorMessageForUploadingProfessorEvent = true;
            isFormValidToSaveEventAsProfessor = false;

            // Validate required fields
            if (string.IsNullOrWhiteSpace(professorEvent.ProfessorEventType) ||
                string.IsNullOrWhiteSpace(professorEvent.ProfessorEventTitle) || 
                string.IsNullOrWhiteSpace(professorEvent.ProfessorEventDescription) || 
                professorEvent.ProfessorEventActiveDate.Date < DateTime.Today ||
                professorEvent.ProfessorEventTimeOnly == TimeOnly.MinValue ||
                IsTimeInRestrictedRangeWhenUploadEventAsCompany(professorEvent.ProfessorEventTimeOnly) || 
                string.IsNullOrWhiteSpace(professorEvent.ProfessorEventPerifereiaLocation) ||
                string.IsNullOrWhiteSpace(professorEvent.ProfessorEventDimosLocation) ||
                string.IsNullOrWhiteSpace(professorEvent.ProfessorEventPlaceLocation) ||
                string.IsNullOrWhiteSpace(professorEvent.ProfessorEventPostalCodeLocation) ||
                professorEvent.ProfessorEventOfferingTransportToEventLocation == null ||
                !SelectedAreasWhenUploadEventAsProfessor.Any())
            {
                return;
            }

            // Validate transport starting points if transport is offered
            if (professorEvent.ProfessorEventOfferingTransportToEventLocation == true && 
                string.IsNullOrWhiteSpace(professorEvent.ProfessorEventStartingPointLocationToTransportPeopleToEvent1))
            {
                return;
            }

            // Validate other organizer if visible
            if (professorEvent.ProfessorEventOtherOrganizerToBeVisible && 
                string.IsNullOrWhiteSpace(professorEvent.ProfessorEventOtherOrganizer))
            {
                return;
            }

            // All validations passed
            isFormValidToSaveEventAsProfessor = true;
            showErrorMessageForUploadingProfessorEvent = false;

            // Prepare data for saving
            professorEvent.ProfessorEventAreasOfInterest = string.Join(",", SelectedAreasWhenUploadEventAsProfessor.Select(a => a.AreaName));

            // Get or create professor information
            var professor = await dbContext.Professors
                .FirstOrDefaultAsync(p => p.ProfEmail == CurrentUserEmail) ?? new Professor();

            // Update professor details
            professor.ProfEmail = CurrentUserEmail;
            professor.ProfName = professorName;
            professor.ProfSurname = professorSurname;
            professor.ProfUniversity = professorUniversity;
            professor.ProfImage = professorImage;

            if (professor.Id == 0) // New professor
            {
                dbContext.Professors.Add(professor);
            }

            // Set event properties
            professorEvent.RNGForEventUploadedAsProfessor = new Random().NextInt64();
            professorEvent.RNGForEventUploadedAsProfessor_HashedAsUniqueID = HashingHelper.HashLong(professorEvent.RNGForEventUploadedAsProfessor);
            professorEvent.ProfessorEventStatus = publishEvent ? "Δημοσιευμένη" : "Μη Δημοσιευμένη";
            professorEvent.ProfessorEmailUsedToUploadEvent = CurrentUserEmail;
            professorEvent.ProfessorEventUploadedDate = DateTime.Now;
            professorEvent.ProfessorEventTime = professorEvent.ProfessorEventTimeOnly.ToTimeSpan();
            professorEvent.Professor = professor;

            if (dbContext.Entry(professorEvent).State == EntityState.Detached)
            {
                dbContext.ProfessorEvents.Add(professorEvent);
            }

            await dbContext.SaveChangesAsync();
        
            saveEventAsProfessorMessage = "Η εκδήλωση δημιουργήθηκε επιτυχώς";
            isSaveAnnouncementAsProfessorSuccessful = true;
            showSuccessMessage = true;
    
            // Refresh the page or navigate as needed
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Σφάλμα κατά την αποθήκευση: {ex.Message}");
            Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
            saveEventAsProfessorMessage = "Προέκυψε σφάλμα κατά την αποθήκευση. Παρακαλώ προσπαθήστε ξανά.";
            isSaveAnnouncementAsProfessorSuccessful = false;
            showSuccessMessage = false;
            showErrorMessageForUploadingProfessorEvent = true;
        }
    }

    private async Task ShowStudentDetailsAtProfessorEventInterest(InterestInProfessorEvent application)
    {
        // === STUDENT LOOKUP ===
        var studentUniqueId = application.StudentUniqueIDShowInterestForEvent;
        selectedStudentFromCache = studentDataCache.Values
            .FirstOrDefault(s => s.Student_UniqueID == studentUniqueId);

        if (selectedStudentFromCache == null)
        {
            Console.WriteLine($"Student with ID {studentUniqueId} not found in cache - loading from DB");

            selectedStudentFromCache = await dbContext.Students
                .Where(s => s.Student_UniqueID == studentUniqueId)
                .Select(s => new Student {
                    Id = s.Id,
                    Student_UniqueID = s.Student_UniqueID,
                    Email = s.Email,
                    Image = s.Image,
                    Name = s.Name,
                    Surname = s.Surname,
                    Telephone = s.Telephone,
                    PermanentAddress = s.PermanentAddress,
                    PermanentRegion = s.PermanentRegion,
                    PermanentTown = s.PermanentTown,
                    Attachment = s.Attachment,
                    LinkedInProfile = s.LinkedInProfile,
                    PersonalWebsite = s.PersonalWebsite,
                    Transport = s.Transport,
                    RegNumber = s.RegNumber,
                    University = s.University,
                    Department = s.Department,
                    EnrollmentDate = s.EnrollmentDate,
                    StudyYear = s.StudyYear,
                    LevelOfDegree = s.LevelOfDegree,
                    AreasOfExpertise = s.AreasOfExpertise,
                    Keywords = s.Keywords
                })
                .FirstOrDefaultAsync();

            if (selectedStudentFromCache != null)
            {
                studentDataCache[selectedStudentFromCache.Email.ToLower()] = selectedStudentFromCache;
            }
        }

        // === PROFESSOR LOOKUP ===
        var professorUniqueId = application.ProfessorUniqueIDWhereStudentShowedInterest;
        Professor selectedProfessorFromCache = null;

        if (!string.IsNullOrEmpty(professorUniqueId))
        {
            professorDataCache.TryGetValue(professorUniqueId, out selectedProfessorFromCache);

            if (selectedProfessorFromCache == null)
            {
                Console.WriteLine($"Professor with ID {professorUniqueId} not found in cache - loading from DB");

                selectedProfessorFromCache = await dbContext.Professors
                    .Where(p => p.Professor_UniqueID == professorUniqueId)
                    .FirstOrDefaultAsync();

                if (selectedProfessorFromCache != null)
                {
                    professorDataCache[professorUniqueId] = selectedProfessorFromCache;
                }
            }
        }

        // === ASSIGN FINAL COMPOSITE OBJECT ===
        selectedStudentToShowDetailsForInterestinProfessorEvent = new InterestInProfessorEvent
        {
            Id = application.Id,
            ProfessorEmailWhereStudentShowedInterest = application.ProfessorEmailWhereStudentShowedInterest,
            ProfessorUniqueIDWhereStudentShowedInterest = application.ProfessorUniqueIDWhereStudentShowedInterest,
            StudentEmailShowInterestForEvent = application.StudentEmailShowInterestForEvent,
            StudentUniqueIDShowInterestForEvent = application.StudentUniqueIDShowInterestForEvent,
            RNGForProfessorEventInterest = application.RNGForProfessorEventInterest,
            RNGForProfessorEventInterest_HashedAsUniqueID = application.RNGForProfessorEventInterest_HashedAsUniqueID,
            DateTimeStudentShowInterest = application.DateTimeStudentShowInterest,
            StudentTransportNeedWhenShowInterestForProfessorEvent = application.StudentTransportNeedWhenShowInterestForProfessorEvent,
            StudentTransportChosenLocationWhenShowInterestForProfessorEvent = application.StudentTransportChosenLocationWhenShowInterestForProfessorEvent,
            ProfessorEventStatusAtStudentSide = application.ProfessorEventStatusAtStudentSide,
            ProfessorEventStatusAtProfessorSide = application.ProfessorEventStatusAtProfessorSide,

            StudentDetails = new InterestInProfessorEvent_StudentDetails
            {
                StudentEmailShowInterestForProfessorEvent = selectedStudentFromCache?.Email,
                StudentUniqueIDShowInterestForProfessorEvent = selectedStudentFromCache?.Student_UniqueID,
                DateTimeStudentShowInterestForProfessorEvent = application.DateTimeStudentShowInterest,
                RNGForProfessorEventShowInterestAsStudent_HashedAsUniqueID = application.RNGForProfessorEventInterest_HashedAsUniqueID
            },

            ProfessorDetails = new InterestInProfessorEvent_ProfessorDetails
            {
                ProfessorEmailWhereStudentShowInterestForProfessorEvent = selectedProfessorFromCache?.ProfEmail,
                ProfessorUniqueIDWhereStudentShowInterestForProfessorEvent = selectedProfessorFromCache?.Professor_UniqueID
            }
        };

        showModalForStudentsAtProfessorEventInterest = true;
        StateHasChanged();
    }

    private void CloseStudentDetailsModalAtProfessorEventInterest()
    {
        showModalForStudentsAtProfessorEventInterest = false;
    }


    private async Task ToggleAndLoadCompanyAndProfessorEventsAsStudent()
    {
        isCompanyEventsVisibleToSeeAsStudent = !isCompanyEventsVisibleToSeeAsStudent;
        isProfessorEventsVisibleToSeeAsStudent = isCompanyEventsVisibleToSeeAsStudent;

        if (isCompanyEventsVisibleToSeeAsStudent)
        {
            // Load company events
            companyEventsToSeeAsStudent = await dbContext.CompanyEvents
                .Where(e => e.CompanyEventStatus == "Δημοσιευμένη")
                .ToListAsync();

            // Load professor events with their professor data included
            professorEventsToSeeAsStudent = await dbContext.ProfessorEvents
                .Include(e => e.Professor)  // Crucial - includes professor data
                .Where(e => e.ProfessorEventStatus == "Δημοσιευμένη")
                .ToListAsync();
        }
        else
        {
            // Clear the lists when toggling off
            companyEventsToSeeAsStudent = new List<CompanyEvent>();
            professorEventsToSeeAsStudent = new List<ProfessorEvent>();
        }
    }

    private void ToggleTransport(long rngForEventUploadedAsProfessor, object value)
    {
        bool isChecked = (bool)value;

        // Update transport need
        needsTransportForProfessorEvent[rngForEventUploadedAsProfessor] = isChecked;

        // If "Χρειάζομαι Μεταφορά" is unticked, remove the selected starting point entirely
        if (!isChecked)
        {
            selectedStartingPoint.Remove(rngForEventUploadedAsProfessor);
        }
    }



    private void CloseEventDetails()
    {
        selectedEvent = null; // Reset the selected event
    }

    private void ShowEventDetails(object eventDetails)
    {
        selectedEvent = eventDetails;
    }


    private void CloseModalForCompanyAndProfessorEventTitles()
    {
        selectedEvent = null; // Reset the selected event
        isModalVisibleToShowEventsOnCalendarForEachClickedDay = false;
        StateHasChanged(); // Refresh the modal content

    }

    private void CloseModal()
    {
        isModalVisibleToShowEventsOnCalendarForEachClickedDay = false;
        selectedEvent = null; // Reset any selected event
        StateHasChanged();
    }



    private DateTime ProfessorEventDate
    {
        get => _professorEventDate;
        set
        {
            _professorEventDate = value;
            CheckForExistingEventsAsProfessor();
        }
    }

    private void CheckForExistingEventsAsProfessor()
    {
        existingEventsCountToCheckAsProfessor = (eventsForDate.ContainsKey(ProfessorEventDate.Day) ? eventsForDate[ProfessorEventDate.Day].Count() : 0) + 
                                                (eventsForDateForProfessors.ContainsKey(ProfessorEventDate.Day) ? eventsForDateForProfessors[ProfessorEventDate.Day].Count() : 0);
    }

    private async Task LoadProfessorInterestsForCompanyEvents()
    {
        filteredProfessorInterestForCompanyEvents = await dbContext.InterestInCompanyEventsAsProfessor
            .Where(i => i.ProfessorEmailShowInterestForCompanyEvent == CurrentUserEmail)
            .ToListAsync();
    }

    private async Task<bool> ShowInterestInCompanyEventAsProfessor(CompanyEvent companyEvent)
    {
        // First ask for confirmation
        var confirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", 
            $"Πρόκεται να δείξετε Ενδιαφέρον για την Εκδήλωση: {companyEvent.CompanyEventTitle} της εταιρείας {companyEvent.Company?.CompanyName}. Είστε σίγουρος/η;");
        if (!confirmed) return false;

        // Retrieve the latest event status
        var latestEvent = await dbContext.CompanyEvents
            .AsNoTracking()
            .Where(e => e.RNGForEventUploadedAsCompany == companyEvent.RNGForEventUploadedAsCompany)
            .Select(e => new { e.CompanyEventStatus })
            .FirstOrDefaultAsync();

        if (latestEvent == null || latestEvent.CompanyEventStatus != "Δημοσιευμένη")
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Η Εκδήλωση έχει Αποδημοσιευτεί. Παρακαλώ δοκιμάστε αργότερα.");
            return false;
        }

        var professor = await GetProfessorDetails(CurrentUserEmail);
        if (professor == null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Δεν βρέθηκαν στοιχεία καθηγητή.");
            return false;
        }

        var company = await GetCompanyDetails(companyEvent.CompanyEmailUsedToUploadEvent);
        if (company == null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Δεν βρέθηκαν στοιχεία εταιρείας.");
            return false;
        }

        // Check for existing interest
        var existingInterest = await dbContext.InterestInCompanyEventsAsProfessor
            .FirstOrDefaultAsync(i => 
                i.ProfessorEmailShowInterestForCompanyEvent == professor.ProfEmail &&
                i.RNGForCompanyEventInterestAsProfessor == companyEvent.RNGForEventUploadedAsCompany);

        if (existingInterest != null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", $"Έχετε ήδη δείξει ενδιαφέρον για: {companyEvent.CompanyEventTitle}!");
            return false;
        }

        using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            // Create main interest record with details
            var interest = new InterestInCompanyEventAsProfessor
            {
                RNGForCompanyEventInterestAsProfessor = companyEvent.RNGForEventUploadedAsCompany,
                RNGForCompanyEventInterestAsProfessor_HashedAsUniqueID = companyEvent.RNGForEventUploadedAsCompany_HashedAsUniqueID,
                DateTimeProfessorShowInterestForCompanyEvent = DateTime.UtcNow,
                CompanyEventStatus_ShowInterestAsProfessor_AtCompanySide = "Προς Επεξεργασία",
                CompanyEventStatus_ShowInterestAsProfessor_AtProfessorSide = "Έχετε Δείξει Ενδιαφέρον",
                ProfessorEmailShowInterestForCompanyEvent = professor.ProfEmail,
                ProfessorUniqueIDShowInterestForCompanyEvent = professor.Professor_UniqueID,
                CompanyEmailWhereProfessorShowedInterest = companyEvent.CompanyEmailUsedToUploadEvent,
                CompanyUniqueIDWhereProfessorShowedInterest = company.Company_UniqueID,
        
                ProfessorDetails = new InterestInCompanyEventAsProfessor_ProfessorDetails
                {
                    ProfessorUniqueIDShowInterestForCompanyEvent = professor.Professor_UniqueID,
                    ProfessorEmailShowInterestForCompanyEvent = professor.ProfEmail,
                    DateTimeProfessorShowInterestForCompanyEvent = DateTime.UtcNow,
                    RNGForCompanyEventShowInterestAsProfessor_HashedAsUniqueID = companyEvent.RNGForEventUploadedAsCompany_HashedAsUniqueID
                },
        
                CompanyDetails = new InterestInCompanyEventAsProfessor_CompanyDetails
                {
                    CompanyUniqueIDWhereProfessorShowInterestForCompanyEvent = company.Company_UniqueID,
                    CompanyEmailWhereProfessorShowInterestForCompanyEvent = companyEvent.CompanyEmailUsedToUploadEvent
                }
            };

            dbContext.InterestInCompanyEventsAsProfessor.Add(interest);

            // Add platform action
            dbContext.PlatformActions.Add(new PlatformActions
            {
                UserRole_PerformedAction = "PROFESSOR",
                ForWhat_PerformedAction = "COMPANY_EVENT",
                HashedPositionRNG_PerformedAction = HashingHelper.HashLong(companyEvent.RNGForEventUploadedAsCompany),
                TypeOfAction_PerformedAction = "SHOW_INTEREST",
                DateTime_PerformedAction = DateTime.UtcNow
            });

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            // Send emails
            try
            {
                await InternshipEmailService.SendConfirmationToProfessorForInterestInCompanyEvent(
                    professor.ProfEmail,
                    professor.ProfName,
                    professor.ProfSurname,
                    companyEvent.CompanyEventTitle,
                    company.CompanyName,
                    companyEvent.RNGForEventUploadedAsCompany_HashedAsUniqueID);

                await InternshipEmailService.SendNotificationToCompanyForProfessorInterestInEvent(
                    companyEvent.CompanyEmailUsedToUploadEvent,
                    company.CompanyName,
                    professor.ProfName,
                    professor.ProfSurname,
                    professor.ProfEmail,
                    professor.ProfWorkTelephone,
                    companyEvent.CompanyEventTitle,
                    companyEvent.RNGForEventUploadedAsCompany_HashedAsUniqueID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
            }

            await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Η έκφραση ενδιαφέροντος για την εκδήλωση {companyEvent.CompanyEventTitle} υποβλήθηκε επιτυχώς!");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Full error: {ex}");
            await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Σφάλμα κατά την υποβολή: {ex.InnerException?.Message ?? ex.Message}");
            return false;
        }

        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        return false;
    }


    private async Task<bool> ShowInterestInProfessorEventAsCompany(ProfessorEvent professorEvent)
    {
        // First ask for confirmation - now using navigation property for professor name
        var confirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", 
            $"Πρόκεται να δείξετε Ενδιαφέρον για την Εκδήλωση: {professorEvent.ProfessorEventTitle} του καθηγητή {professorEvent.Professor?.ProfName} {professorEvent.Professor?.ProfSurname}. Είστε σίγουρος/η;");
        if (!confirmed) return false;

        // Retrieve the latest event status with professor included
        var latestEvent = await dbContext.ProfessorEvents
            .Include(e => e.Professor)
            .AsNoTracking()
            .Where(e => e.RNGForEventUploadedAsProfessor == professorEvent.RNGForEventUploadedAsProfessor)
            .Select(e => new { e.ProfessorEventStatus })
            .FirstOrDefaultAsync();

        if (latestEvent == null || latestEvent.ProfessorEventStatus != "Δημοσιευμένη")
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Η Εκδήλωση έχει Αποδημοσιευτεί. Παρακαλώ δοκιμάστε αργότερα.");
            return false;
        }

        var company = await GetCompanyDetails(CurrentUserEmail);
        if (company == null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Δεν βρέθηκαν στοιχεία εταιρείας.");
            return false;
        }

        // Get professor from navigation property or fall back to query
        var professor = professorEvent.Professor ?? await dbContext.Professors
            .FirstOrDefaultAsync(p => p.ProfEmail == professorEvent.ProfessorEmailUsedToUploadEvent);

        if (professor == null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Δεν βρέθηκαν στοιχεία καθηγητή.");
            return false;
        }

        // Check for existing interest
        var existingInterest = await dbContext.InterestInProfessorEventsAsCompany
            .FirstOrDefaultAsync(i => 
                i.CompanyEmailShowInterestForProfessorEvent == company.CompanyEmail &&
                i.RNGForProfessorEventInterestAsCompany == professorEvent.RNGForEventUploadedAsProfessor);

        if (existingInterest != null)
        {
            await JS.InvokeVoidAsync("confirmActionWithHTML2", $"Έχετε ήδη δείξει ενδιαφέρον για: {professorEvent.ProfessorEventTitle}!");
            return false;
        }

        if (!numberOfCompanyPeopleInputWhenCompanyShowsInterestInProfessorEvent.TryGetValue(professorEvent.RNGForEventUploadedAsProfessor, out var numberOfPeople))
        {
            await ShowAlert("Παρακαλώ επιλέξτε αριθμό ατόμων πριν δείξετε ενδιαφέρον.");
            return false;
        }

        using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            // Create main interest record with details
            var interest = new InterestInProfessorEventAsCompany
            {
                RNGForProfessorEventInterestAsCompany = professorEvent.RNGForEventUploadedAsProfessor,
                RNGForProfessorEventInterestAsCompany_HashedAsUniqueID = professorEvent.RNGForEventUploadedAsProfessor_HashedAsUniqueID,
                DateTimeCompanyShowInterestForProfessorEvent = DateTime.UtcNow,
                ProfessorEventStatus_ShowInterestAsCompany_AtCompanySide = "Έχετε Δείξει Ενδιαφέρον",
                ProfessorEventStatus_ShowInterestAsCompany_AtProfessorSide = "Προς Επεξεργασία",
                ProfessorEmailWhereCompanyShowedInterest = professorEvent.ProfessorEmailUsedToUploadEvent, // Updated to use foreign key
                ProfessorUniqueIDWhereCompanyShowedInterest = professor.Professor_UniqueID,
                CompanyEmailShowInterestForProfessorEvent = company.CompanyEmail,
                CompanyUniqueIDShowInterestForProfessorEvent = company.Company_UniqueID,
                CompanyNumberOfPeopleToShowUpWhenShowInterestForProfessorEvent = numberOfPeople.ToString(),
    
                CompanyDetails = new InterestInProfessorEventAsCompany_CompanyDetails
                {
                    CompanyUniqueIDShowInterestForProfessorEvent = company.Company_UniqueID,
                    CompanyEmailShowInterestForProfessorEvent = company.CompanyEmail,
                    DateTimeCompanyShowInterestForProfessorEvent = DateTime.UtcNow,
                    RNGForProfessorEventShowInterestAsCompany_HashedAsUniqueID = professorEvent.RNGForEventUploadedAsProfessor_HashedAsUniqueID
                },
    
                ProfessorDetails = new InterestInProfessorEventAsCompany_ProfessorDetails
                {
                    ProfessorUniqueIDWhereCompanyShowInterestForProfessorEvent = professor.Professor_UniqueID,
                    ProfessorEmailWhereCompanyShowInterestForProfessorEvent = professorEvent.ProfessorEmailUsedToUploadEvent // Updated to use foreign key
                }
            };

            dbContext.InterestInProfessorEventsAsCompany.Add(interest);

            // Add platform action
            dbContext.PlatformActions.Add(new PlatformActions
            {
                UserRole_PerformedAction = "COMPANY",
                ForWhat_PerformedAction = "PROFESSOR_EVENT",
                HashedPositionRNG_PerformedAction = HashingHelper.HashLong(professorEvent.RNGForEventUploadedAsProfessor),
                TypeOfAction_PerformedAction = "SHOW_INTEREST",
                DateTime_PerformedAction = DateTime.UtcNow
            });

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            // Send emails - updated to use professor from navigation property
            try
            {
                await InternshipEmailService.SendConfirmationToCompanyForInterestInProfessorEvent(
                    company.CompanyEmail,
                    company.CompanyName,
                    professorEvent.ProfessorEventTitle,
                    professor.ProfName,
                    professor.ProfSurname,
                    professorEvent.RNGForEventUploadedAsProfessor_HashedAsUniqueID,
                    numberOfPeople);

                await InternshipEmailService.SendNotificationToProfessorForCompanyInterestInEvent(
                    professor.ProfEmail,
                    professor.ProfName,
                    professor.ProfSurname,
                    company.CompanyName,
                    company.CompanyEmail,
                    company.CompanyTelephone,
                    professorEvent.ProfessorEventTitle,
                    professorEvent.RNGForEventUploadedAsProfessor_HashedAsUniqueID,
                    numberOfPeople);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
            }

            await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Η έκφραση ενδιαφέροντος για την εκδήλωση {professorEvent.ProfessorEventTitle} υποβλήθηκε επιτυχώς!");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Full error: {ex}");
            await JS.InvokeVoidAsync("confirmActionWithHTML2", 
                $"Σφάλμα κατά την υποβολή: {ex.InnerException?.Message ?? ex.Message}");
            return false;
        }

        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        return false;
    }

    private int GetOrAddNumberOfPeople(long eventId)
    {
        if (!numberOfCompanyPeopleInputWhenCompanyShowsInterestInProfessorEvent.ContainsKey(eventId))
        {
            numberOfCompanyPeopleInputWhenCompanyShowsInterestInProfessorEvent[eventId] = 1; // Default value
        }
        return numberOfCompanyPeopleInputWhenCompanyShowsInterestInProfessorEvent[eventId];
    }

    private void SetNumberOfPeople(long eventId, int value)
    {
        if (numberOfCompanyPeopleInputWhenCompanyShowsInterestInProfessorEvent.ContainsKey(eventId))
        {
            numberOfCompanyPeopleInputWhenCompanyShowsInterestInProfessorEvent[eventId] = value;
        }
        else
        {
            numberOfCompanyPeopleInputWhenCompanyShowsInterestInProfessorEvent.Add(eventId, value);
        }
    }


    private async Task HandleFileUploadForProfessorInternships(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file != null && file.ContentType == "application/pdf")
        {
            using var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms); // Copy file stream to memory stream
            professorInternship.ProfessorInternshipAttachment = ms.ToArray(); // Convert memory stream to byte array
        }
    }

    private async Task SaveProfessorInternship(bool isPublished)
    {
        Console.WriteLine($"Validation check - Title: {professorInternship.ProfessorInternshipTitle}");
        Console.WriteLine($"Validation check - Type: {professorInternship.ProfessorInternshipType}");
        Console.WriteLine($"Validation check - ESPA: {professorInternship.ProfessorInternshipESPA}");
        Console.WriteLine($"Validation check - ContactPerson: {professorInternship.ProfessorInternshipContactPerson}");
        Console.WriteLine($"Validation check - Description: {professorInternship.ProfessorInternshipDescription}");
        Console.WriteLine($"Validation check - ActivePeriod: {professorInternship.ProfessorInternshipActivePeriod} (Today: {DateTime.Today})");
        Console.WriteLine($"Validation check - FinishEstimation: {professorInternship.ProfessorInternshipFinishEstimation} (Today: {DateTime.Today})");
        Console.WriteLine($"Validation check - SelectedAreas count: {SelectedAreasWhenUploadInternshipAsProfessor.Count}");
        Console.WriteLine($"Validation check - Email: {professorInternship.ProfessorEmailUsedToUploadInternship} (Valid: {IsValidEmailForProfessorInternships(professorInternship.ProfessorEmailUsedToUploadInternship)})");
        Console.WriteLine($"Validation check - Region: {professorInternship.ProfessorInternshipPerifereiaLocation}");
        Console.WriteLine($"Validation check - Town: {professorInternship.ProfessorInternshipDimosLocation}");

        try
        {
            if (string.IsNullOrWhiteSpace(professorInternship.ProfessorInternshipTitle) ||
                string.IsNullOrWhiteSpace(professorInternship.ProfessorInternshipType) ||
                string.IsNullOrWhiteSpace(professorInternship.ProfessorInternshipESPA) ||
                string.IsNullOrWhiteSpace(professorInternship.ProfessorInternshipContactPerson) ||
                string.IsNullOrWhiteSpace(professorInternship.ProfessorInternshipDescription) ||
                string.IsNullOrWhiteSpace(professorInternship.ProfessorInternshipPerifereiaLocation) ||
                string.IsNullOrWhiteSpace(professorInternship.ProfessorInternshipDimosLocation) ||
                !IsValidEmailForProfessorInternships(professorInternship.ProfessorEmailUsedToUploadInternship) ||
                professorInternship.ProfessorInternshipActivePeriod.Date < DateTime.Today || 
                professorInternship.ProfessorInternshipFinishEstimation.Date < DateTime.Today ||
                !SelectedAreasWhenUploadInternshipAsProfessor.Any())
            {
                showErrorMessage = true;
                Console.WriteLine("Validation failed: Missing fields or invalid date.");
                return;
            }

            // Populate professorInternship with form data
            professorInternship.RNGForInternshipUploadedAsProfessor = new Random().NextInt64(); // Updated property
            professorInternship.RNGForInternshipUploadedAsProfessor_HashedAsUniqueID = HashingHelper.HashLong(professorInternship.RNGForInternshipUploadedAsProfessor); // Updated property
            professorInternship.ProfessorUploadedInternshipStatus = isPublished ? "Δημοσιευμένη" : "Μη Δημοσιευμένη";
            professorInternship.ProfessorInternshipUploadDate = DateTime.Now;
            professorInternship.ProfessorInternshipAreas = string.Join(",", SelectedAreasWhenUploadInternshipAsProfessor.Select(a => a.AreaName));


            // Set EKPA supervisor (if applicable)
            if (selectedCompanyId.HasValue)
            {
                var company = await dbContext.Companies
                    .FirstOrDefaultAsync(p => p.Id == selectedCompanyId.Value);

                professorInternship.ProfessorInternshipEKPASupervisor = company?.CompanyName ?? "Unknown Company";
            }
            else
            {
                professorInternship.ProfessorInternshipEKPASupervisor = "Χωρίς Προτίμηση";
            }

            Console.WriteLine($"Saving internship: {professorInternship.ProfessorInternshipTitle}, Status: {professorInternship.ProfessorUploadedInternshipStatus}");

            // Save to database
            dbContext.ProfessorInternships.Add(professorInternship);
            await dbContext.SaveChangesAsync();

            // Show success message
            showSuccessMessage = true;
            showErrorMessage = false;

            // Reset form and update UI
            professorInternship = new ProfessorInternship();
            SelectedAreasWhenUploadInternshipAsProfessor.Clear();
            StateHasChanged();
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }
        catch (Exception ex)
        {
            showSuccessMessage = false;
            showErrorMessage = true;
            Console.WriteLine($"Error uploading professor internship: {ex.Message}");
            await JS.InvokeVoidAsync("alert", $"Error saving internship: {ex.Message}");
        }
    }



    private async Task HandleSaveClickToSaveProfessorInternship()
    {
        // Show custom confirmation dialog with HTML styling
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
            "Είστε σίγουροι πως θέλετε να υποβάλλετε <strong>Νέα Θέση Πρακτικής Άσκησης</strong>;<br><br>" +
            "Η Θέση θα καταχωρηθεί ως '<strong>Μη Δημοσιευμένη</strong>' στο Ιστορικό Θέσεων Πρακτικής Άσκησης.<br><br>" +
            "<strong style='color: red;'>Αν επιθυμείτε να την Δημοσιεύσετε, απαιτούνται επιπλέον ενέργειες!</strong>"
        });

        if (!isConfirmed)
            return;

        // Save as "Μη Δημοσιευμένη"
        professorInternship.ProfessorUploadedInternshipStatus = "Μη Δημοσιευμένη";

        // Pass 'false' to indicate it's not published
        await SaveProfessorInternship(false);
    }


    private async Task HandlePublishClickToSaveProfessorInternship()
    {
        // Show custom confirmation dialog with HTML styling
        bool isConfirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", new object[] { 
            "Είστε σίγουροι πως θέλετε να υποβάλλετε <strong>Νέα Θέση Πρακτικής Άσκησης</strong>;<br><br>" +
            "Η Θέση θα καταχωρηθεί ως '<strong>Δημοσιευμένη</strong>' στο Ιστορικό Θέσεων Πρακτικής Άσκησης.<br><br>" +
            "<strong style='color: red;'>Αν επιθυμείτε να την Αποδημοσιεύσετε, απαιτούνται επιπλέον ενέργειες!</strong>"
        });

        if (!isConfirmed)
            return;

        // Save as "Δημοσιευμένη"
        professorInternship.ProfessorUploadedInternshipStatus = "Δημοσιευμένη";

        // Pass 'true' to indicate it's published
        await SaveProfessorInternship(true);
    }


    private CompanyInternship ConvertToCompanyInternship(AllInternships internship)
    {
        return new CompanyInternship
        {
            RNGForInternshipUploadedAsCompany = internship.RNGForCompanyInternship, // Updated property
            CompanyInternshipTitle = internship.InternshipTitle,
            CompanyInternshipType = internship.InternshipType,
            CompanyEmailUsedToUploadInternship = internship.CompanyEmail, // Updated property
            RNGForInternshipUploadedAsCompany_HashedAsUniqueID = internship.RNGForCompanyInternship_HashedAsUniqueID, // Updated property
            Company = new Company { CompanyName = internship.CompanyName } // Set via navigation property
        };
    }

    private ProfessorInternship ConvertToProfessorInternship(AllInternships internship)
    {
        return new ProfessorInternship
        {
            RNGForInternshipUploadedAsProfessor = internship.RNGForProfessorInternship,
            ProfessorInternshipTitle = internship.InternshipTitle,
            ProfessorInternshipType = internship.InternshipType,
            ProfessorEmailUsedToUploadInternship = internship.ProfessorEmail,
            RNGForInternshipUploadedAsProfessor_HashedAsUniqueID = internship.RNGForProfessorInternship_HashedAsUniqueID,
            Professor = new Professor { ProfName = internship.ProfessorName } // Set via navigation property

        };
    }

    private async Task ShowCompanyDetailsInThesisCompanyName_StudentThesisApplications(string companyEmail)
    {
        try
        {
            // First check if we already have the company details in cache
            if (companyDataCache.TryGetValue(companyEmail, out var cachedCompany))
            {
                selectedCompanyDetails_ThesisStudentApplicationsToShow = cachedCompany;
            }
            else
            {
                // Fetch the company details from the database using email (more reliable than name)
                selectedCompanyDetails_ThesisStudentApplicationsToShow = await dbContext.Companies
                    .FirstOrDefaultAsync(c => c.CompanyEmail == companyEmail);

                // Add to cache if found
                if (selectedCompanyDetails_ThesisStudentApplicationsToShow != null)
                { 
                    companyDataCache[companyEmail] = selectedCompanyDetails_ThesisStudentApplicationsToShow;
                }
            }

            if (selectedCompanyDetails_ThesisStudentApplicationsToShow != null)
            {
                isModalOpenToSeeCompanyDetails_ThesisStudentApplicationsToShow = true;
                StateHasChanged();
            }
            else
            {
                await JS.InvokeVoidAsync("alert", "Δεν βρέθηκαν στοιχεία εταιρείας");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading company details: {ex.Message}");
            await JS.InvokeVoidAsync("alert", "Σφάλμα φόρτωσης στοιχείων εταιρείας");
        }
    }

    private void CloseCompanyDetailsModal_StudentThesisApplications()
    {
        isModalOpenToSeeCompanyDetails_ThesisStudentApplicationsToShow = false;
        StateHasChanged();
    }

    private async Task ShowProfessorDetailsInThesisProfessorName_StudentThesisApplications(string professorEmail)
    {
        try
        {
            // First check if we already have the professor details in cache
            if (professorDataCache.TryGetValue(professorEmail, out var cachedProfessor))
            {
                selectedProfessorDetails_ThesisStudentApplicationsToShow = cachedProfessor;
            }
            else
            {
                // Fetch the professor details from the database using email (more reliable than name)
                selectedProfessorDetails_ThesisStudentApplicationsToShow = await dbContext.Professors
                    .FirstOrDefaultAsync(p => p.ProfEmail == professorEmail);

                // Add to cache if found
                if (selectedProfessorDetails_ThesisStudentApplicationsToShow != null)
                { 
                    professorDataCache[professorEmail] = selectedProfessorDetails_ThesisStudentApplicationsToShow;
                }
            }

            if (selectedProfessorDetails_ThesisStudentApplicationsToShow != null)
            {
                isModalOpenToSeeProfessorDetails_ThesisStudentApplicationsToShow = true;
                StateHasChanged();
            }
            else
            {
                await JS.InvokeVoidAsync("alert", "Δεν βρέθηκαν στοιχεία καθηγητή");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading professor details: {ex.Message}");
            await JS.InvokeVoidAsync("alert", "Σφάλμα φόρτωσης στοιχείων καθηγητή");
        }
    }


    private void CloseProfessorDetailsModal_StudentThesisApplications()
    {
        isModalOpenToSeeProfessorDetails_ThesisStudentApplicationsToShow = false;
        StateHasChanged();
    }

    private void CloseProfessorDetailsModal_StudentInternshipApplications()
    {
        isProfessorDetailsModalVisible_StudentInternshipApplicationsShow = false;
        StateHasChanged();
    }

    private async Task ShowCompanyThesisDetailsModal_StudentThesisApplications(long thesisRNG)
    {
        // Fetch the company thesis details asynchronously
        selectedCompanyThesisDetails_ThesisStudentApplicationsToShow = await dbContext.CompanyTheses
            .FirstOrDefaultAsync(t => t.RNGForThesisUploadedAsCompany == thesisRNG);

        if (selectedCompanyThesisDetails_ThesisStudentApplicationsToShow != null)
        {
            // Open the modal if the thesis details are found
            isModalOpenToSeeCompanyThesisDetails_ThesisStudentApplicationsToShow = true;
            StateHasChanged(); // Update the UI
        }
        else
        {
            // Show an alert if no thesis details are found
            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Δεν μπορούν να εμφανιστούν οι λεπτομέρειες της Πτυχιακής. <span style='color:darkred;'>Η Πτυχιακή Δεν Είναι Πλέον Διαθέσιμη από τον Φορέα</span>");
        }
    }

    private void CloseCompanyThesisDetailsModal_StudentThesisApplications()
    {
        // Close the modal and reset the thesis details
        isModalOpenToSeeCompanyThesisDetails_ThesisStudentApplicationsToShow = false;
        StateHasChanged(); // Update the UI
    }

    private async Task ShowProfessorThesisDetailsModal_StudentThesisApplications(long thesisRNG)
    {
        // Fetch the professor thesis details asynchronously
        selectedProfessorThesisDetails_ThesisStudentApplicationsToShow = await dbContext.ProfessorTheses
            .FirstOrDefaultAsync(t => t.RNGForThesisUploaded == thesisRNG);

        if (selectedProfessorThesisDetails_ThesisStudentApplicationsToShow != null)
        {
            // Open the modal if the thesis details are found
            isModalOpenToSeeProfessorThesisDetails_ThesisStudentApplicationsToShow = true;
            StateHasChanged(); // Update the UI
        }
        else
        {
            // Show an alert if no thesis details are found
            await JS.InvokeVoidAsync("alert", "Professor thesis details not found.");
        }
    }

    private void CloseProfessorThesisDetailsModal_StudentThesisApplications()
    {
        isModalOpenToSeeProfessorThesisDetails_ThesisStudentApplicationsToShow = false;
        StateHasChanged(); 
    }

    private void CloseCompanyThesisEditModal()
    {
        isModalVisibleToEditCompanyThesisDetails = false;
    }

    private async Task SaveEditedCompanyThesis()
    {
        try
        {
            // Check if required fields are filled
            if (string.IsNullOrWhiteSpace(selectedCompanyThesis.CompanyThesisTitle) ||
                string.IsNullOrWhiteSpace(selectedCompanyThesis.CompanyThesisDescriptionsUploaded))
            {
                showSuccessMessage = false;
                showErrorMessage = true;
                return; // Prevent saving if required fields are missing
            }

            // Ensure SelectedAreasToEditForCompanyThesis contains all checked areas (even if no changes were made)
            if (SelectedAreasToEditForCompanyThesis == null || !SelectedAreasToEditForCompanyThesis.Any())
            {
                var currentAreas = selectedCompanyThesis.CompanyThesisAreasUpload.Split(",").ToList();
                SelectedAreasToEditForCompanyThesis = Areas
                    .Where(area => currentAreas.Contains(area.AreaName)) // Set the areas to the ones already selected
                    .ToList();
            }

            // Ensure SelectedSkillsToEditForCompanyThesis contains all checked skills (even if no changes were made)
            if (SelectedSkillsToEditForCompanyThesis == null || !SelectedSkillsToEditForCompanyThesis.Any())
            {
                var currentSkills = selectedCompanyThesis.CompanyThesisSkillsNeeded.Split(",").ToList();
                SelectedSkillsToEditForCompanyThesis = Skills
                    .Where(skill => currentSkills.Contains(skill.SkillName)) // Set the skills to the ones already selected
                    .ToList();
            }

            // Convert the selected areas to a comma-separated string
            selectedCompanyThesis.CompanyThesisAreasUpload = string.Join(",", SelectedAreasToEditForCompanyThesis.Select(area => area.AreaName));

            // Convert the selected skills to a comma-separated string
            selectedCompanyThesis.CompanyThesisSkillsNeeded = string.Join(",", SelectedSkillsToEditForCompanyThesis.Select(skill => skill.SkillName));

            // Find and update the thesis in the database
            var thesisToUpdate = await dbContext.CompanyTheses.FindAsync(selectedCompanyThesis.Id);
            if (thesisToUpdate != null)
            {
                thesisToUpdate.CompanyThesisTitle = selectedCompanyThesis.CompanyThesisTitle;
                thesisToUpdate.CompanyThesisCompanySupervisorFullName = selectedCompanyThesis.CompanyThesisCompanySupervisorFullName;
                thesisToUpdate.CompanyThesisDescriptionsUploaded = selectedCompanyThesis.CompanyThesisDescriptionsUploaded;
                thesisToUpdate.CompanyThesisAreasUpload = selectedCompanyThesis.CompanyThesisAreasUpload;
                thesisToUpdate.CompanyThesisSkillsNeeded = selectedCompanyThesis.CompanyThesisSkillsNeeded;
                thesisToUpdate.CompanyThesisDepartment = selectedCompanyThesis.CompanyThesisDepartment;
                thesisToUpdate.CompanyThesisStartingDate = selectedCompanyThesis.CompanyThesisStartingDate;
                thesisToUpdate.CompanyThesisContactPersonEmail = selectedCompanyThesis.CompanyThesisContactPersonEmail;
                thesisToUpdate.CompanyThesisContactPersonTelephone = selectedCompanyThesis.CompanyThesisContactPersonTelephone;

                // Update timestamp and count
                thesisToUpdate.CompanyThesisUpdateDateTime = DateTime.Now;
                thesisToUpdate.CompanyThesisTimesUpdated += 1;

                await dbContext.SaveChangesAsync();
                showSuccessMessage = true;
                showErrorMessage = false;
            }
            else
            {
                showSuccessMessage = false;
                showErrorMessage = true;
            }
        }
        catch (Exception ex)
        {
            showSuccessMessage = false;
            showErrorMessage = true;
            Console.Error.WriteLine($"Error saving company thesis: {ex.Message}");
        }
        finally
        {
            isModalVisibleToEditCompanyThesisDetails = false;
            StateHasChanged();
        }
    }



    private async Task FilterThesisApplications(ChangeEventArgs e)
    {

        filterValue = e.Value?.ToString() ?? "all"; // Ensure "all" is the default

        if (filterValue == "company")
        {
            showCompanyThesisApplications = true;
            showProfessorThesisApplications = false;
        }
        else if (filterValue == "professor")
        {
            showCompanyThesisApplications = false;
            showProfessorThesisApplications = true;
        }
        else
        {
            showCompanyThesisApplications = true;
            showProfessorThesisApplications = true;
        }

        await Task.Delay(1000); // Simulate loading delay

        StateHasChanged(); // Update UI
    }

    private async Task OnFilterChange(ChangeEventArgs e)
    {
        // Update the selected filter value
        selectedThesisFilter = e.Value.ToString();
    
        // Check dropdown state
        if (dropdownState == "All")
        {
            // Show all data, no filtering by professor name
            await SearchThesisApplicationsAsStudent(); // Assuming this method fetches all data
        }
        else
        {
            // Show only filtered data (e.g., professor name or other criteria)
            await FilterThesisApplicationsToSearchAsStudent();
        }
    }


    private async Task FilterThesisApplicationsToSearchAsStudent()
    {
        var filteredTheses = sumUpThesesFromBothCompanyAndProfessor
            ?.Where(thesis =>
                (selectedThesisFilter == "company" && thesis.CompanyThesisStatus == "Δημοσιευμένη") ||
                (selectedThesisFilter == "professor" && thesis.ProfessorThesisStatus == "Δημοσιευμένη") ||
                selectedThesisFilter == "all" &&
                (thesis.CompanyThesisStatus == "Δημοσιευμένη" || thesis.ProfessorThesisStatus == "Δημοσιευμένη"))
            .ToList();

        publishedTheses = filteredTheses;

        // Optionally log the filtered count
        Console.WriteLine($"Filtered theses count: {publishedTheses.Count}");

        await Task.Delay(1000); // Optional delay to simulate async loading
        StateHasChanged(); // Trigger UI update
    }


    private int currentPageForThesisToSee = 1;
    private int pageSizeForThesisToSee = 3; // Show only 3 thesis per page
    private int totalThesisCountForThesisToSee = 4; // For example, set to 4 total thesis
    private int totalPagesForThesisToSee = 1; // Initialize to 1 to avoid divide by zero error

    // Method to set the thesis count and calculate total pages
    private void SetTotalThesisCount(int count)
    {
        totalThesisCountForThesisToSee = count;
        totalPagesForThesisToSee = (int)Math.Ceiling((double)totalThesisCountForThesisToSee / pageSizeForThesisToSee);
    }

    private bool IsPreviousDisabled => currentPageForThesisToSee == 1;
    private bool IsNextDisabled => currentPageForThesisToSee >= totalPagesForThesisToSee; // Disable if on the last page

    private void PreviousPageForThesisToSee()
    {
        if (currentPageForThesisToSee > 1)
        {
            currentPageForThesisToSee--;
        }
    }

    private void NextPageForThesisToSee()
    {
        if (currentPageForThesisToSee < totalPagesForThesisToSee)
        {
            currentPageForThesisToSee++;
        }
    }

    private void UpdatePagination()
    {
        totalPagesForThesisToSee = (int)Math.Ceiling((double)totalThesisCountForThesisToSee / pageSizeForThesisToSee);
        StateHasChanged(); // Triggers a re-render to apply changes
    }




    private string GetThesisRowColor(object thesis)
    {
        if (thesis is CompanyThesisApplied companyThesis)
        {
            return companyThesis.CompanyThesisStatusAppliedAtStudentSide switch
            {
                "Επιτυχής" => "lightgreen",
                "Απορρίφθηκε" => "lightcoral",
                "Απορρίφθηκε (Απόσυρση Θέσεως Από Εταιρία)" => "coral",
                "Αποσύρθηκε από τον φοιτητή" => "lightyellow",
                _ => "transparent"
            };
        }
        else if (thesis is ProfessorThesisApplied professorThesis)
        {
            return professorThesis.ProfessorThesisStatusAppliedAtStudentSide switch
            {
                "Επιτυχής" => "lightgreen",
                "Απορρίφθηκε" => "lightcoral",
                "Απορρίφθηκε (Απόσυρση Θέσεως Από τον Καθηγητή)" => "coral",
                "Αποσύρθηκε από τον φοιτητή" => "lightyellow",
                _ => "transparent"
            };
        }
        return "transparent";
    }


    private async Task WithdrawThesisApplication(object thesis)
    {
        try
        {
            PlatformActions platformAction = null;

            if (thesis is CompanyThesisApplied companyThesis)
            {
                var confirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", 
                    $"Πρόκεται να αποσύρετε την Αίτησή σας για την Πτυχιακή Εργασία. Είστε σίγουρος/η;");
                if (!confirmed) return;

                var thesisDetails = await dbContext.CompanyTheses
                    .Include(t => t.Company)
                    .FirstOrDefaultAsync(t => t.RNGForThesisUploadedAsCompany == companyThesis.RNGForCompanyThesisApplied);

                if (thesisDetails == null)
                {
                    await JS.InvokeVoidAsync("alert", "Δεν βρέθηκε η πτυχιακή εργασία.");
                    return;
                }

                companyThesis.CompanyThesisStatusAppliedAtStudentSide = "Αποσύρθηκε από τον φοιτητή";
                companyThesis.CompanyThesisStatusAppliedAtCompanySide = "Αποσύρθηκε από τον φοιτητή";

                platformAction = new PlatformActions
                {
                    UserRole_PerformedAction = "STUDENT",
                    ForWhat_PerformedAction = "COMPANY_THESIS",
                    HashedPositionRNG_PerformedAction = HashingHelper.HashLong(companyThesis.RNGForCompanyThesisApplied),
                    TypeOfAction_PerformedAction = "SELFWITHDRAW",
                    DateTime_PerformedAction = DateTime.UtcNow
                };

                dbContext.PlatformActions.Add(platformAction);
                await dbContext.SaveChangesAsync();

                var student = await GetStudentDetails(companyThesis.StudentEmailAppliedForThesis);
                if (student == null)
                {
                    await JS.InvokeVoidAsync("alert", "Δεν βρέθηκαν στοιχεία φοιτητή.");
                    return;
                }

                var companyName = thesisDetails.Company?.CompanyName ?? "Άγνωστη Εταιρεία";

                await InternshipEmailService.SendStudentThesisWithdrawalNotificationToCompanyOrProfessor(
                    companyThesis.CompanyEmailWhereStudentAppliedForThesis,
                    companyName,
                    student.Name,
                    student.Surname,
                    thesisDetails.CompanyThesisTitle,
                    companyThesis.RNGForCompanyThesisAppliedAsStudent_HashedAsUniqueID);

                await InternshipEmailService.SendStudentThesisWithdrawalConfirmationToStudent(
                    companyThesis.StudentEmailAppliedForThesis,
                    student.Name,
                    student.Surname,
                    thesisDetails.CompanyThesisTitle,
                    companyThesis.RNGForCompanyThesisAppliedAsStudent_HashedAsUniqueID,
                    companyName);

                NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
            }
            else if (thesis is ProfessorThesisApplied professorThesis)
            {
                var confirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML", 
                    $"Πρόκεται να αποσύρετε την Αίτησή σας για την Πτυχιακή Εργασία. Είστε σίγουρος/η;");
                if (!confirmed) return;

                var thesisDetails = await dbContext.ProfessorTheses
                    .Include(t => t.Professor)
                    .FirstOrDefaultAsync(t => t.RNGForThesisUploaded == professorThesis.RNGForProfessorThesisApplied);

                if (thesisDetails == null)
                {
                    await JS.InvokeVoidAsync("alert", "Δεν βρέθηκε η πτυχιακή εργασία.");
                    return;
                }

                professorThesis.ProfessorThesisStatusAppliedAtStudentSide = "Αποσύρθηκε από τον φοιτητή";
                professorThesis.ProfessorThesisStatusAppliedAtProfessorSide = "Αποσύρθηκε από τον φοιτητή";

                platformAction = new PlatformActions
                {
                    UserRole_PerformedAction = "STUDENT",
                    ForWhat_PerformedAction = "PROFESSOR_THESIS",
                    HashedPositionRNG_PerformedAction = HashingHelper.HashLong(professorThesis.RNGForProfessorThesisApplied),
                    TypeOfAction_PerformedAction = "SELFWITHDRAW",
                    DateTime_PerformedAction = DateTime.UtcNow
                };

                dbContext.PlatformActions.Add(platformAction);
                await dbContext.SaveChangesAsync();

                var student = await GetStudentDetails(professorThesis.StudentEmailAppliedForProfessorThesis);
                if (student == null)
                {
                    await JS.InvokeVoidAsync("alert", "Δεν βρέθηκαν στοιχεία φοιτητή.");
                    return;
                }

                var professorName = thesisDetails.Professor != null 
                    ? $"{thesisDetails.Professor.ProfName} {thesisDetails.Professor.ProfSurname}" 
                    : "Άγνωστος Καθηγητής";

                await InternshipEmailService.SendStudentThesisWithdrawalNotificationToCompanyOrProfessor(
                    professorThesis.ProfessorEmailWhereStudentAppliedForProfessorThesis,
                    professorName,
                    student.Name,
                    student.Surname,
                    thesisDetails.ThesisTitle,
                    professorThesis.RNGForProfessorThesisApplied_HashedAsUniqueID);

                await InternshipEmailService.SendStudentThesisWithdrawalConfirmationToStudent(
                    professorThesis.StudentEmailAppliedForProfessorThesis,
                    student.Name,
                    student.Surname,
                    thesisDetails.ThesisTitle,
                    professorThesis.RNGForProfessorThesisApplied_HashedAsUniqueID,
                    professorName);

                NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving withdrawal: {ex.Message}");
            await JS.InvokeVoidAsync("alert", "Σφάλμα κατά την αποθήκευση της απόσυρσης.");
        }
    }

    private async Task HandleFileUploadToEditCompanyAnnouncementAttachment(InputFileChangeEventArgs e)
    {
        var file = e.File;  
        if (file != null)
        {
            Console.WriteLine($"File selected: {file.Name}");

            if (file.ContentType == "application/pdf")
            {
                using (var stream = file.OpenReadStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);  
                        currentAnnouncement.CompanyAnnouncementAttachmentFile = memoryStream.ToArray();  
                        Console.WriteLine($"File uploaded: {file.Name}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Selected file is not a PDF.");
            }
        }
        else
        {
            Console.WriteLine("No file selected.");
        }
    }

    private async Task HandleFileUploadToEditProfessorAnnouncementAttachment(InputFileChangeEventArgs e)
    {
        var file = e.File;  // Access the selected file
        if (file != null)
        {
            Console.WriteLine($"File selected: {file.Name}");

            // Ensure the file is a PDF (optional)
            if (file.ContentType == "application/pdf")
            {
                using (var stream = file.OpenReadStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);  // Copy the file stream to memory stream
                        currentAnnouncementAsProfessor.ProfessorAnnouncementAttachmentFile = memoryStream.ToArray();  // Store file as byte array
                        Console.WriteLine($"File uploaded: {file.Name}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Selected file is not a PDF.");
            }
        }
        else
        {
            Console.WriteLine("No file selected.");
        }
    }


    private async Task HandleFileUploadToEditCompanyJobAttachment(InputFileChangeEventArgs e)
    {
        Console.WriteLine("File upload method triggered.");

        var file = e.File;
        if (file != null)
        {
            Console.WriteLine($"File selected: {file.Name}");

            // Ensure the file is a PDF
            if (file.ContentType == "application/pdf")
            {
                using (var stream = file.OpenReadStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        selectedJob.PositionAttachment = memoryStream.ToArray();  // Store the file as byte array
                        Console.WriteLine($"File uploaded: {file.Name}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Selected file is not a PDF.");
            }
        }
        else
        {
            Console.WriteLine("No file selected.");
        }
    }

    private async Task HandleFileUploadToEditCompanyInternshipAttachment(InputFileChangeEventArgs e)
    {
        Console.WriteLine("File upload method triggered.");

        var file = e.File;
        if (file != null)
        {
            Console.WriteLine($"File selected: {file.Name}");

            // Ensure the file is a PDF
            if (file.ContentType == "application/pdf")
            {
                using (var stream = file.OpenReadStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        selectedInternship.CompanyInternshipAttachment = memoryStream.ToArray();  // Store the file as byte array
                        Console.WriteLine($"File uploaded: {file.Name}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Selected file is not a PDF.");
            }
        }
        else
        {
            Console.WriteLine("No file selected.");
        }
    }

    private void OnCheckedChangedForEditCompanyJobAreas(ChangeEventArgs e, Area area)
    {
        if (e.Value is bool isChecked)
        {
            if (isChecked)
            {
                if (!SelectedAreasToEditForCompanyJob.Any(a => a.AreaName == area.AreaName))  // Check by AreaName or Id
                {
                    SelectedAreasToEditForCompanyJob.Add(area);  // Add the area object
                }
            }
            else
            {
                SelectedAreasToEditForCompanyJob.RemoveAll(a => a.AreaName == area.AreaName);  // Remove by AreaName or Id
            }
        }
        StateHasChanged();
    }

    private void OnCheckedChangedForEditCompanyInternshipAreas(ChangeEventArgs e, Area area)
    {
        if (e.Value is bool isChecked)
        {
            if (isChecked)
            {
                if (!SelectedAreasToEditForCompanyInternship.Any(a => a.AreaName == area.AreaName))  // Check by AreaName or Id
                {
                    SelectedAreasToEditForCompanyInternship.Add(area);  // Add the area object
                }
            }
            else
            {
                SelectedAreasToEditForCompanyInternship.RemoveAll(a => a.AreaName == area.AreaName);  // Remove by AreaName or Id
            }
        }
        StateHasChanged();
    }

    private void OnCheckedChangedForEditCompanyThesisAreas(ChangeEventArgs e, Area area)
    {
        if (e.Value is bool isChecked)
        {
            if (isChecked)
            {
                if (!SelectedAreasToEditForCompanyThesis.Any(a => a.AreaName == area.AreaName))  // Check by AreaName or Id
                {
                    SelectedAreasToEditForCompanyThesis.Add(area);  // Add the area object
                }
            }
            else
            {
                SelectedAreasToEditForCompanyThesis.RemoveAll(a => a.AreaName == area.AreaName);  // Remove by AreaName or Id
            }
        }
        StateHasChanged();
    }

    private void OnCheckedChangedForEditCompanyThesisSkills(ChangeEventArgs e, Skill skill)
    {
        if (e.Value is bool isChecked)
        {
            if (isChecked)
            {
                if (!SelectedSkillsToEditForCompanyThesis.Any(s => s.SkillName == skill.SkillName))  // Check by SkillName or Id
                {
                    SelectedSkillsToEditForCompanyThesis.Add(skill);  // Add the skill object
                }
            }
            else
            {
                SelectedSkillsToEditForCompanyThesis.RemoveAll(s => s.SkillName == skill.SkillName);  // Remove by SkillName or Id
            }
        }
        StateHasChanged();
    }

    private async Task ShowProfessorDetailsAtCompanyEventInterest(InterestInCompanyEventAsProfessor interest)
    {
        try
        {
            // Try to get professor from cache first
            if (!professorDataCache.TryGetValue(interest.ProfessorEmailShowInterestForCompanyEvent, out var professor))
            {
                // If not in cache, fetch from database
                professor = await dbContext.Professors
                    .FirstOrDefaultAsync(p => p.ProfEmail == interest.ProfessorEmailShowInterestForCompanyEvent);
            
                if (professor != null)
                {
                    professorDataCache[interest.ProfessorEmailShowInterestForCompanyEvent] = professor;
                }
            }

            if (professor == null)
            {
                await JS.InvokeVoidAsync("alert", "Professor details not found");
                return;
            }

            selectedProfessorToShowDetailsForInterestinCompanyEvent = professor;
            showProfessorModal = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error showing professor details: {ex.Message}");
            await JS.InvokeVoidAsync("alert", "Σφάλμα κατά την εμφάνιση των στοιχείων του καθηγητή");
        }
        finally
        {
            StateHasChanged();
        }
    }

    


    private void CloseProfessorDetailsModal()
    {
        showProfessorModal = false;
    }

    private async Task UploadFileToUpdateCompanyEventAttachment(ChangeEventArgs e)
    {
        var file = (IBrowserFile)e.Value;
        var buffer = new byte[file.Size];
        await file.OpenReadStream().ReadAsync(buffer, 0, (int)file.Size);
        currentCompanyEvent.CompanyEventAttachmentFile = buffer;
    }

    

    private void OnCheckedChangedForEditCompanyEventAreas(ChangeEventArgs e, Area area)
    {
        if (e.Value is bool isChecked)
        {
            if (isChecked)
            {
                if (!SelectedAreasToEditForCompanyEvent.Any(a => a.AreaName == area.AreaName))  // Check by AreaName or Id
                {
                    SelectedAreasToEditForCompanyEvent.Add(area);  // Add the area object
                }
            }
            else
            {
                SelectedAreasToEditForCompanyEvent.RemoveAll(a => a.AreaName == area.AreaName);  // Remove by AreaName or Id
            }
        }
        StateHasChanged();
    }


    private void UpdateSelectedAreasForCompanyEvent()
    {
        currentCompanyEvent.CompanyEventAreasOfInterest = string.Join(", ", SelectedAreasToEditForCompanyEvent.Select(a => a.AreaName));
    }

    private void OnRegionChangedForEditCompanyEvent(string selectedRegion)
    {
        AvailableTownsForEditCompanyEvent = GetTownsForRegionForEditCompanyEvent(selectedRegion);
        currentCompanyEvent.CompanyEventDimosLocation = ""; // Reset town when region changes
    }




    private List<string> AvailableTownsForEditCompanyEvent = new List<string>();

    private List<string> GetTownsForRegionForEditCompanyEvent(string region)
    {
        if (string.IsNullOrEmpty(region) || !RegionToTownsMap.ContainsKey(region))
        {
            return new List<string>();
        }

        return RegionToTownsMap[region];
    }

    private string _selectedRegion;

    private string SelectedRegion
    {
        get => _selectedRegion;
        set
        {
            _selectedRegion = value;
            OnRegionChangedForEditCompanyEvent(value);
        }
    }


    private void ClearField(int fieldIndex)
    {
        switch (fieldIndex)
        {
            case 1:
                currentCompanyEvent.CompanyEventStartingPointLocationToTransportPeopleToEvent1 = string.Empty;
                break;
            case 2:
                currentCompanyEvent.CompanyEventStartingPointLocationToTransportPeopleToEvent2 = string.Empty;
                break;
            case 3:
                currentCompanyEvent.CompanyEventStartingPointLocationToTransportPeopleToEvent3 = string.Empty;
                break;
        }
    }

    private async Task DownloadStudentListForInterestInCompanyEventAsCompany()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Interested Students");

        worksheet.Cells["A1"].Value = "Πανεπιστήμιο";
        worksheet.Cells["B1"].Value = "Τμήμα Φοίτησης";
        worksheet.Cells["C1"].Value = "Επίπεδο Σπουδών";
        worksheet.Cells["D1"].Value = "Όνομα Φοιτητή";
        worksheet.Cells["E1"].Value = "Επώνυμο Φοιτητή";
        worksheet.Cells["F1"].Value = "Email";
        worksheet.Cells["G1"].Value = "Τηλέφωνο";
        worksheet.Cells["H1"].Value = "Χρειάζεται Μεταφορά";
        worksheet.Cells["I1"].Value = "Επιλεγμένα Σημεία Εκκίνησης";

        using (var headerRange = worksheet.Cells["A1:I1"])
        {
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        int row = 2;

        foreach (var application in InterestedStudents)
        {
            var studentUniqueId = application.StudentUniqueIDShowInterestForEvent;
            var student = studentDataCache.Values.FirstOrDefault(s => s.Student_UniqueID == studentUniqueId);

            if (student == null)
            {
                student = await dbContext.Students
                    .Where(s => s.Student_UniqueID == studentUniqueId)
                    .Select(s => new Student
                    {
                        Name = s.Name,
                        Surname = s.Surname,
                        Email = s.Email,
                        Telephone = s.Telephone,
                        University = s.University,
                        Department = s.Department,
                        LevelOfDegree = s.LevelOfDegree
                    })
                    .FirstOrDefaultAsync();

                if (student != null)
                {
                    studentDataCache[student.Email.ToLower()] = student;
                }
            }

            if (student != null)
            {
                worksheet.Cells[$"A{row}"].Value = student.University;
                worksheet.Cells[$"B{row}"].Value = student.Department;
                worksheet.Cells[$"C{row}"].Value = student.LevelOfDegree;
                worksheet.Cells[$"D{row}"].Value = student.Name;
                worksheet.Cells[$"E{row}"].Value = student.Surname;
                worksheet.Cells[$"F{row}"].Value = student.Email;
                worksheet.Cells[$"G{row}"].Value = student.Telephone;
                worksheet.Cells[$"H{row}"].Value = string.IsNullOrWhiteSpace(application.StudentTransportNeedWhenShowInterestForCompanyEvent)
                                                   ? "Όχι"
                                                   : application.StudentTransportNeedWhenShowInterestForCompanyEvent;
                worksheet.Cells[$"I{row}"].Value = string.IsNullOrWhiteSpace(application.StudentTransportChosenLocationWhenShowInterestForCompanyEvent)
                                                   ? "N/A"
                                                   : application.StudentTransportChosenLocationWhenShowInterestForCompanyEvent;

                row++;
            }
        }

        worksheet.Cells.AutoFitColumns();

        var fileBytes = package.GetAsByteArray();
        string fileName = "Ενδιαφερόμενοι_Φοιτητές_Εκδήλωσης.xlsx";
        await JS.InvokeVoidAsync("saveStudentShownInterestForCompanyEventAsExcelListFile", fileName, Convert.ToBase64String(fileBytes));
    }


    private async Task DownloadStudentListForInterestInProfessorEventAsProfessor()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Interested Students");

        // Set column headers
        worksheet.Cells["A1"].Value = "Πανεπιστήμιο";
        worksheet.Cells["B1"].Value = "Τμήμα Φοίτησης";
        worksheet.Cells["C1"].Value = "Επίπεδο Σπουδών";
        worksheet.Cells["D1"].Value = "Όνομα Φοιτητή";
        worksheet.Cells["E1"].Value = "Επώνυμο Φοιτητή";
        worksheet.Cells["F1"].Value = "Email";
        worksheet.Cells["G1"].Value = "Τηλέφωνο";
        worksheet.Cells["H1"].Value = "Χρειάζεται Μεταφορά";
        worksheet.Cells["I1"].Value = "Επιλεγμένα Σημεία Εκκίνησης";

        // Format headers
        using (var headerRange = worksheet.Cells["A1:I1"])
        {
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        int row = 2;

        foreach (var application in InterestedStudentsForProfessorEvent)
        {
            var studentUniqueId = application.StudentUniqueIDShowInterestForEvent;
            var student = studentDataCache.Values.FirstOrDefault(s => s.Student_UniqueID == studentUniqueId);

            if (student == null)
            {
                student = await dbContext.Students
                    .Where(s => s.Student_UniqueID == studentUniqueId)
                    .Select(s => new Student
                    {
                        Name = s.Name,
                        Surname = s.Surname,
                        Email = s.Email,
                        Telephone = s.Telephone,
                        University = s.University,
                        Department = s.Department,
                        LevelOfDegree = s.LevelOfDegree
                    })
                    .FirstOrDefaultAsync();

                if (student != null)
                {
                    studentDataCache[student.Email.ToLower()] = student;
                }
            }

            if (student != null)
            {
                worksheet.Cells[$"A{row}"].Value = student.University;
                worksheet.Cells[$"B{row}"].Value = student.Department;
                worksheet.Cells[$"C{row}"].Value = student.LevelOfDegree;
                worksheet.Cells[$"D{row}"].Value = student.Name;
                worksheet.Cells[$"E{row}"].Value = student.Surname;
                worksheet.Cells[$"F{row}"].Value = student.Email;
                worksheet.Cells[$"G{row}"].Value = student.Telephone;
                worksheet.Cells[$"H{row}"].Value = string.IsNullOrWhiteSpace(application.StudentTransportNeedWhenShowInterestForProfessorEvent)
                                                   ? "Όχι"
                                                   : application.StudentTransportNeedWhenShowInterestForProfessorEvent;
                worksheet.Cells[$"I{row}"].Value = string.IsNullOrWhiteSpace(application.StudentTransportChosenLocationWhenShowInterestForProfessorEvent)
                                                   ? "N/A"
                                                   : application.StudentTransportChosenLocationWhenShowInterestForProfessorEvent;

                row++;
            }
        }

        worksheet.Cells.AutoFitColumns();

        var fileBytes = package.GetAsByteArray();
        string fileName = "Ενδιαφερόμενοι_Φοιτητές_Εκδήλωσης.xlsx";
        await JS.InvokeVoidAsync("saveStudentShownInterestForProfessorEventAsExcelListFile", fileName, Convert.ToBase64String(fileBytes));
    }

    private async Task DownloadProfessorListForInterestInCompanyEventAsCompany()
    {
        // 🔹 Set the License Context BEFORE using ExcelPackage
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;  

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Interested Professors");

        // 🔹 Set Column Headers
        worksheet.Cells["A1"].Value = "Όνομα";
        worksheet.Cells["B1"].Value = "Επώνυμο";
        worksheet.Cells["C1"].Value = "Πανεπιστήμιο";
        worksheet.Cells["D1"].Value = "Τμήμα";
        worksheet.Cells["E1"].Value = "Βαθμίδα ΔΕΠ";
        worksheet.Cells["F1"].Value = "Email";
        worksheet.Cells["G1"].Value = "Τηλέφωνο Εργασίας";
        worksheet.Cells["H1"].Value = "Τηλέφωνο Προσωπικό";
        worksheet.Cells["I1"].Value = "Ημερομηνία Δήλωσης";
        worksheet.Cells["J1"].Value = "Κατάσταση";

        // 🔹 Apply Bold Style to Column Titles
        using (var headerRange = worksheet.Cells["A1:J1"])
        {
            headerRange.Style.Font.Bold = true; 
            headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray); 
            headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; 
        }

        int row = 2; // Start inserting data from row 2

        foreach (var interest in filteredProfessorInterestForCompanyEvents)
        {
            // Get professor details from the database
            var professor = await GetProfessorDetails(interest.ProfessorEmailShowInterestForCompanyEvent);
    
            worksheet.Cells[$"A{row}"].Value = professor?.ProfName ?? "N/A";
            worksheet.Cells[$"B{row}"].Value = professor?.ProfSurname ?? "N/A";
            worksheet.Cells[$"C{row}"].Value = professor?.ProfUniversity ?? "N/A";
            worksheet.Cells[$"D{row}"].Value = professor?.ProfDepartment ?? "N/A";
            worksheet.Cells[$"E{row}"].Value = professor?.ProfVahmidaDEP ?? "N/A";
            worksheet.Cells[$"F{row}"].Value = interest.ProfessorEmailShowInterestForCompanyEvent;
            worksheet.Cells[$"G{row}"].Value = professor?.ProfWorkTelephone ?? "N/A";
            worksheet.Cells[$"H{row}"].Value = professor?.ProfPersonalTelephoneVisibility == true ? professor?.ProfPersonalTelephone ?? "N/A" : "Μη Δημόσιο";
            worksheet.Cells[$"I{row}"].Value = interest.DateTimeProfessorShowInterestForCompanyEvent.ToString("g");
            worksheet.Cells[$"J{row}"].Value = interest.CompanyEventStatus_ShowInterestAsProfessor_AtCompanySide;
            row++;
        }

        // 🔹 AutoFit Columns for better readability
        worksheet.Cells.AutoFitColumns();

        // 🔹 Format date column
        worksheet.Column(9).Style.Numberformat.Format = "dd/MM/yyyy HH:mm";

        var fileBytes = package.GetAsByteArray();

        // 🔹 Call JavaScript to trigger download
        string fileName = $"Ενδιαφερόμενοι_Καθηγητές_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
        await JS.InvokeVoidAsync("saveProfessorShownInterestForCompanyEventAsExcelListFile", fileName, Convert.ToBase64String(fileBytes));
    }

    private async Task DownloadCompanyListForInterestInProfessorEventAsProfessor()
    {
        // 🔹 Set the License Context BEFORE using ExcelPackage
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;  

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Interested Companies");

        // 🔹 Set Column Headers
        worksheet.Cells["A1"].Value = "Επωνυμία";
        worksheet.Cells["B1"].Value = "Τοποθεσία (Πόλη)";
        worksheet.Cells["C1"].Value = "Διεύθυνση";
        worksheet.Cells["D1"].Value = "Email Επικοινωνίας";
        worksheet.Cells["E1"].Value = "Τηλέφωνο Επικοινωνίας";
        worksheet.Cells["F1"].Value = "Τομείς Ενδιαφέροντος";
        worksheet.Cells["G1"].Value = "Αριθμός Συμμετεχόντων";
        worksheet.Cells["H1"].Value = "Ημερομηνία Δήλωσης";
        worksheet.Cells["I1"].Value = "Κατάσταση";

        // 🔹 Apply Bold Style to Column Titles
        using (var headerRange = worksheet.Cells["A1:I1"])
        {
            headerRange.Style.Font.Bold = true; 
            headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray); 
            headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; 
        }

        int row = 2; // Start inserting data from row 2

        foreach (var interest in filteredCompanyInterestForProfessorEvents)
        {
            // Get company details from the navigation property
            var company = await GetCompanyDetails(interest.CompanyEmailShowInterestForProfessorEvent);
        
            worksheet.Cells[$"A{row}"].Value = company?.CompanyName ?? "N/A";
            worksheet.Cells[$"B{row}"].Value = company?.CompanyTown ?? "N/A";
            worksheet.Cells[$"C{row}"].Value = company?.CompanyLocation ?? "N/A";
            worksheet.Cells[$"D{row}"].Value = interest.CompanyEmailShowInterestForProfessorEvent;
            worksheet.Cells[$"E{row}"].Value = company?.CompanyTelephone ?? "N/A";
            worksheet.Cells[$"F{row}"].Value = company?.CompanyAreas ?? "N/A";
            worksheet.Cells[$"G{row}"].Value = interest.CompanyNumberOfPeopleToShowUpWhenShowInterestForProfessorEvent;
            worksheet.Cells[$"H{row}"].Value = interest.DateTimeCompanyShowInterestForProfessorEvent.ToString("g");
            worksheet.Cells[$"I{row}"].Value = interest.ProfessorEventStatus_ShowInterestAsCompany_AtProfessorSide;
        
            row++;
        }

        // 🔹 AutoFit Columns for better readability
        worksheet.Cells.AutoFitColumns();

        // 🔹 Format date column
        worksheet.Column(8).Style.Numberformat.Format = "dd/MM/yyyy HH:mm";

        var fileBytes = package.GetAsByteArray();

        // 🔹 Call JavaScript to trigger download
        string fileName = $"Ενδιαφερόμενες_Εταιρείες_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
        await JS.InvokeVoidAsync("saveCompanyShownInterestForProfessorEventAsExcelListFile", fileName, Convert.ToBase64String(fileBytes));
    }

    private void HandleNumberChangeForParticipantsWhenShowInterestAsACompanyForAProfessorEvent(ChangeEventArgs e, long professorEventId)
    {
        if (int.TryParse(e.Value?.ToString(), out int number))
        {
            SetNumberOfPeople(professorEventId, number);
        }
        else
        {
            Console.WriteLine("Invalid number entered");
        }
    }

    private void ToggleThesisApplicationsVisibility()
    {
        isThesisApplicationsVisible = !isThesisApplicationsVisible;
        StateHasChanged();
    }

    private void ToggleAnnouncementsAsStudentVisibility()
    {
        isAnnouncementsAsStudentVisible = !isAnnouncementsAsStudentVisible;
        StateHasChanged();
    }

    private void ToggleAnnouncementsAsRGVisibility()
    {
        isAnnouncementsAsRGVisible = !isAnnouncementsAsRGVisible;
        StateHasChanged();
    }

    private void ToggleAnnouncementsAsProfessorVisibility()
    {
        isAnnouncementsAsProfessorVisible = !isAnnouncementsAsProfessorVisible;
        StateHasChanged();
    }

    private void ToggleSearchInternshipsAsStudentFiltersVisibility()
    {
        isSearchInternshipsAsStudentFiltersVisible = !isSearchInternshipsAsStudentFiltersVisible;
        StateHasChanged();
    }

    private void ToggleJobApplicationsAsStudentVisibility()
    {
        isJobApplicationsAsStudentVisible = !isJobApplicationsAsStudentVisible;
        StateHasChanged();
    }

    private void ToggleJobPositionAsStudentFiltersVisibility()
    {
        isJobPositionAsStudentFiltersVisible = !isJobPositionAsStudentFiltersVisible;
        StateHasChanged();
    }

    private void ToggleInternshipApplicationsAsStudentVisibility()
    {
        isInternshipApplicationsAsStudentVisible = !isInternshipApplicationsAsStudentVisible;
        StateHasChanged();
    }

    private void ToggleInternshipSearchAsStudentFiltersVisibility()
    {
        isInternshipSearchAsStudentFiltersVisible = !isInternshipSearchAsStudentFiltersVisible;
        StateHasChanged();
    }

    private void ToggleEventSearchAsStudentVisibility()
    {
        isEventSearchAsStudentVisible = !isEventSearchAsStudentVisible;
        StateHasChanged();
    }

    
    private async Task HandleThesisTitleInputForBothCompaniesAndProfessorsWhenSearchForThesisAsStudent(ChangeEventArgs e)
    {
        thesisSearchForThesesAsStudent = e.Value?.ToString();

        if (!string.IsNullOrWhiteSpace(thesisSearchForThesesAsStudent) && 
            thesisSearchForThesesAsStudent.Length >= 2)
        {
            try 
            {
                // Clear ChangeTracker to prevent conflicts
                dbContext.ChangeTracker.Clear();

                var professorTitles = await dbContext.ProfessorTheses
                    .AsNoTracking()
                    .Where(t => EF.Functions.Like(t.ThesisTitle, $"%{thesisSearchForThesesAsStudent}%"))
                    .Select(t => t.ThesisTitle)
                    .Distinct()
                    .Take(5)
                    .ToListAsync();

                var companyTitles = await dbContext.CompanyTheses
                    .AsNoTracking()
                    .Where(t => EF.Functions.Like(t.CompanyThesisTitle, $"%{thesisSearchForThesesAsStudent}%"))
                    .Select(t => t.CompanyThesisTitle)
                    .Distinct()
                    .Take(5)
                    .ToListAsync();

                // Combine and order the results
                thesisTitleSuggestions = professorTitles
                    .Concat(companyTitles)
                    .Distinct()
                    .OrderBy(t => t)
                    .Take(10) 
                    .ToList();
            }
            catch (Exception ex)
            {
                // Log error if needed
                Console.WriteLine($"Error fetching suggestions: {ex.Message}");
                thesisTitleSuggestions.Clear();
            }
        }
        else
        {
            thesisTitleSuggestions.Clear();
        }

        StateHasChanged();
    }

    private void SelectThesisTitleSuggestionForBothCompaniesAndProfessorsWhenSearchForThesisAsStudent(string suggestion)
    {
        thesisSearchForThesesAsStudent = suggestion;
        thesisTitleSuggestions.Clear();
        StateHasChanged();
    }

    private async Task HandleCompanyNameInputWhenSearchForProfessorThesisAutocompleteNameAsStudent(ChangeEventArgs e)
    {
        companyNameSearchForThesesAsStudent = e.Value?.ToString();

        if (!string.IsNullOrWhiteSpace(companyNameSearchForThesesAsStudent) && 
            companyNameSearchForThesesAsStudent.Length >= 2)
        {
            companyNameSuggestionsWhenSearchForProfessorThesisAutocompleteNameAsStudent = await dbContext.CompanyTheses
                .Include(t => t.Company) // Include Company for navigation property access
                .Where(t => t.Company != null && 
                           EF.Functions.Like(t.Company.CompanyName, $"%{companyNameSearchForThesesAsStudent}%"))
                .Select(t => t.Company.CompanyName) // Get name from navigation property
                .Distinct()
                .Take(10)
                .ToListAsync();
        }
        else
        {
            companyNameSuggestionsWhenSearchForProfessorThesisAutocompleteNameAsStudent.Clear();
        }

        StateHasChanged();
    }



    private async Task SelectCompanyNameSuggestionWhenSearchForProfessorThesisAutocompleteNameAsStudent(string suggestion)
    {
        companyNameSearchForThesesAsStudent = suggestion;
        companyNameSuggestionsWhenSearchForProfessorThesisAutocompleteNameAsStudent.Clear();
        StateHasChanged();
    }

    private async Task HandleJobTitleAutocompleteInputWhenSearchCompanyJobsAsStudent(ChangeEventArgs e)
    {
        jobSearch = e.Value?.ToString();

        if (!string.IsNullOrWhiteSpace(jobSearch) && jobSearch.Length >= 2)
        {
            jobTitleAutocompleteSuggestionsWhenSearchForCompanyJobsAsStudent = await dbContext.CompanyJobs
                .AsNoTracking()
                .Where(j => EF.Functions.Like(j.PositionTitle, $"%{jobSearch}%"))
                .Select(j => j.PositionTitle)
                .Distinct()
                .OrderBy(t => t)
                .Take(10)
                .ToListAsync();
        }
        else
        {
            jobTitleAutocompleteSuggestionsWhenSearchForCompanyJobsAsStudent.Clear();
        }

        StateHasChanged();
    }


    private async Task SelectJobTitleAutocompleteSuggestionWhenSearchCompanyJobsAsStudent(string suggestion)
    {
        jobSearch = suggestion;
        jobTitleAutocompleteSuggestionsWhenSearchForCompanyJobsAsStudent.Clear();
        StateHasChanged();
    }

    private async Task HandleCompanyNameAutocompleteInputWhenSearchCompanyJobsAsStudent(ChangeEventArgs e)
    {
        companyNameSearch = e.Value?.ToString();

        if (!string.IsNullOrWhiteSpace(companyNameSearch) && companyNameSearch.Length >= 2)
        {
            companyNameAutocompleteSuggestionsWhenSearchForCompanyJobsAsStudent = await dbContext.CompanyJobs
                .Include(j => j.Company) // Include the Company navigation property
                .Where(j => j.Company != null && 
                       EF.Functions.Like(j.Company.CompanyName, $"%{companyNameSearch}%"))
                .Select(j => j.Company.CompanyName)
                .Distinct()
                .OrderBy(c => c)
                .Take(10)
                .ToListAsync();
        }
        else
        {
            companyNameAutocompleteSuggestionsWhenSearchForCompanyJobsAsStudent.Clear();
        }

        StateHasChanged();
    }

    private async Task SelectCompanyNameAutocompleteSuggestionWhenSearchCompanyJobsAsStudent(string suggestion)
    {
        companyNameSearch = suggestion;
        companyNameAutocompleteSuggestionsWhenSearchForCompanyJobsAsStudent.Clear();
        StateHasChanged();
    }

    private async Task HandleInternshipTitleAutocompleteInputWhenSearchInternshipAsStudent(ChangeEventArgs e)
    {
        companyinternshipSearch = e.Value?.ToString();

        if (!string.IsNullOrWhiteSpace(companyinternshipSearch) && 
            companyinternshipSearch.Length >= 2)
        {
            try 
            {
                // Clear ChangeTracker to prevent conflicts
                dbContext.ChangeTracker.Clear();

                var searchTerm = $"%{companyinternshipSearch}%";
        
                var companyTitles = await dbContext.CompanyInternships
                    .AsNoTracking()
                    .Where(i => EF.Functions.Like(i.CompanyInternshipTitle, searchTerm))
                    .Select(i => i.CompanyInternshipTitle)
                    .Distinct()
                    .Take(5)
                    .ToListAsync();

                var professorTitles = await dbContext.ProfessorInternships
                    .AsNoTracking()
                    .Where(i => EF.Functions.Like(i.ProfessorInternshipTitle, searchTerm))
                    .Select(i => i.ProfessorInternshipTitle)
                    .Distinct()
                    .Take(5)
                    .ToListAsync();

                // Combine and order the results
                internshipTitleAutocompleteSuggestionsWhenSearchInternshipAsStudent = companyTitles
                    .Concat(professorTitles)
                    .Distinct()
                    .OrderBy(t => t)
                    .Take(10)
                    .ToList();
            }
            catch (Exception ex)
            {
                // Log error if needed
                Console.WriteLine($"Error fetching internship suggestions: {ex.Message}");
                internshipTitleAutocompleteSuggestionsWhenSearchInternshipAsStudent.Clear();
            }
        }
        else
        {
            internshipTitleAutocompleteSuggestionsWhenSearchInternshipAsStudent.Clear();
        }

        StateHasChanged();
    }

    private void SelectInternshipTitleAutocompleteSuggestionWhenSearchInternshipAsStudent(string suggestion)
    {
        companyinternshipSearch = suggestion;
        internshipTitleAutocompleteSuggestionsWhenSearchInternshipAsStudent.Clear();
        StateHasChanged();
    }


    public class NewsArticle
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Date { get; set; }
        public string Category { get; set; }
    }

    public class WeatherData
    {
        public int is_day { get; set; }
        public double temp_c { get; set; }
    }

    public class ConditionInfo
    {
        public string Text { get; set; }
    }
    public class WeatherResponse
    {
        public WeatherData current { get; set; }
    }

    private void ToggleDescriptionForCompanyEvent(int companyeventId)
    {
        if (expandedCompanyEventId == companyeventId)
        {
            expandedCompanyEventId = -1;
            Console.WriteLine($"Collapsed event {companyeventId}");
        }
        else
        {
            expandedCompanyEventId = companyeventId;
            Console.WriteLine($"Expanded event {companyeventId}");
        }
    }

    private void ToggleDescriptionForProfessorEvent(int professoreventId)
    {
        if (expandedProfessorEventId == professoreventId)
        {
            expandedProfessorEventId = -1;
            Console.WriteLine($"Collapsed event {professoreventId}");
        }
        else
        {
            expandedProfessorEventId = professoreventId;
            Console.WriteLine($"Expanded event {professoreventId}");
        }
    }


   
    private async Task<List<AnnouncementAsCompany>> FetchAnnouncementsAsync()
    {
        var announcements = await dbContext.AnnouncementsAsCompany
            .AsNoTracking()
            .Include(a => a.Company)  // This ensures Company data is loaded
            .ToListAsync();
        return announcements;
    }


    private async Task<List<NewsArticle>> FetchNewsArticlesAsync()
    {
        try
        {
            var response = await HttpClient.GetAsync("https://www.uoa.gr/anakoinoseis_kai_ekdiloseis");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(content);

            var articles = new List<NewsArticle>();

            var articleNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'topnews')]");
            if (articleNodes != null)
            {
                // Limit the number of articles to 3
                for (int i = 0; i < Math.Min(articleNodes.Count, 3); i++)
                {
                    var articleNode = articleNodes[i];

                    var titleNode = articleNode.SelectSingleNode(".//h3[@class='article__title']/a");
                    var title = titleNode?.InnerText.Trim();
                    var relativeUrl = titleNode?.Attributes["href"]?.Value;
                    var url = new Uri(new Uri("https://www.uoa.gr"), relativeUrl).ToString(); 

                    var dateNode = articleNode.SelectSingleNode(".//span[@class='article__date']/time");
                    var date = dateNode?.Attributes["datetime"]?.Value;

                    var categoryNode = articleNode.SelectSingleNode(".//span[@class='article__category']/a");
                    var category = categoryNode?.InnerText.Trim();

                    articles.Add(new NewsArticle
                        {
                            Title = title,
                            Url = url,
                            Date = date,
                            Category = category
                        });
                }
            }

            return articles;
        }
        catch (Exception ex)
        {
            fetchError = ex.Message;
            return null;
        }


    }

    private async Task<List<NewsArticle>> FetchSVSENewsArticlesAsync()
    {
        try
        {
            var response = await HttpClient.GetAsync("https://svse.gr/index.php/nea-anakoinoseis");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(content);

            var articles = new List<NewsArticle>();

            var articleNodes = htmlDocument.DocumentNode.SelectNodes("/html/body/div[1]/div/section[2]/div/div/div/main/div/div[3]/div[1]/div/div");

            if (articleNodes != null)
            {
                foreach (var articleNode in articleNodes.Take(3)) // Take only the first 3 articles
                {
                    var titleNode = articleNode.SelectSingleNode(".//h2/a");
                    var title = titleNode?.InnerText.Trim();
                    var relativeUrl = titleNode?.Attributes["href"]?.Value;
                    var url = new Uri(new Uri("https://svse.gr"), relativeUrl).ToString(); 

                    var dateNode = articleNode.SelectSingleNode(".//time");
                    var date = dateNode?.InnerText.Trim();

                    articles.Add(new NewsArticle
                        {
                            Title = title,
                            Url = url,
                            Date = date,
                            Category = "SVSE News" 
                        });
                }
            }
            else
            {
                fetchError = "No articles found with the specified XPath.";
            }

            return articles;
        }
        catch (Exception ex)
        {
            fetchError = ex.Message;
            return null;
        }
    }

    private void ToggleDescription(int announcementId)
    {
        if (expandedAnnouncementId == announcementId)
        {
            expandedAnnouncementId = -1;
        }
        else
        {
            expandedAnnouncementId = announcementId;
        }
    }

    private void ToggleContainer()
    {
        isHidden = !isHidden;
    }

    private async Task DownloadAnnouncementAttachmentFrontPage(byte[] attachmentData, string fileName)
    {
        if (attachmentData != null && attachmentData.Length > 0)
        {
            var mimeType = "application/pdf"; // Correct MIME type for PDF
            await JS.InvokeVoidAsync("BlazorDownloadAttachmentPositionFile", fileName, mimeType, attachmentData);
        }
    }


    private void ChangePage(int pageNumber)
    {
        if (pageNumber >= 1 && pageNumber <= totalPagesForCompanyAnnouncements)
        {
            currentPageForCompanyAnnouncements = pageNumber;
        }
        StateHasChanged();
    }

    private void ToggleDescriptionForProfessorAnnouncements(int announcementId)
    {
        if (expandedProfessorAnnouncementId == announcementId)
        {
            // Collapse if the same announcement is clicked again
            expandedProfessorAnnouncementId = -1;
        }
        else
        {
            // Expand the selected announcement
            expandedProfessorAnnouncementId = announcementId;
        }
    }

    private async Task DownloadProfessorAnnouncementAttachmentFrontPage(byte[] attachmentData, string fileName)
    {
        if (attachmentData != null && attachmentData.Length > 0)
        {
            var mimeType = "application/pdf"; // Correct MIME type for PDF

            // Ensure the file name ends with .pdf
            if (!fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                fileName += ".pdf";
            }

            await JS.InvokeVoidAsync("BlazorDownloadAttachmentPositionFile", fileName, mimeType, attachmentData);
        }
    }



    private void ChangePageForProfessorAnnouncements(int pageNumberForProfessorAnnouncements)
    {
        if (pageNumberForProfessorAnnouncements >= 1 && pageNumberForProfessorAnnouncements <= totalPagesForProfessorAnnouncements)
        {
            currentPageForProfessorAnnouncements = pageNumberForProfessorAnnouncements;
        }
        StateHasChanged();
    }

    private async Task<List<AnnouncementAsProfessor>> FetchProfessorAnnouncementsAsync()
    {
        var professorannouncements = await dbContext.AnnouncementsAsProfessor
            .Include(a => a.Professor)  // Eager load professor data
            .AsNoTracking()
            .OrderByDescending(a => a.ProfessorAnnouncementUploadDate) // Optional: order by date
            .ToListAsync(); 
        return professorannouncements;
    }

    private async Task<List<AnnouncementAsCompany>> FetchCompanyAnnouncementsAsync()
    {
        var companyannouncements = await dbContext.AnnouncementsAsCompany
            .Include(a => a.Company)  // Eager load company data
            .AsNoTracking()
            .OrderByDescending(a => a.CompanyAnnouncementUploadDate) // Optional: order by date
            .ToListAsync();
        return companyannouncements;
    }


    private async Task DownloadCompanyEventAttachmentFrontPage(byte[] attachmentData, string fileName)
    {
        if (attachmentData != null && attachmentData.Length > 0)
        {
            var base64 = Convert.ToBase64String(attachmentData);
            var fileUrl = $"data:application/pdf;base64,{base64}";

            await JS.InvokeVoidAsync("triggerDownload", fileUrl, fileName);
        }
    }

    private async Task DownloadProfessorEventAttachmentFrontPage(byte[] attachmentData, string fileName)
    {
        if (attachmentData != null && attachmentData.Length > 0)
        {
            var base64 = Convert.ToBase64String(attachmentData);
            var fileUrl = $"data:application/pdf;base64,{base64}";

            await JS.InvokeVoidAsync("triggerDownload", fileUrl, fileName);
        }
    }

    private void ChangePageForCompanyEvents(int pageNumberForCompanyEvents)
    {
        if (pageNumberForCompanyEvents >= 1 && pageNumberForCompanyEvents <= totalPagesForCompanyEvents)
        {
            currentCompanyEventPage = pageNumberForCompanyEvents;
        }
        StateHasChanged();
    }

    private void ChangePageForProfessorEvents(int pageNumberForProfessorEvents)
    {
        if (pageNumberForProfessorEvents >= 1 && pageNumberForProfessorEvents <= totalPagesForProfessorEvents)
        {
            currentProfessorEventPage = pageNumberForProfessorEvents;
        }
        StateHasChanged();
    }

    private bool isUniversityNewsVisible = false; // Default to visible
    private void ToggleUniversityNewsVisibility()
    {
        isUniversityNewsVisible = !isUniversityNewsVisible;
        StateHasChanged();
    }

    private bool isSvseNewsVisible = false;
    private void ToggleSvseNewsVisibility()
    {
        isSvseNewsVisible = !isSvseNewsVisible;
        StateHasChanged();
    }

    private bool isCompanyAnnouncementsVisible = false;
    private void ToggleCompanyAnnouncementsVisibility()
    {
        isCompanyAnnouncementsVisible = !isCompanyAnnouncementsVisible;
        StateHasChanged();
    }

    private bool isProfessorAnnouncementsVisible = false;
    private void ToggleProfessorAnnouncementsVisibility()
    {
        isProfessorAnnouncementsVisible = !isProfessorAnnouncementsVisible;
        StateHasChanged();
    }

    private bool isCompanyEventsVisible = false; 
    private void ToggleCompanyEventsVisibility()
    {
        isCompanyEventsVisible = !isCompanyEventsVisible;
        StateHasChanged();
    }

    private bool isProfessorEventsVisible = false;
    private void ToggleProfessorEventsVisibility()
    {
        isProfessorEventsVisible = !isProfessorEventsVisible;
        StateHasChanged();
    }

    private async Task ScrollToElementById(string elementId)
    {
        await JS.InvokeVoidAsync("scrollToElementById", elementId);
    }

    private bool IsTimeInRestrictedRangeWhenUploadEventAsCompany(TimeOnly time)
    {
        // Check if time is before 06:00 or after 22:00
        return time < new TimeOnly(6, 0) || time > new TimeOnly(22, 0);
    }

    private bool HasAtLeastOneStartingPointWhenUploadEventAsCompany()
    {
        return !string.IsNullOrWhiteSpace(companyEvent.CompanyEventStartingPointLocationToTransportPeopleToEvent1) ||
               !string.IsNullOrWhiteSpace(companyEvent.CompanyEventStartingPointLocationToTransportPeopleToEvent2) ||
               !string.IsNullOrWhiteSpace(companyEvent.CompanyEventStartingPointLocationToTransportPeopleToEvent3);
    }


    private async Task HandleAreasInputToFindThesesAsCompany(ChangeEventArgs e)
    {
        searchAreasInputToFindThesesAsCompany = e.Value?.ToString().Trim() ?? string.Empty;
        areaSuggestionsToFindThesesAsCompany = new();

        if (searchAreasInputToFindThesesAsCompany.Length >= 1)
        {
            try
            {
                areaSuggestionsToFindThesesAsCompany = await Task.Run(() =>
                    dbContext.Areas
                        .Where(a => a.AreaName.Contains(searchAreasInputToFindThesesAsCompany))
                        .Select(a => a.AreaName)
                        .Distinct()
                        .Take(10)
                        .ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Σφάλμα κατά την ανάκτηση περιοχών: {ex.Message}");
                areaSuggestionsToFindThesesAsCompany = new List<string>();
            }
        }
        else
        {
            areaSuggestionsToFindThesesAsCompany.Clear();
        }

        StateHasChanged();
    }

    private void SelectAreaSuggestionToFindThesesAsCompany(string suggestion)
    {
        if (!string.IsNullOrWhiteSpace(suggestion) && !selectedAreasToFindThesesAsCompany.Contains(suggestion))
        {
            selectedAreasToFindThesesAsCompany.Add(suggestion);
            areaSuggestionsToFindThesesAsCompany.Clear();
            searchAreasInputToFindThesesAsCompany = string.Empty;
        }
    }

    private void RemoveSelectedAreaToFindThesesAsCompany(string area)
    {
        selectedAreasToFindThesesAsCompany.Remove(area);
        StateHasChanged();
    }

    private async Task DownloadProfessorThesisAttachmentWhenSearchForProfessorThesisAsCompany(int thesisId)
    {
        var thesis = dbContext.ProfessorTheses.FirstOrDefault(t => t.Id == thesisId);
        if (thesis == null)
        {
            Console.WriteLine("Thesis not found.");
            return;
        }

        if (thesis.ThesisAttachment == null)
        {
            Console.WriteLine("No attachment found.");
            return;
        }

        Console.WriteLine($"Attachment found for thesis {thesisId}, size: {thesis.ThesisAttachment.Length} bytes");

        string base64String = Convert.ToBase64String(thesis.ThesisAttachment);
        string fileName = $"Thesis_Attachment_{thesisId}.pdf"; // Adjust file extension if needed

        await JS.InvokeVoidAsync("downloadInternshipAttachmentAsStudent", fileName, base64String); // Reusing the same JS function
    }

    private async Task ScrollToAlertWhenNoJobsFoundWhenSearchAsStudent()
    {
        await JS.InvokeVoidAsync("scrollToElement", "noJobsFoundAlert");
    }
    private async Task ScrollToAlertWhenNoThesesFoundWhenSearchAsStudent()
    {
        await JS.InvokeVoidAsync("scrollToElement", "noThesesFoundAlert");
    }
        private async Task ScrollToAlertWhenNoInternshipsFoundWhenSearchAsStudent()
    {
        await JS.InvokeVoidAsync("scrollToElement", "noInternshipsFoundAlert");
    }


    private async Task ShowProfessorHyperlinkNameDetailsModalInStudentInternship(string professorEmail)
    {
        selectedProfessorDetailsForHyperlinkNameInInternshipAsStudent = await dbContext.Professors
            .FirstOrDefaultAsync(p => p.ProfEmail == professorEmail);

        // Show the modal after fetching the details
        StateHasChanged();
        await JS.InvokeVoidAsync("showProfessorDetailsModal"); // Show the modal using JS
    }

    private async Task ShowCompanyHyperlinkNameDetailsModalInStudentInternship(string companyEmail)
    {
        selectedCompanyDetailsForHyperlinkNameInInternshipAsStudent = await dbContext.Companies
            .FirstOrDefaultAsync(c => c.CompanyEmail == companyEmail);

        if (selectedCompanyDetailsForHyperlinkNameInInternshipAsStudent != null)
        {
            isCompanyDetailsModalOpenForHyperlinkNameAsStudentForCompanyInternship = true;
            StateHasChanged();
            await JS.InvokeVoidAsync("showCompanyDetailsModal"); // Show the modal using JS
        }
    }

    void CloseModalForProfessorNameHyperlinkDetailsInInternship()
    {
        selectedProfessorDetailsForHyperlinkNameInInternshipAsStudent = null;
        StateHasChanged();
        JS.InvokeVoidAsync("hideProfessorDetailsModal");
    }

    void CloseModalForCompanyNameHyperlinkDetailsInInternship()
    {
        isCompanyDetailsModalOpenForHyperlinkNameAsStudentForCompanyInternship = false;
        selectedCompanyDetailsForHyperlinkNameInInternshipAsStudent = null;
        StateHasChanged();
        JS.InvokeVoidAsync("hideCompanyDetailsModal");
    }

    private void OnInternshipFilterChange(ChangeEventArgs e)
    {
        var filterValue = e.Value?.ToString();
    
        if (sumUpInternshipsFromBothCompanyAndProfessor == null) return;

        publishedInternships = filterValue switch
        {
            "company" => sumUpInternshipsFromBothCompanyAndProfessor
                .Where(i => !string.IsNullOrEmpty(i.CompanyName))
                .ToList(),
            "professor" => sumUpInternshipsFromBothCompanyAndProfessor
                .Where(i => !string.IsNullOrEmpty(i.ProfessorName))
                .ToList(),
            _ => sumUpInternshipsFromBothCompanyAndProfessor.ToList() // "all" or default
        };
    
        StateHasChanged();
    }

    // Filtering logic for internships
    private async Task FilterInternshipApplications(ChangeEventArgs e)
    {
        filterValueForInternships = e.Value?.ToString()?.ToLower() ?? "all";

        // Set visibility flags
        showCompanyInternshipApplications = filterValueForInternships == "all" || filterValueForInternships == "company";
        showProfessorInternshipApplications = filterValueForInternships == "all" || filterValueForInternships == "professor";

        try 
        {
            await Task.Delay(1000); // Simulate async operation
        }
        finally
        {
            StateHasChanged(); // Ensure UI updates even if delay fails
        }
    }


    private void SetTotalInternshipCount(int count)
    {
        totalInternshipCount = count;
        totalPagesForInternshipsToSee = (int)Math.Ceiling((double)totalInternshipCount / pageSizeForInternshipsToSee);
    }

    private bool IsPreviousDisabledForInternships => currentPageForInternshipsToSee == 1;
    private bool IsNextDisabledForInternships => currentPageForInternshipsToSee == totalPagesForInternshipsToSee;

    // Helper method to get row color
    private string GetInternshipRowColor(object application)
    {
        if (application is InternshipApplied companyApp)
        {
            return companyApp.InternshipStatusAppliedAtTheStudentSide switch
            {
                "Επιτυχής" => "lightgreen",
                "Απορρίφθηκε" => "lightcoral",
                "Απορρίφθηκε (Απόσυρση Θέσεως Από Εταιρία)" => "coral",
                "Αποσύρθηκε από τον φοιτητή" => "lightyellow",
                _ => "transparent"
            };
        }
        else if (application is ProfessorInternshipApplied professorApp)
        {
            return professorApp.InternshipStatusAppliedAtTheStudentSide switch
            {
                "Επιτυχής" => "lightgreen",
                "Απορρίφθηκε" => "lightcoral",
                "Απορρίφθηκε (Απόσυρση Θέσεως Από τον Καθηγητή)" => "coral",
                "Αποσύρθηκε από τον φοιτητή" => "lightyellow",
                _ => "transparent"
            };
        }
        return "transparent";
    }

    // Pagination variables
    private int currentThesisPage = 1;
    private int thesisPageSize = 3; // Adjust as needed


    // Method to reset to first page when filtering
    private async Task OnFilterChange_PaginationForStudentThesisSearch(ChangeEventArgs e)
    {
        currentThesisPage = 1; // Reset to first page when filter changes
        filterValue = e.Value?.ToString() ?? "all";
        // Your existing filter logic here...
        await LoadThesisData(); // Or whatever your data loading method is
        StateHasChanged();
    }

    private int currentJobPage = 1;
    private int jobPageSize = 3; // Adjust as needed
    private void ChangeJobPage(int newPage)
    {
        var totalPages = (int)Math.Ceiling((double)jobApplications.Count / jobPageSize);
        if (newPage > 0 && newPage <= totalPages)
        {
            currentJobPage = newPage;
        }
    }


    private int currentJobPositionPage = 1;
    private int jobPositionPageSize = 3; // Adjust as needed
    private void ChangeJobPositionPage(int newPage)
    {
        var publishedJobs = jobs?.Where(i => i.PositionStatus == "Δημοσιευμένη").ToList();
        if (publishedJobs != null)
        {
            var totalPages = (int)Math.Ceiling((double)publishedJobs.Count / jobPositionPageSize);
            if (newPage > 0 && newPage <= totalPages)
            {
                currentJobPositionPage = newPage;
            }
        }
    }

    private void GoToFirstPage()
    {
        currentPageForThesisToSee = 1;
    }

    private void GoToLastPage()
    {
        currentPageForThesisToSee = totalPagesForThesisToSee;
    }

    private void GoToPage(int pageNumber)
    {
        currentPageForThesisToSee = pageNumber;
    }

    private List<int> GetVisiblePages()
    {
        var pages = new List<int>();
        int current = currentPageForThesisToSee;
        int total = totalPagesForThesisToSee;
    
        pages.Add(1);
    
        if (current > 3)
        {
            pages.Add(-1);
        }
    
        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);
    
        for (int i = start; i <= end; i++)
        {
            pages.Add(i);
        }
    
        if (current < total - 2)
        {
            pages.Add(-1);
        }
    
        if (total > 1)
        {
            pages.Add(total);
        }
    
        return pages;
    }

    private string GetPageButtonStyle(int pageNumber)
    {
        return pageNumber == currentPageForThesisToSee 
            ? "background-color: #0056b3; color: white;" 
            : "background-color: #007bff; color: white;";
    }

    private List<int> GetVisiblePages(int currentPage, int totalPages)
    {
        var pages = new List<int>();
        
        // Always show first page
        pages.Add(1);
        
        // Show pages around current page
        if (currentPage > 3) pages.Add(-1); // Ellipsis
        
        int start = Math.Max(2, currentPage - 1);
        int end = Math.Min(totalPages - 1, currentPage + 1);
        
        for (int i = start; i <= end; i++)
        {
            pages.Add(i);
        }
        
        if (currentPage < totalPages - 2) pages.Add(-1); // Ellipsis
        
        // Always show last page if different from first
        if (totalPages > 1) pages.Add(totalPages);
        
        return pages;
    }


    private int totalThesisPages_SearchThesisAsStudent => (int)Math.Ceiling((double)publishedTheses.Count / thesisPageSize);
    private void ChangeThesisPage(int newPage)
    {
        if (newPage > 0 && newPage <= totalThesisPages_SearchThesisAsStudent)
        {
            currentThesisPage = newPage;
            StateHasChanged(); // Ensure UI updates
        }
    }


    private void GoToFirstPageForInternships()
    {
        currentPageForInternshipsToSee = 1;
        StateHasChanged();
    }

    private void GoToLastPageForInternships()
    {
        currentPageForInternshipsToSee = totalPagesForInternshipsToSee;
        StateHasChanged();
    }

    private void GoToPageForInternships(int page)
    {
        if (page > 0 && page <= totalPagesForInternshipsToSee)
        {
            currentPageForInternshipsToSee = page;
            StateHasChanged();
        }
    }

    private List<int> GetVisiblePagesForInternships()
    {
        var pages = new List<int>();
        int current = currentPageForInternshipsToSee;
        int total = totalPagesForInternshipsToSee;
        
        // Always show first page
        pages.Add(1);
        
        // Show pages around current page
        if (current > 3) pages.Add(-1); // Ellipsis
        
        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);
        
        for (int i = start; i <= end; i++)
        {
            pages.Add(i);
        }
        
        if (current < total - 2) pages.Add(-1); // Ellipsis
        
        // Always show last page if different from first
        if (total > 1) pages.Add(total);
        
        return pages;
    }

    // Keep your existing methods
    private void PreviousPageForInternshipsToSee()
    {
        if (currentPageForInternshipsToSee > 1)
        {
            currentPageForInternshipsToSee--;
            StateHasChanged();
        }
    }

    private void NextPageForInternshipsToSee()
    {
        if (currentPageForInternshipsToSee < totalPagesForInternshipsToSee)
        {
            currentPageForInternshipsToSee++;
            StateHasChanged();
        }
    }

    // Pagination methods for internships


// Pagination methods
    private void GoToFirstInternshipPage()
    {
        currentInternshipPage = 1;
        StateHasChanged();
    }

    private void GoToLastInternshipPage()
    {
        currentInternshipPage = totalInternshipPages;
        StateHasChanged();
    }

    private void GoToInternshipPage(int page)
    {
        if (page > 0 && page <= totalInternshipPages)
        {
            currentInternshipPage = page;
            StateHasChanged();
        }
    }

    private void PreviousInternshipPage()
    {
        if (currentInternshipPage > 1)
        {
            currentInternshipPage--;
            StateHasChanged();
        }
    }

    private void NextInternshipPage()
    {
        if (currentInternshipPage < totalInternshipPages)
        {
            currentInternshipPage++;
            StateHasChanged();
        }
    }

    private List<int> GetVisibleInternshipPages()
    {
        var pages = new List<int>();
        int current = currentInternshipPage;
        int total = totalInternshipPages;
        
        // Always show first page
        pages.Add(1);
        
        // Show pages around current page
        if (current > 3) pages.Add(-1); // Ellipsis
        
        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);
        
        for (int i = start; i <= end; i++)
        {
            pages.Add(i);
        }
        
        if (current < total - 2) pages.Add(-1); // Ellipsis
        
        // Always show last page if different from first
        if (total > 1) pages.Add(total);
        
        return pages;
    }

    // Method to get paginated internships (3 per page)
    private IEnumerable<AllInternships> GetPaginatedInternships()
    {
        return publishedInternships?
            .Skip((currentInternshipPage - 1) * InternshipsPerPage)
            .Take(InternshipsPerPage);
    }

    private int currentPageForAnnouncements = 1;
    private int pageSizeForAnnouncements = 3;
    private int totalPagesForAnnouncements => (int)Math.Ceiling((double)(FilteredAnnouncements?.Count ?? 0) / pageSizeForAnnouncements);
    private IEnumerable<AnnouncementAsCompany> GetPaginatedAnnouncements()
    {
        return FilteredAnnouncements?
            .Skip((currentPageForAnnouncements - 1) * pageSizeForAnnouncements)
            .Take(pageSizeForAnnouncements) ?? Enumerable.Empty<AnnouncementAsCompany>();
    }

    private List<int> GetVisiblePagesForAnnouncements()
    {
        var pages = new List<int>();
        int current = currentPageForAnnouncements;
        int total = totalPagesForAnnouncements;
    
        pages.Add(1);
        if (current > 3) pages.Add(-1);
    
        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);
    
        for (int i = start; i <= end; i++) pages.Add(i);
    
        if (current < total - 2) pages.Add(-1);
        if (total > 1) pages.Add(total);
    
        return pages;
    }

    private void GoToFirstPageForAnnouncements() => ChangePageForAnnouncements(1);
    private void GoToLastPageForAnnouncements() => ChangePageForAnnouncements(totalPagesForAnnouncements);
    private void PreviousPageForAnnouncements() => ChangePageForAnnouncements(currentPageForAnnouncements - 1);
    private void NextPageForAnnouncements() => ChangePageForAnnouncements(currentPageForAnnouncements + 1);

    private void GoToPageForAnnouncements(int page)
    {
        if (page > 0 && page <= totalPagesForAnnouncements)
        {
            currentPageForAnnouncements = page;
            StateHasChanged();
        }
    }

    private void ChangePageForAnnouncements(int newPage)
    {
        if (newPage > 0 && newPage <= totalPagesForAnnouncements)
        {
            currentPageForAnnouncements = newPage;
            StateHasChanged();
        }
    }

    // Pagination variables for jobs
    private int currentPageForJobs = 1;
    private int JobsPerPage = 3;
    private int totalPagesForJobs => 
        (int)Math.Ceiling((double)(GetFilteredJobs()?.Count() ?? 0) / JobsPerPage);

    // Get filtered jobs based on status
    private IEnumerable<CompanyJob> GetFilteredJobs()
    {
        return jobs?
            .Where(j => selectedStatusFilterForJobs == "Όλα" || j.PositionStatus == selectedStatusFilterForJobs)
            ?? Enumerable.Empty<CompanyJob>();
    }

    // Get paginated jobs
    private IEnumerable<CompanyJob> GetPaginatedJobs()
    {
        return GetFilteredJobs()
            .Skip((currentPageForJobs - 1) * JobsPerPage)
            .Take(JobsPerPage);
    }

    // Navigation methods
    private void GoToFirstPageForJobs()
    {
        currentPageForJobs = 1;
        StateHasChanged();
    }

    private void GoToLastPageForJobs()
    {
        currentPageForJobs = totalPagesForJobs;
        StateHasChanged();
    }

    private void PreviousPageForJobs()
    {
        if (currentPageForJobs > 1)
        {
            currentPageForJobs--;
            StateHasChanged();
        }
    }

    private void NextPageForJobs()
    {
        if (currentPageForJobs < totalPagesForJobs)
        {
            currentPageForJobs++;
            StateHasChanged();
        }
    }

    private void GoToPageForJobs(int page)
    {
        if (page > 0 && page <= totalPagesForJobs)
        {
            currentPageForJobs = page;
            StateHasChanged();
        }
    }

    private List<int> GetVisiblePagesForJobs()
    {
        var pages = new List<int>();
        int current = currentPageForJobs;
        int total = totalPagesForJobs;
    
        pages.Add(1);
        if (current > 3) pages.Add(-1);
    
        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);
    
        for (int i = start; i <= end; i++) pages.Add(i);
    
        if (current < total - 2) pages.Add(-1);
        if (total > 1) pages.Add(total);
    
        return pages;
    }

    // Pagination variables for company internships
    private int currentPageForCompanyInternships = 1;
    private int companyInternshipsPerPage = 3;
    private int totalPagesForCompanyInternships => 
        (int)Math.Ceiling((double)(GetFilteredCompanyInternships()?.Count() ?? 0) / companyInternshipsPerPage);

    // Get filtered company internships based on status
    private IEnumerable<CompanyInternship> GetFilteredCompanyInternships()
    {
        return internships?
            .Where(i => selectedStatusFilterForInternships == "Όλα" || i.CompanyUploadedInternshipStatus == selectedStatusFilterForInternships)
            ?? Enumerable.Empty<CompanyInternship>();
    }

    // Get paginated company internships
    private IEnumerable<CompanyInternship> GetPaginatedCompanyInternships()
    {
        return GetFilteredCompanyInternships()
            .Skip((currentPageForCompanyInternships - 1) * companyInternshipsPerPage)
            .Take(companyInternshipsPerPage);
    }

    // Navigation methods
    private void GoToFirstPageForCompanyInternships()
    {
        currentPageForCompanyInternships = 1;
        StateHasChanged();
    }

    private void GoToLastPageForCompanyInternships()
    {
        currentPageForCompanyInternships = totalPagesForCompanyInternships;
        StateHasChanged();
    }

    private void PreviousPageForCompanyInternships()
    {
        if (currentPageForCompanyInternships > 1)
        {
            currentPageForCompanyInternships--;
            StateHasChanged();
        }
    }

    private void NextPageForCompanyInternships()
    {
        if (currentPageForCompanyInternships < totalPagesForCompanyInternships)
        {
            currentPageForCompanyInternships++;
            StateHasChanged();
        }
    }

    private void GoToPageForCompanyInternships(int page)
    {
        if (page > 0 && page <= totalPagesForCompanyInternships)
        {
            currentPageForCompanyInternships = page;
            StateHasChanged();
        }
    }

    private List<int> GetVisiblePagesForCompanyInternships()
    {
        var pages = new List<int>();
        int current = currentPageForCompanyInternships;
        int total = totalPagesForCompanyInternships;
    
        pages.Add(1);
        if (current > 3) pages.Add(-1);
    
        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);
    
        for (int i = start; i <= end; i++) pages.Add(i);
    
        if (current < total - 2) pages.Add(-1);
        if (total > 1) pages.Add(total);
    
        return pages;
    }

    // Pagination variables for company theses
    private int currentPageForCompanyTheses = 1;
    private int CompanyThesesPerPage = 3;
    private int totalPagesForCompanyTheses => 
        (int)Math.Ceiling((double)(GetFilteredCompanyTheses()?.Count() ?? 0) / CompanyThesesPerPage);

    // Get filtered company theses based on status
    private IEnumerable<CompanyThesis> GetFilteredCompanyTheses()
    {
        return companytheses?
            .Where(j => selectedStatusFilterForCompanyTheses == "Όλα" || j.CompanyThesisStatus == selectedStatusFilterForCompanyTheses)
            ?? Enumerable.Empty<CompanyThesis>();
    }

    // Get paginated company theses
    private IEnumerable<CompanyThesis> GetPaginatedCompanyTheses()
    {
        return GetFilteredCompanyTheses()
            .Skip((currentPageForCompanyTheses - 1) * CompanyThesesPerPage)
            .Take(CompanyThesesPerPage);
    }

    // Navigation methods
    private void GoToFirstPageForCompanyTheses()
    {
        currentPageForCompanyTheses = 1;
        StateHasChanged();
    }

    private void GoToLastPageForCompanyTheses()
    {
        currentPageForCompanyTheses = totalPagesForCompanyTheses;
        StateHasChanged();
    }

    private void PreviousPageForCompanyTheses()
    {
        if (currentPageForCompanyTheses > 1)
        {
            currentPageForCompanyTheses--;
            StateHasChanged();
        }
    }

    private void NextPageForCompanyTheses()
    {
        if (currentPageForCompanyTheses < totalPagesForCompanyTheses)
        {
            currentPageForCompanyTheses++;
            StateHasChanged();
        }
    }

    private void GoToPageForCompanyTheses(int page)
    {
        if (page > 0 && page <= totalPagesForCompanyTheses)
        {
            currentPageForCompanyTheses = page;
            StateHasChanged();
        }
    }

    // Make sure to reset to page 1 when filter changes
    private void HandleStatusFilterChangeForCompanyTheses(ChangeEventArgs e)
    {
        selectedStatusFilterForCompanyTheses = e.Value.ToString();
        currentPageForCompanyTheses = 1; // Reset to first page
        StateHasChanged();
    }

        private List<int> GetVisiblePagesForCompanyTheses()
    {
        var pages = new List<int>();
        int current = currentPageForCompanyTheses;
        int total = totalPagesForCompanyTheses;
    
        pages.Add(1);
        if (current > 3) pages.Add(-1);
    
        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);
    
        for (int i = start; i <= end; i++) pages.Add(i);
    
        if (current < total - 2) pages.Add(-1);
        if (total > 1) pages.Add(total);
    
        return pages;
    }

    // Pagination variables for company events
    private int currentPageForCompanyEvents = 1;
    private int CompanyEventsPerPage = 3;
    private int totalPagesForCompanyEvents_CompanyEventsToSee => 
        (int)Math.Ceiling((double)(FilteredCompanyEvents?.Count ?? 0) / CompanyEventsPerPage);

    // Get paginated company events
    private IEnumerable<CompanyEvent> GetPaginatedCompanyEvents()
    {
        return FilteredCompanyEvents?
            .Skip((currentPageForCompanyEvents - 1) * CompanyEventsPerPage)
            .Take(CompanyEventsPerPage) ?? Enumerable.Empty<CompanyEvent>();
    }

    private List<int> GetVisiblePagesForCompanyEvents()
    {
        var pages = new List<int>();
        int current = currentPageForCompanyEvents;
        int total = totalPagesForCompanyEvents_CompanyEventsToSee;
    
        pages.Add(1);
        if (current > 3) pages.Add(-1);
    
        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);
    
        for (int i = start; i <= end; i++) pages.Add(i);
    
        if (current < total - 2) pages.Add(-1);
        if (total > 1) pages.Add(total);
    
        return pages;
    }

    // Navigation methods
    private void GoToFirstPageForCompanyEvents()
    {
        currentPageForCompanyEvents = 1;
        StateHasChanged();
    }

    private void GoToLastPageForCompanyEvents()
    {
        currentPageForCompanyEvents = totalPagesForCompanyEvents_CompanyEventsToSee;
        StateHasChanged();
    }

    private void PreviousPageForCompanyEvents()
    {
        if (currentPageForCompanyEvents > 1)
        {
            currentPageForCompanyEvents--;
            StateHasChanged();
        }
    }

    private void NextPageForCompanyEvents()
    {
        if (currentPageForCompanyEvents < totalPagesForCompanyEvents_CompanyEventsToSee)
        {
            currentPageForCompanyEvents++;
            StateHasChanged();
        }
    }

    private void GoToPageForCompanyEvents(int page)
    {
        if (page > 0 && page <= totalPagesForCompanyEvents_CompanyEventsToSee)
        {
            currentPageForCompanyEvents = page;
            StateHasChanged();
        }
    }

    // Renamed pagination variables
    private int currentPageForProfessorAnnouncements_ProfessorAnnouncements = 1;
    private int professorAnnouncementsPerPage_SeeMyUploadedAnnouncementsAsProfessor = 3; // Default value
    private int totalPagesForProfessorAnnouncements_ProfessorAnnouncements => 
        (int)Math.Ceiling((double)(FilteredAnnouncementsAsProfessor?.Count ?? 0) / professorAnnouncementsPerPage_SeeMyUploadedAnnouncementsAsProfessor);

    // Renamed methods
    private IEnumerable<AnnouncementAsProfessor> GetPaginatedProfessorAnnouncements_ProfessorAnnouncements()
    {
        return FilteredAnnouncementsAsProfessor?
            .Skip((currentPageForProfessorAnnouncements_ProfessorAnnouncements - 1) * professorAnnouncementsPerPage_SeeMyUploadedAnnouncementsAsProfessor)
            .Take(professorAnnouncementsPerPage_SeeMyUploadedAnnouncementsAsProfessor) ?? Enumerable.Empty<AnnouncementAsProfessor>();
    }

    private List<int> GetVisiblePagesForProfessorAnnouncements_ProfessorAnnouncements()
    {
        var pages = new List<int>();
        int current = currentPageForProfessorAnnouncements_ProfessorAnnouncements;
        int total = totalPagesForProfessorAnnouncements_ProfessorAnnouncements;

        // Always add first page
        pages.Add(1);

        // Add ellipsis (...) if current page is far from the start
        if (current > 3) 
        {
            pages.Add(-1); // -1 represents ellipsis
        }

        // Add pages around current page (1 page before & after)
        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);

        for (int i = start; i <= end; i++) 
        {
            pages.Add(i);
        }

        // Add ellipsis (...) if current page is far from the end
        if (current < total - 2) 
        {
            pages.Add(-1);
        }

        // Add last page if there's more than 1 page
        if (total > 1) 
        {
            pages.Add(total);
        }

        return pages;
    }

    // Renamed navigation methods
    private void GoToFirstPageForProfessorAnnouncements_ProfessorAnnouncements()
    {
        currentPageForProfessorAnnouncements_ProfessorAnnouncements = 1;
        StateHasChanged();
    }

    private void GoToLastPageForProfessorAnnouncements_ProfessorAnnouncements()
    {
        currentPageForProfessorAnnouncements_ProfessorAnnouncements = totalPagesForProfessorAnnouncements_ProfessorAnnouncements;
        StateHasChanged();
    }

    private void PreviousPageForProfessorAnnouncements_ProfessorAnnouncements()
    {
        if (currentPageForProfessorAnnouncements_ProfessorAnnouncements > 1)
        {
            currentPageForProfessorAnnouncements_ProfessorAnnouncements--;
            StateHasChanged();
        }
    }

    private void NextPageForProfessorAnnouncements_ProfessorAnnouncements()
    {
        if (currentPageForProfessorAnnouncements_ProfessorAnnouncements < totalPagesForProfessorAnnouncements_ProfessorAnnouncements)
        {
            currentPageForProfessorAnnouncements_ProfessorAnnouncements++;
            StateHasChanged();
        }
    }

    private void GoToPageForProfessorAnnouncements_ProfessorAnnouncements(int page)
    {
        if (page > 0 && page <= totalPagesForProfessorAnnouncements_ProfessorAnnouncements)
        {
            currentPageForProfessorAnnouncements_ProfessorAnnouncements = page;
            StateHasChanged();
        }
    }

    // Renamed filter handler
    private void HandleStatusFilterChangeForProfessorAnnouncements_ProfessorAnnouncements(ChangeEventArgs e)
    {
        // Your existing filter logic
        currentPageForProfessorAnnouncements_ProfessorAnnouncements = 1;
        StateHasChanged();
    }

    // Pagination variables for professor theses
    private int currentPageForProfessorTheses = 1;
    private int ProfessorThesesPerPage = 3;
    private int totalPagesForProfessorTheses => 
        (int)Math.Ceiling((double)(GetFilteredProfessorTheses()?.Count() ?? 0) / ProfessorThesesPerPage);

    // Get filtered professor theses based on status
    private IEnumerable<ProfessorThesis> GetFilteredProfessorTheses()
    {
        return FilteredThesesAsProfessor?
            .Where(j => selectedStatusFilterForThesesAsProfessor == "Όλα" || j.ThesisStatus == selectedStatusFilterForThesesAsProfessor)
            ?? Enumerable.Empty<ProfessorThesis>();
    }

    // Get paginated professor theses
    private IEnumerable<ProfessorThesis> GetPaginatedProfessorTheses()
    {
        return GetFilteredProfessorTheses()
            .Skip((currentPageForProfessorTheses - 1) * ProfessorThesesPerPage)
            .Take(ProfessorThesesPerPage);
    }

    private List<int> GetVisiblePagesForProfessorTheses()
    {
        var pages = new List<int>();
        int current = currentPageForProfessorTheses;
        int total = totalPagesForProfessorTheses;
    
        pages.Add(1);
        if (current > 3) pages.Add(-1);
    
        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);
    
        for (int i = start; i <= end; i++) pages.Add(i);
    
        if (current < total - 2) pages.Add(-1);
        if (total > 1) pages.Add(total);
    
        return pages;
    }

    // Navigation methods
    private void GoToFirstPageForProfessorTheses()
    {
        currentPageForProfessorTheses = 1;
        StateHasChanged();
    }

    private void GoToLastPageForProfessorTheses()
    {
        currentPageForProfessorTheses = totalPagesForProfessorTheses;
        StateHasChanged();
    }

    private void PreviousPageForProfessorTheses()
    {
        if (currentPageForProfessorTheses > 1)
        {
            currentPageForProfessorTheses--;
            StateHasChanged();
        }
    }

    private void NextPageForProfessorTheses()
    {
        if (currentPageForProfessorTheses < totalPagesForProfessorTheses)
        {
            currentPageForProfessorTheses++;
            StateHasChanged();
        }
    }

    private void GoToPageForProfessorTheses(int page)
    {
        if (page > 0 && page <= totalPagesForProfessorTheses)
        {
            currentPageForProfessorTheses = page;
            StateHasChanged();
        }
    }

    // Make sure to reset to page 1 when filter changes
    private void HandleStatusFilterChangeForProfessorTheses(ChangeEventArgs e)
    {
        selectedStatusFilterForThesesAsProfessor = e.Value.ToString();
        currentPageForProfessorTheses = 1; // Reset to first page
        StateHasChanged();
    }

    private int currentPage_CompanyTheses = 1;
    private int itemsPerPage_CompanyTheses = 3; // adjust as needed
    private int totalPages_CompanyTheses =>
        (int)Math.Ceiling((double)(companyThesesResultsToFindThesesAsProfessor?.Count ?? 0) / itemsPerPage_CompanyTheses);

    private IEnumerable<CompanyThesis> GetPaginatedCompanyTheses_AsProfessor()
    {
        return companyThesesResultsToFindThesesAsProfessor?
            .Skip((currentPage_CompanyTheses - 1) * itemsPerPage_CompanyTheses)
            .Take(itemsPerPage_CompanyTheses) ?? Enumerable.Empty<CompanyThesis>();
    }

    private List<int> GetVisiblePages_CompanyTheses_AsProfessor()
    {
        var pages = new List<int>();
        int current = currentPage_CompanyTheses;
        int total = totalPages_CompanyTheses;

        pages.Add(1);
        if (current > 3) pages.Add(-1);

        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);

        for (int i = start; i <= end; i++) pages.Add(i);
        if (current < total - 2) pages.Add(-1);

        if (total > 1) pages.Add(total);
        return pages;
    }

    private void GoToFirstPage_CompanyTheses() => currentPage_CompanyTheses = 1;
    private void GoToLastPage_CompanyTheses() => currentPage_CompanyTheses = totalPages_CompanyTheses;
    private void PreviousPage_CompanyTheses()
    {
        if (currentPage_CompanyTheses > 1) currentPage_CompanyTheses--;
    }
    private void NextPage_CompanyTheses()
    {
        if (currentPage_CompanyTheses < totalPages_CompanyTheses) currentPage_CompanyTheses++;
    }
    private void GoToPage_CompanyTheses(int page)
    {
        if (page >= 1 && page <= totalPages_CompanyTheses)
            currentPage_CompanyTheses = page;
    }

    private int currentPage_ProfessorInternships = 1;
    private int itemsPerPage_ProfessorInternships = 3;
    private int totalPages_ProfessorInternships =>
        (int)Math.Ceiling((double)(professorInternships?.Count ?? 0) / itemsPerPage_ProfessorInternships);

    private IEnumerable<ProfessorInternship> GetPaginatedProfessorInternships()
    {
        return professorInternships?
            .Skip((currentPage_ProfessorInternships - 1) * itemsPerPage_ProfessorInternships)
            .Take(itemsPerPage_ProfessorInternships) ?? Enumerable.Empty<ProfessorInternship>();
    }

    private List<int> GetVisiblePages_ProfessorInternships()
    {
        var pages = new List<int>();
        int current = currentPage_ProfessorInternships;
        int total = totalPages_ProfessorInternships;

        pages.Add(1);
        if (current > 3) pages.Add(-1);

        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);

        for (int i = start; i <= end; i++) pages.Add(i);
        if (current < total - 2) pages.Add(-1);

        if (total > 1) pages.Add(total);
        return pages;
    }

    private void GoToFirstPage_ProfessorInternships() => currentPage_ProfessorInternships = 1;
    private void GoToLastPage_ProfessorInternships() => currentPage_ProfessorInternships = totalPages_ProfessorInternships;
    private void PreviousPage_ProfessorInternships()
    {
        if (currentPage_ProfessorInternships > 1) currentPage_ProfessorInternships--;
    }
    private void NextPage_ProfessorInternships()
    {
        if (currentPage_ProfessorInternships < totalPages_ProfessorInternships) currentPage_ProfessorInternships++;
    }
    private void GoToPage_ProfessorInternships(int page)
    {
        if (page >= 1 && page <= totalPages_ProfessorInternships)
            currentPage_ProfessorInternships = page;
    }

    private int currentPage_ProfessorEvents = 1;
    private int itemsPerPage_ProfessorEvents = 3;
    private int totalPages_ProfessorEvents =>
        (int)Math.Ceiling((double)(FilteredProfessorEvents?.Count ?? 0) / itemsPerPage_ProfessorEvents);

    private IEnumerable<ProfessorEvent> GetPaginatedProfessorEvents()
    {
        return FilteredProfessorEvents?
            .Skip((currentPage_ProfessorEvents - 1) * itemsPerPage_ProfessorEvents)
            .Take(itemsPerPage_ProfessorEvents) ?? Enumerable.Empty<ProfessorEvent>();
    }

    private List<int> GetVisiblePages_ProfessorEvents()
    {
        var pages = new List<int>();
        int current = currentPage_ProfessorEvents;
        int total = totalPages_ProfessorEvents;

        pages.Add(1);
        if (current > 3) pages.Add(-1);

        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);

        for (int i = start; i <= end; i++) pages.Add(i);
        if (current < total - 2) pages.Add(-1);

        if (total > 1) pages.Add(total);
        return pages;
    }

    private void GoToFirstPage_ProfessorEvents() => currentPage_ProfessorEvents = 1;
    private void GoToLastPage_ProfessorEvents() => currentPage_ProfessorEvents = totalPages_ProfessorEvents;
    private void PreviousPage_ProfessorEvents()
    {
        if (currentPage_ProfessorEvents > 1) currentPage_ProfessorEvents--;
    }
    private void NextPage_ProfessorEvents()
    {
        if (currentPage_ProfessorEvents < totalPages_ProfessorEvents) currentPage_ProfessorEvents++;
    }
    private void GoToPage_ProfessorEvents(int page)
    {
        if (page >= 1 && page <= totalPages_ProfessorEvents)
            currentPage_ProfessorEvents = page;
    }

    private int currentPage_ProfessorTheses = 1;
    private int itemsPerPage_ProfessorTheses = 3; // Adjust as needed
    private int totalPages_ProfessorTheses =>
        (int)Math.Ceiling((double)(professorThesesResultsToFindThesesAsCompany?.Count ?? 0) / itemsPerPage_ProfessorTheses);

    private IEnumerable<ProfessorThesis> GetPaginatedProfessorTheses_AsCompany()
    {
        return professorThesesResultsToFindThesesAsCompany?
            .Skip((currentPage_ProfessorTheses - 1) * itemsPerPage_ProfessorTheses)
            .Take(itemsPerPage_ProfessorTheses) ?? Enumerable.Empty<ProfessorThesis>();
    }

    private List<int> GetVisiblePages_ProfessorTheses_AsCompany()
    {
        var pages = new List<int>();
        int current = currentPage_ProfessorTheses;
        int total = totalPages_ProfessorTheses;

        pages.Add(1);
        if (current > 3) pages.Add(-1);

        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);

        for (int i = start; i <= end; i++) pages.Add(i);
        if (current < total - 2) pages.Add(-1);

        if (total > 1) pages.Add(total);
        return pages;
    }

    private void GoToFirstPage_ProfessorTheses() => currentPage_ProfessorTheses = 1;
    private void GoToLastPage_ProfessorTheses() => currentPage_ProfessorTheses = totalPages_ProfessorTheses;
    private void PreviousPage_ProfessorTheses()
    {
        if (currentPage_ProfessorTheses > 1) currentPage_ProfessorTheses--;
    }
    private void NextPage_ProfessorTheses()
    {
        if (currentPage_ProfessorTheses < totalPages_ProfessorTheses) currentPage_ProfessorTheses++;
    }
    private void GoToPage_ProfessorTheses(int page)
    {
        if (page >= 1 && page <= totalPages_ProfessorTheses)
            currentPage_ProfessorTheses = page;
    }

    // Pagination variables for events
    private int currentPageForEvents = 1;
    private int itemsPerPageForEvents = 3; // Adjust as needed
    private int totalPagesForEvents => 
        (int)Math.Ceiling((double)GetFilteredEventsCount() / itemsPerPageForEvents);

    // Helper methods for pagination
    private IEnumerable<CompanyEvent> GetPaginatedCompanyEvents_StudentSearchEvents()
    {
        if (selectedEventType == "professor") return Enumerable.Empty<CompanyEvent>();
    
        var filtered = companyEventsToSeeAsStudent;
        if (selectedEventType == "all" || selectedEventType == "company")
        {
            return filtered
                .Skip((currentPageForEvents - 1) * itemsPerPageForEvents)
                .Take(itemsPerPageForEvents);
        }
        return Enumerable.Empty<CompanyEvent>();
    }

    private IEnumerable<ProfessorEvent> GetPaginatedProfessorEvents_StudentSearchEvents()
    {
        if (selectedEventType == "company") return Enumerable.Empty<ProfessorEvent>();
    
        var filtered = professorEventsToSeeAsStudent;
        if (selectedEventType == "all" || selectedEventType == "professor")
        {
            return filtered
                .Skip((currentPageForEvents - 1) * itemsPerPageForEvents)
                .Take(itemsPerPageForEvents);
        }
        return Enumerable.Empty<ProfessorEvent>();
    }

    private int GetFilteredEventsCount()
    {
        int count = 0;
    
        if (selectedEventType == "all" || selectedEventType == "company")
        {
            count += companyEventsToSeeAsStudent?.Count() ?? 0;
        }
    
        if (selectedEventType == "all" || selectedEventType == "professor")
        {
            count += professorEventsToSeeAsStudent?.Count() ?? 0;
        }
    
        return count;
    }

    // Pagination methods
    private List<int> GetVisiblePagesForEvents()
    {
        var pages = new List<int>();
        int current = currentPageForEvents;
        int total = totalPagesForEvents;

        pages.Add(1);
        if (current > 3) pages.Add(-1);

        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);

        for (int i = start; i <= end; i++) pages.Add(i);

        if (current < total - 2) pages.Add(-1);
        if (total > 1) pages.Add(total);

        return pages;
    }

    private void GoToFirstPageForEvents() => currentPageForEvents = 1;
    private void PreviousPageForEvents() => currentPageForEvents = Math.Max(1, currentPageForEvents - 1);
    private void NextPageForEvents() => currentPageForEvents = Math.Min(totalPagesForEvents, currentPageForEvents + 1);
    private void GoToLastPageForEvents() => currentPageForEvents = totalPagesForEvents;
    private void GoToPageForEvents(int page) => currentPageForEvents = page;

    private void OnPageSizeChange_SearchForThesisAsStudent(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            thesisPageSize = newSize;
            currentThesisPage = 1; // Reset to first page when changing page size (Idio approach me tis alles methodous)
            StateHasChanged();
        }
    }

    private void OnPageSizeChangeForApplications_SeeMyThesisApplicationsAsStudent(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            pageSizeForThesisToSee = newSize;
            currentPageForThesisToSee = 1;  //reset sto page poy 8elw otan allazw to dropdown menu
            StateHasChanged();
        }
    }

    private void OnJobPageSizeChange_SeeMyJobApplicationsAsStudent(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            jobPageSize = newSize;
            currentJobPage = 1; 
            StateHasChanged();
        }
    }

    private void OnJobPageSizeChange_SearchForJobsAsStudent(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            jobPositionPageSize = newSize;
            currentJobPositionPage = 1; 
            StateHasChanged();
        }
    }

    private void OnPageSizeChange_SeeMyInternshipApplicationsAsStudent(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            pageSizeForInternshipsToSee = newSize;
            currentPageForInternshipsToSee = 1; 
            StateHasChanged();
        }
    } 
    
    private void OnPageSizeChange_SearchForInternshipsAsStudent(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            InternshipsPerPage = newSize;
            currentInternshipPage = 1; 
            StateHasChanged();
        }
    }

    private void OnPageSizeChange_SearchForEventsAsStudent(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            itemsPerPageForEvents = newSize;
            currentPageForEvents = 1; 
            StateHasChanged();
        }
    }

    private void OnPageSizeChange_SeeMyUploadedAnnouncementsAsCompany(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            pageSizeForAnnouncements = newSize;
            currentPageForAnnouncements = 1; // Reset to first page when changing page size
            StateHasChanged();
        }
    }

    private void OnPageSizeChange_SeeMyUploadedJobsAsCompany(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            JobsPerPage = newSize;
            currentPageForJobs = 1; // Reset to first page when changing page size
            StateHasChanged();
        }
    }

    private void OnPageSizeChange_SeeMyUploadedInternshipsAsCompany(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            companyInternshipsPerPage = newSize;
            currentPageForCompanyInternships = 1; // Reset to first page when changing page size
            StateHasChanged();
        }
    }

    private void OnPageSizeChange_SeeMyUploadedThesesAsCompany(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            CompanyThesesPerPage = newSize;
            currentPageForCompanyTheses = 1; // Reset to first page when changing page size
            StateHasChanged();
        }
    }

    private void OnPageSizeChange_SearchForProfessorThesesAsCompany(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            itemsPerPage_ProfessorTheses = newSize;
            currentPage_ProfessorTheses = 1; // Reset to first page when changing page size
            StateHasChanged();
        }
    }

    private void OnPageSizeChange_SeeMyUploadedEventsAsCompany(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            CompanyEventsPerPage = newSize;
            currentPageForCompanyEvents = 1; // Reset to first page when changing page size
            StateHasChanged();
        }
    }

    // Pagination variables for students search
    private int currentPageForStudents_SearchForStudentsAsCompany = 1;
    private int StudentsPerPage_SearchForStudentsAsCompany = 3; // Na einai initiated me 3 stoixeia ana selida tou pinaka
    private int totalPagesForStudents_SearchForStudentsAsCompany => 
        (int)Math.Ceiling((double)(searchResultsAsCompanyToFindStudent?.Count() ?? 0) / StudentsPerPage_SearchForStudentsAsCompany);

    // Get paginated students
    private IEnumerable<Student> GetPaginatedStudents_SearchForStudentsAsCompany()
    {
        return searchResultsAsCompanyToFindStudent?
            .Skip((currentPageForStudents_SearchForStudentsAsCompany - 1) * StudentsPerPage_SearchForStudentsAsCompany)
            .Take(StudentsPerPage_SearchForStudentsAsCompany)
            ?? Enumerable.Empty<Student>();
    }

    // Navigation methods
    private void GoToFirstPageForStudents_SearchForStudentsAsCompany()
    {
        currentPageForStudents_SearchForStudentsAsCompany = 1;
        StateHasChanged();
    }

    private void GoToLastPageForStudents_SearchForStudentsAsCompany()
    {
        currentPageForStudents_SearchForStudentsAsCompany = totalPagesForStudents_SearchForStudentsAsCompany;
        StateHasChanged();
    }

    private void PreviousPageForStudents_SearchForStudentsAsCompany()
    {
        if (currentPageForStudents_SearchForStudentsAsCompany > 1)
        {
            currentPageForStudents_SearchForStudentsAsCompany--;
            StateHasChanged();
        }
    }

    private void NextPageForStudents_SearchForStudentsAsCompany()
    {
        if (currentPageForStudents_SearchForStudentsAsCompany < totalPagesForStudents_SearchForStudentsAsCompany)
        {
            currentPageForStudents_SearchForStudentsAsCompany++;
            StateHasChanged();
        }
    }

    private void GoToPageForStudents_SearchForStudentsAsCompany(int page)
    {
        if (page > 0 && page <= totalPagesForStudents_SearchForStudentsAsCompany)
        {
            currentPageForStudents_SearchForStudentsAsCompany = page;
            StateHasChanged();
        }
    }

    private List<int> GetVisiblePagesForStudents_SearchForStudentsAsCompany()
    {
        var pages = new List<int>();
        int current = currentPageForStudents_SearchForStudentsAsCompany;
        int total = totalPagesForStudents_SearchForStudentsAsCompany;

        pages.Add(1);
        if (current > 3) pages.Add(-1);

        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);

        for (int i = start; i <= end; i++) pages.Add(i);

        if (current < total - 2) pages.Add(-1);
        if (total > 1) pages.Add(total);

        return pages;
    }

    private void OnPageSizeChange_SearchForStudentsAsCompany(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            StudentsPerPage_SearchForStudentsAsCompany = newSize;
            currentPageForStudents_SearchForStudentsAsCompany = 1; 
            StateHasChanged();
        }
    }


    private int ProfessorsPerPage_SearchForProfessorsAsStudent = 3; // Default value
    private int currentProfessorPage_SearchForProfessorsAsStudent = 1;

    private void OnPageSizeChange_SearchForProfessorsAsStudent(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            ProfessorsPerPage_SearchForProfessorsAsStudent = newSize;
            currentProfessorPage_SearchForProfessorsAsStudent = 1;
            StateHasChanged();
        }
    }

    private IEnumerable<Professor> GetPaginatedProfessorResults()
    {
        return searchResultsAsCompanyToFindProfessor?
            .Skip((currentProfessorPage_SearchForProfessorsAsStudent - 1) * ProfessorsPerPage_SearchForProfessorsAsStudent)
            .Take(ProfessorsPerPage_SearchForProfessorsAsStudent) 
            ?? Enumerable.Empty<Professor>();
    }

    private int totalProfessorPages_SearchForProfessorsAsStudent => 
        searchResultsAsCompanyToFindProfessor != null 
            ? (int)Math.Ceiling((double)searchResultsAsCompanyToFindProfessor.Count / ProfessorsPerPage_SearchForProfessorsAsStudent) 
            : 1;

    private List<int> GetVisibleProfessorPages()
    {
        var pages = new List<int>();
        int currentPage = currentProfessorPage_SearchForProfessorsAsStudent;
        int totalPages = totalProfessorPages_SearchForProfessorsAsStudent;
    
        // Always show first page
        pages.Add(1);
    
        // Show pages around current page
        if (currentPage > 3) pages.Add(-1); // Ellipsis
    
        int start = Math.Max(2, currentPage - 1);
        int end = Math.Min(totalPages - 1, currentPage + 1);
    
        for (int i = start; i <= end; i++)
        {
            pages.Add(i);
        }
    
        if (currentPage < totalPages - 2) pages.Add(-1); // Ellipsis
    
        // Always show last page if different from first
        if (totalPages > 1) pages.Add(totalPages);
    
        return pages;
    }

    private void GoToFirstProfessorPage()
    {
        currentProfessorPage_SearchForProfessorsAsStudent = 1;
        StateHasChanged();
    }

    private void PreviousProfessorPage()
    {
        if (currentProfessorPage_SearchForProfessorsAsStudent > 1)
        {
            currentProfessorPage_SearchForProfessorsAsStudent--;
            StateHasChanged();
        }
    }

    private void NextProfessorPage()
    {
        if (currentProfessorPage_SearchForProfessorsAsStudent < totalProfessorPages_SearchForProfessorsAsStudent)
        {
            currentProfessorPage_SearchForProfessorsAsStudent++;
            StateHasChanged();
        }
    }

    private void GoToLastProfessorPage()
    {
        currentProfessorPage_SearchForProfessorsAsStudent = totalProfessorPages_SearchForProfessorsAsStudent;
        StateHasChanged();
    }

    private void GoToProfessorPage(int pageNumber)
    {
        if (pageNumber >= 1 && pageNumber <= totalProfessorPages_SearchForProfessorsAsStudent)
        {
            currentProfessorPage_SearchForProfessorsAsStudent = pageNumber;
            StateHasChanged();
        }
    }

    private void OnPageSizeChange_SeeMyUploadedAnnouncementsAsProfessor(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            professorAnnouncementsPerPage_SeeMyUploadedAnnouncementsAsProfessor = newSize;
            currentPageForProfessorAnnouncements_ProfessorAnnouncements = 1; // Reset to first page
            StateHasChanged();
        }
    }

    private void OnPageSizeChange_SeeMyUploadedThesesAsProfessor(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            ProfessorThesesPerPage = newSize;
            currentPageForProfessorTheses = 1; // Reset to first page
            StateHasChanged();
        }
    }

    private void OnPageSizeChange_SearchForCompanyThesesAsProfessor(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            itemsPerPage_CompanyTheses = newSize;
            currentPage_CompanyTheses = 1; // Reset to first page
            StateHasChanged();
        }
    }

    private void OnPageSizeChange_SeeMyUploadedInternshipsAsProfessor(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            itemsPerPage_ProfessorInternships = newSize;
            currentPage_ProfessorInternships = 1; 
            StateHasChanged();
        }
    }

    private void HandleStatusFilterChangeForProfessorInternships(ChangeEventArgs e)
    {
        selectedStatusFilterForProfessorInternships = e.Value.ToString();
        FilterProfessorInternships();
    }

    private void FilterProfessorInternships()
    {
        // Filter the internships based on the selected filter
        if (selectedStatusFilterForProfessorInternships == "Όλα")
        {
            FilteredInternshipsAsProfessor = professorInternships;
        }
        else
        {
            FilteredInternshipsAsProfessor = professorInternships
                .Where(i => i.ProfessorUploadedInternshipStatus == selectedStatusFilterForProfessorInternships)
                .ToList();
        }

        totalProfessorInternshipsCount = professorInternships.Count;
        publishedProfessorInternshipsCount = professorInternships
            .Count(i => i.ProfessorUploadedInternshipStatus == "Δημοσιευμένη");
        unpublishedProfessorInternshipsCount = professorInternships
            .Count(i => i.ProfessorUploadedInternshipStatus == "Μη Δημοσιευμένη");
        withdrawnProfessorInternshipsCount = professorInternships
            .Count(i => i.ProfessorUploadedInternshipStatus == "Αποσυρμένη");

        currentPage_ProfessorInternships = 1;
        StateHasChanged();
    }

    private void OnPageSizeChange_SeeMyUploadedEventsAsProfessor(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value.ToString(), out int newSize))
        {
            itemsPerPage_ProfessorEvents = newSize;
            currentPage_ProfessorEvents = 1; // Reset to first page when changing page size
            StateHasChanged();
        }
    }

    private List<string> companyNameSuggestions = new List<string>();
    private void HandleCompanyInput(ChangeEventArgs e)
    {
        searchCompanyNameENGAsProfessorToFindCompany = e.Value?.ToString();
        if (!string.IsNullOrWhiteSpace(searchCompanyNameENGAsProfessorToFindCompany) && searchCompanyNameENGAsProfessorToFindCompany.Length >= 2)
        {
            companyNameSuggestions = dbContext.Companies
                .Where(c => c.CompanyNameENG.Contains(searchCompanyNameENGAsProfessorToFindCompany))
                .Select(c => c.CompanyNameENG)
                .Distinct()
                .ToList();
        }
        else
        {
            companyNameSuggestions.Clear();
        }
    }

    private void SelectCompanyNameSuggestion(string suggestion)
    {
        searchCompanyNameENGAsProfessorToFindCompany = suggestion;
        companyNameSuggestions.Clear();
    }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private List<string> companyNameSuggestionsAsRG = new List<string>();
    private void HandleCompanyInputAsRG(ChangeEventArgs e)
    {
        searchCompanyNameENGAsRGToFindCompany = e.Value?.ToString();
        if (!string.IsNullOrWhiteSpace(searchCompanyNameENGAsRGToFindCompany) && searchCompanyNameENGAsRGToFindCompany.Length >= 2)
        {
            companyNameSuggestionsAsRG = dbContext.Companies
                .Where(c => c.CompanyName.Contains(searchCompanyNameENGAsRGToFindCompany))
                .Select(c => c.CompanyName)
                .Distinct()
                .ToList();
        }
        else
        {
            companyNameSuggestionsAsRG.Clear();
        }
    }

    private void SelectCompanyNameSuggestionAsRG(string suggestion)
    {
        searchCompanyNameENGAsRGToFindCompany = suggestion;
        companyNameSuggestionsAsRG.Clear();
    }
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private async Task HandleAreasOfInterestInput_WhenSearchForCompanyAsProfessor(ChangeEventArgs e)
    {
        searchCompanyAreasAsProfessorToFindCompany = e.Value?.ToString().Trim() ?? string.Empty;

        // Ensure areasOfInterestSuggestions is never null
        areasOfInterestSuggestions = new List<string>();

        if (searchCompanyAreasAsProfessorToFindCompany.Length >= 1)
        {
            try
            {
                // Fetch suggestions for Areas of Interest with 1+ characters
                areasOfInterestSuggestions = await Task.Run(() =>
                    dbContext.Areas
                        .Where(a => a.AreaName.Contains(searchCompanyAreasAsProfessorToFindCompany))
                        .Select(a => a.AreaName)
                        .Distinct()
                        .Take(10)
                        .ToList());
            }
            catch (Exception ex)
            {
                // Log the error for debugging purposes
                Console.WriteLine($"Error retrieving Areas of Interest from Database: {ex.Message}");
                areasOfInterestSuggestions = new List<string>();  // Fallback to empty list
            }
        }
        else
        {
            areasOfInterestSuggestions.Clear(); // Clear suggestions for fewer than 1 character
        }

        // Trigger UI refresh
        StateHasChanged();
    }

    private async Task HandleAreasOfInterestInput_WhenSearchForCompanyAsRG(ChangeEventArgs e)
    {
        searchCompanyAreasAsRGToFindCompany = e.Value?.ToString().Trim() ?? string.Empty;

        // Ensure areasOfInterestSuggestions is never null
        areasOfInterestSuggestions = new List<string>();

        if (searchCompanyAreasAsRGToFindCompany.Length >= 1)
        {
            try
            {
                // Fetch suggestions for Areas of Interest with 1+ characters
                areasOfInterestSuggestions = await Task.Run(() =>
                    dbContext.Areas
                        .Where(a => a.AreaName.Contains(searchCompanyAreasAsRGToFindCompany))
                        .Select(a => a.AreaName)
                        .Distinct()
                        .Take(10)
                        .ToList());
            }
            catch (Exception ex)
            {
                // Log the error for debugging purposes
                Console.WriteLine($"Error retrieving Areas of Interest from Database: {ex.Message}");
                areasOfInterestSuggestions = new List<string>();  // Fallback to empty list
            }
        }
        else
        {
            areasOfInterestSuggestions.Clear(); // Clear suggestions for fewer than 1 character
        }

        // Trigger UI refresh
        StateHasChanged();
    }

    private void SelectAreasOfInterestSuggestion_WhenSearchForCompanyAsProfessor(string suggestion)
    {
        if (!string.IsNullOrWhiteSpace(suggestion))
        {
            selectedAreasOfInterest.Add(suggestion);
            searchCompanyAreasAsProfessorToFindCompany = suggestion;
            areasOfInterestSuggestions.Clear();
            StateHasChanged();
        }
    }

    private void SelectAreasOfInterestSuggestion_WhenSearchForCompanyAsRG(string suggestion)
    {
        if (!string.IsNullOrWhiteSpace(suggestion))
        {
            selectedAreasOfInterest.Add(suggestion);
            searchCompanyAreasAsRGToFindCompany = suggestion;
            areasOfInterestSuggestions.Clear();
            StateHasChanged();
        }
    }

    private void RemoveSelectedAreaOfInterest_WhenSearchForCompanyAsProfessor(string area)
    {
        selectedAreasOfInterest.Remove(area);
        StateHasChanged();
    }

    private void RemoveSelectedAreaOfInterest_WhenSearchForCompanyAsRG(string area)
    {
        selectedAreasOfInterest.Remove(area);
        StateHasChanged();
    }

    // Pagination variables for student search as Professor
    private int currentPage_StudentSearch = 1;
    private int StudentSearchPerPage = 3; // Default value

    // Total pages calculation
    private int totalPages_StudentSearch => 
        (int)Math.Ceiling((double)(searchResultsAsProfessorToFindStudent?.Count ?? 0) / StudentSearchPerPage);


    private void OnPageSizeChangeForStudentSearch(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            StudentSearchPerPage = newSize;
            currentPage_StudentSearch = 1; // Reset to first page
            StateHasChanged();
        }
    }

    private IEnumerable<Student> GetPaginatedStudentSearchResults()
    {
        return searchResultsAsProfessorToFindStudent?
            .Skip((currentPage_StudentSearch - 1) * StudentSearchPerPage)
            .Take(StudentSearchPerPage) ?? Enumerable.Empty<Student>();
    }

    private List<int> GetVisiblePages_StudentSearch()
    {
        var pages = new List<int>();
        int current = currentPage_StudentSearch;
        int total = totalPages_StudentSearch;

        pages.Add(1);
        if (current > 3) pages.Add(-1);

        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);

        for (int i = start; i <= end; i++) pages.Add(i);
        if (current < total - 2) pages.Add(-1);

        if (total > 1) pages.Add(total);
        return pages;
    }

    private void GoToFirstPage_StudentSearch()
    {
        currentPage_StudentSearch = 1;
        StateHasChanged();
    }

    private void GoToLastPage_StudentSearch()
    {
        currentPage_StudentSearch = totalPages_StudentSearch;
        StateHasChanged();
    }

    private void PreviousPage_StudentSearch()
    {
        if (currentPage_StudentSearch > 1)
        {
            currentPage_StudentSearch--;
            StateHasChanged();
        }
    }

    private void NextPage_StudentSearch()
    {
        if (currentPage_StudentSearch < totalPages_StudentSearch)
        {
            currentPage_StudentSearch++;
            StateHasChanged();
        }
    }

    private void GoToPage_StudentSearch(int page)
    {
        if (page > 0 && page <= totalPages_StudentSearch)
        {
            currentPage_StudentSearch = page;
            StateHasChanged();
        }
    }

    // Pagination variables for company search
    private int currentPage_CompanySearch = 1;
    private int CompanySearchPerPage = 3; // Default value
    // Total pages calculation
    private int totalPages_CompanySearch => 
        (int)Math.Ceiling((double)(searchResultsAsProfessorToFindCompany?.Count ?? 0) / CompanySearchPerPage);


    // Pagination variables for company search as Research Group
    private int currentPage_CompanySearchAsRG = 1;
    private int CompanySearchPerPageAsRG = 3; // Default value
    // Total pages calculation
    private int totalPages_CompanySearchAsRG => 
        (int)Math.Ceiling((double)(searchResultsAsRGToFindCompany?.Count ?? 0) / CompanySearchPerPageAsRG);



    private void OnPageSizeChangeForCompanySearch(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            CompanySearchPerPage = newSize;
            currentPage_CompanySearch = 1; // Reset to first page
            StateHasChanged();
        }
    }


    private IEnumerable<Company> GetPaginatedCompanySearchResults()
    {
        return searchResultsAsProfessorToFindCompany?
            .Skip((currentPage_CompanySearch - 1) * CompanySearchPerPage)
            .Take(CompanySearchPerPage) ?? Enumerable.Empty<Company>();
    }


    private List<int> GetVisiblePages_CompanySearch()
    {
        var pages = new List<int>();
        int current = currentPage_CompanySearch;
        int total = totalPages_CompanySearch;

        pages.Add(1);
        if (current > 3) pages.Add(-1);

        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);

        for (int i = start; i <= end; i++) pages.Add(i);
        if (current < total - 2) pages.Add(-1);

        if (total > 1) pages.Add(total);
        return pages;
    }


    private void GoToFirstPage_CompanySearch()
    {
        currentPage_CompanySearch = 1;
        StateHasChanged();
    }

    private void GoToLastPage_CompanySearch()
    {
        currentPage_CompanySearch = totalPages_CompanySearch;
        StateHasChanged();
    }

    private void PreviousPage_CompanySearch()
    {
        if (currentPage_CompanySearch > 1)
        {
            currentPage_CompanySearch--;
            StateHasChanged();
        }
    }

    private void NextPage_CompanySearch()
    {
        if (currentPage_CompanySearch < totalPages_CompanySearch)
        {
            currentPage_CompanySearch++;
            StateHasChanged();
        }
    }

    private void GoToPage_CompanySearch(int page)
    {
        if (page > 0 && page <= totalPages_CompanySearch)
        {
            currentPage_CompanySearch = page;
            StateHasChanged();
        }
    }

//////////////////////////////////////////////////////////////////////////////////////////////////
    private void OnPageSizeChangeForCompanySearchAsRG(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            CompanySearchPerPageAsRG = newSize;
            currentPage_CompanySearchAsRG = 1; // Reset to first page
            StateHasChanged();
        }
    }

    private IEnumerable<Company> GetPaginatedCompanySearchResultsAsRG()
    {
        return searchResultsAsRGToFindCompany?
            .Skip((currentPage_CompanySearchAsRG - 1) * CompanySearchPerPageAsRG)
            .Take(CompanySearchPerPageAsRG) ?? Enumerable.Empty<Company>();
    }
    ///
    private List<int> GetVisiblePages_CompanySearchAsRG()
    {
        var pages = new List<int>();
        int current = currentPage_CompanySearchAsRG;
        int total = totalPages_CompanySearchAsRG;

        pages.Add(1);
        if (current > 3) pages.Add(-1);

        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);

        for (int i = start; i <= end; i++) pages.Add(i);
        if (current < total - 2) pages.Add(-1);

        if (total > 1) pages.Add(total);
        return pages;
    }


    private void GoToFirstPage_CompanySearchAsRG()
    {
        currentPage_CompanySearchAsRG = 1;
        StateHasChanged();
    }

    private void GoToLastPage_CompanySearchAsRG()
    {
        currentPage_CompanySearchAsRG = totalPages_CompanySearchAsRG;
        StateHasChanged();
    }

    private void PreviousPage_CompanySearchAsRG()
    {
        if (currentPage_CompanySearchAsRG > 1)
        {
            currentPage_CompanySearchAsRG--;
            StateHasChanged();
        }
    }

    private void NextPage_CompanySearchAsRG()
    {
        if (currentPage_CompanySearchAsRG < totalPages_CompanySearchAsRG)
        {
            currentPage_CompanySearchAsRG++;
            StateHasChanged();
        }
    }

    private void GoToPage_CompanySearchAsRG(int page)
    {
        if (page > 0 && page <= totalPages_CompanySearchAsRG)
        {
            currentPage_CompanySearchAsRG = page;
            StateHasChanged();
        }
    }
//////////////////////////////////////////////////////////////////////////////////////////////////

    private AnnouncementAsProfessor? selectedProfessorAnnouncementToSeeDetails; 
    private void OpenProfessorAnnouncementDetailsModal(AnnouncementAsProfessor currentAnnouncement)
    {
        selectedProfessorAnnouncementToSeeDetails = currentAnnouncement;
    }

    private void CloseProfessorAnnouncementDetailsModal()
    {
        selectedProfessorAnnouncementToSeeDetails = null;
    }

    private async Task HandleProfessorThesisFileUpload(InputFileChangeEventArgs e)
    {
        var file = e.File;  // Access the selected file
        if (file != null)
        {
            Console.WriteLine($"File selected: {file.Name}");

            // Ensure the file is a PDF (optional)
            if (file.ContentType == "application/pdf")
            {
                using (var stream = file.OpenReadStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);  // Copy the file stream to memory stream
                        currentThesisAsProfessor.ThesisAttachment = memoryStream.ToArray();  // Store file as byte array
                        Console.WriteLine($"File uploaded: {file.Name}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Selected file is not a PDF.");
            }
        }
        else
        {
            Console.WriteLine("No file selected.");
        }
    }

    private bool showExpandedAreasInProfessorThesisEditModal = false;
    private bool showExpandedSkillsInProfessorThesisEditModal = false;
    private List<Area> SelectedAreasToEditForProfessorThesis = new List<Area>();
    private List<Skill> SelectedSkillsToEditForProfessorThesis = new List<Skill>();

    private void ToggleAreasInEditProfessorThesisModal() => 
        showExpandedAreasInProfessorThesisEditModal = !showExpandedAreasInProfessorThesisEditModal;

    private void ToggleSkillsInEditProfessorThesisModal() => 
        showExpandedSkillsInProfessorThesisEditModal = !showExpandedSkillsInProfessorThesisEditModal;

    private void OnCheckedChangedForEditProfessorThesisAreas(ChangeEventArgs e, Area area)
    {
        if (e.Value is bool isChecked)
        {
            if (isChecked)
            {
                if (!SelectedAreasToEditForProfessorThesis.Any(a => a.AreaName == area.AreaName))
                {
                    SelectedAreasToEditForProfessorThesis.Add(area);
                }
            }
            else
            {
                SelectedAreasToEditForProfessorThesis.RemoveAll(a => a.AreaName == area.AreaName);
            }
        
            // Update the thesis object immediately
            currentThesisAsProfessor.ThesisAreas = string.Join(",", SelectedAreasToEditForProfessorThesis.Select(a => a.AreaName));
        }
        StateHasChanged();
    }

    private void OnCheckedChangedForEditProfessorThesisSkills(ChangeEventArgs e, Skill skill)
    {
        if (e.Value is bool isChecked)
        {
            if (isChecked)
            {
                if (!SelectedSkillsToEditForProfessorThesis.Any(s => s.SkillName == skill.SkillName))
                {
                    SelectedSkillsToEditForProfessorThesis.Add(skill);
                }
            }
            else
            {
                SelectedSkillsToEditForProfessorThesis.RemoveAll(s => s.SkillName == skill.SkillName);
            }
        
            // Update the thesis object immediately
            currentThesisAsProfessor.ThesisSkills = string.Join(",", SelectedSkillsToEditForProfessorThesis.Select(s => s.SkillName));
        }
        StateHasChanged();
    }

    private void OpenUrl(string url)
    {
        if (!string.IsNullOrWhiteSpace(url))
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "http://" + url;
            }
            NavigationManager.NavigateTo(url, true);
        }
    }

    private void OpenMap(string location)
    {
        if (!string.IsNullOrWhiteSpace(location))
        {
            var mapUrl = $"https://www.google.com/maps/search/{Uri.EscapeDataString(location)}";
            NavigationManager.NavigateTo(mapUrl, true);
        }
    }

    private async Task<IEnumerable<ProfessorInternshipApplied>> GetApplicantsForProfessorInternship(long professorInternshipRNG)
    {
        // Get the professor internship details with professor information
        var internship = await dbContext.ProfessorInternships
            .Include(i => i.Professor) // Include professor details
            .Where(i => i.RNGForInternshipUploadedAsProfessor == professorInternshipRNG) // Updated property
            .Select(i => new {
                i.ProfessorInternshipTitle,
                i.RNGForInternshipUploadedAsProfessor, // Updated property
                ProfessorName = i.Professor.ProfName,
                ProfessorSurname = i.Professor.ProfSurname
            })
            .FirstOrDefaultAsync();

        if (internship == null)
            return Enumerable.Empty<ProfessorInternshipApplied>();

        // Get all applications for this internship (matching by RNG)
        return await dbContext.ProfessorInternshipsApplied
            .Include(a => a.StudentDetails)
            .Include(a => a.ProfessorDetails)
            .Where(a => a.RNGForProfessorInternshipApplied == professorInternshipRNG)
            .ToListAsync();


    }

    private bool showExpandedAreasInProfessorInternshipEditModal = false;
    private List<Area> SelectedAreasToEditForProfessorInternship = new List<Area>();
    private bool showExpandedAreasInProfessorEventEditModal = false;
    // Method to close the edit popup
    private void CloseEditPopupForProfessorInternships()
    {
        isEditPopupVisibleForProfessorInternships = false;
    }

    private async Task SaveEditedProfessorInternship()
    {
        try
        {
            // Check if required fields are filled
            if (string.IsNullOrWhiteSpace(selectedProfessorInternship.ProfessorInternshipTitle) || 
                string.IsNullOrWhiteSpace(selectedProfessorInternship.ProfessorInternshipDescription))
            {
                showSuccessMessage = false;
                showErrorMessage = true;
                return;
            }

            // Set timeout for the database operation
            dbContext.Database.SetCommandTimeout(120); // 120 seconds timeout

            // Handle areas selection
            if (SelectedAreasToEditForProfessorInternship == null || !SelectedAreasToEditForProfessorInternship.Any())
            {
                var currentAreas = selectedProfessorInternship.ProfessorInternshipAreas?.Split(",").ToList() ?? new List<string>();
                SelectedAreasToEditForProfessorInternship = Areas
                    .Where(area => currentAreas.Contains(area.AreaName))
                    .ToList();
            }

            selectedProfessorInternship.ProfessorInternshipAreas = string.Join(",", SelectedAreasToEditForProfessorInternship.Select(area => area.AreaName));

            // Find and update the internship
            var internshipToUpdate = await dbContext.ProfessorInternships.FindAsync(selectedProfessorInternship.Id);
            if (internshipToUpdate != null)
            {
                // Update properties
                internshipToUpdate.ProfessorInternshipTitle = selectedProfessorInternship.ProfessorInternshipTitle;
                internshipToUpdate.ProfessorInternshipDescription = selectedProfessorInternship.ProfessorInternshipDescription;
                internshipToUpdate.ProfessorInternshipType = selectedProfessorInternship.ProfessorInternshipType;
                internshipToUpdate.ProfessorInternshipForeas = selectedProfessorInternship.ProfessorInternshipForeas;
                internshipToUpdate.ProfessorInternshipPerifereiaLocation = selectedProfessorInternship.ProfessorInternshipPerifereiaLocation;
                internshipToUpdate.ProfessorInternshipDimosLocation = selectedProfessorInternship.ProfessorInternshipDimosLocation;
                internshipToUpdate.ProfessorInternshipTransportOffer = selectedProfessorInternship.ProfessorInternshipTransportOffer;
                internshipToUpdate.ProfessorInternshipAreas = selectedProfessorInternship.ProfessorInternshipAreas;
                internshipToUpdate.ProfessorInternshipActivePeriod = selectedProfessorInternship.ProfessorInternshipActivePeriod;
                internshipToUpdate.ProfessorInternshipFinishEstimation = selectedProfessorInternship.ProfessorInternshipFinishEstimation;
                internshipToUpdate.ProfessorInternshipLastUpdate = DateTime.Now;

                // Handle file attachment separately with null check
                if (selectedProfessorInternship.ProfessorInternshipAttachment != null && 
                    selectedProfessorInternship.ProfessorInternshipAttachment.Length > 0)
                {
                    internshipToUpdate.ProfessorInternshipAttachment = selectedProfessorInternship.ProfessorInternshipAttachment;
                }

                // Save changes with try-catch
                try
                {
                    await dbContext.SaveChangesAsync();
                    showSuccessMessage = true;
                    showErrorMessage = false;
                }
                catch (Exception dbEx)
                {
                    Console.Error.WriteLine($"Database save error: {dbEx.Message}");
                    showSuccessMessage = false;
                    showErrorMessage = true;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error in SaveEditedProfessorInternship: {ex.Message}");
            showSuccessMessage = false;
            showErrorMessage = true;
        }
        finally
        {
            isEditPopupVisibleForProfessorInternships = false;
            StateHasChanged();
        }
    }

    private async Task HandleFileUploadToEditProfessorInternshipAttachment(InputFileChangeEventArgs e)
    {
        var file = e.File;  // Access the selected file
        if (file != null)
        {
            Console.WriteLine($"File selected: {file.Name}");

            // Ensure the file is a PDF (optional)
            if (file.ContentType == "application/pdf")
            {
                using (var stream = file.OpenReadStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);  // Copy the file stream to memory stream
                        selectedProfessorInternship.ProfessorInternshipAttachment = memoryStream.ToArray();  // Store file as byte array
                        Console.WriteLine($"File uploaded: {file.Name}");
                    
                        // Optional: Add user feedback
                        uploadSuccess = true;
                        uploadErrorMessage = string.Empty;
                    }
                }
            }
            else
            {
                Console.WriteLine("Selected file is not a PDF.");
                uploadErrorMessage = "Μόνο αρχεία PDF επιτρέπονται";
                uploadSuccess = false;
            }
        }
        else
        {
            Console.WriteLine("No file selected.");
            uploadErrorMessage = "Δεν επιλέχθηκε αρχείο";
            uploadSuccess = false;
        }
        StateHasChanged(); // Update UI to show feedback
    }

    // Method to toggle areas visibility in edit modal
    private void ToggleAreasInEditProfessorInternshipModal()
    {
        showExpandedAreasInProfessorInternshipEditModal = !showExpandedAreasInProfessorInternshipEditModal;
    }

    // Method to handle area selection changes
    private void OnCheckedChangedForEditProfessorInternshipAreas(ChangeEventArgs e, Area area)
    {
        var isChecked = (bool)e.Value;
    
        if (isChecked)
        {
            if (!SelectedAreasToEditForProfessorInternship.Any(a => a.Id == area.Id))
            {
                SelectedAreasToEditForProfessorInternship.Add(area);
            }
        }
        else
        {
            var areaToRemove = SelectedAreasToEditForProfessorInternship.FirstOrDefault(a => a.Id == area.Id);
            if (areaToRemove != null)
            {
                SelectedAreasToEditForProfessorInternship.Remove(areaToRemove);
            }
        }
    }

   
    private void ClearProfessorEventField(int fieldNumber)
    {
        switch (fieldNumber)
        {
            case 1:
                currentProfessorEvent.ProfessorEventStartingPointLocationToTransportPeopleToEvent1 = string.Empty;
                break;
            case 2:
                currentProfessorEvent.ProfessorEventStartingPointLocationToTransportPeopleToEvent2 = string.Empty;
                break;
            case 3:
                currentProfessorEvent.ProfessorEventStartingPointLocationToTransportPeopleToEvent3 = string.Empty;
                break;
        }
    }

    private void ToggleAreasInEditProfessorEventModal()
    {
        showExpandedAreasInProfessorEventEditModal = !showExpandedAreasInProfessorEventEditModal;
    }

    private void OnCheckedChangedForEditProfessorEventAreas(ChangeEventArgs e, Area area)
    {
        var isChecked = (bool)e.Value;
    
        if (isChecked)
        {
            if (!SelectedAreasToEditForProfessorEvent.Any(a => a.Id == area.Id))
            {
                SelectedAreasToEditForProfessorEvent.Add(area);
            }
        }
        else
        {
            var areaToRemove = SelectedAreasToEditForProfessorEvent.FirstOrDefault(a => a.Id == area.Id);
            if (areaToRemove != null)
            {
                SelectedAreasToEditForProfessorEvent.Remove(areaToRemove);
            }
        }
    }

    private async Task HandleFileUploadToEditProfessorEventAttachment(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file != null)
        {
            using (var stream = file.OpenReadStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    currentProfessorEvent.ProfessorEventAttachmentFile = memoryStream.ToArray();
                }
            }
        }
    }

    private List<InterestInProfessorEventAsCompany> InterestedCompaniesForProfessorEvent = new();
    private long? selectedEventIdForCompaniesWhenShowInterestForProfessorEvent;
    private async Task ShowInterestedCompaniesInProfessorEvent(long professoreventRNG)
    {
        if (selectedEventIdForCompaniesWhenShowInterestForProfessorEvent == professoreventRNG)
        {
            // Close the table
            selectedEventIdForCompaniesWhenShowInterestForProfessorEvent = null;
            filteredCompanyInterestForProfessorEvents.Clear();
        }
        else
        {
            // Show the table
            selectedEventIdForCompaniesWhenShowInterestForProfessorEvent = professoreventRNG;
            filteredCompanyInterestForProfessorEvents = await dbContext.InterestInProfessorEventsAsCompany
                .Include(i => i.CompanyDetails)
                .Where(x => x.RNGForProfessorEventInterestAsCompany == professoreventRNG)
                .OrderByDescending(x => x.DateTimeCompanyShowInterestForProfessorEvent)
                .ToListAsync();

            // Load company data for all interests
            foreach (var interest in filteredCompanyInterestForProfessorEvents)
            {
                if (!companyDataCache.ContainsKey(interest.CompanyEmailShowInterestForProfessorEvent))
                {
                    var company = await dbContext.Companies
                        .FirstOrDefaultAsync(c => c.CompanyEmail == interest.CompanyEmailShowInterestForProfessorEvent);
                
                    if (company != null)
                    {
                        companyDataCache[interest.CompanyEmailShowInterestForProfessorEvent] = company;
                    }
                }
            }
        }
        StateHasChanged();
    }



    private bool showModalForCompaniesAtProfessorEventInterest = false;
    private InterestInProfessorEventAsCompany selectedCompanyToShowDetailsForInterestinProfessorEvent;

    private void ShowCompanyDetailsAtProfessorEventInterest(InterestInProfessorEventAsCompany company)
    {
        selectedCompanyToShowDetailsForInterestinProfessorEvent = company;
        showModalForCompaniesAtProfessorEventInterest = true;
        StateHasChanged();
    }

    private void CloseCompanyDetailsModalAtProfessorEventInterest()
    {
        showModalForCompaniesAtProfessorEventInterest = false;
        selectedCompanyToShowDetailsForInterestinProfessorEvent = null;
    }


    private async Task ToggleFormVisibilityToShowStudentStatsAsAdmin()
    {
        isStudentStatsFormVisibleToShowStudentStatsAsAdmin = !isStudentStatsFormVisibleToShowStudentStatsAsAdmin;
        StateHasChanged();
    }

    private Dictionary<string, int> areaDistribution = new();
    private Dictionary<string, int> skillDistributionforadmin = new();
    private void ToggleAnalyticsVisibility()
    {
        isAnalyticsVisible = !isAnalyticsVisible;
        if (isAnalyticsVisible)
        {
            LoadAnalytics();
        }
    }

    private void LoadAnalytics()
    {
        areaDistribution.Clear();
        skillDistributionforadmin.Clear();

        foreach (var student in StudentsWithAuth0Details)
        {
            if (!string.IsNullOrWhiteSpace(student.AreasOfExpertise))
            {
                var areas = student.AreasOfExpertise.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var area in areas.Select(a => a.Trim()))
                {
                    if (areaDistribution.ContainsKey(area))
                        areaDistribution[area]++;
                    else
                        areaDistribution[area] = 1;
                }
            }

            if (!string.IsNullOrWhiteSpace(student.Keywords))
            {
                var skills = student.Keywords.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var skill in skills.Select(s => s.Trim()))
                {
                    if (skillDistributionforadmin.ContainsKey(skill))
                        skillDistributionforadmin[skill]++;
                    else
                        skillDistributionforadmin[skill] = 1;
                }
            }
        }
    }



    private async Task LoadStudentsWithAuth0DetailsAsync()
    {
        var students = await dbContext.Students.ToListAsync();
        var studentsWithDetails = new List<StudentWithAuth0Details>();

        var semaphore = new SemaphoreSlim(5); // max 5 concurrent requests
        var tasks = students.Select(async student =>
        {
            await semaphore.WaitAsync();
            try
            {
                var auth0Details = await Auth0Service.GetUserDetailsAsync(student.Email);
                return new StudentWithAuth0Details
                {
                    Name = student.Name,
                    Surname = student.Surname,
                    Email = student.Email,
                    Department = student.Department,
                    SignUpDate = auth0Details?.CreatedAt,
                    LatestLogin = auth0Details?.LastLogin,
                    LoginBrowser = auth0Details?.LoginBrowser,
                    LoginTimes = auth0Details?.LoginTimes,
                    LastIp = auth0Details?.LastIp,
                    IsEmailVerified = auth0Details?.IsEmailVerified,
                    IsMobile = auth0Details?.IsMobile ?? false,
                    LocationInfo = auth0Details?.LocationInfo,
                    AreasOfExpertise = student.AreasOfExpertise,
                    Keywords = student.Keywords,
                    School = student.School
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get Auth0 details for {student.Email}: {ex.Message}");
                return new StudentWithAuth0Details
                {
                    Name = student.Name,
                    Surname = student.Surname,
                    Email = student.Email,
                    Department = student.Department,
                    SignUpDate = null,
                    LatestLogin = null,
                    LoginBrowser = null,
                    LoginTimes = null,
                    LastIp = null,
                    IsEmailVerified = false,
                    IsMobile = false,
                    LocationInfo = null,
                    AreasOfExpertise = student.AreasOfExpertise,
                    Keywords = student.Keywords,
                    School = student.School
                };
            }
            finally
            {
                semaphore.Release();
            }
        });

        var results = await Task.WhenAll(tasks);
        StudentsWithAuth0Details = results.ToList();
    }

    private async Task<CompanyJob> GetJobDetails(long rngForJob)
    {
        return await dbContext.CompanyJobs
            .FirstOrDefaultAsync(j => j.RNGForPositionUploaded == rngForJob);
    }

    private async Task LoadCompanyJobData()
    {
        try
        {
            if (jobApplications == null || !jobApplications.Any()) 
            {
                jobDataCache = new Dictionary<long, CompanyJob>();
                return;
            }

            var rngsToLoad = jobApplications
                .Select(a => a.RNGForCompanyJobApplied)
                .Distinct()
                .Where(rng => !jobDataCache.ContainsKey(rng))
                .ToList();

            if (rngsToLoad.Any())
            {
                var jobs = await dbContext.CompanyJobs
                    .Where(j => rngsToLoad.Contains(j.RNGForPositionUploaded))
                    .ToListAsync();

                foreach (var job in jobs)
                {
                    jobDataCache[job.RNGForPositionUploaded] = job;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading job data: {ex.Message}");
        }
    }

    private async Task LoadAllStudentData()
    {
        try
        {
            // Get all unique student emails from applications
            var studentEmails = jobApplicantsMap.Values
                .SelectMany(x => x)
                .Select(a => a.StudentEmailAppliedForCompanyJob)
                .Distinct()
                .ToList();

            // Load all students in one query
            var students = await dbContext.Students
                .Where(s => studentEmails.Contains(s.Email))
                .ToListAsync();

            // Populate cache
            foreach (var student in students)
            {
                studentDataCache[student.Email] = student;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading student data: {ex.Message}");
        }
    }

    // Call this when new applications are added
    public async Task RefreshStudentData()
    {
        await LoadAllStudentData();
        StateHasChanged();
    }

    private string GetStatusColor(string status)
    {
        return status switch
        {
            "Επιτυχής" => "lightgreen",
            "Απορρίφθηκε" => "lightcoral",
            "Απορρίφθηκε (Απόσυρση Θέσεως Από Εταιρία)" => "coral",
            "Αποσύρθηκε από τον φοιτητή" => "lightyellow",
            _ => "transparent"
        };
    }

    private async Task HandleProfessorDateChange(ChangeEventArgs e)
    {
        if (DateTime.TryParse(e.Value?.ToString(), out DateTime newDate))
        {
            professorEvent.ProfessorEventActiveDate = newDate;
            await CheckExistingEventsForProfessorDate(); // You'll need to create this method
        }
    }

    private bool hasExistingEventsOnSelectedDateForProfessor = false;
    private async Task CheckExistingEventsForProfessorDate()
    {
        if (professorEvent.ProfessorEventActiveDate.Date >= DateTime.Today.Date)
        {
            // Check for existing company events on this date
            var companyEventsCount = await dbContext.CompanyEvents
                .CountAsync(e => e.CompanyEventActiveDate.Date == professorEvent.ProfessorEventActiveDate.Date && 
                                e.CompanyEventStatus == "Δημοσιευμένη");
        
            // Check for existing professor events on this date
            var professorEventsCount = await dbContext.ProfessorEvents
                .CountAsync(e => e.ProfessorEventActiveDate.Date == professorEvent.ProfessorEventActiveDate.Date && 
                                e.ProfessorEventStatus == "Δημοσιευμένη");
        
            existingEventsCountToCheckAsProfessor = companyEventsCount + professorEventsCount;
            hasExistingEventsOnSelectedDateForProfessor = existingEventsCountToCheckAsProfessor > 0;
        }
        else
        {
            hasExistingEventsOnSelectedDateForProfessor = false;
            existingEventsCountToCheckAsProfessor = 0;
        }
    
        StateHasChanged();
    }

    private async Task LoadResearchGroupStatistics()
    {
        try
        {
            // Get the current research group ID/email
            var currentResearchGroupEmail = CurrentUserEmail;
    
            // Get faculty members count
            numberOfFacultyMembers = await dbContext.ResearchGroup_Professors
                .Where(rp => rp.PK_ResearchGroupEmail == currentResearchGroupEmail)
                .CountAsync();
    
            // Get collaborators count (non-faculty members/students)
            numberOfCollaborators = await dbContext.ResearchGroup_NonFacultyMembers
                .Where(rnf => rnf.PK_ResearchGroupEmail == currentResearchGroupEmail)
                .CountAsync();
    
            // Get research actions count - FIXED: Use ProjectStatus field instead of date comparison
            numberOfActiveResearchActions = await dbContext.ResearchGroup_ResearchActions
                .Where(ra => ra.ResearchGroupEmail == currentResearchGroupEmail && 
                            ra.ResearchGroup_ProjectStatus == "OnGoing")
                .CountAsync();
    
            numberOfInactiveResearchActions = await dbContext.ResearchGroup_ResearchActions
                .Where(ra => ra.ResearchGroupEmail == currentResearchGroupEmail && 
                            ra.ResearchGroup_ProjectStatus == "Past")
                .CountAsync();
    
            // Get patents count
            numberOfActivePatents = await dbContext.ResearchGroup_Patents
                .Where(p => p.ResearchGroupEmail == currentResearchGroupEmail && 
                           p.ResearchGroup_Patent_PatentStatus == "Ενεργή")
                .CountAsync();
    
            numberOfInactivePatents = await dbContext.ResearchGroup_Patents
                .Where(p => p.ResearchGroupEmail == currentResearchGroupEmail && 
                           p.ResearchGroup_Patent_PatentStatus == "Ανενεργή")
                .CountAsync();
    
            // Fetch publications from Google Scholar for all members
            await FetchPublicationsFromGoogleScholar();
    
            // Show success message
            // await ShowSuccessMessage("Τα στατιστικά ενημερώθηκαν επιτυχώς");
        }
        catch (Exception ex)
        {
            // Handle error
            // await ShowErrorMessage($"Σφάλμα κατά τη φόρτωση των στατιστικών: {ex.Message}");
            Console.WriteLine($"Error loading statistics: {ex.Message}");
        }
    }

    private async Task FetchPublicationsFromGoogleScholar()
    {
        try
        {
            var currentResearchGroupEmail = CurrentUserEmail;
        
            // Get all professor emails in this research group with Google Scholar profiles
            var professorEmails = await dbContext.ResearchGroup_Professors
                .Where(rp => rp.PK_ResearchGroupEmail == currentResearchGroupEmail)
                .Select(rp => rp.PK_ProfessorEmail)
                .ToListAsync();
        
            var professorsWithScholar = await dbContext.Professors
                .Where(p => professorEmails.Contains(p.ProfEmail) && 
                           !string.IsNullOrEmpty(p.ProfScholarProfile))
                .Select(p => new { p.ProfEmail, p.ProfScholarProfile })
                .ToListAsync();
        
            // Get all student emails in this research group with Google Scholar profiles
            var studentEmails = await dbContext.ResearchGroup_NonFacultyMembers
                .Where(rnf => rnf.PK_ResearchGroupEmail == currentResearchGroupEmail)
                .Select(rnf => rnf.PK_NonFacultyMemberEmail)
                .ToListAsync();
        
            var studentsWithScholar = await dbContext.Students
                .Where(s => studentEmails.Contains(s.Email) && 
                           !string.IsNullOrEmpty(s.StudentGoogleScholarProfile))
                .Select(s => new { s.Email, s.StudentGoogleScholarProfile })
                .ToListAsync();
        
            // Combine all members with Google Scholar profiles
            var allMembersWithScholar = professorsWithScholar
                .Select(p => new { Email = p.ProfEmail, ScholarProfile = p.ProfScholarProfile, Type = "Professor" })
                .Concat(studentsWithScholar
                    .Select(s => new { Email = s.Email, ScholarProfile = s.StudentGoogleScholarProfile, Type = "Student" }))
                .ToList();
        
            Console.WriteLine($"Found {allMembersWithScholar.Count} members with Google Scholar profiles");
        
            // Fetch publications for each member
            var allPublications = new List<ScholarPublication>();
            var fiveYearsAgo = DateTime.Now.AddYears(-5).Year;
        
            foreach (var member in allMembersWithScholar)
            {
                try
                {
                    Console.WriteLine($"Fetching publications for {member.Email} from {member.ScholarProfile}");
                    var publications = await GoogleScholarService.GetPublications(member.ScholarProfile);
                    Console.WriteLine($"Found {publications.Count} publications for {member.Email}");
                
                    allPublications.AddRange(publications);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching publications for {member.Email}: {ex.Message}");
                }
            }
        
            // Update statistics
            numberOfTotalPublications = allPublications.Count;
            numberOfRecentPublications = allPublications
                .Where(p => !string.IsNullOrEmpty(p.Year) && 
                           int.TryParse(p.Year, out int year) && 
                           year >= fiveYearsAgo)
                .Count();
        
            Console.WriteLine($"Total publications: {numberOfTotalPublications}, Recent publications: {numberOfRecentPublications}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FetchPublicationsForStatistics: {ex.Message}");
            // Set publications to 0 if fetching fails
            numberOfTotalPublications = 0;
            numberOfRecentPublications = 0;
        }
    }

    // Add these fields to your component
    private bool showFacultyMembersModal = false;
    private bool showNonFacultyMembersModal = false;
    private List<FacultyMemberDetail> facultyMembersDetails = new();
    private List<NonFacultyMemberDetail> nonFacultyMembersDetails = new();

    // Add these classes for the modal data
    public class FacultyMemberDetail
    {
        public byte[]? Image { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string School { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class NonFacultyMemberDetail
    {
        public byte[]? Image { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string LevelOfStudies { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string School { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
    }

    // Add these methods
    private async Task ShowFacultyMembersDetails()
    {
        try
        {
            var currentResearchGroupEmail = CurrentUserEmail;
        
            // Get faculty members with their details
            facultyMembersDetails = await dbContext.ResearchGroup_Professors
                .Where(rp => rp.PK_ResearchGroupEmail == currentResearchGroupEmail)
                .Join(dbContext.Professors,
                    rp => rp.PK_ProfessorEmail,
                    p => p.ProfEmail,
                    (rp, p) => new FacultyMemberDetail
                    {
                        Image = p.ProfImage,
                        Name = p.ProfName ?? "",
                        Surname = p.ProfSurname ?? "",
                        School = p.ProfSchool ?? "",
                        Department = p.ProfDepartment ?? "",
                        Role = rp.PK_ProfessorRole ?? ""
                    })
                .ToListAsync();
        
            showFacultyMembersModal = true;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading faculty members details: {ex.Message}");
        }
    }

    private async Task ShowNonFacultyMembersDetails()
    {
        try
        {
            var currentResearchGroupEmail = CurrentUserEmail;
        
            // Get non-faculty members with their details
            nonFacultyMembersDetails = await dbContext.ResearchGroup_NonFacultyMembers
                .Where(rnf => rnf.PK_ResearchGroupEmail == currentResearchGroupEmail)
                .Join(dbContext.Students,
                    rnf => rnf.PK_NonFacultyMemberEmail,
                    s => s.Email,
                    (rnf, s) => new NonFacultyMemberDetail
                    {
                        Image = s.Image,
                        Name = s.Name,
                        Surname = s.Surname,
                        LevelOfStudies = rnf.PK_NonFacultyMemberLevelOfStudies ?? "",
                        Department = s.Department,
                        School = s.School,
                        RegistrationDate = rnf.DateOfRegistrationOnResearchGroup_ForNonFacultyMember
                    })
                .ToListAsync();
        
            showNonFacultyMembersModal = true;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading non-faculty members details: {ex.Message}");
        }
    }

    private void CloseFacultyMembersModal()
    {
        showFacultyMembersModal = false;
        StateHasChanged();
    }

    private void CloseNonFacultyMembersModal()
    {
        showNonFacultyMembersModal = false;
        StateHasChanged();
    }

    private bool showResearchActionsModal = false;
    private List<ResearchActionDetail> researchActionsDetails = new();
    public class ResearchActionDetail
    {
        public string ProjectTitle { get; set; } = string.Empty;
        public string ProjectAcronym { get; set; } = string.Empty;
        public string GrantAgreementNumber { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ProjectCoordinator { get; set; } = string.Empty;
        public string ELKECode { get; set; } = string.Empty;
        public string ScientificResponsibleEmail { get; set; } = string.Empty;
        public string ProjectStatus { get; set; } = string.Empty; // Use the actual status field
    }

    private async Task ShowResearchActionsDetails()
    {
        try
        {
            var currentResearchGroupEmail = CurrentUserEmail;
        
            // Get research actions with their details
            researchActionsDetails = await dbContext.ResearchGroup_ResearchActions
                .Where(ra => ra.ResearchGroupEmail == currentResearchGroupEmail)
                .Select(ra => new ResearchActionDetail
                {
                    ProjectTitle = ra.ResearchGroup_ProjectTitle ?? "",
                    ProjectAcronym = ra.ResearchGroup_ProjectAcronym ?? "",
                    GrantAgreementNumber = ra.ResearchGroup_ProjectGrantAgreementNumber ?? "",
                    StartDate = ra.ResearchGroup_ProjectStartDate,
                    EndDate = ra.ResearchGroup_ProjectEndDate,
                    ProjectCoordinator = ra.ResearchGroup_ProjectCoordinator ?? "",
                    ELKECode = ra.ResearchGroup_ProjectELKECode ?? "",
                    ScientificResponsibleEmail = ra.ResearchGroup_ProjectScientificResponsibleEmail ?? "",
                    ProjectStatus = ra.ResearchGroup_ProjectStatus ?? "" 
                })
                .ToListAsync();
        
            showResearchActionsModal = true;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading research actions details: {ex.Message}");
        }
    }

    private void CloseResearchActionsModal()
    {
        showResearchActionsModal = false;
        StateHasChanged();
    }

    private bool showPatentsModal = false;
    private List<PatentDetail> patentsDetails = new();
    public class PatentDetail
    {
        public string PatentTitle { get; set; } = string.Empty;
        public string PatentType { get; set; } = string.Empty;
        public string PatentDOI { get; set; } = string.Empty;
        public string PatentURL { get; set; } = string.Empty;
        public string PatentDescription { get; set; } = string.Empty;
        public string PatentStatus { get; set; } = string.Empty;
    }


    private async Task ShowPatentsDetails()
    {
        try
        {
            var currentResearchGroupEmail = CurrentUserEmail;
        
            patentsDetails = await dbContext.ResearchGroup_Patents
                .Where(p => p.ResearchGroupEmail == currentResearchGroupEmail)
                .Select(p => new PatentDetail
                {
                    PatentTitle = p.ResearchGroup_Patent_PatentTitle ?? "",
                    PatentType = p.ResearchGroup_Patent_PatentType ?? "",
                    PatentDOI = p.ResearchGroup_Patent_PatentDOI ?? "",
                    PatentURL = p.ResearchGroup_Patent_PatentURL ?? "",
                    PatentDescription = p.ResearchGroup_Patent_PatentDescription ?? "",
                    PatentStatus = p.ResearchGroup_Patent_PatentStatus ?? ""
                })
                .ToListAsync();
        
            showPatentsModal = true;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading patents details: {ex.Message}");
        }
    }

    private void ClosePatentsModal()
    {
        showPatentsModal = false;
        StateHasChanged();
    }


    // Methods for handling inputs and selections
    private async Task HandleResearchGroupNameInput(ChangeEventArgs e)
    {
        searchResearchGroupNameAsCompanyToFindResearchGroup = e.Value?.ToString().Trim() ?? string.Empty;
        researchgroupNameSuggestions = new List<string>();

        if (searchResearchGroupNameAsCompanyToFindResearchGroup.Length >= 1)
        {
            try
            {
                researchgroupNameSuggestions = await Task.Run(() =>
                    dbContext.ResearchGroups
                        .Where(rg => rg.ResearchGroupName.Contains(searchResearchGroupNameAsCompanyToFindResearchGroup))
                        .Select(rg => rg.ResearchGroupName)
                        .Distinct()
                        .Take(10)
                        .ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Πρόβλημα στην Ανάκτηση Ονομάτων Ερευνητικών Ομάδων: {ex.Message}");
                researchgroupNameSuggestions = new List<string>();
            }
        }
        else
        {
            researchgroupNameSuggestions.Clear();
        }

        StateHasChanged();
    }

    private async Task HandleResearchGroupAreasInput(ChangeEventArgs e)
    {
        searchResearchGroupAreasAsCompanyToFindResearchGroup = e.Value?.ToString().Trim() ?? string.Empty;
        researchGroupAreasSuggestions = new List<string>();

        if (searchResearchGroupAreasAsCompanyToFindResearchGroup.Length >= 1)
        {
            try
            {
                // Use the Areas table like Professor search does
                researchGroupAreasSuggestions = await dbContext.Areas
                    .Where(a => a.AreaName.Contains(searchResearchGroupAreasAsCompanyToFindResearchGroup))
                    .Select(a => a.AreaName)
                    .Distinct()
                    .Take(10)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Πρόβλημα στην Ανάκτηση Περιοχών Έρευνας: {ex.Message}");
                researchGroupAreasSuggestions = new List<string>();
            }
        }
        else
        {
            researchGroupAreasSuggestions.Clear();
        }

        StateHasChanged();
    }

    private async Task HandleResearchGroupSkillsInput(ChangeEventArgs e)
    {
        searchResearchGroupSkillsAsCompanyToFindResearchGroup = e.Value?.ToString().Trim() ?? string.Empty;
        researchGroupSkillsSuggestions = new List<string>();

        if (searchResearchGroupSkillsAsCompanyToFindResearchGroup.Length >= 1)
        {
            try
            {
                // Use the Skills table instead of parsing from ResearchGroups
                researchGroupSkillsSuggestions = await dbContext.Skills
                    .Where(s => s.SkillName.Contains(searchResearchGroupSkillsAsCompanyToFindResearchGroup))
                    .Select(s => s.SkillName)
                    .Distinct()
                    .Take(10)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Πρόβλημα στην Ανάκτηση Τεχνολογιών: {ex.Message}");
                researchGroupSkillsSuggestions = new List<string>();
            }
        }
        else
        {
            researchGroupSkillsSuggestions.Clear();
        }

        StateHasChanged();
    }

    private void HandleResearchGroupSkillsKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter" || e.Key == "Tab")
        {
            // Add manual text as a selected skill on Enter/Tab
            if (!string.IsNullOrWhiteSpace(searchResearchGroupSkillsAsCompanyToFindResearchGroup) &&
                !selectedResearchGroupSkills.Contains(searchResearchGroupSkillsAsCompanyToFindResearchGroup))
            {
                selectedResearchGroupSkills.Add(searchResearchGroupSkillsAsCompanyToFindResearchGroup);
                searchResearchGroupSkillsAsCompanyToFindResearchGroup = string.Empty;
                researchGroupSkillsSuggestions.Clear();
            }
        }
    }



    private async Task HandleResearchGroupKeywordsInput(ChangeEventArgs e)
    {
        searchResearchGroupKeywordsAsCompanyToFindResearchGroup = e.Value?.ToString().Trim() ?? string.Empty;
        researchGroupKeywordsSuggestions = new List<string>();

        if (searchResearchGroupKeywordsAsCompanyToFindResearchGroup.Length >= 1)
        {
            try
            {
                // First fetch all research groups from the database
                var allResearchGroups = await dbContext.ResearchGroups.ToListAsync();
            
                // Then perform the split operation on the client side
                researchGroupKeywordsSuggestions = allResearchGroups
                    .SelectMany(rg => rg.ResearchGroupKeywords.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    .Select(keyword => keyword.Trim())
                    .Where(keyword => keyword.Contains(searchResearchGroupKeywordsAsCompanyToFindResearchGroup, StringComparison.OrdinalIgnoreCase))
                    .Distinct()
                    .Take(10)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Πρόβλημα στην Ανάκτηση Λέξεων Κλειδιών: {ex.Message}");
                researchGroupKeywordsSuggestions = new List<string>();
            }
        }
        else
        {
            researchGroupKeywordsSuggestions.Clear();
        }

        StateHasChanged();
    }

    private void SelectResearchGroupNameSuggestion(string suggestion)
    {
        searchResearchGroupNameAsCompanyToFindResearchGroup = suggestion;
        researchgroupNameSuggestions.Clear();
    }

    private void SelectResearchGroupAreasSuggestion(string suggestion)
    {
        if (!string.IsNullOrWhiteSpace(suggestion) && !selectedResearchGroupAreas.Contains(suggestion))
        {
            selectedResearchGroupAreas.Add(suggestion);
            researchGroupAreasSuggestions.Clear();
            searchResearchGroupAreasAsCompanyToFindResearchGroup = string.Empty;
        }
    }

    private void SelectResearchGroupSkillsSuggestion(string suggestion)
    {
        if (!string.IsNullOrWhiteSpace(suggestion) && !selectedResearchGroupSkills.Contains(suggestion))
        {
            selectedResearchGroupSkills.Add(suggestion);
            researchGroupSkillsSuggestions.Clear();
            searchResearchGroupSkillsAsCompanyToFindResearchGroup = string.Empty;
        }
    }

    private void SelectResearchGroupKeywordsSuggestion(string suggestion)
    {
        if (!string.IsNullOrWhiteSpace(suggestion) && !selectedResearchGroupKeywords.Contains(suggestion))
        {
            selectedResearchGroupKeywords.Add(suggestion);
            researchGroupKeywordsSuggestions.Clear();
            searchResearchGroupKeywordsAsCompanyToFindResearchGroup = string.Empty;
        }
    }

    private void RemoveSelectedResearchGroupArea(string area)
    {
        selectedResearchGroupAreas.Remove(area);
        StateHasChanged();
    }

    private void RemoveSelectedResearchGroupSkill(string skill)
    {
        selectedResearchGroupSkills.Remove(skill);
        StateHasChanged();
    }

    private void RemoveSelectedResearchGroupKeyword(string keyword)
    {
        selectedResearchGroupKeywords.Remove(keyword);
        StateHasChanged();
    }

    private async Task SearchResearchGroupsAsCompany()
    {
        try
        {
            hasSearchedForResearchGroups = true;
    
            // First get all research groups from the database
            var allResearchGroups = await dbContext.ResearchGroups.ToListAsync();
    
            // Then filter on the client side
            var filteredResearchGroups = allResearchGroups.AsEnumerable();

            if (!string.IsNullOrEmpty(searchResearchGroupNameAsCompanyToFindResearchGroup))
            {
                filteredResearchGroups = filteredResearchGroups
                    .Where(rg => rg.ResearchGroupName.Contains(searchResearchGroupNameAsCompanyToFindResearchGroup, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(searchResearchGroupSchoolAsCompanyToFindResearchGroup))
            {
                filteredResearchGroups = filteredResearchGroups
                    .Where(rg => rg.ResearchGroupSchool == searchResearchGroupSchoolAsCompanyToFindResearchGroup);
            }

            if (!string.IsNullOrEmpty(searchResearchGroupUniversityDepartmentAsCompanyToFindResearchGroup))
            {
                filteredResearchGroups = filteredResearchGroups
                    .Where(rg => rg.ResearchGroupUniversityDepartment == searchResearchGroupUniversityDepartmentAsCompanyToFindResearchGroup);
            }

            // UPDATED AREAS FILTER: Support both selected areas AND manual text input
            if (selectedResearchGroupAreas.Any() || !string.IsNullOrEmpty(searchResearchGroupAreasAsCompanyToFindResearchGroup))
            {
                // Create a combined list of search terms
                var areaSearchTerms = new List<string>();
            
                // Add manually typed text (if any)
                if (!string.IsNullOrEmpty(searchResearchGroupAreasAsCompanyToFindResearchGroup))
                {
                    areaSearchTerms.Add(searchResearchGroupAreasAsCompanyToFindResearchGroup.Trim());
                }
            
                // Add selected areas from dropdown
                areaSearchTerms.AddRange(selectedResearchGroupAreas);
            
                // Remove duplicates and empty entries
                areaSearchTerms = areaSearchTerms.Where(a => !string.IsNullOrWhiteSpace(a)).Distinct().ToList();

                filteredResearchGroups = filteredResearchGroups
                    .Where(rg => areaSearchTerms.Any(area => 
                        rg.ResearchGroupAreas.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(a => a.Trim())
                            .Any(a => a.Contains(area, StringComparison.OrdinalIgnoreCase))
                    ));
            }

            // UPDATED SKILLS FILTER: Support both selected skills AND manual text input
            if (selectedResearchGroupSkills.Any() || !string.IsNullOrEmpty(searchResearchGroupSkillsAsCompanyToFindResearchGroup))
            {
                // Create a combined list of search terms
                var skillSearchTerms = new List<string>();
            
                // Add manually typed text (if any)
                if (!string.IsNullOrEmpty(searchResearchGroupSkillsAsCompanyToFindResearchGroup))
                {
                    skillSearchTerms.Add(searchResearchGroupSkillsAsCompanyToFindResearchGroup.Trim());
                }
            
                // Add selected skills from dropdown
                skillSearchTerms.AddRange(selectedResearchGroupSkills);
            
                // Remove duplicates and empty entries
                skillSearchTerms = skillSearchTerms.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

                filteredResearchGroups = filteredResearchGroups
                    .Where(rg => skillSearchTerms.Any(skill => 
                        rg.ResearchGroupSkills.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .Any(s => s.Contains(skill, StringComparison.OrdinalIgnoreCase))
                    ));
            }

            // UPDATED KEYWORDS FILTER: Simple text search (no suggestions/selection)
            if (!string.IsNullOrEmpty(searchResearchGroupKeywordsAsCompanyToFindResearchGroup))
            {
                filteredResearchGroups = filteredResearchGroups
                    .Where(rg => rg.ResearchGroupKeywords != null && 
                                rg.ResearchGroupKeywords.Contains(searchResearchGroupKeywordsAsCompanyToFindResearchGroup, StringComparison.OrdinalIgnoreCase));
            }

            searchResultsAsCompanyToFindResearchGroup = filteredResearchGroups.ToList();
            currentResearchGroupPage_SearchForResearchGroupsAsCompany = 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Πρόβλημα στην Αναζήτηση Ερευνητικών Ομάδων: {ex.Message}");
            searchResultsAsCompanyToFindResearchGroup = new List<ResearchGroup>();
        }

        StateHasChanged();
    }

    private void HandleResearchGroupAreasKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter" || e.Key == "Tab")
        {
            if (!string.IsNullOrWhiteSpace(searchResearchGroupAreasAsCompanyToFindResearchGroup) &&
                !selectedResearchGroupAreas.Contains(searchResearchGroupAreasAsCompanyToFindResearchGroup))
            {
                selectedResearchGroupAreas.Add(searchResearchGroupAreasAsCompanyToFindResearchGroup);
                searchResearchGroupAreasAsCompanyToFindResearchGroup = string.Empty;
                researchGroupAreasSuggestions.Clear();
            }
        }
    }

    private void ClearSearchFieldsAsCompanyToFindResearchGroup()
    {
        searchResearchGroupNameAsCompanyToFindResearchGroup = "";
        searchResearchGroupSchoolAsCompanyToFindResearchGroup = "";
        searchResearchGroupUniversityDepartmentAsCompanyToFindResearchGroup = "";
        searchResearchGroupAreasAsCompanyToFindResearchGroup = "";
        searchResearchGroupSkillsAsCompanyToFindResearchGroup = "";
        searchResearchGroupKeywordsAsCompanyToFindResearchGroup = "";
    
        selectedResearchGroupAreas.Clear();
        selectedResearchGroupSkills.Clear();
    
        researchgroupNameSuggestions.Clear();
        researchGroupAreasSuggestions.Clear();
        researchGroupSkillsSuggestions.Clear();
    
        searchResultsAsCompanyToFindResearchGroup = new List<ResearchGroup>();
        hasSearchedForResearchGroups = false;
    
        StateHasChanged();
    }

    private void ShowResearchGroupDetailsOnEyeIconWhenSearchForResearchGroupAsCompany(ResearchGroup researchGroup)
    {
        selectedResearchGroupWhenSearchForResearchGroupsAsCompany = researchGroup;
        showResearchGroupDetailsModalWhenSearchForResearchGroupsAsCompany = true;
        StateHasChanged();
    }


    // Pagination methods (similar to professor search)
    private IEnumerable<ResearchGroup> GetPaginatedResearchGroupResults()
    {
        return searchResultsAsCompanyToFindResearchGroup
            .Skip((currentResearchGroupPage_SearchForResearchGroupsAsCompany - 1) * ResearchGroupsPerPage_SearchForResearchGroupsAsCompany)
            .Take(ResearchGroupsPerPage_SearchForResearchGroupsAsCompany);
    }

    private int totalResearchGroupPages_SearchForResearchGroupsAsCompany => 
        (int)Math.Ceiling((double)searchResultsAsCompanyToFindResearchGroup.Count / ResearchGroupsPerPage_SearchForResearchGroupsAsCompany);

    private void GoToResearchGroupPage(int pageNumber)
    {
        currentResearchGroupPage_SearchForResearchGroupsAsCompany = pageNumber;
    }

    private void OnPageSizeChange_SearchForResearchGroupsAsCompany(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
        {
            ResearchGroupsPerPage_SearchForResearchGroupsAsCompany = newSize;
            currentResearchGroupPage_SearchForResearchGroupsAsCompany = 1;
            StateHasChanged();
        }
    }

    private void GoToFirstResearchGroupPage()
    {
        currentResearchGroupPage_SearchForResearchGroupsAsCompany = 1;
        StateHasChanged();
    }

    private void PreviousResearchGroupPage()
    {
        if (currentResearchGroupPage_SearchForResearchGroupsAsCompany > 1)
        {
            currentResearchGroupPage_SearchForResearchGroupsAsCompany--;
            StateHasChanged();
        }
    }

    private List<int> GetVisibleResearchGroupPages()
    {
        var pages = new List<int>();
        int currentPage = currentResearchGroupPage_SearchForResearchGroupsAsCompany;
        int totalPages = totalResearchGroupPages_SearchForResearchGroupsAsCompany;

        // Always show first page
        pages.Add(1);

        // Show pages around current page
        if (currentPage > 3) pages.Add(-1); // Ellipsis

        int start = Math.Max(2, currentPage - 1);
        int end = Math.Min(totalPages - 1, currentPage + 1);

        for (int i = start; i <= end; i++)
        {
            pages.Add(i);
        }

        if (currentPage < totalPages - 2) pages.Add(-1); // Ellipsis

        // Always show last page if different from first
        if (totalPages > 1) pages.Add(totalPages);

        return pages;
    }

    private void NextResearchGroupPage()
    {
        if (currentResearchGroupPage_SearchForResearchGroupsAsCompany < totalResearchGroupPages_SearchForResearchGroupsAsCompany)
        {
            currentResearchGroupPage_SearchForResearchGroupsAsCompany++;
            StateHasChanged();
        }
    }

    private void GoToLastResearchGroupPage()
    {
        currentResearchGroupPage_SearchForResearchGroupsAsCompany = totalResearchGroupPages_SearchForResearchGroupsAsCompany;
        StateHasChanged();
    }

    private string searchSchoolAsCompanyToFindProfessor = "";
    // Property to get the filtered departments for professors based on selected school
    private List<string> filteredProfessorDepartments => 
        string.IsNullOrEmpty(searchSchoolAsCompanyToFindProfessor) 
            ? GetAllProfessorDepartments() 
            : universityDepartments.ContainsKey(searchSchoolAsCompanyToFindProfessor)
                ? universityDepartments[searchSchoolAsCompanyToFindProfessor]
                : new List<string>();

    // Method to get all departments from all schools (for when no school is selected)
    private List<string> GetAllProfessorDepartments()
    {
        return universityDepartments.Values.SelectMany(depts => depts).Distinct().ToList();
    }

    // Method to handle school change for professor search
    private void OnProfessorSchoolChanged()
    {
        // Clear department selection when school changes
        searchDepartmentAsCompanyToFindProfessor = "";
        StateHasChanged();
    }

    private void HandleProfessorSchoolChanged(ChangeEventArgs e)
    {
        searchSchoolAsCompanyToFindProfessor = e.Value?.ToString();
        OnProfessorSchoolChanged();
    }


    private string searchSchoolAsCompanyToFindStudent = "";
    // Property to get the filtered departments for students based on selected school
    private List<string> filteredStudentDepartments => 
        string.IsNullOrEmpty(searchSchoolAsCompanyToFindStudent) 
            ? GetAllStudentDepartments() 
            : universityDepartments.ContainsKey(searchSchoolAsCompanyToFindStudent)
                ? universityDepartments[searchSchoolAsCompanyToFindStudent]
                : new List<string>();

    // Method to get all departments from all schools (for when no school is selected)
    private List<string> GetAllStudentDepartments()
    {
        return universityDepartments.Values.SelectMany(depts => depts).Distinct().ToList();
    }

    // Method to handle school change for student search
    private async Task OnStudentSchoolChanged(ChangeEventArgs e)
    {
        searchSchoolAsCompanyToFindStudent = e.Value?.ToString() ?? "";
        // Clear department selection when school changes
        searchDepartmentAsCompanyToFindStudent = "";
        await InvokeAsync(StateHasChanged);
    }

    private string searchSchoolAsProfessorToFindStudent = "";
    private List<string> filteredStudentDepartmentsAsProfessor => 
    string.IsNullOrEmpty(searchSchoolAsProfessorToFindStudent) 
        ? GetAllStudentDepartments() 
        : universityDepartments.ContainsKey(searchSchoolAsProfessorToFindStudent)
            ? universityDepartments[searchSchoolAsProfessorToFindStudent]
            : new List<string>();

    // Method to handle school change for student search as Professor
    private async Task OnStudentSchoolChangedAsProfessor(ChangeEventArgs e)
    {
        searchSchoolAsProfessorToFindStudent = e.Value?.ToString() ?? "";
        // Clear department selection when school changes
        searchDepartmentAsProfessorToFindStudent = "";
        await InvokeAsync(StateHasChanged);
    }

    private string searchSchoolAsRGToFindProfessor = "";
    // Property to get the filtered departments for professors based on selected school (Research Group search)
    private List<string> filteredProfessorDepartmentsAsRG => 
        string.IsNullOrEmpty(searchSchoolAsRGToFindProfessor) 
            ? GetAllProfessorDepartments() 
            : universityDepartments.ContainsKey(searchSchoolAsRGToFindProfessor)
                ? universityDepartments[searchSchoolAsRGToFindProfessor]
                : new List<string>();

    // Method to handle school change for professor search as Research Group
    private async Task OnProfessorSchoolChangedAsRG(ChangeEventArgs e)
    {
        searchSchoolAsRGToFindProfessor = e.Value?.ToString() ?? "";
        // Clear department selection when school changes
        searchDepartmentAsRGToFindProfessor = "";
        await InvokeAsync(StateHasChanged);
    }







    // Add these variables to your component
    private List<FacultyMemberInfo> facultyMembers = new List<FacultyMemberInfo>();
    private List<NonFacultyMemberInfo> nonFacultyMembers = new List<NonFacultyMemberInfo>();
    private List<SpinOffCompanyInfo> spinOffCompanies = new List<SpinOffCompanyInfo>();
    private int facultyMembersCount = 0;
    private int nonFacultyMembersCount = 0;
    private int activeResearchActionsCount = 0;
    private int patentsCount = 0;

    // Helper classes
    public class FacultyMemberInfo
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
    }

    public class NonFacultyMemberInfo
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
    }

    public class SpinOffCompanyInfo
    {
        public string CompanyTitle { get; set; } = "";
        public string CompanyAFM { get; set; } = "";
    }

    // Method to load modal data
    private async Task LoadResearchGroupDetailsData(string researchGroupEmail)
    {
        try
        {
            // Load Faculty Members - Get from Professors table using PK_ProfessorEmail
            var facultyMembersData = await dbContext.ResearchGroup_Professors
                .Where(rp => rp.PK_ResearchGroupEmail == researchGroupEmail)
                .Join(dbContext.Professors,
                      rp => rp.PK_ProfessorEmail,
                      p => p.ProfEmail,
                      (rp, p) => new FacultyMemberInfo
                      {
                          FullName = $"{p.ProfName} {p.ProfSurname}",
                          Email = p.ProfEmail
                      })
                .ToListAsync();

            facultyMembers = facultyMembersData;
            facultyMembersCount = facultyMembers.Count;

            // Load Non-Faculty Members - Get from Students table using PK_NonFacultyMemberEmail
            var nonFacultyMembersData = await dbContext.ResearchGroup_NonFacultyMembers
                .Where(rn => rn.PK_ResearchGroupEmail == researchGroupEmail)
                .Join(dbContext.Students,
                      rn => rn.PK_NonFacultyMemberEmail,
                      s => s.Email,
                      (rn, s) => new NonFacultyMemberInfo
                      {
                          FullName = $"{s.Name} {s.Surname}",
                          Email = s.Email
                      })
                .ToListAsync();

            nonFacultyMembers = nonFacultyMembersData;
            nonFacultyMembersCount = nonFacultyMembers.Count;

            // Load Spin-off Companies
            spinOffCompanies = await dbContext.ResearchGroup_SpinOffCompany
                .Where(s => s.ResearchGroupEmail == researchGroupEmail)
                .Select(s => new SpinOffCompanyInfo
                {
                    CompanyTitle = s.ResearchGroup_SpinOff_CompanyTitle,
                    CompanyAFM = s.ResearchGroup_SpinOff_CompanyAFM
                })
                .ToListAsync();

            // Count Active Research Actions
            activeResearchActionsCount = await dbContext.ResearchGroup_ResearchActions
                .Where(r => r.ResearchGroupEmail == researchGroupEmail && 
                           r.ResearchGroup_ProjectStatus == "OnGoing")
                .CountAsync();

            // Count Patents
            patentsCount = await dbContext.ResearchGroup_Patents
                .Where(p => p.ResearchGroupEmail == researchGroupEmail)
                .CountAsync();

            // DEBUG: Check if data is being loaded
            Console.WriteLine($"Faculty Members: {facultyMembersCount}");
            Console.WriteLine($"Non-Faculty Members: {nonFacultyMembersCount}");
            Console.WriteLine($"Spin-off Companies: {spinOffCompanies.Count}");
            Console.WriteLine($"Active Research Actions: {activeResearchActionsCount}");
            Console.WriteLine($"Patents: {patentsCount}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Πρόβλημα στην φόρτωση λεπτομερειών ομάδας: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
    }

    // Update your modal opening method to load data
    private async Task ShowResearchGroupDetailsModal(ResearchGroup researchGroup)
    {
        selectedResearchGroupWhenSearchForResearchGroupsAsCompany = researchGroup;
        showResearchGroupDetailsModalWhenSearchForResearchGroupsAsCompany = true;
    
        // Load additional data
        if (!string.IsNullOrEmpty(researchGroup.ResearchGroupEmail))
        {
            await LoadResearchGroupDetailsData(researchGroup.ResearchGroupEmail);
        }
    
        StateHasChanged();
    }

    // Clear data when closing modal
    private void CloseModalResearchGroupDetailsOnEyeIconWhenSearchForResearchGroupsAsCompany()
    {
        showResearchGroupDetailsModalWhenSearchForResearchGroupsAsCompany = false;
        selectedResearchGroupWhenSearchForResearchGroupsAsCompany = null;
        facultyMembers.Clear();
        nonFacultyMembers.Clear();
        spinOffCompanies.Clear();
        facultyMembersCount = 0;
        nonFacultyMembersCount = 0;
        activeResearchActionsCount = 0;
        patentsCount = 0;
    }

    private void GoToFirstPageForCompanyAnnouncements()
    {
        currentPageForCompanyAnnouncements = 1;
    }

    private void PreviousPageForCompanyAnnouncements()
    {
        if (currentPageForCompanyAnnouncements > 1)
        {
            currentPageForCompanyAnnouncements--;
        }
    }

    private void NextPageForCompanyAnnouncements()
    {
        if (currentPageForCompanyAnnouncements < totalPagesForCompanyAnnouncements)
        {
            currentPageForCompanyAnnouncements++;
        }
    }

    private void GoToLastPageForCompanyAnnouncements()
    {
        currentPageForCompanyAnnouncements = totalPagesForCompanyAnnouncements;
    }

    private void GoToPageForCompanyAnnouncements(int pageNumber)
    {
        currentPageForCompanyAnnouncements = pageNumber;
    }

    private List<int> GetVisiblePagesForCompanyAnnouncements()
    {
        var pages = new List<int>();
        int current = currentPageForCompanyAnnouncements;
        int total = totalPagesForCompanyAnnouncements;

        pages.Add(1);

        if (current > 3)
        {
            pages.Add(-1);
        }

        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);

        for (int i = start; i <= end; i++)
        {
            pages.Add(i);
        }

        if (current < total - 2)
        {
            pages.Add(-1);
        }

        if (total > 1)
        {
            pages.Add(total);
        }

        return pages;
    }

    private void GoToFirstPageForProfessorAnnouncements()
    {
        currentPageForProfessorAnnouncements = 1;
    }

    private void PreviousPageForProfessorAnnouncements()
    {
        if (currentPageForProfessorAnnouncements > 1)
        {
            currentPageForProfessorAnnouncements--;
        }
    }

    private void NextPageForProfessorAnnouncements()
    {
        if (currentPageForProfessorAnnouncements < totalPagesForProfessorAnnouncements)
        {
            currentPageForProfessorAnnouncements++;
        }
    }

    private void GoToLastPageForProfessorAnnouncements()
    {
        currentPageForProfessorAnnouncements = totalPagesForProfessorAnnouncements;
    }

    private void GoToPageForProfessorAnnouncements(int pageNumber)
    {
        currentPageForProfessorAnnouncements = pageNumber;
    }

    private List<int> GetVisiblePagesForProfessorAnnouncements()
    {
        var pages = new List<int>();
        int current = currentPageForProfessorAnnouncements;
        int total = totalPagesForProfessorAnnouncements;

        pages.Add(1);

        if (current > 3)
        {
            pages.Add(-1);
        }

        int start = Math.Max(2, current - 1);
        int end = Math.Min(total - 1, current + 1);

        for (int i = start; i <= end; i++)
        {
            pages.Add(i);
        }

        if (current < total - 2)
        {
            pages.Add(-1);
        }

        if (total > 1)
        {
            pages.Add(total);
        }

        return pages;
    }


    List<string> AvailableSchools = new();
    List<string> AvailableDepartments = new();
    string SelectedAreaSchool = "";
    string SelectedAreaDepartment = "";
    string SelectedSkillSchool = "";
    string SelectedSkillDepartment = "";



    // Event handlers for dropdowns
    private void FilterAreasBySchool(ChangeEventArgs e)
    {
        SelectedAreaSchool = e.Value?.ToString();
        UpdateAreasChart();
    }

    private void FilterAreasByDepartment(ChangeEventArgs e)
    {
        SelectedAreaDepartment = e.Value?.ToString();
        UpdateAreasChart();
    }

    private void FilterSkillsBySchool(ChangeEventArgs e)
    {
        SelectedSkillSchool = e.Value?.ToString();
        UpdateSkillsChart();
    }

    private void FilterSkillsByDepartment(ChangeEventArgs e)
    {
        SelectedSkillDepartment = e.Value?.ToString();
        UpdateSkillsChart();
    }

    private void UpdateAreasChart()
    {
        var filtered = StudentsWithAuth0Details
            .Where(s => (string.IsNullOrEmpty(SelectedAreaSchool) || s.School == SelectedAreaSchool) &&
                       (string.IsNullOrEmpty(SelectedAreaDepartment) || s.Department == SelectedAreaDepartment))
            .ToList();

        // Calculate distribution for filtered data - SPLIT comma-separated areas
        var filteredAreaDistribution = new Dictionary<string, int>();
        foreach (var student in filtered)
        {
            if (!string.IsNullOrWhiteSpace(student.AreasOfExpertise))
            {
                var areas = student.AreasOfExpertise.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var area in areas.Select(a => a.Trim()))
                {
                    if (filteredAreaDistribution.ContainsKey(area))
                        filteredAreaDistribution[area]++;
                    else
                        filteredAreaDistribution[area] = 1;
                }
            }
        }

        var areaLabels = filteredAreaDistribution.Keys.ToArray();
        var areaValues = filteredAreaDistribution.Values.ToArray();

        // Update only the areas chart, preserve skills chart
        JS.InvokeVoidAsync("renderCharts", 
            new { labels = areaLabels, values = areaValues },
            null); // null means don't update skills
    }

    private void UpdateSkillsChart()
    {
        var filtered = StudentsWithAuth0Details
            .Where(s => (string.IsNullOrEmpty(SelectedSkillSchool) || s.School == SelectedSkillSchool) &&
                       (string.IsNullOrEmpty(SelectedSkillDepartment) || s.Department == SelectedSkillDepartment))
            .ToList();

        // Calculate distribution for filtered data
        var filteredSkillDistribution = new Dictionary<string, int>();
        foreach (var student in filtered)
        {
            if (!string.IsNullOrWhiteSpace(student.Keywords))
            {
                var skills = student.Keywords.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var skill in skills.Select(s => s.Trim()))
                {
                    if (filteredSkillDistribution.ContainsKey(skill))
                        filteredSkillDistribution[skill]++;
                    else
                        filteredSkillDistribution[skill] = 1;
                }
            }
        }

        var skillLabels = filteredSkillDistribution.Keys.ToArray();
        var skillValues = filteredSkillDistribution.Values.ToArray();

        // Update only the skills chart, preserve areas chart
        JS.InvokeVoidAsync("renderCharts", 
            null, // null means don't update areas
            new { labels = skillLabels, values = skillValues });
    }

    // Add this method to update both charts when needed
    private void UpdateBothCharts()
    {
        UpdateAreasChart();
        UpdateSkillsChart();
    }

    // EventCallback handlers for two-way binding
    private async Task OnStudentRegisteredChanged(bool value)
    {
        isStudentRegistered = value;
        await InvokeAsync(StateHasChanged);
    }

    private async Task OnCompanyRegisteredChanged(bool value)
    {
        isCompanyRegistered = value;
        await InvokeAsync(StateHasChanged);
    }

    private async Task OnProfessorRegisteredChanged(bool value)
    {
        isProfessorRegistered = value;
        await InvokeAsync(StateHasChanged);
    }

    private async Task OnResearchGroupRegisteredChanged(bool value)
    {
        isResearchGroupRegistered = value;
        await InvokeAsync(StateHasChanged);
    }

    }
}
