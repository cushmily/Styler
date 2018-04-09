using UIStyler.Core;
using UnityEditor;
using UnityEngine;

namespace UIStyler.Editor
{
    [CustomStylerEditor(typeof(TextStyle))]
    public class TextStylerDrawer : BaseUIStylerDrawer
    {
        public override void OnDrawStyler(UIStyle style)
        {
            var textStyle = style as TextStyle;
            base.OnDrawStyler(style);

            EditorGUILayout.Separator();
            textStyle.Font = EditorGUILayout.ObjectField("Font", textStyle.Font, typeof(Font), false) as Font;
            textStyle.FontStyle = (FontStyle) EditorGUILayout.EnumPopup("Font Style", textStyle.FontStyle);
            textStyle.FontSize = EditorGUILayout.IntField("Font Size", textStyle.FontSize);
            textStyle.LineSpace = EditorGUILayout.FloatField("Line Space", textStyle.LineSpace);
            textStyle.RichText = EditorGUILayout.Toggle("Rich Text", textStyle.RichText);

            EditorGUILayout.Separator();
            textStyle.Alignment = (TextAnchor) EditorGUILayout.EnumPopup("Alignment", textStyle.Alignment);
            textStyle.AlignByGeometry = EditorGUILayout.Toggle("Align By Geometry", textStyle.AlignByGeometry);
            textStyle.HorizontalOverflow =
                (HorizontalWrapMode) EditorGUILayout.EnumPopup("Horizontan Overflow", textStyle.HorizontalOverflow);
            textStyle.VerticalOverflow =
                (VerticalWrapMode) EditorGUILayout.EnumPopup("Vertical Overflow", textStyle.VerticalOverflow);
            textStyle.BestFit = EditorGUILayout.Toggle("Best Fit", textStyle.BestFit);
        }
    }
}