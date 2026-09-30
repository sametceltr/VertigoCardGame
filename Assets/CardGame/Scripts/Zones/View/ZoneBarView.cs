using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ZoneBarView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform content;
    [SerializeField] private RectTransform indicatorRect;
    [SerializeField] private Image indicatorBackground;
    [SerializeField] private ZoneItem zoneItemPrefab;

    [Header("Settings")]
    [SerializeField] private int maxLevel = 60;
    [SerializeField] private float transitionDuration = 0.35f;

    private readonly Dictionary<int, ZoneItem> zoneItems = new();
    private ZoneItem currentZoneItem;
    private bool isInitialized = false;

    public bool IsInitialized => isInitialized;

    private void Start() {
        StartCoroutine(InitializeZoneItems());
    }

    private IEnumerator InitializeZoneItems() {
        yield return new WaitForEndOfFrame();
        GenerateZoneItems();
    }

    private void GenerateZoneItems() {
        if (isInitialized) return;

        float itemWidth = indicatorRect.rect.xMax - indicatorRect.rect.xMin;

        for (int i = 1; i <= maxLevel; i++) {
            var zoneItem = Instantiate(zoneItemPrefab, content);
            zoneItem.RectTransform.anchoredPosition = new Vector2((i - 1) * itemWidth, 0);
            zoneItem.RectTransform.sizeDelta = new Vector2(itemWidth, zoneItem.RectTransform.sizeDelta.y);

            var zone = new Zone(i);
            zoneItem.Initialize(zone, indicatorBackground);
            zoneItems.Add(i, zoneItem);
        }

        isInitialized = true;
    }

    public void UpdateZone(Zone zone) {
        if (!isInitialized) GenerateZoneItems();

        if (zone.Index > maxLevel) return;

        if (currentZoneItem != null) {
            currentZoneItem.Configure(zone.Index);
        }

        currentZoneItem = zoneItems[zone.Index];
        currentZoneItem.Configure(zone.Index);

        AnimateToZone(zone.Index);
    }

    private void AnimateToZone(int zoneIndex) {
        float itemWidth = indicatorRect.rect.xMax - indicatorRect.rect.xMin;
        int centerIndex = zoneIndex - 1;
        float targetX = -(centerIndex * itemWidth);

        float originalX = indicatorRect.anchoredPosition.x;
        float originalY = indicatorRect.anchoredPosition.y;

        indicatorRect.anchoredPosition = new Vector2(originalX + itemWidth, originalY);

        var seq = DOTween.Sequence();
        seq.Join(content.DOAnchorPosX(targetX, transitionDuration).SetEase(Ease.OutCubic));
        seq.Join(indicatorRect.DOAnchorPosX(originalX, transitionDuration).SetEase(Ease.OutCubic));
    }
}