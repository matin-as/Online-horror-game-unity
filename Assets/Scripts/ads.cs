using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdiveryUnity;
using System;

public class ads : MonoBehaviour
{
    //private string sample = "38b301f2-5e0c-4776-b671-c6b04a612311";
    private string APP_ID = "c95285b2-efb9-4695-8988-34f8a1f71866";
    private string bannerPlacement = "23b28e39-ec59-4200-aec1-475e46c4b76c";
    private string rewarded_ID = "5d1d59c6-1b67-4773-9f81-25bde7d6bf42";
    AdiveryListener listener;
    AdiveryListener rewarded_listener;
    BannerAd bannerAd;
    // testing
    // Start is called before the first frame update
    void Start()
    {
        // config adivery 
        Adivery.Configure(APP_ID);
        // set the baner ads 
        bannerAd = new BannerAd(bannerPlacement, BannerAd.TYPE_BANNER, BannerAd.POSITION_BOTTOM);
        bannerAd.OnAdLoaded += OnBannerAdLoaded;
        bannerAd.LoadAd();
        // set the rewarded ad 
        Adivery.PrepareRewardedAd(rewarded_ID);
        rewarded_listener = new AdiveryListener();
        rewarded_listener.OnError += OnError;
        rewarded_listener.OnRewardedAdLoaded += OnRewardedLoaded;
        rewarded_listener.OnRewardedAdClosed += OnRewardedClosed;
        Adivery.AddListener(rewarded_listener);
        //


    }

    public void OnInterstitialAdLoaded(object caller, string placementId)
    {
        // Interstitial ad loaded
    }

    public void OnError(object caller, AdiveryError error)
    {
        Debug.Log("placement: " + error.PlacementId + " error: " + error.Reason);
    }
    public void OnBannerAdLoaded(object caller, EventArgs args)
    {
        bannerAd.Show();
    }
    public void OnRewardedLoaded(object caller, string placementId)
    {
        // Rewarded ad loaded
    }

    public void OnRewardedClosed(object caller, AdiveryReward reward)
    {
        // Check if User should receive the reward
        if (reward.IsRewarded)
        {
            //getRewardAmount(reward.PlacementId); // Implrement getRewardAmount yourself
        }
    }
    public bool is_readey()
    {
        if(Application.platform==RuntimePlatform.Android)
        {
            if (Adivery.IsLoaded(rewarded_ID))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }
    public void show_rewarded_ad()
    {
            Adivery.Show(rewarded_ID);
    }
}
