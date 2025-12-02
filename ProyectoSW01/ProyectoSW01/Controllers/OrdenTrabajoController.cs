using Microsoft.AspNetCore.Mvc;
using ProyectoSW01.Data;
using ProyectoSW01.Models;
using System.Threading.Tasks;

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
            var resultado = await _ordenRepo.ObtenerConDetalleAsync(id);
            if (resultado == null)
                return RedirectToAction(nameof(ListarOrdenesDeTrabajo));

            return View(resultado); // luego hacemos la vista fuertemente tipada
        }

        // ============================================
        // CAMBIAR ESTADO (ejemplo simple)
        // POST: /OrdenTrabajo/CambiarEstado
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int ordenId, string nuevoEstado)
        {
            if (string.IsNullOrWhiteSpace(nuevoEstado))
            {
                // podrías manejar error de validación
                return RedirectToAction(nameof(ListarOrdenesDeTrabajo));
            }

            await _ordenRepo.CambiarEstadoAsync(ordenId, nuevoEstado);
            return RedirectToAction(nameof(ListarOrdenesDeTrabajo));
        }
    }
}
