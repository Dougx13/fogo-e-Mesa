using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;

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
    }
}
