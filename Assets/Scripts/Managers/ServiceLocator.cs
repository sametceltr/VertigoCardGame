
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    private static ServiceLocator _instance;

    [SerializeField] private EconomyService economyService;
    [SerializeField] private GameStateService gameStateService;

    public static EconomyService Economy => _instance.economyService;
    public static GameStateService GameState => _instance.gameStateService;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        InitializeServices();
    }

    private void InitializeServices() {
        economyService?.Initialize();
        gameStateService?.Initialize();
    }

    private void OnDestroy() {
        if (_instance == this) {
            _instance = null;
            GameEvents.ClearAllEvents();
        }
    }
}