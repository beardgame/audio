using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;

namespace Bearded.Audio;

public static class ALListener
{
    private static IListener? currentListener;

    public static IListener? Get()
    {
        return currentListener;
    }

    public static void Set(IListener? listener)
    {
        if (currentListener != null)
        {
            currentListener.ListenerUpdated -= updateProperties;
        }

        currentListener = listener;
        if (listener != null)
        {
            updateProperties(listener);
            listener.ListenerUpdated += updateProperties;
        }
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
            Vector3 value1 = value;
            AudioContext.Instance.Call(() => AL.Listener(ALListener3f.Position, ref value1));
        }
    }

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
            Vector3 value1 = value;
            AudioContext.Instance.Call(() => AL.Listener(ALListener3f.Velocity, ref value1));
        }
    }

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
