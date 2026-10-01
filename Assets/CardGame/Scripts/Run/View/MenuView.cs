using CardGame.UI;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Run.View
{
    public class MenuView : MonoBehaviour
    {
        private const string PlayButtonName = "ui_button_play";

        [Header("Buttons")]
        [SerializeField] private Button _playButton;

        private GameRun _run;

        public void Initialize(GameRun run) {
            _run = run;
            _run.StateChanged += OnStateChanged;
            _playButton.onClick.AddListener(_run.StartNewRun);
        }

        private void OnDestroy() {
            if (_run == null) return;

            _run.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(RunState state) {
            gameObject.SetActive(state == RunState.InMenu);
        }

    #if UNITY_EDITOR
        private void OnValidate() {
            if (_playButton == null) _playButton = ChildButtons.Find(transform, PlayButtonName);
            if (_playButton == null) Debug.LogWarning($"{name}: no child button named {PlayButtonName}.", this);
        }
    #endif
    }
}
