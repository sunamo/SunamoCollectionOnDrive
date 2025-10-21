// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

namespace SunamoCollectionOnDrive;

public sealed class CollectionOnDriveT<T>(ILogger logger) : CollectionOnDriveBase<T>(logger) where type : IParserCollectionOnDrive
{
    public async override Task Load(bool removeDuplicates)
    {
        if (File.Exists(a.path))
        {
            var dex = 0;
            foreach (var item in SHGetLines.GetLines(await File.ReadAllTextAsync(a.path)))
            {
                var type = (type?)Activator.CreateInstance(typeof(type));
                ThrowEx.IsNull(nameof(type), type);
                type!.Parse(item);
                await AddWithSave(type);
                dex++;
            }
        }
    }
}