using System;
using UIStyler.Core;
using UnityEditor;
using UnityEngine;

namespace UIStyler.Editor
{
    [CustomPropertyDrawer(typeof(StyleNameSelectorAttribute))]
    public class StyleNameSelectorPropertyDrawer : PropertyDrawer
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
                var _names = UIStyleConfigs.Instance.GetStyleNames();
                
                property.serializedObject.Update();

                index = Array.IndexOf(_names, property.stringValue);
                index = EditorGUI.Popup(position, "Style Name", index, _names);
                if (index >= 0)
                {
                    property.stringValue = _names[index];
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}