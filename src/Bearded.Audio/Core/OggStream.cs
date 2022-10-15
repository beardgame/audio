using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using NVorbis;

namespace Bearded.Audio;

sealed class OggStream : IDisposable
{
    private readonly VorbisReader reader;

    public int ChannelCount => reader.Channels;

    public int SampleRate => reader.SampleRate;

    public bool Ended => reader.IsEndOfStream;

    private OggStream(VorbisReader reader)
    {
        this.reader = reader;
    }

    public IList<short[]> ReadAllRemainingBuffers(int maxBufferSize)
    {
        if (Ended)
        {
            return ImmutableArray<short[]>.Empty;
        }

        var bufferSize = largestBufferSizeDivisibleByChannelCount(maxBufferSize);
        var totalSampleCountRemaining = reader.TotalSamples - reader.SamplePosition;
        var fullBuffers = totalSampleCountRemaining / bufferSize;
        if (fullBuffers > int.MaxValue)
        {
            throw new InvalidOperationException(
                $"Cannot read a stream with more than {int.MaxValue} buffers remaining.");
        }
        var partialBuffers = totalSampleCountRemaining % bufferSize > 0 ? 1 : 0;
        var totalBuffersNeeded = (int) fullBuffers + partialBuffers;

        return Enumerable.Range(0, totalBuffersNeeded).Select(_ =>
        {
            TryReadSingleBuffer(out var buffer, maxBufferSize);
            return buffer!;
        }).ToImmutableArray();
    }

    public bool TryReadSingleBuffer([NotNullWhen(true)] out short[]? buffer, int maxBufferSize)
    {
        if (maxBufferSize <= 0)
        {
            throw new ArgumentException("Max buffer size must be positive.", nameof(maxBufferSize));
        }

        if (reader.IsEndOfStream)
        {
            buffer = default;
            return false;
        }

        var floatBuffer = new float[largestBufferSizeDivisibleByChannelCount(maxBufferSize)];

        var numSamplesRead = reader.ReadSamples(new Span<float>(floatBuffer));

        buffer = new short[numSamplesRead];
        const short maxSafeValue = 32767;
        for (var i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (short) (maxSafeValue * floatBuffer[i]);
        }

        return true;
    }

    private int largestBufferSizeDivisibleByChannelCount(int maxBufferSize)
    {
        return maxBufferSize - (maxBufferSize % ChannelCount);
    }

    public void Dispose()
    {
        reader.Dispose();
    }

    public static OggStream FromFile(Stream file)
    {
        return new OggStream(new VorbisReader(file) { ClipSamples = true });
    }
}
