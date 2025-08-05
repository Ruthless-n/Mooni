using System.ComponentModel.DataAnnotations.Schema;
namespace backend.Models
{
    public class LoginRequest
    {
        [Column("email")]
        public required string Email { get; set; }

        [Column("password")]
        public required string Password { get; set; }
    }
}
