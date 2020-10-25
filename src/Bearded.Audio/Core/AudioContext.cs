using System;
using OpenTK.Audio.OpenAL;

namespace Bearded.Audio
{
    /// <inheritdoc />
    /// <summary>
    /// Main context for using any audio related code.
    /// Should be instantiated before using any of the library's code.
    /// </summary>
    public sealed class AudioContext : IDisposable
    {
        private static AudioContext instance;

        /// <summary>
        /// Gets the sole AudioContext instance.
        /// Throws an exception if the audio context was not initialised.
        /// </summary>
        /// <value>The instance.</value>
        public static AudioContext Instance
        {
            get
            {
                if (instance == null)
                {
                    throw new NullReferenceException(
                        "The AudioContext must be initialised before any audio code can be executed.");
                }

                return instance;
            }
        }

        /// <summary>
        /// Initializes the audio context.
        /// </summary>
        public static void Initialize()
        {
            if (instance != null)
            {
                throw new InvalidOperationException("Only one audio context can be instantiated.");
            }

            instance = new AudioContext();
        }

        private readonly ALDevice device;
        private readonly ALContext ctx;

        private AudioContext()
        {
            var deviceName = findDeviceName();
            device = ALC.OpenDevice(deviceName);
            ctx = ALC.CreateContext(device, (int[]) null!);
            ALC.MakeContextCurrent(ctx);
        }

        private static string findDeviceName()
        {
            // Start with the default device.
            var deviceName = ALC.GetString(ALDevice.Null, AlcGetString.DefaultDeviceSpecifier);

            // Find all remaining devices. Prefer OpenAL Soft if it exists.
            var devices = ALC.GetStringList(GetEnumerationStringList.DeviceSpecifier);
            foreach (var d in devices)
            {
                if (d.Contains("OpenAL Soft"))
                {
                    deviceName = d;
                }
            }

            return deviceName;
        }

        // ReSharper disable MemberCanBeMadeStatic.Global
        // The following methods are non-static to force there being an AudioContext instance.

        /// <summary>
        /// Checks if OpenAL is currently in an error state.
        /// </summary>
        /// <exception cref="ALException"></exception>
        public void CheckErrors()
        {
            ALError error;
            if ((error = AL.GetError()) == ALError.NoError)
            {
                return;
            }

            throwError(error, AL.GetErrorString(error));
        }

        private static void throwError(ALError error, string message)
        {
            switch (error)
            {
                case ALError.NoError:
                    return;
                case ALError.InvalidEnum:
                    throw new InvalidEnumALException(message);
                case ALError.InvalidName:
                    throw new InvalidNameALException(message);
                case ALError.InvalidOperation:
                    throw new InvalidOperationALException(message);
                case ALError.InvalidValue:
                    throw new InvalidValueALException(message);
                case ALError.OutOfMemory:
                    throw new OutOfMemoryALException(message);
                default:
                    throw new ArgumentOutOfRangeException(nameof(error), error, null);
            }
        }

        /// <summary>
        /// Calls a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be called.</param>
        public void Call(Action function)
        {
            function();
            CheckErrors();
        }

        /// <summary>
        /// Calls a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be called.</param>
        /// <param name="parameter">The parameter to be passed to the function.</param>
        /// <typeparam name="TParameter">The type of the parameter of the function.</typeparam>
        public void Call<TParameter>(Action<TParameter> function, TParameter parameter)
        {
            function(parameter);
            CheckErrors();
        }

        /// <summary>
        /// Calls a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be called.</param>
        /// <param name="p1">The first parameter to be passed to the function.</param>
        /// <param name="p2">The second parameter to be passed to the function.</param>
        /// <typeparam name="TParam1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter of the function.</typeparam>
        public void Call<TParam1, TParam2>(
            Action<TParam1, TParam2> function,
            TParam1 p1, TParam2 p2)
        {
            function(p1, p2);
            CheckErrors();
        }

