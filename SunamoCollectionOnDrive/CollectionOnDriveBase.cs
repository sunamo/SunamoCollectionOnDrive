namespace SunamoCollectionOnDrive;

/// <summary>
/// Base class for collections that persist their content to a file on disk.
/// </summary>
/// <typeparam name="T">Type of items in the collection.</typeparam>
public abstract class CollectionOnDriveBase<T> : List<T>
{
    /// <summary>
    /// Logger instance for this collection.
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    /// Whether duplicates should be removed on load and whether duplicate items should not even be saved.
    /// </summary>
    protected bool IsRemovingDuplicates = false;

    /// <summary>
    /// Configuration arguments for the collection.
    /// </summary>
    protected CollectionOnDriveArgs Args = new();

    private bool isSaving;
    private FileSystemWatcher? watcher;

    /// <summary>
    /// Initializes a new instance of the CollectionOnDriveBase class.
    /// </summary>
    /// <param name="logger">Logger instance for logging operations.</param>
    protected CollectionOnDriveBase(ILogger logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// Removes all items from the collection and clears the file on disk.
    /// </summary>
    public async Task RemoveAll()
    {
        await ClearWithSave();
        await File.WriteAllTextAsync(Args.Path, string.Empty);
    }

    /// <summary>
    /// Removes a specific value from the collection and saves the changes.
    /// </summary>
    /// <param name="value">Value to remove.</param>
    public async Task RemoveWithSave(T value)
    {
        base.Remove(value);
        await Save();
    }

    /// <summary>
    /// Clears the collection and saves the empty state.
    /// </summary>
    public async Task ClearWithSave()
    {
        base.Clear();
        await Save();
    }

    /// <summary>
    /// Loads the collection from disk.
    /// </summary>
    /// <param name="isRemovingDuplicates">Whether to remove duplicate entries when loading.</param>
    public abstract Task Load(bool isRemovingDuplicates);

    /// <summary>
    /// Adds a value to the collection without saving to disk. Checks for duplicates if IsRemovingDuplicates is enabled.
    /// </summary>
    /// <param name="value">Value to add.</param>
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
    /// Adds a value to the collection and saves to disk. Checks for duplicates if IsRemovingDuplicates is enabled.
    /// </summary>
    /// <param name="value">Value to add.</param>
    /// <returns>True if the value was added and saved, false if it was a duplicate or empty.</returns>
    /// <exception cref="Exception">Thrown if value is null or ToString returns null.</exception>
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
    /// Saves the collection to disk, removing duplicates.
    /// </summary>
    public async Task Save()
    {
        isSaving = true;
        await File.WriteAllTextAsync(Args.Path, SHJoin.JoinNL<T>(this.Distinct().ToList()));
        isSaving = false;
    }

    /// <summary>
    /// Returns a string representation of the collection with items separated by newlines.
    /// </summary>
    /// <returns>All items joined by newlines.</returns>
    public override string ToString()
    {
        return SHJoin.JoinNL(this);
    }

    #region ctor
    /// <summary>
    /// Initializes the collection with configuration arguments. Call Load() afterwards to load existing records.
    /// </summary>
    /// <param name="arguments">Configuration arguments including file path and whether to watch for changes.</param>
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

    /// <summary>
    /// Handles file change events from the FileSystemWatcher.
    /// </summary>
    private void Watcher_Changed(object sender, FileSystemEventArgs eventArgs)
    {
        if (!isSaving)
            Load(IsRemovingDuplicates);
    }
    #endregion
}
