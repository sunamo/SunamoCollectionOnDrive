namespace SunamoCollectionOnDrive;

// A collection of strings that persists its content to a file on disk.
public sealed class CollectionOnDrive : CollectionOnDriveBase<string>
{
    // Dummy instance for testing or default scenarios. Do not use for production - will throw exception on operations.
    public static CollectionOnDrive Dummy { get; set; } = new CollectionOnDrive(NullLogger.Instance);

    public CollectionOnDrive(ILogger logger) : base(logger)
    {
    }

    public async Task Load(string path, bool isRemovingDuplicates)
    {
        if (Logger == NullLogger.Instance)
        {
            ThrowEx.UseNonDummyCollection();
        }
        Args.Path = path;
        await Load(isRemovingDuplicates);
    }

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
