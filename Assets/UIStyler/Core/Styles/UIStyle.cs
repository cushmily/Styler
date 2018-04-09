using System;
using UnityEngine;
using UnityEngine.UI;

namespace UIStyler.Core
{
    [Serializable]
    public abstract class UIStyle : ICloneable
    {
        public string StyleName { get; set; }

        #region General

        public Material Material;
        public bool RayCastTarget;
        public Color Tint = Color.black;

        #endregion

        public virtual void Apply(MaskableGraphic graphic)
        {
            graphic.material = Material;
            graphic.raycastTarget = RayCastTarget;
            graphic.color = Tint;
        }

        public StyleWrapper Wrap()
        {
            return new StyleWrapper(this);
        }

        public string WrapData()
        {
            return new StyleWrapper(this).ToWarperData();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}