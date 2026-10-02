using UnityEngine;

namespace CardGame.Run.View
{
    public class CardGameView : MonoBehaviour
    {
        private GameRun _run;

        public void Initialize(GameRun run) {
            _run = run;
            _run.StateChanged += OnStateChanged;
        }

        private void OnDestroy() {
            if (_run == null) return;

            _run.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(RunState state) {
            gameObject.SetActive(state != RunState.InMenu);
        }
    }
}
