namespace SunamoCollectionOnDrive;

public sealed class CollectionOnDriveT<T>(ILogger logger) : CollectionOnDriveBase<T>(logger) where T : IParserCollectionOnDrive
{
    public async override Task Load(bool removeDuplicates)
    {
        if (File.Exists(a.path))
        {
            var dex = 0;
            foreach (var item in SHGetLines.GetLines(await File.ReadAllTextAsync(a.path)))
            {
                var t = (T?)Activator.CreateInstance(typeof(T));
                ThrowEx.IsNull(nameof(t), t);
                t!.Parse(item);
                await AddWithSave(t);
                dex++;
            }
        }
    }
}