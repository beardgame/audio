using System;

using ALContext = OpenTK.Audio.AudioContext;

namespace TomRijnbeek.Audio {
    /// <summary>
    /// Main context for using any audio related code.
    /// Should be instantiated before using any of the library's code.
    /// </summary>
    public class AudioContext : IDisposable {
        #region Singleton
        private static AudioContext instance;

        /// <summary>
        /// Gets the sole AudioContext instance.
        /// Returns an exception if 
        /// </summary>
        /// <value>The instance.</value>
        public static AudioContext Instance { 
            get {
                if (instance == null) {
                    throw new NullReferenceException(
                        "The AudioContext must be initialised before any audio code can be executed.");
                }
                return instance;
            }
        }

        /// <summary>
        /// Initializes the audio context with the default configuration.
        /// </summary>
        public static void Initialize() {
            Initialize(AudioConfig.Default);
        }

        /// <summary>
        /// Initializes the audio context with the specified configuration.
        /// </summary>
        /// <param name="config">Config.</param>
        public static void Initialize(AudioConfig config) {
            if (instance != null) {
                throw new InvalidOperationException("Only one audio context can be instantiated.");
            }

            instance = new AudioContext(config);
        }
        #endregion

        private readonly AudioConfig config;
        private readonly ALContext ctx;

        /// <summary>
        /// The configuration used by this context.
        /// </summary>
        /// <value>The config.</value>
        public AudioConfig Config {
            get {
                return this.config;
            }
        }

        private AudioContext(AudioConfig config) {
            this.config = config;
            this.ctx = new ALContext();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        private void dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // Dispose managed state (managed objects).
                }

                this.ctx.Dispose();

                disposedValue = true;
                instance = null;
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="T:TomRijnbeek.Audio.AudioContext"/> is reclaimed by garbage collection.
        /// </summary>
        ~AudioContext() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            dispose(false);
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:TomRijnbeek.Audio.AudioContext"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="T:TomRijnbeek.Audio.AudioContext"/>.
        /// The <see cref="Dispose"/> method leaves the <see cref="T:TomRijnbeek.Audio.AudioContext"/> in an unusable
        /// state. After calling <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:TomRijnbeek.Audio.AudioContext"/> so the garbage collector can reclaim the memory that the
        /// <see cref="T:TomRijnbeek.Audio.AudioContext"/> was occupying.</remarks>
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
