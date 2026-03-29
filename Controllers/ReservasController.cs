using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Models;

namespace RestauranteApp.Controllers
{
    public class ReservasController : Controller
    {
        private readonly AppDbContext _context;

        public ReservasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var reservas = await _context.Reservas
                .AsNoTracking()
                .Include(r => r.Cliente)
                .Include(r => r.Mesa)
                .OrderBy(r => r.DataReserva)
                .ThenBy(r => r.Horario)
                .ToListAsync();
            return View(reservas);
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var reserva = await _context.Reservas
                .AsNoTracking()
                .Include(r => r.Cliente)
                .Include(r => r.Mesa)
                .FirstOrDefaultAsync(r => r.IdReserva == id);
            if (reserva == null) return NotFound();
            return View(reserva);
        }

        // GET: Reservas/Create
        public IActionResult Create()
        {
            PopulateDropDowns();
            ViewBag.SemClientes = !_context.Clientes.Any();
            return View();
        }

        // POST: Reservas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reserva reserva)
        {
            ValidarReserva(reserva);

            if (!ModelState.IsValid)
            {
                PopulateDropDowns(reserva.IdCliente, reserva.IdMesa);
                ViewBag.SemClientes = !_context.Clientes.Any();
                return View(reserva);
            }

            _context.Add(reserva);
            await _context.SaveChangesAsync();
            TempData["Sucesso"] = "Reserva criada com sucesso!";
            return RedirectToAction(nameof(Details), new { id = reserva.IdReserva });
        }
        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return NotFound();
            PopulateDropDowns(reserva.IdCliente, reserva.IdMesa);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReserva,DataReserva,Horario,QuantidadePessoas,Status,IdCliente,IdMesa,PedidoReserva")] Reserva reserva)
        {
            if (id != reserva.IdReserva) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(reserva);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Reserva atualizada!";
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDowns(reserva.IdCliente, reserva.IdMesa);
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var reserva = await _context.Reservas
                .Include(r => r.Cliente).Include(r => r.Mesa)
                .FirstOrDefaultAsync(r => r.IdReserva == id);
            if (reserva == null) return NotFound();
            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null) _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            TempData["Sucesso"] = "Reserva removida.";
            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropDowns(int? idCliente = null, int? idMesa = null)
        {
            ViewData["IdCliente"] = new SelectList(_context.Clientes.OrderBy(c => c.Nome), "IdCliente", "Nome", idCliente);
            ViewData["IdMesa"] = new SelectList(_context.Mesas.Where(m => m.Status == "disponivel").OrderBy(m => m.Numero), "IdMesa", "Numero", idMesa);
            ViewData["Status"] = new SelectList(new[] { "pendente", "confirmada", "cancelada", "concluida" });
        }

        private void ValidarReserva(Reserva reserva)
        {
            var mesaSelecionada = _context.Mesas.FirstOrDefault(m => m.IdMesa == reserva.IdMesa);

            if (mesaSelecionada == null)
            {
                ModelState.AddModelError("IdMesa", "Selecione uma mesa válida.");
                return;
            }

            if (reserva.QuantidadePessoas > mesaSelecionada.Capacidade)
            {
                ModelState.AddModelError("QuantidadePessoas", $"A mesa {mesaSelecionada.Numero} suporta até {mesaSelecionada.Capacidade} pessoas.");
            }

            var horarioOcupado = _context.Reservas.Any(r =>
                r.IdMesa == reserva.IdMesa &&
                r.DataReserva.Date == reserva.DataReserva.Date &&
                r.Horario == reserva.Horario &&
                r.Status != "cancelada");

            if (horarioOcupado)
            {
                ModelState.AddModelError("Horario", "Esta mesa já está reservada para a data e horário informados.");
            }
        }
    }
}
