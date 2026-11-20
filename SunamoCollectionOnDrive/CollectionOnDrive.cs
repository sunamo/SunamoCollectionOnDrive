namespace SunamoCollectionOnDrive;

// A collection of strings that persists its content to a file on disk.
/// <summary>
/// Represents the collection on drive type.
/// </summary>
public sealed class CollectionOnDrive : CollectionOnDriveBase<string>
{
    // Dummy instance for testing or default scenarios. Do not use for production - will throw exception on operations.
    /// <summary>
    /// Gets or sets the dummy.
    /// </summary>
    public static CollectionOnDrive Dummy { get; set; } = new CollectionOnDrive(NullLogger.Instance);

    /// <summary>
    /// Collection on drive.
    /// </summary>
    /// <param name="NullLogger.Instance">The null logger instance.</param>
    /// <returns>The collection on drive result.</returns>
    public CollectionOnDrive(ILogger logger) : base(logger)
    {
    }

    /// <summary>
    /// Load.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="isRemovingDuplicates">The is removing duplicates.</param>
    /// <returns>The load result.</returns>
    public async Task Load(string path, bool isRemovingDuplicates)
    {
        if (Logger == NullLogger.Instance)
        {
            ThrowEx.UseNonDummyCollection();
        }
        Args.Path = path;
        await Load(isRemovingDuplicates);
    }

    /// <summary>
    /// Load.
    /// </summary>
    /// <param name="isRemovingDuplicates">The is removing duplicates.</param>
    /// <returns>The load result.</returns>
    public override async Task Load(bool isRemovingDuplicates)
    {
        if (File.Exists(Args.Path))
        {
            Clear();
            var lines = SHGetLines.GetLines(await FileAsync.ReadAllTextAsync(Args.Path));
            lines = lines.Where(line => line.Trim() != string.Empty).ToList();
            AddRange(lines);
            if (isRemovingDuplicates)
            {
                var distinctList = this.ToList();
                Clear();
                distinctList = distinctList.Distinct().ToList();
                AddRange(distinctList);
                await Save();
            }
        }
    }
}
