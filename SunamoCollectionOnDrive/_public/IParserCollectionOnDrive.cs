namespace SunamoCollectionOnDrive._public;

/// <summary>
/// Interface for types that can be serialized to and from strings in a CollectionOnDrive.
/// </summary>
public interface IParserCollectionOnDrive
{
    /// <summary>
    /// Parses a string representation and populates this instance's properties.
    /// </summary>
    /// <param name="text">String text to parse.</param>
    void Parse(string text);
}