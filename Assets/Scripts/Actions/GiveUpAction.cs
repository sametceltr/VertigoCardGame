public class GiveUpAction : IBombAction
{
    public bool CanExecute() => true;

    public void Execute() {
        ServiceLocator.GameState.ResetProgress();
        ServiceLocator.Economy.ResetReviveCost();
    }
}