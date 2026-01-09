// variables names: ok
namespace SunamoCollectionOnDrive;

/// <summary>
/// A collection of strings that persists its content to a file on disk
/// </summary>
public sealed class CollectionOnDrive : CollectionOnDriveBase<string>
{
    /// <summary>
    /// Dummy instance for testing or default scenarios. Do not use for production - will throw exception on operations.
    /// </summary>
    public static CollectionOnDrive Dummy = new CollectionOnDrive(NullLogger.Instance);

    /// <summary>
    /// Initializes a new instance of the CollectionOnDrive class
    /// </summary>
    /// <param name="logger">Logger instance for logging operations</param>
    public CollectionOnDrive(ILogger logger) : base(logger)
    {
    }

    /// <summary>
    /// Loads the collection from the specified file path
    /// </summary>
    /// <param name="path">Path to the file to load from</param>
    /// <param name="removeDuplicates">Whether to remove duplicate entries when loading</param>
    public async Task Load(string path, bool removeDuplicates)
    {
        if (Logger == NullLogger.Instance)
        {
            ThrowEx.UseNonDummyCollection();
        }
        Args.Path = path;
        await Load(removeDuplicates);
    }

    /// <summary>
    /// Loads the collection from the configured file path
    /// </summary>
    /// <param name="removeDuplicates">Whether to remove duplicate entries when loading</param>
    public override async Task Load(bool removeDuplicates)
    {
        if (File.Exists(Args.Path))
        {
            Clear();
            var rows = SHGetLines.GetLines(await File.ReadAllTextAsync(Args.Path));
            rows = rows.Where(line => line.Trim() != string.Empty).ToList();
            AddRange(rows);
            if (removeDuplicates)
            {
                var distinctItems = this.ToList();
                Clear();
                distinctItems = distinctItems.Distinct().ToList();
                AddRange(distinctItems);
                await Save();
            }
        }
    }
}