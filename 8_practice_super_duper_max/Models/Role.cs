using System.ComponentModel.DataAnnotations;

namespace _8_practice_super_duper_max.Models
{
    public class Role
    {
        [Key]
        public int role_id {  get; set; }
        public string role_name { get; set; }

    }
}
