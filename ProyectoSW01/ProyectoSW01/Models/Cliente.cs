
using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectoSW01.Models
{


    //Promp cumple la funcion de placeholder en los formularios
    ///Display es la etiqueta que se muestra en los formularios
    ///Required indica que el campo es obligatorio
    ///StringLength limita la cantidad de caracteres que se pueden ingresar en el campo
    ///key indica que es la clave primaria de la tabla
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [StringLength(100, ErrorMessage = "Los nombres no deben exceder los 100 caracteres")]
        [Display(Name = "Nombres", Prompt = "Ingrese los nombres del cliente")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100, ErrorMessage = "Los apellidos no deben exceder los 100 caracteres")]
        [Display(Name = "Apellidos", Prompt = "Ingrese los apellidos del cliente")]
        public string Apellidos { get; set; }

        [StringLength(20, ErrorMessage = "El DNI no debe exceder los 20 caracteres")]
        [Display(Name = "DNI", Prompt = "Ingrese el documento de identidad")]
        public string Dni { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no debe exceder los 20 caracteres")]
        [Display(Name = "Teléfono", Prompt = "Ingrese el teléfono del cliente")]
        public string Telefono { get; set; }

        [StringLength(120, ErrorMessage = "El correo no debe exceder los 120 caracteres")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido")]
        [Display(Name = "Correo electrónico", Prompt = "ejemplo@correo.com")]
        public string Correo { get; set; }

        [StringLength(200, ErrorMessage = "La dirección no debe exceder los 200 caracteres")]
        [Display(Name = "Dirección", Prompt = "Ingrese la dirección del cliente")]
        public string Direccion { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime FechaRegistro { get; set; }
    }
}
