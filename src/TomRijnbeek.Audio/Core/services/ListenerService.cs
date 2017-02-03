using OpenTK;
using OpenTK.Audio.OpenAL;

namespace TomRijnbeek.Audio {
    class ListenerService : AudioService<ListenerService> {
        public virtual float GetProperty(ALListenerf property) {
            float value;
            AL.GetListener(property, out value);
            ctx.CheckErrors();
            return value;
        }

        public virtual Vector3 GetProperty(ALListener3f property) {
            Vector3 value;
            AL.GetListener(property, out value);
            ctx.CheckErrors();
            return value;
        }

        public virtual void GetProperty(ALListenerfv property, out Vector3 at, out Vector3 up) {
            AL.GetListener(property, out at, out up);
            ctx.CheckErrors();
        }

        public virtual void SetProperty(ALListenerf property, float value) {
            ctx.Call(AL.Listener, property, value);
        }

        public virtual void SetProperty(ALListener3f property, Vector3 value) {
            ctx.Call(() => AL.Listener(property, ref value));
        }

        public virtual void SetProperty(ALListenerfv property, Vector3 at, Vector3 up) {
            ctx.Call(() => AL.Listener(property, ref at, ref up));
        }
    }
}
