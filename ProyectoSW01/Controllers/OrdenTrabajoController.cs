using Microsoft.AspNetCore.Mvc;
using ProyectoSW01.Data;
using ProyectoSW01.Models;
using System.Threading.Tasks;
using Rotativa.AspNetCore;

namespace ProyectoSW01.Controllers
{
    public class OrdenTrabajoController : Controller
    {
        private readonly OrdenTrabajoRepository _ordenRepo;

        public OrdenTrabajoController(OrdenTrabajoRepository ordenRepo)
        {
            _ordenRepo = ordenRepo;
        }

        // ============================================
        // LISTAR ÓRDENES DE TRABAJO
        // ============================================
        public async Task<IActionResult> ListarOrdenesDeTrabajo()
        {
            var lista = await _ordenRepo.ListarOrdenesAsync();
            return View(lista);
        }

        // ============================================
        // DETALLE DE UNA ORDEN
        // ============================================
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
                return NotFound();

            // Usamos el nuevo método que ya creaste en el repositorio
            var modelo = await _ordenRepo.ObtenerOrdenConDetalleAsync(id);

            if (modelo == null || modelo.Orden == null)
                return NotFound();

            return View(modelo);   // La vista Details ya tiene: @model ProyectoSW01.Models.OrdenTrabajoConDetalle
        }




        // ============================================
        // CAMBIAR ESTADO (ejemplo simple)
        // POST: /OrdenTrabajo/CambiarEstado
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int ordenId, string nuevoEstado)
        {
            if (ordenId <= 0 || string.IsNullOrWhiteSpace(nuevoEstado))
                return RedirectToAction("Details", new { id = ordenId });

            await _ordenRepo.CambiarEstadoAsync(ordenId, nuevoEstado);

            TempData["Mensaje"] = "Estado actualizado correctamente.";

            return RedirectToAction("Details", new { id = ordenId });
        }


        public async Task<IActionResult> Index()
        {
            var ordenes = await _ordenRepo.ListarOrdenesAsync();
            return View(ordenes);
        }


        public IActionResult MenuOrdenDeTrabajo()
        {

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewBag.Rol = rol;

            return View();
        }
        // ============================================
        // GET: OrdenTrabajo/Edit/5
        // ============================================
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0) return NotFound();

            var orden = await _ordenRepo.ObtenerPorIdAsync(id);
            if (orden == null) return NotFound();

            return View(orden); // model = OrdenTrabajo
        }

        // ============================================
        // POST: OrdenTrabajo/Edit
        // Solo actualiza el ESTADO usando el repo
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int ordenId, string estado)
        {
            if (ordenId <= 0 || string.IsNullOrWhiteSpace(estado))
            {
                TempData["Mensaje"] = "El estado es obligatorio.";
                return RedirectToAction("Edit", new { id = ordenId });
            }

            await _ordenRepo.CambiarEstadoAsync(ordenId, estado);

            // 👉 Si se finaliza / entrega, vamos a la vista de resumen
            if (estado == "entregado" || estado == "finalizado")
            {
                TempData["MostrarAlertaFinal"] = true;
                return RedirectToAction("ResumenFinal", new { id = ordenId });
            }

            // 👉 Si solo cambió a pendiente / en_proceso, volvemos al detalle normal
            TempData["Mensaje"] = "Estado actualizado correctamente.";
            return RedirectToAction("Details", new { id = ordenId });
        }

        // ============================================
        // RESUMEN FINAL DE ORDEN (cierre del flujo)
        // ============================================
        public async Task<IActionResult> ResumenFinal(int id)
        {
            if (id <= 0) return NotFound();

            var ordenConDetalle = await _ordenRepo.ObtenerOrdenConDetalleAsync(id);
            if (ordenConDetalle == null) return NotFound();

            return View(ordenConDetalle); // model = OrdenTrabajoConDetalle
        }

        public async Task<IActionResult> ExportarPdf(int id)
        {
            if (id <= 0) return NotFound();

            var ordenConDetalle = await _ordenRepo.ObtenerOrdenConDetalleAsync(id);
            if (ordenConDetalle == null) return NotFound();

            // 👉 Usamos una vista específica para PDF
            return new ViewAsPdf("ResumenFinalPdf", ordenConDetalle)
            {
                FileName = $"OrdenTrabajo_{id}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait
            };
        }

    }
}