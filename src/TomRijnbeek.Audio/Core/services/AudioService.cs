namespace TomRijnbeek.Audio {
    abstract class AudioService<TService> where TService : AudioService<TService>, new() {
        private static TService instance;
        public static TService Instance {
            get {
                if (instance == null) {
                    instance = new TService();
                }
                return instance;
            }
        }

        protected readonly AudioContext ctx;
        protected readonly AudioConfig config;

        protected AudioService() {
            this.ctx = AudioContext.Instance;
            this.config = this.ctx.Config;
        }

        internal static void SetTestInstance(TService testInstance) {
            instance = testInstance;
        }
    }
}
