using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoSW01.Data;
using ProyectoSW01.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProyectoSW01.Controllers
{
    public class DiagnosticoController : Controller
    {
        private readonly DiagnosticoRepository _diagnosticoRepository;
        private readonly VehiculoRepository _vehiculoRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly DiagnosticoServicioRepository _diagServRepo;

        public DiagnosticoController(
            DiagnosticoRepository diagnosticoRepository,
            VehiculoRepository vehiculoRepository,
            UsuarioRepository usuarioRepository,
            DiagnosticoServicioRepository diagServRepo)
        {
            _diagnosticoRepository = diagnosticoRepository;
            _vehiculoRepository = vehiculoRepository;
            _usuarioRepository = usuarioRepository;
            _diagServRepo = diagServRepo;
        }

        // ============================
        // CARGAR COMBOS (Vehículo, Mecánico, Estado)
        // ============================
        private async Task CargarCombosAsync(int? vehiculoId = null, int? mecanicoId = null)
        {
            // Vehículos: PLACA - MARCA MODELO
            var vehiculos = await _vehiculoRepository.ListarVehiculosParaDiagnosticoAsync();
            ViewBag.Vehiculos = vehiculos
                .Select(v => new SelectListItem
                {
                    Value = v.Id.ToString(),
                    Text = v.Texto,
                    Selected = vehiculoId.HasValue && v.Id == vehiculoId.Value
                })
                .ToList();

            // Mecánicos (Usuarios con rol de mecánico)
            var mecanicos = await _usuarioRepository.ListarMecanicosAsync();
            ViewBag.Mecanicos = mecanicos
                .Select(m => new SelectListItem
                {
                    Value = m.IdUsuario.ToString(),
                    Text = m.NombreCompleto,
                    // selecciona correctamente por IdUsuario
                    Selected = mecanicoId.HasValue && m.IdUsuario == mecanicoId.Value
                })
                .ToList();

            // Estados como SelectListItem
            ViewBag.Estados = new List<SelectListItem>
            {
                new SelectListItem { Value = "pendiente",   Text = "Pendiente"   },
                new SelectListItem { Value = "en_proceso", Text = "En proceso"  },
                new SelectListItem { Value = "completado", Text = "Completado"  }
            };
        }

        // ============================
        // LISTAR
        // ============================
        public async Task<IActionResult> ListarDiagnosticos()
        {
            var lista = await _diagnosticoRepository.ListarDiagnosticosAsync();
            return View(lista);
        }

        // ============================
        // SERVICIOS DEL DIAGNÓSTICO
        // ============================
        public async Task<IActionResult> Servicios(int diagnosticoId)
        {
            var lista = await _diagServRepo.ListarPorDiagnosticoAsync(diagnosticoId);
            ViewBag.DiagnosticoId = diagnosticoId;
            return View(lista);
        }

        // ============================
        // DETAILS
        // ============================
        public async Task<IActionResult> Details(int id)
        {
            var diagnostico = await _diagnosticoRepository.ObtenerDiagnosticoPorIdAsync(id);
            if (diagnostico == null)
                return RedirectToAction(nameof(ListarDiagnosticos));

            return View(diagnostico);
        }

        // ============================
        // CREATE GET
        // ============================
        public async Task<IActionResult> Create(int? vehiculoId)
        {
            var diagnostico = new Diagnostico();

            // Si viene desde Detalle Vehículo, precargamos el vehículo
            if (vehiculoId.HasValue)
                diagnostico.VehiculoId = vehiculoId.Value;

            await CargarCombosAsync(diagnostico.VehiculoId, diagnostico.MecanicoId);

            return View(diagnostico);
        }

        // ============================
        // CREATE POST
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Diagnostico diagnostico)
        {
            // Estos los maneja la BD
            ModelState.Remove(nameof(diagnostico.DiagnosticoId));
            ModelState.Remove(nameof(diagnostico.Fecha));

            Console.WriteLine("Entró al POST de Diagnostico/Create");

            if (!ModelState.IsValid)
            {
                // Volvemos a cargar combos si hay error de validación
                await CargarCombosAsync(diagnostico.VehiculoId, diagnostico.MecanicoId);
                return View(diagnostico);
            }

            // Si todo está OK, registramos y redirigimos
            await _diagnosticoRepository.RegistrarDiagnosticoAsync(diagnostico);
            return RedirectToAction(nameof(ListarDiagnosticos));
        }


        // ============================
        // EDIT GET
        // ============================
        public async Task<IActionResult> Edit(int id)
        {
            var diagnostico = await _diagnosticoRepository.ObtenerDiagnosticoPorIdAsync(id);
            if (diagnostico == null)
                return RedirectToAction(nameof(ListarDiagnosticos));

            await CargarCombosAsync(diagnostico.VehiculoId, diagnostico.MecanicoId);
            return View(diagnostico);
        }

        // ============================
        // EDIT POST
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Diagnostico diagnostico)
        {
            if (id != diagnostico.DiagnosticoId)
                return BadRequest();

            ModelState.Remove(nameof(diagnostico.Fecha));

            if (!ModelState.IsValid)
            {
                await CargarCombosAsync(diagnostico.VehiculoId, diagnostico.MecanicoId);
                return View(diagnostico);
            }

            await _diagnosticoRepository.ActualizarDiagnosticoAsync(diagnostico);
            return RedirectToAction(nameof(ListarDiagnosticos));
        }

        // ============================
        // DELETE GET
        // ============================
        public async Task<IActionResult> Delete(int id)
        {
            var diagnostico = await _diagnosticoRepository.ObtenerDiagnosticoPorIdAsync(id);
            if (diagnostico == null)
                return RedirectToAction(nameof(ListarDiagnosticos));

            return View(diagnostico);
        }

        // ============================
        // DELETE POST
        // ============================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _diagnosticoRepository.EliminarDiagnosticoAsync(id);
            return RedirectToAction(nameof(ListarDiagnosticos));
        }
    }
}
