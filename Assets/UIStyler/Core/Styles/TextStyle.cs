using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UIStyler.Core
{
    [Serializable]
    [StyleType("Text")]
    [StylePath("Basic/Text")]
    public class TextStyle : UIStyle
    {
        #region Character

        public Font Font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        public FontStyle FontStyle = FontStyle.Normal;
        public int FontSize = 16;
        public float LineSpace = 0;
        public bool RichText = true;

        #endregion

        #region Paragraph

        public TextAnchor Alignment = TextAnchor.UpperLeft;
        public bool AlignByGeometry = true;
        public HorizontalWrapMode HorizontalOverflow = HorizontalWrapMode.Wrap;
        public VerticalWrapMode VerticalOverflow = VerticalWrapMode.Truncate;
        public bool BestFit = false;

        #endregion

        public override void Apply(MaskableGraphic graphic)
        {
            base.Apply(graphic);
            var text = graphic as Text;
            if (text == null)
            {
                Debug.LogError("Applying TextStyle on wrong ui graphic. - " + graphic);
                return;
            }

            text.font = Font;
            text.fontStyle = FontStyle;
            text.fontSize = FontSize;
            text.lineSpacing = LineSpace;
            text.supportRichText = RichText;

            text.alignment = Alignment;
            text.alignByGeometry = AlignByGeometry;
            text.horizontalOverflow = HorizontalOverflow;
            text.verticalOverflow = VerticalOverflow;
            text.resizeTextForBestFit = BestFit;
        }
    }
}