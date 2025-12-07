using System.ComponentModel.DataAnnotations;

namespace ProyectoSW01.Models
{


    //Promp cumple la funcion de placeholder en los formularios
    ///Display es la etiqueta que se muestra en los formularios
    ///Required indica que el campo es obligatorio
    ///StringLength limita la cantidad de caracteres que se pueden ingresar en el campo
    ///key indica que es la clave primaria de la tabla
    public class DiagnosticoServicio
    {
        [Key]
        public int DiagServId { get; set; }

        [Required(ErrorMessage = "El diagnóstico es obligatorio")]
        [Display(Name = "Diagnóstico", Prompt = "Seleccione el diagnóstico")]
        public int DiagnosticoId { get; set; }

        [Required(ErrorMessage = "El servicio es obligatorio")]
        [Display(Name = "Servicio", Prompt = "Seleccione el servicio sugerido")]
        public int ServicioId { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Display(Name = "Precio sugerido", Prompt = "Ingrese el precio sugerido")]
        public decimal Precio { get; set; }

        [Display(Name = "Servicio")]
        public string ServicioNombre { get; set; }
    }

}
