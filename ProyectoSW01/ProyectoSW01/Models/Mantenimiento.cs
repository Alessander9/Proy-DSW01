using System.ComponentModel.DataAnnotations;

namespace ProyectoSW01.Models
{


    //Promp cumple la funcion de placeholder en los formularios
    ///Display es la etiqueta que se muestra en los formularios
    ///Required indica que el campo es obligatorio
    ///StringLength limita la cantidad de caracteres que se pueden ingresar en el campo
    ///key indica que es la clave primaria de la tabla
    public class Mantenimiento
    {
        [Key]
        public int MantenimientoId { get; set; }

        [Required(ErrorMessage = "El vehículo es obligatorio")]
        [Display(Name = "Vehículo", Prompt = "Seleccione el vehículo")]
        public int VehiculoId { get; set; }

        [Required(ErrorMessage = "El servicio es obligatorio")]
        [Display(Name = "Servicio", Prompt = "Seleccione el servicio")]
        public int ServicioId { get; set; }

        [Display(Name = "Fecha programada", Prompt = "Seleccione la fecha programada")]
        public DateTime? FechaProgramada { get; set; }

        [Display(Name = "Fecha de entrega", Prompt = "Seleccione la fecha de entrega")]
        public DateTime? FechaEntrega { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(20, ErrorMessage = "El estado no debe exceder los 20 caracteres")]
        [Display(Name = "Estado", Prompt = "Ej. programado, en_proceso, entregado, cancelado")]
        public string Estado { get; set; }

        [StringLength(2000, ErrorMessage = "Las notas no deben exceder los 2000 caracteres")]
        [Display(Name = "Notas", Prompt = "Ingrese notas adicionales del mantenimiento")]
        public string Notas { get; set; }

        public string? Placa { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? ServicioNombre { get; set; }



    }
}
