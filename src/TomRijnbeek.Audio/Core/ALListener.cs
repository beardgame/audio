using OpenTK;
using OpenTK.Audio.OpenAL;

namespace TomRijnbeek.Audio {
    public static class ALListener {
        private static IListener currentListener;

        public static IListener Get() {
            return currentListener;
        }

        public static void Set(IListener listener) {
            if (currentListener != null) {
                currentListener.ListenerUpdated -= updateProperties;
            }
            currentListener = listener;
            if (currentListener != null) {
                updateProperties(listener);
                listener.ListenerUpdated += updateProperties;
            }
        }

        private static void updateProperties(IListener reference) {
            if (currentListener == null || reference != currentListener) return;
            if (reference.Position != Position) {
                Position = reference.Position;
            }
            if (reference.Velocity != Velocity) {
                Velocity = reference.Velocity;
            }
            if (System.Math.Abs(reference.Gain - Gain) > .01f) {
                Gain = reference.Gain;
            }
            if (reference.At != At) {
                At = reference.At;
            }
            if (reference.Up != Up) {
                Up = reference.Up;
            }
        }

        public static Vector3 Position {
            get { return AudioContext.Instance.GetListener(ALListener3f.Position); }
            set { AudioContext.Instance.Listener(ALListener3f.Position, value); }
        }

        public static Vector3 Velocity {
            get { return AudioContext.Instance.GetListener(ALListener3f.Velocity); }
            set { AudioContext.Instance.Listener(ALListener3f.Velocity, value); }
        }

        public static float Gain {
            get { return AudioContext.Instance.GetListener(ALListenerf.Gain); }
            set { AudioContext.Instance.Listener(ALListenerf.Gain, value); }
        }

        public static Vector3 At {
            get {
                Vector3 at, up;
                AudioContext.Instance.GetListener(ALListenerfv.Orientation, out at, out up);
                return at;
            }
            set { AudioContext.Instance.Listener(ALListenerfv.Orientation, value, Up); }
        }

        public static Vector3 Up {
            get {
                Vector3 at, up;
                AudioContext.Instance.GetListener(ALListenerfv.Orientation, out at, out up);
                return up;
            }
            set { AudioContext.Instance.Listener(ALListenerfv.Orientation, At, value); }
        }
    }
}
