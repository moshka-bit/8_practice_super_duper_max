using _8_practice_super_duper_max.Models;

namespace _8_practice_super_duper_max.Requests
{
    public class PostNewEmployee
    {
        public string user_fullname { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string phonenumber { get; set; }
        public string login_name { get; set; }
        public string password { get; set; }
    }
}
