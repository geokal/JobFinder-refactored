using Org.BouncyCastle.Crypto.Paddings;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace QuizManager.Models
{
    public class Student
    {

        //ΠΡΟΣΩΠΙΚΑ ΣΤΟΙΧΕΙΑ
        [Key]
        public int Id { get; set; }
        public string Student_UniqueID { get; set; }
        public string Email { get; set; }
        public byte[]? Image { get; set; }
        public string Name { get; set; }    
        public string Surname { get; set; }
        public string? NameOfResearchGroupHeIsMember { get; set; }
        public string Telephone { get; set; }
        public bool PhoneVisibility { get; set; }
        public string PermanentAddress { get; set; }
        public long PermanentPC { get; set; }
        public string PermanentRegion { get; set; } = string.Empty;
        public string PermanentTown { get; set; } = string.Empty;
        public bool HomeVisibility { get; set; }
        public byte[]? Attachment { get; set; }
        public string LinkedInProfile { get; set; }
        public string PersonalWebsite { get; set; }
        public string StudentGoogleScholarProfile { get; set; } //added 08/9/25
        public bool Transport { get; set; }

        //ΣΤΟΙΧΕΙΑ ΦΟΙΤΗΣΗΣ (EDW PREPEI NA KSANA GINEI MIGRATE/UPDATE)
        public long RegNumber { get; set; }
        public string University { get; set; }           //Standard timi "University Of Athens"
        public string School { get; set; } //added 09/09/2025
        public string WhenStudentIsPostDocOrPhD_ResearchTitle { get; set; } //added 09/09/2025
        public string WhenStudentIsPostDocOrPhD_ResearchDescription { get; set; } //added 09/09/2025

        public string Department { get; set; }              //TIMES APO TO REG NUMBER
        public string EnrollmentDate { get; set; }      //TIMES APO TO REG NUMBER
        public string StudyYear { get; set; }           //TIMES APO TO REG NUMBER
        public DateTime ExpectedGraduationDate { get; set; } 
        public int CompletedECTS { get; set; }
        public byte[]? Grades { get; set; }
        public string InternshipStatus { get; set; } = "Ενδιαφέρομαι για θέση Πρακτικής Άσκησης"; 
        public string ThesisStatus { get; set; } = "Ενδιαφέρομαι για ανάληψη Πτυχιακής Εργασίας"; 

        //ΑΥΤΑ ΜΑΛΛΟΝ ΘΑ ΦΥΓΟΥΝ ΔΕΝ ΧΡΕΙΑΖΟΝΤΑΙ
        public bool TechnicalSkills { get; set; }
		public bool Programming { get; set; }
		public bool MachineLearning { get; set; }
		public bool NetworksAndTelecom { get; set; }
		public bool Databases { get; set; }

        //ΕΝΔΙΑΦΕΡΟΝΤΑ & ΔΕΞΙΟΤΗΤΕΣ
        public string AreasOfExpertise { get; set; }
        public string SelfAssesmentAreas { get; set; }
        public string TargetAreas { get; set; }
        public string Keywords { get; set; } //Skills
        public string SelfAssesmentSkills { get; set; }
        public string TargetSkills { get; set; }
        public byte[]? CoverLetter { get; set; }
        public bool PreferredTownsBoolean { get; set; }
        public string PreferedRegions { get; set; }
        public string PreferredTowns { get; set; }


        //ΠΛΗΡΟΦΟΡΙΕΣ ΧΡΗΣΗΣ ΠΛΑΤΦΟΡΜΑΣ (ΑΥΤΑ ΕΙΝΑΙ ΟΛΑ GREY NON EDITABLES)
        //public DateTime SignUpDate { get; set; } //AYTA GINONTAI APO TO AUTH0
        //public DateTime LastLogin { get; set; } // SAME AS ABOVE
        public DateTime? LastProfileUpdate { get; set; }
        public DateTime? LastCVUpdate { get; set; }
        //public DateTime LastSkillsUpdate { get; set; }
        //public DateTime LastAreasUpdate { get; set; }

        public string? LevelOfDegree { get; set; }  
        public bool HasFinishedStudies { get; set; }


        internal static async Task<string> GetRealName(string studentName)
        {
            throw new NotImplementedException();
        }
    }
}
