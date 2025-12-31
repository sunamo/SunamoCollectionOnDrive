namespace SunamoCollectionOnDrive;

/// <summary>
///     
/// </summary>
public sealed class CollectionOnDrive(ILogger logger) : CollectionOnDriveBase<string>(logger)
{
    public static CollectionOnDrive Dummy = new CollectionOnDrive(NullLogger.Instance);

    public async Task Load(string path, bool removeDuplicates)
    {
        if (logger == NullLogger.Instance)
        {
            ThrowEx.UseNonDummyCollection();
        }
        args.Path = path;
        await Load(removeDuplicates);
    }

    public override async Task Load(bool removeDuplicates)
    {
        if (File.Exists(args.Path))
        {
            Clear();
            var rows = SHGetLines.GetLines(await File.ReadAllTextAsync(args.Path));
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