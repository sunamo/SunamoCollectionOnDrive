namespace SunamoCollectionOnDrive._sunamo.SunamoStringGetLines;

/// <summary>
/// Helper class for splitting text into lines handling various newline formats.
/// </summary>
internal class SHGetLines
{
    /// <summary>
    /// Splits text into lines, handling all newline formats (\r\n, \n\r, \r, \n).
    /// </summary>
    /// <param name="text">Text to split into lines.</param>
    /// <returns>List of lines.</returns>
    internal static List<string> GetLines(string text)
    {
        var lines = text.Split(new[] { "\r\n", "\n\r" }, StringSplitOptions.None).ToList();
        SplitByUnixNewline(lines);
        return lines;
    }

    /// <summary>
    /// Further splits lines by Unix-style newlines (\r and \n separately).
    /// </summary>
    /// <param name="list">Lines to process.</param>
    private static void SplitByUnixNewline(List<string> list)
    {
        SplitBy(list, "\r");
        SplitBy(list, "\n");
    }

    /// <summary>
    /// Splits lines by a specific delimiter, validating that Windows/Mac newlines were already handled.
    /// </summary>
    /// <param name="list">Lines to split.</param>
    /// <param name="delimiter">Delimiter to split by (\r or \n).</param>
    /// <exception cref="Exception">Thrown if Windows/Mac newlines are still present when processing Unix newlines.</exception>
    private static void SplitBy(List<string> list, string delimiter)
    {
        for (var i = list.Count - 1; i >= 0; i--)
        {
            if (delimiter == "\r")
            {
                var windowsNewlineSplit = list[i].Split(new[] { "\r\n" }, StringSplitOptions.None);
                var reverseNewlineSplit = list[i].Split(new[] { "\n\r" }, StringSplitOptions.None);

                if (windowsNewlineSplit.Length > 1)
                    ThrowEx.Custom("cannot contain any \r\n, pass already split by this pattern");
                else if (reverseNewlineSplit.Length > 1)
                    ThrowEx.Custom("cannot contain any \n\r, pass already split by this pattern");
            }

            var splitSegments = list[i].Split(new[] { delimiter }, StringSplitOptions.None);

            if (splitSegments.Length > 1)
                InsertOnIndex(list, splitSegments.ToList(), i);
        }
    }

    /// <summary>
    /// Inserts items at a specific index in the list, removing the original element.
    /// </summary>
    /// <param name="list">List to modify.</param>
    /// <param name="insertList">Items to insert (will be reversed before insertion).</param>
    /// <param name="index">Index where to insert the items.</param>
    private static void InsertOnIndex(List<string> list, List<string> insertList, int index)
    {
        insertList.Reverse();

        list.RemoveAt(index);

        foreach (var item in insertList)
            list.Insert(index, item);
    }
}
