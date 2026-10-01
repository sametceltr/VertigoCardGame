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

        public static Button FindIfMissing(Component owner, Button current, string buttonName) {
            if (current != null) return current;

            var button = Find(owner.transform, buttonName);
            if (button == null) Debug.LogWarning($"{owner.name}: no child button named {buttonName}.", owner);
            return button;
        }
    }
}
