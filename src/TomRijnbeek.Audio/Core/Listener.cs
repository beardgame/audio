using OpenTK;

namespace TomRijnbeek.Audio
{
    /// <summary>
    /// This class represents a listener.
    /// </summary>
    /// <remarks>
    /// This class does not update the actual ALListener parameters.
    /// Due to the single listener restriction in OpenAL, we might want to
    /// switch out listeners. Updating this listener might not be the same
    /// as updating the currently active listener, so we leave that up to
    /// the audio manager.
    /// </remarks>
    public class Listener : IListener
    {
        /// <summary>
        /// The position of the listener.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// The velocity with which the listener is moving.
        /// </summary>
        public Vector3 Velocity { get; set; }

        /// <summary>
        /// The gain of the listener.
        /// </summary>
        public float Gain { get; set; }

        /// <summary>
        /// The "up" component of the listener orientation.
        /// </summary>
        public Vector3 Up { get; set; }

        /// <summary>
        /// The "at" component of the listener orientation.
        /// </summary>
        public Vector3 At { get; set; }

        /// <summary>
        /// Creates a new listener with default parameters.
        /// </summary>
        public Listener()
        {
            this.Position = Vector3.Zero;
            this.Velocity = Vector3.Zero;
            this.At = -Vector3.UnitZ;
            this.Up = Vector3.UnitY;
        }

        /// <summary>
        /// Sets the up and at vectors based on the quaternion and forward/up vector.
        /// </summary>
        /// <param name="quaternion">The rotation of the listener.</param>
        /// <param name="forward">The (unit) direction that is considered "forward". Default OpenAL assumes -z direction.</param>
        /// <param name="up">The (unit) direction that is considered "up". Default OpenAL assumes +y direction.</param>
        public void SetOrientationFromQuaternion(Quaternion quaternion, Vector3 forward, Vector3 up)
        {
            this.At = Vector3.Transform(forward, quaternion);
            this.Up = Vector3.Transform(up, quaternion);
        }
    }
}