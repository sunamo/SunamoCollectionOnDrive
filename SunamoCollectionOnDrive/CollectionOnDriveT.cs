namespace SunamoCollectionOnDrive;

/// <summary>
/// Represents the collection on drive t t type.
/// </summary>
public sealed class CollectionOnDriveT<T> : CollectionOnDriveBase<T> where T : IParserCollectionOnDrive
{
    /// <summary>
    /// Initializes a new instance of the collection on drive t class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public CollectionOnDriveT(ILogger logger) : base(logger)
    {
    }

    /// <summary>
    /// Load.
    /// </summary>
    /// <param name="isRemovingDuplicates">The is removing duplicates.</param>
    public async override Task Load(bool isRemovingDuplicates)
    {
        if (File.Exists(Args.Path))
        {
            foreach (var item in SHGetLines.GetLines(await FileAsync.ReadAllTextAsync(Args.Path)))
            {
                var instance = (T?)Activator.CreateInstance(typeof(T));
                ThrowEx.IsNull(nameof(instance), instance);
                instance!.Parse(item);
                await AddWithSave(instance);
            }
        }
    }
}
