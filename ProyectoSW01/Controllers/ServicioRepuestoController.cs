using Microsoft.AspNetCore.Mvc;
using ProyectoSW01.Data;
using ProyectoSW01.Models;
using System.Threading.Tasks;

namespace ProyectoSW01.Controllers
{
    public class ServicioRepuestoController : Controller
    {
        private readonly ServicioRepuestoRepository _servicioRepuestoRepository;

        public ServicioRepuestoController(ServicioRepuestoRepository servicioRepuestoRepository)
        {
            _servicioRepuestoRepository = servicioRepuestoRepository;
        }

        // ============================================
        // LISTAR POR DIAGNOSTICO-SERVICIO
        // /ServicioRepuesto/ListarPorDiagnostico?diagServId=1
        // ============================================
        public async Task<IActionResult> ListarPorDiagnostico(int diagServId)
        {
            var lista = await _servicioRepuestoRepository.ListarPorDiagnosticoAsync(diagServId);
            ViewBag.DiagServId = diagServId;
            return View(lista);
        }

        // ============================================
        // CREATE GET
        // ============================================
        // GET: /ServicioRepuesto/Create?diagServId=#
        public IActionResult Create(int diagServId)
        {
            if (diagServId <= 0)
                return BadRequest("diagServId inválido.");

            var modelo = new ServicioRepuesto
            {
                DiagServId = diagServId
            };

            ViewBag.DiagServId = diagServId;
            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServicioRepuesto modelo)
        {
            ModelState.Remove(nameof(modelo.ServRepId));

            if (modelo.DiagServId <= 0)
                ModelState.AddModelError("", "El Diagnóstico-Servicio asociado no es válido.");

            if (!ModelState.IsValid)
            {
                ViewBag.DiagServId = modelo.DiagServId;
                return View(modelo);
            }

            await _servicioRepuestoRepository.RegistrarAsync(modelo);

            return RedirectToAction(nameof(ListarPorDiagnostico),
                new { diagServId = modelo.DiagServId });
        }

        // ============================================
        // EDIT GET
        // ============================================
        public async Task<IActionResult> Edit(int id)
        {
            var entidad = await _servicioRepuestoRepository.ObtenerPorIdAsync(id);
            if (entidad == null)
                return NotFound();

            ViewBag.DiagServId = entidad.DiagServId;
            return View(entidad);
        }

        // ============================================
        // EDIT POST
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServicioRepuesto modelo)
        {
            if (id != modelo.ServRepId)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.DiagServId = modelo.DiagServId;
                return View(modelo);
            }

            await _servicioRepuestoRepository.ActualizarAsync(modelo);

            return RedirectToAction(nameof(ListarPorDiagnostico), new { diagServId = modelo.DiagServId });
        }

        // ============================================
        // DELETE GET
        // ============================================
        public async Task<IActionResult> Delete(int id)
        {
            var entidad = await _servicioRepuestoRepository.ObtenerPorIdAsync(id);
            if (entidad == null)
                return NotFound();

            ViewBag.DiagServId = entidad.DiagServId;
            return View(entidad);
        }

        // ============================================
        // DELETE POST
        // ============================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int diagServId)
        {
            await _servicioRepuestoRepository.EliminarAsync(id);

            return RedirectToAction(nameof(ListarPorDiagnostico), new { diagServId });
        }

        // ============================================
        // DETAILS (opcional)
        // ============================================
        public async Task<IActionResult> Details(int id)
        {
            var entidad = await _servicioRepuestoRepository.ObtenerPorIdAsync(id);
            if (entidad == null)
                return NotFound();

            ViewBag.DiagServId = entidad.DiagServId;
            return View(entidad);
        }
    }
}
