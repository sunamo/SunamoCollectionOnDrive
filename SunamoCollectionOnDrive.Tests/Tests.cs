namespace SunamoCollectionOnDrive.Tests;

using SunamoTest;

public class Tests
{
    [Fact]
    public async Task LoadAddAndSaveTest()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "LoadAddAndSave.txt");

        CollectionOnDrive d = new CollectionOnDrive(TestLogger.Instance);
        await d.Load(path, true);
        d.Add("a");
        d.Add("c");

        await d.Save();

        await AddDTest();
    }

    [Fact]
    public async Task AddDTest()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "LoadAddAndSave.txt");

        CollectionOnDrive d = new CollectionOnDrive(TestLogger.Instance);
        await d.Load(path, true);
        d.Add("d");

        await d.Save();
    }
}