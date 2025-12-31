namespace SunamoCollectionOnDrive;

public abstract class CollectionOnDriveBase<T>(ILogger logger) : List<T>
{
    /// <summary>
    /// whether duplicates should be removed on load and whether duplicate items should not even be saved
    /// </summary>
    protected bool removeDuplicates = false;
    protected CollectionOnDriveArgs args = new();
    private bool isSaving;
    private FileSystemWatcher? watcher;
    public async Task RemoveAll()
    {
        await ClearWithSave();
        await File.WriteAllTextAsync(args.Path, string.Empty);
    }
    public async Task RemoveWithSave(T item)
    {
        base.Remove(item);
        await Save();
    }
    public async Task ClearWithSave()
    {
        base.Clear();
        await Save();
    }
    public abstract Task Load(bool removeDuplicates);
    /// <summary>
    /// Check whether T is already contained.
    /// </summary>
    /// <param name="item"></param>
    public virtual void AddWithoutSave(T item)
    {
        if (logger == NullLogger.Instance)
        {
            ThrowEx.UseNonDummyCollection();
        }
        if (removeDuplicates)
        {
            if (!Contains(item))
            {
                base.Add(item);
            }
        }
        else
        {
            base.Add(item);
        }
    }
    /// <summary>
    /// Check whether T is already contained.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public virtual async Task<bool> AddWithSave(T? item)
    {
        if (logger == NullLogger.Instance)
        {
            ThrowEx.UseNonDummyCollection();
        }
        if (item is null)
        {
            throw new Exception($"{nameof(item)} is null");
        }
        var wasChanged = false;
        if (removeDuplicates)
        {
            if (!Contains(item))
            {
                var stringValue = item.ToString() ?? throw new Exception($"ToString of type ${item} cannot return null");
                if (stringValue.Trim() != string.Empty)
                {
                    base.Add(item);
                    wasChanged = true;
                }
            }
        }
        else
        {
            base.Add(item);
            wasChanged = true;
        }
        if (wasChanged)
        {
            await Save();
        }
        return wasChanged;
    }
    public async Task Save()
    {
        isSaving = true;
        await File.WriteAllTextAsync(args.Path, SHJoin.JoinNL<T>(this.Distinct().ToList()));
        isSaving = false;
    }
    public override string ToString()
    {
        return SHJoin.JoinNL(this);
    }
    #region ctor
    /// <summary>
    /// optional call only if you want to set by CollectionOnDriveArgs. Calling Load() for already existing records is important.
    /// </summary>
    /// <param name="arguments"></param>
    public void Init(CollectionOnDriveArgs arguments)
    {
        this.args = arguments;
        if (args.LoadChangesFromDrive)
        {
            var parentDirectory = Path.GetDirectoryName(args.Path);
            if (parentDirectory is null)
            {
                logger.LogWarning("FileSystemWatcher cannot be registered because null value");
                return;
            }
            else
            {
                watcher = new FileSystemWatcher
                {
                    Path = args.Path
                };
                watcher.Changed += Watcher_Changed;
            }
        }
    }
    private void Watcher_Changed(object sender, FileSystemEventArgs e)
    {
        if (!isSaving)
            Load(removeDuplicates);
    }
    #endregion
}