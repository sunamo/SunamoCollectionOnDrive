using Microsoft.Extensions.Logging;

namespace SunamoCollectionOnDrive;

public abstract class CollectionOnDriveBase<T>(ILogger logger) : List<T>
{
    protected CollectionOnDriveArgs a = new();
    private bool isSaving;
    private FileSystemWatcher? w;

    public async Task RemoveAll()
    {
        await ClearWithSave();
        await File.WriteAllTextAsync(a.path, string.Empty);
    }

    public async Task RemoveWithSave(T t)
    {
        base.Remove(t);
        await Save();
    }

    public async Task ClearWithSave()
    {
        base.Clear();
        await Save();
    }

    public abstract Task Load();

    /// <summary>
    /// Check whether T is already contained.
    /// </summary>
    /// <param name="t"></param>
    public void AddWithoutSave(T t)
    {
        if (!Contains(t)) base.Add(t);
    }

    /// <summary>
    /// Check whether T is already contained.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<bool> AddWithSave(T? element)
    {
        if (element is null)
        {
            throw new Exception($"{nameof(element)} is null");
        }

        var b = false;
        if (!Contains(element))
        {
            var ts = element.ToString() ?? throw new Exception($"ToString of type ${element} cannot return null");
            if (ts.Trim() != string.Empty)
            {
                base.Add(element);
                b = true;
            }
        }

        await Save();
        return b;
    }


    public async Task Save()
    {
        isSaving = true;
        await File.WriteAllTextAsync(a.path, SHJoin.JoinNL<T>(this));
        isSaving = false;
    }

    public override string ToString()
    {
        return SHJoin.JoinNL(this);
    }

    #region ctor

    public void Init(CollectionOnDriveArgs a)
    {
        this.a = a;
        File.AppendAllText(a.path, "");
        Load();
        if (a.loadChangesFromDrive)
        {
            var up = Path.GetDirectoryName(a.path);
            if (up is null)
            {
                logger.LogWarning("FileSystemWatcher cannot be registered because null value");
                return;
            }
            else
            {
                w = new FileSystemWatcher
                {
                    Path = a.path
                };
                w.Changed += W_Changed;
            }
        }
    }

    private void W_Changed(object sender, FileSystemEventArgs e)
    {
        if (!isSaving)
            Load();
    }

    #endregion
}