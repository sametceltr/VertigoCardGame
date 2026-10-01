using System;

namespace CardGame.Ads
{
    public interface IAdService
    {
        void ShowRewardedAd(Action<bool> onFinished);
    }
}
