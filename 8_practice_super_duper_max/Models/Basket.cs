using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8_practice_super_duper_max.Models
{
    public class Basket
    {
        [Key]
        public int basket_id { get; set; }
        public double result_price { get; set; }

        [Required]
        [ForeignKey("User")]
        public int user_id { get; set; }
        public User User { get; set; }

        [ForeignKey("Order")]
        public int? order_id { get; set; }
        public Order Order { get; set; }
    }
}
