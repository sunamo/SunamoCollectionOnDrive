# SunamoCollectionOnDrive

## Short description

Kolekce, která svůj obsah automaticky ukládá do souboru na disku. Podporuje jednoduché řetězcové kolekce i vlastní typy přes parser.


A .NET library providing a collection that automatically persists its content to a file on disk. Supports both simple string collections and custom types via a parser interface.

## Overview

SunamoCollectionOnDrive is part of the Sunamo package ecosystem, providing modular, platform-independent utilities for .NET development.

## Main Components

### Key Classes

- **CollectionOnDrive** - A string collection that persists to a file on disk
- **CollectionOnDriveT\<T\>** - A generic collection for custom types implementing `IParserCollectionOnDrive`
- **CollectionOnDriveBase\<T\>** - Abstract base class with shared persistence logic
- **CollectionOnDriveArgs** - Configuration arguments for the collection

### Key Methods

- `Load()` - Loads the collection from disk
- `AddWithSave()` - Adds an item and saves to disk
- `AddWithoutSave()` - Adds an item without persisting
- `RemoveWithSave()` - Removes an item and saves to disk
- `RemoveAll()` - Clears the collection and the file
- `Save()` - Saves the current state to disk
- `Init()` - Initializes with configuration and optional file watching

## Installation

```bash
dotnet add package SunamoCollectionOnDrive
```

## Dependencies

- **Microsoft.Extensions.Logging.Abstractions** (v10.0.2)

## Package Information

- **Package Name**: SunamoCollectionOnDrive
- **Target Frameworks**: net10.0; net9.0; net8.0
- **Category**: Platform-Independent NuGet Package

## Related Packages

This package is part of the Sunamo package ecosystem. For more information about related packages, visit the main repository.

## License

MIT

<!-- sunamo:test-coverage:start -->
## Test coverage

| Metric | Value |
|---|---|
| Method | static |
| Test projects | 1 |
| Public types referenced by tests | 1/5 (20.0 %) |
| Public methods referenced by tests | 2/9 (22.2 %) |
| Line coverage | n/a |
| Branch coverage | n/a |
| Generated (UTC) | 2026-11-22 21:24 |

<!-- sunamo:test-coverage:data {"package":"SunamoCollectionOnDrive","container":"pinp","testProjects":1,"publicTypes":5,"typesReferencedByTests":1,"publicMethods":9,"methodsReferencedByTests":2,"lineRate":null,"branchRate":null,"method":"static","generatedUtc":"2026-11-22 21:24"} -->
<!-- sunamo:test-coverage:end -->
