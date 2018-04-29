using UnityEngine;

namespace Styler.Core
{
    public abstract class Styler : MonoBehaviour { }

    [ExecuteInEditMode]
    public abstract class Styler<T> : Styler where T : MonoBehaviour
    {
        public string ThemeName;
        public StyleType StyleType;

        public StyleData StyleData
        {
            get
            {
                var config = StylerConfig.Instance.AvailableThemes;
                if (config.ContainsKey(ThemeName))
                {
                    var styleDict = config[ThemeName];
                    if (StyleType != null && styleDict.ContainsKey(StyleType))
                    {
                        return styleDict[StyleType];
                    }
                }

                return null;
            }
        }

        public abstract T Target { get; }

        protected virtual void Update()
        {
            if (Application.isEditor && StyleData != null)
            {
                StyleData.OnUpdateStyle(Target);
            }
        }
    }
}