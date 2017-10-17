using System;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace Bearded.Audio {
    /// <summary>
    /// Class representing an OpenAL audio source.
    /// </summary>
    public class Source : IDisposable {
        private readonly SourceService svc;
        private readonly int handle;

        #region State
        /// <summary>
        /// Disposal state of this source.
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// The current state of this source.
        /// </summary>
        public ALSourceState State => this.svc.GetState(this);

        /// <summary>
        /// The amount of buffers the source has already played.
        /// </summary>
        public int ProcessedBuffers => this.svc.GetProperty(this, ALGetSourcei.BuffersProcessed);

        /// <summary>
        /// The total amount of buffers to source has queued to play.
        /// </summary>
        public int QueuedBuffers => this.svc.GetProperty(this, ALGetSourcei.BuffersQueued);

        /// <summary>
        /// Whether the source is finished playing all queued buffers.
        /// </summary>
        public bool FinishedPlaying => this.ProcessedBuffers >= this.QueuedBuffers && !this.looping;
        #endregion

        #region Properties
        private float gain, pitch;
        private bool looping;
        private Vector3 position, velocity;

        /// <summary>
        /// The volume at which the source plays its buffers.
        /// </summary>
        public float Gain {
            get { return this.gain; }
            set { this.svc.SetProperty(this, ALSourcef.Gain, this.gain = value); }
        }

        /// <summary>
        /// The pitch at which the source plays its buffers.
        /// </summary>
        public float Pitch {
            get { return this.pitch; }
            set { this.svc.SetProperty(this, ALSourcef.Pitch, this.pitch = value); }
        }

        /// <summary>
        /// Whether the source should repeat itself or not.
        /// </summary>
        public bool Looping {
            get { return this.looping; }
            set { this.svc.SetProperty(this, ALSourceb.Looping, this.looping = value); }
        }

        /// <summary>
        /// The position of the audio source in 3D space.
        /// </summary>
        /// <remarks>With the default listener, OpenAL uses a right hand coordinate system, with x pointing right, y pointing up, and z pointing towards the viewer.</remarks>
        public Vector3 Position {
            get { return this.position; }
            set { this.svc.SetProperty(this, ALSource3f.Position, this.position = value); }
        }

        /// <summary>
        /// The velocity of the audio source in 3D space.
        /// </summary>
        /// <remarks>With the default listener, OpenAL uses a right hand coordinate system, with x pointing right, y pointing up, and z pointing towards the viewer.</remarks>
        public Vector3 Velocity {
            get { return this.velocity; }
            set { this.svc.SetProperty(this, ALSource3f.Position, this.velocity = value); }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new OpenAL source.
        /// </summary>
        public Source() {
            this.svc = SourceService.Instance;
            this.handle = this.svc.Generate();

            this.gain = 1;
            this.pitch = 1;
        }
        #endregion

        #region Buffers
        private void queueBuffersRaw(int bufferLength, int[] bufferIDs) {
            this.svc.QueueBuffers(this, bufferLength, bufferIDs);
        }

        /// <summary>
        /// Queues a sound buffer to be played by this source.
        /// </summary>
        /// <param name="buffer"></param>
        public void QueueBuffer(SoundBuffer buffer) {
            var handles = (int[])buffer;
            this.queueBuffersRaw(handles.Length, handles);
        }

        /// <summary>
        /// Removes all the buffers from the source.
        /// </summary>
        public void UnqueueBuffers() {
            if (this.QueuedBuffers == 0)
                return;

            this.svc.UnqueueBuffers(this, this.QueuedBuffers);
        }

        /// <summary>
        /// Removes all the processed buffers from the source.
        /// </summary>
        public void UnqueueProcessedBuffers() {
            this.svc.UnqueueBuffers(this, this.ProcessedBuffers);
        }
        #endregion

        #region Controls
        /// <summary>
        /// Starts playing the source.
        /// </summary>
        public void Play() => this.svc.Play(this);

        /// <summary>
        /// Pauses playing the source.
        /// </summary>
        public void Pause() => this.svc.Pause(this);

        /// <summary>
        /// Stops playing the source.
        /// </summary>
        public void Stop() => this.svc.Stop(this);

        /// <summary>
        /// Rewinds the source.
        /// </summary>
        public void Rewind() => this.svc.Rewind(this);
        #endregion

        #region IDisposable implementation
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            if (this.Disposed)
                return;

            if (this.State != ALSourceState.Stopped)
                this.Stop();

            this.svc.Delete(this);
            this.Disposed = true;
        }
        #endregion

        #region Operators
        /// <summary>
        /// Casts the source to an integer.
        /// </summary>
        /// <param name="source">The source that should be casted.</param>
        /// <returns>The OpenAL handle of the source.</returns>
        static public implicit operator int(Source source) {
            return source.handle;
        }
        #endregion
    }
}