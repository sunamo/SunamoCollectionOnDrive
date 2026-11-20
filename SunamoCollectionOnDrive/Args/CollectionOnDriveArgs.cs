namespace SunamoCollectionOnDrive.Args;

/// <summary>
/// Represents the collection on drive args type.
/// </summary>
public class CollectionOnDriveArgs
{
    /// <summary>
    /// Gets or sets the path.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the load changes from drive.
    /// </summary>
    public bool LoadChangesFromDrive { get; set; } = true;
}
