using System;
using JetBrains.Annotations;
using OpenTK.Audio.OpenAL;

namespace Bearded.Audio;

/// <summary>
/// Generic OpenAL exception.
/// </summary>
[PublicAPI]
public class ALException : Exception
{
    /// <summary>
    /// The OpenAL error type.
    /// </summary>
    public ALError Type { get; }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="type">The OpenAL error type.</param>
    /// <param name="message">The exception message.</param>
    protected ALException(ALError type, string message) : base(message)
    {
        Type = type;
    }
}
