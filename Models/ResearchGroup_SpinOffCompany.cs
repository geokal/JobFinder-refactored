using Org.BouncyCastle.Crypto.Paddings;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;


namespace QuizManager.Models
{
    public class ResearchGroup_SpinOffCompany
    {
        [Key]
        public int Id { get; set; }
        //Research Group Information
        public string? ResearchGroupEmail { get; set; }
        public string? ResearchGroup_UniqueID { get; set; }

        //SpinOff Company Information
        public string? ResearchGroup_SpinOff_CompanyTitle { get; set; }
        public string? ResearchGroup_SpinOff_CompanyAFM { get; set; }
        public string? ResearchGroup_SpinOff_CompanyDescription { get; set; }
    }
}
