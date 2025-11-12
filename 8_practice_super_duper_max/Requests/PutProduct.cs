namespace _8_practice_super_duper_max.Requests
{
    public class PutProduct
    {
        public string product_name { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public bool is_active { get; set; }
        public int stock { get; set; }
        public int category_id { get; set; }
    }
}
