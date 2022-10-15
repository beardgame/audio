using JetBrains.Annotations;
using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;

namespace Bearded.Audio;

/// <summary>
/// Represents the single static listener in the OpenAL space.
/// This instance can be given an instance of the <see cref="IListener"/> type, and it will automatically synchronize
/// properties. This allows swapping out multiple listeners, or have an object be passed around to update properties.
/// </summary>
[PublicAPI]
public static class ALListener
{
    private static IListener? currentListener;

    /// <summary>
    /// Retrieves the <see cref="IListener"/> currently being synchronized with the AL Listener.
    /// </summary>
    public static IListener? Get()
    {
        return currentListener;
    }

    /// <summary>
    /// Sets a <see cref="IListener"/> instance with which the properties in the OpenAL space will automatically be
    /// synchronized.
    /// </summary>
    /// <param name="listener">The listener from which the properties should be synchronized.</param>
    public static void Set(IListener? listener)
    {
        if (currentListener != null)
        {
            currentListener.ListenerUpdated -= updateProperties;
        }

        currentListener = listener;
        if (listener == null)
        {
            return;
        }

        updateProperties(listener);
        listener.ListenerUpdated += updateProperties;
    }

    private static void updateProperties(IListener? reference)
    {
        if (currentListener == null || reference != currentListener)
        {
            return;
        }

        if (reference.Position != Position)
        {
            Position = reference.Position;
        }

        if (reference.Velocity != Velocity)
        {
            Velocity = reference.Velocity;
        }

        if (System.Math.Abs(reference.Gain - Gain) > .01f)
        {
            Gain = reference.Gain;
        }

        if (reference.At != At)
        {
            At = reference.At;
        }

        if (reference.Up != Up)
        {
            Up = reference.Up;
        }
    }

    /// <summary>
    /// The position of the virtual listener in the OpenAL space used to calculate the relative position of sound
    /// sources to determine channel mixing.
    /// </summary>
    /// <remarks>
    /// The positions of the listener and sound source is ignored for audio playback with multiple channels.
    /// </remarks>
    public static Vector3 Position
    {
        get
        {
            AL.GetListener(ALListener3f.Position, out var value);
            AudioContext.Instance.CheckErrors();
            return value;
        }
        set
        {
            var v = value;
            AudioContext.Instance.Call(() => AL.Listener(ALListener3f.Position, ref v));
        }
    }

    /// <summary>
    /// The velocity of the virtual listener in the OpenAL space used to calculate the relative velocity of sound
    /// sources to simulate the Doppler effect.
    /// </summary>
    /// <remarks>
    /// The velocities of the listener and sound source is ignored for audio playback with multiple channels.
    /// </remarks>
    public static Vector3 Velocity
    {
        get
        {
            AL.GetListener(ALListener3f.Velocity, out var value);
            AudioContext.Instance.CheckErrors();
            return value;
        }
        set
        {
            var v = value;
            AudioContext.Instance.Call(() => AL.Listener(ALListener3f.Velocity, ref v));
        }
    }

    /// <summary>
    /// The factor by which the audio amplitudes is multiplied when played back. This allows control over the global
    /// audio volume.
    /// </summary>
    public static float Gain
    {
        get
        {
            AL.GetListener(ALListenerf.Gain, out var value);
            AudioContext.Instance.CheckErrors();
            return value;
        }
        set => AudioContext.Instance.Call(AL.Listener, ALListenerf.Gain, value);
    }

    /// <summary>
    /// The forward direction of the virtual listener in the OpenAL space used to calculate the relative position of
    /// sound sources to determine channel mixing.
    /// </summary>
    /// <remarks>
    /// The orientations of the listener and sound source is ignored for audio playback with multiple channels.
    /// </remarks>
    public static Vector3 At
    {
        get
        {
            AL.GetListener(ALListenerfv.Orientation, out var at, out _);
            AudioContext.Instance.CheckErrors();
            return at;
        }
        set
        {
            var at = value;
            var up = Up;
            AudioContext.Instance.Call(() => AL.Listener(ALListenerfv.Orientation, ref at, ref up));
        }
    }

    /// <summary>
    /// The upward direction of the virtual listener in the OpenAL space used to calculate the relative position of
    /// sound sources to determine channel mixing.
    /// </summary>
    /// <remarks>
    /// The orientations of the listener and sound source is ignored for audio playback with multiple channels.
    /// </remarks>
    public static Vector3 Up
    {
        get
        {
            AL.GetListener(ALListenerfv.Orientation, out _, out var up);
            AudioContext.Instance.CheckErrors();
            return up;
        }
        set
        {
            var at = At;
            var up = value;
            AudioContext.Instance.Call(() => AL.Listener(ALListenerfv.Orientation, ref at, ref up));
        }
    }
}
