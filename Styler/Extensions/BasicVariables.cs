using System;
using Styler.Core;
using UnityEngine;

namespace UIStyler.Extensions
{
    [Serializable]
    public class SInt : SVar<int>
    {
        public SInt(int defaultValue) : base(defaultValue) { }
    }

    [Serializable]
    public class SFloat : SVar<float>
    {
        public SFloat(float defaultValue) : base(defaultValue) { }
    }

    [Serializable]
    public class SBool : SVar<bool>
    {
        public SBool(bool defaultValue) : base(defaultValue) { }
    }

    [Serializable]
    public class SString : SVar<string>
    {
        public SString(string defaultValue) : base(defaultValue) { }
    }

    [Serializable]
    public class SMaterial : SVar<Material>
    {
        public SMaterial(Material defaultValue) : base(defaultValue) { }
    }

    [Serializable]
    public class SColor : SVar<Color>
    {
        public SColor(Color defaultValue) : base(defaultValue) { }
    }

    [Serializable]
    public class STexture : SVar<Texture>
    {
        public STexture(Texture defaultValue) : base(defaultValue) { }
    }

    #region Text Datas

    [Serializable]
    public class SFont : SVar<Font>
    {
        public SFont(Font defaultValue) : base(defaultValue) { }
    }

    [Serializable]
    public class SFontStyle : SVar<FontStyle>
    {
        public SFontStyle(FontStyle defaultValue) : base(defaultValue) { }
    }

    [Serializable]
    public class STextAnchor : SVar<TextAnchor>
    {
        public STextAnchor(TextAnchor defaultValue) : base(defaultValue) { }
    }

    [Serializable]
    public class SHorizontalWrapMode : SVar<HorizontalWrapMode>
    {
        public SHorizontalWrapMode(HorizontalWrapMode defaultValue) : base(defaultValue) { }
    }

    [Serializable]
    public class SVerticalWrapMode : SVar<VerticalWrapMode>
    {
        public SVerticalWrapMode(VerticalWrapMode defaultValue) : base(defaultValue) { }
    }

    #endregion
}