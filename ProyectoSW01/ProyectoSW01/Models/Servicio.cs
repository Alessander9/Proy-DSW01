using System.ComponentModel.DataAnnotations;

namespace ProyectoSW01.Models
{

    //Promp cumple la funcion de placeholder en los formularios
    ///Display es la etiqueta que se muestra en los formularios
    ///Required indica que el campo es obligatorio
    ///StringLength limita la cantidad de caracteres que se pueden ingresar en el campo
    ///key indica que es la clave primaria de la tabla
    public class Servicio
    {
        [Key]
        public int ServicioId { get; set; }

        [Required(ErrorMessage = "El nombre del servicio es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no debe exceder los 100 caracteres")]
        [Display(Name = "Nombre del servicio", Prompt = "Ej. Cambio de aceite")]
        public string Nombre { get; set; }

        [StringLength(2000, ErrorMessage = "La descripción no debe exceder los 2000 caracteres")]
        [Display(Name = "Descripción", Prompt = "Describa brevemente el servicio")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio base es obligatorio")]
        [Display(Name = "Precio base", Prompt = "Ingrese el precio base")]
        public decimal PrecioBase { get; set; }

        [Display(Name = "Tiempo estimado (horas)", Prompt = "Ingrese el tiempo estimado en horas")]
        public decimal? TiempoEstimadoHoras { get; set; }

        [StringLength(50, ErrorMessage = "La categoría no debe exceder los 50 caracteres")]
        [Display(Name = "Categoría", Prompt = "Ej. Mantenimiento, Frenos, Suspensión")]
        public string Categoria { get; set; }
    }
}
