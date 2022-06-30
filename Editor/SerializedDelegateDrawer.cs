using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEditor;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Arugula.SerializedEvents.Editor
{
    [CustomPropertyDrawer(typeof(SerializedDelegateBase), true)]
    public class SerializedDelegateDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight
                + EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty targetProperty = property.FindPropertyRelative("target");
            SerializedProperty methodNameProperty = property.FindPropertyRelative("methodName");

            Object target = targetProperty.objectReferenceValue;

            Rect targetRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.ObjectField(targetRect, targetProperty);

            Object newTarget = targetProperty.objectReferenceValue;
            if (DidTypeOfTargetChange(target, newTarget))
            {
                methodNameProperty.stringValue = null;
            }

            using (new EditorGUI.DisabledScope(target == null))
            {
                MethodInfo invokeMethod = GetInvokeMethod();
                ParameterInfo[] parameterInfos = invokeMethod.GetParameters();
                Type targetReturnType = invokeMethod.ReturnType;
                string methodDisplayName = GetMethodDisplayName(targetProperty, methodNameProperty, parameterInfos, targetReturnType);
                Rect methodDropdownRect = new Rect(position.x, targetRect.yMax + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.singleLineHeight);
                if (EditorGUI.DropdownButton(methodDropdownRect, new GUIContent(methodDisplayName), FocusType.Keyboard))
                {
                    DoMethodDropdown(methodDropdownRect, targetProperty, methodNameProperty, parameterInfos, invokeMethod.ReturnType);
                }
            }
        }

        private void DoMethodDropdown(Rect methodDropdownRect, SerializedProperty targetProperty, SerializedProperty methodNameProperty, ParameterInfo[] targetParameterInfos, Type targetReturnType)
        {
            GenericMenu menu = new GenericMenu();

            Object rootObject = GetRootObject(targetProperty.objectReferenceValue);
            menu.AddItem(new GUIContent("No function"), false, () =>
            {
                methodNameProperty.stringValue = null;
                targetProperty.objectReferenceValue = rootObject;
                methodNameProperty.serializedObject.ApplyModifiedProperties();
            });
            menu.AddSeparator("");

            HashSet<Type> types = GetTypesOnTarget(rootObject);
            foreach (Type type in types)
            {
                MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                foreach (MethodInfo method in methods)
                {
                    if (!MatchMethod(method, targetReturnType, targetParameterInfos))
                    {
                        continue;
                    }

                    menu.AddItem(new GUIContent($"{type.Name}/{method.Name}"), false, () =>
                    {
                        methodNameProperty.stringValue = method.Name;
                        targetProperty.objectReferenceValue = GetTargetObject(rootObject, type);
                        methodNameProperty.serializedObject.ApplyModifiedProperties();
                    });
                }
            }

            menu.DropDown(methodDropdownRect);
        }

        private Object GetTargetObject(Object rootObject, Type type)
        {
            if (rootObject == null)
            {
                return null;
            }

            if (rootObject.GetType() == type)
            {
                return rootObject;
            }

            if (rootObject is GameObject gameObject && typeof(Component).IsAssignableFrom(type))
            {
                return gameObject.GetComponent(type);
            }

            return rootObject;
        }

        private bool MatchMethod(MethodInfo method, Type targetReturnType, ParameterInfo[] targetParameterInfos)
        {
            if (method.ReturnType != targetReturnType)
            {
                return false;
            }

            ParameterInfo[] parameterInfos = method.GetParameters();
            if (parameterInfos.Length != targetParameterInfos.Length)
            {
                return false;
            }

            for (int i = 0; i < targetParameterInfos.Length; i++)
            {
                if (targetParameterInfos[i].ParameterType != parameterInfos[i].ParameterType)
                {
                    return false;
                }
            }
            return true;
        }

        private static readonly HashSet<Type> types = new HashSet<Type>();
        private HashSet<Type> GetTypesOnTarget(Object rootObject)
        {
            types.Clear();
            types.Add(rootObject.GetType());
            if (rootObject is GameObject gameObject)
            {
                Component[] components = gameObject.GetComponents<Component>();
                foreach (var component in components)
                {
                    types.Add(component.GetType());
                }
            }

            return types;
        }

        private Object GetRootObject(Object _object)
        {
            if (_object is Component component)
            {
                return component.gameObject;
            }
            return _object;
        }

        private bool DidTypeOfTargetChange(Object target, Object newTarget)
        {
            return newTarget == null
                || (target != null && target.GetType() != newTarget.GetType());
        }

        private string GetMethodDisplayName(SerializedProperty targetProperty, SerializedProperty methodNameProperty, ParameterInfo[] parameterInfos, Type targetReturnType)
        {
            Object target = targetProperty.objectReferenceValue;
            string methodName = methodNameProperty.stringValue;
            if (target == null || string.IsNullOrEmpty(methodName))
            {
                return "No function";
            }

            Type targetType = target.GetType();
            MethodInfo targetMethod = GetTargetMethod(targetType, methodName, parameterInfos, targetReturnType);
            if (targetMethod == null)
            {
                return $"<Missing {targetType.Name}.{methodName}>";
            }

            return $"{targetType.Name}.{methodName}";
        }

        private MethodInfo GetTargetMethod(Type targetType, string methodName, ParameterInfo[] parameterInfos, Type targetReturnType)
        {
            Type[] parameterTypes = new Type[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                parameterTypes[i] = parameterInfos[i].ParameterType;
            }

            MethodInfo method = targetType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy, null, parameterTypes, null);

            if (method != null && method.ReturnType != targetReturnType)
            {
                return null;
            }
            return method;
        }

        private MethodInfo GetInvokeMethod()
        {
            Type fieldType = fieldInfo.FieldType;

            if (fieldType.IsArray)
            {
                fieldType = fieldType.GetElementType();
            }
            else if (typeof(IList).IsAssignableFrom(fieldType))
            {
                fieldType = fieldType.GetGenericArguments()[0];
            }

            Type delegateType = fieldType.GenericTypeArguments[0];
            MethodInfo invokeMethod = delegateType.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);
            return invokeMethod;
        }
    }
}
