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

        readFileHeader(reader, out var numChannels, out var sampleRate, out var bitsPerSample);

        var alFormat = getSoundFormat(numChannels, bitsPerSample);

        var data = reader.ReadBytes((int) reader.BaseStream.Length);
        var buffers = convertToBuffers(data);

        return new SoundBufferData(buffers, alFormat, sampleRate);
    }

    private static void readFileHeader(
        BinaryReader reader, out int numChannels, out int sampleRate, out int bitsPerSample)
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

        // WAVE header
        var formatSignature = new string(reader.ReadChars(4));
        if (formatSignature != "fmt ")
        {
            throw new NotSupportedException("Specified wave file is not supported.");
        }

        var formatChunkSize = reader.ReadInt32();
        reader.ReadInt16(); // audioFormat (unused)
        numChannels = reader.ReadInt16();
        sampleRate = reader.ReadInt32();
        reader.ReadInt32(); // byteRate (unused)
        reader.ReadInt16(); // blockAlign (unused)
        bitsPerSample = reader.ReadInt16();

        // We have read 16 bytes so far. If the format chunk size is more than 16 bytes, make sure we move the reader
        // cursor to the end of the format chunk size.
        if (formatChunkSize > 16)
        {
            reader.ReadBytes(formatChunkSize - 16);
        }

        var dataSignature = new string(reader.ReadChars(4));
        if (dataSignature != "data")
        {
            throw new NotSupportedException("Only uncompressed wave files are supported.");
        }

        reader.ReadInt32(); // dataChunkSize (unused)
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
        const int maxBufferSize = 16384;

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
