# Analyseur de Formules Coptis

Un outil d'analyse de formules cosmétiques complet développé avec .NET 8.0 et Blazor Server. Cette application offre des capacités avancées pour l'importation, l'analyse et la gestion des formulations cosmétiques avec des calculs de coûts en temps réel et une analyse des substances.

## Fonctionnalités Principales

### 1. Importation et Gestion des Formules
- **Importation JSON via Interface Web** : Importation manuelle de formules à travers une interface web intuitive
- **Traitement Automatique de Fichiers** : Service de surveillance de fichiers qui traite automatiquement les fichiers JSON déposés dans un dossier surveillé
- **Visualisation des Formules** : Vue d'ensemble complète de toutes les formules avec nom, poids total et coûts calculés
- **Suppression de Formules** : Suppression sécurisée des formules du système

### 2. Analyses Avancées
- **Analyse des Substances par Poids** : Identifier les substances les plus utilisées selon le poids total dans toutes les formules
- **Analyse des Substances par Usage** : Suivre les substances selon le nombre de formules qui les utilisent
- **Calculs de Coûts en Temps Réel** : Mise à jour automatique des coûts lors de changements de prix des matières premières
- **Mise en Évidence des Mises à Jour de Prix** : Indicateurs visuels pour les formules affectées par les changements de prix

### 3. Gestion des Matières Premières
- **Gestion des Prix** : Mettre à jour les prix des matières premières avec recalcul immédiat des coûts
- **Mises à Jour en Temps Réel** : Reflet instantané des changements de prix dans toutes les formules affectées
- **Suivi des Matériaux** : Vue d'ensemble complète de toutes les matières premières et de leur tarification actuelle

## Architecture

L'application suit les principes de l'**Architecture Propre** (Clean Architecture) avec une séparation claire des responsabilités :

```
┌─────────────────────────────────────────┐
│            Couche Présentation         │
│    - Pages et Composants Blazor Server │
│    - Intégration SignalR Temps Réel    │
│    - Interface et Interactions Utilis. │
└─────────────────────────────────────────┘
            ↓ Injection de Dépendances
┌─────────────────────────────────────────┐
│           Couche Application            │
│    - Logique Métier et Services        │
│    - Import et Traitement de Fichiers  │
│    - Calculs de Coûts des Formules     │
│    - Surveillance de Fichiers en Arr.  │
└─────────────────────────────────────────┘
            ↓ Implémentation d'Interfaces
┌─────────────────────────────────────────┐
│          Couche Infrastructure          │
│    - Entity Framework Core             │
│    - Base de Données SQL Server        │
│    - Pattern Repository                │
│    - Persistance des Données           │
└─────────────────────────────────────────┘
            ↓ Abstractions du Domaine
┌─────────────────────────────────────────┐
│             Couche Domaine              │
│    - Entités du Domaine                │
│    - DTOs Métier                       │
│    - Contrats d'Interfaces             │
│    - Règles du Domaine                 │
└─────────────────────────────────────────┘
```

## Concepts et Patterns Utilisés

### Concepts Architecturaux

#### Clean Architecture (Architecture Propre)
- **Séparation des responsabilités** : Chaque couche a une responsabilité spécifique et bien définie
- **Inversion des dépendances** : Les couches externes dépendent des couches internes, jamais l'inverse
- **Indépendance des frameworks** : La logique métier ne dépend pas des technologies externes
- **Testabilité** : Architecture facilitant les tests unitaires et d'intégration

#### Domain-Driven Design (DDD)
- **Entités métier** : Modélisation des concepts du domaine cosmétique (Formula, RawMaterial, FormulaComponent)
- **Services de domaine** : Logique métier complexe encapsulée dans des services spécialisés
- **DTOs (Data Transfer Objects)** : Objets dédiés au transfert de données entre les couches
- **Agrégats** : Groupement cohérent d'entités liées (Formula avec ses FormulaComponents)

### Patterns de Conception

#### Repository Pattern
- **Abstraction de l'accès aux données** : Interface uniforme pour l'accès aux données
- **Séparation des préoccupations** : Logique métier séparée de la persistance
- **Facilitation des tests** : Possibilité de mocker les repositories pour les tests
- **Centralisation des requêtes** : Toutes les requêtes base de données dans un endroit dédié

#### Dependency Injection (Injection de Dépendances)
- **Couplage faible** : Réduction des dépendances directes entre les classes
- **Configuration centralisée** : Toutes les dépendances configurées dans Program.cs
- **Facilitation des tests** : Injection de mocks et stubs pour les tests
- **Gestion du cycle de vie** : Contrôle automatique de la durée de vie des objets

