using System;

public static class GameEvents
{
    public static event Action OnSpinStarted;
    public static event Action<Reward> OnSpinCompleted;
    public static event Action<Reward> OnRewardAwarded;

    public static event Action<int> OnZoneChanged;
    public static event Action OnZoneLevelUp;

    public static event Action<int> OnCoinBalanceChanged;
    public static event Action<int> OnCoinsSpent;
    public static event Action<int> OnCoinsEarned;

    public static event Action OnBombHit;
    public static event Action OnReviveRequested;
    public static event Action OnGameOver;

    public static void SpinStarted() => OnSpinStarted?.Invoke();
    public static void SpinCompleted(Reward reward) => OnSpinCompleted?.Invoke(reward);
    public static void RewardAwarded(Reward reward) => OnRewardAwarded?.Invoke(reward);

    public static void ZoneChanged(int newZone) => OnZoneChanged?.Invoke(newZone);
    public static void ZoneLevelUp() => OnZoneLevelUp?.Invoke();

    public static void CoinBalanceChanged(int newBalance) => OnCoinBalanceChanged?.Invoke(newBalance);
    public static void CoinsSpent(int amount) => OnCoinsSpent?.Invoke(amount);
    public static void CoinsEarned(int amount) => OnCoinsEarned?.Invoke(amount);

    public static void BombHit() => OnBombHit?.Invoke();
    public static void ReviveRequested() => OnReviveRequested?.Invoke();
    public static void GameOver() => OnGameOver?.Invoke();

    public static void ClearAllEvents() {
        OnSpinStarted = null;
        OnSpinCompleted = null;
        OnRewardAwarded = null;
        OnZoneChanged = null;
        OnZoneLevelUp = null;
        OnCoinBalanceChanged = null;
        OnCoinsSpent = null;
        OnCoinsEarned = null;
        OnBombHit = null;
        OnReviveRequested = null;
        OnGameOver = null;
    }
}