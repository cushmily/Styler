using UIStyler.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Samples.Scripts
{
    [CreateAssetMenu(menuName = "UI Styler/Style Data/New Text Style Data")]
    public class TextStyleData : UIGraphicStyleData
    {
        [Header("Character")]
        public SFont Font;

        public SFontStyle FontStyle = new SFontStyle(UnityEngine.FontStyle.Normal);
        public SInt FontSize = new SInt(16);
        public SFloat LineSpace;
        public SBool RichText = new SBool(true);

        [Header("Paragraph")]
        public STextAnchor Alignment = new STextAnchor(TextAnchor.MiddleCenter);

        public SBool AlignByGeometry = new SBool(true);

        public SHorizontalWrapMode HorizontalOverflow = new SHorizontalWrapMode(HorizontalWrapMode.Overflow);
        public SVerticalWrapMode VerticalOverflow = new SVerticalWrapMode(VerticalWrapMode.Overflow);
        public SBool BestFit;

        public override void OnUpdateStyle(object obj)
        {
            base.OnUpdateStyle(obj);
            var text = obj as Text;
            if (text == null) return;
            if (Font == null || Font.Value == null) Font = new SFont(Resources.GetBuiltinResource<Font>("Arial.ttf"));

            Font.SetTargetValue(f => text.font = f);
            FontStyle.SetTargetValue(fs => text.fontStyle = fs);
            FontSize.SetTargetValue(fs => text.fontSize = fs);
            LineSpace.SetTargetValue(ls => text.lineSpacing = ls);
            RichText.SetTargetValue(rt => text.supportRichText = rt);

            Alignment.SetTargetValue(a => text.alignment = a);
            AlignByGeometry.SetTargetValue(abg => text.alignByGeometry = abg);
            HorizontalOverflow.SetTargetValue(hw => text.horizontalOverflow = hw);
            VerticalOverflow.SetTargetValue(vw => text.verticalOverflow = vw);
            BestFit.SetTargetValue(bf => text.resizeTextForBestFit = bf);
        }
    }
}