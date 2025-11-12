using System.ComponentModel.DataAnnotations;

namespace _8_practice_super_duper_max.Models
{
    public class Category
    {
        [Key]
        public int category_id { get; set; }
        public string category_name { get; set; }
    }
}
