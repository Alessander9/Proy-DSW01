using System;
using System.ComponentModel.DataAnnotations;


namespace ProyectoSW01.Models
{

    //Promp cumple la funcion de placeholder en los formularios
    ///Display es la etiqueta que se muestra en los formularios
    ///Required indica que el campo es obligatorio
    ///StringLength limita la cantidad de caracteres que se pueden ingresar en el campo
    ///key indica que es la clave primaria de la tabla
    public class Rol
    {
        [Key]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre del rol no debe exceder los 50 caracteres")]
        [Display(Name = "Nombre del rol", Prompt = "Ej. ADMIN, MECANICO, RECEPCION")]
        public string Nombre { get; set; }
    }
}
