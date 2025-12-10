namespace ProyectoSW01.Models
{
    public class OrdenTrabajoConDetalleYRepuestos
    {
        public OrdenTrabajo Orden { get; set; }
        public List<OrdenTrabajoDetalle> DetallesServicios { get; set; } = new();
        public List<OrdenTrabajoRepuestoDetalle> DetallesRepuestos { get; set; } = new();
    }
}
