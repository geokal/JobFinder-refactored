using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class InterestInProfessorEvent
    {
        [Key]
        public int Id { get; set; }
        public string ProfessorEmailWhereStudentShowedInterest { get; set; }
        public string ProfessorUniqueIDWhereStudentShowedInterest { get; set; }
        public string StudentEmailShowInterestForEvent { get; set; }
        public string StudentUniqueIDShowInterestForEvent { get; set; }
        public long RNGForProfessorEventInterest { get; set; }
        public string RNGForProfessorEventInterest_HashedAsUniqueID { get; set; }
        public DateTime DateTimeStudentShowInterest { get; set; }
        public string? StudentTransportNeedWhenShowInterestForProfessorEvent { get; set; }
        public string? StudentTransportChosenLocationWhenShowInterestForProfessorEvent { get; set; }
        public string ProfessorEventStatusAtStudentSide { get; set; } 
        public string ProfessorEventStatusAtProfessorSide { get; set; }

        public InterestInProfessorEvent_StudentDetails StudentDetails { get; set; }
        public InterestInProfessorEvent_ProfessorDetails ProfessorDetails { get; set; }
    }
}
