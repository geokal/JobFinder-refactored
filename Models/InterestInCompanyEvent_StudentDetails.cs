using QuizManager.Models;
using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class InterestInCompanyEvent_StudentDetails
    {
        [Key]
        public int Id { get; set; }
        public string StudentUniqueIDShowInterestForCompanyEvent { get; set; }
        public string StudentEmailShowInterestForCompanyEvent { get; set; }
        public DateTime DateTimeStudentShowInterestForCompanyEvent { get; set; }
        public string RNGForCompanyEventShowInterestAsStudent_HashedAsUniqueID { get; set; }
        public InterestInCompanyEvent Application { get; set; }
    }
}
