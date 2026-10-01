using CardGame.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Wheel.View
{
    public class SliceView : MonoBehaviour
    {
        [SerializeField] private Image rewardIcon;
        [SerializeField] private ParentFitter rewardIconFitter;
        [SerializeField] private TextMeshProUGUI rewardAmount;
    }
}