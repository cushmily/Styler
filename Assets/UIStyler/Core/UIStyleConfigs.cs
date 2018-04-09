using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEditor.Callbacks;
using Utf8Json;

namespace UIStyler.Core
{
    [ResourcePath("Datas")]
    [CreateAssetMenu(menuName = "UIStyler/New StyleData Themes")]
    public class UIStyleConfigs : ResourcesSingleton<UIStyleConfigs>
    {
        #region Runtime Only

        [NonSerialized]
        public UITheme _currentThemeObject;

        [NonSerialized]
        public List<UIStyle> _styleDatas = new List<UIStyle>();

        #endregion

        private string _currentTheme;

        public string CurrentTheme
        {
            get { return _currentTheme; }
            set
            {
                if (_currentThemeObject == null || _currentThemeObject.ThemeName != value)
                {
                    _currentThemeObject = Themes.FirstOrDefault(x => x.ThemeName == value);
                    if (_currentThemeObject == null)
                    {
                        Debug.LogError("Theme nout found : " + CurrentTheme);
                        return;
                    }
                }

                var wrappers = _currentThemeObject.StyleWrappers.Select(JsonSerializer.Deserialize<StyleWrapper>);
                _styleDatas = wrappers
                    .Select(x => JsonSerializer.NonGeneric.Deserialize(x.Type, x.StyleData) as UIStyle).ToList();
                _currentTheme = value;
            }
        }

        public List<UITheme> Themes = new List<UITheme>();

        public string[] StyleTypes;

        public string[] GetStyleNames()
        {
            if (_styleDatas.Count <= 0 && !string.IsNullOrEmpty(CurrentTheme))
            {
                CurrentTheme = CurrentTheme;
            }

            return _styleDatas.Select(x => x.StyleName).ToArray();
        }

        public static readonly Dictionary<string, Type> StylePathMap = new Dictionary<string, Type>();

        public static readonly Dictionary<string, Type> StyleTypeMap = new Dictionary<string, Type>();

        [DidReloadScripts]
        private static void Init()
        {
            var styleList = new List<string>();

            var assembly = typeof(UIStyle).Assembly;
            var uistyles = assembly.GetTypes().Where(x => typeof(UIStyle).IsAssignableFrom(x) && x != typeof(UIStyle));
            foreach (var uistyleType in uistyles)
            {
                var pathAttributes = uistyleType.GetCustomAttributes(typeof(StylePathAttribute), false);
                if (pathAttributes.Length > 0)
                {
                    StylePathMap.Add(((StylePathAttribute) pathAttributes.FirstOrDefault()).Path, uistyleType);
                }

                var styleAttributes = uistyleType.GetCustomAttributes(typeof(StyleTypeAttribute), false);
                if (styleAttributes.Length > 0)
                {
                    styleList.Add(((StyleTypeAttribute) styleAttributes.FirstOrDefault()).Type);
                    StyleTypeMap.Add(((StyleTypeAttribute) styleAttributes.FirstOrDefault()).Type, uistyleType);
                }
            }

            Instance.StyleTypes = styleList.ToArray();
        }
    }
}