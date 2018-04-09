using UIStyler.Core;
using UnityEditor;
using UnityEngine;

namespace UIStyler.Editor
{
    public class BaseUIStylerDrawer : UIStylerDrawer
    {
        public override void OnDrawStyler(UIStyle style)
        {
            style.Tint = EditorGUILayout.ColorField("Color", style.Tint);
            style.RayCastTarget = EditorGUILayout.Toggle("RayCastTarget", style.RayCastTarget);
            style.Material =
                EditorGUILayout.ObjectField("Material", style.Material, typeof(Material), false) as Material;
        }
    }
}