using UnityEngine;

namespace Styler.Core
{
    [CreateAssetMenu(menuName = "UI Styler/New Style Type")]
    public class StyleType : ScriptableObject
    {
        public SerializableType Type;
    }
}