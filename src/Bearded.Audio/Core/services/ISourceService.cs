using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;

namespace Bearded.Audio {
    interface ISourceService {
        void Delete(int handle);
        int Generate();
        bool GetProperty(int handle, ALSourceb property);
        float GetProperty(int handle, ALSourcef property);
        int GetProperty(int handle, ALGetSourcei property);
        ALSourceState GetState(int handle);
        void Pause(int handle);
        void Play(int handle);
        void QueueBuffers(int handle, int bufferLength, int[] bufferIDs);
        void Rewind(int handle);
        void SetProperty(int handle, ALSourceb property, bool value);
        void SetProperty(int handle, ALSourcef property, float value);
        void SetProperty(int handle, ALSource3f property, Vector3 value);
        void Stop(int handle);
        int[] UnqueueBuffers(int handle, int n);
    }
}
