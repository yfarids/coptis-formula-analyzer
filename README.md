# 🧪 Analyseur de Formules Coptis

Une application .NET Blazor Server complète pour l'analyse de formules cosmétiques, la gestion des matières premières et le suivi de l'utilisation des substances.

## ⚡ Démarrage rapide pour évaluateur

**Prérequis minimum :** .NET 8.0 SDK + SQL Server LocalDB (ou SQL Express)

```powershell
# 1. Extraire le ZIP et naviguer dans le dossier
cd "chemin\vers\CoptisFormulaAnalyzer"

# 2. Restaurer les dépendances et compiler
dotnet restore
dotnet build

# 3. Lancer l'application
cd src\CoptisFormulaAnalyzer.Web
dotnet run

# 4. Ouvrir https://localhost:5001 dans le navigateur
```

**Temps total d'installation :** ~5-10 minutes | **Test avec données d'exemple incluses**

---

## 📋 Vue d'ensemble du projet

Cette application .NET 8 Blazor Server implémente un système complet d'analyse de formules cosmétiques basé sur les exigences techniques fournies. Elle démontre les principes d'architecture logicielle moderne, les pratiques de code propre et un ensemble de fonctionnalités complètes pour la gestion des formulations cosmétiques.

## ✨ Fonctionnalités implémentées

### 🎯 Exigences principales ✅

#### 1. **Import de formules via interface web**
- ✅ Import JSON via zone de texte avec validation en temps réel
- ✅ Gestion des erreurs avec messages utilisateur détaillés
- ✅ Validation automatique du format JSON
- ✅ Calcul automatique des poids et coûts totaux

#### 2. **Import automatique via surveillance de dossier**
- ✅ Surveillance automatique d'un dossier local configurable
- ✅ Traitement automatique des fichiers JSON déposés
- ✅ Organisation automatique : Processed/Errors
- ✅ Service en arrière-plan (FileWatcherService)

#### 3. **Visualisation des formules**
- ✅ Liste complète avec nom, poids total (g) et coût total (EUR)
- ✅ Mise en évidence des formules avec prix mis à jour
- ✅ Rafraîchissement automatique toutes les 5 secondes
- ✅ Bouton de rafraîchissement manuel

#### 4. **Suppression de formules**
- ✅ Suppression sécurisée avec confirmation utilisateur
- ✅ Nettoyage automatique des matières premières orphelines
- ✅ Logging complet des opérations

#### 5. **Analyse des substances les plus utilisées**
- ✅ **Par poids total** : Classement par poids cumulé (g)
- ✅ **Par fréquence d'utilisation** : Classement par nombre de formules [BONUS]
- ✅ Tableaux détaillés avec poids total et nombre de formules

#### 6. **Mise à jour des prix des matières premières**
- ✅ Interface d'édition en ligne des prix (EUR/kg)
- ✅ Validation des entrées avec 2 décimales
- ✅ Recalcul automatique des coûts de toutes les formules

#### 7. **Mise en évidence des formules mises à jour**
- ✅ Highlighting visuel (classe CSS `table-warning`)
- ✅ Indicateur persistant jusqu'au prochain rafraîchissement
- ✅ Feedback utilisateur lors des mises à jour

### 🚀 Fonctionnalités supplémentaires

#### Interface utilisateur avancée
- ✅ **Blazor Server** : Mises à jour en temps réel
- ✅ **Bootstrap 5.1** : Design moderne et responsive
- ✅ **Icônes Bootstrap** : Interface intuitive
- ✅ **Messages de feedback** : Succès/erreur en temps réel

#### Robustesse et fiabilité
- ✅ **Gestion d'erreurs complète** : Try-catch avec logging
- ✅ **Validation des données** : JSON schema validation
- ✅ **Logging structuré** : Microsoft.Extensions.Logging
- ✅ **Concurrence** : Gestion des accès simultanés
- ✅ **Tests unitaires** : 56 tests couvrant services, entités et DTOs

#### Architecture technique
- ✅ **Clean Architecture** : Séparation des responsabilités
- ✅ **Principes SOLID** : Code maintenable et extensible
- ✅ **Entity Framework Core** : ORM avec migrations automatiques
- ✅ **Injection de dépendances** : Couplage faible
- ✅ **Tests d'intégration** : Repositories avec base en mémoire

## 🏗️ Architecture technique

