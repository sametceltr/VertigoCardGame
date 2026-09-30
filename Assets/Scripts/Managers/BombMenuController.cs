using UnityEngine;

public class BombMenuController : MonoBehaviour
{
    [SerializeField] private BombPopupView bombMenuPrefab;
    [SerializeField] private Canvas canvas;

    private BombPopupView currentBombMenu;

    private void OnEnable() {
        GameEvents.OnBombHit += ShowBombMenu;
        GameEvents.OnReviveRequested += HideBombMenu;
        GameEvents.OnGameOver += HideBombMenu;
    }

    private void OnDisable() {
        GameEvents.OnBombHit -= ShowBombMenu;
        GameEvents.OnReviveRequested -= HideBombMenu;
        GameEvents.OnGameOver -= HideBombMenu;
    }

    private void ShowBombMenu() {
        if (currentBombMenu != null) return;

        currentBombMenu = Instantiate(bombMenuPrefab, canvas.transform);
        currentBombMenu.transform.SetAsLastSibling();
    }

    private void HideBombMenu() {
        if (currentBombMenu != null) {
            Destroy(currentBombMenu.gameObject);
            currentBombMenu = null;
        }
    }
}