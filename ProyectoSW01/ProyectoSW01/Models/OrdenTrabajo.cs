using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoSW01.Models
{


    //Promp cumple la funcion de placeholder en los formularios
    ///Display es la etiqueta que se muestra en los formularios
    ///Required indica que el campo es obligatorio
    ///StringLength limita la cantidad de caracteres que se pueden ingresar en el campo
    ///key indica que es la clave primaria de la tabla
    public class OrdenTrabajo
    {
        [Key]
        public int OrdenId { get; set; }

        [Required(ErrorMessage = "El vehículo es obligatorio")]
        [Display(Name = "Vehículo", Prompt = "Seleccione el vehículo")]
        public int VehiculoId { get; set; }

        [Display(Name = "Diagnóstico", Prompt = "Seleccione el diagnóstico (opcional)")]
        public int? DiagnosticoId { get; set; }

        [Display(Name = "Mecánico", Prompt = "Seleccione el mecánico asignado")]
        public int? MecanicoId { get; set; }

        [Display(Name = "Fecha de la orden")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El estado de la orden es obligatorio")]
        [StringLength(20, ErrorMessage = "El estado no debe exceder los 20 caracteres")]
        [Display(Name = "Estado de la orden", Prompt = "Ej. pendiente, en_proceso, terminado, entregado, cancelado")]
        public string Estado { get; set; }

        [Display(Name = "Total", Prompt = "Total calculado de la orden")]
        public decimal Total { get; set; }

        [StringLength(2000, ErrorMessage = "Las observaciones no deben exceder los 2000 caracteres")]
        [Display(Name = "Observaciones", Prompt = "Ingrese observaciones adicionales de la orden")]
        public string Observaciones { get; set; }

        [NotMapped]
        public string ClienteNombreCompleto { get; set; }

        [NotMapped]
        public string MecanicoNombreCompleto { get; set; }

        [NotMapped]
        public string VehiculoDescripcion { get; set; }

        [NotMapped]
        public string VehiculoPlaca
        {
            get; set;

        }
    }
}
