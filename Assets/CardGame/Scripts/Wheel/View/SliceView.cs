using CardGame.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Wheel.View
{
    public class SliceView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private ParentFitter _iconFitter;
        [SerializeField] private TextMeshProUGUI _amountText;

        public void Show(WheelSlice slice, Sprite bombIcon) {
            bool isBomb = slice.Type == SliceType.Bomb;
            _icon.sprite = isBomb ? bombIcon : slice.Reward.Icon;
            _iconFitter.CalculateAspectRatio();
            _amountText.text = isBomb ? string.Empty : AmountFormatter.Format(slice.Amount);
            SetAlpha(1f);
        }

        public void SetAlpha(float alpha) {
            var color = _icon.color;
            color.a = alpha;
            _icon.color = color;
        }
    }
}
