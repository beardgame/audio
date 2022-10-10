using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace Bearded.Audio.Tests;

[UsesVerify]
public sealed class SoundBufferDataTests
{
    [Theory]
    [InlineData("pcm_8000hz_8bit_stereo")]
    [InlineData("pcm_11025hz_8bit_mono")]
    public Task VerifyWavDeserialization(string filename)
    {
        var soundBufferData = SoundBufferData.FromWav($"assets/{filename}.wav");
        var buffers = soundBufferData.Buffers.Select(toBase64String).ToArray();

        var settings = StaticConfig.DefaultVerifySettings;
        settings.UseParameters(filename);
        return Verifier.Verify(new
        {
            Buffers = buffers,
            Format = soundBufferData.Format,
            SampleRate = soundBufferData.SampleRate,
        }, settings);
    }

    private static string toBase64String(IEnumerable<short> arr)
    {
        return string.Join(' ', arr.Select(i => Convert.ToBase64String(BitConverter.GetBytes(i))));
    }
}
