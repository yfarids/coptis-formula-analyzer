using CoptisFormulaAnalyzer.Core.Interfaces;
using CoptisFormulaAnalyzer.Application.Services;
using CoptisFormulaAnalyzer.Infrastructure.Data;
using CoptisFormulaAnalyzer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

// Add file watcher as hosted service
builder.Services.AddHostedService<FileWatcherService>();

var app = builder.Build();

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

// Start file watcher - commented out since FileImportService is now scoped
// File watching functionality can be accessed through the UI
// var fileImportService = app.Services.GetRequiredService<FileImportService>();
// fileImportService.StartFileWatcher();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FormulaAnalyzerContext>();
    context.Database.EnsureCreated();
}

app.Run();
