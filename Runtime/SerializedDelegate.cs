using System;
using System.Reflection;
using System.Runtime.CompilerServices;

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
        where T : Delegate
    {
        [SerializeField]
        private Object target;

        [SerializeField]
        private string methodName;

        [SerializeField, HideInInspector]
        private bool skipSerialization;

        public Object Target { get => target; set => target = value; }
        public string MethodName { get => methodName; set => methodName = value; }

        private T _callback;

        public SerializedDelegate(T callback)
        {
            if (callback.Target is Object obj)
            {
                target = obj;
                methodName = callback.Method.Name;
            }
            else
            {
                if (!Application.isPlaying)
                {
                    throw new InvalidOperationException("Cannot serialize static or anonymous methods in the editor.");
                }

                skipSerialization = true;
                Type targetType;
                if (callback.Target == null)
                {
                    // Static method
                    targetType = callback.Method.DeclaringType;
                }
                else
                {
                    // Anonymous method
                    targetType = callback.Target.GetType();
                    if (targetType.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
                    {
                        skipSerialization = true;
                        methodName = "Anonymous Method";
                    }
                }

                methodName = $"{targetType.Name}.{callback.Method.Name}";
            }
            _callback = callback;
        }

        internal SerializedDelegate(Object target, string methodName)
        {
            skipSerialization = target == null;
            this.target = target;
            this.methodName = methodName;

            _callback = CreateDelegate();
        }

        public T Callback
        {
            get
            {
                if (_callback == null)
                {
                    _callback = CreateDelegate();
                }
                return _callback;
            }

            set => _callback = value;
        }

        private T CreateDelegate()
        {
            if (skipSerialization || target == null || methodName == null)
            {
                return null;
            }
            return (T)Delegate.CreateDelegate(typeof(T), target, methodName);
        }
    }
}
