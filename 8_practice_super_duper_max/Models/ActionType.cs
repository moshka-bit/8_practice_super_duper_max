using System.ComponentModel.DataAnnotations;

namespace _8_practice_super_duper_max.Models
{
    public class ActionType
    {
        [Key]
        public int action_type_id { get; set; }
        public string action_name { get; set; }
        public string action_description { get; set; }
    }
}