#### Service Layer Pattern
- **Encapsulation de la logique métier** : Services dédiés pour chaque domaine fonctionnel
- **Coordination des opérations** : Orchestration des appels aux repositories
- **Gestion des transactions** : Contrôle de la cohérence des données
- **Interface uniforme** : API cohérente pour la couche présentation

### Concepts de Développement

#### SOLID Principles
- **S - Single Responsibility** : Chaque classe a une seule raison de changer
- **O - Open/Closed** : Classes ouvertes à l'extension, fermées à la modification
- **L - Liskov Substitution** : Les objets dérivés peuvent remplacer leurs objets de base
- **I - Interface Segregation** : Interfaces spécifiques plutôt qu'une interface générale
- **D - Dependency Inversion** : Dépendre des abstractions, pas des implémentations concrètes

#### Async/Await Pattern
- **Programmation asynchrone** : Opérations non-bloquantes pour l'interface utilisateur
- **Gestion des ressources** : Utilisation efficace des threads et de la mémoire
- **Scalabilité** : Capacité à gérer de nombreuses requêtes simultanées
- **Responsivité** : Interface utilisateur réactive pendant les opérations longues

### Concepts Techniques Spécifiques

#### File System Watcher
- **Surveillance en temps réel** : Détection automatique des nouveaux fichiers JSON
- **Traitement en arrière-plan** : Service hébergé (Hosted Service) pour la surveillance continue
- **Gestion des événements** : Réaction aux événements de création/modification de fichiers
- **Coordination des imports** : Synchronisation entre imports manuels et automatiques

#### Real-time Communication (SignalR)
- **Communication bidirectionnelle** : Serveur vers client en temps réel
- **Notifications automatiques** : Mise à jour de l'interface lors d'imports automatiques
- **Gestion de l'état** : Synchronisation de l'état entre plusieurs clients
- **Hub Pattern** : Centralisation de la communication temps réel

#### Error Handling & Resilience
- **Gestion gracieuse des erreurs** : Aucun plantage système sur données invalides
- **Validation en cascade** : Validation à tous les niveaux (DTO, Service, Repository)
- **Logging structuré** : Journalisation avec Serilog pour le monitoring
- **Messages utilisateur** : Retours d'erreurs compréhensibles pour l'utilisateur

#### Business Rules Engine
- **Calculs de coûts** : Algorithmes de calcul basés sur les poids et prix
- **Règles de validation** : Validation des formules selon les règles métier
- **Analyse des substances** : Agrégation et classement des données de substances
- **Recalcul automatique** : Mise à jour en cascade lors de changements de prix

## Stack Technologique

### Technologies Backend
- **.NET 8.0** : Dernière version du framework .NET
- **Entity Framework Core 8.0** : Mapping objet-relationnel avec SQL Server
- **SQL Server** : Base de données robuste pour le stockage des formules et matériaux
- **Serilog** : Journalisation structurée pour le monitoring et le débogage

### Technologies Frontend
- **Blazor Server** : Rendu côté serveur avec mises à jour UI en temps réel
- **Bootstrap 5** : Composants UI modernes et responsifs
- **SignalR** : Communication temps réel pour l'actualisation automatique des données

### Patterns Architecturaux
- **Architecture Propre** : Séparation des préoccupations avec inversion des dépendances
- **Pattern Repository** : Abstraction de l'accès aux données
- **Injection de Dépendances** : Couplage faible et testabilité
- **Principes SOLID** : Code maintenable et extensible

## Modèle de Données

### Entités Principales
- **Formula** : Représente une formulation cosmétique avec nom, poids et calculs de coûts
- **RawMaterial** : Matières premières avec informations de tarification (prix par kg)
- **FormulaComponent** : Entité de liaison reliant les formules aux matières premières avec des poids spécifiques

### Logique Métier
- Tous les calculs maintiennent une précision de **2 décimales** comme requis
- Recalcul automatique des coûts lors de mise à jour des prix des matières premières
- Importation JSON robuste avec gestion d'erreurs (aucun plantage système sur données invalides)
- Règles métier centralisées dans la couche Application

## Système d'Importation

### Importation Manuelle
- Interface web pour collage et importation directe de JSON
- Validation en temps réel et retour d'erreurs
- Actualisation immédiate de l'UI lors d'importation réussie

### Importation Automatique
- Surveillance de fichiers surveille un dossier local désigné
- Traite automatiquement les fichiers JSON lors de création/modification de fichier
- Service en arrière-plan assure une surveillance continue
- Déclencheurs de traitement de fichiers configurables

### Support de Format JSON
- Importe les fichiers JSON fournis sans modification
- Gère les structures imbriquées complexes avec matières premières et substances
- Gestion gracieuse des erreurs pour données malformées ou dupliquées

## Analyses et Rapports

