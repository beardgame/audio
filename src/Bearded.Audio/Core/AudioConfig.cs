namespace Bearded.Audio {
    /// <summary>
    /// Immutable container class for the audio configuration.
    /// </summary>
    public sealed class AudioConfig {
        /// <summary>
        /// The maximum number of sources that can be active simulataneously.
        /// </summary>
        public int MaxSources { get; private set; }

        private AudioConfig() { }

        #region Defaults
        /// <summary>
        /// The default configuration object.
        /// </summary>
        public static readonly AudioConfig Default;

        static AudioConfig() {
            Default = new AudioConfig();
            Default.MaxSources = 32;
        }
        #endregion

        #region Builder
        /// <summary>
        /// Returns a new builder based on the default configuration.
        /// </summary>
        /// <returns>The builder.</returns>
        public static Builder NewBuilder() {
            return Builder.From(Default);
        }

        /// <summary>
        /// Builder object that can be used to customise the configuration.
        /// </summary>
        public class Builder {
            /// <summary>
            /// Creates a new builder based on an existing audio configuration.
            /// </summary>
            /// <param name="reference">Reference.</param>
            public static Builder From(AudioConfig reference) {
                var builder = new Builder();
                copyConfig(reference, builder.workInstance);
                return builder;
            }

            private AudioConfig workInstance;

            private Builder() {
                workInstance = new AudioConfig();
            }

            /// <summary>
            /// Sets the maximum number of sources.
            /// </summary>
            /// <returns>The builder.</returns>
            /// <param name="maxSources">maximum number of sources.</param>
            public Builder SetMaxSources(int maxSources) {
                workInstance.MaxSources = maxSources;
                return this;
            }

            /// <summary>
            /// Build this instance.
            /// </summary>
            public AudioConfig Build() {
                var returnInstance = new AudioConfig();
                copyConfig(workInstance, returnInstance);
                return returnInstance;
            }
        }
        #endregion

        private static void copyConfig(AudioConfig from, AudioConfig to) {
            to.MaxSources = from.MaxSources;
        }
    }
}
