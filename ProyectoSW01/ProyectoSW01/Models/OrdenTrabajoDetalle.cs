using System.ComponentModel.DataAnnotations;

namespace ProyectoSW01.Models
{


    //Promp cumple la funcion de placeholder en los formularios
    ///Display es la etiqueta que se muestra en los formularios
    ///Required indica que el campo es obligatorio
    ///StringLength limita la cantidad de caracteres que se pueden ingresar en el campo
    ///key indica que es la clave primaria de la tabla
    public class OrdenTrabajoDetalle
    {
        [Key]
        public int OrdDetId { get; set; }

        [Required(ErrorMessage = "La orden de trabajo es obligatoria")]
        [Display(Name = "Orden de trabajo", Prompt = "Seleccione la orden de trabajo")]
        public int OrdenId { get; set; }

        [Required(ErrorMessage = "El servicio es obligatorio")]
        [Display(Name = "Servicio", Prompt = "Seleccione el servicio")]
        public int ServicioId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Display(Name = "Cantidad", Prompt = "Ingrese la cantidad del servicio")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Display(Name = "Precio", Prompt = "Ingrese el precio unitario del servicio")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El subtotal es obligatorio")]
        [Display(Name = "Subtotal", Prompt = "Ingrese o calcule el subtotal")]
        public decimal Subtotal { get; set; }
    }
}
