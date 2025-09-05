# 🧪 Coptis Formula Analyzer - Technical Test Implementation

## 📋 Project Overview

This is a complete .NET 8 Blazor Server application implementing a cosmetic formula analysis system based on the provided technical requirements. The application demonstrates modern software architecture principles, clean code practices, and a comprehensive feature set for managing cosmetic formulations.

## ✨ Features Implemented

### Core Requirements ✅
- ✅ **Formula Import/Export**: JSON-based formula import with validation
- ✅ **Raw Material Management**: Price tracking and automatic cost recalculation
- ✅ **Substance Analysis**: Analysis by weight and usage frequency
- ✅ **File Watching**: Automatic processing of JSON files in watch folder
- ✅ **Database Integration**: SQL Server with Entity Framework Core
- ✅ **Clean Architecture**: SOLID principles, dependency injection

### Additional Features 🚀
- ✅ **Real-time UI Updates**: Blazor Server for responsive interface
- ✅ **Bootstrap Integration**: Modern, responsive design
- ✅ **Comprehensive Logging**: Structured logging throughout the application
- ✅ **Error Handling**: Robust error handling with user feedback
- ✅ **Data Validation**: Input validation and business rule enforcement
- ✅ **Sample Data**: Ready-to-use sample formulas for testing

## 🏗️ Architecture

### Clean Architecture Layers
```
┌─────────────────────────────┐
│       CoptisFormulaAnalyzer.Web      │ ← Blazor Server UI
├─────────────────────────────┤
│    CoptisFormulaAnalyzer.Application │ ← Business Logic
├─────────────────────────────┤
│  CoptisFormulaAnalyzer.Infrastructure│ ← Data Access
├─────────────────────────────┤
│      CoptisFormulaAnalyzer.Core      │ ← Domain Models
└─────────────────────────────┘
```

### Key Components
- **Entities**: Formula, RawMaterial, FormulaComponent
- **Services**: FormulaService, RawMaterialService, FileImportService
- **Repositories**: FormulaRepository, RawMaterialRepository
- **Data Context**: Entity Framework Core with SQL Server

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK
- SQL Server LocalDB (included with Visual Studio)
- PowerShell (for setup script)

### Installation & Run
```powershell
# Navigate to project directory
cd "c:\Users\Yassine\Desktop\Test_technique_coptis\Coptis_FullStack_Test_Technique"

# Run setup script (recommended)
.\setup.ps1

# Or manual setup:
dotnet restore
dotnet build
cd src\CoptisFormulaAnalyzer.Web
dotnet run
```

### Access Application
- 🌐 **HTTPS**: https://localhost:5001
- 🌐 **HTTP**: http://localhost:5000

## 📊 Testing the Application

### Sample Data Import
1. **Web Interface**: Copy/paste JSON from `sample-formulas/CitralSoap.json`
2. **File Watching**: Copy JSON files to the auto-import folder (path shown in UI)

### Expected Results
- **2 Formulas**: CitralSoap (3.40 EUR), GeraniolSoap (2.50 EUR)
- **3 Raw Materials**: CitralEsence (100 EUR/kg), Water (1 EUR/kg), NaturalGlycerin (3 EUR/kg)
- **Substance Analysis**: Ordered by weight and usage frequency

### Key Test Scenarios
1. ✅ Import valid formulas via web interface
2. ✅ Import formulas via file watching
3. ✅ Update raw material prices and see cost recalculation
4. ✅ Delete formulas
5. ✅ View substance analysis reports

## 🎯 Technical Highlights

### SOLID Principles Implementation
- **Single Responsibility**: Each class has one clear purpose
- **Open/Closed**: Extensible service architecture
- **Liskov Substitution**: Interface-based design
- **Interface Segregation**: Focused, minimal interfaces
- **Dependency Inversion**: Dependency injection throughout

