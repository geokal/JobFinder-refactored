using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
	public class InterestInCompanyEventAsProfessor_ProfessorDetails
	{
		[Key]
		public int Id { get; set; }
		public string ProfessorUniqueIDShowInterestForCompanyEvent { get; set; }
		public string ProfessorEmailShowInterestForCompanyEvent { get; set; }
		public DateTime DateTimeProfessorShowInterestForCompanyEvent { get; set; }
		public string RNGForCompanyEventShowInterestAsProfessor_HashedAsUniqueID { get; set; }
		public InterestInCompanyEventAsProfessor EventInterest { get; set; }
	}
}
