using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace swagger_basic.Models.Users
{
    public class UserResponseModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime Data_Registro { get; set; }
        public string Tipo_User { get; set; }
    }
}
