using System;
using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;

namespace Bearded.Audio;

/// <summary>
/// Class representing an OpenAL audio source.
/// </summary>
public sealed class Source : IDisposable
{
    private readonly ISourceService svc;
    private readonly int handle;

    /// <summary>
    /// Disposal state of this source.
    /// </summary>
    public bool Disposed { get; private set; }

    /// <summary>
    /// The current state of this source.
    /// </summary>
    public ALSourceState State => svc.GetState(this);

    /// <summary>
    /// The amount of buffers the source has already played.
    /// </summary>
    public int ProcessedBuffers => svc.GetProperty(this, ALGetSourcei.BuffersProcessed);

    /// <summary>
    /// The total amount of buffers to source has queued to play.
    /// </summary>
    public int QueuedBuffers => svc.GetProperty(this, ALGetSourcei.BuffersQueued);

    /// <summary>
    /// Whether the source is finished playing all queued buffers.
    /// </summary>
    public bool FinishedPlaying => ProcessedBuffers >= QueuedBuffers && !looping;

    private float gain, pitch;
    private bool looping;
    private Vector3 position, velocity;

    /// <summary>
    /// The volume at which the source plays its buffers.
    /// </summary>
    public float Gain
    {
        get => gain;
        set => svc.SetProperty(this, ALSourcef.Gain, gain = value);
    }

    /// <summary>
    /// The pitch at which the source plays its buffers.
    /// </summary>
    public float Pitch
    {
        get => pitch;
        set => svc.SetProperty(this, ALSourcef.Pitch, pitch = value);
    }

    /// <summary>
    /// Whether the source should repeat itself or not.
    /// </summary>
    public bool Looping
    {
        get => looping;
        set => svc.SetProperty(this, ALSourceb.Looping, looping = value);
    }

    /// <summary>
    /// The position of the audio source in 3D space.
    /// </summary>
    /// <remarks>With the default listener, OpenAL uses a right hand coordinate system, with x pointing right, y pointing up, and z pointing towards the viewer.</remarks>
    public Vector3 Position
    {
        get => position;
        set => svc.SetProperty(this, ALSource3f.Position, position = value);
    }

    /// <summary>
    /// The velocity of the audio source in 3D space.
    /// </summary>
    /// <remarks>With the default listener, OpenAL uses a right hand coordinate system, with x pointing right, y pointing up, and z pointing towards the viewer.</remarks>
    public Vector3 Velocity
    {
        get => velocity;
        set => svc.SetProperty(this, ALSource3f.Position, velocity = value);
    }

    /// <summary>
    /// Creates a new OpenAL source.
    /// </summary>
    public Source()
    {
        svc = SourceService.Instance;
        handle = svc.Generate();

        gain = 1;
        pitch = 1;
    }

    private void queueBuffersRaw(int bufferLength, int[] bufferIDs)
    {
        svc.QueueBuffers(this, bufferLength, bufferIDs);
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

        svc.UnqueueBuffers(this, QueuedBuffers);
    }

    /// <summary>
    /// Removes all the processed buffers from the source.
    /// </summary>
    public void DequeueProcessedBuffers()
    {
        svc.UnqueueBuffers(this, ProcessedBuffers);
    }

    /// <summary>
    /// Starts playing the source.
    /// </summary>
    public void Play()
    {
        svc.Play(this);
    }

    /// <summary>
    /// Pauses playing the source.
    /// </summary>
    public void Pause()
    {
        svc.Pause(this);
    }

    /// <summary>
    /// Stops playing the source.
    /// </summary>
    public void Stop()
    {
        svc.Stop(this);
    }

    /// <summary>
    /// Rewinds the source.
    /// </summary>
    public void Rewind()
    {
        svc.Rewind(this);
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

        svc.Delete(this);
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
