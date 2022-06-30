using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Arugula.SerializedEvents
{
    [Serializable]
    public class SerializedCallback<T>
        : IEnumerable<T>
        where T : Delegate
    {
        [SerializeField]
        private List<SerializedDelegate<T>> delegates;

        public SerializedCallback()
        {
            delegates = new List<SerializedDelegate<T>>();
        }

        public void Clear()
        {
            delegates.Clear();
        }

        public void Add(T listener)
        {
            SerializedDelegate<T> callback = new SerializedDelegate<T>(listener);
            delegates.Add(callback);
        }

        public void Remove(T listener)
        {
            for (int i = delegates.Count - 1; i >= 0; i--)
            {
                if (delegates[i].Callback == listener)
                {
                    delegates.RemoveAt(i);
                    return;
                }
            }
        }

        internal void Add(Object target, string methodName)
        {
            delegates.Add(new SerializedDelegate<T>(target, methodName));
        }

        internal void Remove(Object target, string methodName)
        {
            for (int i = delegates.Count - 1; i >= 0; i--)
            {
                if (delegates[i].Target == target && delegates[i].MethodName == methodName)
                {
                    delegates.RemoveAt(i);
                    return;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(delegates);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(delegates);
        }

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
    }

    [Serializable]
    public abstract class SerializedEventBase<T>
        where T : Delegate
    {
        [SerializeField]
        protected SerializedCallback<T> callback;

        public SerializedEventBase()
        {
            callback = new SerializedCallback<T>();
        }

        public void AddListener(T listener)
        {
            callback.Add(listener);
        }

        public void RemoveListener(T listener)
        {
            callback.Remove(listener);
        }

        public void Clear()
        {
            callback.Clear();
        }

        internal void AddListener(Object target, string methodName)
        {
            callback.Add(target, methodName);
        }

        internal void RemoveListener(Object target, string methodName)
        {
            callback.Remove(target, methodName);
        }
    }
}
