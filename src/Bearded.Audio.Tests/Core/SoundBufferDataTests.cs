using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography;
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

        var bufferChecksums = soundBufferData.Buffers
            .Select(buffer => new { Hash = calculateHash(buffer), Size = buffer.Length })
            .ToImmutableArray();

        var settings = StaticConfig.DefaultVerifySettings;
        settings.UseParameters(filename);
        return Verifier.Verify(new
        {
            Buffers = bufferChecksums,
            soundBufferData.Format,
            soundBufferData.SampleRate,
        }, settings);
    }

    private static string calculateHash(IEnumerable<short> data)
    {
        using var sha256 = SHA256.Create();

        var dataAsBytes = data.SelectMany(BitConverter.GetBytes).ToArray();
        var dataHashAsBytes = sha256.ComputeHash(dataAsBytes);
        var dataHashAsHex = dataHashAsBytes.Select(d => d.ToString("x2"));
        return string.Concat(dataHashAsHex);
    }

}
