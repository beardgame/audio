namespace Bearded.Audio {
    abstract class AudioService<TService> where TService : AudioService<TService>, new() {
        private static TService instance;
        public static TService Instance => instance ?? (instance = new TService());

        protected readonly AudioContext Context;
        protected readonly AudioConfig Config;

        protected AudioService() {
            Context = AudioContext.Instance;
            Config = Context.Config;
        }

        internal static void SetTestInstance(TService testInstance) {
            instance = testInstance;
        }
    }
}
