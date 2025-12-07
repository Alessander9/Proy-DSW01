using System.ComponentModel.DataAnnotations;

namespace ProyectoSW01.Models
{

    //Promp cumple la funcion de placeholder en los formularios
    ///Display es la etiqueta que se muestra en los formularios
    ///Required indica que el campo es obligatorio
    ///StringLength limita la cantidad de caracteres que se pueden ingresar en el campo
    ///key indica que es la clave primaria de la tabla
    
    public class Carrito
    {
        [Key]
        public int CarritoId { get; set; }

        [Required(ErrorMessage = "El vehículo es obligatorio")]
        [Display(Name = "Vehículo", Prompt = "Seleccione el vehículo del carrito")]
        public int VehiculoId { get; set; }

        [Display(Name = "Creado en")]
        public DateTime CreadoEn { get; set; }
    }
}
