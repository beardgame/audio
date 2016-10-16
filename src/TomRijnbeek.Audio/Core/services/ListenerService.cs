using OpenTK;
using OpenTK.Audio.OpenAL;

namespace TomRijnbeek.Audio {
    class ListenerService : AudioService<ListenerService> {
        public float GetProperty(ALListenerf property) {
            float value;
            AL.GetListener(property, out value);
            ctx.CheckErrors();
            return value;
        }

        public Vector3 GetProperty(ALListener3f property) {
            Vector3 value;
            AL.GetListener(property, out value);
            ctx.CheckErrors();
            return value;
        }

        public void GetProperty(ALListenerfv property, out Vector3 at, out Vector3 up) {
            AL.GetListener(property, out at, out up);
            ctx.CheckErrors();
        }

        public void SetProperty(ALListenerf property, float value) {
            ctx.Call(AL.Listener, property, value);
        }

        public void SetProperty(ALListener3f property, Vector3 value) {
            ctx.Call(() => AL.Listener(property, ref value));
        }

        public void SetProperty(ALListenerfv property, Vector3 at, Vector3 up) {
            ctx.Call(() => AL.Listener(property, ref at, ref up));
        }
    }
}
