namespace ProyectoSW01.Models
{
    public class OrdenTrabajoRepuestoDetalle
    {
        public int OrdRepId { get; set; }
        public int OrdenId { get; set; }
        public int RepuestoId { get; set; }
        public string RepuestoNombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
    }
}
