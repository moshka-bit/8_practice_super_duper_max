using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8_practice_super_duper_max.Models
{
    public class Product
    {
        [Key]
        public int product_id { get; set; }
        public string product_name { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public DateOnly created_at { get; set; }
        public bool is_active { get; set; }
        public int stock { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int category_id { get; set; }
        public Category Category { get; set; }
    }
}
