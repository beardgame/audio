using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;

namespace Bearded.Audio {
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
            get => ListenerService.Instance.GetProperty(ALListener3f.Position);
            set => ListenerService.Instance.SetProperty(ALListener3f.Position, value);
        }

        public static Vector3 Velocity {
            get => ListenerService.Instance.GetProperty(ALListener3f.Velocity);
            set => ListenerService.Instance.SetProperty(ALListener3f.Velocity, value);
        }

        public static float Gain {
            get => ListenerService.Instance.GetProperty(ALListenerf.Gain);
            set => ListenerService.Instance.SetProperty(ALListenerf.Gain, value);
        }

        public static Vector3 At {
            get {
                ListenerService.Instance.GetProperty(ALListenerfv.Orientation, out var at, out _);
                return at;
            }
            set => ListenerService.Instance.SetProperty(ALListenerfv.Orientation, value, Up);
        }

        public static Vector3 Up {
            get {
                ListenerService.Instance.GetProperty(ALListenerfv.Orientation, out _, out var up);
                return up;
            }
            set => ListenerService.Instance.SetProperty(ALListenerfv.Orientation, At, value);
        }
    }
}
