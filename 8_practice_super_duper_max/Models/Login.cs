using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8_practice_super_duper_max.Models
{
    public class Login
    {
        [Key]
        public int login_id { get; set; }
        public string login_name { get; set; }
        public string password { get; set; }

        [Required]
        [ForeignKey("User")]
        public int user_id { get; set; }
        public User User { get; set; }
    }
}
