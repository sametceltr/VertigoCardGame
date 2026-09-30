using UnityEngine;

namespace CardGame.Run
{
    public class GameBootstrap : MonoBehaviour
    {
        private void OnDestroy() {
            GameEvents.ClearAllEvents();
        }
    }
}