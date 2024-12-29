using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using DesktopApp.Core.Services;
using DesktopApp.Core.Entities;
using DesktopApp.Infrastructure.Data.Context;
using DesktopApp.Presentation.Forms;

namespace DesktopApp;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var services = new ServiceCollection();
        ConfigureServices(services);

        using var serviceProvider = services.BuildServiceProvider();
        var mainForm = serviceProvider.GetRequiredService<MainForm>();
        Application.Run(mainForm);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Database context
        services.AddDbContext<DatabaseContext>();

        // Services
        services.AddScoped<ProductService>();
        services.AddScoped<StockMovementService>();

        // Forms
        services.AddTransient<MainForm>();
        services.AddTransient<ProductForm>();
        services.AddTransient<StockMovementForm>();

        // Initialize database
        using var context = services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
        context.Database.EnsureCreated();
    }
} 