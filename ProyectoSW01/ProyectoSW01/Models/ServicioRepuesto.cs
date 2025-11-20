using System.ComponentModel.DataAnnotations;

namespace ProyectoSW01.Models
{


    //Promp cumple la funcion de placeholder en los formularios
    ///Display es la etiqueta que se muestra en los formularios
    ///Required indica que el campo es obligatorio
    ///StringLength limita la cantidad de caracteres que se pueden ingresar en el campo
    ///key indica que es la clave primaria de la tabla
    public class ServicioRepuesto
    {
        [Key]
        public int ServRepId { get; set; }

        [Required(ErrorMessage = "El diagnóstico-servicio es obligatorio")]
        [Display(Name = "Diagnóstico-Servicio", Prompt = "Seleccione el diagnóstico-servicio")]
        public int DiagServId { get; set; }

        [Required(ErrorMessage = "El repuesto es obligatorio")]
        [Display(Name = "Repuesto", Prompt = "Seleccione el repuesto")]
        public int RepuestoId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Display(Name = "Cantidad", Prompt = "Ingrese la cantidad de repuestos usados")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Display(Name = "Precio", Prompt = "Ingrese el precio unitario del repuesto")]
        public decimal Precio { get; set; }
    }
}
