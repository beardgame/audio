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
public sealed class SoundBufferData
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

        // RIFF header
        var signature = new string(reader.ReadChars(4));
        if (signature != "RIFF")
        {
            throw new NotSupportedException("Specified stream is not a wave file.");
        }

        reader.ReadInt32(); // riffChunkSize

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
        reader.ReadInt16(); // audioFormat
        int numChannels = reader.ReadInt16();
        var sampleRate = reader.ReadInt32();
        reader.ReadInt32(); // byteRate
        reader.ReadInt16(); // blockAlign
        int bitsPerSample = reader.ReadInt16();

        if (formatChunkSize > 16)
        {
            reader.ReadBytes(formatChunkSize - 16);
        }

        var dataSignature = new string(reader.ReadChars(4));

        if (dataSignature != "data")
        {
            throw new NotSupportedException("Only uncompressed wave files are supported.");
        }

        reader.ReadInt32(); // dataChunkSize

        var alFormat = getSoundFormat(numChannels, bitsPerSample);

        var data = reader.ReadBytes((int) reader.BaseStream.Length);
        var buffers = new List<short[]>();
        int count;
        var i = 0;
        const int bufferSize = 16384;

        while ((count = (Math.Min(data.Length, (i + 1) * bufferSize * 2) - i * bufferSize * 2) / 2) > 0)
        {
            var buffer = new short[bufferSize];
            convertBuffer(data, buffer, count, i * bufferSize * 2);
            buffers.Add(buffer);
            i++;
        }

        return new SoundBufferData(buffers, alFormat, sampleRate);
    }

    private static void convertBuffer(byte[] inBuffer, short[] outBuffer, int length, int inOffset = 0)
    {
        for (var i = 0; i < length; i++)
        {
            outBuffer[i] = BitConverter.ToInt16(inBuffer, inOffset + 2 * i);
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
}
