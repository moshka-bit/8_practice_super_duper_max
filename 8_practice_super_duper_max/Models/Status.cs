using System.ComponentModel.DataAnnotations;

namespace _8_practice_super_duper_max.Models
{
    public class Status
    {
        [Key]
        public int status_id {  get; set; }
        public string status_name { get; set; }
    }
}
