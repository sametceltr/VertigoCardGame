public class CoinReviveAction : IBombAction
{
    public bool CanExecute() {
        return ServiceLocator.Economy.CanAfford(ServiceLocator.Economy.ReviveCost);
    }

    public void Execute() {
        if (ServiceLocator.Economy.TryRevive()) {
        }
    }
}