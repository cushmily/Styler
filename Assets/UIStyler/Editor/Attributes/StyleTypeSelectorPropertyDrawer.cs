using System;
using UIStyler.Core;
using UnityEditor;
using UnityEngine;

namespace UIStyler.Editor
{
    [CustomPropertyDrawer(typeof(StyleTypeSelectorAttribute))]
    public class StyleTypeSelectorPropertyDrawer : PropertyDrawer
    {
        private int index = -1;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, "Can only use on string field.");
            }
            else
            {
                var _types = UIStyleConfigs.Instance.StyleTypes;

                property.serializedObject.Update();

                index = Array.IndexOf(_types, property.stringValue);
                index = EditorGUI.Popup(position, "Style Type", index, _types);
                if (index >= 0)
                {
                    property.stringValue = _types[index];
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}