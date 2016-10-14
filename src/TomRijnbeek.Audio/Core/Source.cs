using System;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace TomRijnbeek.Audio {
    /// <summary>
    /// Class representing an OpenAL audio source.
    /// </summary>
    public class Source : IDisposable {
        private readonly int handle;

        #region State
        /// <summary>
        /// Disposal state of this source.
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// The current state of this source.
        /// </summary>
        public ALSourceState State => ALHelper.Eval(AL.GetSourceState, this.handle);

        /// <summary>
        /// The amount of buffers the source has already played.
        /// </summary>
        public int ProcessedBuffers {
            get {
                int processedBuffers;
                AL.GetSource(this.handle, ALGetSourcei.BuffersProcessed, out processedBuffers);
                ALHelper.Check();
                return processedBuffers;
            }
        }

        /// <summary>
        /// The total amount of buffers to source has queued to play.
        /// </summary>
        public int QueuedBuffers {
            get {
                int queuedBuffers;
                AL.GetSource(this.handle, ALGetSourcei.BuffersQueued, out queuedBuffers);
                ALHelper.Check();
                return queuedBuffers;
            }
        }

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
            set {
                ALHelper.Call(AL.Source, this.handle, ALSourcef.Gain, this.gain = value);
            }
        }

        /// <summary>
        /// The pitch at which the source plays its buffers.
        /// </summary>
        public float Pitch {
            get { return this.pitch; }
            set {
                ALHelper.Call(AL.Source, this.handle, ALSourcef.Pitch, this.pitch = value);
            }
        }

        /// <summary>
        /// Whether the source should repeat itself or not.
        /// </summary>
        public bool Looping {
            get { return this.looping; }
            set {
                ALHelper.Call(AL.Source, this.handle, ALSourceb.Looping, this.looping = value);
            }
        }

        /// <summary>
        /// The position of the audio source in 3D space.
        /// </summary>
        /// <remarks>With the default listener, OpenAL uses a right hand coordinate system, with x pointing right, y pointing up, and z pointing towards the viewer.</remarks>
        public Vector3 Position {
            get { return this.position; }
            set {
                this.position = value;
                ALHelper.Call(() => AL.Source(this, ALSource3f.Position, ref this.position));
            }
        }

        /// <summary>
        /// The velocity of the audio source in 3D space.
        /// </summary>
        /// <remarks>With the default listener, OpenAL uses a right hand coordinate system, with x pointing right, y pointing up, and z pointing towards the viewer.</remarks>
        public Vector3 Velocity {
            get { return this.velocity; }
            set {
                this.velocity = value;
                ALHelper.Call(() => AL.Source(this, ALSource3f.Velocity, ref this.velocity));
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new OpenAL source.
        /// </summary>
        public Source() {
            this.handle = ALHelper.Eval(AL.GenSource);

            this.gain = 1;
            this.pitch = 1;
        }
        #endregion

        #region Buffers
        private void queueBuffersRaw(int bufferLength, int[] bufferIDs) {
            ALHelper.Call(AL.SourceQueueBuffers, this.handle, bufferLength, bufferIDs);
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

            ALHelper.Call(() => AL.SourceUnqueueBuffers(this.handle, this.QueuedBuffers));
        }

        /// <summary>
        /// Removes all the processed buffers from the source.
        /// </summary>
        public void UnqueueProcessedBuffers() {
            ALHelper.Call(() => AL.SourceUnqueueBuffers(this.handle, this.ProcessedBuffers));
        }
        #endregion

        #region Controls
        /// <summary>
        /// Starts playing the source.
        /// </summary>
        public void Play() {
            ALHelper.Call(AL.SourcePlay, this.handle);
        }

        /// <summary>
        /// Pauses playing the source.
        /// </summary>
        public void Pause() {
            ALHelper.Call(AL.SourcePause, this.handle);
        }

        /// <summary>
        /// Stops playing the source.
        /// </summary>
        public void Stop() {
            ALHelper.Call(AL.SourceStop, this.handle);
        }

        /// <summary>
        /// Rewinds the source.
        /// </summary>
        public void Rewind() {
            ALHelper.Call(AL.SourceRewind, this.handle);
        }
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

            ALHelper.Call(AL.DeleteSource, this.handle);
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