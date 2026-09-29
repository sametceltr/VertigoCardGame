using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [Header("Core Systems")]
    [SerializeField] private ServiceLocator serviceLocator;

    [Header("Controllers")]
    [SerializeField] private ZoneController zoneController;
    [SerializeField] private RewardController rewardController;
    [SerializeField] private BombMenuController bombMenuController;
    [SerializeField] private UIController uiController;

    private void Awake() {
        InitializeGame();
    }

    private void InitializeGame() {
        Debug.Log("Game initialized successfully!");
    }

    private void OnDestroy() {
        GameEvents.ClearAllEvents();
    }
}