using System;
using UnityEngine;

namespace Styler.Core
{
    public abstract class Styler : MonoBehaviour
    {
        public abstract Type Type { get; }
    }

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

        public override Type Type
        {
            get { return typeof(T); }
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