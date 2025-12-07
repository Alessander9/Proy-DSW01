
using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectoSW01.Models
{


    //Promp cumple la funcion de placeholder en los formularios
    ///Display es la etiqueta que se muestra en los formularios
    ///Required indica que el campo es obligatorio
    ///StringLength limita la cantidad de caracteres que se pueden ingresar en el campo
    ///key indica que es la clave primaria de la tabla
    public class Diagnostico
    {
        [Key]
        public int DiagnosticoId { get; set; }

        [Required(ErrorMessage = "El vehículo es obligatorio")]
        [Display(Name = "Vehículo", Prompt = "Seleccione el vehículo")]
        public int VehiculoId { get; set; }

        [Required(ErrorMessage = "El mecánico es obligatorio")]
        [Display(Name = "Mecánico", Prompt = "Seleccione el mecánico")]
        public int MecanicoId { get; set; }

        [Required(ErrorMessage = "La descripción del diagnóstico es obligatoria")]
        [StringLength(2000, ErrorMessage = "La descripción no debe exceder los 2000 caracteres")]
        [Display(Name = "Descripción del diagnóstico", Prompt = "Describa el problema del vehículo")]
        public string Descripcion { get; set; }

        [Display(Name = "Fecha del diagnóstico")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El estado del diagnóstico es obligatorio")]
        [StringLength(20, ErrorMessage = "El estado no debe exceder los 20 caracteres")]
        [Display(Name = "Estado del diagnóstico", Prompt = "Ej. pendiente, en_proceso, completado")]
        public string Estado { get; set; }

        public string? Placa { get; set; }
        public string? Modelo { get; set; }
        public string? MecanicoNombre { get; set; }

  
    }
}
