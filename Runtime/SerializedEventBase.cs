using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Arugula.SerializedEvents
{
    /// <summary>
    /// Base class for all serialized events.
    /// </summary>
    /// <typeparam name="T">The delegate type.</typeparam>
    [Serializable]
    public abstract class SerializedEventBase<T>
        : IEnumerable<T>, IEnumerable
        where T : Delegate
    {
        /// <summary>
        /// The invocation list.
        /// </summary>
        /// <remarks>
        /// We use a list of callbacks instead of a registering them all
        /// to a single delegate so they can be "unsubscribed" in the editor 
        /// easily.
        [SerializeField]
        private List<SerializedDelegate<T>> callback;

        private class Enumerator : IEnumerator<T>, IEnumerator
        {
            private List<SerializedDelegate<T>>.Enumerator enumerator;

            public Enumerator(List<SerializedDelegate<T>> callback)
            {
                enumerator = callback.GetEnumerator();
            }

            public T Current => enumerator.Current.Callback;

            object IEnumerator.Current => enumerator.Current.Callback;

            public void Dispose()
            {
                enumerator.Dispose();
            }

            public bool MoveNext()
            {
                return enumerator.MoveNext();
            }

            public void Reset()
            {
                ((IEnumerator)enumerator).Reset();
            }
        }

        public SerializedEventBase()
        {
            callback = new List<SerializedDelegate<T>>();
        }

        public void AddListener(T listener)
        {
            if (listener == null)
            {
                return;
            }

            foreach (T _listener in listener.GetInvocationList())
            {
                SerializedDelegate<T> callback = new SerializedDelegate<T>(_listener);
                this.callback.Add(callback);
            }
        }

        public void RemoveListener(T listener)
        {
            if (listener == null)
            {
                return;
            }

            foreach (T _listener in listener.GetInvocationList())
            {
                for (int i = callback.Count - 1; i >= 0; i--)
                {
                    if (callback[i].Callback == _listener)
                    {
                        callback.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public void Clear()
        {
            callback.Clear();
        }

#if UNITY_EDITOR
        internal void AddListener(Object target, string methodName)
        {
            callback.Add(new SerializedDelegate<T>(target, methodName));
        }

        internal void RemoveListener(Object target, string methodName)
        {
            for (int i = callback.Count - 1; i >= 0; i--)
            {
                if (callback[i].Target == target && callback[i].MethodName == methodName)
                {
                    callback.RemoveAt(i);
                    return;
                }
            }
        }
#endif

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(callback);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(callback);
        }
    }
}
