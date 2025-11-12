using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _8_practice_super_duper_max.Models
{
    public class Order
    {
        [Key]
        public int order_id { get; set; }
        public DateOnly order_date { get; set; }
        public double total_amount { get; set; }
        public string delivery_adress { get; set; }

        [Required]
        [ForeignKey("Status")]
        public int status_id { get; set; }
        public Status Status { get; set; }

        [Required]
        [ForeignKey("User")]
        public int user_id { get; set; }
        public User User { get; set; }

        [Required]
        [ForeignKey("DeliveryType")]
        public int delivery_type_id { get; set; }
        public DeliveryType DeliveryType { get; set; }

        [Required]
        [ForeignKey("PaymentType")]
        public int payment_type_id { get; set; }
        public PaymentType PaymentType { get; set; }

    }
}
