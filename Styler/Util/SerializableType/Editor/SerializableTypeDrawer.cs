using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UIStylerA.Editor
{
    [CustomPropertyDrawer(typeof(SerializableType))]
    public class SerializableTypeDrawer : PropertyDrawer
    {
        private List<string> typeNames;
        private static GUIContent[] typeGUIIContents;
        private SerializedProperty typeNameProp;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (typeNames == null) typeNames = TypeCache.CachedTypes.Select(x=>x.Key).ToList();
            
            if (typeGUIIContents == null)
            {
                typeGUIIContents = TypeCache.CachedTypes.Select(x => new GUIContent(x.Key.Replace('.', '/'))).ToArray();
            }

            property.serializedObject.Update();

            if (typeNameProp == null) typeNameProp = property.FindPropertyRelative("_typeFullName");
            var typeFullName = typeNameProp.stringValue;
            var index = typeNames.IndexOf(typeFullName);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                index = EditorGUI.Popup(position, label, index, typeGUIIContents);
                if (check.changed)
                {
                    typeNameProp.stringValue = typeNames[index];
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}