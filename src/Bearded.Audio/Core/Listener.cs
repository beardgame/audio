using JetBrains.Annotations;
using OpenTK.Mathematics;

namespace Bearded.Audio;

/// <summary>
/// This class represents a listener.
/// </summary>
/// <remarks>
/// This basic implementation should generally be preferred over the use
/// of the interface, since it makes sure that the ListenerUpdated event
/// is called whenever one of its properties is updated.
/// </remarks>
[PublicAPI]
public abstract class Listener : IListener
{
    /// <summary>
    /// Listener event handler.
    /// </summary>
    public delegate void ListenerEventHandler(IListener listener);

    private Vector3 position;

    /// <summary>
    /// The position of the listener.
    /// </summary>
    public Vector3 Position
    {
        get => position;
        set
        {
            position = value;
            broadcastChange();
        }
    }

    private Vector3 velocity;

    /// <summary>
    /// The velocity with which the listener is moving.
    /// </summary>
    public Vector3 Velocity
    {
        get => velocity;
        set
        {
            velocity = value;
            broadcastChange();
        }
    }

    private float gain;

    /// <summary>
    /// The gain of the listener.
    /// </summary>
    public float Gain
    {
        get => gain;
        set
        {
            gain = value;
            broadcastChange();
        }
    }

    private Vector3 up;

    /// <summary>
    /// The "up" component of the listener orientation.
    /// </summary>
    public Vector3 Up
    {
        get => up;
        set
        {
            up = value;
            broadcastChange();
        }
    }

    private Vector3 at;

    /// <summary>
    /// The "at" component of the listener orientation.
    /// </summary>
    public Vector3 At
    {
        get => at;
        set
        {
            at = value;
            broadcastChange();
        }
    }

    /// <summary>
    /// Occurs when listener updated.
    /// </summary>
    public event ListenerEventHandler? ListenerUpdated;

    /// <summary>
    /// Creates a new listener with default parameters.
    /// </summary>
    protected Listener()
    {
        position = Vector3.Zero;
        velocity = Vector3.Zero;
        gain = 1f;
        at = -Vector3.UnitZ;
        up = Vector3.UnitY;
    }

    /// <summary>
    /// Sets the up and at vectors based on the quaternion and forward/up vector.
    /// </summary>
    /// <param name="quaternion">The rotation of the listener.</param>
    /// <param name="forward">The (unit) direction that is considered "forward". Default OpenAL assumes -z direction.</param>
    /// <param name="up">The (unit) direction that is considered "up". Default OpenAL assumes +y direction.</param>
    public void SetOrientationFromQuaternion(Quaternion quaternion, Vector3 forward, Vector3 up)
    {
        At = Vector3.Transform(forward, quaternion);
        Up = Vector3.Transform(up, quaternion);
    }

    private void broadcastChange()
    {
        ListenerUpdated?.Invoke(this);
    }
}
