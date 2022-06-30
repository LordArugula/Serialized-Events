﻿using System;

namespace Arugula.SerializedEvents
{
    [Serializable]
    public class SerializedFunc<TResult>
        : SerializedEventBase<Func<TResult>>
    {
        public TResult Invoke()
        {
            TResult result = default;
            foreach (Func<TResult> func in callback)
            {
                result = func.Invoke();
            }
            return result;
        }
    }

    [Serializable]
    public class SerializedFunc<T, TResult>
        : SerializedEventBase<Func<T, TResult>>
    {
        public TResult Invoke(T arg)
        {
            TResult result = default;
            foreach (Func<T, TResult> func in callback)
            {
                result = func.Invoke(arg);
            }
            return result;
        }
    }

    [Serializable]
    public class SerializedFunc<T1, T2, TResult>
        : SerializedEventBase<Func<T1, T2, TResult>>
    {
        public TResult Invoke(T1 arg1, T2 arg2)
        {
            TResult result = default;
            foreach (Func<T1, T2, TResult> func in callback)
            {
                result = func.Invoke(arg1, arg2);
            }
            return result;
        }
    }

    [Serializable]
    public class SerializedFunc<T1, T2, T3, TResult>
        : SerializedEventBase<Func<T1, T2, T3, TResult>>
    {
        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            TResult result = default;
            foreach (Func<T1, T2, T3, TResult> func in callback)
            {
                result = func.Invoke(arg1, arg2, arg3);
            }
            return result;
        }
    }

    [Serializable]
    public class SerializedFunc<T1, T2, T3, T4, TResult>
        : SerializedEventBase<Func<T1, T2, T3, T4, TResult>>
    {
        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            TResult result = default;
            foreach (Func<T1, T2, T3, T4, TResult> func in callback)
            {
                result = func.Invoke(arg1, arg2, arg3, arg4);
            }
            return result;
        }
    }
}
