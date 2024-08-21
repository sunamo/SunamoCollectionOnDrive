namespace SunamoCollectionOnDrive;

public class CollectionOnDriveT<T> : CollectionOnDriveBase<T> where T : IParserCollectionOnDrive
{
    public CollectionOnDriveT(PpkOnDriveArgs a) : base(a)
    {
    }

    public CollectionOnDriveT(string file2, bool load = true) : base(new PpkOnDriveArgs { file = file2, load = load })
    {
    }

    public CollectionOnDriveT(string file, bool load, bool save) : base(new PpkOnDriveArgs
        { file = file, load = load, save = save })
    {
    }

    public override
#if ASYNC
        async Task
#else
void
#endif
        Load()
    {
        if (File.Exists(a.file))
        {
            var dex = 0;
            foreach (var item in SHGetLines.GetLines(
#if ASYNC
                         await
#endif
                             File.ReadAllTextAsync(a.file)))
                //TF.ReadAllLines(a.file))
            {
                var t = (T)Activator.CreateInstance(typeof(T));
                t.Parse(item);
                Add(t);
                dex++;
            }
        }
    }
}