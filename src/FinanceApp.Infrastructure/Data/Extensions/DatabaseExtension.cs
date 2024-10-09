using FinanceApp.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinanceApp.Infrastructure.Data.Extensions;

public static class DatabaseExtension
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Aplicar migraciones
        await context.Database.MigrateAsync();

        // Ejecutar el seeding de la base de datos
        await SeedAsync(scope.ServiceProvider);
    }
    
    private static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        // Obtén el loggerFactory, context y userManager
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await SeedUserAsync(context, userManager, roleManager, loggerFactory);
    }
    
    private static async Task SeedUserAsync(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILoggerFactory loggerFactory)
    {
        await InitialData.LoadDataAsync(context, userManager, roleManager, loggerFactory);
    }
}