using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using LinqKit;
using Newtonsoft.Json;
using QuizManager.Data;
using System;
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
    public partial class CompanyLayoutSection
    {
        [Inject] private Data.AppDbContext dbContext { get; set; }
        [Inject] private Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IJSRuntime JS { get; set; }

        [Parameter] public EventCallback<CompanyThesis> EditCompanyThesisDetailsRequested { get; set; }
        [Parameter] public EventCallback<ThesisStatusChangeRequest> UpdateThesisStatusAsCompanyRequested { get; set; }
        [Parameter] public EventCallback<long> ToggleCompanyThesesExpandedRequested { get; set; }
        [Parameter] public EventCallback<long> ToggleCompanyThesesExpandedForProfessorInterestRequested { get; set; }
        [Parameter] public EventCallback<CompanyThesisApplicationDecision> ConfirmAndAcceptStudentThesisApplicationAsCompanyRequested { get; set; }
        [Parameter] public Dictionary<long, IEnumerable<CompanyThesisApplied>> CompanyThesisApplicantsMap { get; set; } = new();
        [Parameter] public Dictionary<string, Professor>? ProfessorDataCache { get; set; }
        [Parameter] public bool IsModalVisibleToEditCompanyThesisDetails { get; set; }
        [Parameter] public CompanyThesis? SelectedCompanyThesis { get; set; }
        [Parameter] public bool ShowExpandedAreasInCompanyThesisEditModalAsCompany { get; set; }

        // Company-specific properties
        private Company companyData;
        private List<CompanyJob> jobs = new();
        private List<CompanyInternship> internships = new();
        private List<CompanyThesis> companytheses = new();
        private List<AnnouncementAsCompany> announcements = new();
        private List<CompanyEvent> companyEvents = new();

        // UI state properties
        private bool isInitializedAsCompanyUser = false;
        private bool isCompanyRegistered = false;
        private bool isForm1Visible = false;
        private bool isForm2Visible = false;
        private bool isAnnouncementsFormVisible = false;
        private bool isUploadCompanyInternshipsFormVisible = false;
        private bool isUploadCompanyThesisFormVisible = false;
        private bool isUploadCompanyEventFormVisible = false;
        private bool isShowActiveJobsAsCompanyFormVisible = false;
        private bool isShowActiveInternshipsAsCompanyFormVisible = false;
        private bool isShowActiveThesesAsCompanyFormVisible = false;
        private bool isAnnouncementsFormVisibleToShowGeneralAnnouncementsAndEventsAsCompany = false;
        private bool isUploadedAnnouncementsVisible = false;
        private bool isUploadedEventsVisible = false;
        private bool isCompanySearchStudentVisible = false;
        private bool isCompanySearchProfessorVisible = false;
        private bool isCompanySearchResearchGroupVisible = false;

        // Search and filter properties
        private string companyNameSearch = "";
        private string emailSearch = "";
        private string positionTypeSearch = "";
        private string companyThesisSearch = "";
        private string companyEmailSearch = "";
        private string normalizedThesisSearch = "";
        private string normalizedEmailSearch = "";
        private string normalizedProfessorNameSearch = "";
        private string normalizedProfessorSurnameSearch = "";
        private string searchNameOrSurname = string.Empty;
        private List<string> nameSurnameSuggestions = new();
        private string searchNameAsCompanyToFindStudent = "";
        private string searchSurnameAsCompanyToFindStudent = "";
        private string searchRegNumberAsCompanyToFindStudent = "";
        private string searchDepartmentAsCompanyToFindStudent = "";
        private string searchSchoolAsCompanyToFindStudent = "";
        private string selectedDegreeLevel = string.Empty;
        private string InternshipStatus = string.Empty;
        private string ThesisStatus = string.Empty;
        private string searchAreasOfExpertise = string.Empty;
        private List<string> areasOfExpertiseSuggestions = new();
        private List<string> selectedAreasOfExpertise = new();
        private string searchKeywords = string.Empty;
        private List<string> keywordsSuggestions = new();
        private List<string> selectedKeywords = new();
        private string searchNameSurnameAsCompanyToFindProfessor = "";
        private string searchSchoolAsCompanyToFindProfessor = "";
        private string searchDepartmentAsCompanyToFindProfessor = "";
        private string searchAreasOfInterestAsCompanyToFindProfessor = "";
        private string searchResearchGroupNameAsCompanyToFindResearchGroup = "";
        private string searchResearchGroupSchoolAsCompanyToFindResearchGroup = "";
        private string searchResearchGroupUniversityDepartmentAsCompanyToFindResearchGroup = "";
        private string searchResearchGroupAreasAsCompanyToFindResearchGroup = "";
        private string searchResearchGroupSkillsAsCompanyToFindResearchGroup = "";
        private string searchResearchGroupKeywordsAsCompanyToFindResearchGroup = "";

        // Research group search helpers
        private bool hasSearchedForResearchGroups = false;
        private List<string> researchgroupNameSuggestions = new();
        private List<string> researchGroupAreasSuggestions = new();
        private List<string> researchGroupSkillsSuggestions = new();
        private List<string> researchGroupKeywordsSuggestions = new();
        private List<string> selectedResearchGroupAreas = new();
        private List<string> selectedResearchGroupSkills = new();
        private List<string> selectedResearchGroupKeywords = new();
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

        private List<string> researchGroupSchools => universityDepartments.Keys.ToList();

        private List<string> filteredDepartments =>
            string.IsNullOrEmpty(searchResearchGroupSchoolAsCompanyToFindResearchGroup)
                ? new List<string>()
                : universityDepartments.ContainsKey(searchResearchGroupSchoolAsCompanyToFindResearchGroup)
                    ? universityDepartments[searchResearchGroupSchoolAsCompanyToFindResearchGroup]
                    : new List<string>();

        private async Task OnSchoolSelectionChanged(ChangeEventArgs e)
        {
            searchResearchGroupSchoolAsCompanyToFindResearchGroup = e.Value?.ToString() ?? "";
            searchResearchGroupUniversityDepartmentAsCompanyToFindResearchGroup = "";
            await InvokeAsync(StateHasChanged);
        }

        private List<string> filteredProfessorDepartments =>
            string.IsNullOrEmpty(searchSchoolAsCompanyToFindProfessor)
                ? GetAllProfessorDepartments()
                : universityDepartments.TryGetValue(searchSchoolAsCompanyToFindProfessor, out var departments)
                    ? departments
                    : new List<string>();

        protected Task InvokeEditCompanyThesisDetailsAsync(CompanyThesis thesis) =>
            EditCompanyThesisDetailsRequested.HasDelegate
                ? EditCompanyThesisDetailsRequested.InvokeAsync(thesis)
                : Task.CompletedTask;

        protected Task InvokeUpdateThesisStatusAsCompanyAsync(int companyThesisId, string status) =>
            UpdateThesisStatusAsCompanyRequested.HasDelegate
                ? UpdateThesisStatusAsCompanyRequested.InvokeAsync(new ThesisStatusChangeRequest(companyThesisId, status))
                : Task.CompletedTask;

        protected Task InvokeToggleCompanyThesesExpandedAsync(long companyThesisRng) =>
            ToggleCompanyThesesExpandedRequested.HasDelegate
                ? ToggleCompanyThesesExpandedRequested.InvokeAsync(companyThesisRng)
                : Task.CompletedTask;

        protected Task InvokeToggleCompanyThesesExpandedForProfessorInterestAsync(long companyThesisRng) =>
            ToggleCompanyThesesExpandedForProfessorInterestRequested.HasDelegate
                ? ToggleCompanyThesesExpandedForProfessorInterestRequested.InvokeAsync(companyThesisRng)
                : Task.CompletedTask;

        protected Task InvokeConfirmAndAcceptStudentThesisApplicationAsCompanyAsync(long companyThesisId, string studentUniqueId) =>
            ConfirmAndAcceptStudentThesisApplicationAsCompanyRequested.HasDelegate
                ? ConfirmAndAcceptStudentThesisApplicationAsCompanyRequested.InvokeAsync(new CompanyThesisApplicationDecision(companyThesisId, studentUniqueId))
                : Task.CompletedTask;

        private List<string> GetAllProfessorDepartments() =>
            universityDepartments.Values.SelectMany(depts => depts).Distinct().ToList();

        // Data collections
        private List<Student>? searchResultsAsCompanyToFindStudent;
        private List<Professor> searchResultsAsCompanyToFindProfessor = new();
        private List<ResearchGroup> searchResultsAsCompanyToFindResearchGroup = new();
        private List<CompanyJobApplied> jobApplicationsmadeToCompany = new();
        private List<InternshipApplied> internshipApplications = new();
        private List<CompanyThesisApplied> companyThesisApplications = new();
        private List<InterestInCompanyEventAsProfessor> filteredProfessorInterestForCompanyEvents = new();
        private List<InterestInProfessorEventAsCompany> filteredCompanyInterestForProfessorEvents = new();
        private Professor? selectedProfessorWhenSearchForProfessorsAsCompany;
        private bool showProfessorDetailsModalWhenSearchForProfessorsAsCompany = false;
        private Student? selectedStudentWhenSearchForStudentsAsCompany;
        private bool showStudentDetailsModalWhenSearchForStudentsAsCompany = false;
        private Dictionary<string, Student> studentDataCache = new();
        private Student? selectedStudentFromCache;
        private InterestInCompanyEvent? selectedStudentToShowDetailsForInterestinCompanyEvent;
        private bool showModal = false;
        private ResearchGroup? selectedResearchGroupWhenSearchForResearchGroupsAsCompany;
        private bool showResearchGroupDetailsModalWhenSearchForResearchGroupsAsCompany = false;
        private int currentResearchGroupPage_SearchForResearchGroupsAsCompany = 1;
        private int ResearchGroupsPerPage_SearchForResearchGroupsAsCompany = 5;
        private List<int> pageSizeOptions_SearchForResearchGroupsAsCompany = new() { 5, 10, 20 };
        private List<FacultyMemberInfo> facultyMembers = new();
        private List<NonFacultyMemberInfo> nonFacultyMembers = new();
        private List<SpinOffCompanyInfo> spinOffCompanies = new();
        private int facultyMembersCount = 0;
        private int nonFacultyMembersCount = 0;
        private int activeResearchActionsCount = 0;
        private int patentsCount = 0;

        // Form objects
        private CompanyJob job = new();
        private CompanyInternship companyInternship = new();
        private CompanyThesis thesis = new();
        private AnnouncementAsCompany announcement = new();
        private CompanyEvent companyEvent = new();

        // Status and pagination
        private string selectedStatusFilterForInternships = "Όλα";
        private string selectedStatusFilterForJobs = "Όλα";
        private string selectedStatusFilterForCompanyTheses = "Όλα";
        private string selectedStatusFilterForAnnouncements = "Όλα";
        private string selectedStatusFilterForEventsAsCompany = "Όλα";
        private int currentPageForJobs = 1;
        private int currentPageForInternships = 1;
        private int currentPageForCompanyTheses = 1;
        private int currentPageForAnnouncements = 1;
        private int currentPageForEvents = 1;

        // Pagination options
        private int[] pageSizeOptions_SeeMyUploadedAnnouncementsAsCompany = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int[] pageSizeOptions_SeeMyUploadedJobsAsCompany = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int[] pageSizeOptions_SeeMyUploadedInternshipsAsCompany = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int[] pageSizeOptions_SeeMyUploadedThesesAsCompany = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int[] pageSizeOptions_SeeMyUploadedEventsAsCompany = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int[] pageSizeOptions_SearchForStudentsAsCompany = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int currentPageForStudents_SearchForStudentsAsCompany = 1;
        private int StudentsPerPage_SearchForStudentsAsCompany = 3;
        private int[] pageSizeOptions_SearchForProfessorsAsStudent = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // Areas and skills
        private List<Area> Areas = new();
        private List<Skill> Skills = new();
        private List<Area> SelectedAreasWhenUploadJobAsCompany = new();
        private List<Area> SelectedAreasWhenUploadInternshipAsCompany = new();
        private List<Area> SelectedAreasWhenUploadThesisAsCompany = new();
        private List<Area> SelectedAreasWhenUploadEventAsCompany = new();
        private List<Skill> SelectedSkillsWhenUploadThesisAsCompany = new();
        private List<Area> SelectedAreasToEditForCompanyJob = new();
        private List<Area> SelectedAreasToEditForCompanyInternship = new();
        private List<Area> SelectedAreasToEditForCompanyThesis = new();
        private List<Skill> SelectedSkillsToEditForCompanyThesis = new();

        // Status counts
        private int totalCount, publishedCount, unpublishedCount, withdrawnCount;
        private int totalCountForCompanyTheses, publishedCountForCompanyTheses, unpublishedCountForCompanyTheses, withdrawnCountForCompanyTheses;
        private int totalCountJobs, publishedCountJobs, unpublishedCountJobs, withdrawnCountJobs;
        private int totalCountAnnouncements, publishedCountAnnouncements, unpublishedCountAnnouncements, withdrawnCountAnnouncements;
        private int totalCountEventsAsCompany, publishedCountEventsAsCompany, unpublishedCountEventsAsCompany, withdrawnCountEventsAsCompany;

        // UI state
        private bool showSuccessMessage = false;
        private bool showErrorMessage = false;
        private bool showErrorMessagesForAreasWhenUploadJobAsCompany = false;
        private bool showErrorMessagesForAreasWhenUploadInternshipAsCompany = false;
        private bool showErrorMessagesForSkillsWhenUploadThesisAsCompany = false;
        private bool showErrorMessageforUploadingjobsAsCompany = false;
        private bool showErrorMessageforUploadinginternshipsAsCompany = false;
        private bool showErrorMessageforUploadingthesisAsCompany = false;
        private bool showErrorMessageforUploadingannouncementsAsCompany = false;
        private bool showErrorMessageForUploadingCompanyEvent = false;
        private bool showSuccessUpdateMessage = false;
        private bool isEditing = false;

        // Company data
        private string companyName = "";
        private string companyAreas = "";
        private string companyTelephone = "";
        private string companyWebsite = "";
        private byte[] companyLogo;
        private string companyDescription = "";
        private string companyShortName = "";
        private string companyType = "";
        private string companyActivity = "";
        private string companyCountry = "";
        private string companyLocation = "";
        private long? companyPermanentPC;
        private string companyRegions = "";
        private string companyTown = "";
        private string companyHRName = "";
        private string companyHRSurname = "";
        private string companyHREmail = "";
        private string companyHRTelephone = "";
        private string companyAdminName = "";
        private string companyAdminSurname = "";
        private string companyAdminEmail = "";
        private string companyAdminTelephone = "";

        // Form validation
        private bool isFormValidToSaveAnnouncementAsCompany = true;
        private bool isFormValidToSaveEventAsCompany = true;
        private string saveAnnouncementAsCompanyMessage = "";
        private string saveEventAsCompanyMessage = "";
        private bool isSaveAnnouncementAsCompanySuccessful = false;

        // Event handling
        private Dictionary<long, bool> positionDetails = new();
        private Dictionary<long, bool> expandedInternships = new();
        private Dictionary<long, bool> expandedJobs = new();
        private Dictionary<long, bool> expandedCompanyTheses = new();
        private Dictionary<long, bool> expandedCompanyThesesForProfessorInterest = new();
        private Dictionary<long, bool> expandedProfessorThesesForCompanyInterest = new();
        private Dictionary<long, IEnumerable<CompanyJobApplied>> jobApplicants = new();
        private Dictionary<long, IEnumerable<InternshipApplied>> internshipApplicants = new();
        private Dictionary<long, IEnumerable<CompanyThesis>> companyThesesProfessors = new();
        private List<string> professorNameSurnameSuggestions = new();
        private List<string> areasOfInterestSuggestions = new();
        private List<string> selectedAreasOfInterest = new();
        private int ProfessorsPerPage_SearchForProfessorsAsStudent = 3;
        private int currentProfessorPage_SearchForProfessorsAsStudent = 1;

        // Pagination
        private int JobsPerPage = 3;
        private int InternshipsPerPage = 3;
        private int CompanyThesesPerPage = 3;
        private int EventsPerPage = 3;
        private int pageSize = 3;
        private int pageSizeForAnnouncements = 3;

        // Component initialization
        protected override async Task OnInitializedAsync()
        {
            isInitializedAsCompanyUser = false;
            isCompanyRegistered = false;

            await LoadAreasAsync();
            await LoadSkillsAsync();
            await LoadCompanyData();
            await LoadJobs();
            await LoadInternships();
            await LoadTheses();
            await LoadAnnouncements();
            await LoadEvents();
            await CalculateStatusCounts();

            isInitializedAsCompanyUser = true;
        }

        // Data loading methods
        private async Task LoadAreasAsync()
        {
            Areas = await dbContext.Areas.ToListAsync();
        }

        private async Task LoadSkillsAsync()
        {
            Skills = await dbContext.Skills.ToListAsync();
        }

        private async Task LoadCompanyData()
        {
            companyData = null;
            isCompanyRegistered = false;

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (!(user.Identity?.IsAuthenticated ?? false))
            {
                return;
            }

            var userEmail = user.FindFirst("name")?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return;
            }

            companyData = await dbContext.Companies.FirstOrDefaultAsync(c => c.CompanyEmail == userEmail);
            isCompanyRegistered = companyData != null;

            if (!isCompanyRegistered)
            {
                return;
            }

            // Load company details
            companyName = companyData.CompanyName;
            companyAreas = companyData.CompanyAreas;
            companyTelephone = companyData.CompanyTelephone;
            companyWebsite = companyData.CompanyWebsite;
            companyLogo = companyData.CompanyLogo;
            companyDescription = companyData.CompanyDescription;
            companyShortName = companyData.CompanyShortName;
            companyType = companyData.CompanyType;
            companyActivity = companyData.CompanyActivity;
            companyCountry = companyData.CompanyCountry;
            companyLocation = companyData.CompanyLocation;
            companyPermanentPC = companyData.CompanyPC;
            companyRegions = companyData.CompanyRegions;
            companyTown = companyData.CompanyTown;
            companyHRName = companyData.CompanyHRName;
            companyHRSurname = companyData.CompanyHRSurname;
            companyHREmail = companyData.CompanyHREmail;
            companyHRTelephone = companyData.CompanyHRTelephone;
            companyAdminName = companyData.CompanyAdminName;
            companyAdminSurname = companyData.CompanyAdminSurname;
            companyAdminEmail = companyData.CompanyAdminEmail;
            companyAdminTelephone = companyData.CompanyAdminTelephone;
        }

        private async Task LoadJobs()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var userEmail = user.FindFirst("name")?.Value;
                if (!string.IsNullOrEmpty(userEmail))
                {
                    jobs = await dbContext.CompanyJobs
                        .Where(j => j.EmailUsedToUploadJobs == userEmail)
                        .ToListAsync();
                }
            }
        }

        private async Task LoadInternships()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var userEmail = user.FindFirst("name")?.Value;
                if (!string.IsNullOrEmpty(userEmail))
                {
                    internships = await dbContext.CompanyInternships
                        .Where(i => i.CompanyEmailUsedToUploadInternship == userEmail)
                        .ToListAsync();
                }
            }
        }

        private async Task LoadTheses()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var userEmail = user.FindFirst("name")?.Value;
                if (!string.IsNullOrEmpty(userEmail))
                {
                    companytheses = await dbContext.CompanyTheses
                        .Where(t => t.CompanyEmailUsedToUploadThesis == userEmail)
                        .ToListAsync();
                }
            }
        }

        private async Task LoadAnnouncements()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var userEmail = user.FindFirst("name")?.Value;
                if (!string.IsNullOrEmpty(userEmail))
                {
                    announcements = await dbContext.AnnouncementsAsCompany
                        .Where(a => a.CompanyAnnouncementCompanyEmail == userEmail)
                        .ToListAsync();
                }
            }
        }

        private async Task LoadEvents()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var userEmail = user.FindFirst("name")?.Value;
                if (!string.IsNullOrEmpty(userEmail))
                {
                    companyEvents = await dbContext.CompanyEvents
                        .Where(e => e.CompanyEmailUsedToUploadEvent == userEmail)
                        .ToListAsync();
                }
            }
        }

        private async Task CalculateStatusCounts()
        {
            await CalculateStatusCountsForInternships();
            await CalculateStatusCountsForJobs();
            await CalculateStatusCountsForCompanyTheses();
            await CalculateStatusCountsForAnnouncements();
        }

        private async Task CalculateStatusCountsForInternships()
        {
            totalCount = internships.Count();
            publishedCount = internships.Count(i => i.CompanyUploadedInternshipStatus == "Δημοσιευμένη");
            unpublishedCount = internships.Count(i => i.CompanyUploadedInternshipStatus == "Μη Δημοσιευμένη");
            withdrawnCount = internships.Count(i => i.CompanyUploadedInternshipStatus == "Αποσυρμένη");
        }

        private async Task CalculateStatusCountsForJobs()
        {
            totalCountJobs = jobs.Count();
            publishedCountJobs = jobs.Count(j => j.PositionStatus == "Δημοσιευμένη");
            unpublishedCountJobs = jobs.Count(j => j.PositionStatus == "Μη Δημοσιευμένη");
            withdrawnCountJobs = jobs.Count(j => j.PositionStatus == "Αποσυρμένη");
        }

        private async Task CalculateStatusCountsForCompanyTheses()
        {
            totalCountForCompanyTheses = companytheses.Count();
            publishedCountForCompanyTheses = companytheses.Count(t => t.CompanyThesisStatus == "Δημοσιευμένη");
            unpublishedCountForCompanyTheses = companytheses.Count(t => t.CompanyThesisStatus == "Μη Δημοσιευμένη");
            withdrawnCountForCompanyTheses = companytheses.Count(t => t.CompanyThesisStatus == "Αποσυρμένη");
        }

        private async Task CalculateStatusCountsForAnnouncements()
        {
            totalCountAnnouncements = announcements.Count();
            publishedCountAnnouncements = announcements.Count(a => a.CompanyAnnouncementStatus == "Δημοσιευμένη");
            unpublishedCountAnnouncements = announcements.Count(a => a.CompanyAnnouncementStatus == "Μη Δημοσιευμένη");
        }

        // UI toggle methods
        private void ToggleFormVisibilityForUploadCompanyJobs()
        {
            isForm1Visible = !isForm1Visible;
        }

        private void ToggleFormVisibilityForUploadCompanyAnnouncements()
        {
            isAnnouncementsFormVisible = !isAnnouncementsFormVisible;
        }

        private void ToggleFormVisibilityToShowGeneralAnnouncementsAndEventsAsCompany()
        {
            isAnnouncementsFormVisibleToShowGeneralAnnouncementsAndEventsAsCompany = !isAnnouncementsFormVisibleToShowGeneralAnnouncementsAndEventsAsCompany;
        }

        private void ToggleFormVisibilityForUploadCompanyInternships()
        {
            isUploadCompanyInternshipsFormVisible = !isUploadCompanyInternshipsFormVisible;
        }

        private void ToggleFormVisibilityForUploadCompanyThesis()
        {
            isUploadCompanyThesisFormVisible = !isUploadCompanyThesisFormVisible;
        }

        private void ToggleFormVisibilityForUploadCompanyEvent()
        {
            isUploadCompanyEventFormVisible = !isUploadCompanyEventFormVisible;
        }

        private async Task ToggleFormVisibilityToShowMyActiveJobsAsCompany()
        {
            isForm2Visible = !isForm2Visible;
            if (isForm2Visible)
            {
                await LoadJobs();
            }
        }

        private void ToggleFormVisibilityToShowMyActiveInternshipsAsCompany()
        {
            isShowActiveInternshipsAsCompanyFormVisible = !isShowActiveInternshipsAsCompanyFormVisible;
        }

        private async Task ToggleFormVisibilityToShowMyActiveThesesAsCompany()
        {
            isShowActiveThesesAsCompanyFormVisible = !isShowActiveThesesAsCompanyFormVisible;
            if (isShowActiveThesesAsCompanyFormVisible)
            {
                await LoadTheses();
            }
        }

        private void ToggleUploadedAnnouncementsVisibility()
        {
            isUploadedAnnouncementsVisible = !isUploadedAnnouncementsVisible;
        }

        private void ToggleUploadedEventsVisibility()
        {
            isUploadedEventsVisible = !isUploadedEventsVisible;
        }

        private void ToggleCompanySearchStudentVisible()
        {
            isCompanySearchStudentVisible = !isCompanySearchStudentVisible;
        }

        private void ToggleCompanySearchProfessorVisible()
        {
            isCompanySearchProfessorVisible = !isCompanySearchProfessorVisible;
        }

        private void ToggleCompanySearchResearchGroupVisible()
        {
            isCompanySearchResearchGroupVisible = !isCompanySearchResearchGroupVisible;
        }

        // Form submission methods
        private async Task UploadJobAsCompany(bool publishJob = false)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (string.IsNullOrEmpty(userEmail)) return;

            // Validation
            if (string.IsNullOrWhiteSpace(job.PositionType) ||
                string.IsNullOrWhiteSpace(job.PositionTitle) ||
                string.IsNullOrWhiteSpace(job.PositionContactPerson) ||
                string.IsNullOrWhiteSpace(job.PositionPerifereiaLocation) ||
                string.IsNullOrWhiteSpace(job.PositionDimosLocation) ||
                string.IsNullOrWhiteSpace(job.PositionDescription) ||
                string.IsNullOrWhiteSpace(job.PositionAddressLocation) ||
                !SelectedAreasWhenUploadJobAsCompany.Any())
            {
                showErrorMessageforUploadingjobsAsCompany = true;
                return;
            }

            // Create job
            job.RNGForPositionUploaded = new Random().NextInt64();
            job.RNGForPositionUploaded_HashedAsUniqueID = HashingHelper.HashLong(job.RNGForPositionUploaded);
            job.EmailUsedToUploadJobs = userEmail;
            job.UploadDateTime = DateTime.Now;
            job.PositionForeas = companyData.CompanyType;
            job.PositionAreas = string.Join(",", SelectedAreasWhenUploadJobAsCompany.Select(a => a.AreaName));
            job.PositionStatus = publishJob ? "Δημοσιευμένη" : "Μη Δημοσιευμένη";

            dbContext.CompanyJobs.Add(job);
            await dbContext.SaveChangesAsync();

            showSuccessMessage = true;
            showErrorMessageforUploadingjobsAsCompany = false;

            // Reset form
            job = new CompanyJob();
            SelectedAreasWhenUploadJobAsCompany.Clear();
        }

        private async Task UploadInternshipAsCompany()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (string.IsNullOrEmpty(userEmail)) return;

            // Validation
            if (string.IsNullOrWhiteSpace(companyInternship.CompanyInternshipTitle) ||
                string.IsNullOrWhiteSpace(companyInternship.CompanyInternshipType) ||
                string.IsNullOrWhiteSpace(companyInternship.CompanyInternshipESPA) ||
                string.IsNullOrWhiteSpace(companyInternship.CompanyInternshipContactPerson) ||
                string.IsNullOrWhiteSpace(companyInternship.CompanyInternshipDescription) ||
                string.IsNullOrWhiteSpace(companyInternship.CompanyInternshipPerifereiaLocation) ||
                string.IsNullOrWhiteSpace(companyInternship.CompanyInternshipDimosLocation) ||
                !SelectedAreasWhenUploadInternshipAsCompany.Any())
            {
                showErrorMessage = true;
                return;
            }

            // Create internship
            companyInternship.RNGForInternshipUploadedAsCompany = new Random().NextInt64();
            companyInternship.RNGForInternshipUploadedAsCompany_HashedAsUniqueID = HashingHelper.HashLong(companyInternship.RNGForInternshipUploadedAsCompany);
            companyInternship.CompanyEmailUsedToUploadInternship = userEmail;
            companyInternship.CompanyInternshipUploadDate = DateTime.Now;
            companyInternship.CompanyInternshipForeas = companyData.CompanyType;
            companyInternship.CompanyInternshipAreas = string.Join(",", SelectedAreasWhenUploadInternshipAsCompany.Select(a => a.AreaName));
            companyInternship.CompanyUploadedInternshipStatus = "Μη Δημοσιευμένη";

            dbContext.CompanyInternships.Add(companyInternship);
            await dbContext.SaveChangesAsync();

            showSuccessMessage = true;
            showErrorMessage = false;

            // Reset form
            companyInternship = new CompanyInternship();
            SelectedAreasWhenUploadInternshipAsCompany.Clear();
        }

        private async Task UploadThesisAsCompany(bool publishThesis = false)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (string.IsNullOrEmpty(userEmail)) return;

            // Validation
            if (string.IsNullOrWhiteSpace(thesis.CompanyThesisTitle) ||
                string.IsNullOrWhiteSpace(thesis.CompanyThesisDescriptionsUploaded) ||
                string.IsNullOrWhiteSpace(thesis.CompanyThesisCompanySupervisorFullName) ||
                string.IsNullOrWhiteSpace(thesis.CompanyThesisContactPersonEmail) ||
                !SelectedAreasWhenUploadThesisAsCompany.Any() ||
                !SelectedSkillsWhenUploadThesisAsCompany.Any())
            {
                showErrorMessageforUploadingthesisAsCompany = true;
                return;
            }

            // Create thesis
            thesis.RNGForThesisUploadedAsCompany = new Random().NextInt64();
            thesis.RNGForThesisUploadedAsCompany_HashedAsUniqueID = HashingHelper.HashLong(thesis.RNGForThesisUploadedAsCompany);
            thesis.CompanyEmailUsedToUploadThesis = userEmail;
            thesis.CompanyThesisUploadDateTime = DateTime.Now;
            thesis.ThesisType = ThesisType.Company;
            thesis.CompanyThesisAreasUpload = string.Join(",", SelectedAreasWhenUploadThesisAsCompany.Select(a => a.AreaName));
            thesis.CompanyThesisSkillsNeeded = string.Join(",", SelectedSkillsWhenUploadThesisAsCompany.Select(s => s.SkillName));
            thesis.CompanyThesisStatus = publishThesis ? "Δημοσιευμένη" : "Μη Δημοσιευμένη";

            dbContext.CompanyTheses.Add(thesis);
            await dbContext.SaveChangesAsync();

            showSuccessMessage = true;
            showErrorMessageforUploadingthesisAsCompany = false;

            // Reset form
            thesis = new CompanyThesis();
            SelectedAreasWhenUploadThesisAsCompany.Clear();
            SelectedSkillsWhenUploadThesisAsCompany.Clear();
        }

        private async Task SaveAnnouncementAsCompany(bool publishAnnouncement = false)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (string.IsNullOrEmpty(userEmail)) return;

            // Validation
            if (string.IsNullOrWhiteSpace(announcement.CompanyAnnouncementTitle) ||
                string.IsNullOrWhiteSpace(announcement.CompanyAnnouncementDescription))
            {
                showErrorMessageforUploadingannouncementsAsCompany = true;
                return;
            }

            // Create announcement
            announcement.CompanyAnnouncementRNG = new Random().NextInt64();
            announcement.CompanyAnnouncementRNG_HashedAsUniqueID = HashingHelper.HashLong(announcement.CompanyAnnouncementRNG ?? 0);
            announcement.CompanyAnnouncementCompanyEmail = userEmail;
            announcement.CompanyAnnouncementUploadDate = DateTime.Now;
            announcement.CompanyAnnouncementStatus = publishAnnouncement ? "Δημοσιευμένη" : "Μη Δημοσιευμένη";

            dbContext.AnnouncementsAsCompany.Add(announcement);
            await dbContext.SaveChangesAsync();

            showSuccessMessage = true;
            showErrorMessageforUploadingannouncementsAsCompany = false;

            // Reset form
            announcement = new AnnouncementAsCompany();
        }

        // Search methods
        private void SearchStudentsAsCompanyToFindStudent()
        {
            // Implementation for student search
        }

        private int totalPagesForStudents_SearchForStudentsAsCompany =>
            Math.Max(1,
                (int)Math.Ceiling((double)(searchResultsAsCompanyToFindStudent?.Count ?? 0) /
                                  Math.Max(1, StudentsPerPage_SearchForStudentsAsCompany)));

        private IEnumerable<Student> GetPaginatedStudents_SearchForStudentsAsCompany()
        {
            if (searchResultsAsCompanyToFindStudent == null || searchResultsAsCompanyToFindStudent.Count == 0)
            {
                return Enumerable.Empty<Student>();
            }

            return searchResultsAsCompanyToFindStudent
                .Skip((currentPageForStudents_SearchForStudentsAsCompany - 1) * StudentsPerPage_SearchForStudentsAsCompany)
                .Take(StudentsPerPage_SearchForStudentsAsCompany);
        }

        private void GoToFirstPageForStudents_SearchForStudentsAsCompany()
        {
            currentPageForStudents_SearchForStudentsAsCompany = 1;
            StateHasChanged();
        }

        private void GoToLastPageForStudents_SearchForStudentsAsCompany()
        {
            currentPageForStudents_SearchForStudentsAsCompany =
                Math.Max(1, totalPagesForStudents_SearchForStudentsAsCompany);
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
            if (currentPageForStudents_SearchForStudentsAsCompany <
                Math.Max(1, totalPagesForStudents_SearchForStudentsAsCompany))
            {
                currentPageForStudents_SearchForStudentsAsCompany++;
                StateHasChanged();
            }
        }

        private void GoToPageForStudents_SearchForStudentsAsCompany(int page)
        {
            if (page > 0 && page <= Math.Max(1, totalPagesForStudents_SearchForStudentsAsCompany))
            {
                currentPageForStudents_SearchForStudentsAsCompany = page;
                StateHasChanged();
            }
        }

        private List<int> GetVisiblePagesForStudents_SearchForStudentsAsCompany()
        {
            var pages = new List<int>();
            int total = Math.Max(1, totalPagesForStudents_SearchForStudentsAsCompany);
            int current = Math.Min(currentPageForStudents_SearchForStudentsAsCompany, total);

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

        private void OnPageSizeChange_SearchForStudentsAsCompany(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                StudentsPerPage_SearchForStudentsAsCompany = newSize;
                currentPageForStudents_SearchForStudentsAsCompany = 1;
                StateHasChanged();
            }
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

        private async Task DownloadStudentAttachmentAsCompanyInSearchForStudents(long studentId)
        {
            var student = await dbContext.Students
                .Where(s => s.Id == (int)studentId)
                .FirstOrDefaultAsync();

            if (student?.Attachment != null)
            {
                string fileName = $"{student.Name}_{student.Surname}_CV.pdf";
                const string mimeType = "application/pdf";
                string base64Data = Convert.ToBase64String(student.Attachment);
                await JS.InvokeVoidAsync("downloadFile", fileName, mimeType, base64Data);
            }
        }

        private void HandleProfessorSchoolChanged(ChangeEventArgs e)
        {
            searchSchoolAsCompanyToFindProfessor = e.Value?.ToString() ?? string.Empty;
            searchDepartmentAsCompanyToFindProfessor = string.Empty;
        }

        private void HandleProfessorInput(ChangeEventArgs e)
        {
            searchNameSurnameAsCompanyToFindProfessor = e.Value?.ToString()?.Trim() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(searchNameSurnameAsCompanyToFindProfessor) &&
                searchNameSurnameAsCompanyToFindProfessor.Length >= 2)
            {
                professorNameSurnameSuggestions = dbContext.Professors
                    .Where(p => (p.ProfName + " " + p.ProfSurname)
                        .Contains(searchNameSurnameAsCompanyToFindProfessor))
                    .Select(p => $"{p.ProfName} {p.ProfSurname}")
                    .Distinct()
                    .Take(10)
                    .ToList();
            }
            else
            {
                professorNameSurnameSuggestions.Clear();
            }
        }

        private void SelectProfessorNameSurnameSuggestion(string suggestion)
        {
            searchNameSurnameAsCompanyToFindProfessor = suggestion;
            professorNameSurnameSuggestions.Clear();
        }

        private async Task HandleAreasOfInterestInput(ChangeEventArgs e)
        {
            searchAreasOfInterestAsCompanyToFindProfessor = e.Value?.ToString()?.Trim() ?? string.Empty;
            areasOfInterestSuggestions = new List<string>();

            if (searchAreasOfInterestAsCompanyToFindProfessor.Length >= 1)
            {
                try
                {
                    areasOfInterestSuggestions = await dbContext.Areas
                        .Where(a => a.AreaName.Contains(searchAreasOfInterestAsCompanyToFindProfessor))
                        .Select(a => a.AreaName)
                        .Distinct()
                        .Take(10)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching areas of interest: {ex.Message}");
                    areasOfInterestSuggestions.Clear();
                }
            }
        }

        private void SelectAreasOfInterestSuggestion(string suggestion)
        {
            if (!string.IsNullOrWhiteSpace(suggestion) && !selectedAreasOfInterest.Contains(suggestion))
            {
                selectedAreasOfInterest.Add(suggestion);
                areasOfInterestSuggestions.Clear();
                searchAreasOfInterestAsCompanyToFindProfessor = string.Empty;
            }
        }

        private void RemoveSelectedAreaOfInterest(string area)
        {
            selectedAreasOfInterest.Remove(area);
        }

        private async Task SearchProfessorsAsCompanyToFindProfessor()
        {
            var professorsQuery = dbContext.Professors.AsQueryable();

            if (!string.IsNullOrEmpty(searchNameSurnameAsCompanyToFindProfessor))
            {
                professorsQuery = professorsQuery.Where(p =>
                    (p.ProfName + " " + p.ProfSurname)
                        .Contains(searchNameSurnameAsCompanyToFindProfessor));
            }

            if (!string.IsNullOrEmpty(searchSchoolAsCompanyToFindProfessor) &&
                universityDepartments.TryGetValue(searchSchoolAsCompanyToFindProfessor, out var departmentsForSchool))
            {
                professorsQuery = professorsQuery.Where(p =>
                    !string.IsNullOrEmpty(p.ProfDepartment) &&
                    departmentsForSchool.Contains(p.ProfDepartment));
            }

            if (!string.IsNullOrEmpty(searchDepartmentAsCompanyToFindProfessor))
            {
                professorsQuery = professorsQuery.Where(p =>
                    p.ProfDepartment == searchDepartmentAsCompanyToFindProfessor);
            }

            var professorsList = await professorsQuery.ToListAsync();

            searchResultsAsCompanyToFindProfessor = professorsList
                .Where(p =>
                    string.IsNullOrEmpty(searchAreasOfInterestAsCompanyToFindProfessor) &&
                    !selectedAreasOfInterest.Any()
                    ||
                    (!string.IsNullOrEmpty(p.ProfGeneralFieldOfWork) &&
                        (selectedAreasOfInterest.Any(area =>
                            p.ProfGeneralFieldOfWork.Contains(area, StringComparison.OrdinalIgnoreCase)) ||
                         p.ProfGeneralFieldOfWork.Contains(searchAreasOfInterestAsCompanyToFindProfessor ?? string.Empty,
                             StringComparison.OrdinalIgnoreCase))))
                .ToList();

            currentProfessorPage_SearchForProfessorsAsStudent = 1;
        }

        private IEnumerable<Professor> GetPaginatedProfessorResults()
        {
            return (searchResultsAsCompanyToFindProfessor ?? new List<Professor>())
                .Skip((currentProfessorPage_SearchForProfessorsAsStudent - 1) * ProfessorsPerPage_SearchForProfessorsAsStudent)
                .Take(ProfessorsPerPage_SearchForProfessorsAsStudent);
        }

        private int totalProfessorPages_SearchForProfessorsAsStudent =>
            Math.Max(1,
                (int)Math.Ceiling((double)(searchResultsAsCompanyToFindProfessor?.Count ?? 0) /
                                  Math.Max(1, ProfessorsPerPage_SearchForProfessorsAsStudent)));

        private List<int> GetVisibleProfessorPages()
        {
            var pages = new List<int>();
            int total = totalProfessorPages_SearchForProfessorsAsStudent;
            int current = currentProfessorPage_SearchForProfessorsAsStudent;

            pages.Add(1);

            if (current > 3) pages.Add(-1);

            int start = Math.Max(2, current - 1);
            int end = Math.Min(total - 1, current + 1);

            for (int i = start; i <= end; i++)
            {
                pages.Add(i);
            }

            if (current < total - 2) pages.Add(-1);

            if (total > 1) pages.Add(total);

            return pages;
        }

        private void GoToFirstProfessorPage()
        {
            currentProfessorPage_SearchForProfessorsAsStudent = 1;
        }

        private void PreviousProfessorPage()
        {
            if (currentProfessorPage_SearchForProfessorsAsStudent > 1)
            {
                currentProfessorPage_SearchForProfessorsAsStudent--;
            }
        }

        private void NextProfessorPage()
        {
            if (currentProfessorPage_SearchForProfessorsAsStudent < totalProfessorPages_SearchForProfessorsAsStudent)
            {
                currentProfessorPage_SearchForProfessorsAsStudent++;
            }
        }

        private void GoToProfessorPage(int pageNumber)
        {
            if (pageNumber >= 1 && pageNumber <= totalProfessorPages_SearchForProfessorsAsStudent)
            {
                currentProfessorPage_SearchForProfessorsAsStudent = pageNumber;
            }
        }

        private void GoToLastProfessorPage()
        {
            currentProfessorPage_SearchForProfessorsAsStudent = totalProfessorPages_SearchForProfessorsAsStudent;
        }

        private void OnPageSizeChange_SearchForProfessorsAsStudent(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                ProfessorsPerPage_SearchForProfessorsAsStudent = newSize;
                currentProfessorPage_SearchForProfessorsAsStudent = 1;
            }
        }

        private void ClearSearchFieldsAsCompanyToFindProfessor()
        {
            searchNameSurnameAsCompanyToFindProfessor = string.Empty;
            searchSchoolAsCompanyToFindProfessor = string.Empty;
            searchDepartmentAsCompanyToFindProfessor = string.Empty;
            searchAreasOfInterestAsCompanyToFindProfessor = string.Empty;
            selectedAreasOfInterest.Clear();
            professorNameSurnameSuggestions.Clear();
            areasOfInterestSuggestions.Clear();
            searchResultsAsCompanyToFindProfessor = new List<Professor>();
            currentProfessorPage_SearchForProfessorsAsStudent = 1;
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

        private async Task HandleResearchGroupNameInput(ChangeEventArgs e)
        {
            searchResearchGroupNameAsCompanyToFindResearchGroup = e.Value?.ToString().Trim() ?? string.Empty;
            researchgroupNameSuggestions.Clear();

            if (searchResearchGroupNameAsCompanyToFindResearchGroup.Length >= 1)
            {
                try
                {
                    researchgroupNameSuggestions = await dbContext.ResearchGroups
                        .Where(rg => !string.IsNullOrEmpty(rg.ResearchGroupName) &&
                                     rg.ResearchGroupName.Contains(searchResearchGroupNameAsCompanyToFindResearchGroup))
                        .Select(rg => rg.ResearchGroupName!)
                        .Distinct()
                        .Take(10)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Πρόβλημα στην Ανάκτηση Ονομάτων Ερευνητικών Ομάδων: {ex.Message}");
                    researchgroupNameSuggestions.Clear();
                }
            }

            await InvokeAsync(StateHasChanged);
        }

        private async Task HandleResearchGroupAreasInput(ChangeEventArgs e)
        {
            searchResearchGroupAreasAsCompanyToFindResearchGroup = e.Value?.ToString().Trim() ?? string.Empty;
            researchGroupAreasSuggestions.Clear();

            if (searchResearchGroupAreasAsCompanyToFindResearchGroup.Length >= 1)
            {
                try
                {
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
                    researchGroupAreasSuggestions.Clear();
                }
            }

            await InvokeAsync(StateHasChanged);
        }

        private void HandleResearchGroupAreasKeyDown(KeyboardEventArgs e)
        {
            if ((e.Key == "Enter" || e.Key == "Tab") &&
                !string.IsNullOrWhiteSpace(searchResearchGroupAreasAsCompanyToFindResearchGroup) &&
                !selectedResearchGroupAreas.Contains(searchResearchGroupAreasAsCompanyToFindResearchGroup))
            {
                selectedResearchGroupAreas.Add(searchResearchGroupAreasAsCompanyToFindResearchGroup);
                searchResearchGroupAreasAsCompanyToFindResearchGroup = string.Empty;
                researchGroupAreasSuggestions.Clear();
            }
        }

        private async Task HandleResearchGroupSkillsInput(ChangeEventArgs e)
        {
            searchResearchGroupSkillsAsCompanyToFindResearchGroup = e.Value?.ToString().Trim() ?? string.Empty;
            researchGroupSkillsSuggestions.Clear();

            if (searchResearchGroupSkillsAsCompanyToFindResearchGroup.Length >= 1)
            {
                try
                {
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
                    researchGroupSkillsSuggestions.Clear();
                }
            }

            await InvokeAsync(StateHasChanged);
        }

        private void HandleResearchGroupSkillsKeyDown(KeyboardEventArgs e)
        {
            if ((e.Key == "Enter" || e.Key == "Tab") &&
                !string.IsNullOrWhiteSpace(searchResearchGroupSkillsAsCompanyToFindResearchGroup) &&
                !selectedResearchGroupSkills.Contains(searchResearchGroupSkillsAsCompanyToFindResearchGroup))
            {
                selectedResearchGroupSkills.Add(searchResearchGroupSkillsAsCompanyToFindResearchGroup);
                searchResearchGroupSkillsAsCompanyToFindResearchGroup = string.Empty;
                researchGroupSkillsSuggestions.Clear();
            }
        }

        private async Task HandleResearchGroupKeywordsInput(ChangeEventArgs e)
        {
            searchResearchGroupKeywordsAsCompanyToFindResearchGroup = e.Value?.ToString().Trim() ?? string.Empty;
            researchGroupKeywordsSuggestions.Clear();

            if (searchResearchGroupKeywordsAsCompanyToFindResearchGroup.Length >= 1)
            {
                try
                {
                    var allResearchGroups = await dbContext.ResearchGroups.ToListAsync();

                    researchGroupKeywordsSuggestions = allResearchGroups
                        .Where(rg => !string.IsNullOrEmpty(rg.ResearchGroupKeywords))
                        .SelectMany(rg => (rg.ResearchGroupKeywords ?? string.Empty)
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(keyword => keyword.Trim()))
                        .Where(keyword => keyword.Contains(searchResearchGroupKeywordsAsCompanyToFindResearchGroup, StringComparison.OrdinalIgnoreCase))
                        .Distinct()
                        .Take(10)
                        .ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Πρόβλημα στην Ανάκτηση Λέξεων Κλειδιών: {ex.Message}");
                    researchGroupKeywordsSuggestions.Clear();
                }
            }

            await InvokeAsync(StateHasChanged);
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
        }

        private void RemoveSelectedResearchGroupSkill(string skill)
        {
            selectedResearchGroupSkills.Remove(skill);
        }

        private void RemoveSelectedResearchGroupKeyword(string keyword)
        {
            selectedResearchGroupKeywords.Remove(keyword);
        }

        private async Task SearchResearchGroupsAsCompany()
        {
            try
            {
                hasSearchedForResearchGroups = true;

                var allResearchGroups = await dbContext.ResearchGroups.ToListAsync();
                var filteredResearchGroups = allResearchGroups.AsEnumerable();

                if (!string.IsNullOrEmpty(searchResearchGroupNameAsCompanyToFindResearchGroup))
                {
                    filteredResearchGroups = filteredResearchGroups
                        .Where(rg => !string.IsNullOrEmpty(rg.ResearchGroupName) &&
                                     rg.ResearchGroupName.Contains(searchResearchGroupNameAsCompanyToFindResearchGroup, StringComparison.OrdinalIgnoreCase));
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

                if (selectedResearchGroupAreas.Any() || !string.IsNullOrEmpty(searchResearchGroupAreasAsCompanyToFindResearchGroup))
                {
                    var areaSearchTerms = new List<string>();

                    if (!string.IsNullOrEmpty(searchResearchGroupAreasAsCompanyToFindResearchGroup))
                    {
                        areaSearchTerms.Add(searchResearchGroupAreasAsCompanyToFindResearchGroup.Trim());
                    }

                    areaSearchTerms.AddRange(selectedResearchGroupAreas);
                    areaSearchTerms = areaSearchTerms.Where(a => !string.IsNullOrWhiteSpace(a)).Distinct().ToList();

                    filteredResearchGroups = filteredResearchGroups
                        .Where(rg =>
                        {
                            var rgAreas = (rg.ResearchGroupAreas ?? string.Empty)
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(a => a.Trim());

                            return areaSearchTerms.Any(area => rgAreas.Any(a => a.Contains(area, StringComparison.OrdinalIgnoreCase)));
                        });
                }

                if (selectedResearchGroupSkills.Any() || !string.IsNullOrEmpty(searchResearchGroupSkillsAsCompanyToFindResearchGroup))
                {
                    var skillSearchTerms = new List<string>();

                    if (!string.IsNullOrEmpty(searchResearchGroupSkillsAsCompanyToFindResearchGroup))
                    {
                        skillSearchTerms.Add(searchResearchGroupSkillsAsCompanyToFindResearchGroup.Trim());
                    }

                    skillSearchTerms.AddRange(selectedResearchGroupSkills);
                    skillSearchTerms = skillSearchTerms.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

                    filteredResearchGroups = filteredResearchGroups
                        .Where(rg =>
                        {
                            var rgSkills = (rg.ResearchGroupSkills ?? string.Empty)
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(s => s.Trim());

                            return skillSearchTerms.Any(skill => rgSkills.Any(s => s.Contains(skill, StringComparison.OrdinalIgnoreCase)));
                        });
                }

                if (!string.IsNullOrEmpty(searchResearchGroupKeywordsAsCompanyToFindResearchGroup))
                {
                    filteredResearchGroups = filteredResearchGroups
                        .Where(rg => !string.IsNullOrEmpty(rg.ResearchGroupKeywords) &&
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

            await InvokeAsync(StateHasChanged);
        }

        private IEnumerable<ResearchGroup> GetPaginatedResearchGroupResults()
        {
            return searchResultsAsCompanyToFindResearchGroup
                .Skip((currentResearchGroupPage_SearchForResearchGroupsAsCompany - 1) * ResearchGroupsPerPage_SearchForResearchGroupsAsCompany)
                .Take(ResearchGroupsPerPage_SearchForResearchGroupsAsCompany);
        }

        private int totalResearchGroupPages_SearchForResearchGroupsAsCompany =>
            (int)Math.Ceiling((double)(searchResultsAsCompanyToFindResearchGroup.Count) / ResearchGroupsPerPage_SearchForResearchGroupsAsCompany);

        private List<int> GetVisibleResearchGroupPages()
        {
            var pages = new List<int>();
            int currentPage = currentResearchGroupPage_SearchForResearchGroupsAsCompany;
            int totalPages = totalResearchGroupPages_SearchForResearchGroupsAsCompany;

            pages.Add(1);

            if (currentPage > 3)
            {
                pages.Add(-1);
            }

            int start = Math.Max(2, currentPage - 1);
            int end = Math.Min(totalPages - 1, currentPage + 1);

            for (int i = start; i <= end; i++)
            {
                pages.Add(i);
            }

            if (currentPage < totalPages - 2)
            {
                pages.Add(-1);
            }

            if (totalPages > 1)
            {
                pages.Add(totalPages);
            }

            return pages;
        }

        private void GoToResearchGroupPage(int pageNumber)
        {
            currentResearchGroupPage_SearchForResearchGroupsAsCompany = pageNumber;
        }

        private void GoToFirstResearchGroupPage()
        {
            currentResearchGroupPage_SearchForResearchGroupsAsCompany = 1;
        }

        private void PreviousResearchGroupPage()
        {
            if (currentResearchGroupPage_SearchForResearchGroupsAsCompany > 1)
            {
                currentResearchGroupPage_SearchForResearchGroupsAsCompany--;
            }
        }

        private void NextResearchGroupPage()
        {
            if (currentResearchGroupPage_SearchForResearchGroupsAsCompany < totalResearchGroupPages_SearchForResearchGroupsAsCompany)
            {
                currentResearchGroupPage_SearchForResearchGroupsAsCompany++;
            }
        }

        private void GoToLastResearchGroupPage()
        {
            currentResearchGroupPage_SearchForResearchGroupsAsCompany = totalResearchGroupPages_SearchForResearchGroupsAsCompany;
        }

        private void OnPageSizeChange_SearchForResearchGroupsAsCompany(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                ResearchGroupsPerPage_SearchForResearchGroupsAsCompany = newSize;
                currentResearchGroupPage_SearchForResearchGroupsAsCompany = 1;
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
            selectedResearchGroupKeywords.Clear();

            researchgroupNameSuggestions.Clear();
            researchGroupAreasSuggestions.Clear();
            researchGroupSkillsSuggestions.Clear();
            researchGroupKeywordsSuggestions.Clear();

            searchResultsAsCompanyToFindResearchGroup = new List<ResearchGroup>();
            hasSearchedForResearchGroups = false;
        }

        private async Task ShowResearchGroupDetailsModal(ResearchGroup researchGroup)
        {
            selectedResearchGroupWhenSearchForResearchGroupsAsCompany = researchGroup;
            showResearchGroupDetailsModalWhenSearchForResearchGroupsAsCompany = true;

            facultyMembers.Clear();
            nonFacultyMembers.Clear();
            spinOffCompanies.Clear();
            facultyMembersCount = 0;
            nonFacultyMembersCount = 0;
            activeResearchActionsCount = 0;
            patentsCount = 0;

            if (!string.IsNullOrEmpty(researchGroup.ResearchGroupEmail))
            {
                await LoadResearchGroupDetailsData(researchGroup.ResearchGroupEmail);
            }

            await InvokeAsync(StateHasChanged);
        }

        private async Task CloseModalResearchGroupDetailsOnEyeIconWhenSearchForResearchGroupsAsCompany()
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

            await InvokeAsync(StateHasChanged);
        }

        private async Task LoadResearchGroupDetailsData(string researchGroupEmail)
        {
            try
            {
                facultyMembers = await dbContext.ResearchGroup_Professors
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
                facultyMembersCount = facultyMembers.Count;

                nonFacultyMembers = await dbContext.ResearchGroup_NonFacultyMembers
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
                nonFacultyMembersCount = nonFacultyMembers.Count;

                spinOffCompanies = await dbContext.ResearchGroup_SpinOffCompany
                    .Where(s => s.ResearchGroupEmail == researchGroupEmail)
                    .Select(s => new SpinOffCompanyInfo
                    {
                        CompanyTitle = s.ResearchGroup_SpinOff_CompanyTitle,
                        CompanyAFM = s.ResearchGroup_SpinOff_CompanyAFM
                    })
                    .ToListAsync();

                activeResearchActionsCount = await dbContext.ResearchGroup_ResearchActions
                    .Where(r => r.ResearchGroupEmail == researchGroupEmail &&
                                r.ResearchGroup_ProjectStatus == "OnGoing")
                    .CountAsync();

                patentsCount = await dbContext.ResearchGroup_Patents
                    .Where(p => p.ResearchGroupEmail == researchGroupEmail)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Πρόβλημα στην φόρτωση λεπτομερειών ομάδας: {ex.Message}");
            }
        }

        // Application management methods
        private async Task LoadJobApplications(long jobId)
        {
            jobApplicationsmadeToCompany = await dbContext.CompanyJobsApplied
                .Where(a => a.RNGForCompanyJobApplied == jobId)
                .ToListAsync();
        }

        private async Task LoadInternshipApplications(long internshipId)
        {
            internshipApplications = await dbContext.InternshipsApplied
                .Where(a => a.RNGForInternshipApplied == internshipId)
                .ToListAsync();
        }

        private async Task LoadThesisApplications(long thesisId)
        {
            companyThesisApplications = await dbContext.CompanyThesesApplied
                .Where(a => a.RNGForCompanyThesisApplied == thesisId)
                .ToListAsync();
        }

        private async Task AcceptJobApplication(long jobRNG, string studentUniqueID)
        {
            var application = await dbContext.CompanyJobsApplied
                .FirstOrDefaultAsync(a => a.RNGForCompanyJobApplied == jobRNG &&
                                        a.StudentUniqueIDAppliedForCompanyJob == studentUniqueID);

            if (application != null)
            {
                application.CompanyPositionStatusAppliedAtTheCompanySide = "Επιτυχής";
                application.CompanyPositionStatusAppliedAtTheStudentSide = "Επιτυχής";
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task RejectJobApplication(long jobRNG, string studentUniqueID)
        {
            var application = await dbContext.CompanyJobsApplied
                .FirstOrDefaultAsync(a => a.RNGForCompanyJobApplied == jobRNG &&
                                        a.StudentUniqueIDAppliedForCompanyJob == studentUniqueID);

            if (application != null)
            {
                application.CompanyPositionStatusAppliedAtTheCompanySide = "Απορρίφθηκε";
                application.CompanyPositionStatusAppliedAtTheStudentSide = "Απορρίφθηκε";
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task AcceptInternshipApplication(long internshipRNG, string studentUniqueID)
        {
            var application = await dbContext.InternshipsApplied
                .FirstOrDefaultAsync(a => a.RNGForInternshipApplied == internshipRNG &&
                                        a.StudentUniqueIDAppliedForInternship == studentUniqueID);

            if (application != null)
            {
                application.InternshipStatusAppliedAtTheCompanySide = "Επιτυχής";
                application.InternshipStatusAppliedAtTheStudentSide = "Επιτυχής";
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task RejectInternshipApplication(long internshipRNG, string studentUniqueID)
        {
            var application = await dbContext.InternshipsApplied
                .FirstOrDefaultAsync(a => a.RNGForInternshipApplied == internshipRNG &&
                                        a.StudentUniqueIDAppliedForInternship == studentUniqueID);

            if (application != null)
            {
                application.InternshipStatusAppliedAtTheCompanySide = "Απορρίφθηκε";
                application.InternshipStatusAppliedAtTheStudentSide = "Απορρίφθηκε";
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task AcceptThesisApplication(long thesisRNG, string studentUniqueID)
        {
            var application = await dbContext.CompanyThesesApplied
                .FirstOrDefaultAsync(a => a.RNGForCompanyThesisApplied == thesisRNG &&
                                        a.StudentUniqueIDAppliedForThesis == studentUniqueID);

            if (application != null)
            {
                application.CompanyThesisStatusAppliedAtCompanySide = "Έχει γίνει Αποδοχή";
                application.CompanyThesisStatusAppliedAtStudentSide = "Επιτυχής";
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task RejectThesisApplication(long thesisRNG, string studentUniqueID)
        {
            var application = await dbContext.CompanyThesesApplied
                .FirstOrDefaultAsync(a => a.RNGForCompanyThesisApplied == thesisRNG &&
                                        a.StudentUniqueIDAppliedForThesis == studentUniqueID);

            if (application != null)
            {
                application.CompanyThesisStatusAppliedAtCompanySide = "Έχει Απορριφθεί";
                application.CompanyThesisStatusAppliedAtStudentSide = "Απορρίφθηκε";
                await dbContext.SaveChangesAsync();
            }
        }

        // Status update methods
        private async Task UpdateJobStatus(int jobId, string newStatus)
        {
            var job = await dbContext.CompanyJobs.FindAsync(jobId);
            if (job != null)
            {
                job.PositionStatus = newStatus;
                await dbContext.SaveChangesAsync();
                await LoadJobs();
            }
        }

        private async Task UpdateInternshipStatus(int internshipId, string newStatus)
        {
            var internship = await dbContext.CompanyInternships.FindAsync(internshipId);
            if (internship != null)
            {
                internship.CompanyUploadedInternshipStatus = newStatus;
                await dbContext.SaveChangesAsync();
                await LoadInternships();
            }
        }

        private async Task UpdateThesisStatus(int thesisId, string newStatus)
        {
            var thesis = await dbContext.CompanyTheses.FindAsync(thesisId);
            if (thesis != null)
            {
                thesis.CompanyThesisStatus = newStatus;
                await dbContext.SaveChangesAsync();
                await LoadTheses();
            }
        }

        private async Task UpdateAnnouncementStatus(int announcementId, string newStatus)
        {
            var announcement = await dbContext.AnnouncementsAsCompany.FindAsync(announcementId);
            if (announcement != null)
            {
                announcement.CompanyAnnouncementStatus = newStatus;
                await dbContext.SaveChangesAsync();
                await LoadAnnouncements();
            }
        }

        // Delete methods
        private async Task DeleteJob(int jobId)
        {
            var job = await dbContext.CompanyJobs.FindAsync(jobId);
            if (job != null)
            {
                dbContext.CompanyJobs.Remove(job);
                await dbContext.SaveChangesAsync();
                await LoadJobs();
            }
        }

        private async Task DeleteInternship(int internshipId)
        {
            var internship = await dbContext.CompanyInternships.FindAsync(internshipId);
            if (internship != null)
            {
                dbContext.CompanyInternships.Remove(internship);
                await dbContext.SaveChangesAsync();
                await LoadInternships();
            }
        }

        private async Task DeleteThesis(int thesisId)
        {
            var thesis = await dbContext.CompanyTheses.FindAsync(thesisId);
            if (thesis != null)
            {
                dbContext.CompanyTheses.Remove(thesis);
                await dbContext.SaveChangesAsync();
                await LoadTheses();
            }
        }

        private async Task DeleteAnnouncement(int announcementId)
        {
            var announcement = await dbContext.AnnouncementsAsCompany.FindAsync(announcementId);
            if (announcement != null)
            {
                dbContext.AnnouncementsAsCompany.Remove(announcement);
                await dbContext.SaveChangesAsync();
                await LoadAnnouncements();
            }
        }

        // File handling methods
        private async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file != null && file.ContentType == "application/pdf")
            {
                using var ms = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(ms);
                job.PositionAttachment = ms.ToArray();
            }
        }

        private async Task HandleFileSelectedForCompanyThesisAttachment(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file != null && file.ContentType == "application/pdf")
            {
                using var ms = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(ms);
                thesis.CompanyThesisAttachmentUpload = ms.ToArray();
            }
        }

        private async Task HandleFileSelectedForAnnouncementAttachment(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file != null && file.ContentType == "application/pdf")
            {
                using var ms = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(ms);
                announcement.CompanyAnnouncementAttachmentFile = ms.ToArray();
            }
        }

        private async Task HandleFileSelectedForCompanyInternshipAttachment(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file != null && file.ContentType == "application/pdf")
            {
                using var ms = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(ms);
                companyInternship.CompanyInternshipAttachment = ms.ToArray();
            }
        }

        // Utility methods
        private bool IsSelectedAreasWhenUploadJobAsCompany(Area area)
        {
            return SelectedAreasWhenUploadJobAsCompany.Contains(area);
        }

        private bool IsSelectedAreasWhenUploadInternshipAsCompany(Area area)
        {
            return SelectedAreasWhenUploadInternshipAsCompany.Contains(area);
        }

        private bool IsSelectedAreasWhenUploadThesisAsCompany(Area area)
        {
            return SelectedAreasWhenUploadThesisAsCompany.Contains(area);
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
                        SelectedAreasWhenUploadJobAsCompany.Add(area);
                }
                else
                {
                    SelectedAreasWhenUploadJobAsCompany.Remove(area);
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
                        SelectedAreasWhenUploadInternshipAsCompany.Add(area);
                }
                else
                {
                    SelectedAreasWhenUploadInternshipAsCompany.Remove(area);
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
                        SelectedAreasWhenUploadThesisAsCompany.Add(area);
                }
                else
                {
                    SelectedAreasWhenUploadThesisAsCompany.Remove(area);
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
                        SelectedSkillsWhenUploadThesisAsCompany.Add(skill);
                }
                else
                {
                    SelectedSkillsWhenUploadThesisAsCompany.Remove(skill);
                }
            }
        }

        // Pagination methods
        private void OnPageSizeChangeForJobs(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                JobsPerPage = newSize;
                currentPageForJobs = 1;
            }
        }

        private void OnPageSizeChangeForInternships(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                InternshipsPerPage = newSize;
                currentPageForInternships = 1;
            }
        }

        private void OnPageSizeChangeForCompanyTheses(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                CompanyThesesPerPage = newSize;
                currentPageForCompanyTheses = 1;
            }
        }

        private void OnPageSizeChangeForAnnouncements(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                pageSize = newSize;
                currentPageForAnnouncements = 1;
            }
        }

        // Navigation methods
        private void NavigateToSearchJobs()
        {
            NavigationManager.NavigateTo("/searchjobs");
        }

        private void NavigateToSearchThesis()
        {
            NavigationManager.NavigateTo("/searchthesis");
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

        // Clear search methods
        private void ClearSearchFieldsForStudents()
        {
            searchNameAsCompanyToFindStudent = "";
            searchSurnameAsCompanyToFindStudent = "";
            searchRegNumberAsCompanyToFindStudent = "";
            searchDepartmentAsCompanyToFindStudent = "";
            searchResultsAsCompanyToFindStudent.Clear();
        }

        private void ClearSearchFieldsForProfessors() => ClearSearchFieldsAsCompanyToFindProfessor();

        private void ClearSearchFieldsForResearchGroups()
        {
            searchResearchGroupNameAsCompanyToFindResearchGroup = "";
            searchResearchGroupSchoolAsCompanyToFindResearchGroup = "";
            searchResearchGroupUniversityDepartmentAsCompanyToFindResearchGroup = "";
            searchResearchGroupAreasAsCompanyToFindResearchGroup = "";
            searchResearchGroupSkillsAsCompanyToFindResearchGroup = "";
            searchResearchGroupKeywordsAsCompanyToFindResearchGroup = "";
            searchResultsAsCompanyToFindResearchGroup.Clear();
        }

        // Helper methods
        private List<string> GetTownsForRegion(string region)
        {
            if (string.IsNullOrEmpty(region) || !RegionToTownsMap.ContainsKey(region))
                return new List<string>();
            return RegionToTownsMap[region];
        }

        private void UpdateTransportOffer(bool offer)
        {
            companyInternship.CompanyInternshipTransportOffer = offer;
        }

        private async Task DownloadAttachmentForCompanyJobs(int jobId)
        {
            var job = await dbContext.CompanyJobs.FindAsync(jobId);
            if (job != null && job.PositionAttachment != null)
            {
                var fileName = $"{job.PositionTitle}_Attachment.pdf";
                var mimeType = "application/pdf";
                await JS.InvokeVoidAsync("BlazorDownloadAttachmentPositionFile", fileName, mimeType, job.PositionAttachment);
            }
        }

        private async Task DownloadAttachmentForCompanyInternships(int internshipId)
        {
            var internship = await dbContext.CompanyInternships.FindAsync(internshipId);
            if (internship != null && internship.CompanyInternshipAttachment != null)
            {
                var fileName = $"{internship.CompanyInternshipTitle}_Attachment.pdf";
                var mimeType = "application/pdf";
                await JS.InvokeVoidAsync("BlazorDownloadAttachmentPositionFile", fileName, mimeType, internship.CompanyInternshipAttachment);
            }
        }

        private async Task DownloadAttachmentForCompanyTheses(int thesisId)
        {
            var thesis = await dbContext.CompanyTheses.FindAsync(thesisId);
            if (thesis != null && thesis.CompanyThesisAttachmentUpload != null)
            {
                var fileName = $"{thesis.CompanyThesisTitle}_Attachment.pdf";
                var mimeType = "application/pdf";
                await JS.InvokeVoidAsync("BlazorDownloadAttachmentPositionFile", fileName, mimeType, thesis.CompanyThesisAttachmentUpload);
            }
        }

        private class FacultyMemberInfo
        {
            public string FullName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
        }

        private class NonFacultyMemberInfo
        {
            public string FullName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
        }

        private class SpinOffCompanyInfo
        {
            public string CompanyTitle { get; set; } = string.Empty;
            public string CompanyAFM { get; set; } = string.Empty;
        }

        // Region and town mapping
        private Dictionary<string, List<string>> RegionToTownsMap = new Dictionary<string, List<string>>
        {
            {"Ανατολική Μακεδονία και Θράκη", new List<string> {"Κομοτηνή", "Αλεξανδρούπολη", "Καβάλα", "Ξάνθη", "Δράμα"}},
            {"Κεντρική Μακεδονία", new List<string> {"Θεσσαλονίκη", "Κατερίνη", "Σέρρες", "Κιλκίς", "Πολύγυρος"}},
            {"Δυτική Μακεδονία", new List<string> {"Κοζάνη", "Φλώρινα", "Καστοριά", "Γρεβενά"}},
            {"Ήπειρος", new List<string> {"Ιωάννινα", "Άρτα", "Πρέβεζα", "Ηγουμενίτσα"}},
            {"Θεσσαλία", new List<string> {"Λάρισα", "Βόλος", "Τρίκαλα", "Καρδίτσα"}},
            {"Ιόνια Νησιά", new List<string> {"Κέρκυρα", "Λευκάδα", "Κεφαλονιά", "Ζάκυνθος"}},
            {"Δυτική Ελλάδα", new List<string> {"Πάτρα", "Μεσολόγγι", "Αμφιλοχία", "Πύργος"}},
            {"Κεντρική Ελλάδα", new List<string> {"Λαμία", "Χαλκίδα", "Λιβαδειά", "Θήβα"}},
            {"Αττική", new List<string> {"Αθήνα", "Πειραιάς", "Κηφισιά", "Παλλήνη"}},
            {"Πελοπόννησος", new List<string> {"Τρίπολη", "Καλαμάτα", "Κορίνθος", "Άργος"}},
            {"Βόρειο Αιγαίο", new List<string> {"Μυτιλήνη", "Χίος", "Λήμνος", "Σάμος"}},
            {"Νότιο Αιγαίο", new List<string> {"Ρόδος", "Κως", "Κρήτη", "Κάρπαθος"}},
            {"Κρήτη", new List<string> {"Ηράκλειο", "Χανιά", "Ρέθυμνο", "Αγία Νικόλαος"}}
        };

        // Company types
        private List<string> ForeasType = new List<string>
        {
            "Ιδιωτικός Φορέας",
            "Δημόσιος Φορέας",
            "Μ.Κ.Ο.",
            "Άλλο"
        };

        // Regions
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
    }

    public readonly record struct ThesisStatusChangeRequest(int CompanyThesisId, string Status);

    public readonly record struct CompanyThesisApplicationDecision(long CompanyThesisId, string StudentUniqueId);
}
