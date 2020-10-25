using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;

namespace Bearded.Audio {
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    class SourceService : AudioService<ISourceService, SourceService>, ISourceService {
        public virtual void Delete(int handle) => Context.Call(AL.DeleteSource, handle);

        public virtual int Generate() => Context.Eval(AL.GenSource);

        public virtual bool GetProperty(int handle, ALSourceb property) {
            AL.GetSource(handle, property, out var value);
            Context.CheckErrors();
            return value;
        }

        public virtual float GetProperty(int handle, ALSourcef property) {
            AL.GetSource(handle, property, out var value);
            Context.CheckErrors();
            return value;
        }

        public virtual int GetProperty(int handle, ALGetSourcei property) {
            AL.GetSource(handle, property, out var value);
            Context.CheckErrors();
            return value;
        }

        public virtual ALSourceState GetState(int handle) {
            return Context.Eval(AL.GetSourceState, handle);
        }

        public virtual void Pause(int handle) => Context.Call(AL.SourcePause, handle);

        public virtual void Play(int handle) => Context.Call(AL.SourcePlay, handle);

        public virtual void QueueBuffers(int handle, int bufferLength, int[] bufferIDs) {
            Context.Call(AL.SourceQueueBuffers, handle, bufferLength, bufferIDs);
        }

        public virtual void Rewind(int handle) => Context.Call(AL.SourceRewind, handle);

        public virtual void SetProperty(int handle, ALSourceb property, bool value) {
            Context.Call(AL.Source, handle, property, value);
        }

        public virtual void SetProperty(int handle, ALSourcef property, float value) {
            Context.Call(AL.Source, handle, property, value);
        }

        public virtual void SetProperty(int handle, ALSource3f property, Vector3 value) {
            Context.Call(() => AL.Source(handle, property, ref value));
        }

        public virtual void Stop(int handle) => Context.Call(AL.SourceStop, handle);

        public virtual int[] UnqueueBuffers(int handle, int n) {
            return Context.Eval(AL.SourceUnqueueBuffers, handle, n);
        }
    }
}
