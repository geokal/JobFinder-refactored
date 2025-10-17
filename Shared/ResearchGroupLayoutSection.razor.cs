using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
    public partial class ResearchGroupLayoutSection
    {
        [Inject] private Data.AppDbContext dbContext { get; set; }
        [Inject] private Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IJSRuntime JS { get; set; }

        // Authentication and registration state
        private bool isInitializedAsResearchGroupUser = false;
        private bool isResearchGroupRegistered = false;
        private string CurrentUserEmail = string.Empty;

        // Research Group-specific properties
        private ResearchGroup researchGroupData;
        private List<ResearchGroup_Professors> facultyMembers = new();
        private List<ResearchGroup_NonFacultyMembers> nonFacultyMembers = new();
        private List<ResearchGroup_ResearchActions> researchActions = new();
        private List<ResearchGroup_Patents> patents = new();
        private List<ResearchGroup_SpinOffCompany> spinOffCompanies = new();

        // UI state properties
        private bool isAnnouncementsFormVisibleToShowGeneralAnnouncementsAndEventsAsRG = false;
        private bool isAnnouncementsAsRGVisible = false;
        private bool isRGSearchCompanyFormVisible = false;
        private bool isRGSearchProfessorVisible = false;
        private bool isStatisticsVisible = false;
        private bool isUniversityNewsVisible = false;
        private bool isSvseNewsVisible = false;
        private bool isCompanyAnnouncementsVisible = false;
        private bool isProfessorAnnouncementsVisible = false;
        private bool isCompanyEventsVisible = false;
        private bool isProfessorEventsVisible = false;
        private bool showCompanyDetailsModal = false;
        private bool showProfessorDetailsModalWhenSearchForProfessorsAsRG = false;

        // Announcement and news data
        private List<NewsArticle> newsArticles = new();
        private List<NewsArticle> svseNewsArticles = new();
        private List<AnnouncementAsCompany> Announcements = new();
        private List<AnnouncementAsProfessor> ProfessorAnnouncements = new();
        private int pageSize = 3;
        private int currentPageForCompanyAnnouncements = 1;
        private int currentPageForProfessorAnnouncements = 1;
        private int expandedAnnouncementId = -1;
        private int expandedProfessorAnnouncementId = -1;
        private int expandedCompanyEventId = -1;
        private int expandedProfessorEventId = -1;
        private int currentCompanyEventPage = 1;
        private int currentProfessorEventPage = 1;
        private int currentCompanyEventpageSize = 3;
        private int currentProfessorEventpageSize = 3;

        // Event calendar state
        private DateTime currentMonth = DateTime.Today;
        private readonly string[] daysOfWeek = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        private int selectedDay = 0;
        private int highlightedDay = 0;
        private DateTime? selectedDate;
        private bool isModalVisibleToShowEventsOnCalendarForEachClickedDay = false;
        private object selectedEvent;
        private string selectedEventFilter
        {
            get => _selectedEventFilter;
            set
            {
                if (_selectedEventFilter != value)
                {
                    _selectedEventFilter = value;
                    UpdateFilteredEvents();
                }
            }
        }
        private string _selectedEventFilter = "All";
        private List<CompanyEvent> eventsForCurrentMonth = new();
        private List<ProfessorEvent> professorEventsForCurrentMonth = new();
        private readonly Dictionary<int, List<CompanyEvent>> eventsForDate = new();
        private readonly Dictionary<int, List<ProfessorEvent>> eventsForDateForProfessors = new();
        private List<CompanyEvent> selectedDateCompanyEvents = new();
        private List<ProfessorEvent> selectedDateProfessorEvents = new();
        private List<CompanyEvent> filteredCompanyEvents = new();
        private List<ProfessorEvent> filteredProfessorEvents = new();

        // Company search helpers
        private int currentPage_CompanySearchAsRG = 1;
        private int totalPages_CompanySearchAsRG = 1;
        private int CompanySearchPerPageAsRG = 5;
        private int[] companySearchPageSizeOptions = new[] { 5, 10, 15, 20 };
        private List<string> companyNameSuggestionsAsRG = new();
        private List<string> areasOfInterestSuggestions = new();
        private List<string> selectedAreasOfInterest = new();
        private List<string> companyTypesAsRG = new();
        private List<Company> allPublishedCompanies = new();
        private Company selectedCompany;
        private Dictionary<string, List<string>> RegionToTownsMap = new();
        private List<string> Regions => RegionToTownsMap.Keys.OrderBy(r => r).ToList();

        // Professor search helpers
        private Professor selectedProfessorWhenSearchForProfessorsAsRG;
        private int[] pageSizeOptions_SearchForProfessorsAsRG = new[] { 5, 10, 15, 20 };
        private List<string> professorNameSurnameSuggestionsAsRG = new();
        private List<string> areasOfInterestSuggestionsAsRG = new();
        private List<string> selectedAreasOfInterestAsRG = new();
        private Dictionary<string, List<string>> universityDepartments = new();

        // Search and filter properties
        private string searchCompanyEmailAsRGToFindCompany = "";
        private string searchCompanyNameENGAsRGToFindCompany = "";
        private string searchCompanyTypeAsRGToFindCompany = "";
        private string searchCompanyActivityrAsRGToFindCompany = "";
        private string searchCompanyTownAsRGToFindCompany = "";
        private string searchCompanyAreasAsRGToFindCompany = "";
        private string searchCompanyDesiredSkillsAsRGToFindCompany = "";
        private string searchNameSurnameAsRGToFindProfessor = "";
        private string searchDepartmentAsRGToFindProfessor = "";
        private string searchAreasOfInterestAsRGToFindProfessor = "";
        private string searchSchoolAsRGToFindProfessor = "";

        // Data collections
        private List<Company> searchResultsAsRGToFindCompany = new();
        private List<Professor> searchResultsAsRGToFindProfessor = new();
        private List<ResearchGroup_Publications> memberPublications = new();

        // Statistics
        private int? numberOfFacultyMembers;
        private int? numberOfCollaborators;
        private int? numberOfTotalPublications;
        private int? numberOfRecentPublications;
        private int? numberOfActiveResearchActions;
        private int? numberOfInactiveResearchActions;
        private int? numberOfActivePatents;
        private int? numberOfInactivePatents;

        // UI visibility flags
        private bool showFacultyMembersModal = false;
        private bool showNonFacultyMembersModal = false;
        private bool showResearchActionsModal = false;
        private bool showPatentsModal = false;

        // Pagination
        private int currentCompanyPage_SearchForCompaniesAsRG = 1;
        private int currentProfessorPage_SearchForProfessorsAsRG = 1;
        private int CompaniesPerPage_SearchForCompaniesAsRG = 3;
        private int ProfessorsPerPage_SearchForProfessorsAsRG = 3;

        // Component initialization
        protected override async Task OnInitializedAsync()
        {
            await InitializeResearchGroupUserAsync();

            if (!isResearchGroupRegistered)
            {
                isInitializedAsResearchGroupUser = true;
                return;
            }

            await LoadResearchGroupData();
            await LoadFacultyMembers();
            await LoadNonFacultyMembers();
            await LoadResearchActions();
            await LoadPatents();
            await LoadSpinOffCompanies();
            await LoadMemberPublications();
            await LoadNewsAndAnnouncements();
            await LoadEventsForCalendar();
            await LoadCompanyLookupDataAsync();
            await LoadProfessorLookupDataAsync();
            await CalculateStatistics();

            UpdateCompanySearchPagination();
            UpdateProfessorSearchPagination();

            isInitializedAsResearchGroupUser = true;
        }

        private async Task InitializeResearchGroupUserAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                CurrentUserEmail = user.FindFirst("name")?.Value ?? string.Empty;
                if (!string.IsNullOrEmpty(CurrentUserEmail))
                {
                    isResearchGroupRegistered = await dbContext.ResearchGroups
                        .AnyAsync(r => r.ResearchGroupEmail == CurrentUserEmail);
                }
            }
        }

        // Data loading methods
        private async Task LoadResearchGroupData()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var userEmail = user.FindFirst("name")?.Value;
                if (!string.IsNullOrEmpty(userEmail))
                {
                    researchGroupData = await dbContext.ResearchGroups.FirstOrDefaultAsync(r => r.ResearchGroupEmail == userEmail);
                }
            }
        }

        private async Task LoadFacultyMembers()
        {
            if (researchGroupData != null)
            {
                facultyMembers = await dbContext.ResearchGroup_Professors
                    .Where(rp => rp.PK_ResearchGroupEmail == researchGroupData.ResearchGroupEmail)
                    .ToListAsync();
            }
        }

        private async Task LoadNonFacultyMembers()
        {
            if (researchGroupData != null)
            {
                nonFacultyMembers = await dbContext.ResearchGroup_NonFacultyMembers
                    .Where(rnf => rnf.PK_ResearchGroupEmail == researchGroupData.ResearchGroupEmail)
                    .ToListAsync();
            }
        }

        private async Task LoadResearchActions()
        {
            if (researchGroupData != null)
            {
                researchActions = await dbContext.ResearchGroup_ResearchActions
                    .Where(ra => ra.ResearchGroupEmail == researchGroupData.ResearchGroupEmail)
                    .ToListAsync();
            }
        }

        private async Task LoadPatents()
        {
            if (researchGroupData != null)
            {
                patents = await dbContext.ResearchGroup_Patents
                    .Where(p => p.ResearchGroupEmail == researchGroupData.ResearchGroupEmail)
                    .ToListAsync();
            }
        }

        private async Task LoadSpinOffCompanies()
        {
            if (researchGroupData != null)
            {
                spinOffCompanies = await dbContext.ResearchGroup_SpinOffCompany
                    .Where(s => s.ResearchGroupEmail == researchGroupData.ResearchGroupEmail)
                    .ToListAsync();
            }
        }

        private async Task LoadMemberPublications()
        {
            if (researchGroupData != null)
            {
                memberPublications = await dbContext.ResearchGroup_Publications
                    .Where(p => p.PK_ResearchGroupEmail == researchGroupData.ResearchGroupEmail)
                    .OrderByDescending(p => p.PK_ResearchGroupMemberPublication_Year)
                    .ToListAsync();
            }
        }

        private async Task LoadNewsAndAnnouncements()
        {
            newsArticles = await FetchNewsArticlesAsync();
            svseNewsArticles = await FetchSVSENewsArticlesAsync();
            Announcements = await FetchAnnouncementsAsync();
            ProfessorAnnouncements = await FetchProfessorAnnouncementsAsync();
        }

        private async Task<List<NewsArticle>> FetchNewsArticlesAsync()
        {
            try
            {
                return await HttpClient.GetFromJsonAsync<List<NewsArticle>>("api/news/university") ?? new List<NewsArticle>();
            }
            catch
            {
                return new List<NewsArticle>();
            }
        }

        private async Task<List<NewsArticle>> FetchSVSENewsArticlesAsync()
        {
            try
            {
                return await HttpClient.GetFromJsonAsync<List<NewsArticle>>("api/news/svse") ?? new List<NewsArticle>();
            }
            catch
            {
                return new List<NewsArticle>();
            }
        }

        private async Task<List<AnnouncementAsCompany>> FetchAnnouncementsAsync()
        {
            return await dbContext.AnnouncementsAsCompany
                .Include(a => a.Company)
                .Where(a => a.CompanyAnnouncementStatus == "Δημοσιευμένη")
                .OrderByDescending(a => a.CompanyAnnouncementUploadDate)
                .ToListAsync();
        }

        private async Task<List<AnnouncementAsProfessor>> FetchProfessorAnnouncementsAsync()
        {
            return await dbContext.AnnouncementsAsProfessor
                .Include(a => a.Professor)
                .Where(a => a.ProfessorAnnouncementStatus == "Δημοσιευμένη")
                .OrderByDescending(a => a.ProfessorAnnouncementUploadDate)
                .ToListAsync();
        }

        private async Task LoadEventsForCalendar()
        {
            eventsForCurrentMonth = await dbContext.CompanyEvents
                .Include(e => e.Company)
                .Where(e => e.CompanyEventStatus == "Δημοσιευμένη" &&
                            e.CompanyEventActiveDate.Year == currentMonth.Year &&
                            e.CompanyEventActiveDate.Month == currentMonth.Month)
                .ToListAsync();

            professorEventsForCurrentMonth = await dbContext.ProfessorEvents
                .Include(e => e.Professor)
                .Where(e => e.ProfessorEventStatus == "Δημοσιευμένη" &&
                            e.ProfessorEventActiveDate.Year == currentMonth.Year &&
                            e.ProfessorEventActiveDate.Month == currentMonth.Month)
                .ToListAsync();

            eventsForDate.Clear();
            foreach (var companyEvent in eventsForCurrentMonth)
            {
                int day = companyEvent.CompanyEventActiveDate.Day;
                if (!eventsForDate.ContainsKey(day))
                {
                    eventsForDate[day] = new List<CompanyEvent>();
                }
                eventsForDate[day].Add(companyEvent);
            }

            eventsForDateForProfessors.Clear();
            foreach (var professorEvent in professorEventsForCurrentMonth)
            {
                int day = professorEvent.ProfessorEventActiveDate.Day;
                if (!eventsForDateForProfessors.ContainsKey(day))
                {
                    eventsForDateForProfessors[day] = new List<ProfessorEvent>();
                }
                eventsForDateForProfessors[day].Add(professorEvent);
            }

            UpdateFilteredEvents();
        }

        private async Task LoadCompanyLookupDataAsync()
        {
            companyTypesAsRG = await dbContext.Companies
                .Where(c => !string.IsNullOrEmpty(c.CompanyType))
                .Select(c => c.CompanyType)
                .Distinct()
                .OrderBy(type => type)
                .ToListAsync();

            areasOfInterestSuggestions = await dbContext.Companies
                .Where(c => !string.IsNullOrEmpty(c.CompanyAreas))
                .Select(c => c.CompanyAreas)
                .Distinct()
                .OrderBy(area => area)
                .Take(50)
                .ToListAsync();

            allPublishedCompanies = await dbContext.Companies
                .Where(c => c.CompanyAcceptRules)
                .OrderBy(c => c.CompanyName)
                .ToListAsync();

            RegionToTownsMap = await dbContext.Companies
                .Where(c => !string.IsNullOrEmpty(c.CompanyRegions) && !string.IsNullOrEmpty(c.CompanyTown))
                .GroupBy(c => c.CompanyRegions)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => g.Select(c => c.CompanyTown).Distinct().OrderBy(town => town).ToList());
        }

        private async Task LoadProfessorLookupDataAsync()
        {
            areasOfInterestSuggestionsAsRG = await dbContext.Areas
                .OrderBy(a => a.AreaName)
                .Select(a => a.AreaName)
                .Take(100)
                .ToListAsync();

            professorNameSurnameSuggestionsAsRG = await dbContext.Professors
                .Where(p => !string.IsNullOrEmpty(p.ProfName) && !string.IsNullOrEmpty(p.ProfSurname))
                .Select(p => p.ProfName + " " + p.ProfSurname)
                .OrderBy(name => name)
                .Take(100)
                .ToListAsync();

            universityDepartments = await dbContext.Professors
                .Where(p => !string.IsNullOrEmpty(p.ProfUniversity) && !string.IsNullOrEmpty(p.ProfDepartment))
                .GroupBy(p => p.ProfUniversity)
                .ToDictionaryAsync(g => g.Key, g => g.Select(p => p.ProfDepartment).Distinct().OrderBy(d => d).ToList());
        }

        private Task CalculateStatistics()
        {
            if (researchGroupData != null)
            {
                numberOfFacultyMembers = facultyMembers.Count;
                numberOfCollaborators = nonFacultyMembers.Count;

                // Calculate publication statistics
                numberOfTotalPublications = memberPublications.Count;
                numberOfRecentPublications = memberPublications
                    .Where(p => !string.IsNullOrEmpty(p.PK_ResearchGroupMemberPublication_Year) &&
                               int.TryParse(p.PK_ResearchGroupMemberPublication_Year, out int year) &&
                               year >= DateTime.Now.AddYears(-5).Year)
                    .Count();

                // Calculate research actions
                numberOfActiveResearchActions = researchActions.Count(ra => ra.ResearchGroup_ProjectStatus == "OnGoing");
                numberOfInactiveResearchActions = researchActions.Count(ra => ra.ResearchGroup_ProjectStatus == "Past");

                // Calculate patents
                numberOfActivePatents = patents.Count(p => p.ResearchGroup_Patent_PatentStatus == "Ενεργή");
                numberOfInactivePatents = patents.Count(p => p.ResearchGroup_Patent_PatentStatus == "Ανενεργή");
            }

            return Task.CompletedTask;
        }

        // UI toggle methods
        private void ToggleFormVisibilityToShowGeneralAnnouncementsAndEventsAsRG()
        {
            isAnnouncementsFormVisibleToShowGeneralAnnouncementsAndEventsAsRG = !isAnnouncementsFormVisibleToShowGeneralAnnouncementsAndEventsAsRG;
        }

        private void ToggleAnnouncementsAsRGVisibility()
        {
            isAnnouncementsAsRGVisible = !isAnnouncementsAsRGVisible;
        }

        private void ToggleRGSearchCompanyFormVisible()
        {
            isRGSearchCompanyFormVisible = !isRGSearchCompanyFormVisible;
        }

        private void ToggleRGSearchProfessorVisible()
        {
            isRGSearchProfessorVisible = !isRGSearchProfessorVisible;
        }

        private async Task ToggleStatisticsVisibility()
        {
            isStatisticsVisible = !isStatisticsVisible;
            if (isStatisticsVisible)
            {
                await CalculateStatistics();
            }
        }

        private void ToggleFormVisibilityForSearchCompanyAsRG() => ToggleRGSearchCompanyFormVisible();

        private void ToggleUniversityNewsVisibility() => isUniversityNewsVisible = !isUniversityNewsVisible;

        private void ToggleSvseNewsVisibility() => isSvseNewsVisible = !isSvseNewsVisible;

        private void ToggleCompanyAnnouncementsVisibility() => isCompanyAnnouncementsVisible = !isCompanyAnnouncementsVisible;

        private void ToggleProfessorAnnouncementsVisibility() => isProfessorAnnouncementsVisible = !isProfessorAnnouncementsVisible;

        private void ToggleCompanyEventsVisibility() => isCompanyEventsVisible = !isCompanyEventsVisible;

        private void ToggleProfessorEventsVisibility() => isProfessorEventsVisible = !isProfessorEventsVisible;

        // Modal methods
        private void ShowFacultyMembersDetails()
        {
            showFacultyMembersModal = true;
        }

        private void ShowNonFacultyMembersDetails()
        {
            showNonFacultyMembersModal = true;
        }

        private void ShowResearchActionsDetails()
        {
            showResearchActionsModal = true;
        }

        private void ShowPatentsDetails()
        {
            showPatentsModal = true;
        }

        private void CloseFacultyMembersModal()
        {
            showFacultyMembersModal = false;
        }

        private void CloseNonFacultyMembersModal()
        {
            showNonFacultyMembersModal = false;
        }

        private void CloseResearchActionsModal()
        {
            showResearchActionsModal = false;
        }

        private void ClosePatentsModal()
        {
            showPatentsModal = false;
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

        private void ShowProfessorDetailsOnEyeIconWhenSearchForProfessorAsRG(Professor professor)
        {
            selectedProfessorWhenSearchForProfessorsAsRG = professor;
            showProfessorDetailsModalWhenSearchForProfessorsAsRG = true;
        }

        private void CloseModalProfessorDetailsOnEyeIconWhenSearchForProfessorsAsRG()
        {
            showProfessorDetailsModalWhenSearchForProfessorsAsRG = false;
            selectedProfessorWhenSearchForProfessorsAsRG = null;
        }

        // Search methods
        private async Task SearchCompaniesAsRG()
        {
            IQueryable<Company> query = dbContext.Companies
                .Where(c => c.CompanyAcceptRules);

            if (!string.IsNullOrWhiteSpace(searchCompanyEmailAsRGToFindCompany))
            {
                var normalized = searchCompanyEmailAsRGToFindCompany.Trim();
                query = query.Where(c => c.CompanyEmail.Contains(normalized));
            }

            if (!string.IsNullOrWhiteSpace(searchCompanyNameENGAsRGToFindCompany))
            {
                var normalized = searchCompanyNameENGAsRGToFindCompany.Trim();
                query = query.Where(c => c.CompanyName.Contains(normalized));
            }

            if (!string.IsNullOrWhiteSpace(searchCompanyTypeAsRGToFindCompany))
            {
                query = query.Where(c => c.CompanyType == searchCompanyTypeAsRGToFindCompany);
            }

            if (!string.IsNullOrWhiteSpace(searchCompanyActivityrAsRGToFindCompany))
            {
                query = query.Where(c => c.CompanyActivity.Contains(searchCompanyActivityrAsRGToFindCompany));
            }

            if (!string.IsNullOrWhiteSpace(searchCompanyTownAsRGToFindCompany))
            {
                query = query.Where(c => c.CompanyTown.Contains(searchCompanyTownAsRGToFindCompany));
            }

            if (!string.IsNullOrWhiteSpace(searchCompanyAreasAsRGToFindCompany))
            {
                query = query.Where(c => c.CompanyAreas.Contains(searchCompanyAreasAsRGToFindCompany));
            }

            if (selectedAreasOfInterest.Any())
            {
                foreach (var area in selectedAreasOfInterest)
                {
                    query = query.Where(c => c.CompanyAreas.Contains(area));
                }
            }

            if (!string.IsNullOrWhiteSpace(searchCompanyDesiredSkillsAsRGToFindCompany))
            {
                query = query.Where(c => c.CompanyDesiredSkills.Contains(searchCompanyDesiredSkillsAsRGToFindCompany));
            }

            searchResultsAsRGToFindCompany = await query
                .OrderBy(c => c.CompanyName)
                .Take(500)
                .ToListAsync();

            currentPage_CompanySearchAsRG = 1;
            UpdateCompanySearchPagination();
            showCompanyDetailsModal = false;
            selectedCompany = null;
        }

        private async Task SearchProfessorsAsRGToFindProfessor()
        {
            IQueryable<Professor> query = dbContext.Professors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchNameSurnameAsRGToFindProfessor))
            {
                var normalized = searchNameSurnameAsRGToFindProfessor.Trim();
                query = query.Where(p => (p.ProfName + " " + p.ProfSurname).Contains(normalized));
            }

            if (!string.IsNullOrWhiteSpace(searchSchoolAsRGToFindProfessor))
            {
                query = query.Where(p => p.ProfUniversity == searchSchoolAsRGToFindProfessor);
            }

            if (!string.IsNullOrWhiteSpace(searchDepartmentAsRGToFindProfessor))
            {
                query = query.Where(p => p.ProfDepartment == searchDepartmentAsRGToFindProfessor);
            }

            if (!string.IsNullOrWhiteSpace(searchAreasOfInterestAsRGToFindProfessor))
            {
                query = query.Where(p => p.ProfGeneralFieldOfWork != null &&
                                         p.ProfGeneralFieldOfWork.Contains(searchAreasOfInterestAsRGToFindProfessor));
            }

            if (selectedAreasOfInterestAsRG.Any())
            {
                foreach (var area in selectedAreasOfInterestAsRG)
                {
                    query = query.Where(p => p.ProfGeneralFieldOfWork != null &&
                                             p.ProfGeneralFieldOfWork.Contains(area));
                }
            }

            searchResultsAsRGToFindProfessor = await query
                .OrderBy(p => p.ProfSurname)
                .ThenBy(p => p.ProfName)
                .Take(500)
                .ToListAsync();

            currentProfessorPage_SearchForProfessorsAsRG = 1;
            UpdateProfessorSearchPagination();
            showProfessorDetailsModalWhenSearchForProfessorsAsRG = false;
            selectedProfessorWhenSearchForProfessorsAsRG = null;
        }

        // Pagination methods
        private void OnPageSizeChange_SearchForCompaniesAsRG(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                CompaniesPerPage_SearchForCompaniesAsRG = newSize;
                currentCompanyPage_SearchForCompaniesAsRG = 1;
            }
        }

        private void OnPageSizeChange_SearchForProfessorsAsRG(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                ProfessorsPerPage_SearchForProfessorsAsRG = newSize;
                currentProfessorPage_SearchForProfessorsAsRG = 1;
            }
        }

        private void GoToFirstCompanyPageAsRG()
        {
            currentCompanyPage_SearchForCompaniesAsRG = 1;
        }

        private void GoToLastCompanyPageAsRG()
        {
            currentCompanyPage_SearchForCompaniesAsRG = (int)Math.Ceiling((double)searchResultsAsRGToFindCompany.Count / CompaniesPerPage_SearchForCompaniesAsRG);
        }

        private void PreviousCompanyPageAsRG()
        {
            if (currentCompanyPage_SearchForCompaniesAsRG > 1)
                currentCompanyPage_SearchForCompaniesAsRG--;
        }

        private void NextCompanyPageAsRG()
        {
            int totalPages = (int)Math.Ceiling((double)searchResultsAsRGToFindCompany.Count / CompaniesPerPage_SearchForCompaniesAsRG);
            if (currentCompanyPage_SearchForCompaniesAsRG < totalPages)
                currentCompanyPage_SearchForCompaniesAsRG++;
        }

        private void GoToCompanyPageAsRG(int pageNumber)
        {
            int totalPages = (int)Math.Ceiling((double)searchResultsAsRGToFindCompany.Count / CompaniesPerPage_SearchForCompaniesAsRG);
            if (pageNumber >= 1 && pageNumber <= totalPages)
                currentCompanyPage_SearchForCompaniesAsRG = pageNumber;
        }

        private void GoToFirstProfessorPageAsRG()
        {
            currentProfessorPage_SearchForProfessorsAsRG = 1;
        }

        private void GoToLastProfessorPageAsRG()
        {
            currentProfessorPage_SearchForProfessorsAsRG = (int)Math.Ceiling((double)searchResultsAsRGToFindProfessor.Count / ProfessorsPerPage_SearchForProfessorsAsRG);
        }

        private void PreviousProfessorPageAsRG()
        {
            if (currentProfessorPage_SearchForProfessorsAsRG > 1)
                currentProfessorPage_SearchForProfessorsAsRG--;
        }

        private void NextProfessorPageAsRG()
        {
            int totalPages = (int)Math.Ceiling((double)searchResultsAsRGToFindProfessor.Count / ProfessorsPerPage_SearchForProfessorsAsRG);
            if (currentProfessorPage_SearchForProfessorsAsRG < totalPages)
                currentProfessorPage_SearchForProfessorsAsRG++;
        }

        private void GoToProfessorPageAsRG(int pageNumber)
        {
            int totalPages = (int)Math.Ceiling((double)searchResultsAsRGToFindProfessor.Count / ProfessorsPerPage_SearchForProfessorsAsRG);
            if (pageNumber >= 1 && pageNumber <= totalPages)
                currentProfessorPage_SearchForProfessorsAsRG = pageNumber;
        }

        private void OnPageSizeChangeForCompanySearchAsRG(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                CompanySearchPerPageAsRG = newSize;
                currentPage_CompanySearchAsRG = 1;
                UpdateCompanySearchPagination();
            }
        }

        private IEnumerable<Company> GetPaginatedCompanySearchResultsAsRG()
        {
            return searchResultsAsRGToFindCompany
                .Skip((currentPage_CompanySearchAsRG - 1) * CompanySearchPerPageAsRG)
                .Take(CompanySearchPerPageAsRG);
        }

        private void GoToFirstPage_CompanySearchAsRG()
        {
            currentPage_CompanySearchAsRG = 1;
        }

        private void PreviousPage_CompanySearchAsRG()
        {
            if (currentPage_CompanySearchAsRG > 1)
            {
                currentPage_CompanySearchAsRG--;
            }
        }

        private void GoToPage_CompanySearchAsRG(int pageNumber)
        {
            if (pageNumber >= 1 && pageNumber <= totalPages_CompanySearchAsRG)
            {
                currentPage_CompanySearchAsRG = pageNumber;
            }
        }

        private void NextPage_CompanySearchAsRG()
        {
            if (currentPage_CompanySearchAsRG < totalPages_CompanySearchAsRG)
            {
                currentPage_CompanySearchAsRG++;
            }
        }

        private void GoToLastPage_CompanySearchAsRG()
        {
            currentPage_CompanySearchAsRG = totalPages_CompanySearchAsRG;
        }

        private List<int> GetVisiblePages_CompanySearchAsRG()
        {
            var pages = new List<int>();
            pages.Add(1);

            if (totalPages_CompanySearchAsRG <= 1)
            {
                return pages;
            }

            if (currentPage_CompanySearchAsRG > 3)
            {
                pages.Add(-1);
            }

            int start = Math.Max(2, currentPage_CompanySearchAsRG - 1);
            int end = Math.Min(totalPages_CompanySearchAsRG - 1, currentPage_CompanySearchAsRG + 1);

            for (int i = start; i <= end; i++)
            {
                pages.Add(i);
            }

            if (currentPage_CompanySearchAsRG < totalPages_CompanySearchAsRG - 2)
            {
                pages.Add(-1);
            }

            pages.Add(totalPages_CompanySearchAsRG);
            return pages;
        }

        private List<int> GetVisiblePagesForProfessorSearchAsRG()
        {
            var pages = new List<int>();
            int totalPages = (int)Math.Ceiling((double)searchResultsAsRGToFindProfessor.Count / ProfessorsPerPage_SearchForProfessorsAsRG);

            pages.Add(1);

            if (totalPages <= 1)
            {
                return pages;
            }

            if (currentProfessorPage_SearchForProfessorsAsRG > 3)
            {
                pages.Add(-1);
            }

            int start = Math.Max(2, currentProfessorPage_SearchForProfessorsAsRG - 1);
            int end = Math.Min(totalPages - 1, currentProfessorPage_SearchForProfessorsAsRG + 1);

            for (int i = start; i <= end; i++)
            {
                pages.Add(i);
            }

            if (currentProfessorPage_SearchForProfessorsAsRG < totalPages - 2)
            {
                pages.Add(-1);
            }

            pages.Add(totalPages);
            return pages;
        }

        // Helper methods
        private IEnumerable<Company> GetPaginatedCompanyResultsAsRG()
        {
            return searchResultsAsRGToFindCompany
                .Skip((currentCompanyPage_SearchForCompaniesAsRG - 1) * CompaniesPerPage_SearchForCompaniesAsRG)
                .Take(CompaniesPerPage_SearchForCompaniesAsRG);
        }

        private IEnumerable<Professor> GetPaginatedProfessorResultsAsRG()
        {
            return searchResultsAsRGToFindProfessor
                .Skip((currentProfessorPage_SearchForProfessorsAsRG - 1) * ProfessorsPerPage_SearchForProfessorsAsRG)
                .Take(ProfessorsPerPage_SearchForProfessorsAsRG);
        }

        private List<int> GetVisibleCompanyPagesAsRG()
        {
            var pages = new List<int>();
            int current = currentCompanyPage_SearchForCompaniesAsRG;
            int total = (int)Math.Ceiling((double)searchResultsAsRGToFindCompany.Count / CompaniesPerPage_SearchForCompaniesAsRG);

            pages.Add(1);
            if (current > 3) pages.Add(-1);
            int start = Math.Max(2, current - 1);
            int end = Math.Min(total - 1, current + 1);
            for (int i = start; i <= end; i++) pages.Add(i);
            if (current < total - 2) pages.Add(-1);
            if (total > 1) pages.Add(total);

            return pages;
        }

        private List<int> GetVisibleProfessorPagesAsRG()
        {
            var pages = new List<int>();
            int current = currentProfessorPage_SearchForProfessorsAsRG;
            int total = (int)Math.Ceiling((double)searchResultsAsRGToFindProfessor.Count / ProfessorsPerPage_SearchForProfessorsAsRG);

            pages.Add(1);
            if (current > 3) pages.Add(-1);
            int start = Math.Max(2, current - 1);
            int end = Math.Min(total - 1, current + 1);
            for (int i = start; i <= end; i++) pages.Add(i);
            if (current < total - 2) pages.Add(-1);
            if (total > 1) pages.Add(total);

            return pages;
        }

        private List<int> GetVisiblePagesForCompanyAnnouncements()
        {
            var pages = new List<int>();
            int totalPages = Math.Max(1, (int)Math.Ceiling((double)Announcements.Count(a => a.CompanyAnnouncementStatus == "Δημοσιευμένη") / pageSize));

            pages.Add(1);
            if (totalPages <= 1)
            {
                return pages;
            }

            if (currentPageForCompanyAnnouncements > 3)
            {
                pages.Add(-1);
            }

            int start = Math.Max(2, currentPageForCompanyAnnouncements - 1);
            int end = Math.Min(totalPages - 1, currentPageForCompanyAnnouncements + 1);

            for (int i = start; i <= end; i++)
            {
                pages.Add(i);
            }

            if (currentPageForCompanyAnnouncements < totalPages - 2)
            {
                pages.Add(-1);
            }

            if (!pages.Contains(totalPages))
            {
                pages.Add(totalPages);
            }
            return pages;
        }

        private List<int> GetVisiblePagesForProfessorAnnouncements()
        {
            var pages = new List<int>();
            int totalPages = Math.Max(1, (int)Math.Ceiling((double)ProfessorAnnouncements.Count(a => a.ProfessorAnnouncementStatus == "Δημοσιευμένη") / pageSize));

            pages.Add(1);
            if (totalPages <= 1)
            {
                return pages;
            }

            if (currentPageForProfessorAnnouncements > 3)
            {
                pages.Add(-1);
            }

            int start = Math.Max(2, currentPageForProfessorAnnouncements - 1);
            int end = Math.Min(totalPages - 1, currentPageForProfessorAnnouncements + 1);

            for (int i = start; i <= end; i++)
            {
                pages.Add(i);
            }

            if (currentPageForProfessorAnnouncements < totalPages - 2)
            {
                pages.Add(-1);
            }

            if (!pages.Contains(totalPages))
            {
                pages.Add(totalPages);
            }
            return pages;
        }

        private IEnumerable<CompanyEvent> GetPaginatedCompanyEvents()
        {
            var events = CompanyEventsToShowAtFrontPage;
            return events
                .Where(e => e.CompanyEventStatus == "Δημοσιευμένη")
                .OrderByDescending(e => e.CompanyEventActiveDate)
                .Skip((currentCompanyEventPage - 1) * currentCompanyEventpageSize)
                .Take(currentCompanyEventpageSize);
        }

        private IEnumerable<ProfessorEvent> GetPaginatedProfessorEvents()
        {
            var events = ProfessorEventsToShowAtFrontPage;
            return events
                .Where(e => e.ProfessorEventStatus == "Δημοσιευμένη")
                .OrderByDescending(e => e.ProfessorEventActiveDate)
                .Skip((currentProfessorEventPage - 1) * currentProfessorEventpageSize)
                .Take(currentProfessorEventpageSize);
        }

        private IEnumerable<CompanyEvent> CompanyEventsToShowAtFrontPage => eventsForCurrentMonth;

        private IEnumerable<ProfessorEvent> ProfessorEventsToShowAtFrontPage => professorEventsForCurrentMonth;

        private int adjustedFirstDayOfMonth
        {
            get
            {
                int firstDay = (int)new DateTime(currentMonth.Year, currentMonth.Month, 1).DayOfWeek;
                return firstDay == 0 ? 6 : firstDay - 1;
            }
        }

        private int daysInCurrentMonth => DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);

        // Clear search methods
        private void ClearSearchFieldsAsRGToFindCompany()
        {
            searchCompanyEmailAsRGToFindCompany = "";
            searchCompanyNameENGAsRGToFindCompany = "";
            searchCompanyTypeAsRGToFindCompany = "";
            searchCompanyActivityrAsRGToFindCompany = "";
            searchCompanyTownAsRGToFindCompany = "";
            searchCompanyAreasAsRGToFindCompany = "";
            searchCompanyDesiredSkillsAsRGToFindCompany = "";
            searchResultsAsRGToFindCompany.Clear();
            companyNameSuggestionsAsRG.Clear();
            areasOfInterestSuggestions.Clear();
            selectedAreasOfInterest.Clear();
            UpdateCompanySearchPagination();
            showCompanyDetailsModal = false;
            selectedCompany = null;
        }

        private void ClearSearchFieldsAsRGToFindProfessor()
        {
            searchNameSurnameAsRGToFindProfessor = "";
            searchDepartmentAsRGToFindProfessor = "";
            searchAreasOfInterestAsRGToFindProfessor = "";
            searchSchoolAsRGToFindProfessor = "";
            searchResultsAsRGToFindProfessor.Clear();
            selectedAreasOfInterestAsRG.Clear();
            areasOfInterestSuggestionsAsRG.Clear();
            professorNameSurnameSuggestionsAsRG.Clear();
            UpdateProfessorSearchPagination();
            showProfessorDetailsModalWhenSearchForProfessorsAsRG = false;
            selectedProfessorWhenSearchForProfessorsAsRG = null;
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

        private async Task OpenMap(string location)
        {
            if (!string.IsNullOrWhiteSpace(location))
            {
                var encodedLocation = Uri.EscapeDataString(location);
                await JS.InvokeVoidAsync("open", $"https://www.google.com/maps/search/?api=1&query={encodedLocation}", "_blank");
            }
        }

        private async Task OpenUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                    !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    url = $"http://{url}";
                }
                await JS.InvokeVoidAsync("open", url, "_blank");
            }
        }

        // Announcement helpers
        private void ToggleDescription(int announcementId)
        {
            expandedAnnouncementId = expandedAnnouncementId == announcementId ? -1 : announcementId;
        }

        private void ToggleDescriptionForProfessorAnnouncements(int announcementId)
        {
            expandedProfessorAnnouncementId = expandedProfessorAnnouncementId == announcementId ? -1 : announcementId;
        }

        private async Task DownloadAnnouncementAttachmentFrontPage(byte[] attachmentData, string fileName)
        {
            if (attachmentData != null && attachmentData.Length > 0)
            {
                await JS.InvokeVoidAsync("BlazorDownloadAttachmentPositionFile", fileName, "application/pdf", attachmentData);
            }
        }

        private async Task DownloadProfessorAnnouncementAttachmentFrontPage(byte[] attachmentData, string fileName)
        {
            if (attachmentData != null && attachmentData.Length > 0)
            {
                await JS.InvokeVoidAsync("BlazorDownloadAttachmentPositionFile", fileName, "application/pdf", attachmentData);
            }
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

        private void GoToPageForCompanyAnnouncements(int pageNumber)
        {
            if (pageNumber >= 1 && pageNumber <= Math.Max(1, (int)Math.Ceiling((double)Announcements.Count(a => a.CompanyAnnouncementStatus == "Δημοσιευμένη") / pageSize)))
            {
                currentPageForCompanyAnnouncements = pageNumber;
            }
        }

        private void NextPageForCompanyAnnouncements()
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling((double)Announcements.Count(a => a.CompanyAnnouncementStatus == "Δημοσιευμένη") / pageSize));
            if (currentPageForCompanyAnnouncements < totalPages)
            {
                currentPageForCompanyAnnouncements++;
            }
        }

        private void GoToLastPageForCompanyAnnouncements()
        {
            currentPageForCompanyAnnouncements = Math.Max(1, (int)Math.Ceiling((double)Announcements.Count(a => a.CompanyAnnouncementStatus == "Δημοσιευμένη") / pageSize));
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

        private void GoToPageForProfessorAnnouncements(int pageNumber)
        {
            if (pageNumber >= 1 && pageNumber <= Math.Max(1, (int)Math.Ceiling((double)ProfessorAnnouncements.Count(a => a.ProfessorAnnouncementStatus == "Δημοσιευμένη") / pageSize)))
            {
                currentPageForProfessorAnnouncements = pageNumber;
            }
        }

        private void NextPageForProfessorAnnouncements()
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling((double)ProfessorAnnouncements.Count(a => a.ProfessorAnnouncementStatus == "Δημοσιευμένη") / pageSize));
            if (currentPageForProfessorAnnouncements < totalPages)
            {
                currentPageForProfessorAnnouncements++;
            }
        }

        private void GoToLastPageForProfessorAnnouncements()
        {
            currentPageForProfessorAnnouncements = Math.Max(1, (int)Math.Ceiling((double)ProfessorAnnouncements.Count(a => a.ProfessorAnnouncementStatus == "Δημοσιευμένη") / pageSize));
        }

        // Calendar helpers
        private async Task ShowPreviousMonth()
        {
            currentMonth = currentMonth.AddMonths(-1);
            await LoadEventsForCalendar();
        }

        private async Task ShowNextMonth()
        {
            currentMonth = currentMonth.AddMonths(1);
            await LoadEventsForCalendar();
        }

        private void OnDateClicked(DateTime date)
        {
            selectedDay = date.Day;
            highlightedDay = selectedDay;
            selectedDate = date;

            selectedDateCompanyEvents = eventsForDate.TryGetValue(date.Day, out var companyEvents)
                ? companyEvents
                : new List<CompanyEvent>();

            selectedDateProfessorEvents = eventsForDateForProfessors.TryGetValue(date.Day, out var professorEvents)
                ? professorEvents
                : new List<ProfessorEvent>();

            UpdateFilteredEvents();

            if (selectedDateCompanyEvents.Any() || selectedDateProfessorEvents.Any())
            {
                isModalVisibleToShowEventsOnCalendarForEachClickedDay = true;
            }
        }

        private void ShowEventDetails(CompanyEvent companyEvent)
        {
            selectedEvent = companyEvent;
        }

        private void ShowEventDetails(ProfessorEvent professorEvent)
        {
            selectedEvent = professorEvent;
        }

        private void CloseEventDetails()
        {
            selectedEvent = null;
        }

        private void CloseModalForCompanyAndProfessorEventTitles()
        {
            isModalVisibleToShowEventsOnCalendarForEachClickedDay = false;
            selectedEvent = null;
            selectedDate = null;
            selectedDateCompanyEvents.Clear();
            selectedDateProfessorEvents.Clear();
            filteredCompanyEvents.Clear();
            filteredProfessorEvents.Clear();
        }

        private async Task<bool> ShowInterestInCompanyEventAsProfessor(CompanyEvent companyEvent)
        {
            if (companyEvent == null || string.IsNullOrEmpty(CurrentUserEmail))
            {
                return false;
            }

            var confirmed = await JS.InvokeAsync<bool>("confirmActionWithHTML",
                $"Πρόκεται να δείξετε Ενδιαφέρον για την Εκδήλωση: {companyEvent.CompanyEventTitle}. Είστε σίγουρος/η;");

            if (!confirmed)
            {
                return false;
            }

            var alreadyInterested = await dbContext.InterestInCompanyEventsAsProfessor
                .AnyAsync(i => i.ProfessorEmailShowInterestForCompanyEvent == CurrentUserEmail &&
                               i.RNGForCompanyEventInterestAsProfessor == companyEvent.RNGForEventUploadedAsCompany);

            if (alreadyInterested)
            {
                await JS.InvokeVoidAsync("confirmActionWithHTML2", "Έχετε ήδη δείξει ενδιαφέρον για αυτήν την Εκδήλωση.");
                return false;
            }

            var professor = await dbContext.Professors.FirstOrDefaultAsync(p => p.ProfEmail == CurrentUserEmail);
            if (professor == null)
            {
                await JS.InvokeVoidAsync("confirmActionWithHTML2", "Δεν βρέθηκαν στοιχεία Καθηγητή.");
                return false;
            }

            var interest = new InterestInCompanyEventAsProfessor
            {
                RNGForCompanyEventInterestAsProfessor = companyEvent.RNGForEventUploadedAsCompany,
                RNGForCompanyEventInterestAsProfessor_HashedAsUniqueID = companyEvent.RNGForEventUploadedAsCompany_HashedAsUniqueID,
                DateTimeProfessorShowInterestForCompanyEvent = DateTime.UtcNow,
                CompanyEventStatus_ShowInterestAsProfessor_AtCompanySide = "Προς Επεξεργασία",
                CompanyEventStatus_ShowInterestAsProfessor_AtProfessorSide = "Έχετε Δείξει Ενδιαφέρον",
                ProfessorEmailShowInterestForCompanyEvent = professor.ProfEmail,
                ProfessorUniqueIDShowInterestForCompanyEvent = professor.ProfUniqueID,
                ProfessorNameShowInterestForCompanyEvent = professor.ProfName,
                ProfessorSurnameShowInterestForCompanyEvent = professor.ProfSurname,
                ProfessorDepartmentShowInterestForCompanyEvent = professor.ProfDepartment,
                ProfessorUniversityShowInterestForCompanyEvent = professor.ProfUniversity,
                CompanyEmailShowInterestAsProfessorForCompanyEvent = companyEvent.CompanyEmailUsedToUploadEvent,
                CompanyNameShowInterestAsProfessorForCompanyEvent = companyEvent.Company?.CompanyName,
                CompanyUniqueIDShowInterestAsProfessorForCompanyEvent = companyEvent.Company?.CompanyUniqueId,
                CompanyEventTitleShowInterestAsProfessorForCompanyEvent = companyEvent.CompanyEventTitle,
                CompanyEventDescriptionShowInterestAsProfessorForCompanyEvent = companyEvent.CompanyEventDescription,
                CompanyEventActiveDateShowInterestAsProfessorForCompanyEvent = companyEvent.CompanyEventActiveDate,
                CompanyEventTimeShowInterestAsProfessorForCompanyEvent = companyEvent.CompanyEventTime,
                CompanyEventLocationShowInterestAsProfessorForCompanyEvent = companyEvent.CompanyEventLocation,
                CompanyEventCityShowInterestAsProfessorForCompanyEvent = companyEvent.CompanyEventCity,
                CompanyEventNeedsTransportShowInterestAsProfessorForCompanyEvent = companyEvent.CompanyEventNeedsTransport,
                CompanyEventStartingPointShowInterestAsProfessorForCompanyEvent = companyEvent.CompanyEventStartingPoint,
                CompanyEventStatusShowInterestAsProfessorForCompanyEvent = companyEvent.CompanyEventStatus,
                CompanyEventUploadDateShowInterestAsProfessorForCompanyEvent = companyEvent.CompanyEventUploadDate,
                CompanyEventRNGForCompanyEventUploadedAsProfessor = companyEvent.RNGForEventUploadedAsCompany
            };

            dbContext.InterestInCompanyEventsAsProfessor.Add(interest);
            await dbContext.SaveChangesAsync();

            await JS.InvokeVoidAsync("confirmActionWithHTML2", "Η εκδήλωση προστέθηκε στις εκδηλώσεις ενδιαφέροντός σας.");
            return true;
        }

        private void UpdateFilteredEvents()
        {
            filteredCompanyEvents = selectedDateCompanyEvents
                .Where(e => selectedEventFilter == "All" || selectedEventFilter == "Company")
                .OrderBy(e => e.CompanyEventActiveDate)
                .ToList();

            filteredProfessorEvents = selectedDateProfessorEvents
                .Where(e => selectedEventFilter == "All" || selectedEventFilter == "Professor")
                .OrderBy(e => e.ProfessorEventActiveDate)
                .ToList();
        }

        private void UpdateCompanySearchPagination()
        {
            totalPages_CompanySearchAsRG = Math.Max(1, (int)Math.Ceiling((double)searchResultsAsRGToFindCompany.Count / CompanySearchPerPageAsRG));
            if (currentPage_CompanySearchAsRG > totalPages_CompanySearchAsRG)
            {
                currentPage_CompanySearchAsRG = totalPages_CompanySearchAsRG;
            }
        }

        private void UpdateProfessorSearchPagination()
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling((double)searchResultsAsRGToFindProfessor.Count / ProfessorsPerPage_SearchForProfessorsAsRG));
            if (currentProfessorPage_SearchForProfessorsAsRG > totalPages)
            {
                currentProfessorPage_SearchForProfessorsAsRG = totalPages;
            }
        }

        private async Task HandleCompanyInputAsRG(ChangeEventArgs e)
        {
            searchCompanyNameENGAsRGToFindCompany = e.Value?.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(searchCompanyNameENGAsRGToFindCompany))
            {
                companyNameSuggestionsAsRG.Clear();
            }
            else
            {
                var normalized = searchCompanyNameENGAsRGToFindCompany.Trim();
                companyNameSuggestionsAsRG = await dbContext.Companies
                    .Where(c => c.CompanyName.Contains(normalized))
                    .OrderBy(c => c.CompanyName)
                    .Select(c => c.CompanyName)
                    .Distinct()
                    .Take(10)
                    .ToListAsync();
            }
        }

        private async Task HandleAreasOfInterestInput_WhenSearchForCompanyAsRG(ChangeEventArgs e)
        {
            searchCompanyAreasAsRGToFindCompany = e.Value?.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(searchCompanyAreasAsRGToFindCompany))
            {
                areasOfInterestSuggestions.Clear();
            }
            else
            {
                var normalized = searchCompanyAreasAsRGToFindCompany.Trim();
                areasOfInterestSuggestions = await dbContext.Companies
                    .Where(c => c.CompanyAreas.Contains(normalized))
                    .OrderBy(c => c.CompanyAreas)
                    .Select(c => c.CompanyAreas)
                    .Distinct()
                    .Take(10)
                    .ToListAsync();
            }
        }

        private async Task HandleProfessorInputWhenSearchForProfessorAsRG(ChangeEventArgs e)
        {
            searchNameSurnameAsRGToFindProfessor = e.Value?.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(searchNameSurnameAsRGToFindProfessor))
            {
                professorNameSurnameSuggestionsAsRG.Clear();
            }
            else
            {
                var normalized = searchNameSurnameAsRGToFindProfessor.Trim();
                professorNameSurnameSuggestionsAsRG = await dbContext.Professors
                    .Where(p => (p.ProfName + " " + p.ProfSurname).Contains(normalized))
                    .OrderBy(p => p.ProfSurname)
                    .Select(p => p.ProfName + " " + p.ProfSurname)
                    .Distinct()
                    .Take(10)
                    .ToListAsync();
            }
        }

        private async Task HandleAreasOfInterestInputAsRG(ChangeEventArgs e)
        {
            searchAreasOfInterestAsRGToFindProfessor = e.Value?.ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(searchAreasOfInterestAsRGToFindProfessor))
            {
                areasOfInterestSuggestionsAsRG.Clear();
            }
            else
            {
                var normalized = searchAreasOfInterestAsRGToFindProfessor.Trim();
                areasOfInterestSuggestionsAsRG = await dbContext.Areas
                    .Where(a => a.AreaName.Contains(normalized))
                    .OrderBy(a => a.AreaName)
                    .Select(a => a.AreaName)
                    .Take(10)
                    .ToListAsync();
            }
        }

        private IEnumerable<string> ForeasType => companyTypesAsRG.Any()
            ? companyTypesAsRG
            : new List<string>
            {
                "Εταιρεία",
                "Ερευνητικός Φορέας",
                "Δημόσιος Οργανισμός",
                "Μη Κερδοσκοπικός Οργανισμός",
                "Άλλο"
            };

        private IEnumerable<string> filteredProfessorDepartmentsAsRG
        {
            get
            {
                if (string.IsNullOrWhiteSpace(searchSchoolAsRGToFindProfessor))
                {
                    return universityDepartments.SelectMany(x => x.Value).Distinct().OrderBy(x => x);
                }

                if (universityDepartments.TryGetValue(searchSchoolAsRGToFindProfessor, out var departments))
                {
                    return departments;
                }

                return Enumerable.Empty<string>();
            }
        }

        private IEnumerable<string> GetAllProfessorDepartments()
        {
            return universityDepartments.SelectMany(kvp => kvp.Value).Distinct().OrderBy(d => d);
        }

        private async Task OnProfessorSchoolChangedAsRG(ChangeEventArgs e)
        {
            searchSchoolAsRGToFindProfessor = e.Value?.ToString() ?? string.Empty;
            searchDepartmentAsRGToFindProfessor = string.Empty;
            await InvokeAsync(StateHasChanged);
        }

        private void SelectAreasOfInterestSuggestionAsRG(string suggestion)
        {
            if (!selectedAreasOfInterestAsRG.Contains(suggestion))
            {
                selectedAreasOfInterestAsRG.Add(suggestion);
            }

            searchAreasOfInterestAsRGToFindProfessor = string.Join(", ", selectedAreasOfInterestAsRG);
            areasOfInterestSuggestionsAsRG.Clear();
        }

        private void RemoveSelectedAreaOfInterestAsRG(string area)
        {
            if (selectedAreasOfInterestAsRG.Remove(area))
            {
                searchAreasOfInterestAsRGToFindProfessor = selectedAreasOfInterestAsRG.Any()
                    ? string.Join(", ", selectedAreasOfInterestAsRG)
                    : string.Empty;
            }
        }

        private void SelectCompanyNameSuggestionAsRG(string suggestion)
        {
            searchCompanyNameENGAsRGToFindCompany = suggestion;
            companyNameSuggestionsAsRG.Clear();
        }

        private void SelectAreasOfInterestSuggestion_WhenSearchForCompanyAsRG(string suggestion)
        {
            if (!selectedAreasOfInterest.Contains(suggestion))
            {
                selectedAreasOfInterest.Add(suggestion);
            }

            searchCompanyAreasAsRGToFindCompany = string.Join(", ", selectedAreasOfInterest);
            areasOfInterestSuggestions.Clear();
        }

        private void RemoveSelectedAreaOfInterest_WhenSearchForCompanyAsRG(string area)
        {
            if (selectedAreasOfInterest.Remove(area) && !selectedAreasOfInterest.Any())
            {
                searchCompanyAreasAsRGToFindCompany = string.Empty;
            }
            else
            {
                searchCompanyAreasAsRGToFindCompany = string.Join(", ", selectedAreasOfInterest);
            }
        }

        private void SelectProfessorNameSurnameSuggestionAsRG(string suggestion)
        {
            searchNameSurnameAsRGToFindProfessor = suggestion;
            professorNameSurnameSuggestionsAsRG.Clear();
        }

        // Helper classes for modal data
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
            public string ProjectStatus { get; set; } = string.Empty;
        }

        public class PatentDetail
        {
            public string PatentTitle { get; set; } = string.Empty;
            public string PatentType { get; set; } = string.Empty;
            public string PatentDOI { get; set; } = string.Empty;
            public string PatentURL { get; set; } = string.Empty;
            public string PatentDescription { get; set; } = string.Empty;
            public string PatentStatus { get; set; } = string.Empty;
        }

        // Get faculty members details for modal
        private async Task<List<FacultyMemberDetail>> GetFacultyMembersDetails()
        {
            if (researchGroupData == null) return new List<FacultyMemberDetail>();

            return await dbContext.ResearchGroup_Professors
                .Where(rp => rp.PK_ResearchGroupEmail == researchGroupData.ResearchGroupEmail)
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
        }

        // Get non-faculty members details for modal
        private async Task<List<NonFacultyMemberDetail>> GetNonFacultyMembersDetails()
        {
            if (researchGroupData == null) return new List<NonFacultyMemberDetail>();

            return await dbContext.ResearchGroup_NonFacultyMembers
                .Where(rnf => rnf.PK_ResearchGroupEmail == researchGroupData.ResearchGroupEmail)
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
        }

        // Get research actions details for modal
        private async Task<List<ResearchActionDetail>> GetResearchActionsDetails()
        {
            if (researchGroupData == null) return new List<ResearchActionDetail>();

            return await dbContext.ResearchGroup_ResearchActions
                .Where(ra => ra.ResearchGroupEmail == researchGroupData.ResearchGroupEmail)
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
        }

        // Get patents details for modal
        private async Task<List<PatentDetail>> GetPatentsDetails()
        {
            if (researchGroupData == null) return new List<PatentDetail>();

            return await dbContext.ResearchGroup_Patents
                .Where(p => p.ResearchGroupEmail == researchGroupData.ResearchGroupEmail)
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
        }

        // Utility methods
        private string GetImageSource(byte[] imageBytes)
        {
            if (imageBytes != null && imageBytes.Length > 0)
            {
                return "data:image/png;base64," + Convert.ToBase64String(imageBytes);
            }
            return string.Empty;
        }

    }
}
