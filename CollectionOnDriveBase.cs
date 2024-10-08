namespace SunamoCollectionOnDrive;

public abstract class CollectionOnDriveBase<T> : List<T>
{
    #region DPP

    protected CollectionOnDriveArgs a;

    #endregion

    private bool isSaving;

    /// <summary>
    ///     Must use FileSystemWatcher, not FileSystemWatcher because its in sunamo, not desktop
    /// </summary>
    private readonly FileSystemWatcher w;

    public
#if ASYNC
        async Task
#else
void
#endif
        RemoveAll()
    {
        Clear();
#if ASYNC
        await
#endif
            File.WriteAllTextAsync(a.file, string.Empty);
    }

    public new void Remove(T t)
    {
        base.Remove(t);
        Save();
    }

    public new void Clear()
    {
        base.Clear();
        Save();
    }

    public abstract
#if ASYNC
        Task
#else
void
#endif
        Load();

    public void AddWithoutSave(T t)
    {
        if (!Contains(t)) base.Add(t);
    }

    public void Add(IList<T> prvek)
    {
        foreach (var item in prvek) Add(item);
    }

    public new bool Add(T prvek)
    {
        var b = false;
        if (!Contains(prvek))
        {
            if (prvek.ToString().Trim() != string.Empty)
            {
                base.Add(prvek);
                b = true;
            }
            // keep on false
        }

        // keep on false
        Save();
        return b;
    }

    private void Load(bool loadImmediately)
    {
        if (loadImmediately) Load();
    }

    public async Task Save()
    {

        isSaving = true;
        var removedOrNotExists = false;
        //if (FS.ExistsFile(a.file))
        //{
        //    removedOrNotExists = FS.TryDeleteFile(a.file);
        //}
        if (removedOrNotExists)
        {
            string obsah;
            obsah = ReturnContent();
            await File.WriteAllTextAsync(a.file, obsah);
        }

        isSaving = false;

    }

    private string ReturnContent()
    {
        string obsah;
        var sb = new StringBuilder();
        foreach (var var in this) sb.AppendLine(var.ToString());
        obsah = sb.ToString();
        return obsah;
    }

    public override string ToString()
    {
        return ReturnContent();
    }

    #region base

    public CollectionOnDriveBase(CollectionOnDriveArgs a)
    {
        this.a = a;
        File.AppendAllText(a.file, "");
        //FS.CreateFileIfDoesntExists(a.file);
        Load();
        if (a.loadChangesFromDrive)
        {
            w = new FileSystemWatcher(Path.GetDirectoryName(a.file));
            w.Filter = a.file;
            w.Changed += W_Changed;
        }
    }

    private void W_Changed(object sender, FileSystemEventArgs e)
    {
        if (!isSaving) Load();
    }

    #endregion
}