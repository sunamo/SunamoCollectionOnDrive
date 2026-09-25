namespace SunamoCollectionOnDrive._sunamo.SunamoExceptions;

internal partial class ThrowEx
{
    internal static bool Custom(string message, bool shouldThrow = true, string secondMessage = "")
    {
        string joined = string.Join(" ", message, secondMessage);
        string? exceptionMessage = Exceptions.Custom(FullNameOfExecutedCode(), joined);
        return ThrowIsNotNull(exceptionMessage, shouldThrow);
    }

    internal static bool IsNull(string variableName, object? variable = null)
    {
        return ThrowIsNotNull(Exceptions.IsNull(FullNameOfExecutedCode(), variableName, variable));
    }

    #region Other
    internal static string FullNameOfExecutedCode()
    {
        Tuple<string, string, string> placeOfException = Exceptions.PlaceOfException();
        string fullName = FullNameOfExecutedCode(placeOfException.Item1, placeOfException.Item2, true);
        return fullName;
    }

    static string FullNameOfExecutedCode(object typeSource, string methodName, bool isFromThrowEx = false)
    {
        if (methodName is null)
        {
            int depth = 2;
            if (isFromThrowEx)
            {
                depth++;
            }

            methodName = Exceptions.CallingMethod(depth);
        }
        string typeFullName;
        if (typeSource is Type actualType)
        {
            typeFullName = actualType.FullName ?? "Type could not be obtained via Type pattern match";
        }
        else if (typeSource is MethodBase methodBase)
        {
            typeFullName = methodBase.ReflectedType?.FullName ?? "Type could not be obtained via MethodBase pattern match";
            methodName = methodBase.Name;
        }
        else if (typeSource is string)
        {
            typeFullName = typeSource.ToString() ?? "Type could not be obtained via string conversion";
        }
        else
        {
            Type objectType = typeSource.GetType();
            typeFullName = objectType.FullName ?? "Type could not be obtained via GetType()";
        }
        return string.Concat(typeFullName, ".", methodName);
    }

    internal static bool ThrowIsNotNull(string? exceptionMessage, bool shouldThrow = true)
    {
        if (exceptionMessage is not null)
        {
            Debugger.Break();
            if (shouldThrow)
            {
                throw new Exception(exceptionMessage);
            }
            return true;
        }
        return false;
    }

    internal static void UseNonDummyCollection()
    {
        throw new NotImplementedException();
    }
    #endregion
}