### Best Practices
- **Repository Pattern**: Clean data access abstraction
- **Service Layer**: Business logic separation
- **DTO Pattern**: Data transfer between layers
- **Configuration Management**: appsettings.json for environment-specific settings
- **Error Handling**: Try-catch with logging and user feedback
- **Async/Await**: Non-blocking operations throughout

### Data Design
- **Normalized Schema**: Proper foreign key relationships
- **Unique Constraints**: Prevent duplicate formulas/materials
- **Decimal Precision**: Accurate financial calculations
- **Audit Fields**: Created/modified timestamps

## 📁 Project Structure

```
CoptisFormulaAnalyzer/
├── 📁 src/
│   ├── 📁 CoptisFormulaAnalyzer.Core/
│   │   ├── 📁 Entities/           # Domain models
│   │   ├── 📁 DTOs/              # Data transfer objects
│   │   └── 📁 Interfaces/        # Service contracts
│   ├── 📁 CoptisFormulaAnalyzer.Application/
│   │   └── 📁 Services/          # Business logic
│   ├── 📁 CoptisFormulaAnalyzer.Infrastructure/
│   │   ├── 📁 Data/              # EF Context
│   │   └── 📁 Repositories/      # Data access
│   └── 📁 CoptisFormulaAnalyzer.Web/
│       ├── 📁 Pages/             # Blazor pages
│       ├── 📁 Shared/            # Shared components
│       └── 📁 wwwroot/           # Static files
├── 📁 sample-formulas/           # Test data
├── 📄 README.md                  # Main documentation
├── 📄 SETUP.md                   # Setup instructions
└── 📄 setup.ps1                  # Automated setup script
```

## 🔧 Configuration

### Database Connection
Default: SQL Server LocalDB
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CoptisFormulaAnalyzer;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### File Import Folder
Automatically created at: `[Application Directory]/ImportFolder/`

## 🐛 Troubleshooting

### Common Issues & Solutions

1. **Database Connection Issues**
   ```bash
   # Ensure LocalDB is running
   sqllocaldb start mssqllocaldb
   
   # Recreate database if needed
   dotnet ef database drop --force
   dotnet ef database update
   ```

2. **Build Issues**
   ```bash
   dotnet clean
   dotnet restore
   dotnet build
   ```

3. **Port Conflicts**
   - Edit `Properties/launchSettings.json` to change ports
   - Or use: `dotnet run --urls "https://localhost:7001;http://localhost:7000"`

## 📈 Performance Considerations

- **Entity Framework**: Includes for related data loading
- **Async Operations**: Non-blocking I/O throughout
- **Decimal Precision**: Accurate financial calculations
- **Memory Management**: Proper disposal patterns
- **File Watching**: Efficient file system monitoring

## 🔒 Security Features

- **Input Validation**: JSON validation and sanitization
- **SQL Injection Prevention**: Entity Framework parameterized queries
- **XSS Prevention**: Blazor automatic HTML encoding
- **Error Information**: Safe error messages in production

## 📦 Dependencies

### Core Frameworks
- .NET 8.0
- Entity Framework Core 8.0
- Blazor Server
- Bootstrap 5.1

### Key Packages
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.Extensions.Logging
- System.Text.Json

## 🎯 Future Enhancements

### Potential Improvements
- **Authentication/Authorization**: User management
- **API Layer**: REST API for external integrations
- **Export Features**: Excel/PDF export functionality
- **Advanced Search**: Full-text search capabilities
- **Audit Logging**: Change tracking and history
- **Unit Testing**: Comprehensive test coverage
- **Docker Support**: Containerization for deployment

## 📞 Technical Contact

This application was developed as a technical test demonstrating:
- Modern .NET development practices
- Clean architecture implementation
- Full-stack web development skills
- Database design and Entity Framework expertise
- UI/UX design with Blazor and Bootstrap

---

### 🎉 Ready to Run!

The application is fully functional and ready for evaluation. Simply run `.\setup.ps1` and start exploring the cosmetic formula analysis capabilities!

**Thank you for the opportunity to demonstrate these technical skills! 🚀**
