namespace SunamoCollectionOnDrive._sunamo.SunamoExceptions;

internal sealed partial class Exceptions
{
    #region Other
    internal static string CheckBefore(string prefix)
    {
        return string.IsNullOrWhiteSpace(prefix) ? string.Empty : prefix + ": ";
    }

    internal static Tuple<string, string, string> PlaceOfException(bool shouldFillFirstTwo = true)
    {
        StackTrace stackTrace = new();
        var stackTraceText = stackTrace.ToString();
        var lines = stackTraceText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        lines.RemoveAt(0);
        var i = 0;
        string type = string.Empty;
        string methodName = string.Empty;
        for (; i < lines.Count; i++)
        {
            var line = lines[i];
            if (shouldFillFirstTwo)
                if (!line.StartsWith("   at ThrowEx"))
                {
                    TypeAndMethodName(line, out type, out methodName);
                    shouldFillFirstTwo = false;
                }
            if (line.StartsWith("at System."))
            {
                lines.Add(string.Empty);
                lines.Add(string.Empty);
                break;
            }
        }
        return new Tuple<string, string, string>(type, methodName, string.Join(Environment.NewLine, lines));
    }

    internal static void TypeAndMethodName(string line, out string type, out string methodName)
    {
        var trimmedLine = line.Split("at ")[1].Trim();
        var text = trimmedLine.Split("(")[0];
        var nameParts = text.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        methodName = nameParts[^1];
        nameParts.RemoveAt(nameParts.Count - 1);
        type = string.Join(".", nameParts);
    }

    internal static string CallingMethod(int frameIndex = 1)
    {
        StackTrace stackTrace = new();
        var methodBase = stackTrace.GetFrame(frameIndex)?.GetMethod();
        if (methodBase is null)
        {
            return "Method name could not be obtained";
        }
        var methodName = methodBase.Name;
        return methodName;
    }
    #endregion

    #region OnlyReturnString
    internal static string? Custom(string prefix, string message)
    {
        return CheckBefore(prefix) + message;
    }
    #endregion

    internal static string? IsNull(string prefix, string variableName, object? variable)
    {
        return variable is null ? CheckBefore(prefix) + variableName + " " + "is null" + "." : null;
    }
}
