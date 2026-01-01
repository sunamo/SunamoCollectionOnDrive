namespace SunamoCollectionOnDrive._sunamo.SunamoExceptions;

/// <summary>
/// Helper class for throwing exceptions with context information
/// </summary>
internal partial class ThrowEx
{
    /// <summary>
    /// Throws or returns a custom exception with optional second message
    /// </summary>
    /// <param name="message">Primary exception message</param>
    /// <param name="shouldThrow">Whether to actually throw the exception or just return true</param>
    /// <param name="secondMessage">Optional additional message to append</param>
    /// <returns>True if exception would be thrown, false if message is null/empty</returns>
    internal static bool Custom(string message, bool shouldThrow = true, string secondMessage = "")
    {
        string joined = string.Join(" ", message, secondMessage);
        string? exceptionMessage = Exceptions.Custom(FullNameOfExecutedCode(), joined);
        return ThrowIsNotNull(exceptionMessage, shouldThrow);
    }
    /// <summary>
    /// Checks if a variable is null and throws an exception if so
    /// </summary>
    /// <param name="variableName">Name of the variable to check</param>
    /// <param name="variable">The variable to check for null</param>
    /// <returns>True if exception was thrown, false otherwise</returns>
    internal static bool IsNull(string variableName, object? variable = null)
    {
        return ThrowIsNotNull(Exceptions.IsNull(FullNameOfExecutedCode(), variableName, variable));
    }

    #region Other
    /// <summary>
    /// Gets the full name (type.method) of the code location where this was called
    /// </summary>
    /// <returns>Full name in format Type.Method</returns>
    internal static string FullNameOfExecutedCode()
    {
        Tuple<string, string, string> placeOfException = Exceptions.PlaceOfException();
        string fullName = FullNameOfExecutedCode(placeOfException.Item1, placeOfException.Item2, true);
        return fullName;
    }

    /// <summary>
    /// Gets the full name (type.method) from various type representations
    /// </summary>
    /// <param name="type">Type object, can be Type, MethodBase, string, or any object</param>
    /// <param name="methodName">Method name, if null will be extracted from stack trace</param>
    /// <param name="fromThrowEx">Whether this is called from ThrowEx (adjusts stack depth)</param>
    /// <returns>Full name in format Type.Method</returns>
    static string FullNameOfExecutedCode(object type, string methodName, bool fromThrowEx = false)
    {
        if (methodName == null)
        {
            int depth = 2;
            if (fromThrowEx)
            {
                depth++;
            }

            methodName = Exceptions.CallingMethod(depth);
        }
        string typeFullName;
        if (type is Type actualType)
        {
            typeFullName = actualType.FullName ?? "Type cannot be get via type is Type actualType";
        }
        else if (type is MethodBase method)
        {
            typeFullName = method.ReflectedType?.FullName ?? "Type cannot be get via type is MethodBase method";
            methodName = method.Name;
        }
        else if (type is string)
        {
            typeFullName = type.ToString() ?? "Type cannot be get via type is string";
        }
        else
        {
            Type objectType = type.GetType();
            typeFullName = objectType.FullName ?? "Type cannot be get via type.GetType()";
        }
        return string.Concat(typeFullName, ".", methodName);
    }

    /// <summary>
    /// Throws an exception if the message is not null, or returns whether it would throw
    /// </summary>
    /// <param name="exception">Exception message, null if no exception should be thrown</param>
    /// <param name="shouldThrow">Whether to actually throw the exception or just return true</param>
    /// <returns>True if exception was/would be thrown, false if message is null</returns>
    internal static bool ThrowIsNotNull(string? exception, bool shouldThrow = true)
    {
        if (exception != null)
        {
            Debugger.Break();
            if (shouldThrow)
            {
                throw new Exception(exception);
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Throws an exception indicating that a dummy collection instance cannot be used for operations
    /// </summary>
    /// <exception cref="NotImplementedException">Always thrown</exception>
    internal static void UseNonDummyCollection()
    {
        throw new NotImplementedException();
    }
    #endregion
}