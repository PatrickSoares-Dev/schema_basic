using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace swagger_basic.Entities
{
    [Table("auth_tokens")]
    public class AuthToken
    {
        [Key]
        public int Id { get; set; }
        public int User_Id { get; set; }
        public User User { get; set; } 
        public string Token { get; set; }
        public DateTime Expiration_Date { get; set; }
        public DateTime Generation_Date { get; set; }

        public AuthToken()
        {
            Generation_Date = DateTime.UtcNow;
        }
    }
}
