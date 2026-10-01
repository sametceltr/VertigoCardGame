using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Wheel.View
{
    public class WheelView : MonoBehaviour
    {
        [Header("Visual References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Image baseImage;
        [SerializeField] private Image indicatorImage;
        [SerializeField] private SliceView[] rewardSlices;

        [Header("Dependencies")]
        [SerializeField] private Button spinButton;
    }
}