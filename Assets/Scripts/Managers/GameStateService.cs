using UnityEngine;

[System.Serializable]
public class GameStateService
{
    [SerializeField] private int maxLevel = 60;

    private int currentZone = 1;
    private bool isGameActive = true;

    public int CurrentZone => currentZone;
    public int MaxLevel => maxLevel;
    public bool IsGameActive => isGameActive;

    public void Initialize() {
        currentZone = 1;
        isGameActive = true;
        //GameEvents.ZoneChanged(currentZone);
    }

    public void LevelUp() {
        if (currentZone >= maxLevel) return;

        currentZone++;
        Debug.Log($"GameState.LevelUp called - New Zone: {currentZone}");
        //GameEvents.ZoneLevelUp();
        GameEvents.ZoneChanged(currentZone);
    }

    public void ResetProgress() {
        currentZone = 1;
        isGameActive = true;
        GameEvents.GameOver();
        GameEvents.ZoneChanged(currentZone);
    }

    public void SetGameActive(bool active) {
        isGameActive = active;
    }
}