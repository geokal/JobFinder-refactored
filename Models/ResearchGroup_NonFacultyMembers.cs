using System.ComponentModel.DataAnnotations;


namespace QuizManager.Models
{
    public class ResearchGroup_NonFacultyMembers //Students
    {
        [Key]
        public int Id { get; set; }
        public string? PK_ResearchGroupEmail { get; set; }
        public string? PK_NonFacultyMemberEmail { get; set; }
        public DateTime DateOfRegistrationOnResearchGroup_ForNonFacultyMember { get; set; }
        public string? PK_NonFacultyMemberLevelOfStudies { get; set; }
    }
}
