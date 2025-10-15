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
        [Inject] private Data.AppDbContext dbContext { get; set; }
        [Inject] private Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IJSRuntime JS { get; set; }

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
        private bool showThesisApplications = false;

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

        // UI visibility flags
        private bool isUniversityNewsVisible = false;
        private bool isSvseNewsVisible = false;
        private bool isCompanyAnnouncementsVisible = false;
        private bool isProfessorAnnouncementsVisible = false;
        private bool isCompanyEventsVisible = false;
        private bool isProfessorEventsVisible = false;

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
            await LoadAreasAsync();
            await LoadSkillsAsync();
            await LoadUserApplications();
            await LoadUserJobApplications();
            await LoadUserInternshipApplications();
            await LoadNewsAndAnnouncements();
            await LoadEventsForCalendar();
            await LoadUserInterestsAndApplications();
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

        private async Task LoadUserApplications()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var userEmail = user.FindFirst("name")?.Value;
                if (!string.IsNullOrEmpty(userEmail))
                {
                    userData = await dbContext.Students.FirstOrDefaultAsync(s => s.Email == userEmail);
                    if (userData != null)
                    {
                        // Load thesis applications
                        thesisApplications = await dbContext.ProfessorThesesApplied
                            .Include(a => a.StudentDetails)
                            .Include(a => a.ProfessorDetails)
                            .Where(j => j.StudentEmailAppliedForProfessorThesis == userEmail &&
                                       j.StudentUniqueIDAppliedForProfessorThesis == userData.Student_UniqueID)
                            .OrderByDescending(j => j.DateTimeStudentAppliedForProfessorThesis)
                            .ToListAsync();

                        companythesisApplications = await dbContext.CompanyThesesApplied
                            .Include(a => a.StudentDetails)
                            .Include(a => a.CompanyDetails)
                            .Where(j => j.StudentEmailAppliedForThesis == userEmail &&
                                       j.StudentUniqueIDAppliedForThesis == userData.Student_UniqueID)
                            .OrderByDescending(j => j.DateTimeStudentAppliedForThesis)
                            .ToListAsync();
                    }
                }
            }
        }

        private async Task LoadUserJobApplications()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var userEmail = user.FindFirst("name")?.Value;
                if (!string.IsNullOrEmpty(userEmail) && userData != null)
                {
                    jobApplications = await dbContext.CompanyJobsApplied
                        .Where(j => j.StudentEmailAppliedForCompanyJob == userEmail &&
                                   j.StudentUniqueIDAppliedForCompanyJob == userData.Student_UniqueID)
                        .OrderByDescending(j => j.DateTimeStudentAppliedForCompanyJob)
                        .ToListAsync();
                }
            }
        }

        private async Task LoadUserInternshipApplications()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var userEmail = user.FindFirst("name")?.Value;
                if (!string.IsNullOrEmpty(userEmail) && userData != null)
                {
                    // Load company internships
                    internshipApplications = await dbContext.InternshipsApplied
                        .Include(a => a.StudentDetails)
                        .Include(a => a.CompanyDetails)
                        .Where(a => a.StudentDetails.StudentUniqueIDAppliedForInternship == userData.Student_UniqueID)
                        .ToListAsync();

                    // Load professor internships
                    professorInternshipApplications = await dbContext.ProfessorInternshipsApplied
                        .Include(a => a.StudentDetails)
                        .Include(a => a.ProfessorDetails)
                        .Where(a => a.StudentDetails.StudentUniqueIDAppliedForProfessorInternship == userData.Student_UniqueID)
                        .ToListAsync();
                }
            }
        }

        private async Task LoadNewsAndAnnouncements()
        {
            newsArticles = await FetchNewsArticlesAsync();
            svseNewsArticles = await FetchSVSENewsArticlesAsync();
            Announcements = await FetchAnnouncementsAsync();
            ProfessorAnnouncements = await FetchProfessorAnnouncementsAsync();
        }

        private async Task LoadEventsForCalendar()
        {
            var companyEvents = await dbContext.CompanyEvents
                .Where(e => e.CompanyEventStatus == "Δημοσιευμένη")
                .ToListAsync();

            var professorEvents = await dbContext.ProfessorEvents
                .Where(e => e.ProfessorEventStatus == "Δημοσιευμένη")
                .ToListAsync();

            eventsForDate.Clear();
            eventsForDateForProfessors.Clear();

            foreach (var eventItem in companyEvents)
            {
                if (eventItem.CompanyEventActiveDate.Year == currentMonth.Year &&
                    eventItem.CompanyEventActiveDate.Month == currentMonth.Month)
                {
                    int day = eventItem.CompanyEventActiveDate.Day;
                    if (!eventsForDate.ContainsKey(day))
                        eventsForDate[day] = new List<CompanyEvent>();
                    eventsForDate[day].Add(eventItem);
                }
            }

            foreach (var eventItem in professorEvents)
            {
                if (eventItem.ProfessorEventActiveDate.Year == currentMonth.Year &&
                    eventItem.ProfessorEventActiveDate.Month == currentMonth.Month)
                {
                    int day = eventItem.ProfessorEventActiveDate.Day;
                    if (!eventsForDateForProfessors.ContainsKey(day))
                        eventsForDateForProfessors[day] = new List<ProfessorEvent>();
                    eventsForDateForProfessors[day].Add(eventItem);
                }
            }
        }

        private async Task LoadUserInterestsAndApplications()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated && userData != null)
            {
                var userEmail = user.FindFirst("name")?.Value;

                // Load interests and applications
                alreadyInterestedCompanyEventIds = (await dbContext.InterestInCompanyEvents
                    .Where(e => e.StudentUniqueIDShowInterestForEvent == userData.Student_UniqueID &&
                                e.StudentEmailShowInterestForEvent == userData.Email)
                    .Select(e => e.RNGForCompanyEventInterest)
                    .ToListAsync()).ToHashSet();

                interestedProfessorEventIds = (await dbContext.InterestInProfessorEvents
                    .Where(e => e.StudentUniqueIDShowInterestForEvent == userData.Student_UniqueID &&
                                e.StudentEmailShowInterestForEvent == userData.Email)
                    .Select(e => e.RNGForProfessorEventInterest)
                    .ToListAsync()).ToHashSet();

                professorThesisIdsApplied = dbContext.ProfessorThesesApplied
                    .Where(x => x.StudentUniqueIDAppliedForProfessorThesis == userData.Student_UniqueID &&
                                x.StudentEmailAppliedForProfessorThesis == userData.Email)
                    .Select(x => x.RNGForProfessorThesisApplied)
                    .ToHashSet();

                companyThesisIdsApplied = dbContext.CompanyThesesApplied
                    .Where(x => x.StudentUniqueIDAppliedForThesis == userData.Student_UniqueID &&
                                x.StudentEmailAppliedForThesis == userData.Email)
                    .Select(x => x.RNGForCompanyThesisApplied)
                    .ToHashSet();

                jobIdsApplied = dbContext.CompanyJobsApplied
                    .Where(x => x.StudentUniqueIDAppliedForCompanyJob == userData.Student_UniqueID &&
                                x.StudentEmailAppliedForCompanyJob == userData.Email)
                    .Select(x => x.RNGForCompanyJobApplied)
                    .ToHashSet();

                internshipIdsApplied = dbContext.InternshipsApplied
                    .Where(x => x.StudentDetails.StudentUniqueIDAppliedForInternship == userData.Student_UniqueID)
                    .Select(x => x.RNGForInternshipApplied)
                    .ToHashSet();

                professorInternshipIdsApplied = dbContext.ProfessorInternshipsApplied
                    .Where(x => x.StudentDetails.StudentUniqueIDAppliedForProfessorInternship == userData.Student_UniqueID)
                    .Select(x => x.RNGForProfessorInternshipApplied)
                    .ToHashSet();
            }
        }

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

        private void ToggleCompanyAnnouncementsVisibility()
        {
            isCompanyAnnouncementsVisible = !isCompanyAnnouncementsVisible;
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

        // Application visibility toggles
        private async Task ToggleAndLoadStudentThesisApplications()
        {
            showStudentThesisApplications = !showStudentThesisApplications;
            if (showStudentThesisApplications)
            {
                await LoadUserApplications();
            }
        }

        private async Task ToggleAndLoadStudentJobApplications()
        {
            showStudentJobApplications = !showStudentJobApplications;
            if (showStudentJobApplications)
            {
                await LoadUserJobApplications();
            }
        }

        private async Task ToggleAndLoadStudentInternshipApplications()
        {
            showStudentInternshipApplications = !showStudentInternshipApplications;
            if (showStudentInternshipApplications)
            {
                await LoadUserInternshipApplications();
            }
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
            LoadEventsForCalendar();
        }

        private void ShowNextMonth()
        {
            currentMonth = currentMonth.AddMonths(1);
            LoadEventsForCalendar();
        }

        private void OnDateClicked(DateTime clickedDate)
        {
            selectedDay = clickedDate.Day;
            highlightedDay = selectedDay;
            selectedDate = clickedDate;

            // Load events for selected date
            selectedDateEvents = dbContext.CompanyEvents
                .Where(e => e.CompanyEventStatus == "Δημοσιευμένη" &&
                           e.CompanyEventActiveDate.Date == clickedDate.Date)
                .ToList();

            selectedProfessorDateEvents = dbContext.ProfessorEvents
                .Where(e => e.ProfessorEventStatus == "Δημοσιευμένη" &&
                           e.ProfessorEventActiveDate.Date == clickedDate.Date)
                .ToList();

            if (selectedDateEvents.Any() || selectedProfessorDateEvents.Any())
            {
                isModalVisibleToShowEventsOnCalendarForEachClickedDay = true;
            }
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
            return await dbContext.AnnouncementsAsCompany
                .Where(a => a.CompanyAnnouncementStatus == "Δημοσιευμένη")
                .ToListAsync();
        }

        private async Task<List<AnnouncementAsProfessor>> FetchProfessorAnnouncementsAsync()
        {
            return await dbContext.AnnouncementsAsProfessor
                .Where(a => a.ProfessorAnnouncementStatus == "Δημοσιευμένη")
                .ToListAsync();
        }

        // News Article class
        public class NewsArticle
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public string Date { get; set; }
            public string Category { get; set; }
        }
    }
}
