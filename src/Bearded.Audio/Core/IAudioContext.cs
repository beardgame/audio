using System;

namespace Bearded.Audio {
    internal interface IAudioContext {
        /// <summary>
        /// The configuration used by this context.
        /// </summary>
        /// <value>The config.</value>
        AudioConfig Config { get; }

        /// <summary>
        /// Checks if OpenAL is currently in an error state.
        /// </summary>
        void CheckErrors();

        /// <summary>
        /// Calls a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be called.</param>
        void Call(Action function);

        /// <summary>
        /// Calls a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be called.</param>
        /// <param name="parameter">The parameter to be passed to the function.</param>
        /// <typeparam name="TParameter">The type of the parameter of the function.</typeparam>
        void Call<TParameter>(Action<TParameter> function, TParameter parameter);

        /// <summary>
        /// Calls a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be called.</param>
        /// <param name="p1">The first parameter to be passed to the function.</param>
        /// <param name="p2">The second parameter to be passed to the function.</param>
        /// <typeparam name="TParam1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter of the function.</typeparam>
        void Call<TParam1, TParam2>(
            Action<TParam1, TParam2> function,
            TParam1 p1, TParam2 p2);

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
        void Call<TParam1, TParam2, TParam3>(
            Action<TParam1, TParam2, TParam3> function,
            TParam1 p1, TParam2 p2, TParam3 p3);

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
        void Call<TParam1, TParam2, TParam3, TParam4>(
            Action<TParam1, TParam2, TParam3, TParam4> function,
            TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4);

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
        void Call<TParam1, TParam2, TParam3, TParam4, TParam5>(
            Action<TParam1, TParam2, TParam3, TParam4, TParam5> function,
            TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5);

        /// <summary>
        /// Evaluates a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be evaluated.</param>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        TReturn Eval<TReturn>(Func<TReturn> function);

        /// <summary>
        /// Evaluates a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be evaluated.</param>
        /// <param name="parameter">The type of the parameter of the function.</param>
        /// <typeparam name="TParameter">The type of the parameter of the function.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        TReturn Eval<TParameter, TReturn>(Func<TParameter, TReturn> function, TParameter parameter);

        /// <summary>
        /// Evaluates a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be evaluated.</param>
        /// <param name="p1">The first parameter to be passed to the function.</param>
        /// <param name="p2">The second parameter to be passed to the function.</param>
        /// <typeparam name="TParam1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        TReturn Eval<TParam1, TParam2, TReturn>(
            Func<TParam1, TParam2, TReturn> function,
            TParam1 p1, TParam2 p2);
    }
}