
using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectoSW01.Models
{


    //Promp cumple la funcion de placeholder en los formularios
    ///Display es la etiqueta que se muestra en los formularios
    ///Required indica que el campo es obligatorio
    ///StringLength limita la cantidad de caracteres que se pueden ingresar en el campo
    ///key indica que es la clave primaria de la tabla
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre completo no debe exceder los 100 caracteres")]
        [Display(Name = "Nombre completo", Prompt = "Ingrese el nombre y apellido")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [StringLength(100, ErrorMessage = "El correo no debe exceder los 100 caracteres")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido")]
        [Display(Name = "Correo electrónico", Prompt = "ejemplo@correo.com")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(255, ErrorMessage = "La contraseña no debe exceder los 255 caracteres")]
        [Display(Name = "Contraseña", Prompt = "Ingrese su contraseña")]
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "El rol del usuario es obligatorio")]
        [Display(Name = "Rol", Prompt = "Seleccione un rol")]
        public int IdRol { get; set; }
    }
}
