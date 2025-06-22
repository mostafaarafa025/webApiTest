using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wabApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
        public int Age {  get; set; }

        public string? RefreshToken {  get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

    }
}
