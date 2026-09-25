namespace SunamoCollectionOnDrive._sunamo.SunamoStringGetLines;

internal class SHGetLines
{
    internal static List<string> GetLines(string text)
    {
        var lines = text.Split(new[] { "\r\n", "\n\r" }, StringSplitOptions.None).ToList();
        SplitByUnixNewline(lines);
        return lines;
    }

    private static void SplitByUnixNewline(List<string> list)
    {
        SplitBy(list, "\r");
        SplitBy(list, "\n");
    }

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

    private static void InsertOnIndex(List<string> list, List<string> insertList, int index)
    {
        insertList.Reverse();

        list.RemoveAt(index);

        foreach (var item in insertList)
            list.Insert(index, item);
    }
}
