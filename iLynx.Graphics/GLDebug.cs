using System;
using OpenTK.Graphics.OpenGL;
using iLynx.Common;

namespace iLynx.Graphics
{
    // ReSharper disable once InconsistentNaming
    public static class GLDebug
    {
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
                Console.WriteLine($"{method.Target}.{method.Method} {arguments.ToString(", ")}");
                var error = GL.GetError();
                if (error != ErrorCode.NoError)
                    throw new OpenGLException(method, error);
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

    public class OpenGLException : Exception
    {
        public OpenGLException(Delegate action, ErrorCode error)
            : base($"{action.Target}.{action.Method} failed with error: {error}")
        {
        }
    }
}
