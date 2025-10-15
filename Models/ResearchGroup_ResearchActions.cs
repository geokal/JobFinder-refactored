using Org.BouncyCastle.Crypto.Paddings;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;


namespace QuizManager.Models
{
    public class ResearchGroup_ResearchActions
    {
        [Key]
        public int Id { get; set; }
        //Research Action Status [OnGoing / Past] - Based on EndDate
        public string? ResearchGroup_ProjectStatus { get; set; }

        //Research Group Information
        public string? ResearchGroupEmail { get; set; }
        public string? ResearchGroup_UniqueID { get; set; }

        //Research Group Project Cordis Details
        public string? ResearchGroup_ProjectFramework { get; set; } // ayto prepei na to pairnei apo list opws ginetai me ta departments. Sto chat me Dionysi iparxei to link
        public string? ResearchGroup_ProjectTitle { get; set; } 
        public string? ResearchGroup_ProjectAcronym { get; set; }
        public string? ResearchGroup_ProjectGrantAgreementNumber { get; set; }
        public DateTime? ResearchGroup_ProjectStartDate { get; set; }
        public DateTime? ResearchGroup_ProjectEndDate { get; set; }
        public string? ResearchGroup_ProjectTotalCost { get; set; }
        public string? ResearchGroup_ProjectTotalEUContribution { get; set; }
        public string? ResearchGroup_ProjectCoordinator { get; set; }
        public string? ResearchGroup_ProjectKeywords { get; set; }
        public string? ResearchGroup_ProjectProgramme { get; set; }
        public string? ResearchGroup_ProjectTopic { get; set; }
        public string? ResearchGroup_ProjectDescription { get; set; }


        //Research Group Project Other Details (Manual registration)
        public string? ResearchGroup_ProjectELKECode { get; set; }
        public string? ResearchGroup_ProjectScientificResponsibleEmail { get; set; } // Taken from Research Group Professors - This will take the name from Professor Navigation Property
        public string? ResearchGroup_EuropaCordisWebsite { get; set; }
        public string? ResearchGroup_ProjectWebsite { get; set; }
        public string? ResearchGroup_OurProjectBudget { get; set; }    
    } 
}