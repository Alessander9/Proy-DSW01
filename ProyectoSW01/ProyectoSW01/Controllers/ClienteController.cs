using Microsoft.AspNetCore.Mvc;
using ProyectoSW01.Data;
using ProyectoSW01.Models;
using System.Threading.Tasks;

namespace ProyectoSW01.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ClienteRepository _clienteRepo;

        public ClienteController(ClienteRepository clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        // LISTA
        public async Task<IActionResult> ListarClientes()
        {
            var clientes = await _clienteRepo.ListarClientesAsync();
            return View(clientes);
        }

        // CREATE GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            await _clienteRepo.RegistrarClienteAsync(cliente);
            return RedirectToAction(nameof(ListarClientes));
        }

        // EDIT GET
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _clienteRepo.ObtenerClientePorIdAsync(id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            await _clienteRepo.ActualizarClienteAsync(cliente);
            return RedirectToAction(nameof(ListarClientes));
        }

        // DETAILS GET
        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _clienteRepo.ObtenerClientePorIdAsync(id);
            if (cliente == null)
                return NotFound();

            return View(cliente);
        }


        // DELETE GET: mostrar confirmación
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _clienteRepo.ObtenerClientePorIdAsync(id);
            if (cliente == null)
                return NotFound();

            return View(cliente); // pasa el cliente a la vista de confirmación
        }

        // DELETE POST: confirmar y eliminar
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clienteRepo.EliminarClienteAsync(id);
            return RedirectToAction(nameof(ListarClientes)); // regresa al listado de clientes
        }

        public IActionResult MenuCliente()
        {

            return View();
        }

        // BUSCAR CLIENTES - GET (muestra la página vacía)
        public IActionResult BuscarClientes()
        {
            return View(); // mostrará solo el formulario al inicio
        }

        // BUSCAR CLIENTES - POST/GET (procesa la búsqueda)
        [HttpPost]
        public async Task<IActionResult> BuscarClientes(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                ViewBag.Error = "Ingresa un nombre, apellido o DNI para buscar.";
                return View();
            }

            var resultados = await _clienteRepo.BuscarClientesAsync(texto);
            ViewBag.TextoBuscado = texto;

            return View(resultados); // devuelve la lista de clientes encontrados
        }

      


    }
}
