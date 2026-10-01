using UnityEngine;
using UnityEngine.UI;

namespace CardGame.UI
{
    public static class ChildButtons
    {
        public static Button Find(Transform root, string buttonName) {
            foreach (var button in root.GetComponentsInChildren<Button>(true)) {
                if (button.name == buttonName) return button;
            }
            return null;
        }
    }
}
