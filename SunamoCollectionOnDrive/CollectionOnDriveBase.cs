// variables names: ok
namespace SunamoCollectionOnDrive;

/// <summary>
/// Base class for collections that persist their content to a file on disk
/// </summary>
/// <typeparam name="T">Type of items in the collection</typeparam>
public abstract class CollectionOnDriveBase<T> : List<T>
{
    /// <summary>
    /// Logger instance for this collection
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    /// Whether duplicates should be removed on load and whether duplicate items should not even be saved
    /// </summary>
    protected bool RemoveDuplicates = false;

    /// <summary>
    /// Configuration arguments for the collection
    /// </summary>
    protected CollectionOnDriveArgs Args = new();

    private bool isSaving;
    private FileSystemWatcher? watcher;

    /// <summary>
    /// Initializes a new instance of the CollectionOnDriveBase class
    /// </summary>
    /// <param name="logger">Logger instance for logging operations</param>
    protected CollectionOnDriveBase(ILogger logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// Removes all items from the collection and clears the file on disk
    /// </summary>
    public async Task RemoveAll()
    {
        await ClearWithSave();
        await File.WriteAllTextAsync(Args.Path, string.Empty);
    }

    /// <summary>
    /// Removes a specific item from the collection and saves the changes
    /// </summary>
    /// <param name="item">Item to remove</param>
    public async Task RemoveWithSave(T item)
    {
        base.Remove(item);
        await Save();
    }

    /// <summary>
    /// Clears the collection and saves the empty state
    /// </summary>
    public async Task ClearWithSave()
    {
        base.Clear();
        await Save();
    }

    /// <summary>
    /// Loads the collection from disk
    /// </summary>
    /// <param name="removeDuplicates">Whether to remove duplicate entries when loading</param>
    public abstract Task Load(bool removeDuplicates);
    /// <summary>
    /// Adds an item to the collection without saving to disk. Checks for duplicates if RemoveDuplicates is enabled.
    /// </summary>
    /// <param name="item">Item to add</param>
    public virtual void AddWithoutSave(T item)
    {
        if (Logger == NullLogger.Instance)
        {
            ThrowEx.UseNonDummyCollection();
        }
        if (RemoveDuplicates)
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
    /// Adds an item to the collection and saves to disk. Checks for duplicates if RemoveDuplicates is enabled.
    /// </summary>
    /// <param name="item">Item to add</param>
    /// <returns>True if the item was added and saved, false if it was a duplicate or empty</returns>
    /// <exception cref="Exception">Thrown if item is null or ToString returns null</exception>
    public virtual async Task<bool> AddWithSave(T? item)
    {
        if (Logger == NullLogger.Instance)
        {
            ThrowEx.UseNonDummyCollection();
        }
        if (item is null)
        {
            throw new Exception($"{nameof(item)} is null");
        }
        var wasChanged = false;
        if (RemoveDuplicates)
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
    /// <summary>
    /// Saves the collection to disk, removing duplicates
    /// </summary>
    public async Task Save()
    {
        isSaving = true;
        await File.WriteAllTextAsync(Args.Path, SHJoin.JoinNL<T>(this.Distinct().ToList()));
        isSaving = false;
    }

    /// <summary>
    /// Returns a string representation of the collection with items separated by newlines
    /// </summary>
    public override string ToString()
    {
        return SHJoin.JoinNL(this);
    }
    #region ctor
    /// <summary>
    /// Initializes the collection with configuration arguments. Call Load() afterwards to load existing records.
    /// </summary>
    /// <param name="arguments">Configuration arguments including file path and whether to watch for changes</param>
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
    /// Handles file change events from the FileSystemWatcher
    /// </summary>
    private void Watcher_Changed(object sender, FileSystemEventArgs e)
    {
        if (!isSaving)
            Load(RemoveDuplicates);
    }
    #endregion
}