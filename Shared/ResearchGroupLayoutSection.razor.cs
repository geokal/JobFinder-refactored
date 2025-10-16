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
            await LoadResearchGroupData();
            await LoadFacultyMembers();
            await LoadNonFacultyMembers();
            await LoadResearchActions();
            await LoadPatents();
            await LoadSpinOffCompanies();
            await LoadMemberPublications();
            await CalculateStatistics();
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

        private async Task CalculateStatistics()
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

        private void ToggleStatisticsVisibility()
        {
            isStatisticsVisible = !isStatisticsVisible;
            if (isStatisticsVisible)
            {
                CalculateStatistics();
            }
        }

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

        // Search methods
        private void SearchCompaniesAsRG()
        {
            // Implementation for company search
        }

        private void SearchProfessorsAsRGToFindProfessor()
        {
            // Implementation for professor search
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
        }

        private void ClearSearchFieldsAsRGToFindProfessor()
        {
            searchNameSurnameAsRGToFindProfessor = "";
            searchDepartmentAsRGToFindProfessor = "";
            searchAreasOfInterestAsRGToFindProfessor = "";
            searchResultsAsRGToFindProfessor.Clear();
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
                var mapUrl = $"https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(location)}";
                NavigationManager.NavigateTo(mapUrl, true);
            }
        }
    }
}
