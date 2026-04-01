using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Infrastructure;
using RestauranteApp.Models;
using RestauranteApp.ViewModels;

namespace RestauranteApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32(SessionKeys.UserId).HasValue)
            {
                return RedirectToRoleHome();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Email == model.Email && c.Senha == model.Senha);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
                return View(model);
            }

            HttpContext.Session.SetInt32(SessionKeys.UserId, usuario.IdCliente);
            HttpContext.Session.SetString(SessionKeys.UserName, usuario.Nome);
            HttpContext.Session.SetString(SessionKeys.UserRole, usuario.Perfil);

            return RedirectToRoleHome();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var emailExistente = await _context.Clientes.AnyAsync(c => c.Email == model.Email);
            if (emailExistente)
            {
                ModelState.AddModelError("Email", "Já existe uma conta cadastrada com este e-mail.");
                return View(model);
            }

            var cliente = new Cliente
            {
                Nome = model.Nome,
                Telefone = model.Telefone,
                Email = model.Email,
                Senha = model.Senha,
                Perfil = "Cliente"
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Cadastro realizado com sucesso. Faça login para reservar.";
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        private IActionResult RedirectToRoleHome()
        {
            var role = HttpContext.Session.GetString(SessionKeys.UserRole);
            return role == "Administrador"
                ? RedirectToAction("Admin", "Reservas")
                : RedirectToAction("Index", "Reservas");
        }
    }
}
