using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;
using RestauranteApp.Infrastructure;
using RestauranteApp.Models;

namespace RestauranteApp.Controllers
{
    public class ReservasController : Controller
    {
        private static readonly TimeSpan HorarioAbertura = new(18, 0, 0);
        private static readonly TimeSpan HorarioEncerramento = new(23, 0, 0);
        private readonly AppDbContext _context;

        public ReservasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            if (!UsuarioLogado())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (UsuarioEhAdmin())
            {
                return RedirectToAction(nameof(Admin));
            }

            var clienteId = HttpContext.Session.GetInt32(SessionKeys.UserId)!.Value;
            var reservas = await _context.Reservas
                .AsNoTracking()
                .Include(r => r.Cliente)
                .Include(r => r.Mesa)
                .Where(r => r.IdCliente == clienteId)
                .OrderBy(r => r.DataReserva)
                .ThenBy(r => r.Horario)
                .ToListAsync();
            return View(reservas);
        }

        public async Task<IActionResult> Admin()
        {
            if (!UsuarioEhAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

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
            if (!UsuarioLogado()) return RedirectToAction("Login", "Auth");

            var reserva = await _context.Reservas
                .AsNoTracking()
                .Include(r => r.Cliente)
                .Include(r => r.Mesa)
                .FirstOrDefaultAsync(r => r.IdReserva == id);
            if (reserva == null) return NotFound();
            if (!UsuarioEhAdmin() && reserva.IdCliente != HttpContext.Session.GetInt32(SessionKeys.UserId))
            {
                return RedirectToAction(nameof(Index));
            }
            return View(reserva);
        }

        // GET: Reservas/Create
        public IActionResult Create()
        {
            if (!UsuarioLogado())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (UsuarioEhAdmin())
            {
                return RedirectToAction(nameof(Admin));
            }

            PopulateDropDowns();
            PrepararRestricoesDeReserva();
            ViewBag.ClienteLogado = HttpContext.Session.GetString(SessionKeys.UserName);
            return View();
        }

        // POST: Reservas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reserva reserva)
        {
            if (!UsuarioLogado())
            {
                return RedirectToAction("Login", "Auth");
            }

            if (UsuarioEhAdmin())
            {
                return RedirectToAction(nameof(Admin));
            }

            reserva.IdCliente = HttpContext.Session.GetInt32(SessionKeys.UserId) ?? 0;
            reserva.Status = "pendente";
            ValidarReserva(reserva);

            if (!ModelState.IsValid)
            {
                PopulateDropDowns(reserva.IdMesa);
                PrepararRestricoesDeReserva();
                ViewBag.ClienteLogado = HttpContext.Session.GetString(SessionKeys.UserName);
                return View(reserva);
            }

            _context.Add(reserva);
            await AtualizarStatusMesaAsync(reserva.IdMesa, "reservada");
            await _context.SaveChangesAsync();
            TempData["Sucesso"] = "Reserva criada com sucesso!";
            return RedirectToAction(nameof(Details), new { id = reserva.IdReserva });
        }
        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!UsuarioEhAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null) return NotFound();
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return NotFound();
            PopulateDropDowns(reserva.IdMesa, reserva.IdMesa);
            PrepararRestricoesDeReserva();
            ViewBag.Clientes = new SelectList(_context.Clientes.OrderBy(c => c.Nome), "IdCliente", "Nome", reserva.IdCliente);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReserva,DataReserva,Horario,QuantidadePessoas,Status,IdCliente,IdMesa,PedidoReserva")] Reserva reserva)
        {
            if (!UsuarioEhAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            if (id != reserva.IdReserva) return NotFound();
            var reservaOriginal = await _context.Reservas.AsNoTracking().FirstOrDefaultAsync(r => r.IdReserva == id);
            if (reservaOriginal == null) return NotFound();

            ValidarReserva(reserva, reserva.IdReserva);

            if (!ModelState.IsValid)
            {
                PopulateDropDowns(reserva.IdMesa, reservaOriginal.IdMesa);
                PrepararRestricoesDeReserva();
                ViewBag.Clientes = new SelectList(_context.Clientes.OrderBy(c => c.Nome), "IdCliente", "Nome", reserva.IdCliente);
                return View(reserva);
            }

            _context.Update(reserva);
            if (reservaOriginal.IdMesa != reserva.IdMesa)
            {
                await AtualizarStatusMesaAsync(reservaOriginal.IdMesa, "disponivel");
            }

            await AtualizarStatusMesaAsync(reserva.IdMesa, StatusMantemMesaReservada(reserva.Status) ? "reservada" : "disponivel");
            await _context.SaveChangesAsync();
            TempData["Sucesso"] = "Reserva atualizada!";
            return RedirectToAction(nameof(Admin));
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!UsuarioEhAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

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
            if (!UsuarioEhAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                await AtualizarStatusMesaAsync(reserva.IdMesa, "disponivel");
            }
            await _context.SaveChangesAsync();
            TempData["Sucesso"] = "Reserva removida.";
            return RedirectToAction(nameof(Admin));
        }

        private void PopulateDropDowns(int? idMesa = null, int? mesaAtualId = null)
        {
            var mesas = _context.Mesas
                .Where(m => m.Status == "disponivel" || (mesaAtualId.HasValue && m.IdMesa == mesaAtualId.Value))
                .OrderBy(m => m.Numero)
                .ToList();

            ViewData["IdMesa"] = new SelectList(mesas, "IdMesa", "Numero", idMesa);
            ViewData["Status"] = new SelectList(new[] { "pendente", "confirmada", "cancelada", "concluida" });
        }

        private void ValidarReserva(Reserva reserva, int? reservaAtualId = null)
        {
            var hoje = DateOnly.FromDateTime(DateTime.Now);
            var dataReserva = DateOnly.FromDateTime(reserva.DataReserva);
            var agora = DateTime.Now.TimeOfDay;
            var minimoHoje = ArredondarParaProximoIntervalo(agora.Add(TimeSpan.FromHours(1)));

            if (minimoHoje > HorarioEncerramento && dataReserva == hoje)
            {
                ModelState.AddModelError("DataReserva", "Nao ha mais horarios disponiveis para hoje. Escolha uma nova data.");
            }

            if (dataReserva < hoje)
            {
                ModelState.AddModelError("DataReserva", "A reserva não pode ser feita para uma data anterior ao dia atual.");
            }

            if (reserva.Horario < HorarioAbertura)
            {
                ModelState.AddModelError("Horario", $"O primeiro horário disponível para reservas é às {HorarioAbertura:hh\\:mm}.");
            }

            if (reserva.Horario > HorarioEncerramento)
            {
                ModelState.AddModelError("Horario", $"O último horário permitido para reservas é às {HorarioEncerramento:hh\\:mm}.");
            }

            if (dataReserva == hoje && reserva.Horario < minimoHoje)
            {
                ModelState.AddModelError("Horario", $"Para reservas no dia atual, escolha um horário a partir de {minimoHoje:hh\\:mm}.");
            }

            var mesaSelecionada = _context.Mesas.FirstOrDefault(m => m.IdMesa == reserva.IdMesa);

            if (mesaSelecionada == null)
            {
                ModelState.AddModelError("IdMesa", "Selecione uma mesa válida.");
                return;
            }

            var mesaDaReservaAtual = reservaAtualId.HasValue
                ? _context.Reservas.AsNoTracking().Where(r => r.IdReserva == reservaAtualId.Value).Select(r => r.IdMesa).FirstOrDefault()
                : 0;

            if (mesaSelecionada.Status != "disponivel" && mesaSelecionada.IdMesa != mesaDaReservaAtual)
            {
                ModelState.AddModelError("IdMesa", "Esta mesa esta reservada ate ser liberada pelo restaurante.");
            }

            if (reserva.QuantidadePessoas > mesaSelecionada.Capacidade)
            {
                ModelState.AddModelError("QuantidadePessoas", $"A mesa {mesaSelecionada.Numero} suporta até {mesaSelecionada.Capacidade} pessoas.");
            }

            var horarioOcupado = _context.Reservas.Any(r =>
                (!reservaAtualId.HasValue || r.IdReserva != reservaAtualId.Value) &&
                r.IdMesa == reserva.IdMesa &&
                r.DataReserva.Date == reserva.DataReserva.Date &&
                r.Horario == reserva.Horario &&
                r.Status != "cancelada" &&
                r.Status != "concluida");

            if (horarioOcupado)
            {
                ModelState.AddModelError("Horario", "Esta mesa já está reservada para a data e horário informados.");
            }
        }

        private void PrepararRestricoesDeReserva()
        {
            var minimoHoje = ArredondarParaProximoIntervalo(DateTime.Now.TimeOfDay.Add(TimeSpan.FromHours(1)));

            ViewBag.DataMinima = DateTime.Today.ToString("yyyy-MM-dd");
            ViewBag.HorarioAbertura = HorarioAbertura.ToString(@"hh\:mm");
            ViewBag.HorarioEncerramento = HorarioEncerramento.ToString(@"hh\:mm");
            ViewBag.HorarioMinimoHoje = (minimoHoje > HorarioEncerramento ? HorarioEncerramento : minimoHoje).ToString(@"hh\:mm");
        }

        private static TimeSpan ArredondarParaProximoIntervalo(TimeSpan horario)
        {
            var minutos = horario.Minutes;
            var resto = minutos % 30;

            if (resto == 0)
            {
                return horario;
            }

            return horario.Add(TimeSpan.FromMinutes(30 - resto));
        }

        private bool UsuarioLogado()
        {
            return HttpContext.Session.GetInt32(SessionKeys.UserId).HasValue;
        }

        private bool UsuarioEhAdmin()
        {
            return HttpContext.Session.GetString(SessionKeys.UserRole) == "Administrador";
        }

        private async Task AtualizarStatusMesaAsync(int idMesa, string status)
        {
            var mesa = await _context.Mesas.FindAsync(idMesa);
            if (mesa != null)
            {
                mesa.Status = status;
            }
        }

        private static bool StatusMantemMesaReservada(string status)
        {
            return status is "pendente" or "confirmada";
        }
    }
}
