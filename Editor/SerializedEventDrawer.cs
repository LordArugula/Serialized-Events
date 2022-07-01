using System;
using System.Reflection;
using System.Text;

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

namespace Arugula.SerializedEvents.Editor
{
    [CustomPropertyDrawer(typeof(SerializedEventBase<>), true)]
    public class SerializedEventDrawer : PropertyDrawer
    {
        private ReorderableList reorderableList;
        private StringBuilder builder;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (reorderableList == null)
            {
                InitializeList(property, label);
            }
            return reorderableList.GetHeight();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (reorderableList == null)
            {
                InitializeList(property, label);
            }

            reorderableList.DoList(position);
        }

        private void InitializeList(SerializedProperty property, GUIContent label)
        {
            SerializedProperty callbackProperty = property.FindPropertyRelative("callback");

            reorderableList = new ReorderableList(
                property.serializedObject,
                callbackProperty,
                true,
                true,
                true,
                true)
            {
                drawHeaderCallback = (rect) => DrawHeader(rect, label),
                drawElementCallback = DrawElement,
                elementHeightCallback = GetElementHeight,
                onAddCallback = AddElement,
            };
        }

        private float GetElementHeight(int index)
        {
            if (index >= reorderableList.serializedProperty.arraySize)
            {
                return 0;
            }
            SerializedProperty property = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            return EditorGUI.GetPropertyHeight(property, true);
        }

        private void AddElement(ReorderableList list)
        {
            int count = list.serializedProperty.arraySize;
            list.serializedProperty.InsertArrayElementAtIndex(count);
            SerializedProperty eventProperty = list.serializedProperty.GetArrayElementAtIndex(count);
            eventProperty.FindPropertyRelative("target").objectReferenceValue = null;
            eventProperty.FindPropertyRelative("methodName").stringValue = null;
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty property = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, property, true);
        }

        private void DrawHeader(Rect rect, GUIContent label)
        {
            Type fieldType = fieldInfo.FieldType;
            MethodInfo invokeMethod = fieldType.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);
            if (invokeMethod == null)
            {
                EditorGUI.LabelField(rect, label.text);
                return;
            }

            string formattedMethodLabel = GetFormattedLabel(label.text, invokeMethod);
            EditorGUI.LabelField(rect, new GUIContent(formattedMethodLabel));
        }

        private string GetFormattedLabel(string propertyDisplayName, MethodInfo invokeMethod)
        {
            if (builder == null)
            {
                builder = new StringBuilder();
            }
            else
            {
                builder.Clear();
            }

            Type returnType = invokeMethod.ReturnType;
            if (returnType != typeof(void))
            {
                builder.Append(returnType.Name)
                    .Append(' ');
            }

            builder.Append(propertyDisplayName);

            ParameterInfo[] parameterInfos = invokeMethod.GetParameters();
            builder.Append(' ')
                .Append('(');

            if (parameterInfos.Length > 0)
            {
                AppendParameters(builder, parameterInfos);
            }

            builder.Append(')');

            string formattedMethodLabel = builder.ToString();
            return formattedMethodLabel;

            static void AppendParameters(StringBuilder _builder, ParameterInfo[] _parameterInfos)
            {
                for (int i = 0; i < _parameterInfos.Length; i++)
                {
                    ParameterInfo parameter = _parameterInfos[i];
                    _builder.Append(parameter.ParameterType.Name);
                    if (i + 1 < _parameterInfos.Length)
                    {
                        _builder.Append(',')
                            .Append(' ');
                    }
                }
            }
        }
    }
}
