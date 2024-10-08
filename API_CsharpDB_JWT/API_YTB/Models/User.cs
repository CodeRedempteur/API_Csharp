using System.ComponentModel.DataAnnotations;

namespace API_YTB.Models
{
    public class User
    {
        public int Id_User { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

