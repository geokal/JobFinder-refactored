using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
	public class InterestInProfessorEventAsCompany_ProfessorDetails
	{
		[Key]
		public int Id { get; set; }
		public string ProfessorUniqueIDWhereCompanyShowInterestForProfessorEvent { get; set; }
		public string ProfessorEmailWhereCompanyShowInterestForProfessorEvent { get; set; }
		public InterestInProfessorEventAsCompany EventInterest { get; set; }
	}
}
