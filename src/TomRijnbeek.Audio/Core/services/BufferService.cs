using OpenTK.Audio.OpenAL;

namespace TomRijnbeek.Audio {
    class BufferService : AudioService<BufferService> {
        public void Delete(int[] handles) => ctx.Call(AL.DeleteBuffers, handles);

        public void Fill(int handle, ALFormat format, short[] data, int sampleRate) {
            ctx.Call(AL.BufferData, handle, format, data, sizeof(short) * data.Length, sampleRate);
        }

        public int[] Generate(int n) => ctx.Eval(AL.GenBuffers, n);
    }
}
