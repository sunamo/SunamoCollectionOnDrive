// variables names: ok
namespace SunamoCollectionOnDrive.Tests;

using SunamoTest;

/// <summary>
/// Tests for CollectionOnDrive functionality
/// </summary>
public class Tests
{
    /// <summary>
    /// Tests loading, adding items, and saving to disk
    /// </summary>
    [Fact]
    public async Task LoadAddAndSaveTest()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "LoadAddAndSave.txt");

        CollectionOnDrive collection = new CollectionOnDrive(TestLogger.Instance);
        await collection.Load(path, true);
        collection.Add("a");
        collection.Add("c");

        await collection.Save();

        await AddDTest();
    }

    /// <summary>
    /// Tests adding an item 'd' to the collection
    /// </summary>
    [Fact]
    public async Task AddDTest()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "LoadAddAndSave.txt");

        CollectionOnDrive collection = new CollectionOnDrive(TestLogger.Instance);
        await collection.Load(path, true);
        collection.Add("d");

        await collection.Save();
    }
}