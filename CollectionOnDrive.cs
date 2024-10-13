using Microsoft.Extensions.Logging;

namespace SunamoCollectionOnDrive;

/// <summary>
///     
/// </summary>
public sealed class CollectionOnDrive(ILogger logger) : CollectionOnDriveBase<string>(logger)
{
    public bool removeDuplicates = false;
    public async Task Load(string path)
    {
        a.path = path;
        await Load();
    }

    public override async Task Load()
    {
        if (File.Exists(a.path))
        {
            await ClearWithSave();
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