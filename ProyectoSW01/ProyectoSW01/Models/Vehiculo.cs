
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoSW01.Models
{


    //Promp cumple la funcion de placeholder en los formularios
    ///Display es la etiqueta que se muestra en los formularios
    ///Required indica que el campo es obligatorio
    ///StringLength limita la cantidad de caracteres que se pueden ingresar en el campo
    ///key indica que es la clave primaria de la tabla
    public class Vehiculo
    {
        [Key]
        public int VehiculoId { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio")]
        [Display(Name = "Cliente", Prompt = "Seleccione el cliente propietario")]
        public int ClienteId { get; set; }

        [Display(Name = "Cliente", Prompt = "Ingrese nombre del propietario")]
        [NotMapped]

        public string? ClienteNombreCompleto { get; set; }

        [StringLength(50, ErrorMessage = "La marca no debe exceder los 50 caracteres")]
        [Display(Name = "Marca", Prompt = "Ej. Toyota, Hyundai, Kia")]
        public string Marca { get; set; }

        [StringLength(50, ErrorMessage = "El modelo no debe exceder los 50 caracteres")]
        [Display(Name = "Modelo", Prompt = "Ej. Yaris, Elantra, Rio")]
        public string Modelo { get; set; }

        [Display(Name = "Año", Prompt = "Ingrese el año del vehículo")]
        public int? Anio { get; set; }

        [StringLength(20, ErrorMessage = "La placa no debe exceder los 20 caracteres")]
        [Display(Name = "Placa", Prompt = "Ej. ABC-123")]
        public string Placa { get; set; }

        [StringLength(50, ErrorMessage = "El VIN no debe exceder los 50 caracteres")]
        [Display(Name = "VIN", Prompt = "Ingrese el número de VIN")]
        public string Vin { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime FechaRegistro { get; set; }
    }

}
