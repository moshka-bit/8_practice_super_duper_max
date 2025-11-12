using System.ComponentModel.DataAnnotations;

namespace _8_practice_super_duper_max.Models
{
    public class PaymentType
    {
        [Key]
        public int payment_type_id { get; set; }
        public string payment_type_name { get; set; }
    }
}
