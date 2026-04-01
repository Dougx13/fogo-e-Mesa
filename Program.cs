using Microsoft.EntityFrameworkCore;
using RestauranteApp.Data;

var builder = WebApplication.CreateBuilder(args);

// ── MVC ─────────────────────────────────────────
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// ── Entity Framework + SQL Server ───────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

var app = builder.Build();

// ── Pipeline ────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

// ── Rota padrão ────────────────────────────────
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
