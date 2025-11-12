using System.ComponentModel.DataAnnotations;

namespace _8_practice_super_duper_max.Models
{
    public class DeliveryType
    {
        [Key]
        public int delivery_type_id { get; set; }
        public string delivery_type_name { get; set; }
    }
}
