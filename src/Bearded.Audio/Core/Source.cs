using System;
using JetBrains.Annotations;
using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;

namespace Bearded.Audio;

/// <summary>
/// Class representing an OpenAL audio source.
/// </summary>
[PublicAPI]
public sealed class Source : IDisposable
{
    private readonly int handle;

    /// <summary>
    /// Disposal state of this source.
    /// </summary>
    public bool Disposed { get; private set; }

    /// <summary>
    /// The current state of this source.
    /// </summary>
    public ALSourceState State =>
        (ALSourceState) AudioContext.Instance.Eval(AL.GetSource, (int) this, ALGetSourcei.SourceState);

    /// <summary>
    /// The amount of buffers the source has already played.
    /// </summary>
    public int ProcessedBuffers
    {
        get
        {
            AL.GetSource((int) this, ALGetSourcei.BuffersProcessed, out var value);
            AudioContext.Instance.CheckErrors();
            return value;
        }
    }

    /// <summary>
    /// The total amount of buffers to source has queued to play.
    /// </summary>
    public int QueuedBuffers
    {
        get
        {
            AL.GetSource((int) this, ALGetSourcei.BuffersQueued, out var value);
            AudioContext.Instance.CheckErrors();
            return value;
        }
    }

    /// <summary>
    /// Whether the source is finished playing all queued buffers.
    /// </summary>
    public bool FinishedPlaying => ProcessedBuffers >= QueuedBuffers && !looping;

    private float gain;
    private float pitch;
    private bool looping;
    private Vector3 position;
    private bool positionIsRelative;
    private Vector3 velocity;

    /// <summary>
    /// The volume at which the source plays its buffers.
    /// </summary>
    public float Gain
    {
        get => gain;
        set
        {
            gain = value;
            AudioContext.Instance.Call(AL.Source, (int) this, ALSourcef.Gain, gain);
        }
    }

    /// <summary>
    /// The pitch at which the source plays its buffers.
    /// </summary>
    public float Pitch
    {
        get => pitch;
        set
        {
            pitch = value;
            AudioContext.Instance.Call(AL.Source, (int) this, ALSourcef.Pitch, pitch);
        }
    }

    /// <summary>
    /// Whether the source should repeat itself or not.
    /// </summary>
    public bool Looping
    {
        get => looping;
        set
        {
            looping = value;
            AudioContext.Instance.Call(AL.Source, (int) this, ALSourceb.Looping, looping);
        }
    }

    /// <summary>
    /// The position of the audio source in 3D space.
    /// </summary>
    /// <remarks>With the default listener, OpenAL uses a right hand coordinate system, with x pointing right, y pointing up, and z pointing towards the viewer.</remarks>
    public Vector3 Position
    {
        get => position;
        set
        {
            position = value;
            AudioContext.Instance.Call(() => AL.Source((int) this, ALSource3f.Position, ref position));
        }
    }

    /// <summary>
    /// Whether the position of the source is relative to the listener or not.
    /// </summary>
    public bool PositionIsRelative
    {
        get => positionIsRelative;
        set
        {
            positionIsRelative = value;
            AudioContext.Instance.Call(AL.Source, (int) this, ALSourceb.SourceRelative, positionIsRelative);
        }
    }

    /// <summary>
    /// The velocity of the audio source in 3D space.
    /// </summary>
    /// <remarks>With the default listener, OpenAL uses a right hand coordinate system, with x pointing right, y pointing up, and z pointing towards the viewer.</remarks>
    public Vector3 Velocity
    {
        get => velocity;
        set
        {
            velocity = value;
            AudioContext.Instance.Call(() => AL.Source((int) this, ALSource3f.Position, ref velocity));
        }
    }

    /// <summary>
    /// Creates a new OpenAL source.
    /// </summary>
    public Source()
    {
        handle = AudioContext.Instance.Eval(AL.GenSource);

        gain = 1;
        pitch = 1;
    }

    private void queueBuffersRaw(int bufferLength, int[] bufferIDs)
    {
        AudioContext.Instance.Call(AL.SourceQueueBuffers, (int) this, bufferLength, bufferIDs);
    }

    /// <summary>
    /// Queues a sound buffer to be played by this source.
    /// </summary>
    /// <param name="buffer"></param>
    public void QueueBuffer(SoundBuffer buffer)
    {
        var handles = (int[]) buffer;
        queueBuffersRaw(handles.Length, handles);
    }

    /// <summary>
    /// Removes all the buffers from the source.
    /// </summary>
    public void DequeueBuffers()
    {
        if (QueuedBuffers == 0)
        {
            return;
        }

        AudioContext.Instance.Eval(AL.SourceUnqueueBuffers, (int) this, QueuedBuffers);
    }

    /// <summary>
    /// Removes all the processed buffers from the source.
    /// </summary>
    public void DequeueProcessedBuffers()
    {
        if (ProcessedBuffers == 0)
        {
            return;
        }

        AudioContext.Instance.Eval(AL.SourceUnqueueBuffers, (int) this, ProcessedBuffers);
    }

    /// <summary>
    /// Starts playing the source.
    /// </summary>
    public void Play()
    {
        AudioContext.Instance.Call(AL.SourcePlay, (int) this);
    }

    /// <summary>
    /// Pauses playing the source.
    /// </summary>
    public void Pause()
    {
        AudioContext.Instance.Call(AL.SourcePause, (int) this);
    }

    /// <summary>
    /// Stops playing the source.
    /// </summary>
    public void Stop()
    {
        AudioContext.Instance.Call(AL.SourceStop, (int) this);
    }

    /// <summary>
    /// Rewinds the source.
    /// </summary>
    public void Rewind()
    {
        AudioContext.Instance.Call(AL.SourceRewind, (int) this);
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

        if (State != ALSourceState.Stopped)
        {
            Stop();
        }

        AudioContext.Instance.Call(AL.DeleteSource, (int) this);
        Disposed = true;
    }

    /// <summary>
    /// Casts the source to an integer.
    /// </summary>
    /// <param name="source">The source that should be casted.</param>
    /// <returns>The OpenAL handle of the source.</returns>
    public static implicit operator int(Source source)
    {
        return source.handle;
    }
}
