### SunamoCollectionOnDrive

A .NET library providing a collection that automatically persists its content to a file on disk. Supports both simple string collections and custom types via a parser interface.

Part of PlatformIndependentNuGetPackages:

- [nuget.org](https://www.nuget.org/profiles/sunamo)
- [github.org](https://github.com/sunamo/PlatformIndependentNuGetPackages)

Another links:

- [Developer site](https://sunamo.cz)

Request for new features / bug report / etc: [Mail](mailto:radek.jancik@sunamo.cz) or on GitHub

## Features

- **CollectionOnDrive** - A string collection that persists to a file on disk
- **CollectionOnDriveT\<T\>** - A generic collection for custom types implementing `IParserCollectionOnDrive`
- Automatic file watching and reloading when the file changes on disk
- Duplicate removal support
- Async save/load operations

## Target Frameworks

**TargetFrameworks:** `net10.0;net9.0;net8.0`
