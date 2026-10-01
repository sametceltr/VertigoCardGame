using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Run.View
{
    public class BombPopupView : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button giveUpButton;
        [SerializeField] private Button coinReviveButton;
        [SerializeField] private Button watchReviveButton;

        [Header("Text")]
        [SerializeField] private TextMeshProUGUI coinReviveText;

    #if UNITY_EDITOR
        private void OnValidate() {
            if (giveUpButton != null && coinReviveButton != null && watchReviveButton != null) return;

            var children = transform.GetComponentsInChildren<Button>();

            foreach (var child in children) {
                if (child.name.Contains("give")) giveUpButton = child;
                else if (child.name.Contains("coin")) coinReviveButton = child;
                else if (child.name.Contains("watch")) watchReviveButton = child;
            }
        }
    #endif
    }
}