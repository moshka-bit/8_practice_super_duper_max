using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8_practice_super_duper_max.Models
{
    public class BasketItems
    {
        [Key]
        public int basket_items_id { get; set; }
        public int quantity { get; set; }

        [Required]
        [ForeignKey("Basket")]
        public int basket_id { get; set; }
        public Basket Basket { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int product_id { get; set; }
        public Product Product { get; set; }

    }
}
