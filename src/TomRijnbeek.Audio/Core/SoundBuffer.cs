using System;
using OpenTK.Audio.OpenAL;

namespace TomRijnbeek.Audio {
    /// <summary>
    /// Class representing a group of OpenAL audio buffers.
    /// </summary>
    public class SoundBuffer : IDisposable {
        /// <summary>
        /// List of OpenAL buffer handles.
        /// </summary>
        private readonly int[] handles;

        #region State
        /// <summary>
        /// Disposal state of this buffer.
        /// </summary>
        public bool Disposed { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Generates a new sound buffer of the given size.
        /// </summary>
        /// <param name="amount">The amount of buffers to reserve.</param>
        public SoundBuffer(int amount) {
            this.handles = ALHelper.Eval(AL.GenBuffers, amount);
        }

        /// <summary>
        /// Generates a news sound buffer and fills it.
        /// </summary>
        /// <param name="data">The data to load the buffers with.</param>
        public SoundBuffer(SoundBufferData data)
            : this(data.Buffers.Count) {
            this.FillBuffer(data);
        }
        #endregion

        #region Buffer filling
        private void fillBufferRaw(int index, short[] data, ALFormat format, int sampleRate) {
            if (index < 0 || index >= this.handles.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            ALHelper.Call(
                () => AL.BufferData(this.handles[index], format, data, data.Length * sizeof(short), sampleRate));
        }

        /// <summary>
        /// Fills the buffer with new data.
        /// </summary>
        /// <param name="data">The new content of the buffers.</param>
        public void FillBuffer(SoundBufferData data) {
            this.FillBuffer(0, data);
        }

        /// <summary>
        /// Fills the buffer with new data.
        /// </summary>
        /// <param name="index">The starting index from where to fill the buffer.</param>
        /// <param name="data">The new content of the buffers.</param>
        public void FillBuffer(int index, SoundBufferData data) {
            if (index < 0 || index >= this.handles.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (data.Buffers.Count > this.handles.Length)
                throw new ArgumentException("This data does not fit in the buffer.", nameof(data));

            for (int i = 0; i < data.Buffers.Count; i++)
                this.fillBufferRaw((index + i) % this.handles.Length, data.Buffers[i], data.Format, data.SampleRate);
        }
        #endregion

        #region IDisposable implementation
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            if (this.Disposed)
                return;

            ALHelper.Call(AL.DeleteBuffers, this.handles);

            this.Disposed = true;
        }
        #endregion

        #region Operators
        /// <summary>
        /// Casts the buffer to an integer array.
        /// </summary>
        /// <param name="buffer">The buffer that should be casted.</param>
        /// <returns>The OpenAL handles of the buffers.</returns>
        static public implicit operator int[] (SoundBuffer buffer) {
            return buffer.handles;
        }
        #endregion
    }
}