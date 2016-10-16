using OpenTK;
using OpenTK.Audio.OpenAL;

namespace TomRijnbeek.Audio {
    class SourceService : AudioService<SourceService> {
        public void Delete(int handle) => ctx.Call(AL.DeleteSource, handle);

        public int Generate() => ctx.Eval(AL.GenSource);

        public bool GetProperty(int handle, ALSourceb property) {
            bool value;
            AL.GetSource(handle, property, out value);
            ctx.CheckErrors();
            return value;
        }

        public float GetProperty(int handle, ALSourcef property) {
            float value;
            AL.GetSource(handle, property, out value);
            ctx.CheckErrors();
            return value;
        }

        public int GetProperty(int handle, ALGetSourcei property) {
            int value;
            AL.GetSource(handle, property, out value);
            ctx.CheckErrors();
            return value;
        }

        public ALSourceState GetState(int handle) {
            return ctx.Eval(AL.GetSourceState, handle);
        }

        public void Pause(int handle) => ctx.Call(AL.SourcePause, handle);

        public void Play(int handle) => ctx.Call(AL.SourcePlay, handle);

        public void QueueBuffers(int handle, int bufferLength, int[] bufferIDs) {
            ctx.Call(AL.SourceQueueBuffers, handle, bufferLength, bufferIDs);
        }

        public void Rewind(int handle) => ctx.Call(AL.SourceRewind, handle);

        public void SetProperty(int handle, ALSourceb property, bool value) {
            ctx.Call(AL.Source, handle, property, value);
        }

        public void SetProperty(int handle, ALSourcef property, float value) {
            ctx.Call(AL.Source, handle, property, value);
        }

        public void SetProperty(int handle, ALSource3f property, Vector3 value) {
            ctx.Call(() => AL.Source(handle, property, ref value));
        }

        public void Stop(int handle) => ctx.Call(AL.SourceStop, handle);

        public int[] UnqueueBuffers(int handle, int n) {
            return ctx.Eval(AL.SourceUnqueueBuffers, handle, n);
        }
    }
}
