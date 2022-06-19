using System;
using OpenTK.Audio.OpenAL;

namespace Bearded.Audio;

/// <summary>
/// Class representing a group of OpenAL audio buffers.
/// </summary>
public sealed class SoundBuffer : IDisposable
{
    /// <summary>
    /// List of OpenAL buffer handles.
    /// </summary>
    private readonly int[] handles;

    /// <summary>
    /// Disposal state of this buffer.
    /// </summary>
    public bool Disposed { get; private set; }

    /// <summary>
    /// Generates a new sound buffer of the given size.
    /// </summary>
    /// <param name="n">The number of buffers to reserve.</param>
    public SoundBuffer(int n)
    {
        handles = AudioContext.Instance.Eval(AL.GenBuffers, n);
    }

    /// <summary>
    /// Generates a news sound buffer and fills it.
    /// </summary>
    /// <param name="data">The data to load the buffers with.</param>
    public SoundBuffer(SoundBufferData data)
        : this(data.Buffers.Count)
    {
        FillBuffer(data);
    }

    private void fillBufferRaw(int index, short[] data, ALFormat format, int sampleRate)
    {
        if (index < 0 || index >= handles.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        int handle = this[index];
        AudioContext.Instance.Call(AL.BufferData, handle, format, data, sampleRate);
    }

    /// <summary>
    /// Fills the buffer with new data.
    /// </summary>
    /// <param name="data">The new content of the buffers.</param>
    public void FillBuffer(SoundBufferData data)
    {
        FillBuffer(0, data);
    }

    /// <summary>
    /// Fills the buffer with new data.
    /// </summary>
    /// <param name="index">The starting index from where to fill the buffer.</param>
    /// <param name="data">The new content of the buffers.</param>
    public void FillBuffer(int index, SoundBufferData data)
    {
        if (index < 0 || index >= handles.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        if (data.Buffers.Count > handles.Length)
        {
            throw new ArgumentException("This data does not fit in the buffer.", nameof(data));
        }

        for (var i = 0; i < data.Buffers.Count; i++)
        {
            fillBufferRaw((index + i) % handles.Length, data.Buffers[i], data.Format, data.SampleRate);
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        if (Disposed)
        {
            return;
        }

        AudioContext.Instance.Call(AL.DeleteBuffers, (int[])this);

        Disposed = true;
    }

    /// <summary>
    /// Casts the buffer to an integer array.
    /// </summary>
    /// <param name="buffer">The buffer that should be casted.</param>
    /// <returns>The OpenAL handles of the buffers.</returns>
    public static implicit operator int[](SoundBuffer buffer)
    {
        return buffer.handles;
    }

    /// <summary>
    /// Gets the <see cref="T:Bearded.Audio.SoundBuffer"/> handle at the specified index.
    /// </summary>
    /// <param name="i">The index.</param>
    public int this[int i] => handles[i];
}
