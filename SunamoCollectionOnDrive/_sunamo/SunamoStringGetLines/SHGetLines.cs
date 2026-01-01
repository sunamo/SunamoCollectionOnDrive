namespace SunamoCollectionOnDrive._sunamo.SunamoStringGetLines;

/// <summary>
/// Helper class for splitting text into lines handling various newline formats
/// </summary>
internal class SHGetLines
{
    /// <summary>
    /// Splits text into lines, handling all newline formats (\r\n, \n\r, \r, \n)
    /// </summary>
    /// <param name="text">Text to split into lines</param>
    /// <returns>List of lines</returns>
    internal static List<string> GetLines(string text)
    {
        var parts = text.Split(new[] { "\r\n", "\n\r" }, StringSplitOptions.None).ToList();
        SplitByUnixNewline(parts);
        return parts;
    }

    /// <summary>
    /// Further splits lines by Unix-style newlines (\r and \n separately)
    /// </summary>
    /// <param name="lines">Lines to process</param>
    private static void SplitByUnixNewline(List<string> lines)
    {
        SplitBy(lines, "\r");
        SplitBy(lines, "\n");
    }

    /// <summary>
    /// Splits lines by a specific delimiter, validating that Windows/Mac newlines were already handled
    /// </summary>
    /// <param name="lines">Lines to split</param>
    /// <param name="delimiter">Delimiter to split by (\r or \n)</param>
    /// <exception cref="Exception">Thrown if Windows/Mac newlines are still present when processing Unix newlines</exception>
    private static void SplitBy(List<string> lines, string delimiter)
    {
        for (var i = lines.Count - 1; i >= 0; i--)
        {
            if (delimiter == "\r")
            {
                var rnParts = lines[i].Split(new[] { "\r\n" }, StringSplitOptions.None);
                var nrParts = lines[i].Split(new[] { "\n\r" }, StringSplitOptions.None);

                if (rnParts.Length > 1)
                    ThrowEx.Custom("cannot contain any \r\n, pass already split by this pattern");
                else if (nrParts.Length > 1)
                    ThrowEx.Custom("cannot contain any \n\r, pass already split by this pattern");
            }

            var splitParts = lines[i].Split(new[] { delimiter }, StringSplitOptions.None);

            if (splitParts.Length > 1)
                InsertOnIndex(lines, splitParts.ToList(), i);
        }
    }

    /// <summary>
    /// Inserts items at a specific index in the list, removing the original item
    /// </summary>
    /// <param name="lines">List to modify</param>
    /// <param name="itemsToInsert">Items to insert (will be reversed before insertion)</param>
    /// <param name="index">Index where to insert the items</param>
    private static void InsertOnIndex(List<string> lines, List<string> itemsToInsert, int index)
    {
        itemsToInsert.Reverse();

        lines.RemoveAt(index);

        foreach (var item in itemsToInsert)
            lines.Insert(index, item);
    }
}