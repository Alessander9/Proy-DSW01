using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProyectoSW01.Data;
using ProyectoSW01.Models;

namespace ProyectoSW01.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepository _usuarioRepo;

        public UsuarioController(UsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        // ==============================
        // GET: /Usuarios
        // ==============================
        public async Task<IActionResult> ListarUsuario()
        {
            var usuarios = await _usuarioRepo.ObtenerUsuariosAsync();
            return View(usuarios);
        }

        // ==============================
        // GET: /Usuarios/Create
        // ==============================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // ==============================
        // POST: /Usuarios/Create
        // ==============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            await _usuarioRepo.AgregarUsuarioAsync(usuario);
            return RedirectToAction(nameof(ListarUsuario));
        }

        // ==============================
        // GET: /Usuarios/Edit/5
        // ==============================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _usuarioRepo.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // ==============================
        // POST: /Usuarios/Edit/5
        // ==============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            await _usuarioRepo.ActualizarUsuarioAsync(usuario);
            return RedirectToAction(nameof(Index));
        }

        // ==============================
        // GET: /Usuarios/Delete/5
        // ==============================
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _usuarioRepo.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // ==============================
        // POST: /Usuarios/DeleteConfirmed/5
        // ==============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _usuarioRepo.EliminarUsuarioAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
