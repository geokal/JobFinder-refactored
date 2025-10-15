using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class ProfessorInternshipsApplied_ProfessorDetails
    {
        [Key]
        public int Id { get; set; }
        public string ProfessorUniqueIDWhereStudentAppliedForProfessorInternship { get; set; }
        public string ProfessorEmailWhereStudentAppliedForProfessorInternship { get; set; }
        public ProfessorInternshipApplied Application { get; set; }
    }
}
