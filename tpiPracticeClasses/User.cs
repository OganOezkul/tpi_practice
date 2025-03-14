using System.ComponentModel.DataAnnotations;

namespace tpiPracticeClasses
{
    public class User
    {
        [Key]
        public int userId { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string name { get; set; }
    }
}
