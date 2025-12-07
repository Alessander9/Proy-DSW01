using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoSW01.Models;
using ProyectoSW01.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSW01.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly RolRepository _rolRepository;

        public UsuarioController(UsuarioRepository usuarioRepository, RolRepository rolRepository)
        {
            _usuarioRepository = usuarioRepository;
            _rolRepository = rolRepository;
        }

        // ============================================
        // MÉTODO PARA CARGAR ROLES EN DROPDOWN
        // ============================================
        private async Task CargarRolesAsync(int? seleccionado = null)
        {
            var roles = await _rolRepository.ListarRolesAsync();

            ViewBag.Roles = roles.Select(r => new SelectListItem
            {
                Value = r.IdRol.ToString(),
                Text = r.Nombre,
                Selected = seleccionado != null && r.IdRol == seleccionado.Value
            }).ToList();
        }

        // ============================================
        // LISTAR USUARIOS
        // ============================================
        public async Task<IActionResult> ListarUsuario()
        {
            var usuarios = await _usuarioRepository.ListarUsuariosAsync();
            return View(usuarios);
        }

        // ============================================
        // CREAR USUARIO (GET)
        // ============================================
        public async Task<IActionResult> Create()
        {
            await CargarRolesAsync();
            return View();
        }

        public async Task<IActionResult> MenuUsuario() 
        {
            return View();
        }

        // ============================================
        // CREAR USUARIO (POST)
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            // ❗ Importante: IdUsuario NO se valida ni se envía
            ModelState.Remove(nameof(usuario.IdUsuario));

            if (!ModelState.IsValid)
            {
                await CargarRolesAsync(usuario.IdRol);
                return View(usuario);
            }

            await _usuarioRepository.RegistrarUsuarioAsync(usuario);
            return RedirectToAction(nameof(ListarUsuario));
        }

        // ============================================
        // EDITAR USUARIO (GET)
        // ============================================
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _usuarioRepository.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null)
                return RedirectToAction(nameof(ListarUsuario));

            await CargarRolesAsync(usuario.IdRol);
            return View(usuario);
        }

        // ============================================
        // EDITAR USUARIO (POST)
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            ModelState.Remove(nameof(usuario.IdUsuario));

            if (id != usuario.IdUsuario)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                await CargarRolesAsync(usuario.IdRol);
                return View(usuario);
            }

            await _usuarioRepository.ActualizarUsuarioAsync(usuario);
            return RedirectToAction(nameof(ListarUsuario));
        }

        public async Task<IActionResult> Details(int id)
        {
            // 1. Obtener el usuario por ID
            var usuario = await _usuarioRepository.ObtenerUsuarioPorIdAsync(id);

            // 2. Verificar si el usuario existe
            if (usuario == null)
                return NotFound(); // O RedirectToAction(nameof(ListarUsuario)) si prefieres volver a la lista

            // 3. Opcional: Cargar roles (si la vista de detalles también los necesita mostrar)
            // Generalmente, la vista de detalles solo muestra el nombre del rol,
            // pero si tu modelo no tiene el nombre, podrías necesitarlo.
            // await CargarRolesAsync(usuario.IdRol); 

            // 4. Devolver la vista con el modelo
            return View(usuario);
        }



        // ============================================
        // ELIMINAR USUARIO (GET)
        // ============================================
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _usuarioRepository.ObtenerUsuarioPorIdAsync(id);
            if (usuario == null)
                return RedirectToAction(nameof(ListarUsuario));

            return View(usuario);
        }

        // ============================================
        // ELIMINAR USUARIO (POST)
        // ============================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _usuarioRepository.EliminarUsuarioAsync(id);
            return RedirectToAction(nameof(ListarUsuario));
        }



    }
}
