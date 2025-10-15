using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }  
        public string SkillName { get; set; } 
    }
}
