using System.ComponentModel.DataAnnotations;


namespace QuizManager.Models
{
    public class ResearchGroup_Professors //Faculty Members
    {
        [Key]
        public int Id { get; set; }
        public string? PK_ResearchGroupEmail { get; set; }
        public string? PK_ProfessorEmail { get; set; }
        public DateTime DateOfRegistrationOnResearchGroup_ForProfessorMember { get; set; }
        public string? PK_ProfessorRole { get; set; }
    }
}
