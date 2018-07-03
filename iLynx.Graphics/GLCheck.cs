#region LICENSE
/*
 * Copyright 2018 Melanie Hjorth
 *
 * Redistribution and use in source and binary forms,
 * with or without modification,
 * are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice,
 * this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 *
 * 3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote
 * products derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
 * EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 */
#endregion
using System;
using OpenTK.Graphics.OpenGL;
using iLynx.Common;

namespace iLynx.Graphics
{
    // ReSharper disable once InconsistentNaming
    public static class GLCheck
    {
        public delegate void RefAction<T>(ref T arg);

        public static void Check<T>(RefAction<T> action, ref T arg)
        {
            try
            {
                action.Invoke(ref arg);
            }
            finally
            {
                Console.WriteLine($"{action.Target}.{action.Method}");
                Console.WriteLine($"{arg}");
                var error = GL.GetError();
                if (error != ErrorCode.NoError)
                    throw new OpenGLCallException(action, error);
            }
        }

        public static void Check(Action a)
        {
            Check((Delegate)a);
        }

        public static TResult Check<TResult>(Func<TResult> f)
        {
            return (TResult)Check((Delegate)f);
        }

        public static TResult Check<T1, TResult>(Func<T1, TResult> f, T1 arg)
        {
            return (TResult)Check((Delegate)f, arg);
        }

        private static object Check(Delegate method, params object[] arguments)
        {
            try
            {
                return method.DynamicInvoke(arguments);
            }
            finally
            {
                Console.WriteLine($"{method.Target}.{method.Method}");
                Console.WriteLine($"{arguments.ToString(", ")}");
                var error = GL.GetError();
                if (error != ErrorCode.NoError)
                    throw new OpenGLCallException(method, error);
            }
        }

        public static void Check<T>(Action<T> target, T argument)
        {
            Check((Delegate)target, argument);
        }

        public static void Check<T1, T2>(Action<T1, T2> target, T1 argument1, T2 argument2)
        {
            Check((Delegate)target, argument1, argument2);
        }

        public static void Check<T1, T2, T3>(Action<T1, T2, T3> target, T1 argument1, T2 argument2, T3 argument3)
        {
            Check((Delegate)target, argument1, argument2, argument3);
        }

        public static void Check<T1, T2, T3, T4>(Action<T1, T2, T3, T4> target, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            Check((Delegate)target, t1, t2, t3, t4);
        }

        public static void Check<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> target, T1 t1, T2 t2, T3 t3, T4 t4,
            T5 t5)
        {
            Check((Delegate)target, t1, t2, t3, t4, t5);
        }

        public static void Check<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> target, T1 t1, T2 t2, T3 t3, T4 t4,
            T5 t5, T6 t6)
        {
            Check((Delegate)target, t1, t2, t3, t4, t5, t6);
        }

        public static TResult Check<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 argument1, T2 argument2)
        {
            return (TResult)Check((Delegate)func, argument1, argument2);
        }
    }

    // ReSharper disable once InconsistentNaming
    public class OpenGLCallException : OpenGLException
    {
        public OpenGLCallException(Delegate action, ErrorCode error)
            : base($"{action.Target}.{action.Method} failed with error: {error}")
        {
        }
    }

    // ReSharper disable once InconsistentNaming
    public abstract class OpenGLException : Exception
    {
        protected OpenGLException() { }

        protected OpenGLException(string message) : base(message) { }

        protected OpenGLException(string message, Exception innerException) : base(message, innerException) { }
    }
}
