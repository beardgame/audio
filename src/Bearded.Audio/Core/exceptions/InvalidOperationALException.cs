using OpenTK.Audio.OpenAL;

namespace Bearded.Audio;

/// <summary>
/// Thrown when an invalid operation is executed.
/// </summary>
public sealed class InvalidOperationALException : ALException
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public InvalidOperationALException(string message) : base(ALError.InvalidOperation, message) { }
}
