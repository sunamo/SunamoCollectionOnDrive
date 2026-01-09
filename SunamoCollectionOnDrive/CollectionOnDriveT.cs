// variables names: ok
namespace SunamoCollectionOnDrive;

/// <summary>
/// A generic collection that persists custom types to disk using a parser interface
/// </summary>
/// <typeparam name="T">Type that implements IParserCollectionOnDrive for serialization/deserialization</typeparam>
public sealed class CollectionOnDriveT<T> : CollectionOnDriveBase<T> where T : IParserCollectionOnDrive
{
    /// <summary>
    /// Initializes a new instance of the CollectionOnDriveT class
    /// </summary>
    /// <param name="logger">Logger instance for logging operations</param>
    public CollectionOnDriveT(ILogger logger) : base(logger)
    {
    }

    /// <summary>
    /// Loads the collection from disk, creating instances of T and parsing each line
    /// </summary>
    /// <param name="removeDuplicates">Whether to remove duplicate entries when loading</param>
    public async override Task Load(bool removeDuplicates)
    {
        if (File.Exists(Args.Path))
        {
            foreach (var item in SHGetLines.GetLines(await File.ReadAllTextAsync(Args.Path)))
            {
                var instance = (T?)Activator.CreateInstance(typeof(T));
                ThrowEx.IsNull(nameof(instance), instance);
                instance!.Parse(item);
                await AddWithSave(instance);
            }
        }
    }
}