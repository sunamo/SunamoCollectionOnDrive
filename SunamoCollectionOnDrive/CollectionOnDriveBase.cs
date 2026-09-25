namespace SunamoCollectionOnDrive;

public abstract class CollectionOnDriveBase<T> : List<T>
{
    protected ILogger Logger { get; }

    protected bool IsRemovingDuplicates { get; set; } = false;

    protected CollectionOnDriveArgs Args { get; set; } = new();

    private bool isSaving;
    private FileSystemWatcher? watcher;

    protected CollectionOnDriveBase(ILogger logger)
    {
        Logger = logger;
    }

    public async Task RemoveAll()
    {
        await ClearWithSave();
        await FileAsync.WriteAllTextAsync(Args.Path, string.Empty);
    }

    public async Task RemoveWithSave(T value)
    {
        base.Remove(value);
        await Save();
    }

    public async Task ClearWithSave()
    {
        base.Clear();
        await Save();
    }

    public abstract Task Load(bool isRemovingDuplicates);

    public virtual void AddWithoutSave(T value)
    {
        if (Logger == NullLogger.Instance)
        {
            ThrowEx.UseNonDummyCollection();
        }
        if (IsRemovingDuplicates)
        {
            if (!Contains(value))
            {
                base.Add(value);
            }
        }
        else
        {
            base.Add(value);
        }
    }

    public virtual async Task<bool> AddWithSave(T? value)
    {
        if (Logger == NullLogger.Instance)
        {
            ThrowEx.UseNonDummyCollection();
        }
        if (value is null)
        {
            throw new Exception($"{nameof(value)} is null");
        }
        var wasChanged = false;
        if (IsRemovingDuplicates)
        {
            if (!Contains(value))
            {
                var stringValue = value.ToString() ?? throw new Exception($"ToString of type ${value} cannot return null");
                if (stringValue.Trim() != string.Empty)
                {
                    base.Add(value);
                    wasChanged = true;
                }
            }
        }
        else
        {
            base.Add(value);
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
        await FileAsync.WriteAllTextAsync(Args.Path, SHJoin.JoinNL<T>(this.Distinct().ToList()));
        isSaving = false;
    }

    public override string ToString()
    {
        return SHJoin.JoinNL(this);
    }

    #region ctor
    public void Init(CollectionOnDriveArgs arguments)
    {
        this.Args = arguments;
        if (Args.LoadChangesFromDrive)
        {
            var parentDirectory = Path.GetDirectoryName(Args.Path);
            if (parentDirectory is null)
            {
                Logger.LogWarning("FileSystemWatcher cannot be registered because parent directory is null");
                return;
            }
            else
            {
                var fileName = Path.GetFileName(Args.Path);
                watcher = new FileSystemWatcher
                {
                    Path = parentDirectory,
                    Filter = fileName
                };
                watcher.Changed += Watcher_Changed;
                watcher.EnableRaisingEvents = true;
            }
        }
    }

    private void Watcher_Changed(object sender, FileSystemEventArgs eventArgs)
    {
        if (!isSaving)
            Load(IsRemovingDuplicates);
    }
    #endregion
}
