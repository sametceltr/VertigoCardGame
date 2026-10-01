using TMPro;
using UnityEngine;

namespace CardGame.Zones.View
{
    public class ZoneItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI zoneText;
        [SerializeField] private RectTransform rectTransform;

        public RectTransform RectTransform => rectTransform;
    }
}
