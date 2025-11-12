using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8_practice_super_duper_max.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }
        public string user_fullname { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string phonenumber { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly UpdatedAt { get; set; }

        [Required]
        [ForeignKey("Role")]
        public int role_id { get; set; }
        public Role Role { get; set; }
    }
}
