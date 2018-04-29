using Styler.Core;
using UnityEditor;
using UnityEngine;

namespace Styler.Editor
{
    [CustomPropertyDrawer(typeof(SVar), true)]
    public class SVarPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.Update();
            var enableProp = property.FindPropertyRelative("_enable");
            var valueProp = property.FindPropertyRelative("_value");

            var togglePos = new Rect(position.position, new Vector2(30, position.size.y));
            var valuePos = new Rect(new Vector2(30, position.position.y),
                new Vector2(position.size.x - 30, position.size.y));

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                enableProp.boolValue = EditorGUI.Toggle(togglePos, enableProp.boolValue);
                EditorGUI.PropertyField(valuePos, valueProp, label);

                if (check.changed) property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}