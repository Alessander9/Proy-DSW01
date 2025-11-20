using System.ComponentModel.DataAnnotations;

namespace ProyectoSW01.Models

//Promp cumple la funcion de placeholder en los formularios
///Display es la etiqueta que se muestra en los formularios
///Required indica que el campo es obligatorio
///StringLength limita la cantidad de caracteres que se pueden ingresar en el campo
///key indica que es la clave primaria de la tabla
{
    public class Repuesto
    {
        [Key]
        public int RepuestoId { get; set; }

        [Required(ErrorMessage = "El nombre del repuesto es obligatorio")]
        [StringLength(120, ErrorMessage = "El nombre no debe exceder los 120 caracteres")]
        [Display(Name = "Nombre del repuesto", Prompt = "Ej. Filtro de aceite")]
        public string Nombre { get; set; }

        [StringLength(2000, ErrorMessage = "La descripción no debe exceder los 2000 caracteres")]
        [Display(Name = "Descripción", Prompt = "Describa el repuesto")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Display(Name = "Precio", Prompt = "Ingrese el precio del repuesto")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Display(Name = "Stock", Prompt = "Ingrese el stock disponible")]
        public int Stock { get; set; }
    }
}
