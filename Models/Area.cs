using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class Area
    {
        [Key]
        public int Id { get; set; }
        public string AreaName { get; set; }
        public string? AreaSubFields { get; set; } 
    }
}
