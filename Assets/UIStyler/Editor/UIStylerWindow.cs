using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UIStyler.Core;
using UIStyler.Editor;
using UIStyler.Library;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS;
using UnityEngine;
using Utf8Json;
using Utf8Json.Resolvers;

public class UIStylerWindow : SearchableEditorWindow
{
    private static UIStylerWindow instance;

    private UIStyleConfigs _overrideConfigs;
    private UIStyleConfigs _temp;
    private UIStyleConfigs _configs;

    private Vector2 _scroll;

    private List<bool> _unfoldThemes = new List<bool>();
    private Dictionary<int, string> _renamingThemeNames = new Dictionary<int, string>();

    private Dictionary<string, List<bool>> _styleIsUnfold = new Dictionary<string, List<bool>>();

    private Dictionary<string, Dictionary<int, string>> _renamingStyle =
        new Dictionary<string, Dictionary<int, string>>();

    #region Runtime Datas

    #endregion

    [MenuItem("UIStyle/Config Window #%u")]
    private static void Open()
    {
        if (instance == null)
        {
            instance = CreateInstance<UIStylerWindow>();
        }

        instance.minSize = new Vector2(475, 350);
        instance.titleContent = new GUIContent("UIStyler");
        instance.Show();
    }

    [DidReloadScripts]
    private static void RegisterResolver()
    {
        CompositeResolver.RegisterAndSetAsDefault(new IJsonFormatter[]
        {
            new ObjectJsonFormatter(),
            new MaterialJsonFormatter(),
            new FontJsonFormatter(),
        }, new[]
        {
            StandardResolver.AllowPrivateExcludeNullSnakeCase
        });
    }

    private void OnGUI()
    {
        using (var scrollView = new EditorGUILayout.ScrollViewScope(_scroll))
        {
            _scroll = scrollView.scrollPosition;

            // Load Themes
            LoadThemeSection();

            // Show Themes
            ShowThemes();
        }
    }

    #region Themes

    /// <summary>
    /// Load Theme datas
    /// </summary>
    private void LoadThemeSection()
    {
        if (_configs == null)
        {
            _overrideConfigs =
                EditorGUILayout.ObjectField(_overrideConfigs, typeof(UIStyleConfigs), false) as UIStyleConfigs;
            if (GUILayout.Button(_overrideConfigs == null ? "Load Default" : "Load Themes"))
            {
                _configs = _overrideConfigs != null ? _overrideConfigs : UIStyleConfigs.Instance;
                _temp = _configs;
            }
        }
        else
        {
            _temp = EditorGUILayout.ObjectField(_temp, typeof(UIStyleConfigs), false) as UIStyleConfigs;
            if (_temp != _configs)
            {
                if (GUILayout.Button("Reload Themes"))
                {
                    _configs = _temp;
                }
            }
        }
    }

