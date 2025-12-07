using System.ComponentModel.DataAnnotations;

namespace ProyectoSW01.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        public string Contrasena { get; set; }
    }
}