using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;

namespace Bearded.Audio {
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    class ListenerService : AudioService<IListenerService, ListenerService>, IListenerService {

        public virtual float GetProperty(ALListenerf property) {
            AL.GetListener(property, out var value);
            Context.CheckErrors();
            return value;
        }

        public virtual Vector3 GetProperty(ALListener3f property) {
            AL.GetListener(property, out var value);
            Context.CheckErrors();
            return value;
        }

        public virtual void GetProperty(ALListenerfv property, out Vector3 at, out Vector3 up) {
            AL.GetListener(property, out at, out up);
            Context.CheckErrors();
        }

        public virtual void SetProperty(ALListenerf property, float value) {
            Context.Call(AL.Listener, property, value);
        }

        public virtual void SetProperty(ALListener3f property, Vector3 value) {
            Context.Call(() => AL.Listener(property, ref value));
        }

        public virtual void SetProperty(ALListenerfv property, Vector3 at, Vector3 up) {
            Context.Call(() => AL.Listener(property, ref at, ref up));
        }
    }
}
