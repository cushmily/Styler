using Styler.Core;
using UnityEngine.UI;

public class TextStyler : Styler<Text>
{
    private Text _target;

    public override Text Target
    {
        get
        {
            if (_target == null)
            {
                _target = GetComponentInChildren<Text>();
            }

            return _target;
        }
    }
}