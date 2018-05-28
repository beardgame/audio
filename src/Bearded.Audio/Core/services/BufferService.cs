using OpenTK.Audio.OpenAL;

namespace Bearded.Audio {
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    class BufferService : AudioService<BufferService> {
        public virtual void Delete(int[] handles) => Context.Call(AL.DeleteBuffers, handles);

        public virtual void Fill(int handle, ALFormat format, short[] data, int sampleRate) {
            Context.Call(AL.BufferData, handle, format, data, sizeof(short) * data.Length, sampleRate);
        }

        public virtual int[] Generate(int n) => Context.Eval(AL.GenBuffers, n);
    }
}
