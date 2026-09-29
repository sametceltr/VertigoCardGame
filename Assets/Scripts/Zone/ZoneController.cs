using System.Collections;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private ZoneBarView zoneBarView;
    [SerializeField] private WheelView wheelView;
    [SerializeField] private ZoneRNG zoneRNG;
    [SerializeField] private ZoneSO[] predefinedZones;

    private ZoneFactory zoneFactory;
    private Zone currentZone;

    private void Awake() {
        Debug.Log("ZoneController Awake");
        InitializeFactory();
    }

    private void InitializeFactory() {
        Debug.Log("InitializeFactory called");
        var rewardStrategy = new RNGRewardStrategy(zoneRNG);
        zoneFactory = new ZoneFactory(rewardStrategy, predefinedZones);
        Debug.Log("Factory initialized");
    }

    private void OnEnable() {
        Debug.Log("ZoneController OnEnable - Subscribing to events");
        GameEvents.OnZoneLevelUp += HandleZoneLevelUp;
        GameEvents.OnGameOver += HandleGameOver;
    }

    private void OnDisable() {
        Debug.Log("ZoneController OnDisable - Unsubscribing from events");
        GameEvents.OnZoneLevelUp -= HandleZoneLevelUp;
        GameEvents.OnGameOver -= HandleGameOver;
    }

    private IEnumerator Start() {
        Debug.Log("ZoneController Start - waiting for ZoneBarView");
        while (!zoneBarView.IsInitialized) {
            yield return null;
        }
        Debug.Log("ZoneBarView initialized, setting zone");
        SetZone(ServiceLocator.GameState.CurrentZone);
    }

    private void HandleZoneLevelUp() {
        Debug.Log("ZoneController.HandleZoneLevelUp called");
        //int newZone = ServiceLocator.GameState.CurrentZone + 1;
        ServiceLocator.GameState.LevelUp();
        SetZone(ServiceLocator.GameState.CurrentZone);
    }

    private void HandleGameOver() {
        SetZone(1);
    }

    private void SetZone(int zoneIndex) {
        Debug.Log($"SetZone called with index: {zoneIndex}");

        if (zoneIndex > ServiceLocator.GameState.MaxLevel) return;

        currentZone = zoneFactory.CreateZone(zoneIndex);
        Debug.Log($"Zone created: {currentZone.Index}, WheelType: {currentZone.WheelType}, Rewards: {currentZone.SliceEntries?.Length}");

        zoneBarView.UpdateZone(currentZone);

        if (wheelView != null) {
            Debug.Log("Configuring wheel...");
            wheelView.Configure(currentZone.WheelType, currentZone.SliceEntries);
        } else {
            Debug.LogError("WheelView is null!");
        }
    }
}
