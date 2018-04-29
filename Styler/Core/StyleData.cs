using UnityEngine;

namespace Styler.Core
{
    public abstract class StyleData : ScriptableObject
    {
        public abstract void OnUpdateStyle(object target);
    }
}