namespace SunamoCollectionOnDrive;

/// <summary>
/// Represents the collection on drive base t type.
/// </summary>
public abstract class CollectionOnDriveBase<T> : List<T>
{
    /// <summary>
    /// Gets the logger.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Gets or sets the is removing duplicates.
    /// </summary>
    protected bool IsRemovingDuplicates { get; set; } = false;

    /// <summary>
    /// Gets or sets the args.
    /// </summary>
    protected CollectionOnDriveArgs Args { get; set; } = new();

    private bool isSaving;
    private FileSystemWatcher? watcher;

    /// <summary>
    /// Collection on drive base.
    /// </summary>
    /// <returns>The collection on drive base result.</returns>
    protected CollectionOnDriveBase(ILogger logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// Remove all.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <returns>The remove all result.</returns>
    public async Task RemoveAll()
    {
        await ClearWithSave();
        await FileAsync.WriteAllTextAsync(Args.Path, string.Empty);
    }

    /// <summary>
    /// Remove with save.
    /// </summary>
    /// <returns>The remove with save result.</returns>
    public async Task RemoveWithSave(T value)
    {
        base.Remove(value);
        await Save();
    }

    /// <summary>
    /// Clear with save.
    /// </summary>
    /// <param name="Args.Path">The args path.</param>
    /// <param name="string.Empty">The string empty.</param>
    /// <returns>The clear with save result.</returns>
    public async Task ClearWithSave()
    {
        base.Clear();
        await Save();
    }

    /// <summary>
    /// Load.
    /// </summary>
    /// <returns>The load result.</returns>
    public abstract Task Load(bool isRemovingDuplicates);

    /// <summary>
    /// Add without save.
    /// </summary>
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

    /// <summary>
    /// Initializes a new instance of the add with save class.
    /// </summary>
    /// <param name="value">The value.</param>
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

    /// <summary>
    /// Save.
    /// </summary>
    /// <param name="wasChanged">The was changed.</param>
    /// <returns>The save result.</returns>
    public async Task Save()
    {
        isSaving = true;
        await FileAsync.WriteAllTextAsync(Args.Path, SHJoin.JoinNL<T>(this.Distinct().ToList()));
        isSaving = false;
    }

    /// <summary>
    /// To string.
    /// </summary>
    /// <returns>The to string result.</returns>
    public override string ToString()
    {
        return SHJoin.JoinNL(this);
    }

    #region ctor
    /// <summary>
    /// Init.
    /// </summary>
    /// <returns>The init result.</returns>
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
