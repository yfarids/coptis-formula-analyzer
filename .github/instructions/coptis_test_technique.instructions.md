---
applyTo: '**'
---

Développement d'un outil d'analyse de formules cosmétiques
Features à développer :
1. Importation d'une formule via une page web depuis le format json
2. Importation automatique via un dépôt de fichier dans un dossier local depuis le même
format json, le développeur a le choix du déclencheur du traitement automatique
3. Visualisation de toutes les formules sous forme de liste avec leur nom, poids et cout
total
4. Suppression d'une formule de la liste
5. Visualisation des substances les plus utilisées sous forme de liste
a. Soit en fonction du poids qu'elles représentent dans tout le "stock de formules"
b. Soit en fonction du nombre de formule les utilisant [BONUS]
6. Mise à jour du prix d'une des matières premières présente depuis la liste des matières
premières
7. Les formules avec un nouveau prix seront mise en avant/highlited dans la visualisation
des formules
Requirement techniques:
Utiliser le maximum de technologie .Net
BDD SqlServer / EntityFramework / Blazor Server ou WebAssembly
Le choix de l'architecture est libre mais doit être pertinent
Les fichiers json de formules doivent pouvoir être importés en l'état sans être modifiés
et sans plantage du système (sans exception)
Les règles de gestion doivent être centralisées dans une seule application (pas de
partage de code)
Tous les calculs/affichages des prix/poids doivent être fait avec 2 chiffres après la
virgule
Le style des composants front est libre, les maquettes sont là à titre d’exemple pour
apporter plus de clarté uniquement
Output:
Le système d'importation/analyse de formulation doit fonctionner sans aucun bug
Le système doit gérer les erreurs métiers de telle sorte que des utilisateurs puissent
corriger leurs données par exemple
Le développeur devra fournir un schéma de conception de son architecture
Evaluation:
La livraison des 7 features fonctionnelles + améliorations proposées/implémentées
dans l UX
Qualité du code et respect des normes SOLID
Les règles de gestion qui seront implémentées ne sont pas décrite dans l'exercice,
mais seront chalengées en termes de logique
Les choix d'architecture et d'implémentation seront discuté et devront être justifiés de
manière pertinente
Input:
5 formules au format json sont fournies et prête à être importées en l'état
Les contrats DTO C# des formules seront aussi fournis (les DTO ne seront pas
forcément le format de stockage en BDD)
Stratégie de test:
Etape 0 : installation/build du system en local sur un PC Coptis
Etape 1: import des formules dans l'ordre une par une via le front web
Etape 2: suppression des formules une par une
Etape 3: importation en masse des 5 fichiers json via l'import automatique du dossier
local contenant les fichiers json

