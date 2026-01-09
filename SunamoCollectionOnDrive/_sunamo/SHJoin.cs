// variables names: ok
namespace SunamoCollectionOnDrive._sunamo;

/// <summary>
/// Helper class for joining collections with newlines
/// </summary>
internal class SHJoin
{
    /// <summary>
    /// Joins a list of items with newline separators
    /// </summary>
    /// <typeparam name="T">Type of items in the list</typeparam>
    /// <param name="list">List of items to join</param>
    /// <returns>String with items separated by newlines</returns>
    internal static string JoinNL<T>(List<T> list)
    {
        var stringValues = list.ConvertAll(item => item?.ToString() ?? string.Empty);
        return string.Join("\n", stringValues);
    }
}