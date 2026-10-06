namespace SunamoCollectionOnDrive;

public sealed class CollectionOnDriveT<T> : CollectionOnDriveBase<T> where T : IParserCollectionOnDrive
{
    public CollectionOnDriveT(ILogger logger) : base(logger)
    {
    }

    public async override Task Load(bool isRemovingDuplicates)
    {
        if (File.Exists(Args.Path))
        {
            foreach (var item in SHGetLines.GetLines(await FileAsync.ReadAllTextAsync(Args.Path)))
            {
                var instance = (T?)Activator.CreateInstance(typeof(T));
                ThrowEx.IsNull(nameof(instance), instance);
                instance!.Parse(item);
                await AddWithSave(instance);
            }
        }
    }
}
