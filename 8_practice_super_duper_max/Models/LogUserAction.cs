using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8_practice_super_duper_max.Models
{
    public class LogUserAction
    {
        [Key]
        public int log_user_action_id {  get; set; }
        public DateTime created_at { get; set; }

        [Required]
        [ForeignKey("User")]
        public int user_id { get; set; }
        public User User { get; set; }

        [Required]
        [ForeignKey("ActionType")]
        public int action_type_id { get; set; }
        public ActionType ActionType { get; set; }
    }
}
