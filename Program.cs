using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using DesktopApp.Core.Services;
using DesktopApp.Core.Entities;
using DesktopApp.Infrastructure.Data.Context;
using DesktopApp.Presentation.Forms;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace DesktopApp;

static class Program
{
    [STAThread]
    static void Main()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        // Use connectionString as needed

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        try
        {
            Log.Information("Starting application");
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            ConfigureServices(services);

            using var serviceProvider = services.BuildServiceProvider();
            var mainForm = serviceProvider.GetRequiredService<MainForm>();
            Application.Run(mainForm);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application start-up failed");
        }
        finally
        {
            Log.CloseAndFlush();
        }

        Application.ThreadException += (sender, args) =>
        {
            Log.Error(args.Exception, "Unhandled thread exception");
            MessageBox.Show("An unexpected error occurred. Please contact support.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        };

        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            if (args.ExceptionObject is Exception ex)
            {
                Log.Error(ex, "Unhandled domain exception");
                MessageBox.Show("A critical error occurred. The application will close.", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Environment.Exit(1);
        };
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