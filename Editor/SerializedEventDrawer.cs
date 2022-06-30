using System;

using UnityEditor;

using UnityEditorInternal;

using UnityEngine;

namespace Arugula.SerializedEvents.Editor
{
    [CustomPropertyDrawer(typeof(SerializedEventBase<>), true)]
    public class SerializedEventDrawer : PropertyDrawer
    {
        private ReorderableList reorderableList;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (reorderableList == null)
            {
                InitializeList(property, label);
            }
            return reorderableList.GetHeight();
        }

        private void InitializeList(SerializedProperty property, GUIContent label)
        {
            SerializedProperty callbackProperty = property.FindPropertyRelative("callback");
            SerializedProperty delegateListProperty = callbackProperty.FindPropertyRelative("delegates");

            reorderableList = new ReorderableList(property.serializedObject,
                delegateListProperty,
                true,
                true,
                true,
                true)
            {
                drawHeaderCallback = (rect) =>
                {
                    EditorGUI.LabelField(rect, label);
                },
                drawElementCallback = (rect, index, _, _) =>
                {
                    SerializedProperty property = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, property, true);
                },
                elementHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                onAddCallback = (list) =>
                {
                    int count = list.serializedProperty.arraySize;
                    list.serializedProperty.InsertArrayElementAtIndex(count);
                    SerializedProperty eventProperty = list.serializedProperty.GetArrayElementAtIndex(count);
                    eventProperty.FindPropertyRelative("target").objectReferenceValue = null;
                    eventProperty.FindPropertyRelative("methodName").stringValue = null;
                },
            };
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (reorderableList == null)
            {
                InitializeList(property, label);
            }

            reorderableList.DoList(position);
        }
    }
}
