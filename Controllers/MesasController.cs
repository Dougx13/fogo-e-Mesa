using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Infrastructure;
using RestauranteApp.Models;

namespace RestauranteApp.Controllers
{
    public class MesasController : Controller
    {
        private readonly AppDbContext _context;

        public MesasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var mesas = await _context.Mesas
                .AsNoTracking()
                .OrderBy(m => m.Numero)
                .ToListAsync();

            return View(mesas);
        }

        public async Task<IActionResult> Admin()
        {
            if (HttpContext.Session.GetString(SessionKeys.UserRole) != "Administrador")
            {
                return RedirectToAction(nameof(Index));
            }

            var mesas = await _context.Mesas
                .AsNoTracking()
                .OrderBy(m => m.Numero)
                .ToListAsync();

            return View(mesas);
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString(SessionKeys.UserRole) != "Administrador")
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Numero,Capacidade")] Mesa mesa)
        {
            if (HttpContext.Session.GetString(SessionKeys.UserRole) != "Administrador")
            {
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(mesa);
            }

            var numeroExistente = await _context.Mesas.AnyAsync(m => m.Numero == mesa.Numero);
            if (numeroExistente)
            {
                ModelState.AddModelError("Numero", "Ja existe uma mesa com este numero.");
                return View(mesa);
            }

            mesa.Status = "disponivel";
            _context.Mesas.Add(mesa);
            await _context.SaveChangesAsync();
            TempData["Sucesso"] = "Nova mesa cadastrada com sucesso!";
            return RedirectToAction(nameof(Admin));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlternarStatus(int idMesa)
        {
            if (HttpContext.Session.GetString(SessionKeys.UserRole) != "Administrador")
            {
                return RedirectToAction(nameof(Index));
            }

            var mesa = await _context.Mesas.FindAsync(idMesa);
            if (mesa == null)
            {
                return NotFound();
            }

            mesa.Status = mesa.Status == "disponivel" ? "reservada" : "disponivel";
            await _context.SaveChangesAsync();
            TempData["Sucesso"] = $"Status da mesa {mesa.Numero} atualizado para {mesa.Status}.";
            return RedirectToAction(nameof(Admin));
        }
    }
}