        /// <summary>
        /// Calls a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be called.</param>
        /// <param name="p1">The first parameter to be passed to the function.</param>
        /// <param name="p2">The second parameter to be passed to the function.</param>
        /// <param name="p3">The third parameter to be passed to the function.</param>
        /// <typeparam name="TParam1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="TParam3">The type of the third parameter of the function.</typeparam>
        public void Call<TParam1, TParam2, TParam3>(
            Action<TParam1, TParam2, TParam3> function,
            TParam1 p1, TParam2 p2, TParam3 p3)
        {
            function(p1, p2, p3);
            CheckErrors();
        }

        /// <summary>
        /// Calls a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be called.</param>
        /// <param name="p1">The first parameter to be passed to the function.</param>
        /// <param name="p2">The second parameter to be passed to the function.</param>
        /// <param name="p3">The third parameter to be passed to the function.</param>
        /// <param name="p4">The fourth parameter to be passed to the function.</param>
        /// <typeparam name="TParam1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="TParam3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="TParam4">The type of the fourth parameter of the function.</typeparam>
        public void Call<TParam1, TParam2, TParam3, TParam4>(
            Action<TParam1, TParam2, TParam3, TParam4> function,
            TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            function(p1, p2, p3, p4);
            CheckErrors();
        }

        /// <summary>
        /// Calls a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be called.</param>
        /// <param name="p1">The first parameter to be passed to the function.</param>
        /// <param name="p2">The second parameter to be passed to the function.</param>
        /// <param name="p3">The third parameter to be passed to the function.</param>
        /// <param name="p4">The fourth parameter to be passed to the function.</param>
        /// <param name="p5">The fifth parameter to be passed to the function.</param>
        /// <typeparam name="TParam1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="TParam3">The type of the third parameter of the function.</typeparam>
        /// <typeparam name="TParam4">The type of the fourth parameter of the function.</typeparam>
        /// <typeparam name="TParam5">The type of the fifth parameter of the function.</typeparam>
        public void Call<TParam1, TParam2, TParam3, TParam4, TParam5>(
            Action<TParam1, TParam2, TParam3, TParam4, TParam5> function,
            TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5)
        {
            function(p1, p2, p3, p4, p5);
            CheckErrors();
        }

        /// <summary>
        /// Evaluates a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be evaluated.</param>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        public TReturn Eval<TReturn>(Func<TReturn> function)
        {
            var val = function();
            CheckErrors();
            return val;
        }

        /// <summary>
        /// Evaluates a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be evaluated.</param>
        /// <param name="parameter">The type of the parameter of the function.</param>
        /// <typeparam name="TParameter">The type of the parameter of the function.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        public TReturn Eval<TParameter, TReturn>(Func<TParameter, TReturn> function, TParameter parameter)
        {
            var val = function(parameter);
            CheckErrors();
            return val;
        }

        /// <summary>
        /// Evaluates a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be evaluated.</param>
        /// <param name="p1">The first parameter to be passed to the function.</param>
        /// <param name="p2">The second parameter to be passed to the function.</param>
        /// <typeparam name="TParam1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        public TReturn Eval<TParam1, TParam2, TReturn>(
            Func<TParam1, TParam2, TReturn> function,
            TParam1 p1, TParam2 p2)
        {
            var val = function(p1, p2);
            CheckErrors();
            return val;
        }

        // ReSharper restore MemberCanBeMadeStatic.Global

        private bool isDisposed; // To detect redundant calls

        private void dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {
                // Dispose managed state (managed objects).
            }

            ALC.DestroyContext(ctx);
            ALC.CloseDevice(device);

            isDisposed = true;
            instance = null;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="T:Bearded.Audio.AudioContext"/> is reclaimed by garbage collection.
        /// </summary>
        ~AudioContext()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            dispose(false);
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:Bearded.Audio.AudioContext"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="T:Bearded.Audio.AudioContext"/>.
        /// The <see cref="Dispose"/> method leaves the <see cref="T:Bearded.Audio.AudioContext"/> in an unusable
        /// state. After calling <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:Bearded.Audio.AudioContext"/> so the garbage collector can reclaim the memory that the
        /// <see cref="T:Bearded.Audio.AudioContext"/> was occupying.</remarks>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
