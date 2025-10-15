using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class ThesisApplication
    {
        [Key]
        public int Id { get; set; } // Primary key for the application
        public long RNGForThesisUploaded { get; set; } // Unique identifier for the uploaded thesis
        public DateTime DateTimeApplied { get; set; } // Date and time when the application was submitted
        public string StudentName { get; set; } // Name of the student applying
        public string StudentSurname { get; set; } // Surname of the student applying
        public long StudentRegNumber { get; set; } // Registration number of the student
        public byte[] StudentCV { get; set; } // CV of the student
        public byte[] StudentImage { get; set; } // Image of the student
        public string SupervisorName { get; set; } // Name of the professor supervising the thesis
        public string SupervisorSurname { get; set; }
        public string SupervisorEmail { get; set; } // Email of the professor supervising the thesis
        public string ThesisTitle { get; set; } // Title of the thesis
    }

}
