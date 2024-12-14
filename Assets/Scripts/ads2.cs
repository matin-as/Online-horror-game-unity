using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdiveryUnity;
using System;
using Photon.Pun;

public class ads2 : MonoBehaviour
{
    private PhotonView photonView;
    //private string sample = "38b301f2-5e0c-4776-b671-c6b04a612311";
    private string APP_ID = "c95285b2-efb9-4695-8988-34f8a1f71866";
    private string PLACEMENT_ID_InterstitialAd = "0e02d12f-be81-4a96-b8c3-7ade303d50b0";
    private string rewarded_ID = "5d1d59c6-1b67-4773-9f81-25bde7d6bf42";
    AdiveryListener listener;
    AdiveryListener rewarded_listener;
    // testing
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if(photonView.IsMine)
        {
            // config adivery 
            Adivery.Configure(APP_ID);
            // set the inter ads
            Adivery.PrepareInterstitialAd(PLACEMENT_ID_InterstitialAd);
            listener = new AdiveryListener();
            listener.OnError += OnError;
            listener.OnInterstitialAdLoaded += OnInterstitialAdLoaded;
            Adivery.AddListener(listener);
            // set the baner ads 
            // set the rewarded ad 
            Adivery.PrepareRewardedAd(rewarded_ID);
            rewarded_listener = new AdiveryListener();
            rewarded_listener.OnError += OnError;
            rewarded_listener.OnRewardedAdLoaded += OnRewardedLoaded;
            rewarded_listener.OnRewardedAdClosed += OnRewardedClosed;
            Adivery.AddListener(rewarded_listener);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //text_number.text = number.ToString();
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
        // bannerAd.Hide(); می‌توانید در هر زمانی نمایش تبلیغ بنری را غیرفعال کنید.
    }
    public void OnRewardedLoaded(object caller, string placementId)
    {
        // Rewarded ad loaded
    }

    public void OnRewardedClosed(object caller, AdiveryReward reward)
    {
        // Check if User should receive the reward
            if(photonView.IsMine)
            {
                transform.GetParentComponent<player>().cont();
            }
            //getRewardAmount(reward.PlacementId); // Implrement getRewardAmount yourself
    }
    public bool is_readey()
    {
        if(Application.platform==RuntimePlatform.Android)
        {
            if (photonView.IsMine)
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
        if(photonView.IsMine)
        {
            Adivery.Show(rewarded_ID);
        }
    }
}
