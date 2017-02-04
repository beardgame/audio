using OpenTK.Audio.OpenAL;

namespace TomRijnbeek.Audio {
    /// <summary>
    /// Thrown when a specified name or handle is invalid (e.g. does not exist).
    /// </summary>
    public sealed class InvalidNameALException : ALException {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public InvalidNameALException(string message) : base(ALError.InvalidName, message) { }
    }
}