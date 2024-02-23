using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace swagger_basic.Models.Users
{
    public class UserUpdateModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        [JsonIgnore]
        public string Tipo_User { get; set; } = "Usuario";
    }
}
