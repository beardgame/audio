using OpenTK;

namespace TomRijnbeek.Audio {
    /// <summary>
    /// This class represents a listener.
    /// </summary>
    /// <remarks>
    /// This basic implementation should generally be preferred over the use
    /// of the interface, since it makes sure that the ListenerUpdated event
    /// is called whenever one of its properties is updated.
    /// </remarks>
    public abstract class Listener : IListener {
        /// <summary>
        /// Listener event handler.
        /// </summary>
        public delegate void ListenerEventHandler(IListener listener);

        private Vector3 position;

        /// <summary>
        /// The position of the listener.
        /// </summary>
        public Vector3 Position {
            get {
                return this.position;
            }
            set {
                this.position = value;
                this.broadcastChange();
            }
        }

        Vector3 velocity;

        /// <summary>
        /// The velocity with which the listener is moving.
        /// </summary>
        public Vector3 Velocity {
            get {
                return this.velocity;
            }
            set {
                this.velocity = value;
                this.broadcastChange();
            }
        }

        float gain;

        /// <summary>
        /// The gain of the listener.
        /// </summary>
        public float Gain {
            get {
                return this.gain;
            }
            set {
                this.gain = value;
                this.broadcastChange();
            }
        }

        Vector3 up;

        /// <summary>
        /// The "up" component of the listener orientation.
        /// </summary>
        public Vector3 Up {
            get {
                return this.up;
            }
            set {
                this.up = value;
                this.broadcastChange();
            }
        }

        Vector3 at;

        /// <summary>
        /// The "at" component of the listener orientation.
        /// </summary>
        public Vector3 At {
            get {
                return this.at;
            }
            set {
                this.at = value;
                this.broadcastChange();
            }
        }

        /// <summary>
        /// Occurs when listener updated.
        /// </summary>
        public event ListenerEventHandler ListenerUpdated;

        /// <summary>
        /// Creates a new listener with default parameters.
        /// </summary>
        protected Listener() {
            this.position = Vector3.Zero;
            this.velocity = Vector3.Zero;
            this.gain = 1f;
            this.at = -Vector3.UnitZ;
            this.up = Vector3.UnitY;
        }

        /// <summary>
        /// Sets the up and at vectors based on the quaternion and forward/up vector.
        /// </summary>
        /// <param name="quaternion">The rotation of the listener.</param>
        /// <param name="forward">The (unit) direction that is considered "forward". Default OpenAL assumes -z direction.</param>
        /// <param name="up">The (unit) direction that is considered "up". Default OpenAL assumes +y direction.</param>
        public void SetOrientationFromQuaternion(Quaternion quaternion, Vector3 forward, Vector3 up) {
            this.At = Vector3.Transform(forward, quaternion);
            this.Up = Vector3.Transform(up, quaternion);
        }

        private void broadcastChange() {
            if (this.ListenerUpdated != null) {
                this.ListenerUpdated(this);
            }
        }
    }
}