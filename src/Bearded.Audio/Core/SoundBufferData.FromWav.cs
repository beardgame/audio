using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Audio.OpenAL;

namespace Bearded.Audio;

public sealed partial class SoundBufferData
{
    /// <summary>
    /// Extracts the buffer data from an uncompressed wave-file.
    /// </summary>
    /// <param name="file">The file to load the data from.</param>
    /// <returns>A SoundBufferData object containing the data from the specified file.</returns>
    public static SoundBufferData FromWav(string file)
    {
        return FromWav(File.OpenRead(file));
    }

    /// <summary>
    /// Extracts the buffer data from an uncompressed wave-file.
    /// </summary>
    /// <param name="file">The file to load the data from.</param>
    /// <returns>A SoundBufferData object containing the data from the specified file.</returns>
    public static SoundBufferData FromWav(Stream file)
    {
        using var reader = new BinaryReader(file);

        readFileHeader(reader, out var numChannels, out var sampleRate, out var bitsPerSample, out var dataSize);

        var alFormat = getSoundFormat(numChannels, bitsPerSample);

        var data = reader.ReadBytes(dataSize);
        var buffers = convertToBuffers(data);

        return new SoundBufferData(buffers, alFormat, sampleRate);
    }

    private static void readFileHeader(
        BinaryReader reader, out short numChannels, out int sampleRate, out short bitsPerSample, out int dataSize)
    {
        // RIFF header
        var signature = new string(reader.ReadChars(4));
        if (signature != "RIFF")
        {
            throw new NotSupportedException("Specified stream is not a wave file.");
        }

        reader.ReadInt32(); // riffChunkSize (unused)

        var format = new string(reader.ReadChars(4));
        if (format != "WAVE")
        {
            throw new NotSupportedException("Specified stream is not a wave file.");
        }

        numChannels = 0;
        sampleRate = 0;
        bitsPerSample = 0;
        dataSize = 0;

        while (reader.BaseStream.Position < reader.BaseStream.Length)
        {
            var chunkId = new string(reader.ReadChars(4));
            var chunkSize = reader.ReadInt32();

            if (chunkId == "fmt ")
            {
                reader.ReadInt16(); // audioFormat (unused)

                numChannels = reader.ReadInt16();
                sampleRate = reader.ReadInt32();
                reader.ReadInt32(); // byteRate (unused)
                reader.ReadInt16(); // blockAlign (unused)
                bitsPerSample = reader.ReadInt16();

                if (bitsPerSample != 8 && bitsPerSample != 16 && bitsPerSample != 24 && bitsPerSample != 32)
                {
                    throw new NotSupportedException($"Unsupported bits per sample: {bitsPerSample}.");
                }

                // We have read 16 bytes so far. If the format chunk size is more than 16 bytes, make sure we move the
                // reader cursor to the end of the format chunk size.
                if (chunkSize > 16)
                {
                    reader.ReadBytes(chunkSize - 16);
                }

                continue;
            }

            if (chunkId == "data")
            {
                dataSize = chunkSize;
                break;
            }

            // Unknown chunk, skip it
            reader.ReadBytes(chunkSize);
        }

        if (dataSize == 0)
        {
            throw new NotSupportedException("No data chunk found.");
        }
    }

    private static ALFormat getSoundFormat(int channels, int bits) => (channels, bits) switch
    {
        (1, 8) => ALFormat.Mono8,
        (1, 16) => ALFormat.Mono16,
        (2, 8) => ALFormat.Stereo8,
        (2, 16) => ALFormat.Stereo16,
        _ => throw new NotSupportedException("The specified sound format is not supported.")
    };

    private static List<short[]> convertToBuffers(byte[] data)
    {
        var dataPointCount = data.Length / 2;
        var buffersNeeded = dataPointCount / maxBufferSize + (dataPointCount % maxBufferSize == 0 ? 0 : 1);

        var buffers = new List<short[]>();

        for (var i = 0; i < buffersNeeded; i++)
        {
            var startOffset = i * maxBufferSize * 2;
            var dataPointsRemaining = (data.Length - startOffset) / 2;
            var bufferSize = Math.Min(maxBufferSize, dataPointsRemaining);
            var buffer = new short[bufferSize];
            copyAsShorts(data, buffer, bufferSize, startOffset);
            buffers.Add(buffer);
        }

        return buffers;
    }

    private static void copyAsShorts(byte[] inBuffer, short[] outBuffer, int length, int inOffset = 0)
    {
        for (var i = 0; i < length; i++)
        {
            outBuffer[i] = BitConverter.ToInt16(inBuffer, inOffset + 2 * i);
        }
    }
}
