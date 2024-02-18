using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace Bearded.Audio.Tests;

public sealed class SoundBufferDataTests
{
    [Theory]
    [InlineData("pcm_8000hz_8bit_stereo")]
    [InlineData("pcm_11025hz_8bit_mono")]
    public Task VerifyWavDeserialization(string filename)
    {
        var soundBufferData = SoundBufferData.FromWav($"assets/{filename}.wav");

        var obj = objectToCompare(soundBufferData);
        var settings = settingsForVerify(filename);
        return Verifier.Verify(obj, settings);
    }

    [Theory]
    [InlineData("44100hz_mono")]
    public Task VerifyOggDeserialization(string filename)
    {
        var soundBufferData = SoundBufferData.FromOgg($"assets/{filename}.ogg");

        var obj = objectToCompare(soundBufferData);
        var settings = settingsForVerify(filename);
        return Verifier.Verify(obj, settings);
    }

    private static object objectToCompare(SoundBufferData soundBufferData)
    {
        var buffers = soundBufferData.Buffers.Select(toBase64String).ToArray();
        return new
        {
            Buffers = buffers,
            soundBufferData.Format,
            soundBufferData.SampleRate,
        };
    }

    private static string toBase64String(IEnumerable<short> arr)
    {
        return string.Join(' ', arr.Select(i => Convert.ToBase64String(BitConverter.GetBytes(i))));
    }

    private static VerifySettings settingsForVerify(string filename)
    {
        var settings = StaticConfig.DefaultVerifySettings;
        settings.UseParameters(filename);
        return settings;
    }
}