### Analyse des Substances
1. **Par Poids Total** : Substances classées par poids cumulé dans toutes les formules
2. **Par Nombre d'Utilisations** : Substances classées par fréquence d'utilisation dans différentes formules

### Gestion des Coûts
- Calculs de coûts en temps réel utilisant les prix actuels des matières premières
- Suivi historique des changements de prix
- Mise en évidence visuelle des formules affectées par les récentes mises à jour de prix

## Fonctionnalités de Développement

### Assurance Qualité
- Tests unitaires complets pour la logique du domaine
- Tests d'intégration pour les couches d'accès aux données
- Respect des principes SOLID dans tout le code

### Gestion d'Erreurs
- Gestion gracieuse des erreurs d'importation sans plantage système
- Messages d'erreur conviviaux pour la correction des données
- Validation robuste à tous les niveaux

### Mises à Jour Temps Réel
- Actualisation automatique de l'UI pour les importations basées sur fichiers
- Notifications temps réel pour les changements de données
- Expérience utilisateur fluide avec données en direct

## Conformité aux Exigences

**Fonctionnalité 1** : Importation JSON manuelle via interface web  
**Fonctionnalité 2** : Importation JSON automatique via surveillance de fichiers  
**Fonctionnalité 3** : Visualisation des formules avec nom, poids et coût  
**Fonctionnalité 4** : Capacité de suppression de formules  
**Fonctionnalité 5a** : Analyse des substances par poids total  
**Fonctionnalité 5b** : Analyse des substances par nombre d'utilisations (BONUS)  
**Fonctionnalité 6** : Mises à jour des prix des matières premières  
**Fonctionnalité 7** : Mise en évidence visuelle des formules avec prix mis à jour  

### Exigences Techniques
Utilisation maximale des technologies .NET  
SQL Server avec Entity Framework Core  
Implémentation Blazor Server  
Conception Architecture Propre  
Importation des fichiers JSON sans modification  
Aucun plantage système sur données invalides  
Règles métier centralisées  
2 décimales pour tous les calculs  

## Valeur Métier

### Pour les Formulateurs Cosmétiques
- **Optimisation des Coûts** : Suivi des coûts en temps réel aide à optimiser la rentabilité des formules
- **Gestion des Substances** : Identifier les matières premières les plus critiques pour la planification des stocks
- **Analyse d'Impact des Prix** : Visibilité immédiate sur l'impact des changements de prix sur les formulations

### Pour les Opérations Métier
- **Flux de Travail Automatisés** : Réduire la saisie manuelle avec le traitement automatique de fichiers
- **Intégrité des Données** : Système d'importation robuste prévient la corruption des données
- **Analyses Temps Réel** : Prendre des décisions éclairées avec des données d'usage des substances à jour

### Pour les Équipes IT
- **Architecture Maintenable** : Séparation claire des préoccupations pour des modifications faciles
- **Conception Évolutive** : Pattern repository et injection de dépendances pour la croissance future
- **Monitoring et Débogage** : Journalisation complète avec intégration Serilog

## Améliorations Futures

L'architecture actuelle supporte une extension facile pour des fonctionnalités supplémentaires :
- **Rapports Avancés** : Capacités d'export et analyses détaillées
- **Gestion des Utilisateurs** : Support multi-utilisateurs avec accès basé sur les rôles
- **Intégration API** : APIs RESTful pour l'intégration de systèmes externes
- **Support Mobile** : Design responsif prêt pour l'optimisation mobile
- **Opérations par Lots** : Opérations et importations de formules en masse
- **Piste d'Audit** : Historique complet des changements et modifications

## Installation et Démarrage

### Prérequis
- .NET 8.0 SDK
- SQL Server (LocalDB ou instance complète)
- Visual Studio 2022 ou VS Code

### Instructions de Build
```bash

# Naviguer vers le répertoire du projet
cd Coptis_FullStack_Test_Technique

# Restaurer les packages NuGet
dotnet restore

# Construire la solution
dotnet build

# Exécuter les migrations de base de données
cd src/CoptisFormulaAnalyzer.Web
dotnet ef database update

# Lancer l'application
dotnet run
```

### Configuration
1. Modifier la chaîne de connexion dans `appsettings.json`
2. Configurer le dossier de surveillance pour l'importation automatique
3. Ajuster les niveaux de journalisation selon les besoins

## Stratégie de Test

### Tests Automatisés
- **Tests Unitaires** : Logique métier et calculs de formules
- **Tests d'Intégration** : Accès aux données et importation de fichiers
- **Tests de Validation** : Format JSON et règles métier

### Tests Manuels
1. **Étape 0** : Installation/build du système en local
2. **Étape 1** : Import des formules une par une via l'interface web
3. **Étape 2** : Suppression des formules une par une
4. **Étape 3** : Importation en masse via le dossier local

