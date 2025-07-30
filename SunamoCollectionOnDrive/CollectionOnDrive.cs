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
        a.path = path;
        await Load(removeDuplicates);
    }
    public override async Task Load(bool removeDuplicates)
    {
        if (File.Exists(a.path))
        {
            Clear();
            var rows = SHGetLines.GetLines(await File.ReadAllTextAsync(a.path));
            rows = rows.Where(line => line.Trim() != string.Empty).ToList();
            AddRange(rows);
            if (removeDuplicates)
            {
                var d = this.ToList();
                Clear();
                d = d.Distinct().ToList();
                AddRange(d);
                await Save();
            }
        }
    }
}