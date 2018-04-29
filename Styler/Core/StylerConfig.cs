using System.Collections.Generic;
using UIStyler.Extensions;
using UnityEngine;

namespace Styler.Core
{
    [ResourcePath("Default Config")]
    [CreateAssetMenu(menuName = "UI Styler/New Config")]
    public class StylerConfig : ResourcesSingleton<StylerConfig>
    {
        public List<StyleType> StyleTypes = new List<StyleType>();

        public ThemeStyleDictionary AvailableThemes = new ThemeStyleDictionary();
    }
}