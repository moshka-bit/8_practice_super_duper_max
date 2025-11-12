using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8_practice_super_duper_max.Models
{
    public class Session
    {
        [Key]
        public int session_id { get; set; }
        public string token { get; set; }

        [Required]
        [ForeignKey("User")]
        public int user_id { get; set; }
        public User User { get; set; }
    }
}
