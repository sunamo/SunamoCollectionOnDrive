namespace SunamoCollectionOnDrive._sunamo;
using System.Collections.Generic;

internal class SHJoin
{
    internal static string JoinNL<T>(List<T> list)
    {
        var ts = list.ConvertAll(d => d.ToString());
        return string.Join("\n", ts);
    }
}