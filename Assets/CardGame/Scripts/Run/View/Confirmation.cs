using System;
using UnityEngine;

namespace CardGame.Run.View
{
    [Serializable]
    public class Confirmation
    {
        [SerializeField] private string _title;
        [SerializeField, TextArea] private string _message;
        [SerializeField] private string _confirmLabel;
        [SerializeField] private Color _confirmColor = Color.white;

        public string Title => _title;
        public string Message => _message;
        public string ConfirmLabel => _confirmLabel;
        public Color ConfirmColor => _confirmColor;
    }
}
