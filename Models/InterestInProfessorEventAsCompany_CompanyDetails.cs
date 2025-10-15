using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
	public class InterestInProfessorEventAsCompany_CompanyDetails
	{
		[Key]
		public int Id { get; set; }
		public string CompanyUniqueIDShowInterestForProfessorEvent { get; set; }
		public string CompanyEmailShowInterestForProfessorEvent { get; set; }
		public DateTime DateTimeCompanyShowInterestForProfessorEvent { get; set; }
		public string RNGForProfessorEventShowInterestAsCompany_HashedAsUniqueID { get; set; }
		public InterestInProfessorEventAsCompany EventInterest { get; set; }
	}
}
