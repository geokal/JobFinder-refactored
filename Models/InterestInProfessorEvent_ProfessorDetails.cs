using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class InterestInProfessorEvent_ProfessorDetails
    {
        [Key]
        public int Id { get; set; }
        public string ProfessorUniqueIDWhereStudentShowInterestForProfessorEvent { get; set; }
        public string ProfessorEmailWhereStudentShowInterestForProfessorEvent { get; set; }
        public InterestInProfessorEvent Application { get; set; }
    }
}
