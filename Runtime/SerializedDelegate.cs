using System;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Arugula.SerializedEvents
{
    [Serializable]
    public abstract class SerializedDelegateBase
    {
        [SerializeField]
        protected Object target;

        [SerializeField]
        protected string methodName;

        public Object Target { get => target; set => target = value; }
        public string MethodName { get => methodName; set => methodName = value; }

        public SerializedDelegateBase(Object target, string methodName)
        {
            this.target = target;
            this.methodName = methodName;
        }
    }

    [Serializable]
    public class SerializedDelegate<T>
        : SerializedDelegateBase
        where T : Delegate
    {
        private T callback;

        public SerializedDelegate(T callback)
            : this(null, null)
        {
            this.callback = callback;
        }

        public SerializedDelegate(Object target, string methodName)
            : base(target, methodName)
        {
            callback = CreateDelegate();
        }

        public T Callback
        {
            get
            {
                if (callback == null)
                {
                    callback = CreateDelegate();
                }
                return callback;
            }

            set => callback = value;
        }

        private T CreateDelegate()
        {
            if (target == null || methodName == null)
            {
                return null;
            }
            return (T)Delegate.CreateDelegate(typeof(T), target, methodName);
        }
    }
}
