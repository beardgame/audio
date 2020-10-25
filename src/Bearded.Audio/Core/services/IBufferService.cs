using OpenTK.Audio.OpenAL;

namespace Bearded.Audio
{
    interface IBufferService
    {
        void Delete(int[] handles);
        void Fill(int handle, ALFormat format, short[] data, int sampleRate);
        int[] Generate(int n);
    }
}
