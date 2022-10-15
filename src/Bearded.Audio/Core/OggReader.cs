using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using NVorbis;

namespace Bearded.Audio;

sealed class OggReader : IDisposable
{
    private readonly VorbisReader reader;

    public int ChannelCount => reader.Channels;

    public int SampleRate => reader.SampleRate;

    public bool Ended => reader.IsEndOfStream;

    private OggReader(VorbisReader reader)
    {
        this.reader = reader;
    }

    public ImmutableArray<short[]> ReadAllRemainingBuffers(int maxBufferSize)
    {
        if (maxBufferSize <= 0)
        {
            throw new ArgumentException("Max buffer size must be positive.", nameof(maxBufferSize));
        }
        if (Ended)
        {
            return ImmutableArray<short[]>.Empty;
        }

        var bufferSize = largestBufferSizeDivisibleByChannelCount(maxBufferSize);
        var totalSampleCountRemaining = reader.TotalSamples - reader.SamplePosition;
        var fullBuffersNeeded = totalSampleCountRemaining / bufferSize;
        var partialBuffersNeeded = totalSampleCountRemaining % bufferSize > 0 ? 1 : 0;
        var totalBuffersNeeded = fullBuffersNeeded + partialBuffersNeeded;

        if (totalBuffersNeeded > int.MaxValue)
        {
            throw new InvalidOperationException(
                $"Cannot read a stream with more than {int.MaxValue} buffers remaining.");
        }

        var builder = ImmutableArray.CreateBuilder<short[]>((int) totalBuffersNeeded);
        for (var i = 0; i < builder.Capacity; i++)
        {
            builder.Add(readSamples(bufferSize));
        }

        return builder.ToImmutable();
    }

    public bool TryReadSingleBuffer([NotNullWhen(true)] out short[]? buffer, int maxBufferSize)
    {
        if (maxBufferSize <= 0)
        {
            throw new ArgumentException("Max buffer size must be positive.", nameof(maxBufferSize));
        }

        if (Ended)
        {
            buffer = default;
            return false;
        }

        var bufferSize = largestBufferSizeDivisibleByChannelCount(maxBufferSize);
        buffer = readSamples(bufferSize);

        return true;
    }

    private short[] readSamples(int maxSampleCount)
    {
        const short maxSafeValue = 32767;

        Span<float> floatSpan = stackalloc float[maxSampleCount];

        var numSamplesRead = reader.ReadSamples(floatSpan);

        var buffer = new short[numSamplesRead];
        for (var i = 0; i < buffer.Length; i++)
        {
            buffer[i] = (short) (maxSafeValue * floatSpan[i]);
        }

        return buffer;
    }

    private int largestBufferSizeDivisibleByChannelCount(int maxBufferSize)
    {
        return maxBufferSize - (maxBufferSize % ChannelCount);
    }

    public void Dispose()
    {
        reader.Dispose();
    }

    public static OggReader FromStream(Stream file)
    {
        return new OggReader(new VorbisReader(file) { ClipSamples = true });
    }
}
