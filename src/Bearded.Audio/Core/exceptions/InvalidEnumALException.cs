using JetBrains.Annotations;
using OpenTK.Audio.OpenAL;

namespace Bearded.Audio;

/// <summary>
/// Thrown when a specified enum value is invalid in OpenAL.
/// </summary>
[PublicAPI]
public sealed class InvalidEnumALException : ALException
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public InvalidEnumALException(string message) : base(ALError.InvalidEnum, message) { }
}
