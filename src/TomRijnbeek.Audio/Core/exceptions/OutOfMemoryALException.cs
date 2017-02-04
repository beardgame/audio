using OpenTK.Audio.OpenAL;

namespace TomRijnbeek.Audio {
    /// <summary>
    /// Thrown when OpenAL runs out of memory.
    /// </summary>
    public class OutOfMemoryALException : ALException {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public OutOfMemoryALException(string message) : base(ALError.OutOfMemory, message) { }
    }
}