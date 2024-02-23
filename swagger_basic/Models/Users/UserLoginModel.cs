using swagger_basic.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace swagger_basic.Models.Users
{
    public class UserLoginModel
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo Email deve ser um endereço de e-mail válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string Senha { get; set; }
        public void SetSenhaHash()
        {
            Senha = Senha.GerarHash();
        }

    }
}
