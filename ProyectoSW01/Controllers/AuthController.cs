using Microsoft.AspNetCore.Mvc;
using ProyectoSW01.Data;
using ProyectoSW01.Models;

namespace ProyectoSW01.Controllers
{
    public class AuthController : Controller
    {
        private readonly UsuarioRepository _usuarioRepo;

        public AuthController(UsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _usuarioRepo.ValidarUsuarioAsync(
                model.Correo,
                model.Contrasena
            );

            if (usuario == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos";
                return View(model);
            }

            HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);
            HttpContext.Session.SetString("Nombre", usuario.NombreCompleto);
            HttpContext.Session.SetInt32("Rol", usuario.IdRol);


            if (usuario.IdRol == 1)

            {
                return RedirectToAction("MenuPrincipal", "Home");
            }
            else if (usuario.IdRol == 2)
            {
                return RedirectToAction("MenuMecanico", "Home");
            }
            else
            { 
                return RedirectToAction("Index", "Home");
            }
            
        }

    }
}