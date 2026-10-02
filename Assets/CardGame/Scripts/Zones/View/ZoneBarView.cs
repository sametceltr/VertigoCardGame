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

        private RectTransform BoxRect => _currentZoneBox.rectTransform;
        private bool HasCurrentZone => _currentZone >= ZoneProgressionSO.FirstZone;

        public void Initialize(GameRun run, ZoneProgressionSO progression, ZoneVisualsSO visuals) {
            _run = run;
            _visuals = visuals;
            _boxRestX = BoxRect.anchoredPosition.x;

            SpawnItems(progression);
            _run.ZoneChanged += OnZoneChanged;
        }

        private void OnDestroy() {
            KillTweens();
            if (_run == null) return;

            _run.ZoneChanged -= OnZoneChanged;
        }

        private void OnRectTransformDimensionsChange() {
            if (_items == null) return;

            LayOutItems();
            if (HasCurrentZone) SnapToCurrent();
        }

        private void OnZoneChanged(ZoneInfo zone) {
            if (_itemWidth == 0f) LayOutItems();

            bool isNextZone = HasCurrentZone && zone.Zone == _currentZone + 1;

            Highlight(zone.Zone);
            ApplyBoxVisuals(zone.Type);

            if (isNextZone) SlideToCurrent();
            else SnapToCurrent();
        }

        private void SpawnItems(ZoneProgressionSO progression) {
            _items = new ZoneItem[IndexOf(progression.MaxZone) + 1];
            for (int zone = ZoneProgressionSO.FirstZone; zone <= progression.MaxZone; zone++) {
                var item = Instantiate(_zoneItemPrefab, _content);
                item.Initialize(zone, _visuals.For(progression.GetZoneType(zone)));
                _items[IndexOf(zone)] = item;
            }
        }

        private void LayOutItems() {
            _itemWidth = BoxRect.rect.width;

            for (int i = 0; i < _items.Length; i++) {
                var rect = _items[i].RectTransform;
                rect.anchoredPosition = new Vector2(i * _itemWidth, 0f);
                rect.sizeDelta = new Vector2(_itemWidth, rect.sizeDelta.y);
            }
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

        private static int IndexOf(int zone) => zone - ZoneProgressionSO.FirstZone;

        private void KillTweens() {
            _content.DOKill();
            BoxRect.DOKill();
        }

        private static void SetX(RectTransform rect, float x) {
            rect.anchoredPosition = new Vector2(x, rect.anchoredPosition.y);
        }
    }
}
