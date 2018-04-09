using UnityEngine;
using UnityEngine.UI;

namespace UIStyler.Core
{
    [RequireComponent(typeof(MaskableGraphic))]
    public class UIStyler : MonoBehaviour
    {
        [StyleNameSelector]
        public string StyleName;

        [StyleTypeSelector]
        public string Type;

        public bool UpdateOnStart;

        private MaskableGraphic _graphic;

        public MaskableGraphic Graphic
        {
            get
            {
                if (_graphic == null)
                {
                    _graphic = GetComponent<MaskableGraphic>();
                }

                return _graphic;
            }
        }

        private void Awake()
        {
            if (UpdateOnStart)
            {
                SetType();
            }
        }

        private void SetType()
        {
            _graphic = GetComponent<MaskableGraphic>();
        }

        private void OnStyleUpdate(string type, UIStyle style)
        {
            if (Type != type)
            {
                return;
            }

            if (style.StyleName != StyleName)
            {
                return;
            }

            if (_graphic != null)
            {
                style.Apply(_graphic);
            }
        }
    }
}