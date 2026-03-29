using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;

namespace RestauranteApp.Controllers
{
    public class ClientesController : Controller
    {
        private readonly AppDbContext _context;
        public ClientesController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index() =>
            View(await _context.Clientes.OrderBy(c => c.Nome).ToListAsync());

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.Clientes
                .Include(c => c.Reservas)
                .FirstOrDefaultAsync(c => c.IdCliente == id);
            return cliente == null ? NotFound() : View(cliente);
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Telefone,Email")] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            var emailJaExiste = await _context.Clientes
                .AnyAsync(c => c.Email == cliente.Email);

            if (emailJaExiste)
            {
                ModelState.AddModelError("Email", "Já existe um cliente cadastrado com este e-mail.");
                return View(cliente);
            }

            _context.Add(cliente);
            await _context.SaveChangesAsync();
            TempData["Sucesso"] = "Cliente cadastrado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var c = await _context.Clientes.FindAsync(id);
            return c == null ? NotFound() : View(c);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCliente,Nome,Telefone,Email")] Cliente cliente)
        {
            if (id != cliente.IdCliente) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var c = await _context.Clientes.FirstOrDefaultAsync(x => x.IdCliente == id);
            return c == null ? NotFound() : View(c);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var c = await _context.Clientes.FindAsync(id);
            if (c != null) _context.Clientes.Remove(c);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
