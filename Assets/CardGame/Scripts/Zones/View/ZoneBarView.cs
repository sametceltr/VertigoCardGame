using CardGame.Run;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Zones.View
{
    public class ZoneBarView : MonoBehaviour
    {
        [Header("Bar")]
        [SerializeField] private RectTransform _content;
        [SerializeField] private Image _currentZoneBox;
        [SerializeField] private ZoneItem _zoneItemPrefab;

        [Header("Animation")]
        [SerializeField] private float _transitionDuration = 0.35f;
        [SerializeField] private Ease _transitionEase = Ease.OutCubic;

        private GameRun _run;
        private ZoneVisualsSO _visuals;
        private ZoneItem[] _items;
        private float _itemWidth;
        private float _boxRestX;
        private int _currentZone;
        private bool _isLaidOut;

        private RectTransform BoxRect => _currentZoneBox.rectTransform;
        private bool HasCurrentZone => _currentZone > 0;

        public void Initialize(GameRun run, ZoneProgressionSO progression, ZoneVisualsSO visuals) {
            _run = run;
            _visuals = visuals;

            SpawnItems(progression);
            _run.ZoneChanged += OnZoneChanged;
        }

        private void OnDestroy() {
            KillTweens();
            if (_run == null) return;

            _run.ZoneChanged -= OnZoneChanged;
        }

        private void OnZoneChanged(ZoneInfo zone) {
            if (!_isLaidOut) LayOutItems();

            bool isNextZone = HasCurrentZone && zone.Zone == _currentZone + 1;

            Highlight(zone.Zone);
            ApplyBoxVisuals(zone.Type);

            if (isNextZone) SlideToCurrent();
            else SnapToCurrent();
        }

        private void SpawnItems(ZoneProgressionSO progression) {
            _items = new ZoneItem[progression.MaxZone];
            for (int zone = 1; zone <= _items.Length; zone++) {
                var item = Instantiate(_zoneItemPrefab, _content);
                item.Initialize(zone, _visuals.For(progression.GetZoneType(zone)));
                _items[IndexOf(zone)] = item;
            }
        }

        private void LayOutItems() {
            _itemWidth = BoxRect.rect.width;
            _boxRestX = BoxRect.anchoredPosition.x;

            for (int zone = 1; zone <= _items.Length; zone++) {
                var rect = ItemFor(zone).RectTransform;
                rect.anchoredPosition = new Vector2(IndexOf(zone) * _itemWidth, 0f);
                rect.sizeDelta = new Vector2(_itemWidth, rect.sizeDelta.y);
            }
            _isLaidOut = true;
        }

        private void Highlight(int zone) {
            if (HasCurrentZone) ItemFor(_currentZone).SetCurrent(false);
            _currentZone = zone;
            ItemFor(_currentZone).SetCurrent(true);
        }

        private void ApplyBoxVisuals(ZoneType type) {
            var visuals = _visuals.For(type);
            _currentZoneBox.sprite = visuals.CurrentZoneBoxSprite;
            _currentZoneBox.color = visuals.CurrentZoneBoxColor;
        }

        private void SlideToCurrent() {
            KillTweens();
            SetX(BoxRect, _boxRestX + _itemWidth);
            _content.DOAnchorPosX(ContentXForCurrent(), _transitionDuration).SetEase(_transitionEase);
            BoxRect.DOAnchorPosX(_boxRestX, _transitionDuration).SetEase(_transitionEase);
        }

        private void SnapToCurrent() {
            KillTweens();
            SetX(_content, ContentXForCurrent());
            SetX(BoxRect, _boxRestX);
        }

        private float ContentXForCurrent() => -IndexOf(_currentZone) * _itemWidth;

        private ZoneItem ItemFor(int zone) => _items[IndexOf(zone)];

        private static int IndexOf(int zone) => zone - 1;

        private void KillTweens() {
            _content.DOKill();
            BoxRect.DOKill();
        }

        private static void SetX(RectTransform rect, float x) {
            rect.anchoredPosition = new Vector2(x, rect.anchoredPosition.y);
        }
    }
}
