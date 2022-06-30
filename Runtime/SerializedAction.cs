using System;

namespace Arugula.SerializedEvents
{
    [Serializable]
    public class SerializedAction
        : SerializedEventBase<Action>
    {
        public void Invoke()
        {
            foreach (Action action in callback)
            {
                action.Invoke();
            }
        }
    }

    [Serializable]
    public class SerializedAction<T>
        : SerializedEventBase<Action<T>>
    {
        public void Invoke(T arg)
        {
            foreach (Action<T> action in callback)
            {
                action.Invoke(arg);
            }
        }
    }

    [Serializable]
    public class SerializedAction<T1, T2>
        : SerializedEventBase<Action<T1, T2>>
    {
        public void Invoke(T1 arg1, T2 arg2)
        {
            foreach (Action<T1, T2> action in callback)
            {
                action.Invoke(arg1, arg2);
            }
        }
    }

    [Serializable]
    public class SerializedAction<T1, T2, T3>
        : SerializedEventBase<Action<T1, T2, T3>>
    {
        public void Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            foreach (Action<T1, T2, T3> action in callback)
            {
                action.Invoke(arg1, arg2, arg3);
            }
        }
    }

    [Serializable]
    public class SerializedAction<T1, T2, T3, T4>
        : SerializedEventBase<Action<T1, T2, T3, T4>>
    {
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            foreach (Action<T1, T2, T3, T4> action in callback)
            {
                action.Invoke(arg1, arg2, arg3, arg4);
            }
        }
    }
}
