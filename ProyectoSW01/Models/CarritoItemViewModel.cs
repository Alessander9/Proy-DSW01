using System;

namespace ProyectoSW01.Models
{
    public class CarritoItemViewModel
    {
        public int CarDetId { get; set; }
        public int CarritoId { get; set; }
        public int ServicioId { get; set; }
        public string ServicioNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal => Cantidad * Precio;
    }
}
