using OpenTK.Audio.OpenAL;

namespace TomRijnbeek.Audio {
    /// <summary>
    /// Thrown when a variable is set to an invalid value in OpenAL.
    /// </summary>
    public sealed class InvalidValueALException : ALException {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public InvalidValueALException(string message) : base(ALError.InvalidValue, message) { }
    }
}
