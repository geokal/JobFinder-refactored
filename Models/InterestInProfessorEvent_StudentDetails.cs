using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class InterestInProfessorEvent_StudentDetails
    {
        [Key]
        public int Id { get; set; }
        public string StudentUniqueIDShowInterestForProfessorEvent { get; set; }
        public string StudentEmailShowInterestForProfessorEvent { get; set; }
        public DateTime DateTimeStudentShowInterestForProfessorEvent { get; set; }
        public string RNGForProfessorEventShowInterestAsStudent_HashedAsUniqueID { get; set; }
        public InterestInProfessorEvent Application { get; set; }
    }
}
