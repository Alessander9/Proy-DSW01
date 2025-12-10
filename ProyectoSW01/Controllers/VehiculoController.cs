using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoSW01.Data;
using ProyectoSW01.Models;

namespace ProyectoSW01.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly VehiculoRepository _vehiculoRepository;
        private readonly ClienteRepository _clienteRepository;

        public VehiculoController(
            VehiculoRepository vehiculoRepository,
            ClienteRepository clienteRepository)
        {
            _vehiculoRepository = vehiculoRepository;
            _clienteRepository = clienteRepository;
        }

        // =========================================
        // MÉTODO PRIVADO PARA CARGAR COMBO CLIENTES
        // =========================================
        private async Task CargarClientesAsync(int? clienteSeleccionado = null)
        {
            var clientes = await _clienteRepository.ListarClientesAsync();

            ViewBag.Clientes = clientes
                .Select(c => new SelectListItem
                {
                    Value = c.ClienteId.ToString(),
                    Text = $"{c.Nombres} {c.Apellidos} - {c.Dni}",
                    Selected = clienteSeleccionado.HasValue && c.ClienteId == clienteSeleccionado.Value
                })
                .ToList();
        }

        // =========================================
        // LISTAR VEHÍCULOS
        // =========================================
        // GET: Vehiculo/ListarVehiculos
        public async Task<IActionResult> ListarVehiculos()
        {
            var lista = await _vehiculoRepository.ListarVehiculosAsync();
            return View(lista); // Views/Vehiculo/ListarVehiculos.cshtml
        }

        // =========================================
        // DETALLE
        // =========================================
        // GET: Vehiculo/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var vehiculo = await _vehiculoRepository.ObtenerVehiculoPorIdAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo); // Views/Vehiculo/Details.cshtml
        }

        // =========================================
        // CREAR
        // =========================================
        // GET: Vehiculo/Create
        public async Task<IActionResult> Create()
        {
            await CargarClientesAsync();
            return View(); // Views/Vehiculo/Create.cshtml
        }

        // POST: Vehiculo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehiculo vehiculo)
        {
            if (!ModelState.IsValid)
            {
                // Para ver claramente qué está fallando
                ViewBag.ModelErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                await CargarClientesAsync(vehiculo.ClienteId);
                return View(vehiculo);
            }

            try
            {
                await _vehiculoRepository.RegistrarVehiculoAsync(vehiculo);
                return RedirectToAction(nameof(ListarVehiculos));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                await CargarClientesAsync(vehiculo.ClienteId);
                return View(vehiculo);
            }
        }

        // =========================================
        // EDITAR
        // =========================================
        // GET: Vehiculo/Edit/5
        // GET: Vehiculo/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var vehiculo = await _vehiculoRepository.ObtenerVehiculoPorIdAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            await CargarClientesAsync(vehiculo.ClienteId); // importante para el combo
            return View(vehiculo);
        }

        // POST: Vehiculo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehiculo vehiculo)
        {
            if (id != vehiculo.VehiculoId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ModelErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                await CargarClientesAsync(vehiculo.ClienteId);
                return View(vehiculo);
            }

            try
            {
                await _vehiculoRepository.ActualizarVehiculoAsync(vehiculo);
                return RedirectToAction(nameof(ListarVehiculos));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                await CargarClientesAsync(vehiculo.ClienteId);
                return View(vehiculo);
            }
        }

        // =========================================
        // ELIMINAR
        // =========================================
        // GET: Vehiculo/Delete/5
 
        public async Task<IActionResult> Delete(int id)
        {
            var vehiculo = await _vehiculoRepository.ObtenerVehiculoPorIdAsync(id);
            if (vehiculo == null)
            {
                // Si no existe, volvemos al listado
                return RedirectToAction(nameof(ListarVehiculos));
            }

            return View(vehiculo); // Views/Vehiculo/Delete.cshtml
        }

        // POST: Vehiculo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vehiculoRepository.EliminarVehiculoAsync(id);
            return RedirectToAction(nameof(ListarVehiculos));
        }

        public IActionResult MenuVehiculo()
        {

            var rol = HttpContext.Session.GetInt32("Rol");
            ViewBag.Rol = rol;

            return View();
        }

        // GET: Vehiculo/BuscarVehiculos
        [HttpGet]
        public IActionResult BuscarVehiculos()
        {
            // muestra solo el formulario
            return View();
        }

        // POST: Vehiculo/BuscarVehiculos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuscarVehiculos(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                ViewBag.Error = "Ingresa nombre de cliente o número de placa para buscar.";
                return View();
            }

            var resultados = await _vehiculoRepository.BuscarVehiculosAsync(texto);
            ViewBag.TextoBuscado = texto;

            return View(resultados); // misma vista, ahora con resultados
        }


    }
}