    /// <summary>
    /// Show all themes
    /// </summary>
    private void ShowThemes()
    {
        if (_configs == null)
        {
            return;
        }

        var themes = _configs.Themes;

        if (GUILayout.Button("Add New Theme"))
        {
            AddNewTheme(new UITheme("Theme"));
        }

        if (GUILayout.Button("Save Themes"))
        {
            SaveAssets();
        }

        ResizeTempDatas(themes);

        if (themes.Count > 0)
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                for (var i = 0; i < themes.Count; i++)
                {
                    var theme = themes[i];
                    GUI.backgroundColor = new Color(0.93f, 0.8f, 0.41f);
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUI.backgroundColor = Color.white;

                    using (new GUILayout.HorizontalScope())
                    {
                        // Fold StyleData Contents
                        if (FoldableButton(_unfoldThemes[i]))
                        {
                            _unfoldThemes[i] = !_unfoldThemes[i];
                        }

                        // rename theme ?
                        if (_renamingThemeNames.ContainsKey(i))
                        {
                            _renamingThemeNames[i] = EditorGUILayout.TextField(_renamingThemeNames[i]);
                        }
                        else if (GUILayout.Button(new GUIContent(themes[i].ThemeName), EditorStyles.label))
                        {
                            _unfoldThemes[i] = !_unfoldThemes[i];
                        }

                        if (_renamingThemeNames.ContainsKey(i) ? ComfirmButton() : EditButton())
                        {
                            if (_renamingThemeNames.ContainsKey(i))
                            {
                                var preName = theme.ThemeName;
                                var newName = _renamingThemeNames[i];
                                _renamingThemeNames.Remove(i);

                                if (!preName.Equals(newName))
                                {
                                    // Rename all relevant
                                    theme.ThemeName = newName;
                                    _styleIsUnfold.Add(newName, new List<bool>(_styleIsUnfold[preName]));
                                    _styleIsUnfold.Remove(preName);
                                    _renamingStyle.Add(newName, new Dictionary<int, string>(_renamingStyle[preName]));
                                    _renamingStyle.Remove(preName);

                                    if (_configs.CurrentTheme == preName)
                                    {
                                        _configs.CurrentTheme = newName;
                                    }
                                }
                            }
                            else
                            {
                                _renamingThemeNames.Add(i, theme.ThemeName);
                            }
                        }

                        if (DeleteButton())
                        {
                            RemoveThemeAt(i);
                            return;
                        }

                        if (DuplicateButton())
                        {
                            CloneNewTheme(theme);
                            return;
                        }

                        if (ApplyButton(theme.ThemeName == _configs.CurrentTheme))
                        {
                            _configs.CurrentTheme = theme.ThemeName;
                            ApplyAllStyler();
                        }
                    }

                    if (_unfoldThemes[i])
                    {
                        ShowThemesContents(i);
                    }

                    GUILayout.EndVertical();
                }
            }
        }
    }
    
    public void ApplyAllStyler()
    {
        var map = UIStyleConfigs.StyleTypeMap;

        var allStyles = _configs._styleDatas.ToDictionary(x=>x.StyleName);
        
        var allStylers = Resources.FindObjectsOfTypeAll<UIStyler.Core.UIStyler>();
        
        for (var i = 0; i < allStylers.Length; i++)
        {
            var styler = allStylers[i];
            var style = allStyles[styler.StyleName];
            var formatter = map[styler.Type];
            if (formatter != style.GetType())
            {
                Debug.Log("UI Style Type nout mapping.");
                continue;
            }
            allStyles[styler.StyleName].Apply(styler.Graphic);
        }
    }

    /// <summary>
    /// Themes contents
    /// </summary>
    /// <param name="index"></param>
    private void ShowThemesContents(int index)
    {
        var preset = _configs.Themes.ElementAt(index);

        if (GUILayout.Button("Add New Style"))
        {
            var newMenu = new GenericMenu();
            for (var i = 0; i < UIStyleConfigs.StylePathMap.Count; i++)
            {
                foreach (var styleType in UIStyleConfigs.StylePathMap)
                {
                    newMenu.AddItem(new GUIContent(styleType.Key), false, () => { AddStyle(preset, styleType.Value); });
                }
            }

            newMenu.ShowAsContext();
        }

        EditorGUI.indentLevel++;
        for (var i = 0; i < preset.StyleWrappers.Count; i++)
        {
            GUI.backgroundColor = new Color(0.48f, 0.93f, 0.62f);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.backgroundColor = Color.white;

            var styleWrapper = JsonSerializer.Deserialize<StyleWrapper>(preset.StyleWrappers[i]);
            var style = JsonSerializer.NonGeneric.Deserialize(styleWrapper.Type, styleWrapper.StyleData) as UIStyle;
            var editor = UIStylerDrawer.GetEditor(style.GetType());

            var toggles = _styleIsUnfold[preset.ThemeName];

            using (new EditorGUILayout.HorizontalScope(GUILayout.Height(20)))
            {
                IndentSpace();

                if (FoldableButton(toggles[i]))
                {
                    toggles[i] = !toggles[i];
                }

                // renaming?
                if (_renamingStyle[preset.ThemeName].ContainsKey(i))
                {
                    _renamingStyle[preset.ThemeName][i] =
                        EditorGUILayout.TextField(_renamingStyle[preset.ThemeName][i]);
                }
                else if (GUILayout.Button(new GUIContent(style.StyleName), EditorStyles.label))
                {
                    toggles[i] = !toggles[i];
                }

                if (_renamingStyle[preset.ThemeName].ContainsKey(i) ? ComfirmButton() : EditButton())
                {
                    if (_renamingStyle[preset.ThemeName].ContainsKey(i))
                    {
                        style.StyleName = _renamingStyle[preset.ThemeName][i];

                        _renamingStyle[preset.ThemeName].Remove(i);
                        styleWrapper.StyleData = JsonSerializer.ToJsonString(style);
                        preset.StyleWrappers[i] = JsonSerializer.ToJsonString(styleWrapper);
                    }
                    else
                    {
                        _renamingStyle[preset.ThemeName].Add(i, style.StyleName);
                    }

                    return;
                }

                if (DeleteButton())
                {
                    RemoveStyleAt(preset, i);
                    return;
                }

                if (DuplicateButton())
                {
                    CloneNewStyle(preset, style);
                    return;
                }
            }

            // if display
            if (toggles[i])
            {
                // style drawer
                editor.PreDrawStyler(style);
                editor.OnDrawStyler(style);
                if (editor.PostDrawStyler(style))
                {
                    styleWrapper.StyleData = JsonSerializer.ToJsonString((object) style);
                    preset.StyleWrappers[i] = JsonSerializer.ToJsonString(styleWrapper);
                }
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUI.indentLevel--;
    }

    #endregion

    #region Internal Operations

    private void ResizeTempDatas(IList<UITheme> presets)
    {
        if (_unfoldThemes == null || _unfoldThemes.Count != presets.Count)
        {
            _unfoldThemes = Enumerable.Repeat(false, presets.Count).ToList();
        }

        if (_styleIsUnfold == null || _styleIsUnfold.Count != presets.Count)
        {
            _styleIsUnfold = new Dictionary<string, List<bool>>();
            for (var i = 0; i < presets.Count; i++)
            {
                var preset = presets[i];
                _styleIsUnfold.Add(preset.ThemeName, Enumerable.Repeat(false, preset.StyleWrappers.Count).ToList());
            }
        }

        if (_renamingStyle == null || _renamingStyle.Count != presets.Count)
        {
            _renamingStyle = new Dictionary<string, Dictionary<int, string>>();
            for (var i = 0; i < presets.Count; i++)
            {
                var preset = presets[i];
                _renamingStyle.Add(preset.ThemeName, new Dictionary<int, string>());
            }
        }
    }

    private void SaveAssets()
    {
        EditorUtility.SetDirty(_configs);
        AssetDatabase.SaveAssets();
    }

    private void SetDefaultTheme()
    {
        UITheme first = null;
        foreach (var theme in _configs.Themes)
        {
            first = theme;
            break;
        }

        if (first != null)
        {
            _configs.CurrentTheme = first.ThemeName;
        }
    }

    private void AddNewTheme(UITheme newPreset)
    {
        var presets = _configs.Themes;
        var index = 0;
        var newName = newPreset.ThemeName;
        while (presets.Any(x => x.ThemeName == newName))
        {
            newName = newPreset.ThemeName + "-" + index;
            index++;
        }

        newPreset.ThemeName = newName;

        presets.Add(newPreset);
        _unfoldThemes.Add(false);
        _styleIsUnfold.Add(newPreset.ThemeName, new List<bool> (Enumerable.Repeat(false, presets.Count)));
        _renamingStyle.Add(newPreset.ThemeName, new Dictionary<int, string>());

        if (presets.Count == 1 || presets.All(x => x.ThemeName != _configs.CurrentTheme))
        {
            SetDefaultTheme();
        }
    }

    private void RemoveThemeAt(int index)
    {
        var presets = _configs.Themes;
        _styleIsUnfold.Remove(presets[index].ThemeName);
        _renamingStyle.Remove(presets[index].ThemeName);
        _unfoldThemes.RemoveAt(index);
        if (_renamingThemeNames.ContainsKey(index))
        {
            _renamingThemeNames.Remove(index);
        }

        presets.RemoveAt(index);
        
        if (presets.Count == 1 || presets.All(x => x.ThemeName != _configs.CurrentTheme))
        {
            SetDefaultTheme();
        }
    }

    private void CloneNewTheme(UITheme template)
    {
        var newPreset = template.Clone() as UITheme;
        if (newPreset != null)
        {
            newPreset.ThemeName = template.ThemeName + "-New";
            AddNewTheme(newPreset);
        }
        else
        {
            Debug.LogError("Clone failed.");
        }
    }

    private void AddStyle(UITheme preset, Type styleType)
    {
        var style = Activator.CreateInstance(styleType) as UIStyle;
        style.StyleName = styleType.Name + " " + preset.StyleWrappers.Count;
        preset.StyleWrappers.Add(style.WrapData());
        _styleIsUnfold[preset.ThemeName].Add(false);
    }

    private void AddStyle(UITheme preset, UIStyle style)
    {
        preset.StyleWrappers.Add(style.WrapData());
        _styleIsUnfold[preset.ThemeName].Add(false);
    }

    private void RemoveStyleAt(UITheme preset, int index)
    {
        _styleIsUnfold[preset.ThemeName].RemoveAt(index);
        if (_renamingStyle[preset.ThemeName].ContainsKey(index))
        {
            _renamingStyle[preset.ThemeName].Remove(index);
        }

        preset.StyleWrappers.RemoveAt(index);
    }

    private void CloneNewStyle(UITheme preset, UIStyle style)
    {
        var newStyle = style.Clone() as UIStyle;
        AddStyle(preset, newStyle);
    }

    #endregion

    #region widgets

    public void IndentSpace()
    {
        GUILayout.Label("", GUILayout.Width(15));
    }

    public bool FoldableButton(bool isUnfold)
    {
        return GUILayout.Button(isUnfold ? "\u25B6" : "\u25BC", EditorStyles.label,
            GUILayout.Width(17));
    }

    public bool ApplyButton(bool inUsed)
    {
        GUI.backgroundColor = inUsed ? Color.green : Color.white;
        var click = GUILayout.Button("APPLY", GUILayout.Width(65));
        GUI.backgroundColor = Color.white;
        return click;
    }

    public bool ComfirmButton()
    {
        return GUILayout.Button("COMFIRM", GUILayout.Width(65));
    }

    public bool EditButton()
    {
        return GUILayout.Button("EDIT", GUILayout.Width(65));
    }

    public bool DeleteButton()
    {
        return GUILayout.Button("DEL", GUILayout.Width(65));
    }

    public bool DuplicateButton()
    {
        return GUILayout.Button("DUP", GUILayout.Width(65));
    }

    #endregion
}