// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoCollectionOnDrive._sunamo;

internal class SHJoin
{
    internal static string JoinNL<T>(List<T> list)
    {
        var ts = list.ConvertAll(d => d.ToString());
        return string.Join("\n", ts);
    }
}