namespace TomRijnbeek.Audio {
    internal abstract class AudioService<TService> where TService : AudioService<TService>, new() {
        private static TService instance;
        public static TService Instance => instance ?? (instance = new TService());

        protected readonly IAudioContext ctx;
        protected readonly AudioConfig config;

        protected AudioService() {
            ctx = AudioContext.Instance;
            config = ctx.Config;
        }

        internal static void SetTestInstance(TService testInstance) {
            instance = testInstance;
        }
    }
}
