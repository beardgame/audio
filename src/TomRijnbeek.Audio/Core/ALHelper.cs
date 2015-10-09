using System;
using System.Diagnostics;
using OpenTK.Audio.OpenAL;

namespace TomRijnbeek.Audio
{
    /// <summary>
    /// A general helper class for OpenAL related functions.
    /// </summary>
    public class ALHelper
    {
        /// <summary>
        /// Checks whether OpenAL has thrown an error and throws an exception if so.
        /// </summary>
        public static void Check()
        {
            ALError error;
            if ((error = AL.GetError()) == ALError.NoError)
                return;

            // TODO: fail silently for now
            Debug.Print(AL.GetErrorString(error));
        }

        /// <summary>
        /// Calls a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be called.</param>
        public static void Call(Action function)
        {
            function();
            ALHelper.Check();
        }

        /// <summary>
        /// Calls a function and then checks for an OpenAL error.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter of the function.</typeparam>
        /// <param name="function">The function to be called.</param>
        /// <param name="parameter">The parameter to be passed to the function.</param>
        public static void Call<TParameter>(Action<TParameter> function, TParameter parameter)
        {
            function(parameter);
            ALHelper.Check();
        }

        /// <summary>
        /// Calls a function and then checks for an OpenAL error.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter of the function.</typeparam>
        /// <param name="function">The function to be called.</param>
        /// <param name="p1">The first parameter to be passed to the function.</param>
        /// <param name="p2">The second parameter to be passed to the function.</param>
        public static void Call<TParam1, TParam2>(Action<TParam1, TParam2> function, TParam1 p1, TParam2 p2)
        {
            function(p1, p2);
            ALHelper.Check();
        }

        /// <summary>
        /// Calls a function and then checks for an OpenAL error.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter of the function.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter of the function.</typeparam>
        /// <typeparam name="TParam3">The type of the third parameter of the function.</typeparam>
        /// <param name="function">The function to be called.</param>
        /// <param name="p1">The first parameter to be passed to the function.</param>
        /// <param name="p2">The second parameter to be passed to the function.</param>
        /// <param name="p3">The third parameter to be passed to the function.</param>
        public static void Call<TParam1, TParam2, TParam3>(Action<TParam1, TParam2, TParam3> function, TParam1 p1,
            TParam2 p2, TParam3 p3)
        {
            function(p1, p2, p3);
            ALHelper.Check();
        }

        /// <summary>
        /// Evaluates a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be evaluated.</param>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        public static TReturn Eval<TReturn>(Func<TReturn> function)
        {
            var val = function();
            ALHelper.Check();
            return val;
        }

        /// <summary>
        /// Evaluates a function and then checks for an OpenAL error.
        /// </summary>
        /// <param name="function">The function to be evaluated.</param>
        /// <param name="parameter">The type of the parameter of the function.</param>
        /// <typeparam name="TParameter">The type of the parameter of the function.</typeparam>
        /// <typeparam name="TReturn">The type of the return value.</typeparam>
        public static TReturn Eval<TParameter, TReturn>(Func<TParameter, TReturn> function, TParameter parameter)
        {
            var val = function(parameter);
            ALHelper.Check();
            return val;
        }
    }
}