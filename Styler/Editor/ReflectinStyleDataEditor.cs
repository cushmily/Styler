using System.Linq;
using Styler.Core;
using UnityEditor;
using UnityEngine;

namespace Styler.Editor
{
    //TODO
    public class ReflectinStyleDataEditor<T> : UnityEditor.Editor where T : MonoBehaviour
    {
        protected ReflectionStyleData<T> StyleData;

        private void OnEnable()
        {
            StyleData = target as ReflectionStyleData<T>;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Fields"));

            var keys = StyleData.Fields.Select(x => x.Key).ToList();
            for (var i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                StyleData.Fields[key] = EditorGUILayout.ToggleLeft(key, StyleData.Fields[key]);
            }
        }
    }
}