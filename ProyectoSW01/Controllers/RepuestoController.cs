using Microsoft.AspNetCore.Mvc;
using ProyectoSW01.Data;
using ProyectoSW01.Models;
using System.Threading.Tasks;

namespace ProyectoSW01.Controllers
{
    public class RepuestoController : Controller
    {
        private readonly RepuestoRepository _repuestoRepository;

        public RepuestoController(RepuestoRepository repuestoRepository)
        {
            _repuestoRepository = repuestoRepository;
        }

        // ============================
        // LISTAR
        // ============================
        public async Task<IActionResult> ListarRepuestos()
        {
            var lista = await _repuestoRepository.ListarRepuestosAsync();
            return View(lista);
        }

        // ============================
        // DETAILS
        // ============================
        public async Task<IActionResult> Details(int id)
        {
            var repuesto = await _repuestoRepository.ObtenerRepuestoPorIdAsync(id);
            if (repuesto == null)
                return RedirectToAction(nameof(ListarRepuestos));

            return View(repuesto);
        }

        // ============================
        // CREATE
        // ============================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Repuesto repuesto)
        {
            if (!ModelState.IsValid)
                return View(repuesto);

            await _repuestoRepository.RegistrarRepuestoAsync(repuesto);
            return RedirectToAction(nameof(ListarRepuestos));
        }

        // ============================
        // EDIT
        // ============================
        public async Task<IActionResult> Edit(int id)
        {
            var repuesto = await _repuestoRepository.ObtenerRepuestoPorIdAsync(id);
            if (repuesto == null)
                return RedirectToAction(nameof(ListarRepuestos));

            return View(repuesto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Repuesto repuesto)
        {
            if (id != repuesto.RepuestoId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(repuesto);

            await _repuestoRepository.ActualizarRepuestoAsync(repuesto);
            return RedirectToAction(nameof(ListarRepuestos));
        }

        // ============================
        // DELETE
        // ============================
        public async Task<IActionResult> Delete(int id)
        {
            var repuesto = await _repuestoRepository.ObtenerRepuestoPorIdAsync(id);
            if (repuesto == null)
                return RedirectToAction(nameof(ListarRepuestos));

            return View(repuesto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repuestoRepository.EliminarRepuestoAsync(id);
            return RedirectToAction(nameof(ListarRepuestos));
        }

        public IActionResult MenuRepuestos()
        {
            return View();
        }

       
    }
}
