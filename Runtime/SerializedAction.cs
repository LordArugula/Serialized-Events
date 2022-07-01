using System;

namespace Arugula.SerializedEvents
{
    /// <summary>
    /// A zero argument callback that can saved in the scene.
    /// </summary>
    [Serializable]
    public class SerializedAction
        : SerializedEventBase<Action>
    {
        public void Invoke()
        {
            foreach (Action action in this)
            {
                action.Invoke();
            }
        }
    }

    /// <summary>
    /// A one argument callback that can saved in the scene.
    /// </summary>
    [Serializable]
    public class SerializedAction<T>
        : SerializedEventBase<Action<T>>
    {
        public void Invoke(T arg)
        {
            foreach (Action<T> action in this)
            {
                action.Invoke(arg);
            }
        }
    }

    /// <summary>
    /// A two argument callback that can saved in the scene.
    /// </summary>
    [Serializable]
    public class SerializedAction<T1, T2>
        : SerializedEventBase<Action<T1, T2>>
    {
        public void Invoke(T1 arg1, T2 arg2)
        {
            foreach (Action<T1, T2> action in this)
            {
                action.Invoke(arg1, arg2);
            }
        }
    }

    /// <summary>
    /// A three argument callback that can saved in the scene.
    /// </summary>
    [Serializable]
    public class SerializedAction<T1, T2, T3>
        : SerializedEventBase<Action<T1, T2, T3>>
    {
        public void Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            foreach (Action<T1, T2, T3> action in this)
            {
                action.Invoke(arg1, arg2, arg3);
            }
        }
    }

    /// <summary>
    /// A four argument callback that can saved in the scene.
    /// </summary>
    [Serializable]
    public class SerializedAction<T1, T2, T3, T4>
        : SerializedEventBase<Action<T1, T2, T3, T4>>
    {
        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            foreach (Action<T1, T2, T3, T4> action in this)
            {
                action.Invoke(arg1, arg2, arg3, arg4);
            }
        }
    }
}
