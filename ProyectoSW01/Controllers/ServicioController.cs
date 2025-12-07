using Microsoft.AspNetCore.Mvc;
using ProyectoSW01.Data;
using ProyectoSW01.Models;
using System.Threading.Tasks;

namespace ProyectoSW01.Controllers
{
    public class ServicioController : Controller
    {
        private readonly ServicioRepository _servicioRepo;

        public ServicioController(ServicioRepository servicioRepo)
        {
            _servicioRepo = servicioRepo;
        }

        // LISTA
        public async Task<IActionResult> ListarServicios()
        {
            var servicios = await _servicioRepo.ListarServiciosAsync();
            return View(servicios);
        }

        // CREATE GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Servicio servicio)
        {
            if (!ModelState.IsValid)
                return View(servicio);

            await _servicioRepo.RegistrarServicioAsync(servicio);
            return RedirectToAction(nameof(ListarServicios));
        }

        // EDIT GET
        public async Task<IActionResult> Edit(int id)
        {
            var servicio = await _servicioRepo.ObtenerServicioPorIdAsync(id);
            if (servicio == null) return NotFound();

            return View(servicio);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Servicio servicio)
        {
            if (!ModelState.IsValid)
                return View(servicio);

            await _servicioRepo.ActualizarServicioAsync(servicio);
            return RedirectToAction(nameof(ListarServicios));
        }

        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var servicio = await _servicioRepo.ObtenerServicioPorIdAsync(id);
            if (servicio == null) return NotFound();

            return View(servicio);
        }

        // DELETE GET
        public async Task<IActionResult> Delete(int id)
        {
            var servicio = await _servicioRepo.ObtenerServicioPorIdAsync(id);
            if (servicio == null) return NotFound();

            return View(servicio);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _servicioRepo.EliminarServicioAsync(id);
            return RedirectToAction(nameof(ListarServicios));
        }

        public IActionResult MenuServicios()
        {

            return View();
        }

    }
}
