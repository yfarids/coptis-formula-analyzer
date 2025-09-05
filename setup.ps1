# Coptis Formula Analyzer Setup Script

Write-Host "Setting up Coptis Formula Analyzer..." -ForegroundColor Green

# Navigate to solution directory
Set-Location "c:\Users\Yassine\Desktop\Test_technique_coptis\Coptis_FullStack_Test_Technique"

# Restore NuGet packages
Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore

# Build the solution
Write-Host "Building the solution..." -ForegroundColor Yellow
dotnet build

# Navigate to web project
Set-Location "src\CoptisFormulaAnalyzer.Web"

# Run Entity Framework migrations (create database)
Write-Host "Setting up database..." -ForegroundColor Yellow
dotnet ef database update

# Start the application
Write-Host "Starting the application..." -ForegroundColor Green
Write-Host "The application will be available at:" -ForegroundColor Cyan
Write-Host "  - HTTPS: https://localhost:5001" -ForegroundColor Cyan
Write-Host "  - HTTP: http://localhost:5000" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press Ctrl+C to stop the application" -ForegroundColor Yellow

dotnet run
