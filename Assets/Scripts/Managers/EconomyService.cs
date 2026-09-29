using UnityEngine;

[System.Serializable]
public class EconomyService
{
    [SerializeField] private int startingCoins = 100;
    [SerializeField] private int baseReviveCost = 25;

    private int currentBalance;
    private int currentReviveCost;

    public int CurrentBalance => currentBalance;
    public int ReviveCost => currentReviveCost;

    public void Initialize() {
        currentBalance = startingCoins;
        currentReviveCost = baseReviveCost;
        GameEvents.CoinBalanceChanged(currentBalance);
    }

    public bool CanAfford(int amount) {
        return currentBalance >= amount;
    }

    public bool TrySpendCoins(int amount) {
        if (!CanAfford(amount)) return false;

        currentBalance -= amount;
        GameEvents.CoinsSpent(amount);
        GameEvents.CoinBalanceChanged(currentBalance);
        return true;
    }

    public void AddCoins(int amount) {
        currentBalance += amount;
        GameEvents.CoinsEarned(amount);
        GameEvents.CoinBalanceChanged(currentBalance);
    }

    public bool TryRevive() {
        if (!TrySpendCoins(currentReviveCost)) return false;

        currentReviveCost *= 2;
        GameEvents.ReviveRequested();
        return true;
    }

    public void ResetReviveCost() {
        currentReviveCost = baseReviveCost;
    }
}