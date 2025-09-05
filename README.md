# Coptis Formula Analyzer

A comprehensive .NET Blazor Server application for analyzing cosmetic formulas, managing raw materials, and tracking substance usage.

## Features

### Core Functionality
- **Formula Import/Export**: Import formulas from JSON files with automatic validation
- **Raw Material Management**: Track prices and automatically recalculate formula costs
- **Substance Analysis**: Analyze substances by total weight and usage frequency
- **File Watching**: Automatic import of JSON files placed in the watch folder
- **Price Tracking**: Monitor when formula costs change due to raw material price updates

### Technical Features
- **Clean Architecture**: Follows SOLID principles with separation of concerns
- **Entity Framework Core**: SQL Server database with automatic migrations
- **Blazor Server**: Real-time UI updates and responsive design
- **Bootstrap**: Modern, responsive UI components
- **Logging**: Comprehensive logging throughout the application
- **Error Handling**: Robust error handling with user-friendly messages

## Project Structure

```
CoptisFormulaAnalyzer/
├── src/
│   ├── CoptisFormulaAnalyzer.Core/           # Domain entities and interfaces
│   ├── CoptisFormulaAnalyzer.Application/    # Business logic and services
│   ├── CoptisFormulaAnalyzer.Infrastructure/ # Data access and repositories
│   └── CoptisFormulaAnalyzer.Web/           # Blazor Server UI
└── sample-formulas/                         # Sample JSON files for testing
```

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server LocalDB (or SQL Server)
- Visual Studio 2022 or VS Code

### Setup Instructions

1. **Clone and Navigate**
   ```bash
   cd CoptisFormulaAnalyzer
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **Update Database Connection**
   Edit `src/CoptisFormulaAnalyzer.Web/appsettings.json` if needed:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CoptisFormulaAnalyzer;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

4. **Run the Application**
   ```bash
   cd src/CoptisFormulaAnalyzer.Web
   dotnet run
   ```

5. **Access the Application**
   Open your browser and navigate to `https://localhost:5001` or `http://localhost:5000`

## Usage

### Import Formulas

#### Method 1: Web Interface
1. Navigate to the main page
2. Paste JSON formula data in the text area
3. Click "Import Formula"

#### Method 2: File Watching
1. Copy JSON files to the auto-import folder (displayed in the UI)
2. Files are automatically processed and moved to Processed or Errors folders

### JSON Format
```json
{
  "name": "Formula Name",
  "components": [
    {
      "name": "Component Name",
      "weight": 100.0,
      "price": 5.0
    }
  ]
}
```

### Manage Raw Materials
- View all raw materials and their current prices
- Update prices using the "Update Price" button
- Formula costs are automatically recalculated when prices change

### Analyze Substances
- View substances ordered by total weight across all formulas
- View substances ordered by usage frequency (number of formulas)
- Monitor which substances are most commonly used

## Database Schema

### Tables
- **Formulas**: Store formula information (name, total weight, total cost)
- **RawMaterials**: Store raw material information (name, price per kg)
- **FormulaComponents**: Junction table linking formulas to raw materials with quantities

### Key Features
- Automatic cost calculation based on weight and raw material prices
- Unique constraints on formula and raw material names
- Cascade delete for formula components
- Precision decimal handling for weights and costs

## Architecture

### Clean Architecture Layers
1. **Core**: Domain entities, DTOs, and interfaces
2. **Application**: Business logic, services, and use cases
3. **Infrastructure**: Data access, repositories, and external services
4. **Web**: Blazor components, pages, and UI logic

### Key Design Patterns
- Repository Pattern for data access
- Dependency Injection for loose coupling
- Service Layer for business logic
- DTOs for data transfer between layers

## Sample Data

The `sample-formulas` folder contains example JSON files:
- `CitralSoap.json`: Sample citral-based soap formula
- `GeraniolSoap.json`: Sample geraniol-based soap formula

You can use these files to test the import functionality.

## Development

### Adding New Features
1. Define entities in the Core project
2. Create repositories in Infrastructure
3. Implement services in Application
4. Add UI components in Web project

### Database Migrations
```bash
cd src/CoptisFormulaAnalyzer.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../CoptisFormulaAnalyzer.Web
dotnet ef database update --startup-project ../CoptisFormulaAnalyzer.Web
```

## Troubleshooting

### Common Issues
1. **Database Connection**: Ensure SQL Server LocalDB is installed and running
2. **File Permissions**: Ensure the application has write access to the import folder
3. **JSON Format**: Validate JSON format using the sample files as reference

### Logs
Application logs are written to the console and can help diagnose issues with:
- Formula import failures
- Database connection problems
- File watching issues

## License

This project is developed as a technical test for Coptis and demonstrates modern .NET development practices.
