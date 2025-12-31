namespace SunamoCollectionOnDrive._sunamo;

internal class SHJoin
{
    internal static string JoinNL<T>(List<T> list)
    {
        var stringValues = list.ConvertAll(item => item.ToString());
        return string.Join("\n", stringValues);
    }
}