using UIStyler.Core;
using UnityEditor;
using UnityEngine;

namespace UIStyler.Editor
{
    public class DefaultUIStylerDrawer : UIStylerDrawer
    {
        public override void OnDrawStyler(UIStyle style)
        {
            GUI.color = Color.red;
            EditorGUILayout.LabelField("UI Styler Editor Not Found.");
            GUI.color = Color.white;
        }
    }
}