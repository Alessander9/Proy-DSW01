using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoSW01.Data;
using ProyectoSW01.Models;

namespace ProyectoSW01.Controllers
{
    public class MantenimientoController : Controller
    {
        private readonly MantenimientoRepository _mantenimientoRepository;
        private readonly VehiculoRepository _vehiculoRepository;
        private readonly ServicioRepository _servicioRepository;

        public MantenimientoController(
            MantenimientoRepository mantenimientoRepository,
            VehiculoRepository vehiculoRepository,
            ServicioRepository servicioRepository)
        {
            _mantenimientoRepository = mantenimientoRepository;
            _vehiculoRepository = vehiculoRepository;
            _servicioRepository = servicioRepository;
        }

        // Carga combos de Vehículos y Servicios en ViewBag
        private async Task CargarCombosAsync()
        {
            var vehiculos = await _vehiculoRepository.ListarVehiculosAsync();
            ViewBag.Vehiculos = vehiculos.ConvertAll(v =>
                new SelectListItem
                {
                    Value = v.VehiculoId.ToString(),
                    Text = $"{v.Placa} - {v.Marca} {v.Modelo}"
                });

            var servicios = await _servicioRepository.ListarServiciosAsync();
            ViewBag.Servicios = servicios.ConvertAll(s =>
                new SelectListItem
                {
                    Value = s.ServicioId.ToString(),
                    Text = $"{s.Nombre} (S/ {s.PrecioBase:0.00})"
                });
        }

        // GET: Mantenimiento/ListarMantenimientos
        public async Task<IActionResult> ListarMantenimientos()
        {
            var lista = await _mantenimientoRepository.ListarMantenimientosAsync();
            return View(lista); // Views/Mantenimiento/ListarMantenimientos.cshtml
        }

        // GET: Mantenimiento/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var mantenimiento = await _mantenimientoRepository.ObtenerMantenimientoPorIdAsync(id);
            if (mantenimiento == null)
            {
                return NotFound();
            }

            return View(mantenimiento); // Views/Mantenimiento/Details.cshtml
        }

        // GET: Mantenimiento/Create
        public async Task<IActionResult> Create()
        {
            await CargarCombosAsync();
            return View(); // Views/Mantenimiento/Create.cshtml
        }

        // POST: Mantenimiento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Mantenimiento mantenimiento)
        {
            // 1) Comprobamos si el modelo es válido
            if (!ModelState.IsValid)
            {
                // Mostrar todos los errores de validación en ViewBag
                ViewBag.ModelErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                await CargarCombosAsync();
                return View(mantenimiento);
            }

            try
            {
                // 2) Intentamos registrar en BD
                await _mantenimientoRepository.RegistrarMantenimientoAsync(mantenimiento);

                // 3) Si todo ok → redirige al listado
                return RedirectToAction(nameof(ListarMantenimientos));
            }
            catch (Exception ex)
            {
                // 4) Si hay un error SQL u otro, lo mostramos en la vista
                ViewBag.ErrorMessage = ex.Message;

                await CargarCombosAsync();
                return View(mantenimiento);
            }
        }



        // GET: Mantenimiento/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var mantenimiento = await _mantenimientoRepository.ObtenerMantenimientoPorIdAsync(id);
            if (mantenimiento == null)
            {
                return NotFound();
            }

            await CargarCombosAsync();
            return View(mantenimiento); // Views/Mantenimiento/Edit.cshtml
        }

        // POST: Mantenimiento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Mantenimiento mantenimiento)
        {
            if (id != mantenimiento.MantenimientoId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await CargarCombosAsync();
                return View(mantenimiento);
            }

            await _mantenimientoRepository.ActualizarMantenimientoAsync(mantenimiento);
            return RedirectToAction(nameof(ListarMantenimientos));
        }

        // GET: Mantenimiento/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var mantenimiento = await _mantenimientoRepository.ObtenerMantenimientoPorIdAsync(id);
            if (mantenimiento == null)
            {
                return NotFound();
            }

            return View(mantenimiento); // Views/Mantenimiento/Delete.cshtml
        }

        // POST: Mantenimiento/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mantenimientoRepository.EliminarMantenimientoAsync(id);
            return RedirectToAction(nameof(ListarMantenimientos));
        }

        public IActionResult MenuMantenimiento()
        {
            return View();
        }

    }
}
