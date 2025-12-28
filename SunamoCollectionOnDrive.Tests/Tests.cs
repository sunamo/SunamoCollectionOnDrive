// variables names: ok
namespace SunamoCollectionOnDrive.Tests;

using SunamoTest;

public class Tests
{
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