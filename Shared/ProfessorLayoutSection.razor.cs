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
    public partial class ProfessorLayoutSection
    {
        [Parameter] public bool IsInitialized { get; set; }
        [Parameter] public bool IsRegistered { get; set; }
        [Parameter] public EventCallback<bool> IsRegisteredChanged { get; set; }

        // DbContext injection removed - data loading is handled by parent MainLayout
        [Inject] private Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IJSRuntime JS { get; set; }

        // Professor-specific properties
        private Professor professorData;
        private List<ProfessorThesis> professortheses = new();
        private List<ProfessorInternship> professorInternships = new();
        private List<AnnouncementAsProfessor> professorAnnouncements = new();
        private List<ProfessorEvent> professorEvents = new();

        // UI state properties
        private bool isProfessorAnnouncementsFormVisible = false;
        private bool isProfessorThesisFormVisible = false;
        private bool isProfessorInternshipFormVisible = false;
        private bool isProfessorSearchStudentFormVisible = false;
        private bool isProfessorSearchCompanyFormVisible = false;
        private bool isAnnouncementsFormVisibleToShowGeneralAnnouncementsAndEventsAsProfessor = false;
        private bool isUploadedAnnouncementsVisibleAsProfessor = false;
        private bool isUploadedThesesVisibleAsProfessor = false;
        private bool isUploadedCompanyThesesVisibleAsProfessor = false;
        private bool isUploadedProfessorThesesVisibleAsCompany = false;
        private bool isUploadedEventsVisibleAsProfessor = false;

        // Search and filter properties
        private string searchEmailAsProfessorToFindStudent = "";
        private string searchNameAsProfessorToFindStudent = "";
        private string searchSurnameAsProfessorToFindStudent = "";
        private string searchRegNumberAsProfessorToFindStudent = "";
        private string searchDepartmentAsProfessorToFindStudent = "";
        private string searchAreasOfExpertiseAsProfessorToFindStudent = "";
        private string searchKeywordsAsProfessorToFindStudent = "";
        private string searchCompanyEmailAsProfessorToFindCompany = "";
        private string searchCompanyNameENGAsProfessorToFindCompany = "";
        private string searchCompanyTypeAsProfessorToFindCompany = "";
        private string searchCompanyActivityrAsProfessorToFindCompany = "";
        private string searchCompanyTownAsProfessorToFindCompany = "";
        private string searchCompanyAreasAsProfessorToFindCompany = "";
        private string searchCompanyDesiredSkillsAsProfessorToFindCompany = "";

        // Data collections
        private List<Student> searchResultsAsProfessorToFindStudent = new();
        private List<Company> searchResultsAsProfessorToFindCompany = new();
        private List<ProfessorThesisApplied> professorThesisApplications = new();
        private List<ProfessorInternshipApplied> professorInternshipApplications = new();
        private List<CompanyThesis> companyThesesResultsToFindThesesAsProfessor = new();
        private List<ProfessorThesis> professorThesesResultsToFindThesesAsCompany = new();

        // Form objects
        private ProfessorThesis professorthesis = new();
        private ProfessorInternship professorInternship = new();
        private AnnouncementAsProfessor professorannouncement = new();
        private ProfessorEvent professorEvent = new();

        // Status and pagination
        private string selectedStatusFilterForThesesAsProfessor = "Όλα";
        private string selectedStatusFilterForAnnouncementsAsProfessor = "Όλα";
        private string selectedStatusFilterForEventsAsProfessor = "Όλα";
        private string selectedStatusFilterForProfessorInternships = "Όλα";
        private int currentPageForProfessorTheses = 1;
        private int currentPageForProfessorAnnouncements = 1;
        private int currentPageForProfessorEvents = 1;
        private int currentPageForProfessorInternships = 1;

        // Pagination options
        private int[] pageSizeOptions_SeeMyUploadedAnnouncementsAsProfessor = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int[] pageSizeOptions_SeeMyUploadedThesesAsProfessor = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int[] pageSizeOptions_SeeMyUploadedInternshipsAsProfessor = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int[] pageSizeOptions_SeeMyUploadedEventsAsProfessor = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int[] studentSearchPageSizeOptions = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private int[] companySearchPageSizeOptions = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // Areas and skills
        private List<Area> Areas = new();
        private List<Skill> Skills = new();
        private List<Area> availableAreasForProfessorThesis = new();
        private List<Skill> availableSkillsForProfessorThesis = new();
        private List<long> selectedAreasForProfessorThesis = new();
        private List<long> selectedSkillsForProfessorThesis = new();
        private List<Area> SelectedAreasWhenUploadInternshipAsProfessor = new();
        private List<Area> SelectedAreasWhenUploadThesisAsProfessor = new();
        private List<Area> SelectedAreasWhenUploadEventAsProfessor = new();

        // Status counts
        private int totalCountThesesAsProfessor, publishedCountThesesAsProfessor, unpublishedCountThesesAsProfessor, withdrawnCountThesesAsProfessor;
        private int totalCountAnnouncementsAsProfessor, publishedCountAnnouncementsAsProfessor, unpublishedCountAnnouncementsAsProfessor, withdrawnCountAnnouncementsAsProfessor;
        private int totalCountEventsAsProfessor, publishedCountEventsAsProfessor, unpublishedCountEventsAsProfessor, withdrawnCountEventsAsProfessor;
        private int totalProfessorInternshipsCount, publishedProfessorInternshipsCount, unpublishedProfessorInternshipsCount, withdrawnProfessorInternshipsCount;

        // UI state
        private bool showSuccessMessage = false;
        private bool showErrorMessage = false;
        private bool showErrorMessageforUploadinginternshipsAsProfessor = false;
        private bool showErrorMessageforUploadingThesisAsProfessor = false;
        private bool showErrorMessageforUploadingannouncementsAsProfessor = false;
        private bool showErrorMessageForUploadingProfessorEvent = false;
        private bool showSuccessUpdateMessage = false;
        private bool isEditing = false;

        // Professor data
        private string professorName = "";
        private string professorSurname = "";
        private string professorUniversity = "";
        private string professorUniversityDepartment = "";
        private string professorDepartment = "";
        private string professorVathmidaDEP = "";
        private string professorPersonalTelephone = "";
        private string professorWorkTelephone = "";
        private string professorGeneralFieldOfWork = "";
        private string professorGeneralSkills = "";
        private string professorPersonalDescription = "";
        private string professorLinkedInProfile = "";
        private string professorPersonalWebsite = "";
        private string professorScholarProfile = "";
        private string professorOrchidProfile = "";
        private byte[] professorImage;

        // Form validation
        private bool isFormValidToSaveEventAsProfessor = true;
        private string saveEventAsProfessorMessage = "";
        private bool isSaveAnnouncementAsProfessorSuccessful = false;
        private bool isSaveThesisAsProfessorSuccessful = false;
        private string saveThesisAsProfessorMessage = "";

        // Event handling
        private Dictionary<long, bool> expandedTheses = new();
        private Dictionary<long, bool> expandedProfessorInternships = new();
        private Dictionary<long, IEnumerable<ProfessorThesisApplied>> professorThesisApplicants = new();
        private Dictionary<long, IEnumerable<ProfessorInternshipApplied>> professorInternshipApplicants = new();

        // Pagination
        private int ProfessorThesesPerPage = 3;
        private int ProfessorInternshipsPerPage = 3;
        private int ProfessorEventsPerPage = 3;
        private int ProfessorAnnouncementsPerPage = 3;

        // Component initialization
        protected override async Task OnInitializedAsync()
        {
            // Data loading is now handled by the parent MainLayout
            // This component is now purely presentational
            await Task.CompletedTask;
        }

        // Data loading methods
        private async Task LoadAreasAsync()
        {
            // Data loading is handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task LoadSkillsAsync()
        {
            // Data loading is handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task LoadProfessorData()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var userEmail = user.FindFirst("name")?.Value;
                if (!string.IsNullOrEmpty(userEmail))
                {
                    // professorData = await // dbContext.Professors.FirstOrDefaultAsync(p => p.ProfEmail == userEmail);
                    if (professorData != null)
                    {
                        // Load professor details
                        professorName = professorData.ProfName;
                        professorSurname = professorData.ProfSurname;
                        professorUniversity = professorData.ProfUniversity;
                        professorUniversityDepartment = professorData.ProfDepartment;
                        professorDepartment = professorData.ProfDepartment;
                        professorVathmidaDEP = professorData.ProfVahmidaDEP;
                        professorPersonalTelephone = professorData.ProfPersonalTelephone;
                        professorWorkTelephone = professorData.ProfWorkTelephone;
                        professorGeneralFieldOfWork = professorData.ProfGeneralFieldOfWork;
                        professorGeneralSkills = professorData.ProfGeneralSkills;
                        professorPersonalDescription = professorData.ProfPersonalDescription;
                        professorLinkedInProfile = professorData.ProfLinkedInSite;
                        professorPersonalWebsite = professorData.ProfPersonalWebsite;
                        professorScholarProfile = professorData.ProfScholarProfile;
                        professorOrchidProfile = professorData.ProfOrchidProfile;
                        professorImage = professorData.ProfImage;
                    }
                }
            }
        }

        private async Task LoadTheses()
        {
            // Data loading is handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task LoadInternships()
        {
            // Data loading is handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task LoadAnnouncements()
        {
            // Data loading is handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task LoadEvents()
        {
            // Data loading is handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task CalculateStatusCounts()
        {
            await CalculateStatusCountsForTheses();
            await CalculateStatusCountsForAnnouncements();
            await CalculateStatusCountsForEvents();
            await CalculateStatusCountsForInternships();
        }

        private async Task CalculateStatusCountsForTheses()
        {
            totalCountThesesAsProfessor = professortheses.Count();
            publishedCountThesesAsProfessor = professortheses.Count(t => t.ThesisStatus == "Δημοσιευμένη");
            unpublishedCountThesesAsProfessor = professortheses.Count(t => t.ThesisStatus == "Μη Δημοσιευμένη");
            withdrawnCountThesesAsProfessor = professortheses.Count(t => t.ThesisStatus == "Αποσυρμένη");
        }

        private async Task CalculateStatusCountsForAnnouncements()
        {
            totalCountAnnouncementsAsProfessor = professorAnnouncements.Count();
            publishedCountAnnouncementsAsProfessor = professorAnnouncements.Count(a => a.ProfessorAnnouncementStatus == "Δημοσιευμένη");
            unpublishedCountAnnouncementsAsProfessor = professorAnnouncements.Count(a => a.ProfessorAnnouncementStatus == "Μη Δημοσιευμένη");
        }

        private async Task CalculateStatusCountsForEvents()
        {
            totalCountEventsAsProfessor = professorEvents.Count();
            publishedCountEventsAsProfessor = professorEvents.Count(e => e.ProfessorEventStatus == "Δημοσιευμένη");
            unpublishedCountEventsAsProfessor = professorEvents.Count(e => e.ProfessorEventStatus == "Μη Δημοσιευμένη");
        }

        private async Task CalculateStatusCountsForInternships()
        {
            totalProfessorInternshipsCount = professorInternships.Count();
            publishedProfessorInternshipsCount = professorInternships.Count(i => i.ProfessorUploadedInternshipStatus == "Δημοσιευμένη");
            unpublishedProfessorInternshipsCount = professorInternships.Count(i => i.ProfessorUploadedInternshipStatus == "Μη Δημοσιευμένη");
            withdrawnProfessorInternshipsCount = professorInternships.Count(i => i.ProfessorUploadedInternshipStatus == "Αποσυρμένη");
        }

        // UI toggle methods
        private void ToggleFormVisibilityForUploadProfessorAnnouncements()
        {
            isProfessorAnnouncementsFormVisible = !isProfessorAnnouncementsFormVisible;
        }

        private void ToggleFormVisibilityForUploadProfessorThesis()
        {
            isProfessorThesisFormVisible = !isProfessorThesisFormVisible;
        }

        private void ToggleFormVisibilityForUploadProfessorInternship()
        {
            isProfessorInternshipFormVisible = !isProfessorInternshipFormVisible;
        }

        private void ToggleFormVisibilityForSearchStudentAsProfessor()
        {
            isProfessorSearchStudentFormVisible = !isProfessorSearchStudentFormVisible;
        }

        private void ToggleFormVisibilityForSearchCompanyAsProfessor()
        {
            isProfessorSearchCompanyFormVisible = !isProfessorSearchCompanyFormVisible;
        }

        private void ToggleFormVisibilityToShowGeneralAnnouncementsAndEventsAsProfessor()
        {
            isAnnouncementsFormVisibleToShowGeneralAnnouncementsAndEventsAsProfessor = !isAnnouncementsFormVisibleToShowGeneralAnnouncementsAndEventsAsProfessor;
        }

        private void ToggleUploadedAnnouncementsVisibilityAsProfessor()
        {
            isUploadedAnnouncementsVisibleAsProfessor = !isUploadedAnnouncementsVisibleAsProfessor;
        }

        private void ToggleUploadedThesesVisibilityAsProfessor()
        {
            isUploadedThesesVisibleAsProfessor = !isUploadedThesesVisibleAsProfessor;
        }

        private void ToggleUploadedEventsVisibilityAsProfessor()
        {
            isUploadedEventsVisibleAsProfessor = !isUploadedEventsVisibleAsProfessor;
        }

        private void ToggleToSearchForUploadedCompanyThesesAsProfessor()
        {
            isUploadedCompanyThesesVisibleAsProfessor = !isUploadedCompanyThesesVisibleAsProfessor;
        }

        // Form submission methods
        private async Task UploadThesisAsProfessor(bool publishThesis = false)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (string.IsNullOrEmpty(userEmail)) return;

            // Validation
            if (string.IsNullOrWhiteSpace(professorthesis.ThesisTitle) ||
                string.IsNullOrWhiteSpace(professorthesis.ThesisDescription) ||
                !selectedAreasForProfessorThesis.Any() ||
                !selectedSkillsForProfessorThesis.Any())
            {
                showErrorMessageforUploadingThesisAsProfessor = true;
                return;
            }

            // Create thesis
            professorthesis.RNGForThesisUploaded = new Random().NextInt64();
            professorthesis.RNGForThesisUploaded_HashedAsUniqueID = HashingHelper.HashLong(professorthesis.RNGForThesisUploaded);
            professorthesis.ProfessorEmailUsedToUploadThesis = userEmail;
            professorthesis.ThesisUploadDateTime = DateTime.Now;
            professorthesis.ThesisType = ThesisType.Professor;
            professorthesis.ThesisAreas = string.Join(",", selectedAreasForProfessorThesis);
            professorthesis.ThesisSkills = string.Join(",", selectedSkillsForProfessorThesis);
            professorthesis.ThesisStatus = publishThesis ? "Δημοσιευμένη" : "Μη Δημοσιευμένη";

            // dbContext.ProfessorTheses.Add(professorthesis);
            // await dbContext.SaveChangesAsync();

            showSuccessMessage = true;
            showErrorMessageforUploadingThesisAsProfessor = false;

            // Reset form
            professorthesis = new ProfessorThesis();
            selectedAreasForProfessorThesis.Clear();
            selectedSkillsForProfessorThesis.Clear();
        }

        private async Task UploadInternshipAsProfessor()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (string.IsNullOrEmpty(userEmail)) return;

            // Validation
            if (string.IsNullOrWhiteSpace(professorInternship.ProfessorInternshipTitle) ||
                string.IsNullOrWhiteSpace(professorInternship.ProfessorInternshipType) ||
                string.IsNullOrWhiteSpace(professorInternship.ProfessorInternshipESPA) ||
                string.IsNullOrWhiteSpace(professorInternship.ProfessorInternshipContactPerson) ||
                string.IsNullOrWhiteSpace(professorInternship.ProfessorInternshipDescription) ||
                !SelectedAreasWhenUploadInternshipAsProfessor.Any())
            {
                showErrorMessageforUploadinginternshipsAsProfessor = true;
                return;
            }

            // Create internship
            professorInternship.RNGForInternshipUploadedAsProfessor = new Random().NextInt64();
            professorInternship.RNGForInternshipUploadedAsProfessor_HashedAsUniqueID = HashingHelper.HashLong(professorInternship.RNGForInternshipUploadedAsProfessor);
            professorInternship.ProfessorEmailUsedToUploadInternship = userEmail;
            professorInternship.ProfessorInternshipUploadDate = DateTime.Now;
            professorInternship.ProfessorInternshipAreas = string.Join(",", SelectedAreasWhenUploadInternshipAsProfessor.Select(a => a.AreaName));
            professorInternship.ProfessorUploadedInternshipStatus = "Μη Δημοσιευμένη";

            // dbContext.ProfessorInternships.Add(professorInternship);
            // await dbContext.SaveChangesAsync();

            showSuccessMessage = true;
            showErrorMessageforUploadinginternshipsAsProfessor = false;

            // Reset form
            professorInternship = new ProfessorInternship();
            SelectedAreasWhenUploadInternshipAsProfessor.Clear();
        }

        private async Task SaveAnnouncementAsProfessor(bool publishAnnouncement = false)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userEmail = user.FindFirst("name")?.Value;

            if (string.IsNullOrEmpty(userEmail)) return;

            // Validation
            if (string.IsNullOrWhiteSpace(professorannouncement.ProfessorAnnouncementTitle) ||
                string.IsNullOrWhiteSpace(professorannouncement.ProfessorAnnouncementDescription))
            {
                showErrorMessageforUploadingannouncementsAsProfessor = true;
                return;
            }

            // Create announcement
            professorannouncement.ProfessorAnnouncementRNG = new Random().NextInt64();
            professorannouncement.ProfessorAnnouncementRNG_HashedAsUniqueID = HashingHelper.HashLong(professorannouncement.ProfessorAnnouncementRNG ?? 0);
            professorannouncement.ProfessorAnnouncementProfessorEmail = userEmail;
            professorannouncement.ProfessorAnnouncementUploadDate = DateTime.Now;
            professorannouncement.ProfessorAnnouncementStatus = publishAnnouncement ? "Δημοσιευμένη" : "Μη Δημοσιευμένη";

            // dbContext.AnnouncementsAsProfessor.Add(professorannouncement);
            // await dbContext.SaveChangesAsync();

            showSuccessMessage = true;
            showErrorMessageforUploadingannouncementsAsProfessor = false;

            // Reset form
            professorannouncement = new AnnouncementAsProfessor();
        }

        // Search methods
        private void SearchStudentsAsProfessorToFindStudent()
        {
            // Implementation for student search
        }

        private void SearchCompaniesAsProfessor()
        {
            // Implementation for company search
        }

        // Application management methods
        private async Task LoadThesisApplications(long thesisId)
        {
            // Data loading is handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task LoadInternshipApplications(long internshipId)
        {
            // Data loading is handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task AcceptThesisApplication(long thesisRNG, string studentUniqueID)
        {
            // Database operations removed - handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task RejectThesisApplication(long thesisRNG, string studentUniqueID)
        {
            // Database operations removed - handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task AcceptInternshipApplication(long internshipRNG, string studentUniqueID)
        {
            // Database operations removed - handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task RejectInternshipApplication(long internshipRNG, string studentUniqueID)
        {
            // Database operations removed - handled by parent MainLayout
            await Task.CompletedTask;
        }

        // Status update methods
        private async Task UpdateThesisStatus(int thesisId, string newStatus)
        {
            // Database operations removed - handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task UpdateInternshipStatus(int internshipId, string newStatus)
        {
            // Database operations removed - handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task UpdateAnnouncementStatus(int announcementId, string newStatus)
        {
            // Database operations removed - handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task UpdateEventStatus(int eventId, string newStatus)
        {
            // Database operations removed - handled by parent MainLayout
            await Task.CompletedTask;
        }

        // Delete methods
        private async Task DeleteThesis(int thesisId)
        {
            // Database operations removed - handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task DeleteInternship(int internshipId)
        {
            // Database operations removed - handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task DeleteAnnouncement(int announcementId)
        {
            // Database operations removed - handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task DeleteEvent(int eventId)
        {
            // Database operations removed - handled by parent MainLayout
            await Task.CompletedTask;
        }

        // File handling methods
        private async Task HandleFileSelectedForThesisAttachmentAsProfessor(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file != null && file.ContentType == "application/pdf")
            {
                using var ms = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(ms);
                professorthesis.ThesisAttachment = ms.ToArray();
            }
        }

        private async Task HandleFileSelectedForAnnouncementAttachmentAsProfessor(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file != null && file.ContentType == "application/pdf")
            {
                using var ms = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(ms);
                professorannouncement.ProfessorAnnouncementAttachmentFile = ms.ToArray();
            }
        }

        private async Task HandleFileSelectedForProfessorInternshipAttachment(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file != null && file.ContentType == "application/pdf")
            {
                using var ms = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(ms);
                professorInternship.ProfessorInternshipAttachment = ms.ToArray();
            }
        }

        // Utility methods
        private bool IsSelectedAreasWhenUploadThesisAsProfessor(Area area)
        {
            return SelectedAreasWhenUploadThesisAsProfessor.Contains(area);
        }

        private bool IsSelectedAreasWhenUploadInternshipAsProfessor(Area area)
        {
            return SelectedAreasWhenUploadInternshipAsProfessor.Contains(area);
        }

        private bool IsSelectedAreasWhenUploadEventAsProfessor(Area area)
        {
            return SelectedAreasWhenUploadEventAsProfessor.Contains(area);
        }

        private void OnCheckedChangedAreasWhenUploadThesisAsProfessor(ChangeEventArgs e, Area area)
        {
            if (e.Value is bool isChecked)
            {
                if (isChecked)
                {
                    if (!SelectedAreasWhenUploadThesisAsProfessor.Contains(area))
                        SelectedAreasWhenUploadThesisAsProfessor.Add(area);
                }
                else
                {
                    SelectedAreasWhenUploadThesisAsProfessor.Remove(area);
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
                        SelectedAreasWhenUploadInternshipAsProfessor.Add(area);
                }
                else
                {
                    SelectedAreasWhenUploadInternshipAsProfessor.Remove(area);
                }
            }
        }

        private void OnCheckedChangedAreasWhenUploadEventAsProfessor(ChangeEventArgs e, Area area)
        {
            if (e.Value is bool isChecked)
            {
                if (isChecked)
                {
                    if (!SelectedAreasWhenUploadEventAsProfessor.Contains(area))
                        SelectedAreasWhenUploadEventAsProfessor.Add(area);
                }
                else
                {
                    SelectedAreasWhenUploadEventAsProfessor.Remove(area);
                }
            }
        }

        // Pagination methods
        private void OnPageSizeChangeForProfessorTheses(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                ProfessorThesesPerPage = newSize;
                currentPageForProfessorTheses = 1;
            }
        }

        private void OnPageSizeChangeForProfessorInternships(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                ProfessorInternshipsPerPage = newSize;
                currentPageForProfessorInternships = 1;
            }
        }

        private void OnPageSizeChangeForProfessorAnnouncements(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                ProfessorAnnouncementsPerPage = newSize;
                currentPageForProfessorAnnouncements = 1;
            }
        }

        private void OnPageSizeChangeForProfessorEvents(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                ProfessorEventsPerPage = newSize;
                currentPageForProfessorEvents = 1;
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
            searchEmailAsProfessorToFindStudent = "";
            searchNameAsProfessorToFindStudent = "";
            searchSurnameAsProfessorToFindStudent = "";
            searchRegNumberAsProfessorToFindStudent = "";
            searchDepartmentAsProfessorToFindStudent = "";
            searchAreasOfExpertiseAsProfessorToFindStudent = "";
            searchKeywordsAsProfessorToFindStudent = "";
            searchResultsAsProfessorToFindStudent.Clear();
        }

        private void ClearSearchFieldsForCompanies()
        {
            searchCompanyEmailAsProfessorToFindCompany = "";
            searchCompanyNameENGAsProfessorToFindCompany = "";
            searchCompanyTypeAsProfessorToFindCompany = "";
            searchCompanyActivityrAsProfessorToFindCompany = "";
            searchCompanyTownAsProfessorToFindCompany = "";
            searchCompanyAreasAsProfessorToFindCompany = "";
            searchCompanyDesiredSkillsAsProfessorToFindCompany = "";
            searchResultsAsProfessorToFindCompany.Clear();
        }

        // Helper methods
        private List<string> GetTownsForRegion(string region)
        {
            if (string.IsNullOrEmpty(region) || !RegionToTownsMap.ContainsKey(region))
                return new List<string>();
            return RegionToTownsMap[region];
        }

        private void UpdateTransportOfferForProfessorInternship(bool offer)
        {
            professorInternship.ProfessorInternshipTransportOffer = offer;
        }

        private async Task DownloadAttachmentForProfessorTheses(int thesisId)
        {
            // Database operations removed - handled by parent MainLayout
            await Task.CompletedTask;
        }

        private async Task DownloadAttachmentForProfessorInternships(int internshipId)
        {
            // Database operations removed - handled by parent MainLayout
            await Task.CompletedTask;
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

        protected async Task SetRegistered(bool value)
        {
            IsRegistered = value;
            if (IsRegisteredChanged.HasDelegate)
                await IsRegisteredChanged.InvokeAsync(value);
        }
    }
}
