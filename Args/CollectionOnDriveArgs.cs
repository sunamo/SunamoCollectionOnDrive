namespace SunamoCollectionOnDrive.Args;

public class CollectionOnDriveArgs
{
    public string file;

    /// <summary>
    ///     Originally was false but think that true is better
    ///     Its not important because still I'm using old ctor interface and it will set to false if needed
    /// </summary>
    public bool load = true;

    public bool loadChangesFromDrive = true;
    public bool save = true;
}