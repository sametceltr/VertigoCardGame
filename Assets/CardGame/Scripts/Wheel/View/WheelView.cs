using System.Collections.Generic;
using CardGame.Run;
using CardGame.UI;
using CardGame.Zones;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Wheel.View
{
    public class WheelView : MonoBehaviour
    {
        private const string SpinButtonName = "ui_button_spin";
        private const float FullTurn = 360f;

        [Header("Wheel")]
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Image _baseImage;
        [SerializeField] private Image _indicatorImage;
        [SerializeField] private SliceView[] _slices;
        [SerializeField] private Sprite _bombIcon;
        [SerializeField, Range(0f, 1f)] private float _disarmedBombAlpha = 0.4f;

        [Header("Buttons")]
        [SerializeField] private Button _spinButton;

        [Header("Animation")]
        [SerializeField] private float _spinDuration = 3f;
        [SerializeField] private int _extraTurns = 3;
        [SerializeField] private Ease _spinEase = Ease.OutQuart;

        private GameRun _run;
        private ZoneVisualsSO _visuals;
        private IReadOnlyList<WheelSlice> _currentSlices;
        private Tween _spinTween;

        public int SliceCount => _slices.Length;

        public void Initialize(GameRun run, ZoneVisualsSO visuals) {
            _run = run;
            _visuals = visuals;

            _run.ZoneChanged += OnZoneChanged;
            _run.StateChanged += OnStateChanged;
            _run.SpinStarted += OnSpinStarted;
            _spinButton.onClick.AddListener(_run.Spin);
        }

        private void OnDestroy() {
            _spinTween?.Kill();
            if (_run == null) return;

            _run.ZoneChanged -= OnZoneChanged;
            _run.StateChanged -= OnStateChanged;
            _run.SpinStarted -= OnSpinStarted;
        }

        private void OnZoneChanged(ZoneInfo zone) {
            var visuals = _visuals.For(zone.Type);
            _titleText.text = visuals.Title;
            _titleText.color = visuals.TitleColor;
            _baseImage.sprite = visuals.WheelBase;
            _indicatorImage.sprite = visuals.WheelIndicator;

            _currentSlices = zone.Slices;
            for (int i = 0; i < _slices.Length; i++) {
                _slices[i].Show(_currentSlices[i], _bombIcon);
            }

            _baseImage.rectTransform.localRotation = Quaternion.identity;
        }

        private void OnStateChanged(RunState state) {
            _spinButton.interactable = state == RunState.Ready;

            float bombAlpha = _run.IsBombDisarmed ? _disarmedBombAlpha : 1f;
            for (int i = 0; i < _currentSlices.Count; i++) {
                if (_currentSlices[i].Type == SliceType.Bomb) _slices[i].SetAlpha(bombAlpha);
            }
        }

        private void OnSpinStarted(int landingIndex) {
            var wheel = _baseImage.rectTransform;
            float sliceAngle = FullTurn / _slices.Length;
            float currentAngle = wheel.localEulerAngles.z;
            float landingAngle = landingIndex * sliceAngle;
            float clockwiseDistance = Mathf.Repeat(currentAngle - landingAngle, FullTurn);
            float endAngle = currentAngle - (_extraTurns * FullTurn + clockwiseDistance);

            _spinTween = wheel.DOLocalRotate(new Vector3(0f, 0f, endAngle), _spinDuration, RotateMode.FastBeyond360)
                .SetEase(_spinEase)
                .OnComplete(_run.CompleteSpin);
        }

    #if UNITY_EDITOR
        private void OnValidate() {
            if (_spinButton == null) _spinButton = ChildButtons.Find(transform, SpinButtonName);
            if (_spinButton == null) Debug.LogWarning($"{name}: no child button named {SpinButtonName}.", this);
        }
    #endif
    }
}