### Structure en couches (Clean Architecture)
```
┌─────────────────────────────────────────┐
│    CoptisFormulaAnalyzer.Web            │ ← Interface Blazor Server
│    - Pages/Components Blazor             │
│    - Controllers API                     │
│    - Configuration & DI                  │
├─────────────────────────────────────────┤
│    CoptisFormulaAnalyzer.Application    │ ← Logique métier
│    - Services (FormulaService, etc.)     │
│    - Règles de gestion                   │
│    - Orchestration des cas d'usage       │
├─────────────────────────────────────────┤
│    CoptisFormulaAnalyzer.Infrastructure │ ← Accès aux données
│    - Repositories (EF Core)              │
│    - Context de base de données          │
│    - Migrations & configuration DB       │
├─────────────────────────────────────────┤
│    CoptisFormulaAnalyzer.Core          │ ← Modèles de domaine
│    - Entités (Formula, RawMaterial)     │
│    - DTOs & interfaces                   │
│    - Règles métier fondamentales         │
└─────────────────────────────────────────┘
```

### Composants clés

#### Entités de domaine
- **Formula** : Formule avec nom, poids total, coût total
- **RawMaterial** : Matière première avec nom et prix/kg
- **FormulaComponent** : Liaison formule-matière avec quantité

#### Services métier
- **FormulaService** : Gestion complète des formules
- **RawMaterialService** : Gestion des matières premières
- **FileImportService** : Import JSON et surveillance fichiers

#### Accès aux données
- **FormulaRepository** : CRUD formules avec EF Core
- **RawMaterialRepository** : CRUD matières premières
- **FormulaAnalyzerContext** : Contexte EF avec configuration

## 🚀 Installation et démarrage

### ⚡ Installation rapide (pour évaluateur)

Si vous avez reçu cette application en ZIP, suivez ces étapes :

#### Étape 1 : Prérequis à installer
- **.NET 8.0 SDK** (obligatoire)
  ```powershell
  # Vérifier si installé
  dotnet --version
  
  # Si absent, télécharger : https://dotnet.microsoft.com/download/dotnet/8.0
  ```

- **Base de données** (choisir une option) :
  - **SQL Server LocalDB** (recommandé) : https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb
  - **SQL Server Express** (gratuit) : https://www.microsoft.com/en-us/sql-server/sql-server-downloads
  - **Visual Studio** (inclut LocalDB automatiquement)

#### Étape 2 : Configuration après extraction du ZIP
```powershell
# 1. Naviguer vers le dossier extrait
cd "chemin\vers\CoptisFormulaAnalyzer"

# 2. Restaurer les dépendances
dotnet restore

# 3. Compiler l'application
dotnet build

# 4. Lancer l'application
cd src\CoptisFormulaAnalyzer.Web
dotnet run
```

#### Étape 3 : Accès à l'application
- 🌐 **HTTPS** : https://localhost:5001
- 🌐 **HTTP** : http://localhost:5000

