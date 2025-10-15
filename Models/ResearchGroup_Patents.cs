using Org.BouncyCastle.Crypto.Paddings;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;


namespace QuizManager.Models
{
    public class ResearchGroup_Patents
    {

        [Key]
        public int Id { get; set; }
        //Research Group Information
        public string? ResearchGroupEmail { get; set; }
        public string? ResearchGroup_UniqueID { get; set; }

        //SpinOff Company Information
        public string? ResearchGroup_Patent_PatentURL { get; set; }
        public string? ResearchGroup_Patent_PatentTitle { get; set; }
        public string? ResearchGroup_Patent_PatentDescription { get; set; }
        public string? ResearchGroup_Patent_PatentDOI { get; set; }
        public string? ResearchGroup_Patent_PatentType { get; set; } 
        public string? ResearchGroup_Patent_PatentStatus { get; set; } 

    }
}
