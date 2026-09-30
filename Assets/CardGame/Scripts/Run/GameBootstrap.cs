using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    private void OnDestroy() {
        GameEvents.ClearAllEvents();
    }
}