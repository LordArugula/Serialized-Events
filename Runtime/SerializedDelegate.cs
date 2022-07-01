using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Arugula.SerializedEvents
{
    [Serializable]
    public class SerializedDelegate<T>
        where T : Delegate
    {
        [SerializeField]
        private Object target;

        [SerializeField]
        private string methodName;

#if UNITY_EDITOR
        [SerializeField, HideInInspector]
        private bool skipSerialization;
#endif

        public Object Target { get => target; set => target = value; }
        public string MethodName { get => methodName; set => methodName = value; }

        private T _callback;

        public SerializedDelegate(T callback)
        {
            _callback = callback;

#if UNITY_EDITOR
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
                    methodName = $"{targetType.Name}.{callback.Method.Name}";
                    return;
                }

                targetType = callback.Target.GetType();
                if (targetType.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
                {
                    methodName = "Anonymous Method";
                }
                else
                {
                    methodName = $"{targetType.Name}.{callback.Method.Name}";
                }
            }
#endif
        }

#if UNITY_EDITOR
        internal SerializedDelegate(Object target, string methodName)
        {
            skipSerialization = target == null;
            this.target = target;
            this.methodName = methodName;

            _callback = CreateDelegate();
        }
#endif

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
