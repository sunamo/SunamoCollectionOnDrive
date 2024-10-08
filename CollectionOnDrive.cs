namespace SunamoCollectionOnDrive;

/// <summary>
///     Checking whether string is already contained.
/// </summary>
public class CollectionOnDrive : CollectionOnDriveBase<string>
{
    private static CollectionOnDrive wroteOnDrive = null;
    public bool removeDuplicates = false;

    public CollectionOnDrive(CollectionOnDriveArgs a) : base(a)
    {
    }

    public CollectionOnDrive(string file) : base(new CollectionOnDriveArgs { file = file })
    {
    }

    public async Task Load(string file)
    {
        a.file = file;
        await Load();
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
            AddRange(SHGetLines.GetLines(
#if ASYNC
                await
#endif
                    File.ReadAllTextAsync(a.file)));
            //CA.RemoveStringsEmpty2(this);
            if (removeDuplicates)
            {
                //CAG.RemoveDuplicitiesList<string>(this);
                var d = this.ToList();
                Clear();
                d = d.Distinct().ToList();
                AddRange(d);
            }
        }
    }
}