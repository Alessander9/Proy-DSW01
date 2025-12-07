using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoSW01.Data;
using ProyectoSW01.Models;
using System.Threading.Tasks;

namespace ProyectoSW01.Controllers
{
    public class CarritoController : Controller
    {
        private readonly CarritoRepository _carritoRepo;
        private readonly ServicioRepository _servicioRepo;
        private readonly UsuarioRepository _usuarioRepo;
        private readonly RepuestoRepository _repuestoRepo;


        public CarritoController(
             CarritoRepository carritoRepo,
             ServicioRepository servicioRepo,
             UsuarioRepository usuarioRepo,
             RepuestoRepository repuestoRepo
            )
        {
            _carritoRepo = carritoRepo;
            _servicioRepo = servicioRepo;
            _usuarioRepo = usuarioRepo;
            _repuestoRepo = repuestoRepo;
        }

        // ============================================
        // Ver carrito de un vehículo
        // GET: /Carrito/Index?vehiculoId=#
        // ============================================
        public async Task<IActionResult> Index(int vehiculoId)
        {
            if (vehiculoId <= 0)
                return BadRequest("Vehículo inválido.");

            // 1) Obtener o crear carrito
            var carritoId = await _carritoRepo.ObtenerOCrearCarritoAsync(vehiculoId);

            // 2) Listar items de SERVICIOS del carrito
            var itemsServicios = await _carritoRepo.ListarPorVehiculoAsync(vehiculoId);

            // 2b) Listar items de REPUESTOS del carrito
            var itemsRepuestos = await _carritoRepo.ListarRepuestosPorVehiculoAsync(vehiculoId);
            ViewBag.RepuestosCarrito = itemsRepuestos;

            // 3) Combo de SERVICIOS
            var servicios = await _servicioRepo.ListarServiciosAsync();
            ViewBag.Servicios = servicios.Select(s => new SelectListItem
            {
                Value = s.ServicioId.ToString(),
                Text = $"{s.Nombre} - {s.PrecioBase:C}"
            }).ToList();

            // 4) Combo de REPUESTOS
            var repuestos = await _repuestoRepo.ListarRepuestosAsync();
            ViewBag.Repuestos = repuestos.Select(r => new SelectListItem
            {
                Value = r.RepuestoId.ToString(),
                Text = $"{r.Nombre} - {r.Precio:C}"
            }).ToList();

            // 5) Combo de MECÁNICOS
            var mecanicos = await _usuarioRepo.ListarMecanicosAsync();
            ViewBag.Mecanicos = mecanicos.Select(m => new SelectListItem
            {
                Value = m.IdUsuario.ToString(),
                Text = m.NombreCompleto
            }).ToList();

            ViewBag.VehiculoId = vehiculoId;
            ViewBag.CarritoId = carritoId;

            return View(itemsServicios);   // Model = servicios
        }



        // ============================================
        // Agregar servicio al carrito
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(int vehiculoId, int servicioId, int cantidad)
        {
            if (vehiculoId <= 0 || servicioId <= 0 || cantidad <= 0)
                return RedirectToAction(nameof(Index), new { vehiculoId });

            var carritoId = await _carritoRepo.ObtenerOCrearCarritoAsync(vehiculoId);

            var servicio = await _servicioRepo.ObtenerServicioPorIdAsync(servicioId);
            if (servicio == null)
                return RedirectToAction(nameof(Index), new { vehiculoId });

            var precio = servicio.PrecioBase;

            await _carritoRepo.AgregarItemAsync(carritoId, servicioId, cantidad, precio);

            return RedirectToAction(nameof(Index), new { vehiculoId });
        }

        // ============================================
        // Agregar REPUESTO al carrito
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarRepuesto(int vehiculoId, int repuestoId, int cantidad)
        {
            if (vehiculoId <= 0 || repuestoId <= 0 || cantidad <= 0)
                return RedirectToAction(nameof(Index), new { vehiculoId });

            var carritoId = await _carritoRepo.ObtenerOCrearCarritoAsync(vehiculoId);

            var repuesto = await _repuestoRepo.ObtenerRepuestoPorIdAsync(repuestoId);
            if (repuesto == null)
                return RedirectToAction(nameof(Index), new { vehiculoId });

            var precio = repuesto.Precio;

            await _carritoRepo.AgregarRepuestoAsync(carritoId, repuestoId, cantidad, precio);

            return RedirectToAction(nameof(Index), new { vehiculoId });
        }



        // ============================================
        // Eliminar item del carrito
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarItem(int vehiculoId, int carDetId)
        {
            if (carDetId > 0)
            {
                await _carritoRepo.EliminarItemAsync(carDetId);
            }

            return RedirectToAction(nameof(Index), new { vehiculoId });
        }

        // ============================================
        // Crear orden desde el carrito
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearOrden(int vehiculoId, int carritoId, int? mecanicoId)
        {
            if (vehiculoId <= 0 || carritoId <= 0)
                return RedirectToAction(nameof(Index), new { vehiculoId });

            var ordenId = await _carritoRepo.CrearOrdenDesdeCarritoAsync(carritoId, vehiculoId, mecanicoId);

            await _carritoRepo.VaciarCarritoAsync(carritoId);

            return RedirectToAction("Details", "OrdenTrabajo", new { id = ordenId });
        }
        public IActionResult MenuCarrito()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarRepuesto(int vehiculoId, int carRepId)
        {
            if (carRepId > 0)
            {
                await _carritoRepo.EliminarRepuestoAsync(carRepId);
            }

            return RedirectToAction(nameof(Index), new { vehiculoId });
        }

    }

}
