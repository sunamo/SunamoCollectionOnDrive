// variables names: ok
namespace SunamoCollectionOnDrive.Args;

/// <summary>
/// Arguments for configuring a CollectionOnDrive instance
/// </summary>
public class CollectionOnDriveArgs
{
    /// <summary>
    /// Path to the file where the collection data is stored
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Whether to automatically reload the collection when the file changes on disk
    /// </summary>
    public bool LoadChangesFromDrive { get; set; } = true;
}