#### Étape 4 : Test avec données d'exemple
1. **Import web** : Copier le contenu de `sample-formulas\CitralSoap.json` dans l'interface
2. **Import automatique** : Copier des fichiers JSON dans le dossier de surveillance (affiché dans l'UI)

### 🔧 Configuration alternative de base de données

Si vous n'utilisez pas LocalDB, modifier `src\CoptisFormulaAnalyzer.Web\appsettings.json` :
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=VOTRE_SERVEUR;Database=CoptisFormulaAnalyzer;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

## 📊 Test de l'application

### Données d'exemple fournies
Le dossier `sample-formulas` contient :
- **CitralSoap.json** : Savon au citral (3 matières premières)
- **GeraniolSoap.json** : Savon au géraniol
- **1_CitralSoap.json** : Variante du savon citral

### Scénarios de test

#### Import via interface web
1. Copier le contenu de `sample-formulas/CitralSoap.json`
2. Coller dans la zone de texte
3. Cliquer "Import Formula"
4. Vérifier l'affichage dans la liste des formules

#### Import automatique
1. Noter le chemin du dossier de surveillance (affiché dans l'UI)
2. Copier les fichiers JSON dans ce dossier
3. Observer le traitement automatique
4. Vérifier le déplacement vers Processed/Errors

#### Gestion des prix
1. Aller dans la section "Raw Materials"
2. Cliquer "Update Price" sur une matière première
3. Modifier le prix et sauvegarder
4. Observer la mise à jour des coûts de formules

### Résultats attendus
- **Formules** : CitralSoap (≈3.40 EUR), GeraniolSoap (≈2.50 EUR)
- **Matières premières** : CitralEsence (100 EUR/kg), Water (1 EUR/kg), NaturalGlycerin (3 EUR/kg)
- **Analyse substances** : Classement par poids et fréquence

## 💾 Schéma de base de données

### Tables principales

#### **Formulas**
```sql
Id (int, PK, Identity)
Name (nvarchar(255), Unique, Required)
TotalWeight (decimal(18,2))
TotalCost (decimal(18,2))
CreatedDate (datetime2)
LastModifiedDate (datetime2, nullable)
IsPriceUpdated (bit)
```

#### **RawMaterials**
```sql
Id (int, PK, Identity)
Name (nvarchar(255), Unique, Required)
PricePerKg (decimal(18,2))
CreatedDate (datetime2)
LastModifiedDate (datetime2, nullable)
```

#### **FormulaComponents**
```sql
Id (int, PK, Identity)
FormulaId (int, FK → Formulas.Id, Cascade Delete)
RawMaterialId (int, FK → RawMaterials.Id, Restrict Delete)
WeightInGrams (decimal(18,2))
Cost (decimal(18,2))
```

### Contraintes et règles
- **Unicité** : Noms de formules et matières premières uniques
- **Précision** : Calculs avec 2 décimales obligatoires
- **Intégrité** : Suppression en cascade pour composants
- **Performance** : Index sur les clés étrangères

## 🔧 Configuration technique

### Chaîne de connexion (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CoptisFormulaAnalyzer;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "ImportFolder": "./ImportFolder",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Dossier de surveillance
- **Par défaut** : `[Répertoire application]/ImportFolder/`
- **Configurable** : Via appsettings.json
- **Création automatique** : Si n'existe pas

## 📁 Structure détaillée du projet

```
CoptisFormulaAnalyzer/
├── 📁 src/
│   ├── 📁 CoptisFormulaAnalyzer.Core/
│   │   ├── 📁 Entities/                   # Modèles de domaine
│   │   │   ├── Formula.cs                 # Entité Formule
│   │   │   ├── RawMaterial.cs            # Entité Matière première
│   │   │   └── FormulaComponent.cs        # Entité Composant
│   │   ├── 📁 DTOs/                      # Objets de transfert
│   │   │   ├── FormulaDto.cs             # DTO Import/Export
│   │   │   └── SubstanceAnalysisDto.cs   # DTO Analyse
│   │   └── 📁 Interfaces/                # Contrats de service
│   │       ├── IFormulaService.cs
│   │       └── IRawMaterialService.cs
│   ├── 📁 CoptisFormulaAnalyzer.Application/
│   │   └── 📁 Services/                  # Logique métier
│   │       ├── FormulaService.cs         # Service formules
│   │       ├── RawMaterialService.cs     # Service matières premières
│   │       ├── FileImportService.cs      # Service import JSON
│   │       └── FileWatcherService.cs     # Service surveillance
│   ├── 📁 CoptisFormulaAnalyzer.Infrastructure/
│   │   ├── 📁 Data/                      # Contexte EF
│   │   │   └── FormulaAnalyzerContext.cs
│   │   ├── 📁 Migrations/                # Migrations EF
│   │   └── 📁 Repositories/              # Accès données
│   │       ├── FormulaRepository.cs
│   │       └── RawMaterialRepository.cs
│   ├── 📁 CoptisFormulaAnalyzer.Tests/   # Tests unitaires
│   │   ├── 📁 Services/                  # Tests des services
│   │   ├── 📁 Entities/                  # Tests des entités
│   │   ├── 📁 DTOs/                      # Tests des DTOs
│   │   └── 📁 Repositories/              # Tests d'intégration
│   └── 📁 CoptisFormulaAnalyzer.Web/
│       ├── 📁 Pages/                     # Pages Blazor
│       │   ├── Index.razor               # Page principale
│       │   └── _Host.cshtml             # Host Blazor
│       ├── 📁 Shared/                    # Composants partagés
│       │   └── MainLayout.razor
│       ├── 📁 wwwroot/css/              # Styles CSS
│       ├── Program.cs                    # Configuration DI
│       └── appsettings.json             # Configuration
├── 📁 sample-formulas/                   # Données de test
│   ├── CitralSoap.json
│   ├── GeraniolSoap.json
│   └── 1_CitralSoap.json
├── 📄 README.md                          # Documentation principale
└── 📄 CoptisFormulaAnalyzer.sln         # Solution Visual Studio
```

## 🔒 Sécurité et validation

### Validation des données
- **JSON Schema** : Validation structure des formules
- **Data Annotations** : Validation entités EF
- **Business Rules** : Validation métier dans services
- **Input Sanitization** : Protection contre injection

### Sécurité application
- **Entity Framework** : Protection SQL injection
- **Blazor Server** : Protection XSS automatique
- **Error Handling** : Messages d'erreur sécurisés
- **Logging** : Traçabilité des opérations

## 🐛 Dépannage

### Problèmes courants

#### 1. Erreurs de connexion base de données
```powershell
# Vérifier si LocalDB est installé
sqllocaldb info

# Si LocalDB n'est pas installé, télécharger SQL Server Express LocalDB depuis :
# https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb

# Vérifier et démarrer LocalDB
sqllocaldb start mssqllocaldb
sqllocaldb info mssqllocaldb

# Recréer la base si nécessaire
dotnet ef database drop --force --startup-project src\CoptisFormulaAnalyzer.Web
dotnet ef database update --startup-project src\CoptisFormulaAnalyzer.Web
```

#### Alternative : Utiliser une autre base de données
Si LocalDB n'est pas disponible, modifier la chaîne de connexion dans `appsettings.json` :
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=CoptisFormulaAnalyzer;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

#### 2. Problèmes de build
```powershell
dotnet clean
dotnet restore
dotnet build --no-restore
```

#### 3. Conflits de ports
```powershell
# Utiliser des ports différents
dotnet run --urls "https://localhost:7001;http://localhost:7000"
```

#### 4. Permissions dossier surveillance
- Vérifier droits écriture sur dossier ImportFolder
- Exécuter en tant qu'administrateur si nécessaire

### Logs et diagnostic
- **Console** : Logs temps réel pendant exécution
- **Debug** : Messages détaillés en mode développement
- **Erreurs** : Stack traces complètes pour diagnostic

## 📦 Contenu de la livraison

### Fichiers inclus dans le ZIP
```
CoptisFormulaAnalyzer/
├── 📁 src/                          # Code source complet
│   ├── 📁 CoptisFormulaAnalyzer.Core/          # Domaine et interfaces
│   ├── 📁 CoptisFormulaAnalyzer.Application/   # Services métier
│   ├── 📁 CoptisFormulaAnalyzer.Infrastructure/ # Accès données
│   ├── 📁 CoptisFormulaAnalyzer.Web/          # Interface Blazor
│   └── 📁 CoptisFormulaAnalyzer.Tests/        # Tests unitaires (56 tests)
├── 📁 sample-formulas/             # Données de test (3 fichiers JSON)
├── 📄 README.md                    # Documentation complète (ce fichier)
└── 📄 CoptisFormulaAnalyzer.sln   # Solution Visual Studio
```

### Résultats attendus après installation
Une fois l'application lancée, l'évaluateur devrait voir :
- ✅ **Interface web** fonctionnelle à https://localhost:5001
- ✅ **Import de formules** via JSON avec validation
- ✅ **Liste des formules** avec calculs automatiques des coûts
- ✅ **Gestion des matières premières** avec mise à jour des prix
- ✅ **Analyse des substances** par poids et fréquence d'utilisation
- ✅ **Rafraîchissement automatique** toutes les 5 secondes
- ✅ **Surveillance de fichiers** pour import automatique

### Temps d'installation estimé
- **Installation manuelle** : ~5-10 minutes
- **Prérequis non installés** : +15 minutes (.NET SDK + SQL Server)

### Support pour l'évaluateur
En cas de problème lors de l'installation :
1. Vérifier que .NET 8.0 SDK est installé : `dotnet --version`
2. Vérifier que LocalDB fonctionne : `sqllocaldb info`
3. Consulter la section **Dépannage** ci-dessous
4. Tous les logs d'erreur sont affichés dans la console

## 📈 Performances et optimisations

### Base de données
- **Index** : Sur clés étrangères et noms uniques
- **Requêtes** : Include() pour éviter N+1 queries
- **Async/Await** : Opérations non-bloquantes
- **Transactions** : Cohérence des données

### Interface utilisateur
- **Blazor Server** : Rendu côté serveur optimisé
- **SignalR** : Communication temps réel efficace
- **Bootstrap** : CSS optimisé et responsive
- **Composants** : Réutilisation et encapsulation

## 🎯 Évolutions futures possibles

### Fonctionnalités métier
- **Authentification/Autorisation** : Gestion utilisateurs
- **Historique** : Suivi des modifications
- **Export** : PDF/Excel des analyses
- **Recherche avancée** : Filtres et tri
- **Notifications** : Alertes prix/stock

### Aspects techniques
- **API REST** : Intégration externe
- **Tests unitaires** : Couverture complète
- **Docker** : Conteneurisation
- **CI/CD** : Déploiement automatisé
- **Monitoring** : Métriques et alertes

## 📞 Support technique

Cette application a été développée comme test technique démontrant :
- ✅ **Maîtrise .NET moderne** : .NET 8, EF Core, Blazor
- ✅ **Architecture clean** : SOLID, DI, séparation responsabilités
- ✅ **Développement full-stack** : Backend + Frontend intégré
- ✅ **Base de données** : Conception, migrations, requêtes
- ✅ **UX/UI** : Interface moderne et intuitive


