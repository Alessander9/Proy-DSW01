using System.ComponentModel.DataAnnotations;

namespace ProyectoSW01.Models
{


    //Promp cumple la funcion de placeholder en los formularios
    ///Display es la etiqueta que se muestra en los formularios
    ///Required indica que el campo es obligatorio
    ///StringLength limita la cantidad de caracteres que se pueden ingresar en el campo
    ///key indica que es la clave primaria de la tabla
    public class Notificacion
    {
        [Key]
        public int NotificacionId { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [Display(Name = "Usuario", Prompt = "Seleccione el usuario")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El mensaje es obligatorio")]
        [StringLength(2000, ErrorMessage = "El mensaje no debe exceder los 2000 caracteres")]
        [Display(Name = "Mensaje", Prompt = "Ingrese el contenido de la notificación")]
        public string Mensaje { get; set; }

        [Required(ErrorMessage = "El tipo de notificación es obligatorio")]
        [StringLength(20, ErrorMessage = "El tipo no debe exceder los 20 caracteres")]
        [Display(Name = "Tipo", Prompt = "Ej. info, alerta, recordatorio")]
        public string Tipo { get; set; }

        [Display(Name = "Leído")]
        public bool Leido { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }
    }

}
