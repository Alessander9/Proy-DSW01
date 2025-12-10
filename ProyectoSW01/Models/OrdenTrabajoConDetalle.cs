using System.Collections.Generic;

namespace ProyectoSW01.Models
{
    public class OrdenTrabajoConDetalle
    {
        public OrdenTrabajo Orden { get; set; }
        public List<OrdenTrabajoDetalle> Detalles { get; set; } = new();

        public List<OrdenTrabajoRepuestoDetalle> Repuestos { get; set; }
            = new List<OrdenTrabajoRepuestoDetalle>();
    }
}
