using System.Collections.Generic;
using System.Linq;
using Styler.Core;
using UnityEditor;
using UnityEngine;

namespace Styler.Editor
{
    [CustomEditor(typeof(Core.Styler), true)]
    public class StylerEditor : UnityEditor.Editor
    {
        private List<string> ThemeNames;
        private GUIContent[] ThemeNameContents;
        private List<StyleType> StyleTypes;
        private GUIContent[] StyleTypeNameContents;

        private SerializedProperty ThemeNameProp;
        private SerializedProperty StyleTypeProp;
        private SerializedProperty StyleDataProp;

        private void OnEnable()
        {
            ThemeNames = StylerConfig.Instance.AvailableThemes.Select(x => x.Key).ToList();
            ThemeNameContents = ThemeNames.Select(x => new GUIContent(x)).ToArray();

            StyleTypes = StylerConfig.Instance.StyleTypes.ToList();
            StyleTypeNameContents = StyleTypes.Select(x => new GUIContent(x.name)).ToArray();

            ThemeNameProp = serializedObject.FindProperty("ThemeName");
            StyleTypeProp = serializedObject.FindProperty("StyleType");
            StyleDataProp = serializedObject.FindProperty("StyleData");

//            SetStyleData(ThemeNameProp.stringValue, (StyleType) StyleTypeProp.objectReferenceValue);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var themeName = ThemeNameProp.stringValue;
            var index = ThemeNames.IndexOf(themeName);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                index = EditorGUILayout.Popup(new GUIContent("Theme Name"), index, ThemeNameContents);
                ThemeNameProp.stringValue = ThemeNames[index];

                var type = StyleTypeProp.objectReferenceValue as StyleType;
                index = StyleTypes.IndexOf(type);

                index = EditorGUILayout.Popup(new GUIContent("Style Type"), index, StyleTypeNameContents);
                StyleTypeProp.objectReferenceValue = StyleTypes[index];

                if (check.changed)
                {
//                    SetStyleData(themeName, type);
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }

        private void SetStyleData(string themeName, StyleType type)
        {
            if (StylerConfig.Instance.AvailableThemes.ContainsKey(themeName))
            {
                var themeDict = StylerConfig.Instance.AvailableThemes[themeName];
                if (type != null && themeDict.ContainsKey(type))
                {
                    StyleDataProp.objectReferenceValue = themeDict[type];
                }
            }
        }
    }
}