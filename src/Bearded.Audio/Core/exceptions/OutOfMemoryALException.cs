﻿using JetBrains.Annotations;
using OpenTK.Audio.OpenAL;

namespace Bearded.Audio;

/// <summary>
/// Thrown when OpenAL runs out of memory.
/// </summary>
[PublicAPI]
public sealed class OutOfMemoryALException : ALException
{
    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="message">Exception message.</param>
    public OutOfMemoryALException(string message) : base(ALError.OutOfMemory, message) { }
}
