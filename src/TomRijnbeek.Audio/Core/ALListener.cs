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
            get { return getProperty(ALListener3f.Position); }
            set { setProperty(ALListener3f.Position, value); }
        }

        public static Vector3 Velocity {
            get { return getProperty(ALListener3f.Velocity); }
            set { setProperty(ALListener3f.Velocity, value); }
        }

        public static float Gain {
            get { return getProperty(ALListenerf.Gain); }
            set { setProperty(ALListenerf.Gain, value); }
        }

        public static Vector3 At {
            get { return getAtProperty(ALListenerfv.Orientation); }
            set { setProperty(ALListenerfv.Orientation, value, Up); }
        }

        public static Vector3 Up {
            get { return getUpProperty(ALListenerfv.Orientation); }
            set { setProperty(ALListenerfv.Orientation, At, value); }
        }

        private static float getProperty(ALListenerf property) {
            float value;
            AL.GetListener(property, out value);
            return value;
        }

        private static Vector3 getProperty(ALListener3f property) {
            Vector3 value;
            AL.GetListener(property, out value);
            return value;
        }

        private static Vector3 getAtProperty(ALListenerfv property) {
            Vector3 at, up;
            AL.GetListener(property, out at, out up);
            return at;
        }

        private static Vector3 getUpProperty(ALListenerfv property) {
            Vector3 at, up;
            AL.GetListener(property, out at, out up);
            return up;
        }

        private static void setProperty(ALListenerf property, float value) {
            AL.Listener(property, value);
        }

        private static void setProperty(ALListener3f property, Vector3 value) {
            AL.Listener(property, ref value);
        }

        private static void setProperty(ALListenerfv property, Vector3 at, Vector3 up) {
            AL.Listener(property, ref at, ref up);
        }
    }
}
