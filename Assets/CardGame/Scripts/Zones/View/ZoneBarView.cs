using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Zones.View
{
    public class ZoneBarView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform content;
        [SerializeField] private RectTransform indicatorRect;
        [SerializeField] private Image indicatorBackground;
        [SerializeField] private ZoneItem zoneItemPrefab;

        [Header("Settings")]
        [SerializeField] private float transitionDuration = 0.35f;
    }
}