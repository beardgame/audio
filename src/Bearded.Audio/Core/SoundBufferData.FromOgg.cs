using System;
using System.IO;
using OpenTK.Audio.OpenAL;

namespace Bearded.Audio;

public sealed partial class SoundBufferData
{
    /// <summary>
    /// Extracts the buffer data from an ogg file.
    /// </summary>
    /// <param name="file">The file to load the data from.</param>
    /// <returns>A SoundBufferData object containing the data from the specified file.</returns>
    public static SoundBufferData FromOgg(string file)
    {
        return FromOgg(File.OpenRead(file));
    }

    /// <summary>
    /// Extracts the buffer data from an ogg file.
    /// </summary>
    /// <param name="file">The file to load the data from.</param>
    /// <returns>A SoundBufferData object containing the data from the specified file.</returns>
    public static SoundBufferData FromOgg(Stream file)
    {
        using var reader = OggReader.FromStream(file);
        var buffers = reader.ReadAllRemainingBuffers(maxBufferSize);
        return new SoundBufferData(buffers, getSoundFormat(reader.ChannelCount), reader.SampleRate);
    }

    private static ALFormat getSoundFormat(int channels) => channels switch
    {
        1 => ALFormat.Mono16,
        2 => ALFormat.Stereo16,
        _ => throw new NotSupportedException("The specified sound format is not supported.")
    };
}
