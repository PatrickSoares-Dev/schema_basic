using swagger_basic.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace swagger_basic.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O campo Nome deve ter entre 3 e 50 caracteres.")]
        [RegularExpression("^[a-zA-ZÀ-ÿ ]+$", ErrorMessage = "O campo Nome deve conter apenas letras e espaços.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo Email deve ser um endereço de e-mail válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string Senha { get; set; }

        [JsonIgnore]
        public DateTime Data_Registro { get; set; }

        [JsonIgnore]
        public string Tipo_User { get; set; } = "Usuario";

        [JsonIgnore]
        public List<AuthToken> AuthTokens { get; set; } = new List<AuthToken>();

        public void SetSenhaHash()
        {
            Senha = Senha.GerarHash();
        }
    }

}

