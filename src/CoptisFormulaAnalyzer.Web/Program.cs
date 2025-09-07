using CoptisFormulaAnalyzer.Core.Interfaces;
using CoptisFormulaAnalyzer.Application.Services;
using CoptisFormulaAnalyzer.Infrastructure.Data;
using CoptisFormulaAnalyzer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

// Configure Serilog early in the pipeline
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
        .Build())
    .CreateLogger();


Log.Information("Starting CoptisFormulaAnalyzer Web Application");

var builder = WebApplication.CreateBuilder(args);

// Replace default logging with Serilog
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add Entity Framework
builder.Services.AddDbContext<FormulaAnalyzerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repositories
builder.Services.AddScoped<IFormulaRepository, FormulaRepository>();
builder.Services.AddScoped<IRawMaterialRepository, RawMaterialRepository>();

// Add services
builder.Services.AddScoped<IFormulaService, FormulaService>();
builder.Services.AddScoped<IRawMaterialService, RawMaterialService>();
builder.Services.AddScoped<FileImportService>();

// Add simple notification service (no SignalR needed for Blazor Server)
builder.Services.AddScoped<CoptisFormulaAnalyzer.Core.Interfaces.INotificationService, CoptisFormulaAnalyzer.Application.Services.SimpleNotificationService>();

// Add file watcher as hosted service
builder.Services.AddHostedService<FileWatcherService>();

var app = builder.Build();

// Add Serilog request logging
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FormulaAnalyzerContext>();
    context.Database.EnsureCreated();
}

app.Run();
