using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using OpenTK.Audio.OpenAL;

namespace Bearded.Audio;

/// <summary>
/// Class representing sound data that can be loaded into OpenAL sound buffers.
/// </summary>
[PublicAPI]
public sealed partial class SoundBufferData
{
    internal IList<short[]> Buffers { get; }
    internal ALFormat Format { get; }
    internal int SampleRate { get; }

    /// <summary>
    /// Creates a new sound buffer with the specified data.
    /// </summary>
    /// <param name="buffers">The content of the buffers.</param>
    /// <param name="format">The format the buffers are in.</param>
    /// <param name="sampleRate">The sample rate of the buffers.</param>
    public SoundBufferData(IList<short[]> buffers, ALFormat format, int sampleRate)
    {
        Buffers = buffers;
        Format = format;
        SampleRate = sampleRate;
    }
}
