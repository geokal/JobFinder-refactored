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
    public partial class StudentLayoutSection
    {
        [Parameter] public bool IsInitialized { get; set; }
        [Parameter] public bool IsRegistered { get; set; }
        [Parameter] public EventCallback<bool> IsRegisteredChanged { get; set; }

        // DbContext injection removed - data loading is handled by parent MainLayout
        [Inject] private Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private Data.AppDbContext dbContext { get; set; }

        // Student-specific properties
        private List<ProfessorThesisApplied> thesisApplications = new();
        private List<CompanyThesisApplied> companythesisApplications = new();
        private List<CompanyJobApplied> jobApplications = new();
        private List<InternshipApplied> internshipApplications = new();
        private List<ProfessorInternshipApplied> professorInternshipApplications = new();

        // UI state properties
        private bool showStudentThesisApplications = false;
        private bool showStudentJobApplications = false;
        private bool showStudentInternshipApplications = false;
        private bool isAnnouncementsAsStudentVisible = false;
        private bool isEventSearchAsStudentVisible = false;

        private bool isJobApplicationsAsStudentVisible = false;
        private bool isJobPositionAsStudentFiltersVisible = false;
        private bool isInternshipApplicationsAsStudentVisible = false;
        private bool isInternshipSearchAsStudentFiltersVisible = false;
        private bool isThesisAreasVisible = false;
        private bool isPositionAreasVisible = false;
        private bool isInternshipAreasVisible = false;
        private bool showInternships = false;
        private bool showJobApplications = false;

        // Search and filter properties
        private string thesisSearchForThesesAsStudent = "";
        private string professorNameSearchForThesesAsStudent = "";
        private string professorSurnameSearchForThesesAsStudent = "";
        private string companyNameSearchForThesesAsStudent = "";
        private string jobSearch = "";
        private string companyinternshipSearch = "";
        private string companyNameSearch = "";
        private string emailSearch = "";
        private string positionTypeSearch = "";
        private string companyinternshipSearchByType = "";
        private string companyinternshipSearchByESPA = "";
        private string companyinternshipSearchByRegion = "";
        private string jobSearchByTown = "";
        private string jobSearchByRegion = "";
        private string companyinternshipSearchByTown = "";
        private bool companyinternshipSearchByTransportOffer = false;
        private bool companyjobSearchByTransportOffer = false;
        private string companyinternshipSearchByArea = "";
        private DateTime? selectedDateToSearchJob;
        private DateTime? selectedDateToSearchInternship;
        private DateTime? finishEstimationDateToSearchInternship;
        private DateTime? thesisStartDateForThesesAsStudent;
        private int? thesisUploadMonthForThesesAsStudent;

        // Pagination properties
        private int currentPageForThesisToSee = 1;
        private int pageSizeForThesisToSee = 3;
        private int currentJobPage = 1;
        private int jobPageSize = 3;
        private int currentInternshipPage = 1;
        private int InternshipsPerPage = 3;
        private int currentThesisPage = 1;
        private int thesisPageSize = 3;

        // Data collections
        private List<Area> Areas = new();
        private List<Skill> Skills = new();
        private List<string> selectedThesisAreas = new();
        private List<string> selectedPositionAreas = new();
        private List<string> selectedAreas = new();
        private List<AllTheses> sumUpThesesFromBothCompanyAndProfessor = new();
        private List<AllInternships> sumUpInternshipsFromBothCompanyAndProfessor = new();
        private List<CompanyJob> jobs = new();
        private List<CompanyInternship> internships = new();
        private List<ProfessorInternship> professorInternships = new();
        private List<CompanyEvent> companyEventsToSeeAsStudent = new();
        private List<ProfessorEvent> professorEventsToSeeAsStudent = new();
        private List<InterestInCompanyEvent> InterestedStudents = new();
        private List<InterestInProfessorEvent> InterestedStudentsForProfessorEvent = new();

        // News and announcements
        private List<NewsArticle> newsArticles = new();
        private List<NewsArticle> svseNewsArticles = new();
        private List<AnnouncementAsCompany> Announcements = new();
        private List<AnnouncementAsProfessor> ProfessorAnnouncements = new();

        // Thesis functionality
        private List<AllTheses> availableTheses = new();
        private List<ProfessorThesisApplied> professorThesisApplications = new();
        private List<CompanyThesisApplied> companyThesisApplications = new();
        private bool showAvailableTheses = true;
        private bool showThesisApplications = true;
        private string thesisSearchFilter = "all";

        // UI visibility flags
        private bool isUniversityNewsVisible = false;
        private bool isSvseNewsVisible = false;
        private bool isCompanyAnnouncementsVisible = false;
        private int? expandedAnnouncementId = null;
        private int currentPageForCompanyAnnouncements = 1;
        private int pageSize = 5;
        private bool isProfessorAnnouncementsVisible = false;
        private bool isCompanyEventsVisible = false;
        private bool isProfessorEventsVisible = false;

        // Additional properties for missing sections
        private int? expandedProfessorAnnouncementId = null;
        private int currentPageForProfessorAnnouncements = 1;
        private int? expandedCompanyEventId = null;
        private int currentCompanyEventPage = 1;
        private int currentCompanyEventpageSize = 5;
        private int? expandedProfessorEventId = null;
        private int currentProfessorEventPage = 1;
        private int currentProfessorEventpageSize = 5;

        // Data collections for events
        private List<CompanyEvent> CompanyEventsToShowAtFrontPage = new();
        private List<ProfessorEvent> ProfessorEventsToShowAtFrontPage = new();

        // Event handling
        private Dictionary<long, bool> needsTransportForCompanyEvent = new();
        private Dictionary<long, bool> needsTransportForProfessorEvent = new();
        private Dictionary<long, string> selectedStartingPoint = new();
        private HashSet<long> alreadyInterestedCompanyEventIds = new();
        private HashSet<long> interestedProfessorEventIds = new();
        private HashSet<long> professorThesisIdsApplied = new();
        private HashSet<long> companyThesisIdsApplied = new();
        private HashSet<long> jobIdsApplied = new();
        private HashSet<long> internshipIdsApplied = new();
        private HashSet<long> professorInternshipIdsApplied = new();

        // Student data
        private Student userData;
        private string selectedEventType = "all";
        private object selectedEvent = null;
        private string selectedEventFilter = "All";
        private List<CompanyEvent> selectedDateEvents = new();
        private List<ProfessorEvent> selectedProfessorDateEvents = new();
        private bool isModalVisibleToShowEventsOnCalendarForEachClickedDay = false;
        private int selectedDay = 0;
        private int highlightedDay = 0;
        private DateTime? selectedDate;

        // Filter and pagination options
        private int[] pageSizeOptions_SeeMyThesisApplicationsAsStudent = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int[] pageSizeOptions_SearchForThesisAsStudent = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int[] pageSizeOptions_SeeMyJobApplicationsAsStudent = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int[] pageSizeOptions_SearchForJobsAsStudent = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int[] pageSizeOptions_SeeMyInternshipApplicationsAsStudent = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int[] pageSizeOptions_SearchForInternshipsAsStudent = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int[] pageSizeOptions_SearchForEventsAsStudent = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // Autocomplete suggestions
        private List<string> professorNameSurnameSuggestions = new();
        private List<string> thesisTitleSuggestions = new();
        private List<string> companyNameSuggestionsWhenSearchForProfessorThesisAutocompleteNameAsStudent = new();
        private List<string> jobTitleAutocompleteSuggestionsWhenSearchForCompanyJobsAsStudent = new();
        private List<string> companyNameAutocompleteSuggestionsWhenSearchForCompanyJobsAsStudent = new();
        private List<string> internshipTitleAutocompleteSuggestionsWhenSearchInternshipAsStudent = new();

        // Calendar properties
        private DateTime currentMonth = DateTime.Today;
        private string[] daysOfWeek = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        private int firstDayOfMonth => (int)new DateTime(currentMonth.Year, currentMonth.Month, 1).DayOfWeek;
        private int daysInCurrentMonth => DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
        private int adjustedFirstDayOfMonth => (firstDayOfMonth == 0) ? 6 : firstDayOfMonth - 1;
        private Dictionary<int, List<CompanyEvent>> eventsForDate = new();
        private Dictionary<int, List<ProfessorEvent>> eventsForDateForProfessors = new();

        // Component initialization
        protected override async Task OnInitializedAsync()
        {
            // Load news and announcements data
            newsArticles = await FetchNewsArticlesAsync();
            svseNewsArticles = await FetchSVSENewsArticlesAsync();
            Announcements = await FetchAnnouncementsAsync();
            ProfessorAnnouncements = await FetchProfessorAnnouncementsAsync();
            
            // Load thesis data
            await LoadThesisApplications();
            await LoadAvailableTheses();
        }

        // Data loading methods - now handled by parent MainLayout
        // These methods are kept for future use when data is passed as parameters

        // UI toggle methods
        private void ToggleAnnouncementsAsStudentVisibility()
        {
            isAnnouncementsAsStudentVisible = !isAnnouncementsAsStudentVisible;
        }

        private void ToggleEventSearchAsStudentVisibility()
        {
            isEventSearchAsStudentVisible = !isEventSearchAsStudentVisible;
        }

        private void ToggleJobApplicationsAsStudentVisibility()
        {
            isJobApplicationsAsStudentVisible = !isJobApplicationsAsStudentVisible;
        }

        private void ToggleJobPositionAsStudentFiltersVisibility()
        {
            isJobPositionAsStudentFiltersVisible = !isJobPositionAsStudentFiltersVisible;
        }

        private void ToggleInternshipApplicationsAsStudentVisibility()
        {
            isInternshipApplicationsAsStudentVisible = !isInternshipApplicationsAsStudentVisible;
        }

        private void ToggleInternshipSearchAsStudentFiltersVisibility()
        {
            isInternshipSearchAsStudentFiltersVisible = !isInternshipSearchAsStudentFiltersVisible;
        }

        private void ToggleThesisAreasVisibility()
        {
            isThesisAreasVisible = !isThesisAreasVisible;
        }

        private void TogglePositionAreasVisibility()
        {
            isPositionAreasVisible = !isPositionAreasVisible;
        }

        private void ToggleInternshipAreasVisibility()
        {
            isInternshipAreasVisible = !isInternshipAreasVisible;
        }

        // News visibility toggles
        private void ToggleUniversityNewsVisibility()
        {
            isUniversityNewsVisible = !isUniversityNewsVisible;
        }

        private void ToggleSvseNewsVisibility()
        {
            isSvseNewsVisible = !isSvseNewsVisible;
        }

        public void ToggleCompanyAnnouncementsVisibility()
        {
            isCompanyAnnouncementsVisible = !isCompanyAnnouncementsVisible;
        }

        public void ToggleDescription(int announcementId)
        {
            expandedAnnouncementId = expandedAnnouncementId == announcementId ? null : announcementId;
        }

        private void ToggleProfessorAnnouncementsVisibility()
        {
            isProfessorAnnouncementsVisible = !isProfessorAnnouncementsVisible;
        }

        private void ToggleCompanyEventsVisibility()
        {
            isCompanyEventsVisible = !isCompanyEventsVisible;
        }

        private void ToggleProfessorEventsVisibility()
        {
            isProfessorEventsVisible = !isProfessorEventsVisible;
        }

        // Additional toggle methods for missing sections

        public void ToggleDescriptionForProfessorAnnouncements(int announcementId)
        {
            expandedProfessorAnnouncementId = expandedProfessorAnnouncementId == announcementId ? null : announcementId;
        }

        public void ToggleDescriptionForCompanyEvent(int eventId)
        {
            expandedCompanyEventId = expandedCompanyEventId == eventId ? null : eventId;
        }

        public void ToggleDescriptionForProfessorEvent(int eventId)
        {
            expandedProfessorEventId = expandedProfessorEventId == eventId ? null : eventId;
        }

        // Application visibility toggles
        private async Task ToggleAndLoadStudentThesisApplications()
        {
            showStudentThesisApplications = !showStudentThesisApplications;
            // Data loading is now handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task ToggleAndLoadStudentJobApplications()
        {
            showStudentJobApplications = !showStudentJobApplications;
            // Data loading is now handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task ToggleAndLoadStudentInternshipApplications()
        {
            showStudentInternshipApplications = !showStudentInternshipApplications;
            // Data loading is now handled by parent MainLayout
            await Task.CompletedTask;
        }

        // Search methods
        private async Task SearchThesisApplicationsAsStudent()
        {
            // Implementation for thesis search
            await InvokeAsync(StateHasChanged);
        }

        private async Task SearchJobApplicationsAsStudent()
        {
            // Implementation for job search
            await InvokeAsync(StateHasChanged);
        }

        private async Task SearchInternshipsAsStudent()
        {
            // Implementation for internship search
            await InvokeAsync(StateHasChanged);
        }

        // Calendar methods
        private void ShowPreviousMonth()
        {
            currentMonth = currentMonth.AddMonths(-1);
            // Event loading is now handled by parent MainLayout
        }

        private void ShowNextMonth()
        {
            currentMonth = currentMonth.AddMonths(1);
            // Event loading is now handled by parent MainLayout
        }

        private void OnDateClicked(DateTime clickedDate)
        {
            selectedDay = clickedDate.Day;
            highlightedDay = selectedDay;
            selectedDate = clickedDate;

            // Event loading is now handled by parent MainLayout
            // For now, just show the modal
            isModalVisibleToShowEventsOnCalendarForEachClickedDay = true;
        }

        private void CloseModalForEventsOnCalendar()
        {
            isModalVisibleToShowEventsOnCalendarForEachClickedDay = false;
            selectedDateEvents.Clear();
            selectedProfessorDateEvents.Clear();
        }

        // Event interest methods
        private async Task ShowInterestInCompanyEvent(CompanyEvent companyEvent, bool needsTransport)
        {
            var confirmed = await JS.InvokeAsync<bool>("confirm",
                $"Πρόκεται να δείξετε Ενδιαφέρον για την Εκδήλωση: {companyEvent.CompanyEventTitle}. Είστε σίγουρος/η;");

            if (confirmed)
            {
                // Implementation for showing interest in company event
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task ShowInterestInProfessorEvent(ProfessorEvent professorEvent, bool needsTransport)
        {
            var confirmed = await JS.InvokeAsync<bool>("confirm",
                $"Πρόκεται να δείξετε Ενδιαφέρον για την Εκδήλωση: {professorEvent.ProfessorEventTitle}. Είστε σίγουρος/η;");

            if (confirmed)
            {
                // Implementation for showing interest in professor event
                await InvokeAsync(StateHasChanged);
            }
        }

        // Application methods
        private async Task ApplyForThesisAsStudent(AllTheses thesis)
        {
            var confirmed = await JS.InvokeAsync<bool>("confirm",
                $"Πρόκεται να κάνετε Αίτηση για την Πτυχιακή Εργασία: {thesis.ThesisTitle}. Είστε σίγουρος/η;");

            if (confirmed)
            {
                // Implementation for thesis application
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task ApplyForJobAsStudent(CompanyJob job)
        {
            var confirmed = await JS.InvokeAsync<bool>("confirm",
                $"Πρόκεται να κάνετε Αίτηση για την Θέση: {job.PositionTitle}. Είστε σίγουρος/η;");

            if (confirmed)
            {
                // Implementation for job application
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task ApplyForInternshipAsStudent(CompanyInternship internship)
        {
            var confirmed = await JS.InvokeAsync<bool>("confirm",
                $"Πρόκεται να κάνετε Αίτηση για την Πρακτική: {internship.CompanyInternshipTitle}. Είστε σίγουρος/η;");

            if (confirmed)
            {
                // Implementation for internship application
                await InvokeAsync(StateHasChanged);
            }
        }

        // Utility methods
        private void ClearSearchFieldsForThesisAsStudent()
        {
            thesisSearchForThesesAsStudent = "";
            professorNameSearchForThesesAsStudent = "";
            professorSurnameSearchForThesesAsStudent = "";
            companyNameSearchForThesesAsStudent = "";
            selectedThesisAreas.Clear();
            isThesisAreasVisible = false;
        }

        private void ClearSearchFieldsForJobApplicationsAsStudent()
        {
            jobSearch = "";
            companyNameSearch = "";
            emailSearch = "";
            positionTypeSearch = "";
            selectedPositionAreas.Clear();
            isPositionAreasVisible = false;
        }

        private void ClearSearchFieldsForInternshipsAsStudent()
        {
            companyinternshipSearch = "";
            companyinternshipSearchByType = "";
            companyinternshipSearchByRegion = "";
            companyinternshipSearchByTransportOffer = false;
            selectedAreas.Clear();
            isInternshipAreasVisible = false;
        }

        // Pagination methods
        private void OnPageSizeChangeForThesisApplications(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                pageSizeForThesisToSee = newSize;
                currentPageForThesisToSee = 1;
            }
        }

        private void OnPageSizeChangeForJobApplications(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                jobPageSize = newSize;
                currentJobPage = 1;
            }
        }

        private void OnPageSizeChangeForInternshipApplications(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                InternshipsPerPage = newSize;
                currentInternshipPage = 1;
            }
        }

        // External data fetching methods
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
                    for (int i = 0; i < Math.Min(articleNodes.Count, 3); i++)
                    {
                        var articleNode = articleNodes[i];
                        var titleNode = articleNode.SelectSingleNode(".//h3[@class='article__title']/a");
                        var title = titleNode?.InnerText.Trim();
                        var relativeUrl = titleNode?.Attributes["href"]?.Value;
                        var url = new Uri(new Uri("https://www.uoa.gr"), relativeUrl).ToString();

                        articles.Add(new NewsArticle { Title = title, Url = url });
                    }
                }

                return articles;
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
                var response = await HttpClient.GetAsync("https://svse.gr/index.php/nea-anakoinoseis");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                var htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(content);

                var articles = new List<NewsArticle>();
                var articleNodes = htmlDocument.DocumentNode.SelectNodes("/html/body/div[1]/div/section[2]/div/div/div/main/div/div[3]/div[1]/div/div");

                if (articleNodes != null)
                {
                    foreach (var articleNode in articleNodes.Take(3))
                    {
                        var titleNode = articleNode.SelectSingleNode(".//h2/a");
                        var title = titleNode?.InnerText.Trim();
                        var relativeUrl = titleNode?.Attributes["href"]?.Value;
                        var url = new Uri(new Uri("https://svse.gr"), relativeUrl).ToString();

                        articles.Add(new NewsArticle { Title = title, Url = url, Category = "SVSE News" });
                    }
                }

                return articles;
            }
            catch
            {
                return new List<NewsArticle>();
            }
        }

        private async Task<List<AnnouncementAsCompany>> FetchAnnouncementsAsync()
        {
            // Data fetching is now handled by parent MainLayout
            return new List<AnnouncementAsCompany>();
        }

        private async Task<List<AnnouncementAsProfessor>> FetchProfessorAnnouncementsAsync()
        {
            return await dbContext.AnnouncementsAsProfessor
                .Where(a => a.ProfessorAnnouncementStatus == "Δημοσιευμένη")
                .ToListAsync();
        }



        // Pagination methods for company announcements
        public void GoToFirstPageForCompanyAnnouncements()
        {
            currentPageForCompanyAnnouncements = 1;
        }

        public void PreviousPageForCompanyAnnouncements()
        {
            if (currentPageForCompanyAnnouncements > 1)
                currentPageForCompanyAnnouncements--;
        }

        public void NextPageForCompanyAnnouncements()
        {
            if (currentPageForCompanyAnnouncements < GetTotalPagesForCompanyAnnouncements())
                currentPageForCompanyAnnouncements++;
        }

        public void GoToLastPageForCompanyAnnouncements()
        {
            currentPageForCompanyAnnouncements = GetTotalPagesForCompanyAnnouncements();
        }

        public void GoToPageForCompanyAnnouncements(int pageNumber)
        {
            currentPageForCompanyAnnouncements = pageNumber;
        }

        public int GetTotalPagesForCompanyAnnouncements()
        {
            var totalAnnouncements = Announcements?.Count(a => a.CompanyAnnouncementStatus == "Δημοσιευμένη") ?? 0;
            return (int)Math.Ceiling((double)totalAnnouncements / pageSize);
        }

        public List<int> GetVisiblePagesForCompanyAnnouncements()
        {
            var totalPages = GetTotalPagesForCompanyAnnouncements();
            var currentPage = currentPageForCompanyAnnouncements;
            var pages = new List<int>();

            if (totalPages <= 7)
            {
                for (int i = 1; i <= totalPages; i++)
                    pages.Add(i);
            }
            else
            {
                if (currentPage <= 4)
                {
                    for (int i = 1; i <= 5; i++)
                        pages.Add(i);
                    pages.Add(-1); // Ellipsis
                    pages.Add(totalPages);
                }
                else if (currentPage >= totalPages - 3)
                {
                    pages.Add(1);
                    pages.Add(-1); // Ellipsis
                    for (int i = totalPages - 4; i <= totalPages; i++)
                        pages.Add(i);
                }
                else
                {
                    pages.Add(1);
                    pages.Add(-1); // Ellipsis
                    for (int i = currentPage - 1; i <= currentPage + 1; i++)
                        pages.Add(i);
                    pages.Add(-1); // Ellipsis
                    pages.Add(totalPages);
                }
            }

            return pages;
        }

        // Pagination methods for Professor Announcements
        public void GoToFirstPageForProfessorAnnouncements()
        {
            currentPageForProfessorAnnouncements = 1;
        }

        public void PreviousPageForProfessorAnnouncements()
        {
            if (currentPageForProfessorAnnouncements > 1)
                currentPageForProfessorAnnouncements--;
        }

        public void NextPageForProfessorAnnouncements()
        {
            if (currentPageForProfessorAnnouncements < GetTotalPagesForProfessorAnnouncements())
                currentPageForProfessorAnnouncements++;
        }

        public void GoToLastPageForProfessorAnnouncements()
        {
            currentPageForProfessorAnnouncements = GetTotalPagesForProfessorAnnouncements();
        }

        public void GoToPageForProfessorAnnouncements(int pageNumber)
        {
            currentPageForProfessorAnnouncements = pageNumber;
        }

        public int GetTotalPagesForProfessorAnnouncements()
        {
            var totalAnnouncements = ProfessorAnnouncements?.Count(a => a.ProfessorAnnouncementStatus == "Δημοσιευμένη") ?? 0;
            return (int)Math.Ceiling((double)totalAnnouncements / pageSize);
        }

        public List<int> GetVisiblePagesForProfessorAnnouncements()
        {
            var totalPages = GetTotalPagesForProfessorAnnouncements();
            var currentPage = currentPageForProfessorAnnouncements;
            var pages = new List<int>();

            if (totalPages <= 7)
            {
                for (int i = 1; i <= totalPages; i++)
                    pages.Add(i);
            }
            else
            {
                if (currentPage <= 4)
                {
                    for (int i = 1; i <= 5; i++)
                        pages.Add(i);
                    pages.Add(-1); // Ellipsis
                    pages.Add(totalPages);
                }
                else if (currentPage >= totalPages - 3)
                {
                    pages.Add(1);
                    pages.Add(-1); // Ellipsis
                    for (int i = totalPages - 4; i <= totalPages; i++)
                        pages.Add(i);
                }
                else
                {
                    pages.Add(1);
                    pages.Add(-1); // Ellipsis
                    for (int i = currentPage - 1; i <= currentPage + 1; i++)
                        pages.Add(i);
                    pages.Add(-1); // Ellipsis
                    pages.Add(totalPages);
                }
            }

            return pages;
        }

        // Pagination methods for Company Events
        public void GoToFirstPageForCompanyEvents()
        {
            currentCompanyEventPage = 1;
        }

        public void PreviousPageForCompanyEvents()
        {
            if (currentCompanyEventPage > 1)
                currentCompanyEventPage--;
        }

        public void NextPageForCompanyEvents()
        {
            if (currentCompanyEventPage < GetTotalPagesForCompanyEvents())
                currentCompanyEventPage++;
        }

        public void GoToLastPageForCompanyEvents()
        {
            currentCompanyEventPage = GetTotalPagesForCompanyEvents();
        }

        public void GoToPageForCompanyEvents(int pageNumber)
        {
            currentCompanyEventPage = pageNumber;
        }

        public int GetTotalPagesForCompanyEvents()
        {
            var totalEvents = CompanyEventsToShowAtFrontPage?.Count(e => e.CompanyEventStatus == "Δημοσιευμένη") ?? 0;
            return (int)Math.Ceiling((double)totalEvents / currentCompanyEventpageSize);
        }

        public List<int> GetVisiblePagesForCompanyEvents()
        {
            var totalPages = GetTotalPagesForCompanyEvents();
            var currentPage = currentCompanyEventPage;
            var pages = new List<int>();

            if (totalPages <= 7)
            {
                for (int i = 1; i <= totalPages; i++)
                    pages.Add(i);
            }
            else
            {
                if (currentPage <= 4)
                {
                    for (int i = 1; i <= 5; i++)
                        pages.Add(i);
                    pages.Add(-1); // Ellipsis
                    pages.Add(totalPages);
                }
                else if (currentPage >= totalPages - 3)
                {
                    pages.Add(1);
                    pages.Add(-1); // Ellipsis
                    for (int i = totalPages - 4; i <= totalPages; i++)
                        pages.Add(i);
                }
                else
                {
                    pages.Add(1);
                    pages.Add(-1); // Ellipsis
                    for (int i = currentPage - 1; i <= currentPage + 1; i++)
                        pages.Add(i);
                    pages.Add(-1); // Ellipsis
                    pages.Add(totalPages);
                }
            }

            return pages;
        }

        // Pagination methods for Professor Events
        public void GoToFirstPageForProfessorEvents()
        {
            currentProfessorEventPage = 1;
        }

        public void PreviousPageForProfessorEvents()
        {
            if (currentProfessorEventPage > 1)
                currentProfessorEventPage--;
        }

        public void NextPageForProfessorEvents()
        {
            if (currentProfessorEventPage < GetTotalPagesForProfessorEvents())
                currentProfessorEventPage++;
        }

        public void GoToLastPageForProfessorEvents()
        {
            currentProfessorEventPage = GetTotalPagesForProfessorEvents();
        }

        public void GoToPageForProfessorEvents(int pageNumber)
        {
            currentProfessorEventPage = pageNumber;
        }

        public int GetTotalPagesForProfessorEvents()
        {
            var totalEvents = ProfessorEventsToShowAtFrontPage?.Count(e => e.ProfessorEventStatus == "Δημοσιευμένη") ?? 0;
            return (int)Math.Ceiling((double)totalEvents / currentProfessorEventpageSize);
        }

        public List<int> GetVisiblePagesForProfessorEvents()
        {
            var totalPages = GetTotalPagesForProfessorEvents();
            var currentPage = currentProfessorEventPage;
            var pages = new List<int>();

            if (totalPages <= 7)
            {
                for (int i = 1; i <= totalPages; i++)
                    pages.Add(i);
            }
            else
            {
                if (currentPage <= 4)
                {
                    for (int i = 1; i <= 5; i++)
                        pages.Add(i);
                    pages.Add(-1); // Ellipsis
                    pages.Add(totalPages);
                }
                else if (currentPage >= totalPages - 3)
                {
                    pages.Add(1);
                    pages.Add(-1); // Ellipsis
                    for (int i = totalPages - 4; i <= totalPages; i++)
                        pages.Add(i);
                }
                else
                {
                    pages.Add(1);
                    pages.Add(-1); // Ellipsis
                    for (int i = currentPage - 1; i <= currentPage + 1; i++)
                        pages.Add(i);
                    pages.Add(-1); // Ellipsis
                    pages.Add(totalPages);
                }
            }

            return pages;
        }

        // Download attachment method
        public async Task DownloadAnnouncementAttachmentFrontPage(byte[] attachmentFile, string fileName)
        {
            try
            {
                var base64 = Convert.ToBase64String(attachmentFile);
                var mimeType = "application/octet-stream";
                var fileNameWithExtension = $"{fileName}.pdf";
                
                await JS.InvokeVoidAsync("downloadFile", base64, fileNameWithExtension, mimeType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading attachment: {ex.Message}");
            }
        }

        // Additional download methods for missing sections
        public async Task DownloadProfessorAnnouncementAttachmentFrontPage(byte[] attachmentFile, string fileName)
        {
            try
            {
                var base64 = Convert.ToBase64String(attachmentFile);
                var mimeType = "application/octet-stream";
                var fileNameWithExtension = $"{fileName}.pdf";
                
                await JS.InvokeVoidAsync("downloadFile", base64, fileNameWithExtension, mimeType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading professor announcement attachment: {ex.Message}");
            }
        }

        public async Task DownloadCompanyEventAttachmentFrontPage(byte[] attachmentFile, string fileName)
        {
            try
            {
                var base64 = Convert.ToBase64String(attachmentFile);
                var mimeType = "application/octet-stream";
                var fileNameWithExtension = $"{fileName}.pdf";
                
                await JS.InvokeVoidAsync("downloadFile", base64, fileNameWithExtension, mimeType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading company event attachment: {ex.Message}");
            }
        }

        public async Task DownloadProfessorEventAttachmentFrontPage(byte[] attachmentFile, string fileName)
        {
            try
            {
                var base64 = Convert.ToBase64String(attachmentFile);
                var mimeType = "application/octet-stream";
                var fileNameWithExtension = $"{fileName}.pdf";
                
                await JS.InvokeVoidAsync("downloadFile", base64, fileNameWithExtension, mimeType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading professor event attachment: {ex.Message}");
            }
        }

        // News Article class
        public class NewsArticle
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public string Date { get; set; }
            public string Category { get; set; }
        }

        // Thesis functionality methods
        public async Task LoadThesisApplications()
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
                        // Load professor thesis applications
                        professorThesisApplications = await dbContext.ProfessorThesesApplied
                            .Include(a => a.StudentDetails)
                            .Include(a => a.ProfessorDetails)
                            .Where(j => j.StudentEmailAppliedForProfessorThesis == userEmail)
                            .OrderByDescending(j => j.DateTimeStudentAppliedForProfessorThesis)
                            .ToListAsync();

                        // Load company thesis applications
                        companyThesisApplications = await dbContext.CompanyThesesApplied
                            .Include(a => a.StudentDetails)
                            .Include(a => a.CompanyDetails)
                            .Where(j => j.StudentEmailAppliedForThesis == userEmail)
                            .OrderByDescending(j => j.DateTimeStudentAppliedForThesis)
                            .ToListAsync();

                        showThesisApplications = true;
                        StateHasChanged();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading thesis applications: {ex.Message}");
            }
        }

        public async Task LoadAvailableTheses()
        {
            try
            {
                // Load available theses from both professors and companies
                var professorTheses = await dbContext.ProfessorTheses
                    .Where(t => t.ThesisStatus == "Δημοσιευμένη")
                    .Include(t => t.Professor)
                    .ToListAsync();

                var companyTheses = await dbContext.CompanyTheses
                    .Where(t => t.CompanyThesisStatus == "Δημοσιευμένη")
                    .Include(t => t.Company)
                    .ToListAsync();

                availableTheses = new List<AllTheses>();
                
                // Convert professor theses to AllTheses format
                foreach (var thesis in professorTheses)
                {
                    availableTheses.Add(new AllTheses
                    {
                        RNGForProfessorThesisUploaded = thesis.RNGForThesisUploaded,
                        ThesisTitle = thesis.ThesisTitle,
                        ProfessorThesisAreasUpload = thesis.ThesisAreas,
                        ThesisType = Models.ThesisType.Professor,
                        ProfessorName = thesis.Professor?.ProfName,
                        ProfessorSurname = thesis.Professor?.ProfSurname,
                        ThesisUploadDateTime = thesis.ThesisUploadDateTime,
                        ProfessorThesisStatus = thesis.ThesisStatus
                    });
                }

                // Convert company theses to AllTheses format
                foreach (var thesis in companyTheses)
                {
                    availableTheses.Add(new AllTheses
                    {
                        RNGForCompanyThesisUploaded = thesis.RNGForThesisUploadedAsCompany,
                        ThesisTitle = thesis.CompanyThesisTitle,
                        CompanyThesisAreasUpload = thesis.CompanyThesisAreasUpload,
                        ThesisType = Models.ThesisType.Company,
                        CompanyNameUploadedThesis = thesis.Company?.CompanyName,
                        ThesisUploadDateTime = thesis.CompanyThesisUploadDateTime,
                        CompanyThesisStatus = thesis.CompanyThesisStatus
                    });
                }

                showAvailableTheses = true;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading available theses: {ex.Message}");
            }
        }

        public async Task ToggleThesisApplications()
        {
            showThesisApplications = !showThesisApplications;
            if (showThesisApplications)
            {
                await LoadThesisApplications();
            }
            StateHasChanged();
        }

        public async Task ToggleAvailableTheses()
        {
            showAvailableTheses = !showAvailableTheses;
            if (showAvailableTheses)
            {
                await LoadAvailableTheses();
            }
            StateHasChanged();
        }

        public string GetThesisRowColor(object thesis)
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

        public async Task ApplyForThesis(AllTheses thesis)
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
                        // Get student data
                        var student = await dbContext.Students.FirstOrDefaultAsync(s => s.Email == userEmail);
                        if (student != null)
                        {
                        if (thesis.ThesisType == Models.ThesisType.Professor)
                        {
                            var application = new ProfessorThesisApplied
                            {
                                RNGForProfessorThesisApplied = thesis.RNGForProfessorThesisUploaded,
                                StudentEmailAppliedForProfessorThesis = userEmail,
                                StudentUniqueIDAppliedForProfessorThesis = student.Student_UniqueID,
                                DateTimeStudentAppliedForProfessorThesis = DateTime.Now,
                                ProfessorThesisStatusAppliedAtStudentSide = "Εκκρεμής"
                            };
                            dbContext.ProfessorThesesApplied.Add(application);
                        }
                        else if (thesis.ThesisType == Models.ThesisType.Company)
                        {
                            var application = new CompanyThesisApplied
                            {
                                RNGForCompanyThesisApplied = thesis.RNGForCompanyThesisUploaded,
                                StudentEmailAppliedForThesis = userEmail,
                                StudentUniqueIDAppliedForThesis = student.Student_UniqueID,
                                DateTimeStudentAppliedForThesis = DateTime.Now,
                                CompanyThesisStatusAppliedAtStudentSide = "Εκκρεμής"
                            };
                            dbContext.CompanyThesesApplied.Add(application);
                        }

                            await dbContext.SaveChangesAsync();
                            await JS.InvokeVoidAsync("alert", "Η αίτηση υποβλήθηκε επιτυχώς!");
                            
                            // Refresh the data
                            await LoadAvailableTheses();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying for thesis: {ex.Message}");
                await JS.InvokeVoidAsync("alert", "Σφάλμα κατά την υποβολή της αίτησης");
            }
        }

        protected async Task SetRegistered(bool value)
        {
            IsRegistered = value;
            if (IsRegisteredChanged.HasDelegate)
                await IsRegisteredChanged.InvokeAsync(value);
        }

    }
}
