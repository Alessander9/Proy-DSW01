namespace ProyectoSW01.Models
{
    public class RepuestoCarritoItemViewModel
    {
        public int CarRepId { get; set; }        // PK de la tabla
        public int CarritoId { get; set; }       // FK a carrito
        public int RepuestoId { get; set; }      // FK a repuestos
        public string RepuestoNombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal => Cantidad * Precio;

    }
}
