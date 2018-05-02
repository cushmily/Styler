using System.Collections.Generic;
using System.Linq;
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

                var themeNames = _config.AvailableThemes.Select(x => x.Key).ToList();
                var removeThemes = new List<string>();

                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    for (var i = 0; i < themeNames.Count; i++)
                    {
                        var toggleKey = "ToggleTheme-" + themeNames[i];
                        using (new GUILayout.HorizontalScope())
                        {
                            EditorPrefs.SetBool(toggleKey,
                                GUILayout.Toggle(EditorPrefs.GetBool(toggleKey, false), themeNames[i]));
                            if (GUILayout.Button("Remove", EditorStyles.toolbarButton, GUILayout.Width(75)))
                            {
                                removeThemes.Add(themeNames[i]);
                            }

                            GUILayout.Button("Apply", EditorStyles.toolbarButton, GUILayout.Width(75));
                        }

                        var dataDict = _config.AvailableThemes[themeNames[i]];

                        foreach (var styleType in _config.StyleTypes)
                        {
                            if (!dataDict.ContainsKey(styleType))
                            {
                                dataDict.Add(styleType, null);
                            }
                        }

                        if (EditorPrefs.GetBool(toggleKey, false))
                        {
                            EditorGUI.indentLevel++;
                            foreach (var styleType in _config.StyleTypes)
                            {
                                using (new EditorGUILayout.HorizontalScope())
                                {
                                    EditorGUILayout.LabelField(styleType.name);
                                    var data = dataDict[styleType];

                                    var newObj = EditorGUILayout.ObjectField(data, typeof(StyleData), false);

                                    if (newObj != dataDict[styleType])
                                    {
                                        dataDict[styleType] = newObj as StyleData;
                                    }
                                }
                            }

                            EditorGUI.indentLevel--;
                        }
                    }
                }

                for (var i = 0; i < removeThemes.Count; i++)
                {
                    _config.AvailableThemes.Remove(removeThemes[i]);
                }

                if (check.changed) serializedObject.ApplyModifiedProperties();
            }
        }
    }
}