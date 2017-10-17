using OpenTK;
using OpenTK.Audio.OpenAL;

namespace Bearded.Audio {
    class SourceService : AudioService<SourceService> {
        public virtual void Delete(int handle) => ctx.Call(AL.DeleteSource, handle);

        public virtual int Generate() => ctx.Eval(AL.GenSource);

        public virtual bool GetProperty(int handle, ALSourceb property) {
            bool value;
            AL.GetSource(handle, property, out value);
            ctx.CheckErrors();
            return value;
        }

        public virtual float GetProperty(int handle, ALSourcef property) {
            float value;
            AL.GetSource(handle, property, out value);
            ctx.CheckErrors();
            return value;
        }

        public virtual int GetProperty(int handle, ALGetSourcei property) {
            int value;
            AL.GetSource(handle, property, out value);
            ctx.CheckErrors();
            return value;
        }

        public virtual ALSourceState GetState(int handle) {
            return ctx.Eval(AL.GetSourceState, handle);
        }

        public virtual void Pause(int handle) => ctx.Call(AL.SourcePause, handle);

        public virtual void Play(int handle) => ctx.Call(AL.SourcePlay, handle);

        public virtual void QueueBuffers(int handle, int bufferLength, int[] bufferIDs) {
            ctx.Call(AL.SourceQueueBuffers, handle, bufferLength, bufferIDs);
        }

        public virtual void Rewind(int handle) => ctx.Call(AL.SourceRewind, handle);

        public virtual void SetProperty(int handle, ALSourceb property, bool value) {
            ctx.Call(AL.Source, handle, property, value);
        }

        public virtual void SetProperty(int handle, ALSourcef property, float value) {
            ctx.Call(AL.Source, handle, property, value);
        }

        public virtual void SetProperty(int handle, ALSource3f property, Vector3 value) {
            ctx.Call(() => AL.Source(handle, property, ref value));
        }

        public virtual void Stop(int handle) => ctx.Call(AL.SourceStop, handle);

        public virtual int[] UnqueueBuffers(int handle, int n) {
            return ctx.Eval(AL.SourceUnqueueBuffers, handle, n);
        }
    }
}
