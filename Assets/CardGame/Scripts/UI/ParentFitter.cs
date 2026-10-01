using UnityEngine;
using UnityEngine.UI;

namespace CardGame.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(Image))]
    public class ParentFitter : AspectRatioFitter
    {
        private Image _image;
        private float _nativeRatio;

        protected override void Awake() {
            base.Awake();
            _image = GetComponent<Image>();
            aspectMode = AspectMode.FitInParent;
        }

        protected override void Start() {
            base.Start();
            CalculateAspectRatio();
        }

    #if UNITY_EDITOR
        protected override void OnValidate() {
            base.OnValidate();
            if (aspectMode != AspectMode.FitInParent) aspectMode = AspectMode.FitInParent;
            if (aspectRatio != _nativeRatio) CalculateAspectRatio();
        }
    #endif

        public void CalculateAspectRatio() {
            if (_image == null || _image.sprite == null) return;

            var spriteRect = _image.sprite.rect;
            _nativeRatio = spriteRect.width / spriteRect.height;
            aspectRatio = _nativeRatio;
        }
    }
}
