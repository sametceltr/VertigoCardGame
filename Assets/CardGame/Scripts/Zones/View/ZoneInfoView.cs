using CardGame.Run;
using TMPro;
using UnityEngine;

namespace CardGame.Zones.View
{
    public class ZoneInfoView : MonoBehaviour
    {
        [Header("Super Zone")]
        [SerializeField] private GameObject _superPanel;
        [SerializeField] private TextMeshProUGUI _superZoneText;

        [Header("Safe Zone")]
        [SerializeField] private GameObject _safePanel;
        [SerializeField] private TextMeshProUGUI _safeZoneText;

        private GameRun _run;
        private ZoneProgressionSO _progression;

        public void Initialize(GameRun run, ZoneProgressionSO progression) {
            _run = run;
            _progression = progression;
            _run.ZoneChanged += OnZoneChanged;
        }

        private void OnDestroy() {
            if (_run == null) return;

            _run.ZoneChanged -= OnZoneChanged;
        }

        private void OnZoneChanged(ZoneInfo zone) {
            ShowNextZone(zone.Zone, ZoneType.Super, _superPanel, _superZoneText);
            ShowNextZone(zone.Zone, ZoneType.Safe, _safePanel, _safeZoneText);
        }

        private void ShowNextZone(int currentZone, ZoneType type, GameObject panel, TextMeshProUGUI zoneText) {
            bool hasNextZone = _progression.TryGetNextZone(currentZone, type, out int nextZone);
            panel.SetActive(hasNextZone);
            if (hasNextZone) zoneText.text = nextZone.ToString();
        }
    }
}
