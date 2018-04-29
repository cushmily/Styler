using System;
using Styler.Core;
using UIStyler.Extensions;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIGraphicStyleData : StyleData
{
    [Header("Graphic")]
    public SColor Color = new SColor(new Color(1, 1, 1, 1));

    public SMaterial Material;
    public SBool RaycastTarget;

    public override void OnUpdateStyle(object obj)
    {
        var target = obj as Graphic;
        if (target == null) throw new ArgumentNullException("target");

        Color.SetTargetValue(c => target.color = c);
        Material.SetTargetValue(m => target.material = m);
        RaycastTarget.SetTargetValue(r => target.raycastTarget = r);
    }
}