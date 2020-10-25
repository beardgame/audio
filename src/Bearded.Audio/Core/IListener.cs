using OpenTK.Mathematics;

namespace Bearded.Audio
{
    /// <summary>
    /// A listener interface.
    /// Every object implementing this interface can be used as audio listener in OpenAL.
    /// </summary>
    /// <remarks>
    /// It is recommended to use <see cref="Listener"/> instead whenever possible.
    /// </remarks>
    public interface IListener
    {
        /// <summary>
        /// The position of the listener.
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// The velocity with which the listener is moving.
        /// </summary>
        Vector3 Velocity { get; }

        /// <summary>
        /// The gain of the listener.
        /// </summary>
        float Gain { get; }

        /// <summary>
        /// The "up" component of the listener orientation.
        /// </summary>
        Vector3 Up { get; }

        /// <summary>
        /// The "at" component of the listener orientation.
        /// </summary>
        Vector3 At { get; }

        /// <summary>
        /// Occurs when listener updated.
        /// </summary>
        event Listener.ListenerEventHandler ListenerUpdated;
    }
}
