namespace SunamoCollectionOnDrive._sunamo.SunamoStringGetLines;

internal class SHGetLines
{
    internal static List<string> GetLines(string text)
    {
        var parts = text.Split(new[] { "\r\n", "\n\r" }, StringSplitOptions.None).ToList();
        SplitByUnixNewline(parts);
        return parts;
    }

    private static void SplitByUnixNewline(List<string> lines)
    {
        SplitBy(lines, "\r");
        SplitBy(lines, "\n");
    }

    private static void SplitBy(List<string> lines, string delimiter)
    {
        for (var i = lines.Count - 1; i >= 0; i--)
        {
            if (delimiter == "\r")
            {
                var rnParts = lines[i].Split(new[] { "\r\n" }, StringSplitOptions.None);
                var nrParts = lines[i].Split(new[] { "\n\r" }, StringSplitOptions.None);

                if (rnParts.Length > 1)
                    ThrowEx.Custom("cannot contain any \r\name, pass already split by this pattern");
                else if (nrParts.Length > 1) ThrowEx.Custom("cannot contain any \n\r, pass already split by this pattern");
            }

            var splitParts = lines[i].Split(new[] { delimiter }, StringSplitOptions.None);

            if (splitParts.Length > 1) InsertOnIndex(lines, splitParts.ToList(), i);
        }
    }

    private static void InsertOnIndex(List<string> lines, List<string> itemsToInsert, int index)
    {
        itemsToInsert.Reverse();

        lines.RemoveAt(index);

        foreach (var item in itemsToInsert) lines.Insert(index, item);
    }
}