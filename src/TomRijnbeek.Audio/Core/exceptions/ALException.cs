using System;
using OpenTK.Audio.OpenAL;

namespace TomRijnbeek.Audio {
    /// <summary>
    /// Generic OpenAL exception.
    /// </summary>
    public class ALException : Exception {
        /// <summary>
        /// The OpenAL error type.
        /// </summary>
        public ALError Type { get; }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="type">The OpenAL error type.</param>
        /// <param name="message">The exception message.</param>
        public ALException(ALError type, string message) : base(message) {
            Type = type;
        }
    }
}