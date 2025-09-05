# Development Environment Setup

## Quick Start

1. **Run the setup script:**
   ```powershell
   .\setup.ps1
   ```

2. **Or manually:**
   ```bash
   dotnet restore
   dotnet build
   cd src\CoptisFormulaAnalyzer.Web
   dotnet run
   ```

3. **Access the application:**
   - https://localhost:5001
   - http://localhost:5000

## Testing the Application

### Sample Data
Use the JSON files in the `sample-formulas` folder to test the import functionality.

### Test Steps
1. Copy and paste the content from `sample-formulas\CitralSoap.json` into the import text area
2. Click "Import Formula"
3. Repeat with `GeraniolSoap.json`
4. Observe the formulas, raw materials, and substance analysis sections

### File Import Testing
1. Note the auto-import folder path displayed in the UI
2. Copy JSON files from `sample-formulas` to the auto-import folder
3. Files should be automatically processed and moved to subfolders

## Troubleshooting

### Database Issues
If you encounter database connection issues:
1. Ensure SQL Server LocalDB is installed
2. Check the connection string in `appsettings.json`
3. Run: `dotnet ef database update`

### Build Issues
If you encounter build issues:
1. Ensure .NET 8.0 SDK is installed
2. Run: `dotnet clean` then `dotnet restore` then `dotnet build`

### Port Issues
If ports 5000/5001 are in use:
1. Update `launchSettings.json` to use different ports
2. Or kill processes using those ports
