using Styler.Core;
using UIStyler.Extensions;
using UnityEditor;
using UnityEngine;

namespace Styler.Editor
{
    [CustomEditor(typeof(StylerConfig))]
    public class UIStylerConfigEditor : UnityEditor.Editor
    {
        private StylerConfig _config;

        private string _newThemeName;

        private void OnEnable()
        {
            _config = target as StylerConfig;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                // Styles
                EditorGUILayout.PropertyField(serializedObject.FindProperty("StyleTypes"), true);
                EditorGUILayout.Separator();

                // Add new theme
                using (new EditorGUILayout.HorizontalScope())
                {
                    _newThemeName = EditorGUILayout.TextField("Add New Theme", _newThemeName);
                    using (new EditorGUI.DisabledGroupScope(string.IsNullOrEmpty(_newThemeName) ||
                                                            _config.AvailableThemes.ContainsKey(_newThemeName)))
                    {
                        if (GUILayout.Button("Add"))
                        {
                            _config.AvailableThemes.Add(_newThemeName, new StyleDataDictionary());
                        }
                    }
                }

                foreach (var availableTheme in _config.AvailableThemes)
                {
                    var toggleKey = "ToggleTheme-" + availableTheme.Key;
                    EditorPrefs.SetBool(toggleKey,
                        EditorGUILayout.ToggleLeft(availableTheme.Key, EditorPrefs.GetBool(toggleKey, false)));
                    if (EditorPrefs.GetBool(toggleKey, false))
                    {
                        EditorGUI.indentLevel++;
                        var dataDict = availableTheme.Value;
                        foreach (var styleType in _config.StyleTypes)
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.LabelField(styleType.name);

                                if (!dataDict.ContainsKey(styleType))
                                {
                                    dataDict.Add(styleType, null);
                                }

                                var newObj = EditorGUILayout.ObjectField(dataDict[styleType], typeof(StyleData), false);

                                if (newObj != dataDict[styleType])
                                {
                                    dataDict[styleType] = newObj as StyleData;
                                }
                            }
                        }

                        EditorGUI.indentLevel--;
                    }
                }

                if (check.changed) serializedObject.ApplyModifiedProperties();
            }
        }
    }
}