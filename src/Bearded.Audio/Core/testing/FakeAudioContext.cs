using System;

namespace Bearded.Audio {
    internal sealed class FakeAudioContext : IAudioContext {
        public AudioConfig Config => AudioConfig.Default;

        public void CheckErrors() { }

        public void Call(Action function) { }

        public void Call<TParameter>(Action<TParameter> function, TParameter parameter) { }

        public void Call<TParam1, TParam2>(Action<TParam1, TParam2> function, TParam1 p1, TParam2 p2) { }

        public void Call<TParam1, TParam2, TParam3>(Action<TParam1, TParam2, TParam3> function, TParam1 p1, TParam2 p2, TParam3 p3) { }

        public void Call<TParam1, TParam2, TParam3, TParam4>(Action<TParam1, TParam2, TParam3, TParam4> function, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4) { }

        public void Call<TParam1, TParam2, TParam3, TParam4, TParam5>(Action<TParam1, TParam2, TParam3, TParam4, TParam5> function, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4,
                TParam5 p5) { }

        public TReturn Eval<TReturn>(Func<TReturn> function) {
            return default(TReturn);
        }

        public TReturn Eval<TParameter, TReturn>(Func<TParameter, TReturn> function, TParameter parameter) {
            return default(TReturn);
        }

        public TReturn Eval<TParam1, TParam2, TReturn>(Func<TParam1, TParam2, TReturn> function, TParam1 p1,
                TParam2 p2) {
            return default(TReturn);
        }
    }
}