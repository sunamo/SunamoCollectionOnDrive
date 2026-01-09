// variables names: ok
namespace SunamoCollectionOnDrive._sunamo.SunamoExceptions;

/// <summary>
/// Helper class for creating exception messages with context information
/// </summary>
internal sealed partial class Exceptions
{
    #region Other
    /// <summary>
    /// Checks and formats a prefix string for exception messages
    /// </summary>
    /// <param name="prefix">Prefix to add before the exception message</param>
    /// <returns>Empty string if prefix is null/whitespace, otherwise prefix with colon separator</returns>
    internal static string CheckBefore(string prefix)
    {
        return string.IsNullOrWhiteSpace(prefix) ? string.Empty : prefix + ": ";
    }


    /// <summary>
    /// Gets information about where an exception occurred in the call stack
    /// </summary>
    /// <param name="shouldFillFirstTwo">Whether to extract type and method name from the first non-ThrowEx frame</param>
    /// <returns>Tuple containing type name, method name, and full stack trace</returns>
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
            var item = lines[i];
            if (shouldFillFirstTwo)
                if (!item.StartsWith("   at ThrowEx"))
                {
                    TypeAndMethodName(item, out type, out methodName);
                    shouldFillFirstTwo = false;
                }
            if (item.StartsWith("at System."))
            {
                lines.Add(string.Empty);
                lines.Add(string.Empty);
                break;
            }
        }
        return new Tuple<string, string, string>(type, methodName, string.Join(Environment.NewLine, lines));
    }
    /// <summary>
    /// Parses a stack trace line to extract type and method names
    /// </summary>
    /// <param name="line">Stack trace line to parse</param>
    /// <param name="type">Output parameter for the type name</param>
    /// <param name="methodName">Output parameter for the method name</param>
    internal static void TypeAndMethodName(string line, out string type, out string methodName)
    {
        var trimmedLine = line.Split("at ")[1].Trim();
        var text = trimmedLine.Split("(")[0];
        var nameParts = text.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        methodName = nameParts[^1];
        nameParts.RemoveAt(nameParts.Count - 1);
        type = string.Join(".", nameParts);
    }

    /// <summary>
    /// Gets the name of the calling method from the stack trace
    /// </summary>
    /// <param name="frameIndex">Number of frames to skip in the stack trace (1 = immediate caller)</param>
    /// <returns>Name of the calling method or error message if not found</returns>
    internal static string CallingMethod(int frameIndex = 1)
    {
        StackTrace stackTrace = new();
        var methodBase = stackTrace.GetFrame(frameIndex)?.GetMethod();
        if (methodBase == null)
        {
            return "Method name cannot be get";
        }
        var methodName = methodBase.Name;
        return methodName;
    }
    #endregion

    #region IsNullOrWhitespace
    internal readonly static StringBuilder AdditionalInfoInnerStringBuilder = new();
    internal readonly static StringBuilder AdditionalInfoStringBuilder = new();
    #endregion

    #region OnlyReturnString
    /// <summary>
    /// Creates a custom exception message with optional prefix
    /// </summary>
    /// <param name="prefix">Prefix to add before the message (typically class.method context)</param>
    /// <param name="message">The exception message</param>
    /// <returns>Formatted exception message</returns>
    internal static string? Custom(string prefix, string message)
    {
        return CheckBefore(prefix) + message;
    }
    #endregion

    /// <summary>
    /// Checks if a variable is null and creates an exception message if so
    /// </summary>
    /// <param name="prefix">Prefix to add before the message (typically class.method context)</param>
    /// <param name="variableName">Name of the variable being checked</param>
    /// <param name="variable">The variable to check</param>
    /// <returns>Exception message if variable is null, otherwise null</returns>
    internal static string? IsNull(string prefix, string variableName, object? variable)
    {
        return variable == null ? CheckBefore(prefix) + variableName + " " + "is null" + "." : null;
    }
}