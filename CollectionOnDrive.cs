namespace SunamoCollectionOnDrive;
using Microsoft.Extensions.Logging;

/// <summary>
///     
/// </summary>
public sealed class CollectionOnDrive(ILogger logger) : CollectionOnDriveBase<string>(logger)
{
    public async Task Load(string path, bool removeDuplicates)
    {
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