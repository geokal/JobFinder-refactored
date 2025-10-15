using Org.BouncyCastle.Crypto.Paddings;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;



namespace QuizManager.Models
{
    public class ResearchGroup
    {
        [Key]
        public int Id { get; set; }
        public string? ResearchGroupEmail { get; set; }
        public string ResearchGroup_UniqueID { get; set; }
        public byte[]? ResearchGroupImage { get; set; }


        //Research Group Info
        public string? ResearchGroupName { get; set; }
        public string? ResearchGroupAcronym { get; set; } //new
        public byte[]? ResearchGroupTeamImage { get; set; } //new
        public string? ResearchGroupUniversity { get; set; } 
        public string? ResearchGroupSchool { get; set; } 
        public string? ResearchGroupUniversityDepartment { get; set; } 
        public string? ResearchGroupLab { get; set; }
        public string? ResearchGroupFEK { get; set; }
        public string? ResearchGroupContactEmail { get; set; }
        public string? ResearchGroupPostalAddress { get; set; }
        public string? ResearchGroupTelephoneNumber { get; set; }
        public byte[]? ResearchGroup_PresentationAttachment { get; set; } //new

        public string? ResearchGroupAreas { get; set; } 
        public string? ResearchGroupSkills { get; set; }
        public string? ResearchGroupKeywords { get; set; } //added 10/09/2025

        public DateTime ResearchGroup_DateOfCreation { get; set; } //added 09/09/2025


        //Research Group Social Details
        public string? ResearchGroup_Website { get; set; } //new
        public string? ResearchGroup_Facebook { get; set; } //new
        public string? ResearchGroup_Twitter { get; set; } //new
        public string? ResearchGroup_LinkedIn { get; set; } //new
        public string? ResearchGroup_YouTubeChannel { get; set; } //new
        public string? ResearchGroup_EmbeddedPromoVideo { get; set; } //new

    }
}
