using UnityEngine;

public class AdReviveAction : IBombAction
{
    public bool CanExecute() => true;

    public void Execute() {
        Debug.Log("Watch ad to revive");
    }
}   