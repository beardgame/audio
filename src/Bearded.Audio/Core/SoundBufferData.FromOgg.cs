using System;
using System.IO;
using OpenTK.Audio.OpenAL;

namespace Bearded.Audio;

public sealed partial class SoundBufferData
{
    public static SoundBufferData FromOgg(string file)
    {
        return FromOgg(File.OpenRead(file));
    }

    public static SoundBufferData FromOgg(Stream file)
    {
        using var stream = OggStream.FromFile(file);
        var buffers = stream.ReadAllRemainingBuffers(maxBufferSize);
        return new SoundBufferData(buffers, getSoundFormat(stream.ChannelCount), stream.SampleRate);
    }

    private static ALFormat getSoundFormat(int channels) => channels switch
    {
        1 => ALFormat.Mono16,
        2 => ALFormat.Stereo16,
        _ => throw new NotSupportedException("The specified sound format is not supported.")
    };
}
