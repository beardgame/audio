using OpenTK.Audio.OpenAL;

namespace TomRijnbeek.Audio {
    internal class BufferService : AudioService<BufferService> {
        public virtual void Delete(int[] handles) => ctx.Call(AL.DeleteBuffers, handles);

        public virtual void Fill(int handle, ALFormat format, short[] data, int sampleRate) {
            ctx.Call(AL.BufferData, handle, format, data, sizeof(short) * data.Length, sampleRate);
        }

        public virtual int[] Generate(int n) => ctx.Eval(AL.GenBuffers, n);
    }
}
